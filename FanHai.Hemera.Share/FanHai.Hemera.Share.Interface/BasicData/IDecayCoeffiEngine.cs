using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    public interface IDecayCoeffiEngine
    {
        /// <summary>
        /// 获得衰减数据方法
        /// </summary>
        /// <returns></returns>
        DataSet GetDecayCoeffiData();
        /// <summary>
        /// 保存衰减数据
        /// </summary>
        /// <param name="dsDecayCoeffi"></param>
        /// <returns></returns>
        DataSet SaveDecayCoeffiData(DataSet dsDecayCoeffi);
        /// <summary>
        /// 衰减代码不能重复
        /// </summary>
        /// <param name="dtInsertDecayCoeffi"></param>
        /// <returns></returns>
        DataSet IsExistDecayCoeffiData(DataTable dtInsertDecayCoeffi);
    }
}
