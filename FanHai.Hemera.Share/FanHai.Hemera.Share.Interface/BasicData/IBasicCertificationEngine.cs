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
    public interface IBasicCertificationEngine
    {
        /// <summary>
        /// 获取认证数据
        /// </summary>
        /// <returns></returns>
        DataSet GetCertification(string certificationType, DateTime certificationDate);
        /// <summary>
        /// 获取有效的认证数据
        /// </summary>
        /// <returns></returns>
        DataSet GetValidCertification();
        /// <summary>
        /// 获取有效的认证类型
        /// </summary>
        /// <returns></returns>
        DataSet GetValidCertificationType();
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dsChange"></param>
        /// <returns></returns>
        DataSet SaveCertification(DataSet dsChange);
    }
}
