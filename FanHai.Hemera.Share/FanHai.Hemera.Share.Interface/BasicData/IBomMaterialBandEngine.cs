using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IBomMaterialBandEngine
    {
        /// <summary>
        /// 根据物料料号和代码查询结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="barCode"></param>
        /// <returns>结果数据集</returns>
        DataSet GetMaterialDateByCodeAndBarcode(string code, string barCode);
        /// <summary>
        /// 获取工单BOM中的所有料号
        /// </summary>
        /// <returns></returns>
        DataSet GetBomMaterial();
        /// <summary>
        /// 根据料号删除行信息
        /// </summary>
        /// <param name="_code">物料料号</param>
        /// <returns>结果集</returns>
        DataSet DeleteMaterialCode(string _code);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dataSet">数据集</param>
        /// <returns></returns>
        DataSet MaterialDateInsert(DataSet dataSet);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataSet">数据集</param>
        /// <returns></returns>
        DataSet MaterialDateUpdate(DataSet dataSet);
    }
}
