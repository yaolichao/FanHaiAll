using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IByProductEngine
    {
        DataSet GetByProductInf(DataSet dataSet);       // 传入主料号,低效料号,二三级料号查询信息,组件类型，工厂，修改时间，创建时间的dateset
        DataSet GetLotPartInf(string strCode);          // 传入控件主料号,低效料号,二三级料号获取详细信息

        DataSet ByProductCodeInsert(DataSet dataSet);
        DataSet ByProductCodeUpdate(DataSet dataSet, string number);
        DataSet DeleteByProductCode(string key);


        DataSet ProUpdate(DataSet dsSetIn);

        DataSet ProIsert(DataSet dsSetIn);
    }
}
