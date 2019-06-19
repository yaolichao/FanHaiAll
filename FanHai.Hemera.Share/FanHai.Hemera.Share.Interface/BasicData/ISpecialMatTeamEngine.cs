using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface ISpecialMatTeamEngine
    {
        /// <summary>
        /// 获取工单数据集
        /// </summary>
        /// <returns></returns>
        DataSet GetWorkNumber();
        /// <summary>
        /// 获取工单bom物料数据
        /// </summary>
        /// <param name="workOrder">工单号</param>
        /// <returns></returns>
        DataSet GetMaterialByWorkOrder(string workOrder);
        /// <summary>
        /// 根据工单物料参数组获取特殊物料管控设定
        /// </summary>
        /// <param name="_workOrder">工单</param>
        /// <param name="_material">物料号</param>
        /// <param name="_paramTeam">参数组</param>
        /// <returns>特殊物料信息数据集</returns>
        DataSet GetMatSpecialInf(string _workOrder, string _material, string _paramTeam);
        /// <summary>
        /// 根据工单号物料号参数组删除信息
        /// </summary>
        /// <param name="_workOrderNum">工单</param>
        /// <param name="_mat">物料</param>
        /// <param name="_paramerTeam">参数组</param>
        /// <returns></returns>
        DataSet DeleteMatSpecialInf(string _workOrderNum, string _mat, string _paramerTeam);
        /// <summary>
        /// 获取全部信息
        /// </summary>
        /// <returns></returns>
        DataSet GetMatSpecialInf();
        /// <summary>
        /// 修改特殊物料管控参数
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="dataSet">主键</param>
        /// <returns></returns>
        DataSet UpdateSpecialMatTeam(DataSet dataSet,string keyNum);
        /// <summary>
        /// 获取参数组信息
        /// </summary>
        /// <returns></returns>
        DataSet GetParamerTeam();
    }
}
