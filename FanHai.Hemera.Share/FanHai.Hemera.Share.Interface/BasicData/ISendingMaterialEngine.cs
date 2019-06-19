using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface ISendingMaterialEngine
    {
        /// <summary>捞取维护的参数对应扣料信息
        /// 捞取维护的参数对应扣料信息
        /// </summary>
        /// <returns></returns>
        DataSet GetParameters();
        /// <summary>
        /// 插入数据到发料退料记录表，同时修改设备虚拟仓信息
        /// </summary>
        /// <param name="dtInf">数据信息表</param>
        /// <returns></returns>
        DataSet InsertNewInf(DataTable dtInf);
        /// <summary>
        /// 插入数据到发料退料记录表，同时修改设备虚拟仓信息
        /// </summary>
        /// <param name="dtInf">数据信息表</param>
        /// <returns></returns>
        DataSet UpdateParameterInf(DataTable dtInf);

        DataSet GetMatEquipmentStore(string facKey, string equipmentKey, string operationName, string lineKey, string parameterKey, string matCode, string orderNumber,string _type);
    }
}
