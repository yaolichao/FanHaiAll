using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 产品料号数据管理接口。
    /// </summary>
    public interface IPartEngine
    {
        /// <summary>
        /// 新增产品料号数据。
        /// </summary>
        /// <param name="dsParams">包含产品料号数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet PartInsert(DataSet dataSet);
        /// <summary>
        /// 更新产品料号数据。
        /// </summary>
        /// <param name="dsParams">包含产品料号数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet PartUpdate(DataSet dataSet);
        /// <summary>
        /// 根据产品料号主键删除产品料号数据。
        /// </summary>
        /// <param name="partKey">产品料号主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet PartDelete(string partKey);
        /// <summary>
        /// 获取产品料号的自定义属性列数据。
        /// </summary>
        /// <returns>包含产品料号自定义属性列的数据集对象。</returns>
        DataSet GetAttributsColumnsForPart();
        /// <summary>
        /// 根据产品料号名查询产品料号数据。
        /// </summary>
        /// <param name="partName">产品料号名。</param>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        DataSet SearchPart(string partName);
        /// <summary>
        /// 根据产品料号主键获取产品料号数据。
        /// </summary>
        /// <param name="partKey">产品料号主键。</param>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        DataSet GetPartByKey(string partKey);
        /// <summary>
        /// 根据产品料号料号获取产品料号数据。
        /// </summary>
        /// <param name="partNumber">产品料号料号。</param>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        DataSet GetPartByPartNumber(string partNumber);
        /// <summary>
        /// 根据产品料号料号和工步名称获取产品料号数据。
        /// </summary>
        /// <remarks>
        /// 从POR_PART表和V_PROCESS_PLAN视图获取产品料号信息、途程、途程组、途程中的工步等表数据
        /// </remarks>
        /// <param name="partNumber">产品料号物料号</param>
        /// <param name="routStepName">工步名称,可以为空</param>
        /// <returns>
        /// 包含产品料号数据的数据集对象。
        /// [PART_KEY,PART_VERSION,PART_NAME,PART_STATUS,PART_TYPE,PART_MODULE,CUR_ENTERPRISE_VER_KEY,CUR_ROUTE_VER_KEY,
        ///  ROUTE_STEP_KEY,ENTERPRISE_NAME,ENTERPRISE_VERSION,ROUTE_STEP_NAME,ROUTE_NAME,Descriptions]
        /// </returns>
        DataSet GetPartByPartNumberAndStepName(string partNumber, string routStepName);

        //add by chao.pang 2013-12-04 start
        DataSet GetPartType();
        //add by chao.pang 2013-12-04 end
    }
}
