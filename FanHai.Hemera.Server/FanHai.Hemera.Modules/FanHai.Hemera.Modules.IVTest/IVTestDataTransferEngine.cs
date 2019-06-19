using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using System.Data.Common;
using System.Collections;
using FanHai.Hemera.Modules.FMM;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.IVTest
{
    /// <summary>
    /// IV测试数据的操作类。
    /// </summary>
    public class IVTestDataTransferEngine : AbstractEngine, IIVTestDataTransferEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public IVTestDataTransferEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 根据设备代码获取最大的IV测试时间。
        /// </summary>
        /// <param name="deviceNo">设备代码。</param>
        /// <returns>指定设备最大的IV测试时间。</returns>
        public DateTime GetMaxIVTestTime(string deviceNo)
        {
            DateTime dtMaxTestTime = new DateTime(1900, 01, 01);
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(@"SELECT MAX(TTIME) FROM WIP_IV_TEST WHERE DEVICENUM='{0}'",
                                deviceNo.PreventSQLInjection());
            object maxTestTime = this.db.ExecuteScalar(CommandType.Text, sbSql.ToString());
            if (maxTestTime != null && maxTestTime != DBNull.Value)
            {
                dtMaxTestTime = Convert.ToDateTime(maxTestTime);
            }
            return dtMaxTestTime;
        }
        /// <summary>
        /// 新增IV测试数据。
        /// </summary>
        /// <param name="dsParams">包含IV测试数据的数据集对象。</param>
        /// <returns>新增IV测试数据的个数。</returns>
        public int InsertIVTestData(DataSet dsParams)
        {
            int count = 0;
            if (dsParams != null && dsParams.Tables.Count> 0 || dsParams.Tables[0].Rows.Count > 0)
            {
                DataView dvIvTest = dsParams.Tables[0].DefaultView;
                dvIvTest.Sort = "TTIME ASC";
                string operationName=string.Empty;
                string userId = "system";
                if(dsParams.ExtendedProperties.ContainsKey(PROPERTY_FIELDS.IVTEST_DATA_OPERATION_NAME))
                {
                    operationName = Convert.ToString(dsParams.ExtendedProperties[PROPERTY_FIELDS.IVTEST_DATA_OPERATION_NAME]);
                }
                if (dsParams.ExtendedProperties.ContainsKey(PROPERTY_FIELDS.USER_NAME))
                {
                    userId = Convert.ToString(dsParams.ExtendedProperties[PROPERTY_FIELDS.USER_NAME]);
                }

                using (DbConnection conn = this.db.CreateConnection())
                {
                    conn.Open();
                    foreach (DataRowView drv in dvIvTest)
                    {
                        using (DbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                string lotNum = Convert.ToString(drv["LOT_NUM"]);
                                string testTime = Convert.ToString(drv["TTIME"]);
                                string deviceNo = Convert.ToString(drv["DeviceNo"]);
                                //判断是否有lotNum+testTime+设备号相同的记录。如果有则代表数据已经读取过。
                                string sql = string.Format(@"SELECT COUNT(*) FROM WIP_IV_TEST 
                                                            WHERE DEVICENUM='{0}' AND LOT_NUM='{1}' AND TTIME='{2}'",
                                                            deviceNo.PreventSQLInjection(),
                                                            lotNum.PreventSQLInjection(),
                                                            testTime.PreventSQLInjection());
                                int result = Convert.ToInt32(this.db.ExecuteScalar(trans, CommandType.Text, sql));
                                //如果存在RowID标识的数据。则继续执行下一个数据
                                if (result > 0)
                                {
                                    trans.Rollback();
                                    continue;
                                }
                                int vcDefault = 1;
                                string tdate = testTime.Length > 10 ? testTime.Substring(0, 10) : testTime;
                                string celleff = string.Empty;
                                object maxLId = 0;
                                //-----------------------------------
                                //如果序列号以JZ开头，代表是校准版，则更新设备校准版记录。
                                if (lotNum.StartsWith("JZ"))
                                {
                                    //更新设备校准板信息。
                                    sql = string.Format(@"UPDATE WIP_CALIBRATION_INFO
                                                        SET CALIBRATION_NO='{1}',CALIBRATION_TIME=GETDATE()
                                                        WHERE MACHINE_NO='{0}'",
                                                        deviceNo.PreventSQLInjection(),
                                                        lotNum.PreventSQLInjection());
                                    result = this.db.ExecuteNonQuery(trans, CommandType.Text, sql);
                                    //如果更新记录数<1,则插入一笔设备校准板记录
                                    if (result < 1)
                                    {
                                        sql = string.Format(@"INSERT INTO WIP_CALIBRATION_INFO(MACHINE_NO,CALIBRATION_NO,CALIBRATION_TIME)
                                                              VALUES('{0}','{1}',GETDATE())",
                                                              deviceNo.PreventSQLInjection(),
                                                              lotNum.PreventSQLInjection());
                                        this.db.ExecuteNonQuery(trans, CommandType.Text, sql);
                                    }
                                }
                                else
                                {
                                    //根据设备代码+组件序列号获取最大ID
                                    sql = string.Format(@"SELECT ISNULL(MAX(L_ID),0)+1 FROM WIP_IV_TEST 
                                                          WHERE DEVICENUM='{0}' AND LOT_NUM='{1}'",
                                                          deviceNo.PreventSQLInjection(),
                                                          lotNum.PreventSQLInjection());
                                    maxLId = Convert.ToInt32(this.db.ExecuteScalar(trans, CommandType.Text, sql));
                                    //根据批次号获取电池片效率
                                    sql = string.Format(@"SELECT TOP 1 EFFICIENCY FROM POR_LOT WHERE LOT_NUMBER='{0}'",
                                                         lotNum.PreventSQLInjection());
                                    celleff = Convert.ToString(this.db.ExecuteScalar(trans, CommandType.Text, sql));
                                    //-----------------------------------
                                    //只有组件在当前（组件测试站） 或者 组件在终检站但为等待进站 才可以改变测试值为默认值。
                                    if (!string.IsNullOrEmpty(operationName))
                                    {
                                        sql = string.Format(@"SELECT COUNT(1)
                                                            FROM POR_ROUTE_STEP
                                                            WHERE (ROUTE_STEP_KEY=(SELECT TOP 1 CUR_STEP_VER_KEY
                                                                                  FROM POR_LOT
                                                                                  WHERE LOT_NUMBER='{1}')
                                                                   AND ROUTE_STEP_NAME='{0}')
                                                            OR
                                                            (ROUTE_STEP_KEY=(SELECT TOP 1 CUR_STEP_VER_KEY
                                                                              FROM POR_LOT
                                                                              WHERE LOT_NUMBER='{1}' AND STATE_FLAG<9)
                                                             AND ROUTE_STEP_NAME='终检')",
                                                            operationName.PreventSQLInjection(),
                                                            lotNum.PreventSQLInjection());
                                        int lotCount = Convert.ToInt32(this.db.ExecuteScalar(trans, CommandType.Text, sql));
                                        //当前批次不在当前工序。
                                        if (lotCount <= 0)
                                        {
                                            vcDefault = 0;
                                        }
                                    }
                                    //不是有效数据，但IV测试中没有数据，则设置为当前数据为有效数据。
                                    if (vcDefault == 0)
                                    {
                                        sql = string.Format(@"SELECT COUNT(1) FROM WIP_IV_TEST WHERE LOT_NUM='{0}'",
                                                            lotNum.PreventSQLInjection());
                                        int testDataCount = Convert.ToInt32(this.db.ExecuteScalar(trans, CommandType.Text, sql));
                                        if (testDataCount <= 0)
                                        {
                                            vcDefault = 1;
                                        }
                                    }
                                    //如果当前数据为有效数据。
                                    if (vcDefault == 1)
                                    {
                                        //更新之前的测试数据为非默认数据
                                        sql = string.Format(@"UPDATE WIP_IV_TEST SET VC_DEFAULT=0 WHERE LOT_NUM='{0}'",
                                                              lotNum.PreventSQLInjection());
                                        this.db.ExecuteNonQuery(trans, CommandType.Text, sql);
                                    }
                                }
                                //获取校准板信息。
                                sql = string.Format("SELECT TOP 1 CALIBRATION_NO FROM WIP_CALIBRATION_INFO WHERE MACHINE_NO='{0}' ORDER BY CALIBRATION_TIME DESC",
                                                     deviceNo.PreventSQLInjection());
                                string calibrationNo = Convert.ToString(this.db.ExecuteScalar(trans, CommandType.Text, sql));
                                //新增测试数据
                                object ff = drv["FF"];//FF 填充因子
                                object isc = drv["ISC"];//ISC 测试短路电流
                                object eff = drv["EFF"];//--EFF 组件转换效率
                                object rsh = drv["RSH"];//--RSH 串联电阻
                                object rs = drv["RS"];//--RS 并联电阻
                                object voc = drv["VOC"];//--VOC 测试开路电压
                                object ipm = drv["IPM"];// --IPM 测试最大电流
                                object vpm = drv["VPM"];//--VPM 测试最大电压
                                object pm = drv["PM"];//--PM 测试最大功率
                                object ambienttemp = drv["AMBIENTTEMP"];//--AMBIENTTEMP 测度温度
                                object sensortemp = drv["SENSORTEMP"];//--SENSORTEMP 环境温度
                                object intensity = drv["INTENSITY"];//--INTENSITY 光强
                                string ivTestKey = Guid.NewGuid().ToString();

                                decimal decIpm = Convert.ToDecimal(ipm);
                                decimal decIsc = Convert.ToDecimal(isc);
                                decimal decPer = decIsc == 0?1:decIpm/decIsc;

                                //插入对应的数据。
                                sql = string.Format(@"INSERT INTO WIP_IV_TEST
                                                    (IV_TEST_KEY,T_DATE,TTIME,LOT_NUM,FF,ISC,EFF,RSH,RS,VOC,IPM,VPM,PM,AMBIENTTEMP,SENSORTEMP,INTENSITY,DEVICENUM,L_ID,CALIBRATION_NO,VC_CELLEFF,VC_DEFAULT,C_USERID,Imp_Isc,ImpIsc_Control) 
                                                     VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}',{20},'{21}','{22}','{23}')",
                                                     ivTestKey,
                                                     tdate,
                                                     testTime,
                                                     lotNum.PreventSQLInjection(),
                                                     ff,
                                                     isc,
                                                     eff,
                                                     rsh,
                                                     rs,
                                                     voc,
                                                     ipm,
                                                     vpm,
                                                     pm,
                                                     ambienttemp,
                                                     sensortemp,
                                                     intensity,
                                                     deviceNo.PreventSQLInjection(),
                                                     maxLId,
                                                     calibrationNo.PreventSQLInjection(),
                                                     celleff.PreventSQLInjection(),
                                                     vcDefault,
                                                     userId,
                                                     decPer,
                                                     "");
                                count = count + this.db.ExecuteNonQuery(trans, CommandType.Text, sql);
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                    conn.Close();
                }
            }
            return count;
        }
    }
}