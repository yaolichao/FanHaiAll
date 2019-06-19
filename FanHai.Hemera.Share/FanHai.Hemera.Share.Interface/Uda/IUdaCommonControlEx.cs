using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 自定义属性、生产线及其自定义属性数据的管理接口。
    /// </summary>
    public interface IUdaCommonControlEx
    {
        /// <summary>
        /// 获取自定义属性对象类型。
        /// </summary>
        /// <returns>包含自定义属性对象类型的数据集对象。</returns>
        DataSet GetUdaObjectType();
        /// <summary>
        /// 根据自定义属性数据主键获取对象类型。
        /// </summary>
        /// <param name="objKey">自定义属性数据主键。</param>
        /// <returns>自定义属性数据对应的对象类型。</returns>
        DataSet GetUdaObjectNameList(string objType);
        /// <summary>
        /// 为生产线添加自定义属性值。
        /// </summary>
        /// <param name="dataSet">包含生产线自定义属性的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddLineAttributeValue(DataSet dataSet);
        /// <summary>
        /// 根据生产线名获取生产线对应的自定义属性数据。
        /// </summary>
        /// <param name="lineName">生产线名。</param>
        /// <returns>包含自定义属性数据的数据集对象。</returns>
        DataSet GetLineTypeByName(string AttributeName);
        /// <summary>
        /// 查询生产线数据
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含生产线数的数据集对象。</returns>
        DataSet SearchLineAttribute(DataSet dataSet);
        /// 更新生产线及其对应的自定义属性数据。
        /// </summary>
        /// <param name="dsParams">包含生产线及其对应的自定义属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateLineTypeAttribute(DataSet dataSet);
        /// <summary>
        /// 删除生产线及其对应的自定义属性数据。
        /// </summary>
        /// <param name="objectKey">生产线主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteLineTypeAttribute(string objectKey);
        /// <summary>
        /// 根据自定义属性类别获取自定义属性信息。
        /// </summary>
        /// <param name="objCategory">自定义属性类别。</param>
        /// <returns>包含自定义属性信息的数据集对象。</returns>
        DataSet GetObjectAttributsForCategory(string objCategory);
        /// <summary>
        /// 更新生产线信息。
        /// </summary>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="lineName">生产线名称。</param>
        /// <param name="lineCode">生产线代码。</param>
        /// <param name="description">描述。</param>
        /// <param name="editor">编辑人。</param>
        /// <param name="edit_timezone">编辑时区。</param>
        /// <returns>更新生产线信息的记录数。</returns>
        int UpdateLineInfo(string lineKey, string lineName, string lineCode,
                           string description, string editor, string edit_timezone); 

    }
}
