using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Interface.EquipmentManagement;
using FanHai.Hemera.Share.Interface.WarehouseManagement;
using FanHai.Hemera.Share.Interface.RFC;
using FanHai.Hemera.Share.Interface.SystemManagement;


namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// <see cref="IServerObjFactory"/>的扩展类。
    /// </summary>
    public static class ServerObjFactoryExtend
    {
        /// <summary>
        /// 获取SAP的工单信息的管理实例
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ISAPEngine CreateISAPEngine(this IServerObjFactory factory)
        {
            return factory.Get<ISAPEngine>();
        }
        /// <summary>
        /// 获取用户数据和操作员数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>用户数据和操作员数据的管理实例。</returns>
        public static IUserEngine CreateIUserEngine(this IServerObjFactory factory)
        {
            return factory.Get<IUserEngine>();
        }
        /// <summary>
        /// 获取抽检点数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>抽检点数据的管理实例。</returns>
        public static IEDCQuery CreateIEDCQuery(this IServerObjFactory factory)
        {
            return factory.Get<IEDCQuery>();
        }
        /// <summary>
        /// 获取退料数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>退料数据的管理实例。</returns>
        public static IReturnMaterialQueryEngine CreateIReturnMaterialQueryEngine(this IServerObjFactory factory)
        {
            return factory.Get<IReturnMaterialQueryEngine>();
        }
        /// <summary>
        /// 获取线上仓物料库存数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>线上仓物料库存数据的管理实例。</returns>
        public static IOnlineMaterialQueryEngine CreateIOnlineMaterialQueryEngine(this IServerObjFactory factory)
        {
            return factory.Get<IOnlineMaterialQueryEngine>();
        }
        /// <summary>
        /// 获取工单退料数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>工单退料数据的管理实例。</returns>
        public static IWOMaterialReturnEngine CreateIWOMaterialReturnEngine(this IServerObjFactory factory)
        {
            return factory.Get<IWOMaterialReturnEngine>();
        }
        /// <summary>
        /// 获取批次预设暂停数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>批次预设暂停数据的管理实例。</returns>
        public static ILotFutureHoldEngine CreateILotFutureHoldEngine(this IServerObjFactory factory)
        {
            return factory.Get<ILotFutureHoldEngine>();
        }
        /// <summary>
        /// 获取操作数据库数据的实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>操作数据库数据的实例。</returns>
        public static IDBEngine CreateIDBEngine(this IServerObjFactory factory)
        {
            return factory.Get<IDBEngine>();
        }
        /// <summary>
        /// 获取抽检点数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>抽检点数据的管理实例。</returns>
        public static ISampEngine CreateISampEngine(this IServerObjFactory factory)
        {
            return factory.Get<ISampEngine>();
        }
        /// <summary>
        /// 获取抽检参数数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>抽检参数数据的管理实例。</returns>
        public static IParamEngine CreateIParamEngine(this IServerObjFactory factory)
        {
            return factory.Get<IParamEngine>();
        }
        /// <summary>
        /// 获取原因代码数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>原因代码数据的管理实例。</returns>
        public static IReasonCodeEngine CreateIReasonCodeEngine(this IServerObjFactory factory)
        {
            return factory.Get<IReasonCodeEngine>();
        }
        /// <summary>
        /// 获取工艺流程数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>工艺流程数据的管理实例。</returns>
        public static IRouteEngine CreateIRouteEngine(this IServerObjFactory factory)
        {
            return factory.Get<IRouteEngine>();
        }
        /// <summary>
        /// 获取工序数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>工序数据的管理实例。</returns>
        public static IOperationEngine CreateIOperationEngine(this IServerObjFactory factory)
        {
            return factory.Get<IOperationEngine>();
        }
        /// <summary>
        /// 获取运算引擎实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>运算引擎实例。</returns>
        public static ICalculateEngine CreateICalculateEngine(this IServerObjFactory factory)
        {
            return factory.Get<ICalculateEngine>();
        }
        /// <summary>
        /// 获取用户自定义属性数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>用户自定义属性数据的管理实例。</returns>
        public static ICrmAttributeEngine CreateICrmAttributeEngine(this IServerObjFactory factory)
        {
            return factory.Get<ICrmAttributeEngine>();
        }
        /// <summary>
        /// 获取成品数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>成品数据的管理实例。</returns>
        public static IPartEngine CreateIPartEngine(this IServerObjFactory factory)
        {
            return factory.Get<IPartEngine>();
        }

        /// <summary>
        /// 获取设备状态数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>设备状态数据的管理实例。</returns>
        public static IEquipmentStates CreateIEquipmentStates(this IServerObjFactory factory)
        {
            return factory.Get<IEquipmentStates>();
        }
        /// <summary>
        /// 获取生产线数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>生产线数据的管理实例。</returns>
        public static ILineManageEngine CreateILineManageEngine(this IServerObjFactory factory)
        {
            return factory.Get<ILineManageEngine>();
        }
        /// <summary>
        /// 获取设备改变状态数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>设备改变状态数据的管理实例。</returns>
        public static IEquipmentChangeStates CreateIEquipmentChangeStates(this IServerObjFactory factory)
        {
            return factory.Get<IEquipmentChangeStates>();
        }
        /// <summary>
        /// 获取设备改变原因数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>设备改变原因数据的管理实例。</returns>
        public static IEquipmentChangeReasons CreateIEquipmentChangeReasons(this IServerObjFactory factory)
        {
            return factory.Get<IEquipmentChangeReasons>();
        }
        /// <summary>
        /// 获取设备状态事件数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>设备状态事件数据的管理实例。</returns>
        public static IEquipmentStateEvents CreateIEquipmentStateEventsEngine(this IServerObjFactory factory)
        {
            return factory.Get<IEquipmentStateEvents>();
        }
        /// <summary>
        /// 获取设备组数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>设备组数据的管理实例。</returns>
        public static IEquipmentGroups CreateIEquipmentGroups(this IServerObjFactory factory)
        {
            return factory.Get<IEquipmentGroups>();
        }
        /// <summary>
        /// 获取设备数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>设备数据的管理实例。</returns>
        public static IEquipments CreateIEquipments(this IServerObjFactory factory)
        {
            return factory.Get<IEquipments>();
        }
        /// <summary>
        /// 获取工序设备数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>工序设备数据的管理实例。</returns>
        public static IOperationEquipments CreateIOperationEquipments(this IServerObjFactory factory)
        {
            return factory.Get<IOperationEquipments>();
        }
        /// <summary>
        /// 获取工艺流程组数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>工艺流程组数据的管理实例。</returns>
        public static IEnterpriseEngine CreateIEnterpriseEngine(this IServerObjFactory factory)
        {
            return factory.Get<IEnterpriseEngine>();
        }
        /// <summary>
        /// 获取批次数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>批次数据的管理实例。</returns>
        public static ILotEngine CreateILotEngine(this IServerObjFactory factory)
        {
            return factory.Get<ILotEngine>();
        }
        /// <summary>
        /// 获取数据采集的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>数据采集的管理实例。</returns>
        public static IEDCEngine CreateIEDCEngine(this IServerObjFactory factory)
        {
            return factory.Get<IEDCEngine>();
        }
        /// <summary>
        /// 获取角色数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>角色数据的管理实例。</returns>
        public static IRoleEngine CreateIRoleEngine(this IServerObjFactory factory)
        {
            return factory.Get<IRoleEngine>();
        }
        /// <summary>
        /// 获取用户角色数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>用户角色数据的管理实例。</returns>
        public static IUserRoleEngine CreateIUserRoleEngine(this IServerObjFactory factory)
        {
            return factory.Get<IUserRoleEngine>();
        }
        /// <summary>
        /// 获取系统资源组数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>系统资源组数据的管理实例。</returns>
        public static IResourceGroupEngine CreateIResourceGroupEngine(this IServerObjFactory factory)
        {
            return factory.Get<IResourceGroupEngine>();
        }
        /// <summary>
        /// 获取系统资源数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>系统资源数据的管理实例。</returns>
        public static IResourceEngine CreateIResourceEngine(this IServerObjFactory factory)
        {
            return factory.Get<IResourceEngine>();
        }
        /// <summary>
        /// 获取系统操作组数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>系统操作组数据的管理实例。</returns>
        public static IOperationGroupEngine CreateIOperationGroupEngine(this IServerObjFactory factory)
        {
            return factory.Get<IOperationGroupEngine>();
        }
        /// <summary>
        /// 获取系统操作数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>系统操作数据的管理实例。</returns>
        public static IRBACOperationEngine CreateRBACOperationEngine(this IServerObjFactory factory)
        {
            return factory.Get<IRBACOperationEngine>();
        }
        /// <summary>
        /// 获取系统权限数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>系统权限数据的管理实例。</returns>
        public static IPrivilegeEngine CreateIPrivilegeEngine(this IServerObjFactory factory)
        {
            return factory.Get<IPrivilegeEngine>();
        }
        /// <summary>
        /// 获取区域数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>区域数据的管理实例。</returns>
        public static ILocation CreateILocationEngine(this IServerObjFactory factory)
        {
            return factory.Get<ILocation>();
        }
        /// <summary>
        /// 获取线上仓数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>线上仓数据的管理实例。</returns>
        public static IStoreEngine CreateIStoreEngine(this IServerObjFactory factory)
        {
            return factory.Get<IStoreEngine>();
        }
        /// <summary>
        /// 获取班别数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>班别数据的管理实例。</returns>
        public static IShift CreateIShift(this IServerObjFactory factory)
        {
            return factory.Get<IShift>();
        }
        /// <summary>
        /// 获取排班计划数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>排班计划数据的管理实例。</returns>
        public static ISchedule CreateISchedule(this IServerObjFactory factory)
        {
            return factory.Get<ISchedule>();
        }
        /// <summary>
        /// 获取工单数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>工单数据的管理实例。</returns>
        public static IWorkOrdersEngine CreateIWorkOrders(this IServerObjFactory factory)
        {
            return factory.Get<IWorkOrdersEngine>();
        }
        /// <summary>
        /// 获取在制品自动出站数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>在制品自动出站数据的管理实例。</returns>
        public static IWipJobAutoTrack CreateIWipJobAutoTrack(this IServerObjFactory factory)
        {
            return factory.Get<IWipJobAutoTrack>();
        }
        /// <summary>
        /// 获取物料数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>物料数据的管理实例。</returns>
        public static IGetMaterialsEngine CreateIMaterials(this IServerObjFactory factory)
        {
            return factory.Get<IGetMaterialsEngine>();
        }
        /// <summary>
        /// 获取批次创建数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>批次创建数据的管理实例。</returns>
        public static ILotCreateEngine CreateLotCreateEngine(this IServerObjFactory factory)
        {
            return factory.Get<ILotCreateEngine>();
        }
        /// <summary>
        /// 获取客户端配置数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>客户端配置数据的管理实例。</returns>
        public static IComputerEngine CreateIComputerEngine(this IServerObjFactory factory)
        {
            return factory.Get<IComputerEngine>();
        }
        /// <summary>
        /// 获取抽检点设置数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>抽检点设置数据的管理实例。</returns>
        public static IEDCPiont CreateIEDCPiont(this IServerObjFactory factory)
        {
            return factory.Get<IEDCPiont>();
        }
        /// <summary>
        /// 获取RFC函数调用的实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>RFC函数调用的实例。</returns>
        public static IRFCEngine CreateIRFCEngine(this IServerObjFactory factory)
        {
            return factory.Get<IRFCEngine>();
        }
        /// <summary>
        /// 获取用户自定义属性数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>用户自定义属性数据的管理实例。</returns>
        public static IUdaCommonControlEx CreateIUdaCommonControlEx(this IServerObjFactory factory)
        {
            return factory.Get<IUdaCommonControlEx>();
        }
        /// <summary>
        /// 获取SPC数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>SPC数据的管理实例。</returns>
        public static ISpcEngine CreateISpcEngine(this IServerObjFactory factory)
        {
            return factory.Get<ISpcEngine>();
        }
        /// <summary>
        /// 获取来料接收数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>来料接收数据的管理实例。</returns>
        public static IReceiveMaterialEngine CreatIReceiveMaterialEngine(this IServerObjFactory factory)
        {
            return factory.Get<IReceiveMaterialEngine>();
        }
        /// <summary>
        /// 获取物料耗用数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>物料耗用数据的管理实例。</returns>
        public static IUseMaterialEngine CreateIUseMaterialEngine(this IServerObjFactory factory)
        {
            return factory.Get<IUseMaterialEngine>();
        }
        /// <summary>
        /// 获取工序交接班数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>工序交接班数据的管理实例。</returns>
        public static IOperationHandoverEngine CreateIOperationHandoverEngine(this IServerObjFactory factory)
        {
            return factory.Get<IOperationHandoverEngine>();
        }
        /// <summary>
        /// 获取工序报工数据的管理实例。
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>工序报工数据的管理实例。</returns>
        public static IWorkOrderWorkForEngine CreateIWorkOrderWorkForEngine(this IServerObjFactory factory)
        {
            return factory.Get<IWorkOrderWorkForEngine>();
        }
        /// <summary>
        /// 获取产品型号及产品设置的接口实例
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>返回相关的调用数据。</returns>
        public static IProductModelEngine CreateIProductModelEngine(this IServerObjFactory factory)
        {
            return factory.Get<IProductModelEngine>();
        }
        /// <summary>
        /// 获取衰减相关数据的接口
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IDecayCoeffiEngine CreateIDecayCoeffiEngine(this IServerObjFactory factory)
        {
            return factory.Get<IDecayCoeffiEngine>();
        }
        /// <summary>
        /// 档位设置相关的接口
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IBasicPowerSetEngine CreateIBasicPowerSetEngine(this IServerObjFactory factory)
        {
            return factory.Get<IBasicPowerSetEngine>();
        }
        /// <summary>
        /// 测试规则相关的接口
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IBasicTestRuleEngine CreateIBasicTestRuleEngine(this IServerObjFactory factory)
        {
            return factory.Get<IBasicTestRuleEngine>();
        }
        /// <summary>
        ///产品设置的接口
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IPorProductEngine CreateIPorProductEngine(this IServerObjFactory factory)
        {
            return factory.Get<IPorProductEngine>();
        }
        /// <summary>
        ///获得并打印IV测试数据接口
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IPrintIvTestEngine CreateIPrintIvTestEngine(this IServerObjFactory factory)
        {
            return factory.Get<IPrintIvTestEngine>();
        }
        /// <summary>
        ///转工单数据接口
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IExchangeWoEngine CreateIExchangeWoEngine(this IServerObjFactory factory)
        {
            return factory.Get<IExchangeWoEngine>();
        }
        /// <summary>
        /// 通用报表接口
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IRptCommonEngine CreateIRptCommonEngine(this IServerObjFactory factory)
        {
            return factory.Get<IRptCommonEngine>();
        }
        /// <summary>
        ///CodeSoft标签设置接口
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IBasicCodeSoftLabelEngine CreateIBasicCodeSoftLabelEngine(this IServerObjFactory factory)
        {
            return factory.Get<IBasicCodeSoftLabelEngine>();
        }
        /// <summary>
        ///供应商管理
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ISupplierEngine CreateISupplierEngine(this IServerObjFactory factory)
        {
            return factory.Get<ISupplierEngine>();
        }
        /// <summary>
        ///物料维护
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IBomMaterialBandEngine CreateIBomMaterialBandEngine(this IServerObjFactory factory)
        {
            return factory.Get<IBomMaterialBandEngine>();
        }
        /// <summary>
        ///主副产品管理
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IByProductEngine CreateIByProductEngine(this IServerObjFactory factory)
        {
            return factory.Get<IByProductEngine>();
        }
        /// <summary>
        ///组柜管理
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IGroupArkEngine CreateIGroupArkEngine(this IServerObjFactory factory)
        {
            return factory.Get<IGroupArkEngine>();
        }

        /// <summary>
        ///拆柜管理
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ISplitArkEngine CreateISplitArkEngine(this IServerObjFactory factory)
        {
            return factory.Get<ISplitArkEngine>();
        }

        /// <summary>
        ///入库单生成
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IWarehouseEngine CreateIWarehouseEngine(this IServerObjFactory factory)
        {
            return factory.Get<IWarehouseEngine>();
        }

        /// <summary>
        ///半成品高位检
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IHighOfSFinishedProductsEngine CreateIHighOfSFinishedProductsEngine(this IServerObjFactory factory)
        {
            return factory.Get<IHighOfSFinishedProductsEngine>();
        }

        /// <summary>
        ///联副主副产品
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IByProductPartEngine CreateIByProductPartEngine(this IServerObjFactory factory)
        {
            return factory.Get<IByProductPartEngine>();
        }

        /// <summary>
        ///线别维护设置
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ILineSettingEngine CreateILineSettingEngine(this IServerObjFactory factory)
        {
            return factory.Get<ILineSettingEngine>();
        }
        /// <summary>
        ///原材料领料退料
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IMaterialReqOrReturnEngine CreateIMaterialReqOrReturnEngine(this IServerObjFactory factory)
        {
            return factory.Get<IMaterialReqOrReturnEngine>();
        }
        /// <summary>
        ///扣料基础数据
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IMaterialBuckleControlEngine CreateIMaterialBuckleControlEngine(this IServerObjFactory factory)
        {
            return factory.Get<IMaterialBuckleControlEngine>();
        }
        /// <summary>
        ///发料退料
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ISendingMaterialEngine CreateISendingMaterialEngine(this IServerObjFactory factory)
        {
            return factory.Get<ISendingMaterialEngine>();
        }
        /// <summary>
        ///特殊物料管控
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ISpecialMatTeamEngine CreateISpecialMatTeamEngine(this IServerObjFactory factory)
        {
            return factory.Get<ISpecialMatTeamEngine>();
        }
        /// <summary>
        /// 组件不良跟踪
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ILotExceptionEngine CreateILotExceptionEngine(this IServerObjFactory factory)
        { 
            return factory.Get<ILotExceptionEngine>();
        }
        /// <summary>
        ///铭牌打印
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static INameplateLabelPrintEngine CreateINameplateLabelPrintEngine(this IServerObjFactory factory)
        {
            return factory.Get<INameplateLabelPrintEngine>();
        }
        /// <summary>
        /// 铭牌自动打印
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static INameplateLabelAutoPrintEngine CreateINameplateLabelAutoPrintEngine(this IServerObjFactory factory)
        {
            return factory.Get<INameplateLabelAutoPrintEngine>();
        }
        /// <summary>
        ///获取未打印的组件序列号
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ILotNumPrintEngine CreateILotNumPrintEngine(this IServerObjFactory factory)
        {
            return factory.Get<ILotNumPrintEngine>();
        }

        /// <summary>
        /// 获取虚拟托盘管理
        /// </summary>
        /// <param name="factory"><see cref="IServerObjFactory"/></param>
        /// <returns>虚拟托盘管理实例。</returns>
        public static ILotComponentTrayEngine CreateILotComponentTrayEngine(this IServerObjFactory factory)
        {
            return factory.Get<ILotComponentTrayEngine>();
        }
    }
}
