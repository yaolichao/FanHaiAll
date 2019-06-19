
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    public interface IGroupArkEngine
    {
        /// <summary>
        /// 获得托信息
        /// </summary>
        /// <param name="hsTable"></param>
        /// <returns></returns>
        DataSet GetPalletData(string palletNo);

        //获取柜号
        DataSet GetArkNumber(string arkCode);
        //查询明细表中柜主键为同一个且托状态为可用的托号
        DataSet GetContainerDetailInf(string key);
        //根据查询出来的托号,在包装表中查询托对应的详细信息
        DataSet GetWipConInf(DataSet ds);
        
        DataSet UpdateArkInf(DataSet ds, DataTable dt,int flag);
        //有无组柜信息查询
        DataSet QueryInf(string _arkCode, string _status, string _palletNo);

        DataSet QueryInfHaveArked(string _teEntryNo, string _cmbStatus);
    }
}
