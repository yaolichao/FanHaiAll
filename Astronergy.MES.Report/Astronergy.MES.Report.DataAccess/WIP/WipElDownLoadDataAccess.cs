using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Astronergy.MES.Report.DataAccess
{
    public class WipElDownLoadDataAccess : BaseDBAccess
    {
        private static Database db = DatabaseFactory.CreateDatabase();

        public DataSet GetInfByGuiNumPalletNumXuelieNum(string guinum, string palletnum, string xulienum, string strWorkOrderNumber, string strBengin, string strEnd)
        {

            const string storeProcedureName = "SP_ETL_WIP_EL_SELECT";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_guinum", DbType.String, guinum);
                this._db.AddInParameter(cmd, "p_palletnum", DbType.String, palletnum);
                this._db.AddInParameter(cmd, "p_xulienum", DbType.String, xulienum);
                this._db.AddInParameter(cmd, "p_workordernumber", DbType.String, strWorkOrderNumber);
                this._db.AddInParameter(cmd, "p_begin", DbType.String, strBengin);
                this._db.AddInParameter(cmd, "p_end", DbType.String, strEnd);

                return this._db.ExecuteDataSet(cmd);
            }

            //string sql = string.Empty;
            //DataSet dsReturn = new DataSet();
            //sql = "SELECT row_number()over(order by W.SHIPMENT_NO)as " + "行号" +",W.SHIPMENT_NO AS "+"柜号"+", ";
            //sql += " P.PALLET_NO AS "+"托号"+",P.LOT_NUMBER AS "+"组件序列号"+" from dbo.WMS_SHIPMENT W";
            //sql += " LEFT JOIN dbo.POR_LOT P ON W.PALLET_NO = P.PALLET_NO";
            //sql += " where W.IS_FLAG = 1 AND P.DELETED_TERM_FLAG <=1";
            //if(!string.IsNullOrEmpty(guinum))
            //    sql += " AND SHIPMENT_NO = '" + guinum + "' ";
            //if (!string.IsNullOrEmpty(palletnum))
            //    sql += " AND P.PALLET_NO = '" + palletnum + "' ";

            //if (!string.IsNullOrEmpty(palletnum))
            //    sql += " AND P.PALLET_NO IN ('" + palletnum + "' ";
            //dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            //return dsReturn;



        }

        public DataSet GetPicAddressRootPath()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT PIC_TYPE,PIC_ADDRESS,PIC_DATE_FORMAT FROM V_PIC_ADDRESS";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }
    }
}
