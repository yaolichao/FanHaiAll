using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 自定义属性数据的数据管理接口。
    /// </summary>
    /// <remarks>
    /// ------------------------
    /// Category    (类似数据表)
    /// ------------------------
    /// Column      (类似数据表中的列)
    /// -----------------------
    /// Data        (类似数据表中的列对应的数据)
    /// -----------------------
    /// </remarks>
    public interface ICrmAttributeEngine
    {
        /// <summary>
        /// 添加自定义属性数据分类。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddBasicDataCategoryInfo(DataSet dsParams);
        /// <summary>
        /// 获取自定义属性数据分类。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。
        /// 数据表结构：
        ///  -------------------
        /// | 列名    |  列值   |
        ///  -------------------
        /// </param>
        /// <returns>包含自定义属性数据分类的数据集对象。</returns>
        DataSet GetBasicDataCategoryInfo(DataSet dsParams);
        /// <summary>
        /// 删除自定义属性数据分类。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据分类的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteBasicDataCategoryInfo(DataSet dsParams);
        /// <summary>
        /// 添加自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据列的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddBasicDataColumnInfo(DataSet dsParams);
        /// <summary>
        /// 获取自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含自定义属性数据列的数据集对象。</returns>
        DataSet GetBasicDataColumnIInfo(DataSet dsParams);
        /// <summary>
        /// 获取自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含自定义属性数据列的数据集对象。</returns>
        DataSet GetColumns(DataSet dsParams);
        /// <summary>
        /// 删除自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据列的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteBasicDataColumnInfo(DataSet dsParams);
        /// <summary>
        /// 更新自定义属性数据列。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据列的数据集对象</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateBasicDataColumnInfo(DataSet dsParams);
        /// <summary>
        /// 查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换。
        /// 将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="dataSet">
        /// 包含查询条件的数据集对象。数据集对象中包含两个数据表。数据表名分别为"Columns"和"Category"。
        /// "Columns"数据表存放方法返回给调用者的数据表的列名，"Category"数据表存放属性的分类名。
        /// "Columns"数据表包含一列，该列放置数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// "Category"数据表包含两列，第一列存放数据表 BASE_ATTRIBUTE_CATEGORY 的列名（一般设置为"CATEGORY_NAME"），
        /// 第二列为其对应的具体的值（一般为"CATEGORY_NAME"栏位中的值）。
        /// </param>
        /// <returns> 数据集对象。</returns>
        DataSet GetDistinctColumnsData(DataSet dsParams);
        /// <summary>
        /// 获取自定义属性数据列。
        /// </summary>
        /// <param name="dataSet">包含查询条件的数据集对象。
        /// --------------------------------
        /// {FIELDS_ATTRIBUTE_KEY,val}
        /// --------------------------------
        /// </param>
        /// <returns>包含自定义属性数据列的数据集对象。</returns>
        DataSet GetColumnInfoByAttributeKey(DataSet dsParams);
        /// <summary>
        /// 保存自定义属性数据。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveBasicData(DataSet dsParams);
        /// <summary>
        /// 获取所有自定义属性数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet GetAllData(DataSet dsParams);
        /// <summary>
        /// 根据自定义属性分类名或分类主键获取自定义属性列。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含自定义属性列的数据集对象。</returns>
        DataSet GetGruopBasicData(DataSet dsParams);
        /// <summary>
        /// 查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换。
        /// 将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// </param>
        /// <returns>包含基础数据信息的数据集对象。</returns>
        DataSet GetSpecifyAttributeData(DataSet dsParams);
        /// <summary>
        /// 删除自定义属性数据。
        /// </summary>
        /// <param name="dsParams">包含自定义属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteBasicData(DataSet dataSet);
        /// <summary>
        /// 根据自定义属性分类获取自定义属性列。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含自定义属性列的数据集对象。</returns>
        DataSet GetAttributsColumnsForSomeCategory(DataSet dsParams);
        /// <summary>
        /// 根据查询条件获取自定义属性数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// 数据集对象中包含三个数据表。数据表名分别为"Columns","Category"，<see cref="BASIC_CONST.PARAM_TABLENAME_CONDITIONS"/>。
        /// "Columns"数据表存放方法返回给调用者的数据表的列名，"Category"数据表存放属性的分类名。"Columns"数据表包含一列，该列放置数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// "Category"数据表包含两列，第一列存放数据表 BASE_ATTRIBUTE_CATEGORY 的列名（一般设置为"CATEGORY_NAME"）， 第二列为其对应的具体的值（一般为"CATEGORY_NAME"栏位中的值）。
        /// 条件数据表为可选，包含两列，第一个列存放属性名，第二列存放属性值，可以包含多行，使用AND连接起来查询数据。
        /// </param>
        /// <returns>包含自定义属性数据信息的数据集对象。</returns>
        DataSet GetBasicDataByConditons(DataSet dsParams);
    }
}
