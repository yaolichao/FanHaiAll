using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 优品率数据访问类
    /// </summary>
    public class QualityProductDataAccess : BaseDBAccess
    {
        /// <summary>
        /// 获取指定日期区间的优品率数据。
        /// </summary>
        /// <param name="queryType">查询方式 0：按时间范围查询 1：By日期查询。2：By日期查询最后一条记录。</param>
        /// <param name="roomKey">车间主键。多个车间用逗号分开。</param>
        /// <param name="customer">客户。多个客户用逗号分开。</param>
        /// <param name="productModel">产品型号,产品型号（5610M、5612M、6610P、6612P）。多个产品型号用逗号分开。</param>
        /// <param name="proId">产品ID号。多个产品ID号用逗号分开。</param>
        /// <param name="workOrderNo">工单号。多个工单号用逗号分开。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <returns>包含优品率数据的数据集。</returns>
        public DataSet Get(int queryType,
                            string start_time,
                            string end_time,
                            string roomKey,
                            string customer,
                            string productModel,
                            string proId,
                            string workOrderNo,
                            string partNumber
                        
                          )
        {
            const string storeProcedureName = "SP_QRY_QUALITY_PRODUCT_DATA";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                cmd.CommandTimeout = 60;
                this._db.AddInParameter(cmd, "p_query_type", DbType.String, queryType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
                this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }

        public DataSet GetGood(int queryType,
                           string start_time,
                           string end_time,
                           string roomKey,
                           string customer,
                           string productModel,
                           string proId,
                           string workOrderNo,
                           string partNumber,
                           string oprline

                         )
        {
            const string storeProcedureName = "SP_QRY_QUALITY_PRODUCT_DATA_SH";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_query_type", DbType.String, queryType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
                this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                this._db.AddInParameter(cmd, "p_oprline", DbType.String, oprline);
                return this._db.ExecuteDataSet(cmd);
            }
        }

        /// <summary>
        /// 获取指定日期区间的优品率明细数据。
        /// </summary>
        /// <param name="dataType">查询的明细数据类型。</param>
        /// <param name="roomName">车间主键。</param>
        /// <param name="productModel">产品型号,产品型号（5610M、5612M、6610P、6612P）。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <returns>包含优品率明细数据的数据集。</returns>
        public DataSet GetDetail(string dataType,
                                string start_time,
                                string end_time,
                                string roomKey,
                                string customer,
                                string productModel,
                                string proId,
                                string workOrderNo,
                                string partNumber)
        {
            const string storeProcedureName = "SP_QRY_QUALITY_PRODUCT_DATA_DTL";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_data_type", DbType.String, dataType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
                this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }

        /// <summary>
        /// 获取指定日期区间的优品率明细数据。
        /// </summary>
        /// <param name="dataType">查询的明细数据类型。</param>
        /// <param name="roomName">车间主键。</param>
        /// <param name="productModel">产品型号,产品型号（5610M、5612M、6610P、6612P）。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="workOrderNo">工单号。</param>
        /// <param name="start_time">开始时间  2012-05-12。</param>
        /// <param name="end_time">结束时间 2012-05-13。</param>
        /// <returns>包含优品率明细数据的数据集。</returns>
        public DataSet GetDetailLine(string dataType,
                                string start_time,
                                string end_time,
                                string roomKey,
                                string customer,
                                string productModel,
                                string proId,
                                string workOrderNo,
                                string partNumber,
                                string oprline
                                                 )
        {
            const string storeProcedureName = "SP_QRY_QUALITY_PRODUCT_DATA_DTL_SH";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_data_type", DbType.String, dataType);
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, start_time);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, end_time);
                this._db.AddInParameter(cmd, "p_roomKey", DbType.String, roomKey);
                this._db.AddInParameter(cmd, "p_customer", DbType.String, customer);
                this._db.AddInParameter(cmd, "p_productModel", DbType.String, productModel);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, proId);
                this._db.AddInParameter(cmd, "p_workOrderNo", DbType.String, workOrderNo);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                this._db.AddInParameter(cmd, "p_oprline", DbType.String, oprline);
                return this._db.ExecuteDataSet(cmd);
            }
        }


        /// <summary>
        /// 查询优品率数据之后进行数据的行列转换。
        /// </summary>
        /// <param name="dsSource">源数据表。</param>
        /// <param name="queryType">查询方式 0：按时间范围查询 1：By日期查询。2：By日期查询最后一条记录。</param>
        /// <param name="dic"></param>
        /// <returns>行列转换后的数据表。</returns>
        public DataTable TransferDatatable(DataTable dtSource, int queryType, Dictionary<string, string> dic)
        {
            DataTable dtBase = dtSource.DefaultView.ToTable(true, new string[] { "PROMODEL_NAME" });
            //组织新表数据结构。
            DataTable dtNew = new DataTable();
            for (int i = 0; i < dtBase.Columns.Count; i++)
            {
                dtNew.Columns.Add(dtBase.Columns[i].ColumnName);
            }
            dtNew.Columns.Add("COL_VALUE");
            IEnumerable<string> dateColNames = dtSource.AsEnumerable()
                                              .Select(row => Convert.ToString(row["DATA_DATE"]))
                                              .Distinct();
            foreach (string colName in dateColNames)
            {
                DataColumn dc = dtNew.Columns.Add(colName);
                dc.DataType = typeof(double);
            }
            dtNew.Columns.Add("KEY_VALUE");
            //填充数据
            for (int i = 0; i < dtBase.Rows.Count; i++)
            {
                string proModelName = Convert.ToString(dtBase.Rows[i]["PROMODEL_NAME"]);
                //添加数据行
                foreach (string key in dic.Keys)
                {
                    //按时间段查询不显示累计值。
                    if (key.StartsWith("LJ_") && (queryType == 0 || queryType==3))
                    {
                        continue;
                    }
                    DataRow drNew = dtNew.NewRow();
                    drNew["KEY_VALUE"] = string.Format("{0}${1}",proModelName,key);
                    drNew["PROMODEL_NAME"] = proModelName;
                    drNew["COL_VALUE"] = key;
                    for (int j = dtBase.Columns.Count + 1; j < dtNew.Columns.Count-1; j++)
                    {
                        string dataDate = dtNew.Columns[j].ColumnName;
                        string filter = string.Format("PROMODEL_NAME='{0}' AND DATA_DATE='{1}'", proModelName, dataDate);
                        DataRow[] drs = dtSource.Select(filter);
                        if (drs.Length > 0)
                        {
                            //TRACKOUT_QTY
                            //A0J_QTY,DIRECT_A0J_QTY,REWORK_A0J_QTY,PRE_A0J_QTY 
                            //ERSANJ_QTY,NOREWORK_ERSANJ_QTY,REWORK_ERSANJ_QTY,NO_CY_REWORK_ERSANJ_QTY 
                            //SCRAP_QTY,NOREWORK_SCRAP_QTY,NO_CY_REWORK_SCRAP_QTY
                            double trackoutQty = Convert.ToDouble(drs[0]["TRACKOUT_QTY"]);
                            double kjAjQty = Convert.ToDouble(drs[0]["KJ_AJ_QTY"]);
                            double kjA0jQty = Convert.ToDouble(drs[0]["KJ_A0J_QTY"]);
                            double kjErsanJiQty = Convert.ToDouble(drs[0]["KJ_ERSANJI_QTY"]);
                            double kjScrapQty = Convert.ToDouble(drs[0]["KJ_SCRAP_QTY"]);
                            double kjQty = Convert.ToDouble(drs[0]["KJ_QTY"]);
                            double ajQty = Convert.ToDouble(drs[0]["AJ_QTY"]);
                            double a0jQty = Convert.ToDouble(drs[0]["A0J_QTY"]);
                            double directA0JQty = Convert.ToDouble(drs[0]["DIRECT_A0J_QTY"]);
                            double reworkA0JQty = Convert.ToDouble(drs[0]["REWORK_A0J_QTY"]);
                            double preA0JQty = Convert.ToDouble(drs[0]["PRE_A0J_QTY"]);

                            double ersanjQty = Convert.ToDouble(drs[0]["ERSANJ_QTY"]);
                            double noReworkERSANJQty = Convert.ToDouble(drs[0]["NOREWORK_ERSANJ_QTY"]);
                            double reworkERSANJQty = Convert.ToDouble(drs[0]["REWORK_ERSANJ_QTY"]);
                            double noCYReworkERSANJQty = Convert.ToDouble(drs[0]["NO_CY_REWORK_ERSANJ_QTY"]);

                            double scrapQty = Convert.ToDouble(drs[0]["SCRAP_QTY"]);
                            double noReworkScrapQty = Convert.ToDouble(drs[0]["NOREWORK_SCRAP_QTY"]);
                            double noCYReworkScrapQty = Convert.ToDouble(drs[0]["NO_CY_REWORK_SCRAP_QTY"]);

                            if (scrapQty + trackoutQty > 0)
                            {
                                if (key == "TRACKOUT_QTY")
                                {
                                    drNew[dataDate] = trackoutQty;
                                }
                                else if (key == "KJ_AJ_QTY")
                                {
                                    drNew[dataDate] = kjAjQty;
                                }
                                else if (key == "KJ_A0J_QTY")
                                {
                                    drNew[dataDate] = kjA0jQty;
                                }
                                else if (key == "KJ_ERSANJI_QTY")
                                {
                                    drNew[dataDate] = kjErsanJiQty;
                                }
                                else if (key == "KJ_SCRAP_QTY")
                                {
                                    drNew[dataDate] = kjScrapQty;
                                }
                                else if (key == "KJ_QTY")
                                {
                                    drNew[dataDate] = kjQty;
                                }
                                else if (key == "AJ_QTY")
                                {
                                    drNew[dataDate] = ajQty;
                                }
                                else if (key == "A0J_QTY")
                                {
                                    drNew[dataDate] = a0jQty;
                                }
                                else if (key == "DIRECT_A0J_QTY")
                                {
                                    drNew[dataDate] = directA0JQty;
                                }
                                else if (key == "REWORK_A0J_QTY")
                                {
                                    drNew[dataDate] = reworkA0JQty;
                                }
                                else if (key == "PRE_A0J_QTY")
                                {
                                    drNew[dataDate] = preA0JQty;
                                }
                                else if (key == "ERSANJ_QTY")
                                {
                                    drNew[dataDate] = ersanjQty;
                                }
                                else if (key == "NOREWORK_ERSANJ_QTY")
                                {
                                    drNew[dataDate] = noReworkERSANJQty;
                                }
                                else if (key == "REWORK_ERSANJ_QTY")
                                {
                                    drNew[dataDate] = reworkERSANJQty;
                                }
                                else if (key == "NO_CY_REWORK_ERSANJ_QTY")
                                {
                                    drNew[dataDate] = noCYReworkERSANJQty;
                                }
                                else if (key == "SCRAP_QTY")
                                {
                                    drNew[dataDate] = scrapQty;
                                }
                                else if (key == "NOREWORK_SCRAP_QTY")
                                {
                                    drNew[dataDate] = noReworkScrapQty;
                                }
                                else if (key == "NO_CY_REWORK_SCRAP_QTY")
                                {
                                    drNew[dataDate] = noCYReworkScrapQty;
                                }
                                //{"KJ_RATE",                                 "客级品率"}
                                //客级工单客级品数/客级工单的（客级+A+A0+二三级+报废数）;备注：“客级工单客级品数”就是Conergy 与Schueco的工单的客级品数
                                else if (key == "KJ_RATE" && (kjQty + kjAjQty + kjA0jQty + kjErsanJiQty + kjScrapQty)>0)
                                {
                                    double kjRate = kjQty / (kjQty+kjAjQty+kjA0jQty+kjErsanJiQty+ kjScrapQty);
                                    drNew[dataDate] = kjRate;
                                }
                                //{"A_UPON_QUALITY_RATE",                     "A级以上优品率"},
                                //［1-(直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/（入库数+报废数）］*100%
                                else if (key == "A_UPON_QUALITY_RATE")
                                {
                                    double aUponQualityRate = 1 - (directA0JQty + preA0JQty + noCYReworkERSANJQty + noCYReworkScrapQty) / (scrapQty + trackoutQty);
                                    drNew[dataDate] = aUponQualityRate;
                                }
                                //{"A0_UPON_QUALITY_RATE",                    "A0级以上优品率"},
                                //［1-(二三级+报废）/（入库数+报废数）］*100%
                                else if (key == "A0_UPON_QUALITY_RATE")
                                {
                                    double a0UponQualityRate = 1 - (ersanjQty + scrapQty) / (scrapQty + trackoutQty);
                                    drNew[dataDate] = a0UponQualityRate;
                                }
                                //{"A0_ERSANJ_QUALITY_RATE",                  "A0、二三级品率"},
                                //（A0级+二三级）/（入库数+报废数）
                                else if (key == "A0_ERSANJ_QUALITY_RATE")
                                {
                                    double a0ERSANJQualityRate = (a0jQty + ersanjQty) / (scrapQty + trackoutQty);
                                    drNew[dataDate] = a0ERSANJQualityRate;
                                }
                                //{"REWORK_A0_ERSANJ_QUALITY_RATE",           "返修后A0、二三级品率"},
                                //（返修后A0级+返修后二三级）/（入库数+报废数）
                                else if (key == "REWORK_A0_ERSANJ_QUALITY_RATE")
                                {
                                    double reworkA0ERSANJQualityRate = (reworkA0JQty + reworkERSANJQty) / (scrapQty + trackoutQty);
                                    drNew[dataDate] = reworkA0ERSANJQualityRate;
                                }
                                //{"PASS_RATE",                               "合格率"},
                                //[1-报废数/(入库数+报废数)]*100%
                                else if (key == "PASS_RATE")
                                {
                                    double passRate = 1 - scrapQty / (scrapQty + trackoutQty);
                                    drNew[dataDate] = passRate;
                                }
                                //{"SCRAP_RATE",                              "报废品率"},
                                //报废数/(入库数+报废数)
                                else if (key == "SCRAP_RATE")
                                {
                                    double rate = scrapQty / (scrapQty + trackoutQty);
                                    drNew[dataDate] = rate;
                                }
                            }
                            //LJ_TRACKOUT_QTY,
                            //LJ_A0J_QTY, LJ_DIRECT_A0J_QTY,LJ_REWORK_A0J_QTY,LJ_PRE_A0J_QTY,
                            //LJ_ERSANJ_QTY,LJ_NOREWORK_ERSANJ_QTY,LJ_REWORK_ERSANJ_QTY,LJ_NO_CY_REWORK_ERSANJ_QTY,
                            //SCRAP_QTY, LJ_SCRAP_QTY, LJ_NOREWORK_SCRAP_QTY, LJ_NO_CY_REWORK_SCRAP_QTY
                            if (queryType == 1 || queryType==2)
                            {
                                double ljTrackoutQty = Convert.ToDouble(drs[0]["LJ_TRACKOUT_QTY"]);
                                double ljKjAjQty = Convert.ToDouble(drs[0]["LJ_KJ_AJ_QTY"]);
                                double ljKjA0jQty = Convert.ToDouble(drs[0]["LJ_KJ_A0J_QTY"]);
                                double ljKjErsanJiQty = Convert.ToDouble(drs[0]["LJ_KJ_ERSANJI_QTY"]);
                                double ljKjScrapQty = Convert.ToDouble(drs[0]["LJ_KJ_SCRAP_QTY"]);
                                double ljKjQty = Convert.ToDouble(drs[0]["LJ_KJ_QTY"]);

                                double ljAjQty = Convert.ToDouble(drs[0]["LJ_AJ_QTY"]);
                                double ljA0jQty = Convert.ToDouble(drs[0]["LJ_A0J_QTY"]);
                                double ljDirectA0JQty = Convert.ToDouble(drs[0]["LJ_DIRECT_A0J_QTY"]);
                                double ljReworkA0JQty = Convert.ToDouble(drs[0]["LJ_REWORK_A0J_QTY"]);
                                double ljPreA0JQty = Convert.ToDouble(drs[0]["LJ_PRE_A0J_QTY"]);

                                double ljErsanjQty = Convert.ToDouble(drs[0]["LJ_ERSANJ_QTY"]);
                                double ljNoReworkERSANJQty = Convert.ToDouble(drs[0]["LJ_NOREWORK_ERSANJ_QTY"]);
                                double ljReworkERSANJQty = Convert.ToDouble(drs[0]["LJ_REWORK_ERSANJ_QTY"]);
                                double ljNoCYReworkERSANJQty = Convert.ToDouble(drs[0]["LJ_NO_CY_REWORK_ERSANJ_QTY"]);

                                double ljScrapQty = Convert.ToDouble(drs[0]["LJ_SCRAP_QTY"]);
                                double ljNoReworkScrapQty = Convert.ToDouble(drs[0]["LJ_NOREWORK_SCRAP_QTY"]);
                                double ljNoCYReworkScrapQty = Convert.ToDouble(drs[0]["LJ_NO_CY_REWORK_SCRAP_QTY"]);

                                if (ljTrackoutQty + ljScrapQty > 0)
                                {
                                    if (key == "LJ_TRACKOUT_QTY")
                                    {
                                        drNew[dataDate] = ljTrackoutQty;
                                    }
                                    else if (key == "LJ_KJ_AJ_QTY")
                                    {
                                        drNew[dataDate] = ljKjAjQty;
                                    }
                                    else if (key == "LJ_KJ_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljKjA0jQty;
                                    }
                                    else if (key == "LJ_KJ_ERSANJI_QTY")
                                    {
                                        drNew[dataDate] = ljKjErsanJiQty;
                                    }
                                    else if (key == "LJ_KJ_SCRAP_QTY")
                                    {
                                        drNew[dataDate] = ljKjScrapQty;
                                    }
                                    else if (key == "LJ_KJ_QTY")
                                    {
                                        drNew[dataDate] = ljKjQty;
                                    }
                                    else if (key == "LJ_AJ_QTY")
                                    {
                                        drNew[dataDate] = ljAjQty;
                                    }
                                    else if (key == "LJ_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljA0jQty;
                                    }
                                    else if (key == "LJ_DIRECT_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljDirectA0JQty;
                                    }
                                    else if (key == "LJ_REWORK_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljReworkA0JQty;
                                    }
                                    else if (key == "LJ_PRE_A0J_QTY")
                                    {
                                        drNew[dataDate] = ljPreA0JQty;
                                    }
                                    else if (key == "LJ_ERSANJ_QTY")
                                    {
                                        drNew[dataDate] = ljErsanjQty;
                                    }
                                    else if (key == "LJ_NOREWORK_ERSANJ_QTY")
                                    {
                                        drNew[dataDate] = ljNoReworkERSANJQty;
                                    }
                                    else if (key == "LJ_REWORK_ERSANJ_QTY")
                                    {
                                        drNew[dataDate] = ljReworkERSANJQty;
                                    }
                                    else if (key == "LJ_NO_CY_REWORK_ERSANJ_QTY")
                                    {
                                        drNew[dataDate] = ljNoCYReworkERSANJQty;
                                    }
                                    else if (key == "LJ_SCRAP_QTY")
                                    {
                                        drNew[dataDate] = ljScrapQty;
                                    }
                                    else if (key == "LJ_NOREWORK_SCRAP_QTY")
                                    {
                                        drNew[dataDate] = ljNoReworkScrapQty;
                                    }
                                    else if (key == "LJ_NO_CY_REWORK_SCRAP_QTY")
                                    {
                                        drNew[dataDate] = ljNoCYReworkScrapQty;
                                    }
                                    //{"LJ_KJ_RATE",                                 "累计客级品率"}
                                    //累计客级工单的客级品数/累计客级工单的（客级+A+A0+二三级+报废数）
                                    else if (key == "LJ_KJ_RATE" && (ljKjQty + ljKjAjQty + ljKjA0jQty + ljKjErsanJiQty + ljKjScrapQty)>0)
                                    {
                                        double ljKjRate = ljKjQty / (ljKjQty+ljKjAjQty+ljKjA0jQty+ljKjErsanJiQty+ ljKjScrapQty);
                                        drNew[dataDate] = ljKjRate;
                                    }
                                    //{"LJ_A_UPON_QUALITY_RATE",                  "累计 A级以上优品率"},
                                    //1-[累计(直判A0+预判A0+二三级<非层压返修>+报废<非层压返修>）/累计（入库数+报废数）]
                                    else if (key == "LJ_A_UPON_QUALITY_RATE")
                                    {
                                        double ljAUponQualityRate = 1 - (ljDirectA0JQty + ljPreA0JQty + ljNoCYReworkERSANJQty + ljNoCYReworkScrapQty) / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = ljAUponQualityRate;
                                    }
                                    //{"LJ_A0_UPON_QUALITY_RATE",                 "累计 A0级以上优品率"},
                                    //1-[累计(二三级+报废）/累计（入库数+报废数）]
                                    else if (key == "LJ_A0_UPON_QUALITY_RATE")
                                    {
                                        double ljA0UponQualityRate = 1 - (ljErsanjQty + ljScrapQty) / (ljScrapQty + ljTrackoutQty);
                                        drNew[dataDate] = ljA0UponQualityRate;
                                    }
                                    //{"LJ_A0_ERSANJ_QUALITY_RATE",               "累计A0、二三级品率"},
                                    //[累计（A0级+二三级）/累计（入库数+报废数）]
                                    else if (key == "LJ_A0_ERSANJ_QUALITY_RATE")
                                    {
                                        double ljA0ERSANJQualityRate = (ljA0jQty + ljErsanjQty) / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = ljA0ERSANJQualityRate;
                                    }
                                    //{"LJ_REWORK_A0_ERSANJ_QUALITY_RATE",        "累计返修后A0、二三级品率"},
                                    //[累计（返修后A0级+返修后二三级）/累计（入库数+报废数）]
                                    else if (key == "LJ_REWORK_A0_ERSANJ_QUALITY_RATE")
                                    {
                                        double ljReworkA0ERSANJQualityRate = (ljReworkA0JQty + ljReworkERSANJQty) / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = ljReworkA0ERSANJQualityRate;
                                    }
                                    //{"LJ_PASS_RATE",                            "累计合格率"}
                                    else if (key == "LJ_PASS_RATE")
                                    {
                                        double ljPassRate = 1 - ljScrapQty / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = ljPassRate;
                                    }
                                    //{"LJ_SCRAP_RATE",                           "累计报废品率"}
                                    //累计报废数/累计（入库数+报废数）
                                    else if (key == "LJ_SCRAP_RATE")
                                    {
                                        double rate = ljScrapQty / (ljTrackoutQty + ljScrapQty);
                                        drNew[dataDate] = rate;
                                    }
                                }
                            }
                        }
                    }
                    dtNew.Rows.Add(drNew);
                }
            }
            return dtNew;
        }
    }

}

