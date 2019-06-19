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
    public interface IProductModelEngine
    {
        /// <summary>
        /// 保存产品型号及控制项
        /// </summary>
        /// <param name="dsProModel"></param>
        /// <returns></returns>
        DataSet SaveProductModel(DataSet dsProModel);
        /// <summary>
        /// 获得产品型号及控制项
        /// </summary>
        /// <returns></returns>
        DataSet GetProductModelAndCP();
        /// <summary>
        /// 获得产品认证信息
        /// </summary>
        /// <returns></returns>
        DataSet GetCertification();
        /// <summary>
        /// 删除产品型号及控制各项信息
        /// </summary>
        /// <param name="dsProModel"></param>
        /// <returns></returns>
        DataSet DelProductModel(DataSet dsProModel);
        /// <summary>
        /// 产品型号是否重复
        /// </summary>
        /// <param name="dtInsertProductModel"></param>
        /// <returns></returns>
        DataSet IsExistProductModel(DataTable dtInsertProductModel);
    }
}
