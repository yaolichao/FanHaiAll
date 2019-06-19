using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IMaterialBuckleControlEngine
    {
        /// <summary>获取参数值
        /// 获取参数值
        /// </summary>
        /// <returns></returns>
        DataSet GetParameter();
        /// <summary>通过参数名获取材料耗用设定基础表参数信息
        /// 通过参数名获取参数信息
        /// </summary>
        /// <param name="parameter">参数名</param>
        /// <returns></returns>
        DataSet GetInfByParameter(string parameter);
        /// <summary>新增信息到材料耗用设定基础表
        /// 新增信息到材料耗用设定基础表
        /// </summary>
        /// <param name="parameter">参数值</param>
        /// <param name="useqty">扣料数量</param>
        /// <param name="useunit">扣料单位</param>
        /// <param name="conrtastQty">扣料对应数量</param>
        /// <param name="conrtastUnt">扣料对应数量单位</param>
        /// <param name="name">创建人</param>
        /// <returns></returns>
        DataSet InsertNewInf(string parameter, string useqty, string useunit, string conrtastQty, string conrtastUnt, string name);

        /// <summary>删除信息
        /// 删除信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        DataSet DeleteInf(string parameter);
        /// <summary>新增信息到材料耗用设定基础表
        /// 新增信息到材料耗用设定基础表
        /// </summary>
        /// <param name="parameter">参数值</param>
        /// <param name="useqty">扣料数量</param>
        /// <param name="useunit">扣料单位</param>
        /// <param name="conrtastQty">扣料对应数量</param>
        /// <param name="conrtastUnt">扣料对应数量单位</param>
        /// <param name="name">创建人</param>
        /// <returns></returns>
        DataSet UpdateParameterInf(string parameter, string useqty, string useunit, string conrtastQty, string conrtastUnt, string name);
    }
}
