using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Astronergy.MES.Report.DataAccess
{
    public class PevlDataAccess : BaseDBAccess
    {
        public DataTable QueryPevl(string serialNo)
        {

            string sql = string.Format(" select t.ProdType 型号, SerialNo 条码信息, Cell 电池片信息, Eff 电池片效率, Slurry 浆料 from RPT_Pvel t where serialNo = '{0}'", serialNo);

            var ds = _db.ExecuteDataSet(CommandType.Text, sql);

            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return null;

            return ds.Tables[0];
        }
    }
}
