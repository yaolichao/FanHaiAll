using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;


using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;


namespace FanHai.Hemera.Modules.Wip
{
    public class NameplateLabelAutoPrintEngine : AbstractEngine, INameplateLabelAutoPrintEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        public NameplateLabelAutoPrintEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        public override void Initialize()
        {

        }

        #region INameplateLabelAutoPrintEngine 成员
        public DataSet getInfoForNamepalteLabelAutoPrint(string lotNum)
        {
            DataSet dsReturn = new DataSet();

            string sql = string.Empty;
            try{
                sql = string.Format(@"
                     
                DECLARE @PART_NUMBER  VARCHAR(20)
                DECLARE @ORDER_NUMBER  VARCHAR(9)   
                DECLARE @POWER    VARCHAR(5)
                DECLARE @lotNum   varchar(20)

	                set @lotNum='{0}'
                		
		                SELECT @POWER=LEFT(t.MODULE_NAME,3)
			                FROM BASE_POWERSET t 
			                RIGHT JOIN WIP_IV_TEST t2 ON t.PS_CODE=t2.VC_TYPE AND t.PS_SEQ=t2.I_IDE
			                WHERE LOT_NUM=@lotNum
			                AND t2.COEF_PMAX BETWEEN t.P_MIN AND t.P_MAX
			                AND t2.VC_DEFAULT='1' 
			                AND t.ISFLAG=1
                			
                           select @ORDER_NUMBER =WORK_ORDER_NO,@PART_NUMBER=PART_NUMBER from POR_LOT where LOT_NUMBER=@lotNum                                              
                SELECT 
                DISTINCT  
                C.ORDER_NUMBER,A.PART_NUMBER,C.PRODUCT_NAME,PMAXSTAB,D.TemNoct,
			                CELLTYPE=(CASE when CHARINDEX ('P' , C.PROMODEL_NAME ,  0 )>0 THEN 'Poly-Si'
						                   when CHARINDEX ('M' , C.PROMODEL_NAME ,  0 )>0 THEN 'Mono-Si'
			                END ),
			                MAXPOWER=(CASE WHEN CHARINDEX ('HV' , C.PRODUCT_NAME ,  0 )>0 THEN '1500'
                                           WHEN CHARINDEX ('HC' , C.PRODUCT_NAME ,  0 )>0 THEN '1500'
						                   WHEN CHARINDEX ('DG' , C.PRODUCT_NAME ,  0 )>0 THEN '1500'
						                   WHEN CHARINDEX ('DGT' , C.PRODUCT_NAME ,  0 )>0 THEN '1500'
					                  ELSE '1000'	   
			                END),VOCSTAB,VMPPSTAB,ISCSTAB,IMPPSTAB,FUSE,TOLERANCE ,
			                C.PROMODEL_NAME,
			                LABELTYPE,C.LABELVAR,C.MODULE_TYPE_SUFFIX
					                FROM dbo.POR_WO_PRD_PS A 
                                             INNER JOIN POR_WORK_ORDER B 
                                                           ON A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                             INNER JOIN POR_WO_PRD C
                                                            ON B.WORK_ORDER_KEY=C.WORK_ORDER_KEY  
                                             INNER JOIN   (SELECT a.ITEM_ORDER,
											                   MAX(CASE WHEN a.ATTRIBUTE_NAME='ProductType' THEN a.ATTRIBUTE_VALUE END) AS ProductType,
											                   MAX(CASE WHEN a.ATTRIBUTE_NAME='TemNoct' THEN a.ATTRIBUTE_VALUE END) AS TemNoct
                										      
										                    FROM CRM_ATTRIBUTE a
										                    WHERE EXISTS(SELECT ATTRIBUTE_KEY FROM BASE_ATTRIBUTE
													                      WHERE CATEGORY_KEY=(
													                                           SELECT CATEGORY_KEY 
																		                       FROM BASE_ATTRIBUTE_CATEGORY 
																		                       WHERE CATEGORY_NAME='NameplateLabelPrint_Tem')
													                                           AND ATTRIBUTE_KEY=a.ATTRIBUTE_KEY) 
                                                                                               GROUP BY a.ITEM_ORDER) D 
                                                             ON C.PROMODEL_NAME=D.ProductType     AND C.IS_USED='Y'     
                                    where 
                                    A.PART_NUMBER = @PART_NUMBER
                                    AND C.PART_NUMBER = @PART_NUMBER
                                    AND C.ORDER_NUMBER = @ORDER_NUMBER
                                    AND A.IS_USED = 'Y'
                                    AND B.ORDER_STATE = 'REL' 
                                    AND PMAXSTAB = @POWER
                    ", lotNum);

            dsReturn =db.ExecuteDataSet(CommandType.Text,sql);

             }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("getInfoForNamepalteLabelAutoPrint Error: " + ex.Message);
            }
            return dsReturn;
        }
#endregion

