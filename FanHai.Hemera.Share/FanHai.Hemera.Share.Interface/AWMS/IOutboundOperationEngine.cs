using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IOutboundOperationEngine
    {
        //查询外向交货单
        DataSet Query(DataSet dsParams, ref PagingQueryConfig config);
        //根据外向交货单号或者出库单号查询外向交货单抬头相关数据
        DataSet getOutboudInfo(string OutboundNo, string VebelNO, string ShipmentNO, string SType);
        //根据外向交货单号或者出库单号查询明细
        DataSet getOutboudItem(string OutboundNo, string VebelNO, string ShipmentNO, string SType);
        //保存出库检验结果
        string SetQcResult(DataSet dsParams, string OutboundNo, string VebelNO, string QC_PERSON, string IsEdit,out bool result);
        //删除出库检验结果
        void DeleteQcResult(string OutboundNo, string VebelNO, string Del_Person);
        //更新表字段
        string UpdateTable(string Msg, string VebelNO, string Outboudno, int Flag);
        //更新柜号等信息
        DataSet UpdateConteinerNo(string OutboundNo,string VebelNO, string containerNo);
        
    }

}
