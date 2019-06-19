#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
#endregion

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 原因代码数据管理接口。
    /// </summary>
    public interface IReasonCodeEngine
    {
        /// <summary>
        /// 插入原因代码数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含原因代码数据的数据集。<see cref="FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME"/>
        /// </param>
        /// <returns>
        /// 包含执行结果信息的数据集对象。
        /// </returns>
        DataSet ReasonCodeInsert(DataSet dsParams);
        /// <summary>
        /// 更新原因代码数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含原因代码数据的数据表集。<see cref="FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME"/>
        /// </param>
        /// <returns>
        /// 包含执行结果信息的数据集对象。
        /// </returns>
        DataSet ReasonCodeUpdate(DataSet dsParams);
        /// <summary>
        /// 根据原因代码主键删除原因代码数据。
        /// </summary>
        /// <param name="codeKey">
        /// 原因代码主键。
        /// </param>
        /// <returns>
        /// 包含执行结果信息的数据集对象。
        /// </returns>
        DataSet DeleteReasonCode(string codeKey);
        /// <summary>
        /// 获取原因代码数据。
        /// </summary>
        /// <param name="dataTable">
        /// 包含查询条件名称和值的数据表对象。
        /// [REASON_CODE_NAME]（可选，左右模糊匹配查询。）
        /// [REASON_CODE_TYPE]（可选，左右模糊匹配查询。）
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集对象。
        /// [REASON_CODE_KEY,REASON_CODE_NAME,DESCRIPTIONS,EDITOR,EDIT_TIME,EDIT_TIMEZONE,REASON_CODE_TYPE,STATUS]
        /// </returns>
        DataSet GetReasonCode(DataTable dtParams);
        /// <summary>
        /// 获取原因代码数据。
        /// </summary>
        /// <param name="type">
        /// 原因代码类型。
        /// </param>
        /// <param name="name">
        /// 原因代码名称。
        /// </param>
        /// <param name="rcClass">
        /// 原因代码分类。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集对象。
        /// </returns>
        DataSet GetReasonCode(string type, string name, string rcClass);
        /// <summary>
        /// 获取不重复的原因代码数据。
        /// </summary>
        /// <returns>
        /// 包含不重复原因代码数据的数据集对象。
        /// [REASON_CODE_KEY,REASON_CODE_NAME]
        /// </returns>
        DataSet GetDistinctReasonCodeName();

         /// <summary>
        /// 获取不存在于某个类别主键中的原因代码数据。
        /// </summary>
        /// <param name="categoryKey">原因代码类别主键。</param>
        /// <param name="categoryType">原因代码类别所属类型。</param>
        /// <returns>
        /// 包含原因代码数据的数据集对象。
        /// [REASON_CODE_KEY,REASON_CODE_NAME]
        /// </returns>
        DataSet GetReasonCodeNotExistCategory(string categoryKey, string categoryType);
        /// <summary>
        /// 插入原因代码类别数据。
        /// </summary>
        /// <param name="dataSet">
        /// 包含原因代码类别数据的数据表集。<see cref="FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME"/>
        /// </param>
        /// <returns>
        /// 包含执行结果信息的数据集对象。
        /// </returns>
        DataSet ReasonCodeCategoryInsert(DataSet dataSet);
        /// <summary>
        /// 删除原因代码类型。
        /// </summary>
        /// <param name="codeCategoryKey">原因代码类型主键。</param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet DeleteReasonCodeCategory(string codeCategoryKey);
        /// <summary>
        /// 获取原因代码类型数据。
        /// </summary>
        /// <param name="dtParams">包含查询条件的数据集对象。 </param>
        /// <returns>包含原因代码类型数据的数据集对象。</returns>
        DataSet GetReasonCodeCategory(DataTable dtParams);
        /// <summary>
        /// 获取不重复的原因代码类型数据。
        /// </summary>
        /// <returns>包含原因代码类型数据的数据集对象。</returns>
        DataSet GetDistinctReasonCodeCategoryName();
        /// <summary>
        /// 根据原因代码名称和类型获取原因代码类型数据。
        /// </summary>
        /// <param name="dsParams">包含查询参数的数据集对象。</param>
        /// <returns>包含原因代码类型数据的数据集对象。</returns>
        DataSet GetReasonCodeByNameAndType(DataSet dsParams);
        /// <summary>
        /// 更新原因代码类型数据。
        /// </summary>
        /// <param name="dsParams">包含原因代码类型数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet ReasonCodeCategoryUpdate(DataSet dsParams);
        /// <summary>
        /// 根据原因代码分类主键获取原因代码数据。
        /// </summary>
        /// <param name="categoryKey">原因代码分类主键。</param>
        /// <returns>
        /// 包含原因代码数据的数据集对象。[REASON_CODE_KEY, REASON_CODE_NAME]
        /// </returns>
        DataSet GetReasonCategory(string categoryKey);
        /// <summary>
        /// 添加原因代码和类型的关联。
        /// </summary>
        /// <param name="dsParams">包含原因代码和类型关联的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddReasonCategory(DataSet dataset);
        /// <summary>
        /// 删除原因代码和类型的关联。
        /// </summary>
        /// <param name="dsParams">包含原因代码和类型关联的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteReasonCategory(DataSet dataset);
        /// <summary>
        /// 根据原因代码分组名称获取超时原因代码数据。
        /// </summary>
        /// <param name="categoryName">
        /// 原因代码分组名称,
        /// 如果为空则获取原因代码分组名称为DLR_DEFAULT的超时原因代码。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集。
        /// </returns>
        DataSet GetReasonCodeForDelay(string categoryName);
        /// <summary>
        /// 根据原因代码分组名称获取退库原因代码数据。
        /// </summary>
        /// <param name="categoryName">
        /// 原因代码分组名称,
        /// 如果为空则获取原因代码分组名称为TK_DEFAULT的原因代码。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集。
        /// </returns>
        DataSet GetReasonCodeForReturnMaterial(string categoryName);
        /// <summary>
        /// 根据原因代码分组名称获取回收原因代码数据。
        /// </summary>
        /// <param name="categoryName">
        /// 原因代码分组名称,
        /// 如果为空则获取原因代码分组名称为RC_DEFAULT的回收原因代码。
        /// </param>
        /// <returns>
        /// 包含原因代码数据的数据集。
        /// </returns>
        DataSet GetReasonCodeForRecover(string categoryName);

    }
}