        /// <summary>
        /// 根据序列号和功率档位查询对应档位的铭牌打印信息（体现功率档位） yibin.fei 2017.10.30
        /// </summary>
        /// <param name="lotNum">序列号</param>
        /// <param name="AFTERPOWER">要体现的功率</param>
        /// <returns></returns>
        public DataSet getInfoForNamepalteLabelAutoPrintForPowerShow(string lotNum, string AfterPower)
        {
            DataSet dsReturn = new DataSet();

            string sql = string.Empty;
            try
            {
                sql = string.Format(@"
                     
                DECLARE @PART_NUMBER  VARCHAR(20)
                DECLARE @ORDER_NUMBER  VARCHAR(9)   
                DECLARE @POWER    VARCHAR(5)
                DECLARE @lotNum   varchar(20)

	                set @lotNum='{0}'
                	set	@POWER='{1}'
		               
                			
                           select @ORDER_NUMBER =WORK_ORDER_NO,@PART_NUMBER=PART_NUMBER from POR_LOT where LOT_NUMBER=@lotNum                                              
                SELECT 
                DISTINCT  
                C.ORDER_NUMBER,A.PART_NUMBER,C.PRODUCT_NAME,PMAXSTAB,D.TemNoct,
			                CELLTYPE=(CASE when CHARINDEX ('P' , C.PROMODEL_NAME ,  0 )>0 THEN 'Poly-Si'
						                   when CHARINDEX ('M' , C.PROMODEL_NAME ,  0 )>0 THEN 'Mono-Si'
			                END ),
			                MAXPOWER=(CASE WHEN CHARINDEX ('HV' , C.PRODUCT_NAME ,  0 )>0 THEN '1500'
						                   WHEN CHARINDEX ('DG' , C.PRODUCT_NAME ,  0 )>0 THEN '1500'
						                   WHEN CHARINDEX ('DGT' , C.PRODUCT_NAME ,  0 )>0 THEN '1500'
					                  ELSE '1000'	   
			                END),VOCSTAB,VMPPSTAB,ISCSTAB,IMPPSTAB,FUSE,TOLERANCE ,
			                C.PROMODEL_NAME,
			                LABELTYPE,C.LABELVAR,C.MODULE_TYPE_SUFFIX
					                FROM dbo.POR_WO_PRD_PS A 
                                             INNER JOIN POR_WORK_ORDER B 
                                                           ON A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                             INNER JOIN POR_WO_PRD C
                                                            ON B.WORK_ORDER_KEY=C.WORK_ORDER_KEY  
                                             INNER JOIN   (SELECT a.ITEM_ORDER,
											                   MAX(CASE WHEN a.ATTRIBUTE_NAME='ProductType' THEN a.ATTRIBUTE_VALUE END) AS ProductType,
											                   MAX(CASE WHEN a.ATTRIBUTE_NAME='TemNoct' THEN a.ATTRIBUTE_VALUE END) AS TemNoct
                										      
										                    FROM CRM_ATTRIBUTE a
										                    WHERE EXISTS(SELECT ATTRIBUTE_KEY FROM BASE_ATTRIBUTE
													                      WHERE CATEGORY_KEY=(
													                                           SELECT CATEGORY_KEY 
																		                       FROM BASE_ATTRIBUTE_CATEGORY 
																		                       WHERE CATEGORY_NAME='NameplateLabelPrint_Tem')
													                                           AND ATTRIBUTE_KEY=a.ATTRIBUTE_KEY) 
                                                                                               GROUP BY a.ITEM_ORDER) D 
                                                             ON C.PROMODEL_NAME=D.ProductType     AND C.IS_USED='Y'     
                                    where 
                                    A.PART_NUMBER = @PART_NUMBER
                                    AND C.PART_NUMBER = @PART_NUMBER
                                    AND C.ORDER_NUMBER = @ORDER_NUMBER
                                    AND A.IS_USED = 'Y'
                                    AND B.ORDER_STATE = 'REL' 
                                    AND PMAXSTAB = @POWER
                    ", lotNum, AfterPower);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("getInfoForNamepalteLabelAutoPrintForPowerShow Error: " + ex.Message);
            }
            return dsReturn;
        }



        public DataSet getSizeForQTX(string prodId)
        {
            DataSet dsReturn = new DataSet();

            string sql = string.Empty;
            try
            {
                sql = string.Format(@"
                     SELECT CELL_SIZE FROM [POR_PRODUCT] 
                        WHERE  PRODUCT_CODE='{0}'
                        AND ISFLAG=1
                    ", prodId);

                dsReturn = db.ExecuteDataSet(CommandType.Text,sql);

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("getSizeForQTX Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet getInfoForTemplate()
        {
            DataSet dsReturn = new DataSet();

            string sql = string.Empty;
            try
            {
                sql = "SELECT * FROM POR_NAMEPLATE_AUTOPRINT";

                dsReturn = db.ExecuteDataSet(CommandType.Text,sql);

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("getInfoForTemplate Error: " + ex.Message);
            }
            return dsReturn;
        }
        public bool addTemplate(string prodId, string template, string editor, string editTime)
        {
            DataSet dsReturn = new DataSet();
            bool flag = false;
            string sql = string.Empty;
            try
            {
                sql =string.Format( "INSERT INTO POR_NAMEPLATE_AUTOPRINT VALUES ('{0}','{1}','{2}','{3}' ) ",
                    prodId,template,editor,editTime);

                flag = db.ExecuteNonQuery(CommandType.Text,sql) > 0?true:false ;

                return flag;

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("getInfoForTemplate Error: " + ex.Message);
                return false;
            }
            
        }
        public bool updateTemplate(string prodId, string template, string editor, string editTime)
        {
            DataSet dsReturn = new DataSet();
            bool flag=false;
            string sql = string.Empty;
            try
            {
                sql = string.Format("UPDATE POR_NAMEPLATE_AUTOPRINT SET TEMPLATE='{0}',EDITOR='{1}',EDIT_TIME='{2}' WHERE PROD_ID='{3}' ",
                    template, editor, editTime,prodId);

                flag = db.ExecuteNonQuery(CommandType.Text,sql)>0?true:false;

                return flag;

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("updateTemplate Error: " + ex.Message);
                return false;
            }

        }

        public DataSet getTemplateByProdId(string prodId)
        {
            DataSet dsReturn = new DataSet();

            string sql = string.Empty;
            try
            {
                sql = string.Format("SELECT * FROM POR_PRODUCT WHERE PRODUCT_CODE='{0}' AND ISFLAG=1  ", prodId);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("getTemplateByProdId Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
