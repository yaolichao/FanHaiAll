using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 来料接收数据的操作接口。
    /// </summary>
    public interface IMaterialReqOrReturnEngine
    {
        /// <summary>
        /// 根据工单获取原材料物料信息物料
        /// </returns>
        DataSet GetMaterials(string orderNum);
        /// <summary>
        /// 根据领料单号查询领料信息
        /// </summary>
        /// <param name="_numForSelect"></param>
        /// <returns>领料单信息抬头表和明细表的信息</returns>
        DataSet GetMatRequisitionInfByNum(string _numForSelect);
        /// <summary>
        /// 创建领料单
        /// </summary>
        /// <param name="dsIn">领料单抬头和明细信息</param>
        /// <returns></returns>
        DataSet CreateRequistionKoPo(DataSet dsIn);
        /// <summary>
        /// 通过领料单号判定领料单号是否已经存在
        /// </summary>
        /// <param name="number">领料单号</param>
        /// <returns>数量的数据集</returns>
        DataSet GetCountByNumToCheck(string number,int flag);
        /// <summary>
        /// 修改领料单
        /// </summary>
        /// <param name="dsSave">明细数据的新增，删除数据集</param>
        /// <returns></returns>
        DataSet UpdateRequistionKoPo(DataSet dsSave, string _editor, string _mblnr);
        /// <summary>
        /// 根据领料单号修改领料状态
        /// </summary>
        /// <param name="mblnr">领料单号</param>
        DataSet UpdateStatus(string mblnr,string editor);



        /// <summary>
        /// 根据退料单号查询退料信息
        /// </summary>
        /// <param name="_numForSelect"></param>
        /// <returns>退料单信息抬头表和明细表的信息</returns>
        DataSet GetMatRequisitionInfByNumTui(string _numForSelect);
        /// <summary>
        /// 创建退料单
        /// </summary>
        /// <param name="dsIn">退料单抬头和明细信息</param>
        /// <returns></returns>
        DataSet CreateRequistionKoPoTui(DataSet dsIn);
        /// <summary>
        /// 修改退料单
        /// </summary>
        /// <param name="dsSave">明细数据的新增，删除数据集</param>
        /// <returns></returns>
        DataSet UpdateRequistionKoPoTui(DataSet dsSave, string _editor, string _mblnr);
        /// <summary>
        /// 根据退料单号修改领料状态
        /// </summary>
        /// <param name="mblnr">退料单号</param>
        DataSet UpdateStatusTui(string mblnr, string editor);
        /// <summary>
        /// 根据工单,物料代码,批次号 获取已经领料的信息
        /// </summary>
        /// <param name="workorder">工单号</param>
        /// <param name="mat">物料号</param>
        /// <param name="charg">批次号</param>
        /// <returns></returns>
        DataSet GetMaterialstTui(string wordorder, string mat, string num);
        /// <summary>
        /// 根据工单获取已经领料的物料信息
        /// </summary>
        /// <param name="workorder">工单号</param>
        /// <returns></returns>
        DataSet GetMaterialstTui(string orderNumber);
        /// <summary>删除领料单
        /// 删除领料单
        /// </summary>
        /// <param name="_num">领料单号</param>
        /// <param name="name">修改人</param>
        /// <returns></returns>
        DataSet DeleteNum(string _num, string name);

        DataSet GetMaterialInf(DataTable paramTable, ref PagingQueryConfig config);

        DataSet GetMaterialRequisitionList(DataTable paramTable, ref PagingQueryConfig config);

        DataSet GetEquMaterialInf(DataTable paramTable, ref PagingQueryConfig config);

        DataSet GetMaterialSendingList(DataTable paramTable, ref PagingQueryConfig config);
    }
}
