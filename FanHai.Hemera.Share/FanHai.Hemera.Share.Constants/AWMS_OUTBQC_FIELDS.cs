using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    //OUTB_ITEM_FIELDS TableFields
    public class AWMS_OUTB_ITEM_FIELDS : TableFields
    {
        public const string Field_OUTBANDNO = "OUTBANDNO";           //出库交货单号
        public const string Field_VBELN = "VBELN";                   //外向交货单号
        public const string Field_QC_RESULT = "QC_RESULT";
        public const string Field_BATCHNO = "BATCHNO";
        public const string TABLE_NAME_OUTB_ITEM = "AWMS_OUTB_ITEM";  //外向交货明细表

        public override string TABLE_NAME
        {

            get
            {
                return TABLE_NAME_OUTB_ITEM;
            }
        }
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return Dic;
            }
        }

        private static Dictionary<string, FieldProperties> Dic = new Dictionary<string, FieldProperties>
        {
            {Field_OUTBANDNO,new FieldProperties()},
            {Field_VBELN,new FieldProperties()},           
            {Field_QC_RESULT,new FieldProperties()},
            {Field_BATCHNO,new FieldProperties()}                 
        };

    }

    //AWMS_CONTAINER_DETAIL TableFields
    public class AWMS_CONTAINER_DETAIL_FIELDS : TableFields
    {
        public const string Field_PALLET_NO = "PALLET_NO";           //托号
        public const string TABLE_NAME_AWMS_CONTAINER_DETAIL = "AWMS_CONTAINER_DETAIL";  //柜明细表

        public override string TABLE_NAME
        {

            get
            {
                return TABLE_NAME_AWMS_CONTAINER_DETAIL;
            }
        }
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return Dic;
            }
        }

        private static Dictionary<string, FieldProperties> Dic = new Dictionary<string, FieldProperties>
        {
            {Field_PALLET_NO,new FieldProperties()}        
        };

    }
    //AWMS_OUTB_QC 
    public class AWMS_OUTB_QC_FIELDS : TableFields
    {
        public const string Field_VBELN = "VBELN";
        public const string Field_POSNR = "POSNR";
        public const string Field_PKG_MAT = "PKG_MAT";
        public const string Field_BILL_BRND = "BILL_BRND";
        public const string Field_CANTR="CANTR";
        public const string Field_F_OUTB_CUSTM = "F_OUTB_CUSTM";
        public const string Field_EL = "EL";
        public const string Field_DATA_FORMT = "DATA_FORMT";
        public const string Field_LIST_ABSENCE = "LIST_ABSENCE";
        public const string Field_LiST_ERR = "LiST_ERR";
        public const string Field_CELL = "CELL";
        public const string Field_MOD_ERR = "MOD_ERR";
        public const string Field_QLVL_ERR = "QLVL_ERR";
        public const string Field_FRAME = "FRAME";
        public const string Field_BRND_PARM_ERR = "BRND_PARM_ERR";
        public const string Field_CONT_LOCK_BRK = "CONT_LOCK_BRK";
        public const string Field_CUSTM_CK = "CUSTM_CK";
        
        public const string Field_OUTBANDNO = "OUTBANDNO";

        public const string TABLE_NAME_OUTB_QC = "AWMS_OUTB_QC";


        public override string TABLE_NAME
        {

            get
            {
                return TABLE_NAME_OUTB_QC;
            }
        }
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return Dic;
            }
        }

        private static Dictionary<string, FieldProperties> Dic = new Dictionary<string, FieldProperties>
        {
            {Field_OUTBANDNO,new FieldProperties()},
            {Field_VBELN,new FieldProperties()},
            {Field_POSNR,new FieldProperties()},
            {Field_PKG_MAT,new FieldProperties()},
            {Field_BILL_BRND,new FieldProperties()},
            {Field_CANTR,new FieldProperties()},
            {Field_F_OUTB_CUSTM,new FieldProperties()},
            {Field_EL,new FieldProperties()},
            {Field_DATA_FORMT,new FieldProperties()},
            {Field_LIST_ABSENCE,new FieldProperties()},
            {Field_LiST_ERR,new FieldProperties()},
            {Field_CELL,new FieldProperties()},
            {Field_MOD_ERR,new FieldProperties()},
            {Field_QLVL_ERR,new FieldProperties()},
            {Field_FRAME,new FieldProperties()},
            {Field_BRND_PARM_ERR,new FieldProperties()},
            {Field_CONT_LOCK_BRK,new FieldProperties()},
            {Field_CUSTM_CK,new FieldProperties()}
        };

    }
}
