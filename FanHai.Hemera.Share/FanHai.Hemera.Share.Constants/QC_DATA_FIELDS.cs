using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarViewer.Hemera.Share.Constants
{
    public class QC_DATA_FIELDS : TableFields
    {
        #region columns define
        /// <summary>
        /// 主键
        /// </summary>
        public const string FIELD_QC_DATA_KEY = "QC_DATA_KEY";          //主键
        public const string FIELD_FACTORYROOM_KEY = "FACTORYROOM_KEY";  //工厂车间主键
        public const string FIELD_FACTORYROOM = "FACTORYROOM";          //工厂车间
        public const string FIELD_QC_LOT_NO = "QC_LOT_NO";              //送检条码
        public const string FIELD_SHIFTNAME = "SHIFTNAME";              //班别
        public const string FIELD_LINE_KEY = "LINE_KEY";                //线别主键
        public const string FIELD_LINE = "LINE";                        //线别
        public const string FIELD_MATERIAL_LOT = "MATERIAL_LOT";        //原材料批号
        public const string FIELD_OWNER = "OWNER";                      //供应商
        public const string FIELD_TYPE = "TYPE";                        //类型
        public const string FIELD_CLASSIFICATION = "CLASSIFICATION";    //效率档
        public const string FIELD_SEND_QTY = "SEND_QTY";                //送检数量
        public const string FIELD_SENDER = "SENDER";                    //送检人
        public const string FIELD_RECEIVE_QTY = "RECEIVE_QTY";          //接收数量
        public const string FIELD_RECEIVER = "RECEIVER";                //接收人
        public const string FIELD_A_QTY = "A_QTY";                      //A类数量
        public const string FIELD_B1_QTY = "B1_QTY";                    //B1类数量
        public const string FIELD_B2_QTY = "B2_QTY";                    //B2类数量
        public const string FIELD_DEFECT_QTY = "DEFECT_QTY";            //不良数量
        public const string FIELD_SCRAPPED_QTY = "SCRAPPED_QTY";        //废片数量
        public const string FIELD_CHECKER = "CHECKER";                  //检查人
        public const string FIELD_STATUS = "STATUS";                    //状态
        public const string FIELD_ISC = "ISC";                          //短路电流
        public const string FIELD_ORDER_NUMBER = "ORDER_NUMBER";        //工单号
        public const string FIELD_CREATOR = "CREATOR";                  //创建人
        public const string FIELD_CREATE_TIME = "CREATE_TIME";          //创建时间
        public const string FIELD_SEND_TIME = "SEND_TIME";              //送检时间
        public const string FIELD_CREATE_TIMEZONE = "CREATE_TIMEZONE";  //创建时间时区
        public const string FIELD_EDITOR = "EDITOR";                    //修改人
        public const string FIELD_EDIT_TIME = "EDIT_TIME";              //修改时间
        public const string FIELD_EDIT_TIMEZONE = "EDIT_TIMEZONE";      //修改时间时区
        public const string FIELD_EFFICIENCY_GROUP = "EFFICIENCY_GROUP";//效率档组
        #endregion

        #region table name define
        public const string DATABASE_TABLE_NAME = "POR_QC_DATA";        //TABLE NAME
        #endregion

        #region get TABLE_NAME
        /// <summary>
        /// override TABLE_NAME
        /// </summary>
        public override string TABLE_NAME
        {
            get
            {
                return DATABASE_TABLE_NAME;
            }
        }
        #endregion

        #region get FIELDS
        /// <summary>
        /// override fields
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return _fields;
            }
        }
        #endregion

        #region define dictionary of containing column's fields
        //define _fields
        private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_QC_DATA_KEY , new FieldProperties() },                     
                                                                  {FIELD_FACTORYROOM_KEY , new FieldProperties() },   
                                                                  {FIELD_FACTORYROOM , new FieldProperties() },
                                                                  {FIELD_QC_LOT_NO , new FieldProperties() },              
                                                                  {FIELD_SHIFTNAME , new FieldProperties() },                
                                                                  {FIELD_LINE_KEY , new FieldProperties() },
                                                                  {FIELD_LINE , new FieldProperties() },      
                                                                  {FIELD_MATERIAL_LOT , new FieldProperties() },                
                                                                  {FIELD_OWNER , new FieldProperties() },               
                                                                  {FIELD_TYPE , new FieldProperties() },               
                                                                  {FIELD_CLASSIFICATION , new FieldProperties() },             
                                                                  {FIELD_SEND_QTY , new FieldProperties() },                  
                                                                  {FIELD_SENDER , new FieldProperties() },                
                                                                  {FIELD_RECEIVE_QTY , new FieldProperties() },                
                                                                  {FIELD_RECEIVER , new FieldProperties() },                    
                                                                  {FIELD_A_QTY, new FieldProperties() },                
                                                                  {FIELD_B1_QTY , new FieldProperties() },                  
                                                                  {FIELD_B2_QTY , new FieldProperties() },                   
                                                                  {FIELD_DEFECT_QTY , new FieldProperties() },        
                                                                  {FIELD_SCRAPPED_QTY , new FieldProperties() },         
                                                                  {FIELD_CHECKER  , new FieldProperties() },             
                                                                  {FIELD_STATUS  , new FieldProperties(typeof(int),true,0) },  
                                                                  {FIELD_ISC , new FieldProperties() },    
                                                                  {FIELD_ORDER_NUMBER , new FieldProperties() },    
                                                                  {FIELD_CREATOR , new FieldProperties() }, 
                                                                  {FIELD_CREATE_TIME , new FieldProperties(typeof(DateTime),false,DateTime.Now) },          
                                                                  {FIELD_SEND_TIME , new FieldProperties(typeof(DateTime),false,DateTime.Now) },    
                                                                  {FIELD_CREATE_TIMEZONE  , new FieldProperties() },  
                                                                  {FIELD_EDITOR  , new FieldProperties() },  
                                                                  {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime),false,DateTime.Now) } ,
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() },      
                                                                  {FIELD_EFFICIENCY_GROUP , new FieldProperties() }

                                                                  //------------------add by daolin----------------------------------------------------
                                                                  //{FIELD_HOLD_FLAG , new FieldProperties() },                       //0:否,1:是
                                                                  //{FIELD_STATE_FLAG , new FieldProperties() },                      //0:WaitingForTrackIn,4:WaitingForEDC,5:InEDC,9:WaitingForTrackout
                                                                  //{FIELD_WORK_ORDER_SEQ , new FieldProperties() },                  //当前批次在WorkOrder的序列号
                                                                  //{FIELD_EDC_INS_KEY, new FieldProperties() },                      //InEDC的时候才会有值，用于关联EDC_MAIN_INS表
                                                                  //{FIELD_QUANTITY_CLOSED, new FieldProperties() },            //批次关闭时数量，暂不用
                                                                  //{FIELD_QUANTITY_CONSUMED, new FieldProperties() },           //批次关闭时数量，暂不用
                                                                  //{FIELD_QUANTITY_FINISHED, new FieldProperties() },               //暂不用
                                                                  //{FIELD_QUANTITY_SAMPLED, new FieldProperties() },                  //批次结束时数量，暂不用
                                                                  //{FIELD_QUANTITY_SCRAPPED, new FieldProperties() },               //批次抽检数量，暂不用
                                                                  //{FIELD_QUANTITY_SCRAPPED_CONSUMED, new FieldProperties() },      //暂不用
                                                                  //{FIELD_QUANTITY_SERIALIZED, new FieldProperties() },                   //暂不用
                                                                  //{FIELD_QUANTITY_SHIPPED, new FieldProperties() },                          //批次发货数量，暂不用
                                                                  //{FIELD_QUANTITY_YIELD_FINISHED, new FieldProperties() },            //暂不用
                                                                  //{FIELD_QUANTITY_YIELD_INITIAL, new FieldProperties() },              //暂不用
                                                                  //{FIELD_QUANTITY_REWORK, new FieldProperties() },                           //暂不用    
                                                                  //{FIELD_START_WAIT_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)	},
                                                                  //{FIELD_START_PROCESS_TIME,new FieldProperties(typeof(DateTime),true,DateTime.Now)},
                                                                  //{FIELD_DELETED_TERM_FLAG,new FieldProperties()},
                                                                  //{FIELD_SHIPPED_FLAG,new FieldProperties()},
                                                                  //{FIELD_IS_PRINT,new FieldProperties()},
                                                                  ////-----------------------------------------
                                                                  //{FIELD_OPERATOR,new FieldProperties()},
                                                                  //{FIELD_OPR_LINE,new FieldProperties()},
                                                                  //{FIELD_OPR_LINE_PRE,new FieldProperties()},
                                                                  //{FIELD_OPR_COMPUTER,new FieldProperties()},
                                                                  //{FIELD_CHILD_LINE,new FieldProperties()},
                                                                  //{FIELD_SHIPNUMBER,new FieldProperties()}
                                                              };
        #endregion

    }
}
