
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    public interface IPorProductEngine
    {
        /// <summary>
        /// 获得产品数据
        /// </summary>
        /// <param name="hsTable"></param>
        /// <returns></returns>
        DataSet GetPorProductData(Hashtable hsTable);
        /// <summary>
        /// 保存产品设置数据
        /// </summary>
        /// <param name="dsPorProduct"></param>
        /// <returns></returns>
        DataSet SavePorProductData(DataSet dsPorProduct);
        /// <summary>
        /// 根据PROID获得产品号的主产品等级
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        DataSet GetProductDtlGrade(string proid);
        /// <summary>
        /// 通过序列号获取金刚线的值
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns>金刚线的</returns>
        DataSet GetKingLineInf(string lotNumber);
    }
}
