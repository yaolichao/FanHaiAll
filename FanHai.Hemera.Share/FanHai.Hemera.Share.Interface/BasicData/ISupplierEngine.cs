using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface ISupplierEngine
    {
        DataSet GetSupplierCode();                         // 初始化数据
        DataSet GetSupplierCode(string strSupplerName, string strSupplerCode);
        DataSet SupplierCodeInsert(DataSet dataSet);

        DataSet SupplierCodeUpdate(DataSet dataSet,string lblCode);

        DataSet DeleteSupplierCode(string supplierCode);


    }
}
