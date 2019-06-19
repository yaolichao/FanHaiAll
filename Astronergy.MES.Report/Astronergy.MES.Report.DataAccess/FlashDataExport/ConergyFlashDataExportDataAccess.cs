using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Astronergy.MES.Report.DataAccess
{
    public class ConergyFlashDataExportDataAccess : BaseDBAccess
    {
        public DataSet GetConergyFlashData(string invoiceNumber, string deliveryNumber, string startShippingData, string endShippingData)
        {
            string strSql = string.Empty;
            string strWhere = string.Empty;
            DataSet dsConergyFlashData = null;
            try
            {
                strSql = @"SELECT A.LOT_NUMBER AS 'OEMSerialNumber',
	                               A.LOT_NUMBER AS 'SerialNumber',
	                               A.PALLET_NO AS 'PalletNumber',
	                               A.PALLET_NO AS 'OEMPalletNumber',
	                               CAST(B.COEF_PMAX AS decimal(18,2)) AS 'Maxpower',
	                               CAST(B.COEF_VMAX AS decimal(18,2)) AS 'MaxpowerVoltage',
	                               CAST(B.COEF_IMAX AS decimal(18,2)) AS 'MaxpowerCurrent',
	                               CAST(B.COEF_VOC AS decimal(18,2)) AS 'OpencircuitVoltage',
	                               CAST(B.COEF_ISC AS decimal(18,2)) AS 'ShortCircuitCurrent',
	                               CAST(B.COEF_FF * 100 AS decimal(18,2)) AS 'Fillfactor',
	                               REPLACE(REPLACE(B.VC_MODNAME,'P',''),'M','') AS 'NominalMaxpower',
	                               (SELECT TOP 1 C.PS_SUBCODE FROM POR_WO_PRD_PS C WHERE C.PART_NUMBER = A.PART_NUMBER AND C.WORK_ORDER_KEY = A.WORK_ORDER_KEY AND C.PS_SEQ = B.I_IDE ORDER BY CONVERT(INT,C.VERSION_NO) DESC ) AS 'ProductNumber',
	                               SUBSTRING(A.PRO_ID,1,CHARINDEX('-','CHSM6610-C0S'))	AS 'OEMProductID',
	                               'Conergy PH ' + B.VC_MODNAME  AS 'ModuleType',
	                               E.CONTAINER_NO AS 'DeliveryNumber',
	                               '' AS 'NavisionNumber',
	                               CONVERT(varchar(100), B.TTIME, 120)  AS 'Flashtime',
	                               '' AS 'RawFrameCode',
	                               D.CUS_NAME AS 'CustomerName',
	                               D.CUS_PO AS 'CustomerOrderNo',
	                               SUBSTRING(D.REMARK,7,LEN(D.REMARK)) AS 'ShippingAddress',
	                               '' AS 'ShippingDate',
	                               'Chint Solar(Zhejiang)Company Ltd' AS 'Manufacturer',
                                   E.CI_NO AS 'InvoiceNumber'
                            FROM POR_LOT A
                            INNER JOIN WIP_IV_TEST B ON A.LOT_NUMBER = B.LOT_NUM AND B.VC_DEFAULT = 1
                            INNER JOIN POR_WORK_ORDER D ON A.WORK_ORDER_NO = D.ORDER_NUMBER
                            INNER JOIN WMS_SHIPMENT E ON A.PALLET_NO = E.PALLET_NO AND E.IS_FLAG = 1
                            WHERE A.STATUS < 2
                            AND A.LOT_TYPE='N' ";

                if (!string.IsNullOrEmpty(invoiceNumber))
                {
                    strWhere += string.Format(" AND E.CI_NO = '{0}' ", invoiceNumber);
                }
                if (!string.IsNullOrEmpty(deliveryNumber))
                {
                    strWhere += string.Format(" AND E.CONTAINER_NO = '{0}' ", deliveryNumber);
                }
                if (!string.IsNullOrEmpty(startShippingData) && !string.IsNullOrEmpty(endShippingData))
                {
                    strWhere += string.Format(" AND E.SHIPMENT_DATE BETWEEN '{0}' AND '{1}' ", startShippingData,endShippingData);
                }
                strWhere = strWhere + " ORDER BY E.PALLET_NO ASC,A.LOT_NUMBER ASC";

                strSql += strWhere;

                using (DbConnection con = this._db.CreateConnection())
                {
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = strSql;
                    dsConergyFlashData = this._db.ExecuteDataSet(cmd);
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return dsConergyFlashData;
        }
    }
}
