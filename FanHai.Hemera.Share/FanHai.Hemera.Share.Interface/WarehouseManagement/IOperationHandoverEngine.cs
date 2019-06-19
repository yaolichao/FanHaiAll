using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace FanHai.Hemera.Share.Interface.WarehouseManagement
{
    /// <summary>
    /// 工序交接班数据的操作接口。
    /// </summary>
    public interface IOperationHandoverEngine
    {
        /// <summary>
        /// 通过用户拥有权限的工序和线边仓的工厂车间获取工序交接班记录
        /// </summary>
        /// <param name="operations">工序</param>
        /// <param name="stores">线边仓</param>
        /// <returns></returns>
        DataSet GetOperationHandoverBySAndF(string operations,DataTable dt);
        /// <summary>
        /// 查询界面返回参数
        /// </summary>
        /// <param name="_lupFactoryRoom">工厂车间</param>
        /// <param name="_cmbGongXuName">工序名称</param>
        /// <param name="_lupJiaoBanShife">交接班次</param>
        /// <param name="_lupJieBanShife">接班班次</param>
        /// <param name="_timJiaoBanStart">交接时间起</param>
        /// <param name="_timJiaoBanEnd">交接时间末</param>
        /// <param name="_lupZhuangTai">状态</param>
        /// <param name="operations">用户拥有权限的工序</param>
        /// <param name="dt">工厂车间</param>
        /// <returns></returns>
        DataSet GetOperationHandoverByReturn(string _lupFactoryRoom, string _cmbGongXuName, string _lupJiaoBanShife, 
            string _lupJieBanShife, string _timJiaoBanStart, string _timJiaoBanEnd, string _lupZhuangTai, string operations, DataTable dt);

        /// <summary>
        /// 根据当前班次和当前日期获取上一班次和上一班的交班日期。根据上一班次、工厂车间、工序名称、上一班的交班日期获取上一班的交班记录
        /// </summary>
        /// <param name="_lupShift">当前班次</param>
        /// <param name="_lupGongXu">工序名称</param>
        /// <param name="_lupFacRoomKey">工厂车间主键</param>
        /// <returns></returns>
        DataSet GetShangBanShift(string _lupShift, string _lupGongXu, string _lupFacRoomKey);
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="_lupGongXu">工序</param>
        /// <param name="_lupFacRoomKey">工厂车间主键</param>
        /// <returns></returns>
        DataSet GetShangBan(string _lupGongXu, string _lupFacRoomKey);
        /// <summary>
        /// 通过工序交接主键获取物料信息和在制品信息
        /// </summary>
        /// <param name="key">工序交接班主键</param>
        /// <returns>物料信息表在制品信息表</returns>
        DataSet GetWipAndMatByKey(string key);
        /// <summary>
        /// 获取当前班别的工序交接班信息
        /// </summary>
        /// <param name="shift">班次</param>
        /// <param name="operation">工序名称</param>
        /// <param name="factRoom">工厂车间主键</param>
        /// <returns></returns>
        DataSet GetDangQianShiftHandover(string shift, string operation, string factRoom);
        /// <summary>
        /// 没有记录在工序交接班中插入一条记录
        /// </summary>
        /// <param name="dsSetIn"></param>
        /// <returns></returns>
        DataSet InsertHandOver(DataSet dsSetIn);
        /// <summary>
        ///通过根据工序和工厂车间获取所有线上仓中的物料信息（WST_STORE,WST_STORE_MATERIAL)插入到WST_OPERATION_HANDOVER_MAT中
        ///（数量全部设置为0）。根据工序和工厂车间获取所有工单的在制品信息（POR_LOT,POR_WORK_ORDER,WIP_TRANSACTION)插入到
        ///WST_OPERATION_HANDOVER_WIP中（数量全部设置为0）
        /// </summary>
        /// <param name="handOverKey">工序交接班主键</param>
        /// <param name="actRoom">工厂车间</param>
        /// <param name="operation">工序名称</param>
        DataSet InsertHandOverMatAndWip(string handOverKey, string actRoom, string operation);
        /// <summary>
        /// 通过工序交接班的主键然后获取WIP和物料的数量更新到表中
        /// </summary>
        /// <param name="handOverKey">工序交接班主键</param>
        DataSet UpdateHandOverMatAndWip(string handOverKey);
        /// <summary>
        /// UpdateHandOverMatAndWipQiMoShuLiang通过工序交接班的主键更新期末数据
        /// </summary>
        /// <param name="handOverKey"></param>
        /// <returns></returns>
        DataSet UpdateHandOverMatAndWipQiMoShuLiang(string handOverKey);
        /// <summary>
        /// 获取上一班的交接记录
        /// </summary>
        /// <param name="shift">当前班次</param>
        /// <param name="operation">工序名称</param>
        /// <param name="factRoom">工厂主键</param>
        /// <returns></returns>
        DataSet GetShangYiBanHandOver(string shift, string operation, string factRoom);
        /// <summary>
        /// 交班后更新工序交接班内容和MAT内容
        /// </summary>
        /// <param name="dsSetIn">Hash表和界面MAT数据信息</param>
        DataSet UpdateWipMatHandOverBySaveJiaoban(DataSet dsSetIn);
        /// <summary>
        /// 交班后更新工序交接班内容
        /// </summary>
        /// <param name="dsSetIn1"></param>
        DataSet UpdateHandOver(DataSet dsSetIn);
        /// <summary>
        /// 根据上一工序交接班主键获取上一工序交接班的期末数量插入到新生成的数据中的期初数量
        /// </summary>
        /// <param name="handDangqianOverKey">当前工序交接班主键</param>
        /// <param name="handOverKey">上一工序交接班主键</param>
        DataSet InsertMatWipQiChu(string handDangqianOverKey, string handOverKey);

        DataSet GetFacKeyByFacName(string facName);//根据工厂名称获取主键
    }
}
