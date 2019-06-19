using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Astronergy.Modules.WMS
{
    public class GetBill
    {
        public string GetBillNo(string BillType,string TableName, string BillField,Database db)
        {
            string returnNo = string.Empty;
            string dateStr = string.Empty;
            string tempCode = string.Empty;
            DataSet dsRetrun = new DataSet();

            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            dbConn = db.CreateConnection();
            dbConn.Open();
            dbTran = dbConn.BeginTransaction();
            //取当前年月
            string sql = string.Format(@"select convert(char(10), getdate(), 102) as date");
            dsRetrun = db.ExecuteDataSet(dbTran, CommandType.Text, sql.ToString());
            dateStr=(dsRetrun.Tables[0].Rows[0][0].ToString()).Substring(2, 2);
            dateStr += (dsRetrun.Tables[0].Rows[0][0].ToString()).Substring(5, 2);

            sql = string.Format(@"select max(" + BillField + ") from " + TableName+" where "+ BillField
                                        + " like '" + BillType + dateStr+"%'");
            dsRetrun = db.ExecuteDataSet(dbTran, CommandType.Text, sql.ToString());

            if ((dsRetrun.Tables[0].Rows[0][0].ToString())=="")
            {
                returnNo = BillType + dateStr + "00001";
            }
            else
            {
                tempCode = (dsRetrun.Tables[0].Rows[0][0].ToString()).Substring(6, 5);
                tempCode = Convert.ToString(Convert.ToInt16(tempCode) + 1);
                while (tempCode.Length<5)
                {
                    tempCode = "0" + tempCode;
                }
                returnNo = BillType + dateStr + tempCode;
            }
            return returnNo;
        } 
    }
}
