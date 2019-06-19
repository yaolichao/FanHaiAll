using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using FanHai.Hemera.Utils.Dialogs;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData.Gui
{
    public partial class WorkOrderProductSetting : BaseUserCtrl
    {

        WorkOrderEntity _entity = new WorkOrderEntity();
        WorkOrders _workOrdersEntity = new WorkOrders();
        PorProductEntity _porProductEntity = new PorProductEntity();
        BasePowerSetEntity _basePowerSetEntity = new BasePowerSetEntity();
        BaseTestRuleEntity _baseTestRuleEntity = new BaseTestRuleEntity();

        DataTable _dtRuleProperties = null;
        //DataTable _dtPorProduct_dtl = null;
        DataTable _dtWorkOrderAttr = null;
        //DataTable _dtlueWo = null;
        //DataTable _dtlueWoNotExistProid = null;
        DataTable _dtWorkOrderNumber = null;
        DataTable _dtWorkOrderBom = null;
        DataTable _dtPartNumber = null;
        DataTable _dtProList = null;                 //产品列表
        DataTable _dtPorProduct = null;              //工单产品
        DataTable _dtProLevel = null;                //产品等级
        DataTable _dtProDecay = null;                //产品衰减
        DataTable _dtProPrintSet = null;             //产品打印设置
        DataTable _dtProPS = null;                   //功率分档
        DataTable _dtProPSColor = null;              //功率分档花色
        DataTable _dtProPSSub = null;                //功率分档明细
        DataTable _dtWorkOrderOEM = null;            //OEM信息录入
        DataTable _dtWorkOrderRoute = null;          //工单对应工序信息
        DataTable _dtWorkOrderLine = null;           //工单对应线别信息
        DataTable _dtUpAndLow = null;                //功率上下线卡控
        DataTable _dtFactoryLineList = null;         //工厂先别清单
        DataTable _dtProPShow = null;                //标称功率实际值与标签和清单显示的值的关系fyb
        DataTable _dtProListBind = null;             //产品属性信息
        DataTable partNumberBind = null;             //产品描述
        DataTable _dtPorWoElTestRule = null;

        //bool isFirstLoad = true;
        private new AfterStateChanged afterStateChanged = null;
        private ControlState _controlState = ControlState.Empty;
        private new delegate void AfterStateChanged(ControlState controlState);

        string workOrderKey = string.Empty;
        string workOrderNumber = string.Empty;
        string proKey = string.Empty;
        string partNumber = string.Empty;
        string proPowerSetKey = string.Empty;
        string testRuleKey = string.Empty;
        string partGrades = string.Empty;
        string factoryCode = string.Empty;
        string workOrderJunctionBox = string.Empty;
        string printRuleCode = string.Empty;
        bool isAutoFillJunctionBox = false;



        //
        string proRuleCode = string.Empty;
        public WorkOrderProductSetting()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            lblMenu.Text = "基础数据 > 工单管理 > 工单产品属性设置";
            GridViewHelper.SetGridView(gvBOM);
            GridViewHelper.SetGridView(gvDecay);
            GridViewHelper.SetGridView(gvEffCtm);
            GridViewHelper.SetGridView(gvOrderProperties);
            GridViewHelper.SetGridView(gvPowerSet);
            GridViewHelper.SetGridView(gvPowerSetColor);
            GridViewHelper.SetGridView(gvPowerSetSub);
            //GridViewHelper.SetGridView(gvProduct);
            GridViewHelper.SetGridView(gvProLevel);
            GridViewHelper.SetGridView(gvProPrintSet);
            GridViewHelper.SetGridView(gvWorkOrderLine);
            GridViewHelper.SetGridView(gvPackPrint);
            GridViewHelper.SetGridView(gvPowerShow);
            GridViewHelper.SetGridView(gvUPLOWRule);
            GridViewHelper.SetGridView(gvRuleDetail);
            //GridViewHelper.SetGridView(gvWorkOrder);

            this.tsbtnSave.Text = StringParser.Parse("${res:Global.Save}");//保存
            this.tsbtnEdit.Text = StringParser.Parse("${res:Global.Update}");//修改
            this.tsbtnCancel.Text = StringParser.Parse("${res:Global.Cancel}");//取消
            this.tsbtnQuery.Text = StringParser.Parse("${res:Global.Query}");//查询

            xtabBaseInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0002}");//基本信息
            lciOrderNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0003}");//工单号
            lciPartNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0004}");//料号
            lciDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0005}");//描述
            lciPlanQty.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0006}");//工单计划数量
            lciState.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0007}");//工单状态
            lciRevenue.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0008}");//保税手册号
            lciStartTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0009}");//工单开始时间
            lciFinishTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0010}");//工单结束时间
            lciOrderType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0011}");//工单类型
            lciComment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0012}");//备注
            lblEnterName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0013}");//工艺流程组
            lblRoutName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0014}");//工艺流程
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0015}");//首工序
            gcolIndex.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0001}");//序号
            gcolMaterialCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0002}");//物料编码
            gcolDescription.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0003}");//物料描述
            gcolQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0004}");//数量
            gcolUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0005}");//单位

            xtabRuleInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0016}");//规则信息
            sbtnProductAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0017}");//新增
            sbtnProductDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0018}");//删除
            gdRowNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0006}");//序号
            gdProType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0007}");//主副产品
            gdPartNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0008}");//产品料号
            gdProCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0009}");//产品代码
            gdMaxPower.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0010}");//最大功率
            gdMinPower.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0011}");//最小功率
            gdPartDesc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0012}");//产品描述
            gdProName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0013}");//产品名称
            gdTestRule.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0014}");//测试规则
            gdLableType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0015}");//认证类型
            gcLabelVar.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0016}");//认证版本
            gdTolerance.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0017}");//分档方式
            gdCustMark.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0018}");//客户标记
            gdLableCheck.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0019}");//校验铭牌
            gdCustCheckType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0020}");//验证方式

            xtabProBaseInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0019}");//基本信息
            lciLastTestType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0020}");//终检类型
            lciPowerDegree.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0021}");//功率精度
            lciFullPalletQTY.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0022}");//满托数量
            lciFullShipment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0023}");//满柜数量
            lciCalibrationCycle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0024}");//校准周期(分钟)
            lciFix_Cycle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0025}");//固化周期(分钟)
            lciJunctionBox.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0026}");//接线盒批号
            lciCalibrationType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0027}");//校准版类型
            lciModuleTypePrefix.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0028}");//类型前缀
            lciModuleTypeSuffix.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0029}");//类型后缀
            lciConstantTemperatureCycle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0030}");//恒温周期

            xtabPowerShift.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0031}");//功率分档
            sbtnPowerSetAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0032}");//新增
            sbtnPowerSetDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0033}");//删除
            gdRowNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0021}");//序号
            gdModuleName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0022}");//档位名称
            gdNeedNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0023}");//需求数量
            gdPMax.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0024}");//功率上限
            gdPMin.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0025}");//功率下限
            gdPSCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0026}");//分档代码
            gdPSSubCodeDesc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0027}");//分档编码描述
            gdSubPSWay.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0028}");//子分档方式
            gdPowerDifference.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0029}");//功率差
            gdPS_SUBCODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0030}");//分档编码
            xtabPowerSetDTL.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0034}");//子分档
            sbtnPwoerSetSubAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0035}");//新增
            sbtnPowerSetSubDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0036}");//删除
            RowNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0031}");//序号
            gdPowerLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0032}");//档位
            gdPDTLMax.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0033}");//最大值
            gdPDTLMin.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0034}");//最小值
            xtabPowerShiftColor.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0037}");//花色
            sbtnPowerSetColorAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0038}");//新增
            sbtnPowerSetColorDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0039}");//删除
            gdColorCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0035}");//编码
            gdColorName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0036}");//花色
            gdDescription.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0037}");//备注

            xtabProLevel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0040}");//产品等级
            sbtnProLevelAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0041}");//新增
            sbtnProLevelDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0042}");//删除
            gdProLevelRowNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0038}");//序号
            gdLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0039}");//等级
            gdMainLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0040}");//混档(主分档)
            gdMainLevelDetail.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0041}");//混档(子分档)
            gdMixColor.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0042}");//混花
            gdPackage.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0043}");//包装组

            xtabDecay.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0043}");//衰减设置
            sbtnDecayAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0044}");//新增
            sbtnDecayDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0045}");//删除
            gdDecayRowNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0044}");//序号
            gdDecayNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0045}");//衰减系数
            gdPowerMin.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0046}");//功率下限
            gdPowerMax.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0047}");//功率上限

            xtabLablePrintSet.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0046}");//打印标签设置
            sbtnLablePrintAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0047}");//新增
            sbtnLablePrintDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0048}");//删除
            gdViewName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0048}");//打印标签
            gdNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0049}");//打印数量
            gdISLABEL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0050}");//铭牌
            gdISMAIN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0051}");//主标签
            gdPackagePrint.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0052}");//包装打印
            gdLableDecayNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0053}");//衰减系数
            gdPrintDecayMinMax.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0054}");//衰减区间

            xtrabLabelNameplate.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0049}");//标签/铭牌/清单体现功率设置
            spbtnAddPower.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0050}");//新增
            spbtnDelPower.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0051}");//删除
            gridColumn27.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0055}");//工单号
            gridColumn29.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0056}");//规则代码
            gridColumn30.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0057}");//原始档位
            gridColumn31.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0058}");//体现档位(标签、铭牌、包装清单)

            xtabProperties.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0052}");//属性
            sbtnOrderPropertiesAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0053}");//添加工单属性
            sbtnMaterialsPropertiesAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0054}");//添加物料属性
            sbtnOrderPropertiesDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0055}");//删除属性
            gdPropertiesName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0059}");//属性名称
            gdPropertiesValue.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0060}");//属性值
            gdPropertiesType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0061}");//属性类别

            xtabLineBind.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0056}");//线别绑定
            sbtnLineAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0057}");//新增
            sbtnLineDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0058}");//删除
            gdLineWorkOrderNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0062}");//工单号
            gdLineKey.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0063}");//线别
            gdLineFactoryName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0064}");//工厂名称
            gdLineCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0065}");//线别代码

            xtabOEMInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0059}");//OEM信息
            ceIsOEMWorkOrder.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0060}");//OEM工单
            lycgOEMParam.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0061}");//OEM参数设定
            lciCustomer.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0062}");//客户
            lciCellsupplier.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0063}");//电池片供应商
            lciCellType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0064}");//电池片类型
            lciCellModel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0065}");//电池片型号
            lciStructureParam.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0066}");//结构及安装参数
            lciPlaceOrigin.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0067}");//产地
            lciGlassType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0068}");//玻璃类型
            lciJunctionBoxOEM.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0069}");//接线盒信息
            lciBOMAuthenticationCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0070}");//BOM认证代码
            lciBOMDesign.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0071}");//BOM及设计信息
            lyciSEMoudleType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0072}");//产品型号

            xtraTabPage1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0073}");//功率卡控设定
            sbtUPLowRuleAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0074}");//新增
            sbtUPLowRuleRemove.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0075}");//删除
            gcWorkOrder.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0066}");//工单号
            gcHigh.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0067}");//功率卡控上限
            gcLow.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0068}");//功率卡控下限

            xtraTabPage2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0076}");//CTM上下限设定
            layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0077}");//初始效率值
            layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0078}");//自增长幅度
            layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0079}");//行数
            sbtAddCtm.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0080}");//自动生成行记录
            simpleButton1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.btn.0001}");//删除
            smpCtmDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.btn.0002}");//清空
            gcChose.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0071}");//选择
            gcEffUp.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0072}");//效率档设定上限
            gcEffLow.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0073}");//效率档设定下限
            gcCtmUp.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0074}");//CTM上限
            gcCtmLow.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0075}");//CTM下限

            xtraTabPage3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0081}");//打印规则设定
            layoutControlGroup5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0082}");//打印规则
            layoutControlItem19.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0083}");//打印规则代码
            gcPrintCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0076}");//规则代码
            gcPrintCloumnName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0077}");//打印规则参数列
            gcPrintCloumnValue.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0078}");//打印规则列值
            layoutControlGroup6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0084}");//条码生成规则
            layoutControlGroup7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0085}");//条码模板

            xtraTabPage4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0086}");//包装清单打印设置
            groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0087}");//清单打印设置
            layoutControlItem18.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0088}");//工单号：
            sbtnUpDataPrint.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.lbl.0089}");//更新打印清单设置
            gridColumn24.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0079}");//产品料号
            gridColumn25.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0080}");//打印模板
            gridColumn26.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.GridControl.0081}");//打印份数

        }

        private void WorkOrderProductSetting_Load(object sender, EventArgs e)
        {
            this.afterStateChanged += new AfterStateChanged(OnAfterStateChanged);
            this.gvProduct.CellValueChanged += new CellValueChangedEventHandler(gvProduct_CellValueChanged);
            lu_Print_Types.EditValueChanged += new EventHandler(lu_Print_Types_EditValueChanged);



            this.State = ControlState.ReadOnly;
            this.tsbtnEdit.Enabled = false;
            //获取所有的工单列表信息
            GetAllWorkList();

            //获取对应工序信息
            InitControlValue();

            //获取工单属性
            GetWorkOrderAttrData("");
            //获取工单产品信息
            GetWOProData("");
            //选择控件内容加载
            BindAttributeData();
            //对选择控件数据源进行绑定
            BindParamData();
            //检验类型绑定
            BindCustCheckTypeData();
            //是否自动填充接线盒
            OrderMaterialFill();
            //绑定清单打印
            BindPrintRule();
            BindELTestRule();
        }

        /// <summary>
        /// 获取所有的工单列表信息
        /// </summary>
        void GetAllWorkList()
        {
            Hashtable hstable = new Hashtable();
            //if (!string.IsNullOrEmpty(txtWorkOrder.Text.Trim()))
            //    hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = txtWorkOrder.Text.Trim();
            //if (!string.IsNullOrEmpty(txtPro_id.Text.Trim()))
            //    hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE] = txtPro_id.Text.Trim();
            DataSet dsReturn = _workOrdersEntity.GetWorkOrderByNoOrProid(hstable);

            DataTable dtWorkOrder = dsReturn.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            //gcWorkOrderList.DataSource = dtWorkOrder;
        }

        void lu_Print_Types_EditValueChanged(object sender, EventArgs e)
        {
            int i = gvPackPrint.FocusedRowHandle;

            string types = lu_Print_Types.GetCheckedItems().ToString();
            if (!string.IsNullOrEmpty(types))
            {
                StringBuilder sbCopys = new StringBuilder();

                int count = types.Split(',').Count();
                DataTable dt = gcPackPrint.DataSource as DataTable;
                for (int j = 0; j < count; j++)
                {

                    sbCopys.Append("1");
                    if (j != count - 1)
                    {
                        sbCopys.Append(",");
                    }
                }
                DataRow dr = dt.Rows[i];
                dr["PRINT_COPY"] = sbCopys;
                gcPackPrint.DataSource = dt;
            }
        }
        /// <summary>
        /// 绑定打印规则代码
        /// </summary>
        private void BindPrintRule()
        {
            string[] l_s = new string[] { "PRINT_CODE", "PRINT_NAME", "PRINT_DESC", "PRINT_RESOUCE" };
            string category = "BASE_PRINT_CODE";
            DataTable dtCommon = BaseData.Get(l_s, category);
            DataRow dr = dtCommon.NewRow();
            dr["PRINT_CODE"] = "";
            dr["PRINT_NAME"] = "";
            dr["PRINT_DESC"] = "";
            dr["PRINT_RESOUCE"] = "";
            dtCommon.Rows.Add(dr);
            DataView dview = dtCommon.DefaultView;
            dview.Sort = "PRINT_CODE asc";

            lupPrintRule.Properties.DisplayMember = "PRINT_CODE";
            lupPrintRule.Properties.ValueMember = "PRINT_CODE";
            lupPrintRule.Properties.DataSource = dview.Table;
        }

        /// <summary>
        /// 绑定AI测试规则
        /// </summary>
        private void BindELTestRule()
        {
            string[] l_s = new string[] { "RULE_DES", "RULE_NAME", "RULE_ID", "RULE_TYPE" };
            string category = "WIP_ELTEST_RULE";
            DataTable dtCommon = BaseData.Get(l_s, category);
            DataRow dr = dtCommon.NewRow();
            dr["RULE_TYPE"] = "";
            dr["RULE_DES"] = "";
            dr["RULE_NAME"] = "";
            dr["RULE_ID"] = "";
            dtCommon.Rows.Add(dr);
            DataView dview = dtCommon.DefaultView;
            dview.Sort = "RULE_NAME asc";
            DataTable befLamELRule = new DataTable();
            befLamELRule.Columns.Add("RULE_TYPE", Type.GetType("System.String"));
            befLamELRule.Columns.Add("RULE_DES", Type.GetType("System.String"));
            befLamELRule.Columns.Add("RULE_NAME", Type.GetType("System.String"));
            befLamELRule.Columns.Add("RULE_ID", Type.GetType("System.String"));

            DataTable aftLamELRule = new DataTable();
            aftLamELRule.Columns.Add("RULE_TYPE", Type.GetType("System.String"));
            aftLamELRule.Columns.Add("RULE_DES", Type.GetType("System.String"));
            aftLamELRule.Columns.Add("RULE_NAME", Type.GetType("System.String"));
            aftLamELRule.Columns.Add("RULE_ID", Type.GetType("System.String"));

            for (int i = 0; i < dview.Count; i++)
            {
                if (!string.IsNullOrEmpty(dview.Table.Rows[i]["RULE_TYPE"].ToString()) && dview.Table.Rows[i]["RULE_TYPE"].ToString() == "befLam")
                {
                    DataRow befRow = befLamELRule.NewRow();
                    befRow["RULE_TYPE"] = dview.Table.Rows[i]["RULE_TYPE"].ToString();
                    befRow["RULE_DES"] = dview.Table.Rows[i]["RULE_DES"].ToString();
                    befRow["RULE_NAME"] = dview.Table.Rows[i]["RULE_NAME"].ToString();
                    befRow["RULE_ID"] = dview.Table.Rows[i]["RULE_ID"].ToString();
                    befLamELRule.Rows.Add(befRow);
                }
                else if (!string.IsNullOrEmpty(dview.Table.Rows[i]["RULE_TYPE"].ToString()) && dview.Table.Rows[i]["RULE_TYPE"].ToString() == "aftLam")
                {
                    DataRow aftRow = aftLamELRule.NewRow();
                    aftRow["RULE_TYPE"] = dview.Table.Rows[i]["RULE_TYPE"].ToString();
                    aftRow["RULE_DES"] = dview.Table.Rows[i]["RULE_DES"].ToString();
                    aftRow["RULE_NAME"] = dview.Table.Rows[i]["RULE_NAME"].ToString();
                    aftRow["RULE_ID"] = dview.Table.Rows[i]["RULE_ID"].ToString();
                    aftLamELRule.Rows.Add(aftRow);
                }
            }

            lstBefLamRule.Properties.Columns.Clear();
            lstBefLamRule.Properties.DisplayMember = "RULE_NAME";
            lstBefLamRule.Properties.ValueMember = "RULE_ID";
            lstBefLamRule.Properties.DataSource = befLamELRule;
            lstBefLamRule.ItemIndex = 0;

            lstAfterLamRule.Properties.Columns.Clear();
            lstAfterLamRule.Properties.DisplayMember = "RULE_NAME";
            lstAfterLamRule.Properties.ValueMember = "RULE_ID";
            lstAfterLamRule.Properties.DataSource = aftLamELRule;
            lstAfterLamRule.ItemIndex = 0;
        }

        /// <summary>
        /// 默认工单工序绑定
        /// </summary>
        private void InitControlValue()
        {
            RouteQueryEntity routeQueryEntity = new RouteQueryEntity();
            DataSet dsRouteFirstOperation = routeQueryEntity.GetProcessPlanFirstOperation("", string.Empty, false);
            //有获取到首工序工艺流程。
            if (string.IsNullOrEmpty(routeQueryEntity.ErrorMsg)
                && null != dsRouteFirstOperation
                && dsRouteFirstOperation.Tables.Count > 0
                && dsRouteFirstOperation.Tables[0].Rows.Count > 0)
            {
                DataRow drRouteFirstOperation = dsRouteFirstOperation.Tables[0].Rows[0];
                this.beRouteEnterprise.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                this.beRouteEnterprise.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                this.teRouteName.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                this.teRouteName.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                this.teStepName.Tag = Convert.ToString(drRouteFirstOperation[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                this.teStepName.Text = Convert.ToString(drRouteFirstOperation[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            }
            else
            {
                this.beRouteEnterprise.Tag = string.Empty;
                this.beRouteEnterprise.Text = string.Empty;
                this.teRouteName.Tag = string.Empty;
                this.teRouteName.Text = string.Empty;
                this.teStepName.Tag = string.Empty;
                this.teStepName.Text = string.Empty;
            }
        }

        private new ControlState State
        {
            get
            {
                return _controlState;
            }
            set
            {
                _controlState = value;

                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Deal with state change event
        /// </summary>
        /// <param name="state"></param>
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {

                case ControlState.ReadOnly:
                    this.sbtnProductAdd.Enabled = false;
                    this.sbtnProductDelete.Enabled = false;
                    this.sbtnPowerSetAdd.Enabled = false;
                    this.spbtnAddPower.Enabled = false;
                    this.spbtnDelPower.Enabled = false;
                    this.sbtnPowerSetDelete.Enabled = false;
                    this.sbtnPwoerSetSubAdd.Enabled = false;
                    this.sbtnPowerSetSubDelete.Enabled = false;
                    this.sbtnPowerSetColorAdd.Enabled = false;
                    this.sbtnPowerSetColorDelete.Enabled = false;
                    this.sbtnProLevelAdd.Enabled = false;
                    this.sbtnProLevelDelete.Enabled = false;
                    this.sbtnDecayAdd.Enabled = false;
                    this.sbtnDecayDelete.Enabled = false;
                    this.sbtnLablePrintAdd.Enabled = false;
                    this.sbtnLablePrintDelete.Enabled = false;
                    this.sbtnOrderPropertiesAdd.Enabled = false;
                    this.sbtnOrderPropertiesDelete.Enabled = false;
                    this.sbtnMaterialsPropertiesAdd.Enabled = false;
                    this.sbtnLineAdd.Enabled = false;
                    this.sbtnLineDelete.Enabled = false;
                    this.tsbtnEdit.Enabled = true;
                    this.tsbtnCancel.Enabled = false;
                    this.tsbtnSave.Enabled = false;
                    this.gvBOM.OptionsBehavior.ReadOnly = true;
                    this.gvProduct.OptionsBehavior.ReadOnly = true;
                    this.gvPowerSet.OptionsBehavior.ReadOnly = true;
                    this.gvPowerSetColor.OptionsBehavior.ReadOnly = true;
                    this.gvPowerSetSub.OptionsBehavior.ReadOnly = true;
                    this.gvProLevel.OptionsBehavior.ReadOnly = true;
                    this.gvDecay.OptionsBehavior.ReadOnly = true;
                    this.gvProPrintSet.OptionsBehavior.ReadOnly = true;
                    this.gvOrderProperties.OptionsBehavior.ReadOnly = true;
                    this.gvWorkOrderLine.OptionsBehavior.ReadOnly = true;
                    this.lycgProSet.Enabled = false;
                    this.ceIsOEMWorkOrder.Enabled = false;
                    this.cbeCustomer.Enabled = false;
                    this.cbeCellSupplier.Enabled = false;
                    this.cbeCellType.Enabled = false;
                    this.cbeCellModel.Enabled = false;
                    this.cbeStructureParam.Enabled = false;
                    this.cbePlaceOrigin.Enabled = false;
                    this.cbeGlassType.Enabled = false;
                    this.cbeJunctionBoxOEM.Enabled = false;
                    this.cbeBOMAuthenticationCode.Enabled = false;
                    this.cbeBOMDesign.Enabled = false;
                    cbeSEMoudleType.Enabled = false;
                    this.teStepName.Enabled = false;
                    this.sbtUPLowRuleAdd.Enabled = false;
                    this.sbtUPLowRuleRemove.Enabled = false;
                    this.gvUPLOWRule.OptionsBehavior.ReadOnly = true;
                    this.sbtAddCtm.Enabled = false;
                    this.simpleButton1.Enabled = false;
                    this.smpCtmDelete.Enabled = false;
                    this.lupPrintRule.Enabled = false;
                    this.gvRuleDetail.OptionsBehavior.ReadOnly = true;
                    this.gvPackPrint.OptionsBehavior.ReadOnly = true;
                    this.sbtnUpDataPrint.Enabled = false;
                    break;
                case ControlState.Edit:
                    this.sbtnProductAdd.Enabled = true;
                    this.sbtnProductDelete.Enabled = true;
                    this.sbtnPowerSetAdd.Enabled = true;
                    this.spbtnAddPower.Enabled = true;
                    this.spbtnDelPower.Enabled = true;
                    this.sbtnPowerSetDelete.Enabled = true;
                    this.sbtnPwoerSetSubAdd.Enabled = true;
                    this.sbtnPowerSetSubDelete.Enabled = true;
                    this.sbtnPowerSetColorAdd.Enabled = true;
                    this.sbtnPowerSetColorDelete.Enabled = true;
                    this.sbtnProLevelAdd.Enabled = true;
                    this.sbtnProLevelDelete.Enabled = true;
                    this.sbtnDecayAdd.Enabled = true;
                    this.sbtnDecayDelete.Enabled = true;
                    this.sbtnLablePrintAdd.Enabled = true;
                    this.sbtnLablePrintDelete.Enabled = true;
                    this.sbtnOrderPropertiesAdd.Enabled = true;
                    this.sbtnOrderPropertiesDelete.Enabled = true;
                    this.sbtnMaterialsPropertiesAdd.Enabled = true;
                    this.sbtnLineAdd.Enabled = true;
                    this.sbtnLineDelete.Enabled = true;
                    this.tsbtnCancel.Enabled = true;
                    this.tsbtnSave.Enabled = true;
                    this.gvBOM.OptionsBehavior.ReadOnly = false;
                    this.gvProduct.OptionsBehavior.ReadOnly = false;
                    this.gvPowerSet.OptionsBehavior.ReadOnly = false;
                    this.gvPowerSetColor.OptionsBehavior.ReadOnly = false;
                    this.gvProLevel.OptionsBehavior.ReadOnly = false;
                    this.gvDecay.OptionsBehavior.ReadOnly = false;
                    this.gvProPrintSet.OptionsBehavior.ReadOnly = false;
                    this.gvOrderProperties.OptionsBehavior.ReadOnly = false;
                    this.gvWorkOrderLine.OptionsBehavior.ReadOnly = false;
                    this.lycgProSet.Enabled = true;
                    this.ceIsOEMWorkOrder.Enabled = true;
                    this.cbeCustomer.Enabled = true;
                    this.cbeCellSupplier.Enabled = true;
                    this.cbeCellType.Enabled = true;
                    this.cbeCellModel.Enabled = true;
                    this.cbeStructureParam.Enabled = true;
                    this.cbePlaceOrigin.Enabled = true;
                    this.cbeGlassType.Enabled = true;
                    this.cbeJunctionBoxOEM.Enabled = true;
                    this.cbeBOMAuthenticationCode.Enabled = true;
                    this.cbeBOMDesign.Enabled = true;
                    cbeSEMoudleType.Enabled = true;
                    this.teStepName.Enabled = true;
                    this.sbtUPLowRuleAdd.Enabled = true;
                    this.sbtUPLowRuleRemove.Enabled = true;
                    this.gvUPLOWRule.OptionsBehavior.ReadOnly = false;
                    this.sbtAddCtm.Enabled = true;
                    this.simpleButton1.Enabled = true;
                    this.smpCtmDelete.Enabled = true;
                    this.lupPrintRule.Enabled = true;
                    this.gvRuleDetail.OptionsBehavior.ReadOnly = false;
                    this.gvPackPrint.OptionsBehavior.ReadOnly = false;
                    this.sbtnUpDataPrint.Enabled = true;
                    break;
            }
        }


        #region // 工单基本信息

        ///// <summary>
        ///// 获取并绑定工单
        ///// </summary>
        //private void GetWorkOrderList()
        //{
        //    DataSet dsBindAttr = _workOrdersEntity.GetAllWorkOrderData();
        //    _dtlueWo = dsBindAttr.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
        //    _dtlueWoNotExistProid = _dtlueWo.Clone();
        //    DataRow[] drs = _dtlueWo.Select(string.Format(POR_WORK_ORDER_FIELDS.FIELD_PRO_ID + " is null"));
        //    foreach (DataRow dr in drs)
        //        _dtlueWoNotExistProid.ImportRow(dr);

        //    this.teOrderNumber.Properties.Items.Clear();
        //    foreach (DataRow dr in this._dtlueWoNotExistProid.Rows)
        //    {
        //        this.teOrderNumber.Properties.Items.Add(dr["ORDER_NUMBER"]);
        //    }
        //}

        /// <summary>
        /// 获取料信息
        /// </summary>
        //private void GetPartData()
        //{
        //    DataSet ds = _entity.GetPartNumber();
        //    if (string.IsNullOrEmpty(_entity.ErrorMsg))
        //    {
        //        this._dtPartNumber = ds.Tables[0];
        //    }
        //}

        ///// <summary>
        ///// 产品料号值变化时触发。
        ///// </summary>
        //private void tePartNo_EditValueChanged(object sender, EventArgs e)
        //{
        //    string partNumber = this.tePartNo.Text;
        //    EnumerableRowCollection<DataRow> drs = this._dtPartNumber.AsEnumerable()
        //                                             .Where(dr => Convert.ToString(dr[POR_PART_FIELDS.FIELD_PART_ID]) == partNumber);
        //    if (drs.Count() > 0)
        //    {
        //        this.teDescription.Text = Convert.ToString(drs.First()[POR_PART_FIELDS.FIELD_PART_DESC]);
        //        this.teDescription.Properties.ReadOnly = true;
        //    }
        //    else
        //    {
        //        this.teDescription.Text = string.Empty;
        //        this.teDescription.Properties.ReadOnly = true;
        //    }
        //}

        #endregion

        #region //规则信息

        #region //行信息新增处理 功率分档、功率子分档、功率分档花色、产品等级、衰减设置、打印标签设置

        /// <summary>
        /// 对工单产品进行新增
        /// </summary>
        /// <param name="dr">需要新增的行信息</param>
        private void ProProductUpdate(DataRow drPro, DataRow drRule)
        {
            for (int i = 0; i < _dtPorProduct.Rows.Count; i++)
            {
                if (_dtPorProduct.Rows[i].RowState != DataRowState.Deleted && _dtPorProduct.Rows[i].RowState != DataRowState.Detached)
                {
                    if (_dtPorProduct.Rows[i]["PART_NUMBER"].ToString().Equals(partNumber))
                    {
                        _dtPorProduct.Rows[i]["PRODUCT_KEY"] = drPro["PRODUCT_KEY"];
                        _dtPorProduct.Rows[i]["PRODUCT_CODE"] = drPro["PRODUCT_CODE"];
                        _dtPorProduct.Rows[i]["PRODUCT_NAME"] = drPro["PRODUCT_NAME"];
                        _dtPorProduct.Rows[i]["QUANTITY"] = drPro["QUANTITY"];
                        _dtPorProduct.Rows[i]["MAXPOWER"] = drPro["MAXPOWER"];
                        _dtPorProduct.Rows[i]["MINPOWER"] = drPro["MINPOWER"];
                        _dtPorProduct.Rows[i]["PRO_TEST_RULE"] = drPro["PRO_TEST_RULE"];
                        _dtPorProduct.Rows[i]["PROMODEL_NAME"] = drPro["PROMODEL_NAME"];
                        _dtPorProduct.Rows[i]["CODEMARK"] = drPro["CODEMARK"];
                        _dtPorProduct.Rows[i]["CUSTMARK"] = drPro["MEMO1"];
                        _dtPorProduct.Rows[i]["LABELTYPE"] = drPro["LABELTYPE"];
                        _dtPorProduct.Rows[i]["LABELVAR"] = drPro["LABELVAR"];
                        _dtPorProduct.Rows[i]["LABELCHECK"] = drPro["LABELCHECK"];
                        _dtPorProduct.Rows[i]["PRO_LEVEL"] = drPro["PRO_LEVEL"];
                        _dtPorProduct.Rows[i]["SHIP_QTY"] = drPro["SHIP_QTY"];
                        _dtPorProduct.Rows[i]["FULL_PALLET_QTY"] = drRule["FULL_PALLET_QTY"];
                        _dtPorProduct.Rows[i]["POWER_DEGREE"] = int.Parse("2");
                        _dtPorProduct.Rows[i]["LAST_TEST_TYPE"] = drRule["LAST_TEST_TYPE"];
                        _dtPorProduct.Rows[i]["CERTIFICATION"] = drPro["CERTIFICATION"];
                        _dtPorProduct.Rows[i]["TOLERANCE"] = drPro["TOLERANCE"];

                        //是否自动通过工单获取接线盒信息
                        // --modified by yongbing.yang
                        if (isAutoFillJunctionBox)
                        {
                            _dtPorProduct.Rows[i]["JUNCTION_BOX"] = workOrderJunctionBox;
                        }
                        else
                        {
                            _dtPorProduct.Rows[i]["JUNCTION_BOX"] = drPro["JUNCTION_BOX"];
                        }

                        _dtPorProduct.Rows[i]["CALIBRATION_TYPE"] = drPro["CALIBRATION_TYPE"];
                        _dtPorProduct.Rows[i]["CALIBRATION_CYCLE"] = drPro["CALIBRATION_CYCLE"];
                        _dtPorProduct.Rows[i]["FIX_CYCLE"] = drPro["FIX_CYCLE"];
                        _dtPorProduct.Rows[i]["CONSTANT_TEMPERTATURE_CYCLE"] = drPro["CONSTANT_TEMPERTATURE_CYCLE"];
                    }
                }
            }
        }

        /// <summary>
        /// 对产品等级进行新增
        /// </summary>
        /// <param name="dr">需要新增的行信息</param>
        private void ProLevelAdd(DataRow dr)
        {
            DataRow drProLevel = _dtProLevel.NewRow();

            drProLevel["WORK_ORDER_KEY"] = workOrderKey;
            drProLevel["PART_NUMBER"] = partNumber;
            drProLevel["GRADE"] = dr["GRADE"];
            drProLevel["PROLEVEL_SEQ"] = dr["PROLEVEL_SEQ"];
            drProLevel["MIX_LEVEL"] = dr["MIN_LEVEL"];
            drProLevel["MIX_LEVEL_DETAIL"] = 0;
            drProLevel["MIX_COLOR"] = dr["MIN_COLOR"];
            drProLevel["PALLET_GROUP"] = dr["PALLET_GROUP"];

            _dtProLevel.Rows.Add(drProLevel);
        }

        /// <summary>
        /// 对产品衰减系数进行新增
        /// </summary>
        /// <param name="dr">需要新增的行信息</param>
        private void ProDecayAdd(DataRow dr)
        {
            DataRow drProDecay = _dtProDecay.NewRow();

            drProDecay["WORK_ORDER_KEY"] = workOrderKey;
            drProDecay["PART_NUMBER"] = partNumber;
            drProDecay["DECAY_KEY"] = dr["DECAY_KEY"];
            drProDecay["DECAY_NEWKEY"] = CommonUtils.GenerateNewKey(0);
            drProDecay["DECOEFFI_KEY"] = dr["DECOEFFI_KEY"];
            drProDecay["DECAY_SEQ"] = dr["DECAY_SQL"];
            drProDecay["DECAY_POWER_MIN"] = dr["DECAY_POWER_MIN"];
            drProDecay["DECAY_POWER_MAX"] = dr["DECAY_POWER_MAX"];

            _dtProDecay.Rows.Add(drProDecay);
        }

        /// <summary>
        /// 对打印设置进行新增
        /// </summary>
        /// <param name="dr">需要新增的行信息</param>
        private void ProPrintSetAdd(DataRow dr)
        {
            DataRow drProPrintSet = _dtProPrintSet.NewRow();

            drProPrintSet["WORK_ORDER_KEY"] = workOrderKey;
            drProPrintSet["PART_NUMBER"] = partNumber;
            drProPrintSet["PRINTSET_KEY"] = CommonUtils.GenerateNewKey(0);
            drProPrintSet["PRINTLABEL_ID"] = dr["VIEW_NAME"];
            drProPrintSet["ISLABEL"] = dr["ISLABEL"];
            drProPrintSet["ISMAIN"] = dr["ISMAIN"];
            drProPrintSet["ISPACKAGEPRINT"] = dr["ISPACKAGEPRINT"];
            drProPrintSet["PRINT_QTY"] = dr["PRINT_QTY"];

            for (int i = 0; i < _dtProDecay.Rows.Count; i++)
            {
                if (_dtProDecay.Rows[i].RowState != DataRowState.Deleted && _dtProDecay.Rows[i].RowState != DataRowState.Detached)
                {
                    if (_dtProDecay.Rows[i]["DECAY_KEY"].ToString() == dr["DECAY_KEY"].ToString())
                    {
                        drProPrintSet["DECAY_KEY"] = _dtProDecay.Rows[i]["DECAY_NEWKEY"];
                        drProPrintSet["MinMaxPower"] = string.Format("{0}-{1}", _dtProDecay.Rows[i]["DECAY_POWER_MIN"], _dtProDecay.Rows[i]["DECAY_POWER_MAX"]);
                        break;
                    }
                }
            }




            _dtProPrintSet.Rows.Add(drProPrintSet);
        }

        /// <summary>
        /// 对功率分档进行新增
        /// </summary>
        /// <param name="dr">需要新增的行信息</param>
        private void ProPSAdd(DataRow dr)
        {
            DataRow drProPS = _dtProPS.NewRow();

            drProPS["WORK_ORDER_KEY"] = workOrderKey;
            drProPS["PART_NUMBER"] = partNumber;
            drProPS["POWERSET_KEY"] = dr["POWERSET_KEY"];
            drProPS["VERSION_NO"] = 0;
            drProPS["PS_SEQ"] = dr["PS_SEQ"];
            drProPS["PS_CODE"] = dr["PS_CODE"];
            drProPS["PS_RULE"] = dr["PS_RULE"];
            drProPS["MODULE_NAME"] = dr["MODULE_NAME"];
            drProPS["P_MIN"] = dr["P_MIN"];
            drProPS["P_MAX"] = dr["P_MAX"];
            drProPS["PMAXSTAB"] = dr["PMAXSTAB"];
            drProPS["ISCSTAB"] = dr["ISCSTAB"];
            drProPS["VOCSTAB"] = dr["VOCSTAB"];
            drProPS["IMPPSTAB"] = dr["IMPPSTAB"];
            drProPS["VMPPSTAB"] = dr["VMPPSTAB"];
            drProPS["FUSE"] = dr["FUSE"];
            drProPS["PS_SUBCODE"] = dr["PS_SUBCODE"];
            drProPS["PS_SUBCODE_DESC"] = dr["PS_SUBCODE_DESC"];
            drProPS["SUB_PS_WAY"] = dr["SUB_PS_WAY"];
            drProPS["POWER_DIFFERENCE"] = dr["POWER_DIFFERENCE"];

            _dtProPS.Rows.Add(drProPS);
        }

        /// <summary>
        /// 对功率分档花色进行新增
        /// </summary>
        /// <param name="dr">需要新增的行信息</param>
        private void ProPSSubAdd(DataRow dr)
        {
            DataRow drProPSSub = _dtProPSSub.NewRow();

            drProPSSub["WORK_ORDER_KEY"] = workOrderKey;
            drProPSSub["PART_NUMBER"] = partNumber;
            drProPSSub["POWERSET_KEY"] = dr["POWERSET_KEY"];
            drProPSSub["VERSION_NO"] = 0;
            drProPSSub["PS_SUB_CODE"] = dr["PS_DTL_SUBCODE"];
            drProPSSub["POWERLEVEL"] = dr["POWERLEVEL"];
            drProPSSub["P_DTL_MIN"] = dr["P_DTL_MIN"];
            drProPSSub["P_DTL_MAX"] = dr["P_DTL_MAX"];

            _dtProPSSub.Rows.Add(drProPSSub);
        }

        /// <summary>
        /// 对功率分档子分档进行新增
        /// </summary>
        /// <param name="dr">需要新增的行信息</param>
        private void ProPSColorAdd(DataRow dr)
        {
            DataRow drProPSColor = _dtProPSColor.NewRow();

            drProPSColor["WORK_ORDER_KEY"] = workOrderKey;
            drProPSColor["PART_NUMBER"] = partNumber;
            drProPSColor["POWERSET_KEY"] = dr["POWERSET_KEY"];
            drProPSColor["VERSION_NO"] = 0;
            drProPSColor["COLOR_CODE"] = dr["COLOR_CODE"];
            drProPSColor["COLOR_NAME"] = dr["COLOR_NAME"];
            drProPSColor["ARTICNO"] = dr["ARTICNO"];
            drProPSColor["DESCRIPTION"] = dr["DESCRIPTION"];
            drProPSColor["PRO_LEVEL"] = dr["PRO_LEVEL"];

            _dtProPSColor.Rows.Add(drProPSColor);
        }

        #endregion


        #region  //对视图显示信息进行绑定 产品、功率分档、功率子分档、功率分档花色、产品等级、衰减设置、打印标签设置

        /// <summary>
        /// 对产品信息进行绑定
        /// </summary>
        /// <param name="dtBind">对应的数据表</param>
        private void BindProduct()
        {
            this.gcProduct.MainView = gvProduct;
            this.gcProduct.DataSource = null;
            this.gcProduct.DataSource = _dtPorProduct;
        }

        /// <summary>
        /// 对产品等级进行绑定
        /// </summary>
        /// <param name="dr">对应的数据表</param>
        private void BindProLevel()
        {
            //获取选定产品对应的产品等级            
            DataView dv = this._dtProLevel.DefaultView;
            dv.RowFilter = string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber);
            dv.Sort = "PROLEVEL_SEQ ASC";
            this.gcProLevel.DataSource = dv;
            this.gvProLevel.FocusedRowHandle = -1;
        }

        /// <summary>
        /// 对产品衰减系数进行绑定
        /// </summary>        
        private void BindProDecay()
        {
            //获取选定产品对应的衰减信息
            DataView dv = this._dtProDecay.DefaultView;
            dv.RowFilter = string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber);
            dv.Sort = "DECAY_SEQ ASC";
            this.gcDecay.DataSource = dv;
            this.gvDecay.FocusedRowHandle = -1;
        }

        /// <summary>
        /// 对打印设置进行绑定
        /// </summary>
        /// <param name="dr">对应的数据表</param>
        private void BindProPrintSet()
        {
            //获取选定产品对应的打印标签设置
            DataView dv = this._dtProPrintSet.DefaultView;
            dv.RowFilter = string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber);
            this.gcProPrintSet.DataSource = dv;
            this.gvProPrintSet.FocusedRowHandle = -1;
        }

        /// <summary>
        /// 对功率分档进行绑定 Modified  by yongbing.yang 2015年11月20日 15:21:05
        /// </summary>
        /// <param name="dr">对应的数据表</param>
        private void BindProPS()
        {
            //清除分档主键在数据绑定后进行 重新获取
            proPowerSetKey = "";

            //获取产品对应的分档信息
            DataView dv = this._dtProPS.DefaultView;
            dv.RowFilter = string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}'", workOrderKey, partNumber);
            dv.Sort = "P_MIN ASC";
            //对获取的分档信息进行绑定
            this.gcPowerSet.DataSource = dv;


            //获取视图要绑定的行数如果存在信息聚焦到第一行
            if (dv.Count > 0)
            {
                //清除功率分档的聚焦行
                gvPowerSet.FocusedRowHandle = -1;
            }
        }

        /// <summary>
        /// 对功率分档子分档进行绑定
        /// </summary>
        /// <param name="dr">对应的数据表</param>
        private void BindProPSSub()
        {
            DataView dv = this._dtProPSSub.DefaultView;
            dv.RowFilter = string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' AND POWERSET_KEY ='{2}'  ", workOrderKey, partNumber, proPowerSetKey);
            dv.Sort = "PS_SUB_CODE ASC";
            this.gcPowerSetSub.DataSource = dv;
            this.gvPowerSetSub.FocusedRowHandle = -1;
        }

        /// <summary>
        /// 对功率分档花色进行绑定
        /// </summary>
        /// <param name="dr">对应的数据表</param>
        private void BindProPSColor()
        {
            DataView dv = this._dtProPSColor.DefaultView;
            dv.RowFilter = string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' AND POWERSET_KEY ='{2}'  ", workOrderKey, partNumber, proPowerSetKey);
            dv.Sort = "COLOR_CODE ASC";
            this.gcPowerSetColor.DataSource = dv;
            this.gvPowerSetColor.FocusedRowHandle = -1;
        }

        /// <summary>
        /// 对检验规则进行绑定
        /// </summary>
        private void BindCustCheckTypeData()
        {
            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet ds = entity.GetCustCheckTypeData();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.rilueCustCheckType.DataSource = ds.Tables[0];
            this.rilueCustCheckType.ValueMember = "CODE";
            this.rilueCustCheckType.DisplayMember = "NAME";
            this.rilueCustCheckType.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME"));
        }

        #endregion

        /// <summary>
        /// 对自动填充接线盒信息进行判断
        /// </summary>
        private void OrderMaterialFill()
        {
            string[] columns = new string[] { "IsAutoFill" };
            DataTable dtOrderMaterialFill = BaseData.Get(columns, "Basic_OrderMaterialFill");
            if (dtOrderMaterialFill.Rows.Count > 0)
            {
                isAutoFillJunctionBox = Convert.ToBoolean(dtOrderMaterialFill.Rows[0]["IsAutoFill"]);
            }
        }

        /// <summary>
        /// 获取工单对应的工单产品信息
        /// </summary>
        /// <param name="workorderKey">工单主键</param>
        private void GetWOProData(string workorderKey)
        {
            if (string.IsNullOrEmpty(workorderKey)) workorderKey = "";

            DataSet dsWOPro = new DataSet();
            dsWOPro = _workOrdersEntity.GetWorkOrderProByOrderKey(workorderKey);

            if (dsWOPro.Tables.Contains("POR_WO_PRD"))
                _dtPorProduct = dsWOPro.Tables["POR_WO_PRD"];
            if (dsWOPro.Tables.Contains("POR_WO_PRD_DECAY"))
                _dtProDecay = dsWOPro.Tables["POR_WO_PRD_DECAY"];
            if (dsWOPro.Tables.Contains("POR_WO_PRD_LEVEL"))
                _dtProLevel = dsWOPro.Tables["POR_WO_PRD_LEVEL"];
            if (dsWOPro.Tables.Contains("POR_WO_PRD_PRINTSET"))
                _dtProPrintSet = dsWOPro.Tables["POR_WO_PRD_PRINTSET"];
            _dtProPrintSet.Columns.Add("MinMaxPower");
            //对新增列的值进行遍历
            GetMinMaxPower();
            if (dsWOPro.Tables.Contains("POR_WO_PRD_PS"))
                _dtProPS = dsWOPro.Tables["POR_WO_PRD_PS"];
            if (dsWOPro.Tables.Contains("POR_WO_PRD_PS_CLR"))
                _dtProPSColor = dsWOPro.Tables["POR_WO_PRD_PS_CLR"];
            if (dsWOPro.Tables.Contains("POR_WO_PRD_PS_SUB"))
                _dtProPSSub = dsWOPro.Tables["POR_WO_PRD_PS_SUB"];
            if (dsWOPro.Tables.Contains("POR_WO_PRD_POWERSHOW"))//fyb
                _dtProPShow = dsWOPro.Tables["POR_WO_PRD_POWERSHOW"];

            #region 工单OEM信息
            //获取工单OEM信息
            if (dsWOPro.Tables.Contains("POR_WO_OEM"))
            {
                _dtWorkOrderOEM = dsWOPro.Tables["POR_WO_OEM"];
                if (_dtWorkOrderOEM != null && _dtWorkOrderOEM.Rows.Count == 1)
                {
                    ceIsOEMWorkOrder.Checked = true;
                    BindWorkOrderOEMInfo();
                }
                if (_dtWorkOrderOEM != null && _dtWorkOrderOEM.Rows.Count != 1)
                {
                    ceIsOEMWorkOrder.Checked = false;
                    ClearWorkOrderOEMInfo();
                }
            }

            #endregion

            #region 工单绑定默认工序
            if (dsWOPro.Tables.Contains("POR_WO_ROUTE"))
            {
                _dtWorkOrderRoute = dsWOPro.Tables["POR_WO_ROUTE"];

                if (_dtWorkOrderRoute.Rows.Count == 1)
                {
                    DataRow drRouteFirstOperation = _dtWorkOrderRoute.Rows[0];
                    this.beRouteEnterprise.Tag = Convert.ToString(drRouteFirstOperation["ROUTE_ENTERPRISE_VER_KEY"]);
                    this.beRouteEnterprise.Text = Convert.ToString(drRouteFirstOperation["ENTERPRISE_NAME"]);
                    this.teRouteName.Tag = Convert.ToString(drRouteFirstOperation["ROUTE_ROUTE_VER_KEY"]);
                    this.teRouteName.Text = Convert.ToString(drRouteFirstOperation["ROUTE_NAME"]);
                    this.teStepName.Tag = Convert.ToString(drRouteFirstOperation["ROUTE_STEP_KEY"]);
                    this.teStepName.Text = Convert.ToString(drRouteFirstOperation["ROUTE_STEP_NAME"]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(workorderKey) && _dtWorkOrderRoute.Rows.Count == 0)
                    {
                        DataRow drWorkOrderRoute = _dtWorkOrderRoute.NewRow();

                        drWorkOrderRoute["WORK_ORDER_KEY"] = workorderKey;
                        drWorkOrderRoute["WORK_ORDER_NUMBER"] = workOrderNumber;
                        drWorkOrderRoute["ROUTE_ENTERPRISE_VER_KEY"] = this.beRouteEnterprise.Tag;
                        drWorkOrderRoute["ENTERPRISE_NAME"] = this.beRouteEnterprise.Text;
                        drWorkOrderRoute["ROUTE_ROUTE_VER_KEY"] = this.teRouteName.Tag;
                        drWorkOrderRoute["ROUTE_NAME"] = this.teRouteName.Text;
                        drWorkOrderRoute["ROUTE_STEP_KEY"] = this.teStepName.Tag;
                        drWorkOrderRoute["ROUTE_STEP_NAME"] = this.teStepName.Text;

                        _dtWorkOrderRoute.Rows.Add(drWorkOrderRoute);
                    }
                }
            }
            #endregion


            if (dsWOPro.Tables.Contains("POR_WO_LINE"))
                _dtWorkOrderLine = dsWOPro.Tables["POR_WO_LINE"];

            #region 工单功率上下线管控信息
            //工单功率上下线管控信息
            if (dsWOPro.Tables.Contains("POR_WO_PRD_UPLOWRULE"))
                _dtUpAndLow = dsWOPro.Tables["POR_WO_PRD_UPLOWRULE"];

            #endregion

            #region EL 图片规则信息 Add by wubaofeng 2018/6/22
            if (dsWOPro.Tables.Contains("POR_WO_ELTESTRULE"))
            {
                DataTable dtElBefRule = new DataTable();
                dtElBefRule.Columns.Add("FOR_WO_ELESTRULE_KEY");
                dtElBefRule.Columns.Add("WORK_ORDER_KEY");
                dtElBefRule.Columns.Add("ELTESTRULE");
                dtElBefRule.Columns.Add("RULE_TYPE");
                dtElBefRule.Columns.Add("CREATE_USER_ID");
                dtElBefRule.Columns.Add("EDIT_USER_ID");
                DataRow drBefELRule = dtElBefRule.NewRow();

                DataTable dtAftElRule = new DataTable();
                dtAftElRule.Columns.Add("FOR_WO_ELESTRULE_KEY");
                dtAftElRule.Columns.Add("WORK_ORDER_KEY");
                dtAftElRule.Columns.Add("ELTESTRULE");
                dtAftElRule.Columns.Add("RULE_TYPE");
                dtAftElRule.Columns.Add("CREATE_USER_ID");
                dtAftElRule.Columns.Add("EDIT_USER_ID");
                DataRow drElAftRule = dtAftElRule.NewRow();

                _dtPorWoElTestRule = dsWOPro.Tables["POR_WO_ELTESTRULE"];
                if (_dtPorWoElTestRule != null && _dtPorWoElTestRule.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtPorWoElTestRule.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(_dtPorWoElTestRule.Rows[i]["RULE_TYPE"].ToString()) && _dtPorWoElTestRule.Rows[i]["RULE_TYPE"].ToString() == "befLam")
                        {
                            drBefELRule["WORK_ORDER_KEY"] = workOrderKey;
                            drBefELRule["ELTESTRULE"] = _dtPorWoElTestRule.Rows[i]["ELTESTRULE"].ToString();
                            drBefELRule["RULE_TYPE"] = "befLam";
                            drBefELRule["CREATE_USER_ID"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                            drBefELRule["EDIT_USER_ID"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                            dtElBefRule.Rows.Add(drBefELRule);
                        }
                        else if (!string.IsNullOrEmpty(_dtPorWoElTestRule.Rows[i]["RULE_TYPE"].ToString()) && _dtPorWoElTestRule.Rows[i]["RULE_TYPE"].ToString() == "aftLam")
                        {
                            drElAftRule["WORK_ORDER_KEY"] = workOrderKey;
                            drElAftRule["ELTESTRULE"] = _dtPorWoElTestRule.Rows[i]["ELTESTRULE"].ToString();
                            drElAftRule["RULE_TYPE"] = "befLam";
                            drElAftRule["CREATE_USER_ID"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                            drElAftRule["EDIT_USER_ID"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                            dtAftElRule.Rows.Add(drElAftRule);
                        }
                    }
                    if (dtElBefRule.Rows.Count > 0)
                    {
                        lstBefLamRule.Properties.Columns.Clear();
                        lstBefLamRule.EditValue = "ELTESTRULE";
                        lstBefLamRule.Properties.NullText = _dtPorWoElTestRule.Rows[0]["ELTESTRULE"].ToString();
                        lstBefLamRule.Properties.DisplayMember = "ELTESTRULE";
                        lstBefLamRule.Properties.ValueMember = "ELTESTRULE";
                        lstBefLamRule.Properties.DataSource = dtElBefRule;
                        lstBefLamRule.ItemIndex = 0;
                    }
                    else if (dtAftElRule.Rows.Count > 0)
                    {
                        lstAfterLamRule.Properties.Columns.Clear();
                        lstAfterLamRule.EditValue = "ELTESTRULE";
                        lstAfterLamRule.Properties.NullText = _dtPorWoElTestRule.Rows[0]["ELTESTRULE"].ToString();
                        lstAfterLamRule.Properties.DisplayMember = "ELTESTRULE";
                        lstAfterLamRule.Properties.ValueMember = "ELTESTRULE";
                        lstAfterLamRule.Properties.DataSource = dtAftElRule;
                        lstAfterLamRule.ItemIndex = 0;
                    }
                }
                else
                {
                    //lueELTestRule.Checked = false;
                    //lueELTestRule.Text = "";
                    BindELTestRule();
                }

            }
            #endregion

        }

        /// <summary>
        /// 通过工单号获取对应的接线盒信息
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        private void GetWOJunctionBox(string workorderNumber)
        {
            DataSet dsWOJunctionBox = new DataSet();
            dsWOJunctionBox = _workOrdersEntity.GetWOJunctionBox(workorderNumber);

            if (dsWOJunctionBox.Tables.Contains("POR_WO_MATERIAL_JUNCTION_BOX"))
            {
                if (dsWOJunctionBox.Tables["POR_WO_MATERIAL_JUNCTION_BOX"].Rows.Count > 0)
                {
                    workOrderJunctionBox = dsWOJunctionBox.Tables["POR_WO_MATERIAL_JUNCTION_BOX"].Rows[0]["JUNCTION_BOX"].ToString();
                }
                else
                {
                    workOrderJunctionBox = "";
                }
            }
        }

        /// <summary>
        /// 对工单OEM信息进行栏位绑定
        /// </summary>
        private void BindWorkOrderOEMInfo()
        {
            DataRow dr = _dtWorkOrderOEM.Rows[0];

            cbeCustomer.Text = dr["CUSROMER"].ToString();
            cbeCellSupplier.Text = dr["CELL_SUPPLIER"].ToString();
            cbeCellType.Text = dr["CELL_TYPE"].ToString();
            cbeCellModel.Text = dr["CELL_MODEL"].ToString();
            cbeStructureParam.Text = dr["STRUCTURE_PARAM"].ToString();
            cbePlaceOrigin.Text = dr["PLACE_ORIGIN"].ToString();
            cbeGlassType.Text = dr["GLASS_TYPE"].ToString();
            cbeJunctionBoxOEM.Text = dr["JUNCTION_BOX"].ToString();
            cbeBOMAuthenticationCode.Text = dr["BOM_AUTHENTICATION_CODE"].ToString();
            cbeBOMDesign.Text = dr["BOM_DESIGN"].ToString();
            cbeSEMoudleType.Text = dr["SE_MODULE_TYPE"].ToString();
        }

        /// <summary>
        /// 对工单OEM信息进行栏位清除
        /// </summary>
        private void ClearWorkOrderOEMInfo()
        {
            cbeCustomer.Text = "";
            cbeCellSupplier.Text = "";
            cbeCellType.Text = "";
            cbeCellModel.Text = "";
            cbeStructureParam.Text = "";
            cbePlaceOrigin.Text = "";
            cbeGlassType.Text = "";
            cbeJunctionBoxOEM.Text = "";
            cbeBOMAuthenticationCode.Text = "";
            cbeBOMDesign.Text = "";
            cbeSEMoudleType.Text = "";

        }

        /// <summary>
        /// 对标签打印的最大最小值进行设定
        /// </summary>
        private void GetMinMaxPower()
        {
            foreach (DataRow drProPrintSet in _dtProPrintSet.Rows)
            {
                for (int i = 0; i < _dtProDecay.Rows.Count; i++)
                {
                    if (_dtProDecay.Rows[i].RowState != DataRowState.Deleted && _dtProDecay.Rows[i].RowState != DataRowState.Detached)
                    {
                        if (_dtProDecay.Rows[i]["DECAY_KEY"].ToString() == drProPrintSet["DECAY_KEY"].ToString())
                        {
                            drProPrintSet["MinMaxPower"] = string.Format("{0}-{1}", _dtProDecay.Rows[i]["DECAY_POWER_MIN"], _dtProDecay.Rows[i]["DECAY_POWER_MAX"]);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取新增产品对应测试规则的产品等级、衰减设置、打印标签设置数据
        /// </summary>
        private void GetRuleData()
        {

            Hashtable hsTable = new Hashtable();
            hsTable[BASE_TESTRULE.FIELDS_TESTRULE_KEY] = testRuleKey;
            DataSet dsTestRuleAllData = _baseTestRuleEntity.GetTestRuleDeatilData(hsTable);
            if (!string.IsNullOrEmpty(_baseTestRuleEntity.ErrorMsg))
            {
                MessageService.ShowMessage(_baseTestRuleEntity.ErrorMsg);
                return;
            }
            //对衰减设置进行新增，并绑定
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_DECAY.DATABASE_TABLE_NAME))
            {
                DataTable dtDecay = dsTestRuleAllData.Tables[BASE_TESTRULE_DECAY.DATABASE_TABLE_NAME];
                int rowNum = dtDecay.Rows.Count;

                for (int i = 0; i < rowNum; i++)
                {
                    DataRow dr = dtDecay.Rows[i];
                    ProDecayAdd(dr);
                }

                //对产品对应的衰减信息进行绑定
                BindProDecay();
            }

            //对产品等级进行新增，并绑定
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_NAME))
            {
                DataTable dtLevel = dsTestRuleAllData.Tables[BASE_TESTRULE_PROLEVEL.DATABASE_TABLE_NAME];

                if (!string.IsNullOrEmpty(partGrades))
                {


                    DataRow[] drsLevel = dtLevel.Select(string.Format(" GRADE  IN ({0})", partGrades));

                    foreach (DataRow dr in drsLevel)
                        ProLevelAdd(dr);

                }
                else
                {
                    int rowNum = dtLevel.Rows.Count;

                    for (int i = 0; i < rowNum; i++)
                    {
                        DataRow dr = dtLevel.Rows[i];
                        ProLevelAdd(dr);
                    }
                }





                //对产品对应的产品等级进行绑定
                BindProLevel();
            }

            //对打印标签设置进行新增，并绑定
            if (dsTestRuleAllData.Tables.Contains(BASE_TESTRULE_PRINTSET.DATABASE_TABLE_NAME))
            {
                DataTable dtPrintSet = dsTestRuleAllData.Tables[BASE_TESTRULE_PRINTSET.DATABASE_TABLE_NAME];


                int rowNum = dtPrintSet.Rows.Count;
                for (int i = 0; i < rowNum; i++)
                {
                    DataRow dr = dtPrintSet.Rows[i];
                    ProPrintSetAdd(dr);
                }

                //对产品对应的衰减信息进行绑定
                BindProPrintSet();
            }

        }

        /// <summary>
        /// 对选择产品料号需求的产品等级进行遍历
        /// </summary>
        private void GetPartGradeArray()
        {
            if (_dtPartNumber != null && _dtPartNumber.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(partNumber))
                {
                    DataRow drPartInfo = _dtPartNumber.Select(string.Format(" PART_NUMBER = '{0}'", partNumber))[0];

                    //获取栏位对应的产品等级
                    string partGrade = Convert.ToString(drPartInfo["GRADES"]);

                    //进行处理后的产品等级
                    partGrades = "";

                    if (!string.IsNullOrEmpty(partGrade))
                    {
                        string[] partGradeArray = partGrade.Split(',');
                        for (int i = 0; i < partGradeArray.Length; i++)
                        {
                            if (partGrades == "")
                            {
                                partGrades = partGrades + "'" + partGradeArray[i].ToString().Trim() + "'";
                            }
                            else
                            {
                                partGrades = partGrades + ",'" + partGradeArray[i].ToString().Trim() + "'";
                            }
                        }

                    }
                }
            }
            else
            {
                //MessageBox.Show("请先进行料号的维护", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取工单对应的基本信息
        /// </summary>
        /// <param name="orderNumber">工单号</param>
        private void GetWorkOrderData(string orderNumber)
        {
            if (!string.IsNullOrEmpty(orderNumber))
            {
                DataSet dsOrderNumber = this._entity.GetWorkorderInfo(orderNumber);
                if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageService.ShowError(this._entity.ErrorMsg);
                    return;
                }
                if (dsOrderNumber.Tables.Count == 0
                    || dsOrderNumber.Tables[0].Rows.Count == 0)
                {
                    //MessageService.ShowError(string.Format("工单({0})不存在，请确认。", orderNumber));
                    MessageService.ShowError(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0002}"), orderNumber));
                    return;
                }
                this._dtWorkOrderNumber = dsOrderNumber.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
                DataRow drWorkOrderNumber = this._dtWorkOrderNumber.Rows[0];

                //保存选择工单的key和产品编码
                workOrderKey = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY]);
                partNumber = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER]);

                //获取产品料号清单并进行绑定
                BindPartNumber(partNumber);

                //设置控件值。
                this.teOrderNumber.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
                this.tePlanQty.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_QUANTITY_ORDERED]);
                this.teState.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_STATE]);
                this.tePartNo.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER]);
                this.teRevenue.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_REVENUE_TYPE]);
                this.teOrderType.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_ORDER_TYPE]);
                this.teStartTime.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_START_TIME]);
                this.teFinishTime.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_EDIT_TIME]);
                this.meComment.Text = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_COMMENTS]);
                this.factoryCode = Convert.ToString(drWorkOrderNumber[POR_WORK_ORDER_FIELDS.FIELD_FACTORY_NAME]);
                if (dsOrderNumber.Tables.Contains(POR_WORK_ORDER_BOM_FIELDS.DATABASE_TABLE_NAME))
                {
                    this._dtWorkOrderBom = dsOrderNumber.Tables[POR_WORK_ORDER_BOM_FIELDS.DATABASE_TABLE_NAME];
                }
                else
                {
                    POR_WORK_ORDER_BOM_FIELDS orderBom = new POR_WORK_ORDER_BOM_FIELDS();
                    this._dtWorkOrderBom = CommonUtils.CreateDataTable(orderBom);
                }
                this.gcBOM.DataSource = this._dtWorkOrderBom;
            }
        }

        /// <summary>
        /// 通过分档代码获取获取对应的功率分档
        /// </summary>
        /// <param name="powerSetCode">选择产品对应的分档代码</param>
        private void GetPowerSetByPowerSetCode(string powerSetCode)
        {
            string partMinPower = string.Empty;
            string partMaxPower = string.Empty;

            if (_dtPartNumber != null && _dtPartNumber.Rows.Count > 0)
            {
                DataRow drPartInfo = _dtPartNumber.Select(string.Format(" PART_NUMBER = '{0}'", partNumber))[0];

                partMinPower = drPartInfo["MIN_POWER"].ToString();
                partMaxPower = drPartInfo["MAX_POWER"].ToString();

            }

            DataSet dsReturn = _workOrdersEntity.GetPowerSetByPowerSetCode(powerSetCode, partMinPower, partMaxPower);

            if (!string.IsNullOrEmpty(_workOrdersEntity.ErrorMsg))
            {
                MessageService.ShowMessage(_workOrdersEntity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Contains("PowerSet"))
            {
                DataTable dtPowerSet = dsReturn.Tables["PowerSet"];
                if (dtPowerSet != null && dtPowerSet.Rows.Count > 0)
                {
                    //对功率分档进行新增
                    foreach (DataRow drPS in dtPowerSet.Rows)
                    {
                        ProPSAdd(drPS);
                        BindProPS();
                    }
                    //判断功率分档是否存在子分档存在的话进行子分档新增
                    if (dsReturn.Tables.Contains("PowerSetSub"))
                    {
                        DataTable dtPowerSetSub = dsReturn.Tables["PowerSetSub"];
                        if (dtPowerSetSub != null && dtPowerSetSub.Rows.Count > 0)
                        {
                            foreach (DataRow drSub in dtPowerSetSub.Rows)
                                ProPSSubAdd(drSub);

                        }
                    }
                    //判断功率分档是否存在花色存在的话进行花色新增
                    if (dsReturn.Tables.Contains("PowerSetColor"))
                    {
                        DataTable dtPowerSetColor = dsReturn.Tables["PowerSetColor"];
                        if (dtPowerSetColor != null && dtPowerSetColor.Rows.Count > 0)
                        {
                            foreach (DataRow drSub in dtPowerSetColor.Rows)
                                ProPSColorAdd(drSub);

                        }
                    }
                }
            }


            BindProPSColor();
            BindProPSSub();

        }

        /// <summary>
        /// 保存时更新所有选中的视图单元格
        /// </summary>
        private void UpAllFocuseCell()
        {
            if (this.gvBOM.State == GridState.Editing && this.gvBOM.IsEditorFocused && this.gvBOM.EditingValueModified)
            {
                this.gvBOM.SetFocusedRowCellValue(this.gvBOM.FocusedColumn, this.gvBOM.EditingValue);
            }
            this.gvBOM.UpdateCurrentRow();

            if (this.gvProduct.State == GridState.Editing && this.gvProduct.IsEditorFocused && this.gvProduct.EditingValueModified)
            {
                this.gvProduct.SetFocusedRowCellValue(this.gvProduct.FocusedColumn, this.gvProduct.EditingValue);
            }
            this.gvProduct.UpdateCurrentRow();

            if (this.gvPowerSet.State == GridState.Editing && this.gvPowerSet.IsEditorFocused && this.gvPowerSet.EditingValueModified)
            {
                this.gvPowerSet.SetFocusedRowCellValue(this.gvPowerSet.FocusedColumn, this.gvPowerSet.EditingValue);
            }
            this.gvPowerSet.UpdateCurrentRow();

            if (this.gvPowerSetSub.State == GridState.Editing && this.gvPowerSetSub.IsEditorFocused && this.gvPowerSetSub.EditingValueModified)
            {
                this.gvPowerSetSub.SetFocusedRowCellValue(this.gvPowerSetSub.FocusedColumn, this.gvPowerSetSub.EditingValue);
            }
            this.gvPowerSetSub.UpdateCurrentRow();

            if (this.gvPowerSetColor.State == GridState.Editing && this.gvPowerSetColor.IsEditorFocused && this.gvPowerSetColor.EditingValueModified)
            {
                this.gvPowerSetColor.SetFocusedRowCellValue(this.gvPowerSetColor.FocusedColumn, this.gvPowerSetColor.EditingValue);
            }
            this.gvPowerSetColor.UpdateCurrentRow();

            if (this.gvProLevel.State == GridState.Editing && this.gvProLevel.IsEditorFocused && this.gvProLevel.EditingValueModified)
            {
                this.gvProLevel.SetFocusedRowCellValue(this.gvProLevel.FocusedColumn, this.gvProLevel.EditingValue);
            }
            this.gvProLevel.UpdateCurrentRow();

            if (this.gvDecay.State == GridState.Editing && this.gvDecay.IsEditorFocused && this.gvDecay.EditingValueModified)
            {
                this.gvDecay.SetFocusedRowCellValue(this.gvDecay.FocusedColumn, this.gvDecay.EditingValue);
            }
            this.gvDecay.UpdateCurrentRow();

            if (this.gvProPrintSet.State == GridState.Editing && this.gvProPrintSet.IsEditorFocused && this.gvProPrintSet.EditingValueModified)
            {
                this.gvProPrintSet.SetFocusedRowCellValue(this.gvProPrintSet.FocusedColumn, this.gvProPrintSet.EditingValue);
            }
            this.gvProPrintSet.UpdateCurrentRow();

            if (this.gvOrderProperties.State == GridState.Editing && this.gvOrderProperties.IsEditorFocused && this.gvOrderProperties.EditingValueModified)
            {
                this.gvOrderProperties.SetFocusedRowCellValue(this.gvOrderProperties.FocusedColumn, this.gvOrderProperties.EditingValue);
            }
            this.gvOrderProperties.UpdateCurrentRow();

            if (this.gvWorkOrderLine.State == GridState.Editing && this.gvWorkOrderLine.IsEditorFocused && this.gvWorkOrderLine.EditingValueModified)
            {
                this.gvWorkOrderLine.SetFocusedRowCellValue(this.gvWorkOrderLine.FocusedColumn, this.gvWorkOrderLine.EditingValue);
            }
            this.gvWorkOrderLine.UpdateCurrentRow();
            if (this.gvUPLOWRule.State == GridState.Editing && this.gvUPLOWRule.IsEditorFocused && this.gvUPLOWRule.EditingValueModified)
            {
                this.gvUPLOWRule.SetFocusedRowCellValue(this.gvUPLOWRule.FocusedColumn, this.gvUPLOWRule.EditingValue);
            }
            this.gvUPLOWRule.UpdateCurrentRow();

            if (this.gvEffCtm.State == GridState.Editing && this.gvEffCtm.IsEditorFocused && this.gvEffCtm.EditingValueModified)
            {
                this.gvEffCtm.SetFocusedRowCellValue(this.gvEffCtm.FocusedColumn, this.gvEffCtm.EditingValue);
            }
            this.gvEffCtm.UpdateCurrentRow();

            if (this.gvRuleDetail.State == GridState.Editing && this.gvRuleDetail.IsEditorFocused && this.gvRuleDetail.EditingValueModified)
            {
                this.gvRuleDetail.SetFocusedRowCellValue(this.gvRuleDetail.FocusedColumn, this.gvRuleDetail.EditingValue);
            }
            this.gvRuleDetail.UpdateCurrentRow();

            if (this.gvPackPrint.State == GridState.Editing && this.gvPackPrint.IsEditorFocused && this.gvPackPrint.EditingValueModified)
            {
                this.gvPackPrint.SetFocusedRowCellValue(this.gvPackPrint.FocusedColumn, this.gvPackPrint.EditingValue);
            }
            this.gvPackPrint.UpdateCurrentRow();

            if (this.gvPowerShow.State == GridState.Editing && this.gvPowerShow.IsEditorFocused && this.gvPowerShow.EditingValueModified)
            {
                this.gvPowerShow.SetFocusedRowCellValue(this.gvPowerShow.FocusedColumn, this.gvPowerShow.EditingValue);
            }
            this.gvPowerShow.UpdateCurrentRow();
            try
            {
                ((DataView)gvEffCtm.DataSource).Table.AcceptChanges();
            }

            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 判断产品对应的打印设置是否是相同标签模版
        /// </summary>
        /// <param name="dt">产品对应的打印设置</param>
        /// <returns>如果设置标签模版一致的话为：true、反之为：flase</returns>
        private bool IsEqual(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i].RowState != DataRowState.Deleted && dt.Rows[i].RowState != DataRowState.Detached)
                {
                    if (dt.Rows[i]["VIEW_NAME"].Equals(dt.Rows[i + 1]["VIEW_NAME"]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 判断在已绑定数据中是否存在该行数据
        /// </summary>
        /// <param name="key">需要插入的值</param>
        /// <param name="dt">需要校对的数据</param>
        /// <param name="columnName">校对的列名称</param>
        /// <returns></returns>
        private bool IsExist(string value, DataTable dt, string columnName)
        {
            int rowNum = dt.Rows.Count;

            for (int i = 0; i < rowNum; i++)
            {
                if (dt.Rows[i].RowState != DataRowState.Deleted && dt.Rows[i].RowState != DataRowState.Detached)
                {
                    if (value == dt.Rows[i][columnName].ToString())
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        /// <summary>
        /// 同时对两个信息进行信息列进行确认判断是否已插入相同的行信息
        /// </summary>
        /// <param name="value1">第一个参数的值</param>
        /// <param name="value2">第二个参数的值</param>
        /// <param name="dt">需要校对的数据</param>
        /// <param name="columnName1">第一个参数的列名</param>
        /// <param name="columnName2">第二个参数的列名</param>
        /// <returns></returns>
        private bool IsExist(string value1, string value2, DataTable dt, string columnName1, string columnName2)
        {
            int rowNum = dt.Rows.Count;

            for (int i = 0; i < rowNum; i++)
            {
                if (dt.Rows[i].RowState != DataRowState.Deleted && dt.Rows[i].RowState != DataRowState.Detached)
                {
                    if (value1 == dt.Rows[i][columnName1].ToString() && value2 == dt.Rows[i][columnName2].ToString())
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        /// <summary>
        /// 在选择工单改变时对添加的数据进行清除
        /// 并添加产品信息的第一项
        /// </summary>
        private void BindWorOrderPro()
        {
            //清空视图绑定的信息
            ClearWorkOrderPro();

            DataRow drNew = _dtPorProduct.NewRow();

            drNew["WORK_ORDER_KEY"] = workOrderKey;
            drNew["ITEM_NO"] = 1;
            drNew["PART_NUMBER"] = partNumber;
            drNew["VERSION_NO"] = 1;
            drNew["ORDER_NUMBER"] = teOrderNumber.Text;
            drNew["IS_MAIN"] = "Y";

            drNew["CUSTCHECK_TYPE"] = "1";

            _dtPorProduct.Rows.Add(drNew);

            //对产品信息进行绑定
            BindProduct();
            //对选中行的产品对应的信息进行绑定
            BindDataByPro();

        }

        private void ClearWorkOrderPro()
        {
            //对产品表数据进行清除
            _dtPorProduct.Clear();
            _dtPorProduct.AcceptChanges();
            //对功率分档进行数据清除
            _dtProPS.Clear();
            _dtProPS.AcceptChanges();
            //对功率分档子分档数据进行清除
            _dtProPSSub.Clear();
            _dtProPSSub.AcceptChanges();
            //对功率分档花色进行数据清除
            _dtProPSColor.Clear();
            _dtProPSColor.AcceptChanges();
            //对产品等级进行数据清除
            _dtProLevel.Clear();
            _dtProLevel.AcceptChanges();
            //对衰减设置进行数据清除
            _dtProDecay.Clear();
            _dtProDecay.AcceptChanges();
            //对标签打印设置进行数据清除
            _dtProPrintSet.Clear();
            _dtProPrintSet.AcceptChanges();
            //对工单属性进行清空
            _dtWorkOrderAttr.Clear();
            _dtWorkOrderAttr.AcceptChanges();

            //设置工单OEM信息为False，并清空
            ceIsOEMWorkOrder.Checked = false;
            _dtWorkOrderOEM.Clear();
            _dtWorkOrderOEM.AcceptChanges();

            //对工单线别绑定进行清除
            _dtWorkOrderLine.Clear();
            _dtWorkOrderLine.AcceptChanges();

            //对工单功率上下线进行卡控
            _dtUpAndLow.Clear();
            _dtUpAndLow.AcceptChanges();

            if (_dtPorWoElTestRule != null)
            {
                _dtPorWoElTestRule.Clear();
                _dtPorWoElTestRule.AcceptChanges();
            }
        }

        /// <summary>
        /// 在移除产品的时候对产品相关的信息都进行移除
        /// </summary>
        private void RemoveByProKey()
        {
            DataRow[] drs = null;

            //对产品相关的功率分档进行移除
            drs = _dtProPS.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber));
            foreach (DataRow dr in drs)
                dr.Delete();

            //对产品相关的功率分档子分档进行移除
            drs = _dtProPSSub.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber));
            foreach (DataRow dr in drs)
                dr.Delete();

            //对产品相关的功率分档花色进行移除
            drs = _dtProPSColor.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber));
            foreach (DataRow dr in drs)
                dr.Delete();

            //对产品相关的产品等级进行移除
            drs = _dtProLevel.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber));
            foreach (DataRow dr in drs)
                dr.Delete();
            //对产品相关的衰减设置进行移除
            drs = _dtProDecay.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber));
            foreach (DataRow dr in drs)
                dr.Delete();

            //对产品相关的标签打印设置进行移除
            drs = _dtProPrintSet.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' ", workOrderKey, partNumber));
            foreach (DataRow dr in drs)
                dr.Delete();


        }

        /// <summary>
        /// 通过产品料号和功率Key对相关的 子分档、花色进行移除
        /// </summary>
        private void RemoveByProAndPowerKey()
        {
            DataRow[] drs = null;

            //对功率分档对应的子分档进行移除
            drs = _dtProPSSub.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' AND POWERSET_KEY ='{2}'  ", workOrderKey, partNumber, proPowerSetKey));
            foreach (DataRow dr in drs)
                dr.Delete();

            //对功率分档对应的花色进行移除
            drs = _dtProPSColor.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}' AND POWERSET_KEY ='{2}'  ", workOrderKey, partNumber, proPowerSetKey));
            foreach (DataRow dr in drs)
                dr.Delete();
        }

        /// <summary>
        /// 对下拉控件进行数据绑定 绑定衰减系数、标签模版选择
        /// </summary>
        private void BindParamData()
        {
            //绑定衰减系数对应的下拉清单
            DataTable dtDecay = new DecayCoeffiEntity().GetDecayCoeffiData().Tables[BASE_DECAYCOEFFI.DATABASE_TABLE_NAME];
            DataTable dtDecayBind = dtDecay.DefaultView.ToTable(true, new string[] { BASE_DECAYCOEFFI.FIELDS_D_CODE,
                                                                                    BASE_DECAYCOEFFI.FIELDS_D_CODE_DESC,
                                                                                    BASE_DECAYCOEFFI.FIELDS_D_NAME,
                                                                                    BASE_DECAYCOEFFI.FIELDS_DIT,
                                                                                    BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY });

            rpgleDecoeffiKey.DisplayMember = BASE_DECAYCOEFFI.FIELDS_D_CODE;
            rpgleDecoeffiKey.ValueMember = BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY;
            rpgleDecoeffiKey.DataSource = dtDecayBind;


            //绑定标签打印的标签选择下拉清单
            DataSet dsPrint = _baseTestRuleEntity.GetPrintData();
            DataTable dtPrint = dsPrint.Tables[0];
            if (dtPrint.Columns.Contains(CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER))
                dtPrint.Columns.Remove(CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER);
            riglueLabelData.DisplayMember = "LABEL_NAME";
            riglueLabelData.ValueMember = "LABEL_ID";
            riglueLabelData.DataSource = dtPrint;


            //绑定产品设置的产品代码选择
            Hashtable hstable = new Hashtable();

            _dtProList = _porProductEntity.GetPorProductData(hstable).Tables[POR_PRODUCT.DATABASE_TABLE_NAME];
            _dtProListBind = _dtProList.DefaultView.ToTable(true, new string[] {  "PRODUCT_CODE",
                                                                                            "PRODUCT_NAME",
                                                                                            "MINPOWER",
                                                                                            "MAXPOWER",
                                                                                            "PRO_TEST_RULE",
                                                                                            "PROMODEL_NAME",
                                                                                            "PRODUCT_KEY",
                                                                                            "ISKINGLING"});

            rpgleProList.DisplayMember = "PRODUCT_CODE";
            rpgleProList.ValueMember = "PRODUCT_KEY";
            rpgleProList.DataSource = _dtProListBind;


            //绑定共产线别信息
            _dtFactoryLineList = _workOrdersEntity.GetFatoryLine().Tables["FACTORY_LINE"];
            rpgleFactoryLine.DisplayMember = "LINE_NAME";
            rpgleFactoryLine.ValueMember = "LINE_KEY";
            rpgleFactoryLine.DataSource = _dtFactoryLineList;

        }

        /// <summary>
        /// 获去工单对应的产品料号信息
        /// </summary>
        private void BindPartNumber(string mainPartNumber)
        {
            //通过主产品料号获取对应的表信息
            if (_dtPartNumber != null && _dtPartNumber.Rows.Count > 0)
            {
                _dtPartNumber.Clear();
            }

            _dtPartNumber = _workOrdersEntity.GetPartNumberByMainPartNumber(mainPartNumber).Tables["POR_PART_BYPRODUCT"];

            partNumberBind = _dtPartNumber.DefaultView.ToTable(true, new string[] {   "ITEM_NO",
                                                                                                "MAIN_PART_NUMBER",
                                                                                                "PART_NUMBER",
                                                                                                "PART_DESC"
                                                                                             });

            //对工单对应料号描述进行绑定
            if (_dtPartNumber != null && _dtPartNumber.Rows.Count > 0)
            {
                DataRow drMainPartInfo = _dtPartNumber.Select(string.Format(" PART_NUMBER = '{0}'", mainPartNumber))[0];
                this.teDescription.Text = drMainPartInfo["PART_DESC"].ToString();
            }

            rpglePartNumber.DisplayMember = "PART_NUMBER";
            rpglePartNumber.ValueMember = "PART_NUMBER";
            rpglePartNumber.DataSource = partNumberBind;

        }

        /// <summary>
        /// 绑定标签打印的衰减序号
        /// </summary>
        private void BindPrintDeacy()
        {
            DataRow[] drPrintDecays = _dtProDecay.Select(string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}'", workOrderKey, partNumber));
            DataTable dtDecay = _dtProDecay.Clone();

            foreach (DataRow dr in drPrintDecays)
                dtDecay.ImportRow(dr);


            DataTable dtPrintDecayBind = dtDecay.DefaultView.ToTable(true, new string[] { "DECAY_NEWKEY",
                                                                                          "DECAY_SEQ",
                                                                                          "DECAY_POWER_MIN",
                                                                                          "DECAY_POWER_MAX"});
            riglueDECAY_KEY.DisplayMember = "DECAY_SEQ";
            riglueDECAY_KEY.ValueMember = "DECAY_NEWKEY";
            riglueDECAY_KEY.DataSource = dtPrintDecayBind;

        }
        /// <summary>
        /// 绑定EL图片规则 Add by wubaofeng 2018/6/22
        /// </summary>
        private void BindElPicRule(string workOrderKey, string ruleType)
        {
            DataSet dsPrintRule = _entity.GetElPicRuleData(workOrderKey, ruleType);
            if (dsPrintRule.Tables.Contains("POR_WO_ELTESTRULE") && dsPrintRule.Tables["POR_WO_ELTESTRULE"].Rows.Count > 0)
            {
                DataTable dtElPicRuleBind = dsPrintRule.Tables["POR_WO_ELTESTRULE"];
                lstBefLamRule.Properties.DisplayMember = "ELTESTRULE";
                lstBefLamRule.Properties.ValueMember = "ELTESTRULE";
                lstBefLamRule.Properties.DataSource = dtElPicRuleBind;
                lstBefLamRule.Text = dtElPicRuleBind.Rows[0]["ELTESTRULE"].ToString();
            }
            else
            {
                lstBefLamRule.Text = "";
            }
        }

        private void BindAftElPicRule(string workOrderKey, string ruleType)
        {
            DataSet dsPrintRule = _entity.GetElPicRuleData(workOrderKey, ruleType);
            if (dsPrintRule.Tables.Contains("POR_WO_ELTESTRULE") && dsPrintRule.Tables["POR_WO_ELTESTRULE"].Rows.Count > 0)
            {
                DataTable dtElPicRuleBind = dsPrintRule.Tables["POR_WO_ELTESTRULE"];
                lstAfterLamRule.Properties.DisplayMember = "ELTESTRULE";
                lstAfterLamRule.Properties.ValueMember = "ELTESTRULE";
                lstAfterLamRule.Properties.DataSource = dtElPicRuleBind;
                lstAfterLamRule.Text = dtElPicRuleBind.Rows[0]["ELTESTRULE"].ToString();
            }
            else
            {
                lstBefLamRule.Text = "";
            }
        }


        /// <summary>
        /// 在产品变更时对产品信息对应的规则信息进行更新
        /// </summary>
        private void BindDataByPro()
        {
            //产品基本信息的绑定
            BindProBaseInfo();

            //功率分档的绑定
            BindProPS();


            //功率分档子分档的绑定
            BindProPSSub();
            //功率分档花色的绑定
            BindProPSColor();


            //产品等级的绑定
            BindProLevel();
            //绑定衰减设置
            BindProDecay();

            //打印标签衰减系数的绑定
            BindPrintDeacy();
            //绑定打印标签设置
            BindProPrintSet();

            //标签铭牌清单设置fyb
            BindProPShow();
        }

        /// <summary>
        /// 绑定产品到产品基本信息
        /// </summary>
        private void BindProBaseInfo()
        {
            DataRow dr = this.gvProduct.GetFocusedDataRow();
            if (dr == null)
            {
                ClearProBaseInfo();
                return;
            }
            cmbLastTestType.Text = dr["LAST_TEST_TYPE"].ToString();
            tePowerDegree.Text = dr["POWER_DEGREE"].ToString();
            teFullPalletQTY.Text = dr["FULL_PALLET_QTY"].ToString();
            teFullShipment.Text = dr["SHIP_QTY"].ToString();
            teCalibrationCycle.Text = dr["CALIBRATION_CYCLE"].ToString();
            teFixCycle.Text = dr["FIX_CYCLE"].ToString();
            teJunctionBox.Text = dr["JUNCTION_BOX"].ToString();
            teCalibrationType.Text = dr["CALIBRATION_TYPE"].ToString();
            teConstantTemperatureCycle.Text = dr["CONSTANT_TEMPERTATURE_CYCLE"].ToString();
            teModuleTypePrefix.Text = dr["MODULE_TYPE_PREFIX"].ToString();
            teModuleTypeSuffix.Text = dr["MODULE_TYPE_SUFFIX"].ToString();
            // TODO 12312312313
            //txtPortMark.Text = dr["PORT_MARK"].ToString();
        }

        /// <summary>
        /// 清空产品基本信息
        /// </summary>
        private void ClearProBaseInfo()
        {
            cmbLastTestType.Text = string.Empty;
            tePowerDegree.Text = string.Empty;
            teFullPalletQTY.Text = string.Empty;
            teFullShipment.Text = string.Empty;
            teCalibrationCycle.Text = string.Empty;
            teFixCycle.Text = string.Empty;
            teJunctionBox.Text = string.Empty;
            teCalibrationType.Text = string.Empty;
            teConstantTemperatureCycle.Text = string.Empty;
            teModuleTypePrefix.Text = string.Empty;
            teModuleTypeSuffix.Text = string.Empty;
            txtPortMark.Text = string.Empty;
        }

        /// <summary>
        /// 更新产品基本信息
        /// </summary>
        private void EditProBaseInfo(int rowHandle)
        {
            if (rowHandle < 0)
            {
                return;
            }

            if (this.State == ControlState.Edit)
            {
                DataRow dr = this.gvProduct.GetDataRow(rowHandle);
                if (dr != null)
                {
                    dr["LAST_TEST_TYPE"] = Convert.ToString(cmbLastTestType.Text.Trim());
                    if (string.IsNullOrEmpty(tePowerDegree.Text.Trim()))
                    {
                        dr["POWER_DEGREE"] = 0;
                    }
                    else
                    {
                        dr["POWER_DEGREE"] = Convert.ToDecimal(tePowerDegree.Text.Trim());
                    }
                    if (string.IsNullOrEmpty(teFullPalletQTY.Text.Trim()))
                    {
                        dr["FULL_PALLET_QTY"] = 0;
                    }
                    else
                    {
                        dr["FULL_PALLET_QTY"] = Convert.ToInt32(teFullPalletQTY.Text.Trim());
                    }
                    if (string.IsNullOrEmpty(teFullShipment.Text.Trim()))
                    {
                        dr["SHIP_QTY"] = 0;
                    }
                    else
                    {
                        dr["SHIP_QTY"] = Convert.ToInt32(teFullShipment.Text.Trim());
                    }
                    if (string.IsNullOrEmpty(teCalibrationCycle.Text.Trim()))
                    {
                        dr["CALIBRATION_CYCLE"] = 0;
                    }
                    else
                    {
                        dr["CALIBRATION_CYCLE"] = Convert.ToDecimal(teCalibrationCycle.EditValue);
                    }
                    if (string.IsNullOrEmpty(teFixCycle.Text.Trim()))
                    {
                        dr["FIX_CYCLE"] = 0;
                    }
                    else
                    {
                        dr["FIX_CYCLE"] = Convert.ToDecimal(teFixCycle.EditValue);
                    }
                    if (string.IsNullOrEmpty(teFixCycle.Text.Trim()))
                    {
                        dr["FIX_CYCLE"] = 0;
                    }
                    else
                    {
                        dr["FIX_CYCLE"] = Convert.ToDecimal(teFixCycle.EditValue);
                    }
                    if (string.IsNullOrEmpty(teFixCycle.Text.Trim()))
                    {
                        dr["CONSTANT_TEMPERTATURE_CYCLE"] = 0;
                    }
                    else
                    {
                        dr["CONSTANT_TEMPERTATURE_CYCLE"] = Convert.ToDecimal(teConstantTemperatureCycle.Text);
                    }

                    dr["MODULE_TYPE_PREFIX"] = Convert.ToString(teModuleTypePrefix.Text.Trim());
                    dr["MODULE_TYPE_SUFFIX"] = Convert.ToString(teModuleTypeSuffix.Text.Trim());
                    dr["JUNCTION_BOX"] = Convert.ToString(teJunctionBox.Text.Trim());
                    dr["CALIBRATION_TYPE"] = Convert.ToString(teCalibrationType.Text.Trim());
                    //dr["PORT_MARK"] = txtPortMark.Text;
                }
            }
        }

        /// <summary>
        /// 对终检类型、产品等级列进行显示数据绑定
        /// </summary>
        private void BindAttributeData()
        {
            //对终检类型进行绑定
            _dtRuleProperties = _basePowerSetEntity.GetBasicPowerSetEngine_CommonData(string.Empty).Tables[BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtLastTestType = _dtRuleProperties.Clone();
            DataRow[] drs01 = _dtRuleProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.TESTRULE_LASTTESTTYPE));
            foreach (DataRow dr in drs01)
                cmbLastTestType.Properties.Items.Add(dr["Column_Name"].ToString());
            //lueLast_Test_Type.Properties.DisplayMember = "Column_Name";
            //lueLast_Test_Type.Properties.ValueMember = "Column_Name";
            //lueLast_Test_Type.Properties.DataSource = dtLastTestType;



            //对产品等级进行绑定
            DataTable dtTestRule_Grade = _dtRuleProperties.Clone();
            DataRow[] drs05 = _dtRuleProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.PRODUCT_GRADE));
            foreach (DataRow dr in drs05)
                dtTestRule_Grade.ImportRow(dr);
            rpileGrade.DisplayMember = "Column_Name";
            rpileGrade.ValueMember = "Column_code";
            rpileGrade.DataSource = dtTestRule_Grade;
            //repositoryItemGrade

            DataTable dtlueGrid = _basePowerSetEntity.GetPowerSetData(new Hashtable()).Tables[BASE_POWERSET.DATABASE_TABLE_NAME];
            DataView dvGrid = dtlueGrid.DefaultView;
            DataTable dtLueGridBind = dvGrid.ToTable(true, new string[] { BASE_POWERSET.FIELDS_PS_CODE, BASE_POWERSET.FIELDS_PS_SEQ, BASE_POWERSET.FIELDS_POWERSET_KEY });
            //repositoryItemGridlue_powerset_key.View.Columns[BASE_POWERSET.FIELDS_POWERSET_KEY].Visible = false;
            //repositoryItemGridlue_powerset_key.DataSource = dtLueGridBind;
            //repositoryItemGridlue_powerset_key.DisplayMember = BASE_POWERSET.FIELDS_PS_CODE;
            //repositoryItemGridlue_powerset_key.ValueMember = BASE_POWERSET.FIELDS_POWERSET_KEY;

            //repositoryItemGridlue_PowerControl.DataSource = dtLueGridBind;
            //repositoryItemGridlue_PowerControl.DisplayMember = BASE_POWERSET.FIELDS_PS_CODE;
            //repositoryItemGridlue_PowerControl.ValueMember = BASE_POWERSET.FIELDS_POWERSET_KEY;
            DataTable dtDecay = new DecayCoeffiEntity().GetDecayCoeffiData().Tables[BASE_DECAYCOEFFI.DATABASE_TABLE_NAME];
            DataTable dtDecayBind = dtDecay.DefaultView.ToTable(true, new string[] { BASE_DECAYCOEFFI.FIELDS_D_CODE,
                                                                                    BASE_DECAYCOEFFI.FIELDS_D_CODE_DESC,
                                                                                    BASE_DECAYCOEFFI.FIELDS_D_NAME,
                                                                                    BASE_DECAYCOEFFI.FIELDS_DIT,
                                                                                    BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY });

            //repositoryItemGridlue_DecoeffiKey.DisplayMember = BASE_DECAYCOEFFI.FIELDS_D_CODE;
            //repositoryItemGridlue_DecoeffiKey.ValueMember = BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY;
            //repositoryItemGridlue_DecoeffiKey.DataSource = dtDecayBind;

            DataSet dsPrint = _baseTestRuleEntity.GetPrintData();
            DataTable dtPrint = dsPrint.Tables[0];
            if (dtPrint.Columns.Contains(CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER))
                dtPrint.Columns.Remove(CRM_ATTRIBUTE_FIELDS.FIELDS_ITEM_ORDER);
            //riglueLabelData.DisplayMember = "LABEL_NAME";
            //riglueLabelData.ValueMember = "LABEL_ID";
            //riglueLabelData.DataSource = dtPrint;

        }

        /// <summary>
        /// 从产品列表中新增产品
        /// </summary>
        private void sbtnProductAdd_Click(object sender, EventArgs e)
        {

            try
            {
                int rowNum = gvProduct.RowCount;
                DataRow drPro = gvProduct.GetDataRow(rowNum - 1);
                if (!string.IsNullOrEmpty(drPro["PART_NUMBER"].ToString()))
                {
                    DataRow drNew = _dtPorProduct.NewRow();

                    drNew["WORK_ORDER_KEY"] = workOrderKey;
                    drNew["IS_MAIN"] = "N";
                    drNew["VERSION_NO"] = 1;
                    drNew["ORDER_NUMBER"] = teOrderNumber.Text;

                    drNew["CUSTCHECK_TYPE"] = "1";

                    _dtPorProduct.Rows.Add(drNew);
                }
                else
                {
                    //MessageBox.Show("请先对上行产品料号进行选择！", "错误提示");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                }


            }
            catch //(Exception ex) 
            { }
        }

        /// <summary>
        /// 对产品对应的功率分档新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnPowerSetAdd_Click(object sender, EventArgs e)
        {
            ProPowerSetSelectForm proPowerSet = new ProPowerSetSelectForm();

            if (DialogResult.OK == proPowerSet.ShowDialog())
            {
                if (IsExist(partNumber, proPowerSet.proPowerSetKey, _dtProPS, "PART_NUMBER", "POWERSET_KEY"))
                {
                    //MessageBox.Show("该产品对应分档已添加", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string powerMinPower = string.Empty;
                    string powerMaxPower = string.Empty;

                    powerMinPower = proPowerSet.drProPowerSet["P_MIN"].ToString();
                    powerMaxPower = proPowerSet.drProPowerSet["P_MAX"].ToString();

                    DataRow[] drs = _dtPartNumber.Select(string.Format(@"PART_NUMBER = '{0}' 
                                                                        AND MIN_POWER <= '{1}' 
                                                                        AND MAX_POWER >= {2} ", partNumber,
                                                                                               powerMinPower,
                                                                                               powerMaxPower));
                    if (drs.Count() == 1)
                    {
                        PowerSetAdd(proPowerSet.proPowerSetKey, proPowerSet.drProPowerSet, proPowerSet.drsProPowerSetSub, proPowerSet.drsProPowerSetColor);
                    }
                    else
                    {
                        //if (MessageService.AskQuestion("确定要添加不在建议功率范围内的分档么?", "提示"))
                        if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}")))
                        {
                            PowerSetAdd(proPowerSet.proPowerSetKey, proPowerSet.drProPowerSet, proPowerSet.drsProPowerSetSub, proPowerSet.drsProPowerSetColor);
                        }
                    }
                }
            }
        }

        private void PowerSetAdd(string powerSetkey, DataRow drPowerSet, DataRow[] drsPowerSetSub, DataRow[] drsPowerSetColor)
        {
            //获取新增行对应的规则分档Key
            proPowerSetKey = powerSetkey;

            //对功率分档进行新增
            ProPSAdd(drPowerSet);
            //判断功率分档是否存在子分档存在的话进行子分档新增
            if (drsPowerSetSub.Count() > 0)
            {
                foreach (DataRow drSub in drsPowerSetSub)
                    ProPSSubAdd(drSub);
            }
            //判断功率分档是否存在花色存在的话进行花色新增
            if (drsPowerSetColor.Count() > 0)
            {
                foreach (DataRow drColor in drsPowerSetColor)
                    ProPSColorAdd(drColor);
            }
            //对功率分档进行绑定
            BindProPS();

            //把新增的行设定为聚焦行
            if (!string.IsNullOrEmpty(proPowerSetKey))
            {
                for (int i = 0; i < gvPowerSet.RowCount; i++)
                {
                    string sk = Convert.ToString(((DataRowView)(this.gvPowerSet.GetRow(i))).Row["POWERSET_KEY"]);
                    if (proPowerSetKey.Equals(sk.Trim()))
                    {
                        this.gvPowerSet.FocusedRowHandle = i;
                        break;
                    }
                }
            }

            //对功率分档子分档进行绑定
            BindProPSSub();
            //对功率分档花色进行绑定
            BindProPSColor();
        }

        /// <summary>
        /// 对产品等级进行新增
        /// </summary>
        private void sbtnProLevelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drNew = _dtProLevel.NewRow();

                drNew["WORK_ORDER_KEY"] = workOrderKey;
                drNew["PART_NUMBER"] = partNumber;

                _dtProLevel.Rows.Add(drNew);
            }
            catch //(Exception ex) 
            { }
        }

        /// <summary>
        /// 对衰减设置进行新增
        /// </summary>
        private void sbtnDecayAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drNew = _dtProDecay.NewRow();

                drNew["WORK_ORDER_KEY"] = workOrderKey;
                drNew["PART_NUMBER"] = partNumber;
                drNew["DECAY_NEWKEY"] = CommonUtils.GenerateNewKey(0);

                _dtProDecay.Rows.Add(drNew);
            }
            catch //(Exception ex) 
            { }
        }

        /// <summary>
        /// 对打印设置进行新增
        /// </summary>
        private void sbtnLablePrintAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drNew = _dtProPrintSet.NewRow();

                drNew["WORK_ORDER_KEY"] = workOrderKey;
                drNew["PART_NUMBER"] = partNumber;
                drNew["PRINTSET_KEY"] = CommonUtils.GenerateNewKey(0);

                _dtProPrintSet.Rows.Add(drNew);
            }
            catch //(Exception ex) 
            { }
        }

        /// <summary>
        /// 新增功率子分档
        /// </summary>
        private void sbtnPwoerSetSubAdd_Click(object sender, EventArgs e)
        {
            ProPowerSetSubForm proPSSub = new ProPowerSetSubForm();

            if (DialogResult.OK == proPSSub.ShowDialog())
            {
                try
                {
                    DataRow drNew = _dtProPSSub.NewRow();

                    drNew["WORK_ORDER_KEY"] = workOrderKey;
                    drNew["PART_NUMBER"] = partNumber;
                    drNew["POWERSET_KEY"] = proPowerSetKey;
                    drNew["VERSION_NO"] = 1;
                    drNew["PS_SUB_CODE"] = proPSSub.powerSubCode;
                    drNew["POWERLEVEL"] = proPSSub.powerSubLevel;
                    drNew["P_DTL_MIN"] = proPSSub.powerSubPmin;
                    drNew["P_DTL_MAX"] = proPSSub.powerSubPmax;

                    _dtProPSSub.Rows.Add(drNew);
                }
                catch //(Exception ex) 
                { }
            }
        }

        /// <summary>
        /// 新增功率花色
        /// </summary>
        private void sbtnPowerSetColorAdd_Click(object sender, EventArgs e)
        {
            ProPowerSetColorForm proPSColor = new ProPowerSetColorForm();

            if (DialogResult.OK == proPSColor.ShowDialog())
            {
                try
                {
                    DataRow drNew = _dtProPSColor.NewRow();

                    drNew["WORK_ORDER_KEY"] = workOrderKey;
                    drNew["PART_NUMBER"] = partNumber;
                    drNew["POWERSET_KEY"] = proPowerSetKey;
                    drNew["VERSION_NO"] = 1;
                    drNew["COLOR_CODE"] = proPSColor.colorCode;
                    drNew["COLOR_NAME"] = proPSColor.colorName;
                    drNew["ARTICNO"] = proPSColor.articleNo;
                    drNew["DESCRIPTION"] = proPSColor.colorDes;

                    _dtProPSColor.Rows.Add(drNew);
                }
                catch //(Exception ex) 
                { }
            }
        }


        /// <summary>
        /// 产品视图选择行变更后
        /// 对基本信息的修改进行保存
        /// 重新加载产品基本信息、产品等级、衰减设置、打印标签设置、功率分档
        /// </summary> 
        private void gvProduct_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

            if (gvProduct.FocusedRowHandle > -1)
            {
                //判断是否已删除原选中行
                //if (IsExist(partNumber, _dtPorProduct, "PART_NUMBER"))
                //{
                //判断是否为第一次加载若是第一次加载，不对行信息进行更新，否则对原行信息进行更新
                //if (!isFirstLoad)
                //{
                //对之前选择的产品基本信息的修改进行保存
                //EditProBaseInfo();
                //}
                //}
                EditProBaseInfo(e.PrevFocusedRowHandle);
                //获取选中行的产品Key和产品料号
                proKey = gvProduct.GetRowCellValue(gvProduct.FocusedRowHandle, "PRODUCT_KEY").ToString();
                partNumber = gvProduct.GetRowCellValue(gvProduct.FocusedRowHandle, "PART_NUMBER").ToString();
                //对选中行的产品对应的信息进行绑定
                BindDataByPro();

                //对选中行的产品料号对应的等级进行处理
                GetPartGradeArray();

                //isFirstLoad = false;
            }
        }

        /// <summary>
        /// 在功率分档表选择行改变时对对应的子分档信息进行更新
        /// </summary> 
        private void gvPowerSet_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (gvPowerSet.FocusedRowHandle > -1)
            {
                //获取选择的功率分档的Key值
                proPowerSetKey = gvPowerSet.GetRowCellValue(gvPowerSet.FocusedRowHandle, "POWERSET_KEY").ToString();

                //对功率分档子分档进行绑定
                BindProPSSub();
                //对功率分档花色进行绑定
                BindProPSColor();
            }

        }
        #endregion

        #region //工单属性

        /// <summary>
        /// 获取工单属性
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        private void GetWorkOrderAttrData(string workorderNumber)
        {
            DataSet dsBindAll = _workOrdersEntity.GetWorkOrderAttrParamByOrderNumber(workorderNumber);

            _dtWorkOrderAttr = dsBindAll.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME];

            SortGvData(_dtWorkOrderAttr);
        }

        /// <summary>
        /// 绑定工单属性信息
        /// </summary>
        /// <param name="dtBind">工单属性集合</param>
        private void SortGvData(DataTable dtBind)
        {
            this.gcOrderProperties.MainView = gvOrderProperties;
            this.gcOrderProperties.DataSource = null;
            this.gvOrderProperties.FocusedRowHandle = -1;
            this.gcOrderProperties.DataSource = dtBind;
        }

        /// <summary>
        /// 在添加新的工单产品的时候对IV和EL图片的校验进行默认属性设置
        /// </summary>
        private void AddELIVCheck()
        {
            try
            {
                string[] l_s01 = new string[] { "PIC_ADDRESS", "PIC_DATE_FORMAT", "PIC_FACTORY_CODE", "PIC_TYPE", "PIC_ADDRESS_NAME", "PIC_ISCHECK" };
                string category01 = "Uda_Pic_Address";
                DataTable dtPicAddress = BaseData.Get(l_s01, category01);

                DataTable dtAttr = gcOrderProperties.DataSource as DataTable;
                DataRow drNew = dtAttr.NewRow();

                // 添加工单属性 对EL图片检查的判断
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY] = "6a33af74-f1a7-465d-9fd5-1accf73cab1d-000";
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME] = "IsCheckEL";
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_TYPE] = "0";

                DataRow drEL = dtPicAddress.Select(string.Format("PIC_FACTORY_CODE='{0}' AND PIC_TYPE='{1}' ", factoryCode, "EL"))[0];
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = Convert.ToString(drEL["PIC_ISCHECK"]);
                dtAttr.Rows.Add(drNew);

                // 添加工单属性 对IV图片检查的判断
                drNew = dtAttr.NewRow();
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY] = "ab0cf9a6-af2c-49ae-9197-de96a9a0d0eb-000";
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME] = "IsCheckIV";
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_TYPE] = "0";


                DataRow drIV = dtPicAddress.Select(string.Format("PIC_FACTORY_CODE='{0}' AND PIC_TYPE='{1}' ", factoryCode, "IV"))[0];
                string picIsCheckIV = Convert.ToString(drIV["PIC_ISCHECK"]);
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = picIsCheckIV;
                bool bPicIsCheckIV = false;
                if (bool.TryParse(picIsCheckIV, out bPicIsCheckIV) && bPicIsCheckIV == true)
                {
                    //判断工单类型
                    string workOrderType = workOrderNumber.Substring(0, 2);
                    l_s01 = new string[] { "Work_Order_No", "CheckIV" };
                    category01 = "Uda_work_type";
                    List<KeyValuePair<string, string>> lstCondition = new List<KeyValuePair<string, string>>();
                    lstCondition.Add(new KeyValuePair<string, string>("Work_Order_No", workOrderType));
                    DataTable dtIsCheckIV = BaseData.GetBasicDataByCondition(l_s01, category01, lstCondition);
                    if (dtIsCheckIV.Rows.Count > 0)
                    {//指定工单类型是否检查IV曲线
                        string sCheckIV = Convert.ToString(dtIsCheckIV.Rows[0]["CheckIV"]);
                        bool isCheckIV = true;
                        if (!bool.TryParse(sCheckIV, out isCheckIV))
                        {
                            isCheckIV = true;
                        }
                        drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = isCheckIV.ToString();
                    }
                }
                dtAttr.Rows.Add(drNew);
                SortGvData(dtAttr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 添加工单属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnOrderPropertiesAdd_Click(object sender, EventArgs e)
        {
            WorkOrderAttrSetting woas = new WorkOrderAttrSetting();
            woas.sType = "workattr";
            DataTable dt01 = gcOrderProperties.DataSource as DataTable;
            DataTable dtTemp = dt01.Clone();
            DataRow[] drs = dt01.Select(string.Format(@"ATTRIBUTE_TYPE='0'"));
            foreach (DataRow dr in drs)
                dtTemp.ImportRow(dr);

            woas.dtCommon = dtTemp;
            //((DataView)gvWoAttr.DataSource).Table;
            if (DialogResult.OK == woas.ShowDialog())
            {
                string attribute_name = string.Empty;
                DataTable dtAttr = gcOrderProperties.DataSource as DataTable;
                DataRow drNew = dtAttr.NewRow();
                attribute_name = Convert.ToString(woas.drCommon[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME]);
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY] = woas.drCommon[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY];
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME] = attribute_name;
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = CommonUtils.GenerateNewKey(0);
                if (attribute_name.Equals(WORKORDER_SETTING_ATTRIBUTE.IsMustInputModuleColorByCleanOpt))
                {
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "true";
                }
                else if (attribute_name.Equals(WORKORDER_SETTING_ATTRIBUTE.IsReceiveMixWosByPackage))
                {
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "false";
                }
                else if (attribute_name.Equals(WORKORDER_SETTING_ATTRIBUTE.IsExperimentWo))
                {
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "true";
                }
                else if (attribute_name.Equals("IsCheckEL"))
                {
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "true";
                }
                else if (attribute_name.Equals("IsCheckIV"))
                {
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "false";
                }
                if (attribute_name.Equals("IsMustTypeinTestParams"))
                {
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = "false";
                }
                if (attribute_name.Equals("PalletAddLetter"))
                {
                    string[] l_s = new string[] { "PALLETNO_ADD", "NO" };
                    string category = "Basic_AnNengPalletNo";
                    System.Data.DataTable dt_PalletNo = BaseData.Get(l_s, category);
                    DataRow[] drPalletNo = dt_PalletNo.Select(string.Format("NO='{0}'", "1"));
                    string PalletNoADD = drPalletNo[0]["PALLETNO_ADD"].ToString();

                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = PalletNoADD;
                }
                else
                {
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE] = string.Empty;
                }
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_TYPE] = "0";
                dtAttr.Rows.Add(drNew);
                SortGvData(dtAttr);
            }
        }

        /// <summary>
        /// 添加物料属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnMaterialsPropertiesAdd_Click(object sender, EventArgs e)
        {
            WorkOrderAttrSetting woas = new WorkOrderAttrSetting();
            woas.sType = "workparams";
            DataTable dt01 = gcOrderProperties.DataSource as DataTable;
            DataTable dtTemp = dt01.Clone();
            DataRow[] drs = dt01.Select(string.Format(@"ATTRIBUTE_TYPE='1'"));
            foreach (DataRow dr in drs)
                dtTemp.ImportRow(dr);

            woas.dtCommon = dtTemp;

            if (DialogResult.OK == woas.ShowDialog())
            {
                DataTable dtAttr = gcOrderProperties.DataSource as DataTable;
                DataRow drNew = dtAttr.NewRow();
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY] = woas.drCommon[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY];
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME] = woas.drCommon[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME];
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_TYPE] = "1";
                dtAttr.Rows.Add(drNew);
                SortGvData(dtAttr);
            }
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnOrderPropertiesDelete_Click(object sender, EventArgs e)
        {
            if (gvOrderProperties.FocusedRowHandle < 0) return;


            DataTable dtAttr = gcOrderProperties.DataSource as DataTable;
            DataRow dr = gvOrderProperties.GetFocusedDataRow();
            Hashtable hstable = new Hashtable();

            hstable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY];

            DataSet dsReturn = _workOrdersEntity.DelWorkAttrDataBy2Key(hstable);
            if (!string.IsNullOrEmpty(_workOrdersEntity.ErrorMsg))
            {
                MessageService.ShowMessage(_workOrdersEntity.ErrorMsg);
                return;
            }
            else
            {
                //MessageService.ShowMessage("删除成功!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0006}"));
                dtAttr.Rows.Remove(dr);
                this.gcOrderProperties.MainView = gvOrderProperties;
                this.gcOrderProperties.DataSource = null;
                this.gvOrderProperties.FocusedRowHandle = -1;
                this.gcOrderProperties.DataSource = dtAttr;
            }
        }

        #endregion

        /// <summary>
        /// 移除选中的产品
        /// 并把与产品相关联的功率分档、功率分档子分档、功率分档花色、产品等级、衰减设置、打印标签设置进行移除
        /// </summary>
        private void sbtnProductDelete_Click(object sender, EventArgs e)
        {
            if (this.gvProduct.FocusedRowHandle > -1)
            {
                //获取选中行是否为主产品
                string isMain = gvProduct.GetFocusedDataRow()["IS_MAIN"].ToString();

                //判断选择删除行是否为主产品主产品不可删除
                if (isMain == "Y")
                {
                    //MessageBox.Show("主产品不可删除！", "错误提示");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));

                }
                else
                {
                    //通过工单Key和产品Key把产品对应的功率分档、产品等级、衰减设置、打印标签设置进行移除
                    RemoveByProKey();

                    //获取选中的行并进行移除
                    DataRow dr = this.gvProduct.GetFocusedDataRow();
                    dr.Delete();

                    BindProBaseInfo();
                }
            }
        }

        /// <summary>
        /// 移除选中的功率分档并对对应的子分档及花色进行移除
        /// </summary>
        private void sbtnPowerSetDelete_Click(object sender, EventArgs e)
        {
            if (this.gvPowerSet.FocusedRowHandle > -1)
            {
                //通过产品Key和功率Key对功率分档对应的子分档、花色进行移除
                RemoveByProAndPowerKey();

                //获取选中的行并进行移除
                DataRow dr = this.gvPowerSet.GetFocusedDataRow();
                dr.Delete();

                //判断
                if (this.gvPowerSet.FocusedRowHandle > -1)
                {
                    //获取选择的功率分档的Key值
                    proPowerSetKey = gvPowerSet.GetRowCellValue(gvPowerSet.FocusedRowHandle, "POWERSET_KEY").ToString();
                }
                //对功率分档子分档绑定进行更新
                BindProPSSub();
                //对功率分档花色绑定更新
                BindProPSColor();
            }
        }

        /// <summary>
        /// 移除选中的子分档
        /// </summary>
        private void sbtnPowerSetSubDelete_Click(object sender, EventArgs e)
        {
            if (this.gvPowerSetSub.FocusedRowHandle > -1)
            {
                DataRow dr = this.gvPowerSetSub.GetFocusedDataRow();
                dr.Delete();
            }
        }

        /// <summary>
        /// 移除选中的花色
        /// </summary>
        private void sbtnPowerSetColorDelete_Click(object sender, EventArgs e)
        {
            if (this.gvPowerSetColor.FocusedRowHandle > -1)
            {
                DataRow dr = this.gvPowerSetColor.GetFocusedDataRow();
                dr.Delete();
            }
        }

        /// <summary>
        /// 移除选中的等级
        /// </summary>
        private void sbtnProLevelDelete_Click(object sender, EventArgs e)
        {
            if (this.gvProLevel.FocusedRowHandle > -1)
            {
                DataRow dr = this.gvProLevel.GetFocusedDataRow();
                dr.Delete();
            }
        }

        /// <summary>
        /// 移除选中的衰减设置
        /// </summary>
        private void sbtnDecayDelete_Click(object sender, EventArgs e)
        {
            if (this.gvDecay.FocusedRowHandle > -1)
            {
                DataRow dr = this.gvDecay.GetFocusedDataRow();
                for (int i = 0; i < _dtProPrintSet.Rows.Count; i++)
                {
                    if (_dtProPrintSet.Rows[i].RowState != DataRowState.Deleted && _dtProPrintSet.Rows[i].RowState != DataRowState.Detached)
                    {
                        if (dr["DECAY_NEWKEY"].ToString() == _dtProPrintSet.Rows[i]["DECAY_KEY"].ToString())
                        {
                            _dtProPrintSet.Rows[i]["DECAY_KEY"] = "";
                            _dtProPrintSet.Rows[i]["MinMaxPower"] = "";
                        }
                    }
                }

                dr.Delete();
            }
        }

        /// <summary>
        /// 移除选中的打印标签设置
        /// </summary>
        private void sbtnLablePrintDelete_Click(object sender, EventArgs e)
        {
            if (this.gvProPrintSet.FocusedRowHandle > -1)
            {
                DataRow dr = this.gvProPrintSet.GetFocusedDataRow();
                dr.Delete();
            }
        }


        /// <summary>
        /// 当产品视图信息改变的时候触发
        /// </summary>
        private void gvProduct_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //当产品变更时触发
            if (gvProduct.FocusedColumn.FieldName == "PRODUCT_KEY")
            {
                proKey = Convert.ToString(gvProduct.GetFocusedRowCellValue("PRODUCT_KEY"));

                DataRow[] drs = _dtProList.Select(string.Format(" PRODUCT_KEY = '{0}'", proKey));
                foreach (DataRow dr in drs)
                {

                    //通过产品关联的测试规则查询对应的规则信息
                    Hashtable hashTable = new Hashtable();
                    if (!string.IsNullOrEmpty(dr["PRO_TEST_RULE"].ToString()))
                        hashTable[BASE_TESTRULE.FIELDS_TESTRULE_CODE] = dr["PRO_TEST_RULE"].ToString();

                    DataSet dsProTestRule = _baseTestRuleEntity.GetTestRuleMainData(hashTable);

                    //对测试规则主键进行更新
                    testRuleKey = dsProTestRule.Tables[0].Rows[0]["TESTRULE_KEY"].ToString();

                    //把更新的产品信息和测试规则对应的信息更新到产品Table (_dtPorProduct) 中
                    ProProductUpdate(dr, dsProTestRule.Tables[0].Rows[0]);

                    //判断是否为主产品产品的变更，如果是对默认工艺流程进行设定
                    if (gvProduct.GetFocusedDataRow()["IS_MAIN"].ToString() == "Y")
                    {
                        //判断产品信息中是否存在设定的默认工序信息
                        if (!string.IsNullOrEmpty(dr[POR_PRODUCT.FIELDS_ROUTE_ENTERPRISE_VER_KEY].ToString())
                            && !string.IsNullOrEmpty(dr[POR_PRODUCT.FIELDS_ENTERPRISE_NAME].ToString())
                            && !string.IsNullOrEmpty(dr[POR_PRODUCT.FIELDS_ROUTE_ROUTE_VER_KEY].ToString())
                            && !string.IsNullOrEmpty(dr[POR_PRODUCT.FIELDS_ROUTE_NAME].ToString())
                            && !string.IsNullOrEmpty(dr[POR_PRODUCT.FIELDS_ROUTE_STEP_KEY].ToString())
                            && !string.IsNullOrEmpty(dr[POR_PRODUCT.FIELDS_ROUTE_STEP_NAME].ToString()))
                        {
                            this.beRouteEnterprise.Tag = dr[POR_PRODUCT.FIELDS_ROUTE_ENTERPRISE_VER_KEY].ToString();
                            this.beRouteEnterprise.Text = dr[POR_PRODUCT.FIELDS_ENTERPRISE_NAME].ToString();
                            this.teRouteName.Tag = dr[POR_PRODUCT.FIELDS_ROUTE_ROUTE_VER_KEY].ToString();
                            this.teRouteName.Text = dr[POR_PRODUCT.FIELDS_ROUTE_NAME].ToString();
                            this.teStepName.Tag = dr[POR_PRODUCT.FIELDS_ROUTE_STEP_KEY].ToString();
                            this.teStepName.Text = dr[POR_PRODUCT.FIELDS_ROUTE_STEP_NAME].ToString();

                            //判断是否存在存放工艺流程的表，存在把主产品对应的信息存入
                            if (_dtWorkOrderRoute != null
                                && _dtWorkOrderRoute.Rows.Count == 1)
                            {
                                _dtWorkOrderRoute.Rows[0]["ROUTE_ENTERPRISE_VER_KEY"] = this.beRouteEnterprise.Tag;
                                _dtWorkOrderRoute.Rows[0]["ENTERPRISE_NAME"] = this.beRouteEnterprise.Text;
                                _dtWorkOrderRoute.Rows[0]["ROUTE_ROUTE_VER_KEY"] = this.teRouteName.Tag;
                                _dtWorkOrderRoute.Rows[0]["ROUTE_NAME"] = this.teRouteName.Text;
                                _dtWorkOrderRoute.Rows[0]["ROUTE_STEP_KEY"] = this.teStepName.Tag;
                                _dtWorkOrderRoute.Rows[0]["ROUTE_STEP_NAME"] = this.teStepName.Text;
                            }
                        }

                        //add by chao.pang  如果是主产品的变更，这自动变更主产品对应产品型号带出的CTM管控上下限
                        BindCtmInf(proKey, workOrderKey);
                    }


                    //对该产品料号对应的规则信息进行清除
                    RemoveByProKey();

                    //绑定产品基本信息到对应栏位
                    BindProBaseInfo();

                    //获取规则对应的产品等级、衰减设置、打印标签设置
                    GetRuleData();

                    //通过规则对应的分档代码获取对应的分档信息并进行绑定
                    GetPowerSetByPowerSetCode(dsProTestRule.Tables[0].Rows[0]["PS_CODE"].ToString());

                    //对打印设置的衰减序号信息进行绑定
                    BindPrintDeacy();

                }

                //主产品代码内容改变时，清空打印规则--ruhu.yu 2017.02.23
                if (gvProduct.FocusedRowHandle == 0)
                {
                    gcRuleDetail.DataSource = null;
                    lupPrintRule.EditValue = "";
                }
            }
        }

        /// <summary>
        /// 在选择产品代码进行选择下拉关闭时触发
        /// 并对产品代码进行赋值
        /// </summary>
        private void rpgleProList_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            if (this.gvProduct.State == GridState.Editing && this.gvProduct.IsEditorFocused)
            {

                string currentProductKey = Convert.ToString(this.gvProduct.EditingValue);
                string newProductKey = Convert.ToString(e.Value);
                if (currentProductKey != newProductKey)
                {
                    this.gvProduct.SetFocusedRowCellValue(this.gvProduct.FocusedColumn, newProductKey);
                    this.gvProduct.UpdateCurrentRow();
                }
            }
        }



        /// <summary>
        /// 在点击视图进行编辑时出发
        /// </summary>
        private void gvProduct_ShowingEditor(object sender, CancelEventArgs e)
        {
            int index = this.gvProduct.FocusedRowHandle;

            //判断是否有效行
            if (index < 0)
            {
                return;
            }

            string isMain = gvProduct.GetFocusedDataRow()["IS_MAIN"].ToString();
            string proKey = Convert.ToString(gvProduct.GetFocusedRowCellValue("PRODUCT_KEY"));
            //判断修改料号是否为主产品料号如果是提示不能修改
            if ((isMain == "Y" || !string.IsNullOrEmpty(proKey))
                && gvProduct.FocusedColumn.FieldName == "PART_NUMBER")
            {
                e.Cancel = true;
                return;
            }

            //在选择产品时产品料号进行判断
            DataRow dr = this.gvProduct.GetFocusedDataRow();
            string partNumber = Convert.ToString(dr["PART_NUMBER"]);
            if (this.gvProduct.FocusedColumn == this.gdProCode && string.IsNullOrEmpty(partNumber))
            {
                //MessageBox.Show("请先选择对应的产品料号", "错误提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.gvProduct.FocusedColumn = this.gdPartNumber;
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 对产品料号是否重复进行判断
        /// </summary>
        private void rpglePartNumber_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            if (this.gvProduct.State == GridState.Editing && this.gvProduct.IsEditorFocused)
            {
                int rowNum = _dtPorProduct.Rows.Count;

                string newPartNumber = Convert.ToString(e.Value);
                string currenPartNumber = Convert.ToString(this.gvProduct.EditingValue);
                if (newPartNumber != currenPartNumber)
                {
                    for (int i = 0; i < rowNum - 1; i++)
                    {
                        if (_dtPorProduct.Rows[i].RowState != DataRowState.Deleted && _dtPorProduct.Rows[i].RowState != DataRowState.Detached)
                        {
                            if (newPartNumber == _dtPorProduct.Rows[i]["PART_NUMBER"].ToString())
                            {

                                this.gvProduct.SetFocusedRowCellValue("PART_NUMBER", "");
                                //MessageBox.Show("产品料号不能重复！", "错误提示");
                                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0009}"), StringParser.Parse("${res:Global.SystemInfo}"));
                                e.Value = null;
                                return;
                            }
                        }
                    }

                    //对变更的PartNumber进行更新
                    partNumber = newPartNumber;

                    //对产品等级List进行遍历
                    GetPartGradeArray();

                    DataTable dtPartNumberArray = (DataTable)rpglePartNumber.DataSource;
                    int partRowNum = dtPartNumberArray.Rows.Count;

                    //对序号进行更新
                    for (int i = 0; i < partRowNum; i++)
                    {
                        if (newPartNumber == dtPartNumberArray.Rows[i]["PART_NUMBER"].ToString())
                        {
                            this.gvProduct.SetFocusedRowCellValue("ITEM_NO", dtPartNumberArray.Rows[i]["ITEM_NO"]);
                            this.gvProduct.SetFocusedRowCellValue("PART_DESC", dtPartNumberArray.Rows[i]["PART_DESC"]);

                        }
                    }

                    this.gvProduct.SetFocusedRowCellValue(this.gvProduct.FocusedColumn, newPartNumber);
                    this.gvProduct.UpdateCurrentRow();

                }
            }
        }

        private void rpileGrade_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            string newPartGrade = Convert.ToString(e.Value);
            string currenPartGrade = Convert.ToString(this.gvProduct.EditingValue);

            if (newPartGrade != currenPartGrade)
            {
                if (!partGrades.Contains(newPartGrade))
                {
                    //if (MessageService.AskQuestion("确定要添加不在建议等级范围内的等级么?", "提示"))
                    if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}")))
                    {
                        this.gvProLevel.SetFocusedRowCellValue(this.gvProduct.FocusedColumn, newPartGrade);
                        this.gvProLevel.UpdateCurrentRow();
                    }
                    else
                    {
                        e.Value = null;
                    }
                }
                else
                {
                    this.gvProLevel.SetFocusedRowCellValue(this.gvProduct.FocusedColumn, newPartGrade);
                    this.gvProLevel.UpdateCurrentRow();
                }

            }
        }

        //判断要保存的信息是否符合要求
        private bool IsValidData()
        {
            bool bl_bak = true;


            if (string.IsNullOrEmpty(teOrderNumber.Text))
            {
                //MessageService.ShowMessage("工单号不能为空!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0011}"));
                return false;
            }

            //对产品信息进行判断
            for (int i = 0; i < gvProduct.RowCount; i++)
            {
                if (_dtPorProduct.Rows[i].RowState != DataRowState.Deleted && _dtPorProduct.Rows[i].RowState != DataRowState.Detached)
                {
                    string proPartNumber = _dtPorProduct.Rows[i]["PART_NUMBER"].ToString();
                    string proProductCode = _dtPorProduct.Rows[i]["PRODUCT_CODE"].ToString();

                    string cmbLastTestType = _dtPorProduct.Rows[i]["LAST_TEST_TYPE"].ToString();
                    string tePowerDegree = _dtPorProduct.Rows[i]["POWER_DEGREE"].ToString();
                    string teFullPalletQTY = _dtPorProduct.Rows[i]["FULL_PALLET_QTY"].ToString();
                    string teFullShipment = _dtPorProduct.Rows[i]["SHIP_QTY"].ToString();
                    string teCalibrationCycle = _dtPorProduct.Rows[i]["CALIBRATION_CYCLE"].ToString();
                    string teFixCycle = _dtPorProduct.Rows[i]["FIX_CYCLE"].ToString();
                    string teJunctionBox = _dtPorProduct.Rows[i]["JUNCTION_BOX"].ToString();
                    string teCalibrationType = _dtPorProduct.Rows[i]["CALIBRATION_TYPE"].ToString();

                    if (string.IsNullOrEmpty(proPartNumber))
                    {
                        //MessageService.ShowMessage("产品料号不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0012}"));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(proProductCode))
                    {
                        //MessageService.ShowMessage("产品代码不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0013}"));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(cmbLastTestType))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【终检类型】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0014}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(tePowerDegree))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【功率精度】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0015}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(teFullPalletQTY))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【满托数量】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0016}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(teFullShipment))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【满柜数量】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0017}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(teCalibrationCycle))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【校准周期】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0018}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(teFixCycle))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【固化周期】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0019}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(teJunctionBox))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【接线盒】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0020}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(teCalibrationType))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【校准版类型】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0021}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }
                }
            }


            for (int i = 0; i < _dtProLevel.Rows.Count; i++)
            {
                if (_dtProLevel.Rows[i].RowState != DataRowState.Deleted && _dtProLevel.Rows[i].RowState != DataRowState.Detached)
                {
                    string proPartNumber = _dtProLevel.Rows[i]["PART_NUMBER"].ToString();
                    string proGrade = _dtProLevel.Rows[i]["GRADE"].ToString();
                    string proLevelSeq = _dtProLevel.Rows[i]["PROLEVEL_SEQ"].ToString();
                    if (string.IsNullOrEmpty(proLevelSeq))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【产品等级序号】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0022}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }

                    if (string.IsNullOrEmpty(proGrade))
                    {
                        //MessageService.ShowMessage(string.Format("产品料号【{0}】，对应的【产品等级等级】不能为空,请确认!", proPartNumber));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0023}"), proPartNumber));
                        bl_bak = false;
                        break;
                    }
                    else
                    {
                        int qty = 0;
                        for (int j = 0; j < _dtProLevel.Rows.Count; j++)
                        {
                            if (_dtProLevel.Rows[j].RowState != DataRowState.Deleted && _dtProLevel.Rows[j].RowState != DataRowState.Detached)
                            {
                                string grade = _dtProLevel.Rows[j]["GRADE"].ToString();
                                string gradePartNumber = _dtProLevel.Rows[j]["PART_NUMBER"].ToString();
                                if (gradePartNumber.Equals(proPartNumber))
                                {
                                    if (grade.Equals(proGrade))
                                        qty++;
                                }
                            }
                        }
                        if (qty > 1)
                        {
                            //MessageService.ShowMessage(string.Format("产品【{0}】对应的【产品等级】不能设定重复!", proPartNumber), "提示");
                            MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0024}"), proPartNumber), "提示");
                            bl_bak = false;
                            break;
                        }
                    }
                }
            }

            //判断同一产品同一分档下花色是否重复
            for (int i = 0; i < _dtProPSColor.Rows.Count; i++)
            {
                if (_dtProPSColor.Rows[i].RowState != DataRowState.Deleted && _dtProPSColor.Rows[i].RowState != DataRowState.Detached)
                {
                    string proPartNumber = _dtProPSColor.Rows[i]["PART_NUMBER"].ToString();
                    string proPSKey = _dtProPSColor.Rows[i]["POWERSET_KEY"].ToString();
                    string proPSColor = _dtProPSColor.Rows[i]["COLOR_CODE"].ToString();

                    int qty = 0;
                    for (int j = 0; j < _dtProPSColor.Rows.Count; j++)
                    {
                        if (_dtProPSColor.Rows[j].RowState != DataRowState.Deleted && _dtProPSColor.Rows[j].RowState != DataRowState.Detached)
                        {
                            string colorPartNumber = _dtProPSColor.Rows[j]["PART_NUMBER"].ToString();
                            string colorPSKey = _dtProPSColor.Rows[j]["POWERSET_KEY"].ToString();
                            string colorCode = _dtProPSColor.Rows[j]["COLOR_CODE"].ToString();
                            if (proPartNumber.Equals(colorPartNumber) && proPSKey.Equals(colorPSKey) && proPSColor.Equals(colorCode))
                            {
                                qty++;
                            }
                        }
                    }
                    if (qty > 1)
                    {
                        //MessageService.ShowMessage(string.Format("产品【{0}】功率下的花色不能设定重复!", proPartNumber), "提示");
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0025}"), proPartNumber), StringParser.Parse("${res:Global.SystemInfo}"));
                        bl_bak = false;
                        break;
                    }
                }
            }

            //判断同一产品同一分档下子分档是否重复
            for (int i = 0; i < _dtProPSSub.Rows.Count; i++)
            {
                if (_dtProPSSub.Rows[i].RowState != DataRowState.Deleted && _dtProPSSub.Rows[i].RowState != DataRowState.Detached)
                {
                    string proPartNumber = _dtProPSSub.Rows[i]["PART_NUMBER"].ToString();
                    string proPSKey = _dtProPSSub.Rows[i]["POWERSET_KEY"].ToString();
                    string proPSSub = _dtProPSSub.Rows[i]["PS_SUB_CODE"].ToString();

                    int qty = 0;
                    for (int j = 0; j < _dtProPSSub.Rows.Count; j++)
                    {
                        if (_dtProPSSub.Rows[j].RowState != DataRowState.Deleted && _dtProPSSub.Rows[j].RowState != DataRowState.Detached)
                        {
                            string subPartNumber = _dtProPSSub.Rows[j]["PART_NUMBER"].ToString();
                            string subPSKey = _dtProPSSub.Rows[j]["POWERSET_KEY"].ToString();
                            string subCode = _dtProPSSub.Rows[j]["PS_SUB_CODE"].ToString();
                            if (proPartNumber.Equals(subPartNumber) && proPSKey.Equals(subPSKey) && proPSSub.Equals(subCode))
                            {
                                qty++;
                            }
                        }
                    }
                    if (qty > 1)
                    {
                        //MessageService.ShowMessage(string.Format("产品【{0}】功率下的子分档不能设定重复!", proPartNumber), "提示");
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0026}"), proPartNumber), StringParser.Parse("${res:Global.SystemInfo}"));
                        bl_bak = false;
                        break;
                    }
                }
            }


            for (int i = 0; i < gvOrderProperties.RowCount; i++)
            {
                string c1 = gvOrderProperties.GetRowCellValue(i, POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME).ToString();
                string c2 = gvOrderProperties.GetRowCellValue(i, POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE).ToString();
                if (string.IsNullOrEmpty(c1))
                {
                    //MessageService.ShowMessage("工单属性名不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0027}"));
                    bl_bak = false;
                    break;
                }

                if (string.IsNullOrEmpty(c2))
                {
                    //MessageService.ShowMessage("工单属性值/SapNo不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0028}"));
                    bl_bak = false;
                    break;
                }

                if (c1.Equals(WORKORDER_SETTING_ATTRIBUTE.IsMustInputModuleColorByCleanOpt))
                {
                    if (!c2.ToLower().Equals("true") && !c2.ToLower().Equals("false"))
                    {
                        //MessageService.ShowMessage(string.Format("工单属性名称【{0}】值，必须为【true/false】,请确认!", c1));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0029}"), c1));
                        bl_bak = false;
                        break;
                    }
                }
                if (c1.Equals(WORKORDER_SETTING_ATTRIBUTE.IsReceiveMixWosByPackage))
                {
                    if (!c2.ToLower().Equals("true") && !c2.ToLower().Equals("false"))
                    {
                        //MessageService.ShowMessage(string.Format("工单属性名称【{0}】值，必须为【true/false】,请确认!", c1));
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0030}"), c1));
                        bl_bak = false;
                        break;
                    }
                }
            }

            //判断是否需要OEM工单的信息的设定
            if (ceIsOEMWorkOrder.Checked == true)
            {
                //判断之前是否有设定OEM信息
                if (_dtWorkOrderOEM.Rows.Count != 1)
                {
                    _dtWorkOrderOEM.Rows.Add();

                    _dtWorkOrderOEM.Rows[0]["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    _dtWorkOrderOEM.Rows[0]["VERSION_NO"] = 1;

                }

                if (!string.IsNullOrEmpty(cbeCustomer.Text) && !string.IsNullOrEmpty(cbeCellSupplier.Text)
                    && !string.IsNullOrEmpty(cbeCellType.Text) && !string.IsNullOrEmpty(cbeCellModel.Text)
                    && !string.IsNullOrEmpty(cbeStructureParam.Text) && !string.IsNullOrEmpty(cbePlaceOrigin.Text)
                    && !string.IsNullOrEmpty(cbeGlassType.Text) && !string.IsNullOrEmpty(cbeJunctionBoxOEM.Text)
                    && !string.IsNullOrEmpty(cbeBOMAuthenticationCode.Text) && !string.IsNullOrEmpty(cbeBOMDesign.Text)
                    && !string.IsNullOrEmpty(cbeSEMoudleType.Text))
                {

                    _dtWorkOrderOEM.Rows[0]["WORK_ORDER_KEY"] = workOrderKey;
                    _dtWorkOrderOEM.Rows[0]["ORDER_NUMBER"] = workOrderNumber;
                    _dtWorkOrderOEM.Rows[0]["IS_USED"] = "Y";
                    _dtWorkOrderOEM.Rows[0]["CUSROMER"] = cbeCustomer.Text;
                    _dtWorkOrderOEM.Rows[0]["CELL_SUPPLIER"] = cbeCellSupplier.Text;
                    _dtWorkOrderOEM.Rows[0]["CELL_TYPE"] = cbeCellType.Text;
                    _dtWorkOrderOEM.Rows[0]["CELL_MODEL"] = cbeCellModel.Text;
                    _dtWorkOrderOEM.Rows[0]["STRUCTURE_PARAM"] = cbeStructureParam.Text;
                    _dtWorkOrderOEM.Rows[0]["PLACE_ORIGIN"] = cbePlaceOrigin.Text;
                    _dtWorkOrderOEM.Rows[0]["GLASS_TYPE"] = cbeGlassType.Text;
                    _dtWorkOrderOEM.Rows[0]["JUNCTION_BOX"] = cbeJunctionBoxOEM.Text;
                    _dtWorkOrderOEM.Rows[0]["BOM_AUTHENTICATION_CODE"] = cbeBOMAuthenticationCode.Text;
                    _dtWorkOrderOEM.Rows[0]["BOM_DESIGN"] = cbeBOMDesign.Text;
                    _dtWorkOrderOEM.Rows[0]["EDITOR"] = cbeBOMDesign.Text;
                    _dtWorkOrderOEM.Rows[0]["SE_MODULE_TYPE"] = cbeSEMoudleType.Text;
                }
                else
                {
                    bl_bak = false;

                    //MessageService.ShowMessage(string.Format("工单OEM信息不能存在空白栏位！"), "提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0031}"), StringParser.Parse("${res:Global.SystemInfo}"));
                }

            }
            else
            {
                if (_dtWorkOrderOEM.Rows.Count == 1)
                {
                    _dtWorkOrderOEM.Rows[0].Delete();
                }
            }


            //判断工单绑定线别信息是否完整
            for (int i = 0; i < gvWorkOrderLine.RowCount; i++)
            {
                string lineKey = gvWorkOrderLine.GetRowCellValue(i, "LINE_KEY").ToString();
                if (string.IsNullOrEmpty(lineKey))
                {
                    bl_bak = false;
                    //MessageService.ShowMessage(string.Format("第{0}行线别未进行选择，请补充！",i), "提示");
                    MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0032}"), i), StringParser.Parse("${res:Global.SystemInfo}"));
                }
            }
            //判定ctm上下限
            for (int i = 0; i < gvEffCtm.RowCount; i++)
            {
                string ctmUp = ((DataView)this.gvEffCtm.DataSource).Table.Rows[i]["CTM_UP"].ToString();
                string ctmLow = ((DataView)this.gvEffCtm.DataSource).Table.Rows[i]["CTM_LOW"].ToString();

                if (string.IsNullOrEmpty(ctmUp))
                {
                    //MessageService.ShowMessage("CTM上限不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0033}"));
                    bl_bak = false;
                    break;
                }

                if (string.IsNullOrEmpty(ctmLow))
                {
                    //MessageService.ShowMessage("CTM下限不能为空!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0034}"));
                    bl_bak = false;
                    break;
                }
            }

            return bl_bak;
        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            //更新选中的视图Cell
            UpAllFocuseCell();

            //对产品基本信息进行重新赋值
            EditProBaseInfo(this.gvProduct.FocusedRowHandle);

            //判断栏位情况
            if (!IsValidData()) return;

            DataSet dsSave = new DataSet();

            string workOrderKey = _dtWorkOrderNumber.Select(string.Format(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER + "= '{0}'", teOrderNumber.Text.Trim()))[0]["WORK_ORDER_KEY"].ToString();

            #region //对产品更新的遍历保存

            DataTable _dtPorProduct_Insert = _dtPorProduct.GetChanges(DataRowState.Added);
            DataTable _dtPorProduct_Update = _dtPorProduct.GetChanges(DataRowState.Modified);
            DataTable _dtPorProduct_Delete = _dtPorProduct.GetChanges(DataRowState.Deleted);


            if (_dtPorProduct_Insert != null && _dtPorProduct_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtPorProduct_Insert.Clone();

                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtPorProduct_Insert.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drInserts = _dtPorProduct_Insert.Select(string.Format(" PART_NUMBER ='{0}'", dr["PART_NUMBER"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["VERSION_NO"] = 1;
                    drNew["IS_USED"] = "Y";
                    drNew["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.Columns.Remove("PART_DESC");
                dtInsert.TableName = "POR_WO_PRD_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtPorProduct_Update != null && _dtPorProduct_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtPorProduct_Update.Clone();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtPorProduct_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtPorProduct_Update.Select(string.Format(" PART_NUMBER ='{0}'", dr["PART_NUMBER"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    ////drNew["VERSION_NO"] = Convert.ToInt32(drNew["VERSION_NO"]) + 1;
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.Columns.Remove("PART_DESC");
                dtUpdate.TableName = "POR_WO_PRD_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (_dtPorProduct_Delete != null && _dtPorProduct_Delete.Rows.Count > 0)
            {
                DataTable dtDelete = _dtPorProduct_Delete.Clone();
                _dtPorProduct_Delete.RejectChanges();
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtPorProduct_Delete.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drDeletes = _dtPorProduct_Delete.Select(string.Format(" PART_NUMBER ='{0}'", dr["PART_NUMBER"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtDelete.NewRow();
                    for (int i = 0; i < dtDelete.Columns.Count; i++)
                    {
                        drNew[i] = drDeletes[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "N";
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtDelete.Rows.Add(drNew);
                }
                dtDelete.TableName = "POR_WO_PRD_DELETE";
                dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //对功率分档更新的遍历保存

            DataTable _dtProPS_Insert = _dtProPS.GetChanges(DataRowState.Added);
            DataTable _dtProPS_Update = _dtProPS.GetChanges(DataRowState.Modified);
            DataTable _dtProPS_Delete = _dtProPS.GetChanges(DataRowState.Deleted);


            if (_dtProPS_Insert != null && _dtProPS_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtProPS_Insert.Clone();

                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtProPS_Insert.Rows)
                {
                    //定位正在遍历的行信息
                    DataRow[] drInserts = _dtProPS_Insert.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["VERSION_NO"] = 1;
                    drNew["IS_USED"] = "Y";
                    drNew["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.TableName = "POR_WO_PRD_PS_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtProPS_Update != null && _dtProPS_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtProPS_Update.Clone();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPS_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtProPS_Update.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    //drNew["VERSION_NO"] = Convert.ToInt32(drNew["VERSION_NO"]) + 1;
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = "POR_WO_PRD_PS_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (_dtProPS_Delete != null && _dtProPS_Delete.Rows.Count > 0)
            {
                DataTable dtDelete = _dtProPS_Delete.Clone();
                _dtProPS_Delete.RejectChanges();
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPS_Delete.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drDeletes = _dtProPS_Delete.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtDelete.NewRow();
                    for (int i = 0; i < dtDelete.Columns.Count; i++)
                    {
                        drNew[i] = drDeletes[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "N";
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtDelete.Rows.Add(drNew);
                }
                dtDelete.TableName = "POR_WO_PRD_PS_DELETE";
                dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //对功率分档花色更新的遍历保存

            DataTable _dtProPSColor_Insert = _dtProPSColor.GetChanges(DataRowState.Added);
            DataTable _dtProPSColor_Update = _dtProPSColor.GetChanges(DataRowState.Modified);
            DataTable _dtProPSColor_Delete = _dtProPSColor.GetChanges(DataRowState.Deleted);


            if (_dtProPSColor_Insert != null && _dtProPSColor_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtProPSColor_Insert.Clone();

                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtProPSColor_Insert.Rows)
                {
                    //定位正在遍历的行信息
                    DataRow[] drInserts = _dtProPSColor_Insert.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}' AND COLOR_CODE = '{2}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString(), dr["COLOR_CODE"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["VERSION_NO"] = 1;
                    drNew["IS_USED"] = "Y";
                    drNew["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.TableName = "POR_WO_PRD_PS_CLR_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtProPSColor_Update != null && _dtProPSColor_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtProPSColor_Update.Clone();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPSColor_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtProPSColor_Update.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}' AND COLOR_CODE = '{2}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString(), dr["COLOR_CODE"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    //drNew["VERSION_NO"] = Convert.ToInt32(drNew["VERSION_NO"]) + 1;
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = "POR_WO_PRD_PS_CLR_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (_dtProPSColor_Delete != null && _dtProPSColor_Delete.Rows.Count > 0)
            {
                DataTable dtDelete = _dtProPSColor_Delete.Clone();
                _dtProPSColor_Delete.RejectChanges();
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPSColor_Delete.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drDeletes = _dtProPSColor_Delete.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}' AND COLOR_CODE = '{2}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString(), dr["COLOR_CODE"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtDelete.NewRow();
                    for (int i = 0; i < dtDelete.Columns.Count; i++)
                    {
                        drNew[i] = drDeletes[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "N";
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtDelete.Rows.Add(drNew);
                }
                dtDelete.TableName = "POR_WO_PRD_PS_CLR_DELETE";
                dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //对功率分档子分档更新的遍历保存

            DataTable _dtProPSSub_Insert = _dtProPSSub.GetChanges(DataRowState.Added);
            DataTable _dtProPSSub_Update = _dtProPSSub.GetChanges(DataRowState.Modified);
            DataTable _dtProPSSub_Delete = _dtProPSSub.GetChanges(DataRowState.Deleted);


            if (_dtProPSSub_Insert != null && _dtProPSSub_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtProPSSub_Insert.Clone();

                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtProPSSub_Insert.Rows)
                {
                    //定位正在遍历的行信息
                    DataRow[] drInserts = _dtProPSSub_Insert.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}' AND PS_SUB_CODE = '{2}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString(), dr["PS_SUB_CODE"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["VERSION_NO"] = 1;
                    drNew["IS_USED"] = "Y";
                    drNew["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.TableName = "POR_WO_PRD_PS_SUB_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtProPSSub_Update != null && _dtProPSSub_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtProPSSub_Update.Clone();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPSSub_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtProPSSub_Update.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}' AND PS_SUB_CODE = '{2}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString(), dr["PS_SUB_CODE"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    //drNew["VERSION_NO"] = Convert.ToInt32(drNew["VERSION_NO"]) + 1;
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = "POR_WO_PRD_PS_SUB_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (_dtProPSSub_Delete != null && _dtProPSSub_Delete.Rows.Count > 0)
            {
                DataTable dtDelete = _dtProPSSub_Delete.Clone();
                _dtProPSSub_Delete.RejectChanges();
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPSSub_Delete.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drDeletes = _dtProPSSub_Delete.Select(string.Format(" PART_NUMBER ='{0}' AND POWERSET_KEY = '{1}' AND PS_SUB_CODE = '{2}'", dr["PART_NUMBER"].ToString(), dr["POWERSET_KEY"].ToString(), dr["PS_SUB_CODE"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtDelete.NewRow();
                    for (int i = 0; i < dtDelete.Columns.Count; i++)
                    {
                        drNew[i] = drDeletes[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "N";
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtDelete.Rows.Add(drNew);
                }
                dtDelete.TableName = "POR_WO_PRD_PS_SUB_DELETE";
                dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //对产品等级更新的遍历保存

            DataTable _dtProLevel_Insert = _dtProLevel.GetChanges(DataRowState.Added);
            DataTable _dtProLevel_Update = _dtProLevel.GetChanges(DataRowState.Modified);
            DataTable _dtProLevel_Delete = _dtProLevel.GetChanges(DataRowState.Deleted);


            if (_dtProLevel_Insert != null && _dtProLevel_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtProLevel_Insert.Clone();

                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtProLevel_Insert.Rows)
                {
                    //定位正在遍历的行信息
                    DataRow[] drInserts = _dtProLevel_Insert.Select(string.Format(" PART_NUMBER ='{0}' AND GRADE = '{1}' ", dr["PART_NUMBER"].ToString(), dr["GRADE"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["VERSION_NO"] = 1;
                    drNew["IS_USED"] = "Y";
                    drNew["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.TableName = "POR_WO_PRD_LEVEL_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtProLevel_Update != null && _dtProLevel_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtProLevel_Update.Clone();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProLevel_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtProLevel_Update.Select(string.Format(" PART_NUMBER ='{0}' AND GRADE = '{1}' ", dr["PART_NUMBER"].ToString(), dr["GRADE"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    ////drNew["VERSION_NO"] = Convert.ToInt32(drNew["VERSION_NO"]) + 1;
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = "POR_WO_PRD_LEVEL_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (_dtProLevel_Delete != null && _dtProLevel_Delete.Rows.Count > 0)
            {
                DataTable dtDelete = _dtProLevel_Delete.Clone();
                _dtProLevel_Delete.RejectChanges();
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProLevel_Delete.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drDeletes = _dtProLevel_Delete.Select(string.Format(" PART_NUMBER ='{0}' AND GRADE = '{1}' ", dr["PART_NUMBER"].ToString(), dr["GRADE"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtDelete.NewRow();
                    for (int i = 0; i < dtDelete.Columns.Count; i++)
                    {
                        drNew[i] = drDeletes[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "N";
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtDelete.Rows.Add(drNew);
                }
                dtDelete.TableName = "POR_WO_PRD_LEVEL_DELETE";
                dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //对衰减等级更新的遍历保存

            DataTable _dtProDecay_Insert = _dtProDecay.GetChanges(DataRowState.Added);
            DataTable _dtProDecay_Update = _dtProDecay.GetChanges(DataRowState.Modified);
            DataTable _dtProDecay_Delete = _dtProDecay.GetChanges(DataRowState.Deleted);


            if (_dtProDecay_Insert != null && _dtProDecay_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtProDecay_Insert.Clone();

                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtProDecay_Insert.Rows)
                {
                    //定位正在遍历的行信息
                    DataRow[] drInserts = _dtProDecay_Insert.Select(string.Format(" PART_NUMBER ='{0}' AND DECAY_NEWKEY = '{1}' ", dr["PART_NUMBER"].ToString(), dr["DECAY_NEWKEY"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    if (!string.IsNullOrEmpty(dr["DECAY_NEWKEY"].ToString()))
                    {
                        drNew["DECAY_KEY"] = dr["DECAY_NEWKEY"];
                    }
                    drNew["IS_USED"] = "Y";
                    drNew["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.Columns.Remove("DECAY_NEWKEY");
                dtInsert.TableName = "POR_WO_PRD_DECAY_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtProDecay_Update != null && _dtProDecay_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtProDecay_Update.Clone();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProDecay_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtProDecay_Update.Select(string.Format(" PART_NUMBER ='{0}' AND DECAY_KEY = '{1}' ", dr["PART_NUMBER"].ToString(), dr["DECAY_KEY"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["DECAY_NEWKEY"] = CommonUtils.GenerateNewKey(0);
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = "POR_WO_PRD_DECAY_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (_dtProDecay_Delete != null && _dtProDecay_Delete.Rows.Count > 0)
            {
                DataTable dtDelete = _dtProDecay_Delete.Clone();
                _dtProDecay_Delete.RejectChanges();
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProDecay_Delete.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drDeletes = _dtProDecay_Delete.Select(string.Format(" PART_NUMBER ='{0}' AND DECAY_KEY = '{1}' ", dr["PART_NUMBER"].ToString(), dr["DECAY_KEY"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtDelete.NewRow();
                    for (int i = 0; i < dtDelete.Columns.Count; i++)
                    {
                        drNew[i] = drDeletes[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "N";
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtDelete.Rows.Add(drNew);
                }

                dtDelete.Columns.Remove("DECAY_NEWKEY");
                dtDelete.TableName = "POR_WO_PRD_DECAY_DELETE";
                dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //对标签打印设置更新的遍历保存

            DataTable _dtProPrintSet_Insert = _dtProPrintSet.GetChanges(DataRowState.Added);
            DataTable _dtProPrintSet_Update = _dtProPrintSet.GetChanges(DataRowState.Modified);
            DataTable _dtProPrintSet_Delete = _dtProPrintSet.GetChanges(DataRowState.Deleted);


            if (_dtProPrintSet_Insert != null && _dtProPrintSet_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtProPrintSet_Insert.Clone();

                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtProPrintSet_Insert.Rows)
                {
                    //定位正在遍历的行信息
                    DataRow[] drInserts = _dtProPrintSet_Insert.Select(string.Format(" PART_NUMBER ='{0}' AND PRINTSET_KEY = '{1}' ", dr["PART_NUMBER"].ToString(), dr["PRINTSET_KEY"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "Y";
                    drNew["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }

                dtInsert.Columns.Remove("MinMaxPower");
                dtInsert.TableName = "POR_WO_PRD_PRINTSET_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtProPrintSet_Update != null && _dtProPrintSet_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtProPrintSet_Update.Clone();

                dtUpdate.Columns.Add("PRINTSET_NEWKEY");

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPrintSet_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtProPrintSet_Update.Select(string.Format(" PART_NUMBER ='{0}' AND PRINTSET_KEY = '{1}' ", dr["PART_NUMBER"].ToString(), dr["PRINTSET_KEY"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count - 1; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["PRINTSET_NEWKEY"] = CommonUtils.GenerateNewKey(0);
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.Columns.Remove("MinMaxPower");
                dtUpdate.TableName = "POR_WO_PRD_PRINTSET_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (_dtProPrintSet_Delete != null && _dtProPrintSet_Delete.Rows.Count > 0)
            {
                DataTable dtDelete = _dtProPrintSet_Delete.Clone();
                _dtProPrintSet_Delete.RejectChanges();
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPrintSet_Delete.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drDeletes = _dtProPrintSet_Delete.Select(string.Format(" PART_NUMBER ='{0}' AND PRINTSET_KEY = '{1}' ", dr["PART_NUMBER"].ToString(), dr["PRINTSET_KEY"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtDelete.NewRow();
                    for (int i = 0; i < dtDelete.Columns.Count; i++)
                    {
                        drNew[i] = drDeletes[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "N";
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtDelete.Rows.Add(drNew);
                }
                dtDelete.Columns.Remove("MinMaxPower");
                dtDelete.TableName = "POR_WO_PRD_PRINTSET_DELETE";
                dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //产品属性修改信息的保存

            DataTable dtAttr = gcOrderProperties.DataSource as DataTable;

            DataTable dtAttr_Update = dtAttr.GetChanges(DataRowState.Modified);
            DataTable dtAttr_Insert = dtAttr.GetChanges(DataRowState.Added);

            if (dtAttr_Update != null && dtAttr_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = dtAttr_Update.Clone();
                foreach (DataRow dr in dtAttr_Update.Rows)
                {
                    DataRow[] drUpdates = dtAttr.Select(string.Format(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY + "='{0}'", dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString()));
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY] = workOrderKey;
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORUPDATE;
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }
            if (dtAttr_Insert != null && dtAttr_Insert.Rows.Count > 0)
            {
                if (!dtAttr_Insert.Columns.Contains(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY))
                    dtAttr_Insert.Columns.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY);
                if (!dtAttr_Insert.Columns.Contains(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_PRO_ID))
                    dtAttr_Insert.Columns.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_PRO_ID);
                if (!dtAttr_Insert.Columns.Contains(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ISFLAG))
                    dtAttr_Insert.Columns.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ISFLAG);

                DataTable dtInsert = dtAttr_Insert.Clone();
                foreach (DataRow dr in dtAttr_Insert.Rows)
                {
                    DataRow[] drInserts = dtAttr_Insert.Select(string.Format(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY + "='{0}'", dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY] = workOrderKey;
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY] = CommonUtils.GenerateNewKey(0);
                    //插入主产品ID
                    //drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_PRO_ID] = lueProductId.EditValue.ToString();
                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ISFLAG] = 1;

                    drNew[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORINSERT;
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }
            #endregion

            #region //工单OEM信息的更新保存

            DataTable _dtWorkOrderOEM_Insert = _dtWorkOrderOEM.GetChanges(DataRowState.Added);
            DataTable _dtWorkOrderOEM_Update = _dtWorkOrderOEM.GetChanges(DataRowState.Modified);
            DataTable _dtWorkOrderOEM_Delete = _dtWorkOrderOEM.GetChanges(DataRowState.Deleted);


            if (_dtWorkOrderOEM_Insert != null && _dtWorkOrderOEM_Insert.Rows.Count > 0)
            {
                _dtWorkOrderOEM_Insert.TableName = "POR_WO_OEM_INSERT";
                dsSave.Merge(_dtWorkOrderOEM_Insert, true, MissingSchemaAction.Add);
            }

            if (_dtWorkOrderOEM_Update != null && _dtWorkOrderOEM_Update.Rows.Count > 0)
            {
                _dtWorkOrderOEM_Update.TableName = "POR_WO_OEM_UPDATE";
                dsSave.Merge(_dtWorkOrderOEM_Update, true, MissingSchemaAction.Add);
            }

            if (_dtWorkOrderOEM_Delete != null && _dtWorkOrderOEM_Delete.Rows.Count > 0)
            {
                _dtWorkOrderOEM_Delete.RejectChanges();

                _dtWorkOrderOEM_Delete.TableName = "POR_WO_OEM_DELETE";
                dsSave.Merge(_dtWorkOrderOEM_Delete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //工单工艺流程的更新保存

            DataTable _dtWorkOrderRoute_Insert = _dtWorkOrderRoute.GetChanges(DataRowState.Added);
            DataTable _dtWorkOrderRoute_Update = _dtWorkOrderRoute.GetChanges(DataRowState.Modified);
            DataTable _dtWorkOrderRoute_Delete = _dtWorkOrderRoute.GetChanges(DataRowState.Deleted);


            if (_dtWorkOrderRoute_Insert != null && _dtWorkOrderRoute_Insert.Rows.Count > 0)
            {
                _dtWorkOrderRoute_Insert.Rows[0]["VERSION_NO"] = 1;
                _dtWorkOrderRoute_Insert.Rows[0]["IS_USED"] = "Y";
                _dtWorkOrderRoute_Insert.Rows[0]["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                _dtWorkOrderRoute_Insert.TableName = "POR_WO_ROUTE_INSERT";
                dsSave.Merge(_dtWorkOrderRoute_Insert, true, MissingSchemaAction.Add);
            }

            if (_dtWorkOrderRoute_Update != null && _dtWorkOrderRoute_Update.Rows.Count > 0)
            {
                _dtWorkOrderRoute_Update.Rows[0]["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                _dtWorkOrderRoute_Update.TableName = "POR_WO_ROUTE_UPDATE";
                dsSave.Merge(_dtWorkOrderRoute_Update, true, MissingSchemaAction.Add);
            }

            if (_dtWorkOrderRoute_Delete != null && _dtWorkOrderRoute_Delete.Rows.Count > 0)
            {
                _dtWorkOrderRoute_Delete.RejectChanges();

                _dtWorkOrderRoute_Delete.Rows[0]["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                _dtWorkOrderRoute_Delete.TableName = "POR_WO_ROUTE_DELETE";
                dsSave.Merge(_dtWorkOrderRoute_Delete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //工单线别绑定

            DataTable _dtWorkOrderLine_Insert = _dtWorkOrderLine.GetChanges(DataRowState.Added);
            DataTable _dtWorkOrderLine_Update = _dtWorkOrderLine.GetChanges(DataRowState.Modified);
            DataTable _dtWorkOrderLine_Delete = _dtWorkOrderLine.GetChanges(DataRowState.Deleted);


            if (_dtWorkOrderLine_Insert != null && _dtWorkOrderLine_Insert.Rows.Count > 0)
            {
                for (int i = 0; i < _dtWorkOrderLine_Insert.Rows.Count; i++)
                {
                    _dtWorkOrderLine_Insert.Rows[0]["VERSION_NO"] = 1;
                    _dtWorkOrderLine_Insert.Rows[0]["IS_USED"] = "Y";
                    _dtWorkOrderLine_Insert.Rows[0]["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
                _dtWorkOrderLine_Insert.TableName = "POR_WO_LINE_INSERT";
                dsSave.Merge(_dtWorkOrderLine_Insert, true, MissingSchemaAction.Add);
            }

            if (_dtWorkOrderLine_Update != null && _dtWorkOrderLine_Update.Rows.Count > 0)
            {
                for (int i = 0; i < _dtWorkOrderLine_Update.Rows.Count; i++)
                {
                    _dtWorkOrderLine_Update.Rows[0]["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
                _dtWorkOrderLine_Update.TableName = "POR_WO_LINE_UPDATE";
                dsSave.Merge(_dtWorkOrderLine_Update, true, MissingSchemaAction.Add);
            }

            if (_dtWorkOrderLine_Delete != null && _dtWorkOrderLine_Delete.Rows.Count > 0)
            {
                _dtWorkOrderLine_Delete.RejectChanges();
                for (int i = 0; i < _dtWorkOrderLine_Delete.Rows.Count; i++)
                {
                    _dtWorkOrderLine_Delete.Rows[0]["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
                _dtWorkOrderLine_Delete.TableName = "POR_WO_LINE_DELETE";
                dsSave.Merge(_dtWorkOrderLine_Delete, true, MissingSchemaAction.Add);
            }

            #endregion

            #region //工单档位上下限管控
            _dtUpAndLow.AcceptChanges();
            DataTable dtUplowRule = _dtUpAndLow;
            DataTable dtUplowRuleKEY = _dtUpAndLow;
            if (dtUplowRule != null && dtUplowRule.Rows.Count > 0)
            {
                for (int i = 0; i < dtUplowRule.Rows.Count; i++)
                {
                    dtUplowRule.Rows[i]["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dtUplowRule.Rows[i]["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
                dtUplowRule.TableName = "POR_WO_PRD_UPLOWRULE";
                dsSave.Merge(dtUplowRule, true, MissingSchemaAction.Add);
            }
            DataRow drKey = dtUplowRuleKEY.NewRow();
            drKey["WORK_ORDER_KEY"] = workOrderKey;
            dtUplowRuleKEY.Rows.Add(drKey);
            dtUplowRuleKEY.TableName = "POR_WO_PRD_UPLOWRULE_KEY";
            dsSave.Merge(dtUplowRuleKEY, true, MissingSchemaAction.Add);
            #endregion

            #region //工单CTM上下限管控
            DataTable dtEffCtm = new DataTable();
            DataTable dtWorkOrderKey = new DataTable();
            dtWorkOrderKey.Columns.Add("WORKORDERKEY", Type.GetType("System.String"));
            DataRow DR = dtWorkOrderKey.NewRow();
            DR["WORKORDERKEY"] = workOrderKey;
            dtWorkOrderKey.Rows.Add(DR);
            if (gvEffCtm.DataSource != null)
            {
                dtEffCtm = ((DataView)this.gvEffCtm.DataSource).Table;
            }
            dtEffCtm.TableName = "POR_WO_CTM";
            dtWorkOrderKey.TableName = "PRO_WO";
            dsSave.Merge(dtEffCtm, true, MissingSchemaAction.Add);
            dsSave.Merge(dtWorkOrderKey, true, MissingSchemaAction.Add);

            #endregion

            #region //工单打印规则设置
            DataTable dtPrintRule = new DataTable();
            DataTable dtPrintRuleDetail = new DataTable();
            dtPrintRule.Columns.Add("WORK_ORDER_KEY");
            dtPrintRule.Columns.Add("WORK_ORDER_NUMBER");
            dtPrintRule.Columns.Add("PRINT_CODE");
            dtPrintRule.Columns.Add("PRINT_NAME");
            dtPrintRule.Columns.Add("PRINT_DESC");
            dtPrintRule.Columns.Add("CREATOR");
            dtPrintRule.Columns.Add("PRINT_RESOUCE");
            DataRow drRule = dtPrintRule.NewRow();
            if (!string.IsNullOrEmpty(this.lupPrintRule.Text.ToString()))
            {
                drRule["WORK_ORDER_KEY"] = workOrderKey;
                drRule["WORK_ORDER_NUMBER"] = workOrderNumber;
                drRule["PRINT_CODE"] = this.lupPrintRule.Text.ToString();
                drRule["PRINT_NAME"] = this.lupPrintRule.GetColumnValue("PRINT_NAME").ToString();
                drRule["PRINT_DESC"] = this.lupPrintRule.GetColumnValue("PRINT_DESC").ToString();
                drRule["PRINT_RESOUCE"] = this.lupPrintRule.GetColumnValue("PRINT_RESOUCE").ToString();
                drRule["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dtPrintRule.Rows.Add(drRule);
            }

            if (gvRuleDetail.DataSource != null)
            {
                dtPrintRuleDetail = ((DataView)this.gvRuleDetail.DataSource).Table;
            }
            dtPrintRuleDetail.TableName = "POR_WO_PRD_PRINTRULE_DETAIL";
            dtPrintRule.TableName = "POR_WO_PRD_PRINTRULE";
            dsSave.Merge(dtPrintRuleDetail, true, MissingSchemaAction.Add);
            dsSave.Merge(dtPrintRule, true, MissingSchemaAction.Add);
            #endregion
            #region //标签铭牌清单设置 
            DataTable _dtProPShow_Insert = _dtProPShow.GetChanges(DataRowState.Added);
            DataTable _dtProPShow_Update = _dtProPShow.GetChanges(DataRowState.Modified);
            DataTable _dtProPShow_Delete = _dtProPShow.GetChanges(DataRowState.Deleted);


            if (_dtProPShow_Insert != null && _dtProPShow_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtProPShow_Insert.Clone();

                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtProPShow_Insert.Rows)
                {
                    //定位正在遍历的行信息
                    DataRow[] drInserts = _dtProPShow_Insert.Select(string.Format(" PART_NUMBER ='{0}' AND RULE_CODE = '{1}'", dr["PART_NUMBER"].ToString(), dr["RULE_CODE"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["VERSION_NO"] = 1;
                    drNew["IS_USED"] = "Y";
                    drNew["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.TableName = "POR_WO_PRD_POWERSHOW_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtProPShow_Update != null && _dtProPShow_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtProPShow_Update.Clone();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPShow_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtProPShow_Update.Select(string.Format(" PART_NUMBER ='{0}' AND RULE_CODE = '{1}'", dr["PART_NUMBER"].ToString(), dr["RULE_CODE"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    //drNew["VERSION_NO"] = Convert.ToInt32(drNew["VERSION_NO"]) + 1;
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = "POR_WO_PRD_POWERSHOW_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (_dtProPShow_Delete != null && _dtProPShow_Delete.Rows.Count > 0)
            {
                DataTable dtDelete = _dtProPShow_Delete.Clone();
                _dtProPShow_Delete.RejectChanges();
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtProPShow_Delete.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drDeletes = _dtProPShow_Delete.Select(string.Format(" PART_NUMBER ='{0}' AND RULE_CODE = '{1}'", dr["PART_NUMBER"].ToString(), dr["RULE_CODE"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtDelete.NewRow();
                    for (int i = 0; i < dtDelete.Columns.Count; i++)
                    {
                        drNew[i] = drDeletes[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["IS_USED"] = "N";
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtUpdate中
                    dtDelete.Rows.Add(drNew);
                }
                dtDelete.TableName = "POR_WO_PRD_POWERSHOW_DELETE";
                dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            }
            #endregion


            #region //包装打印信息保存
            DataTable dtPackPrint = gcPackPrint.DataSource as DataTable;

            DataTable _dtPackPrint_Insert = dtPackPrint.GetChanges(DataRowState.Added);
            DataTable _dtPackPrint_Update = dtPackPrint.GetChanges(DataRowState.Modified);
            //DataTable _dtPackPrint_Delete = dtPackPrint.GetChanges(DataRowState.Deleted);


            if (_dtPackPrint_Insert != null && _dtPackPrint_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = _dtPackPrint_Insert.Clone();
                dtInsert.Columns.Add("WORK_ORDER_KEY");
                dtInsert.Columns.Add("EDIT_TIME");
                dtInsert.Columns.Add("EDITOR");
                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtPackPrint_Insert.Rows)
                {
                    //定位正在遍历的行信息
                    DataRow[] drInserts = _dtPackPrint_Insert.Select(string.Format("PART_NUMBER = '{0}' ", dr["PART_NUMBER"].ToString()));

                    //对临时表dtInsert的信息进行的遍历更新
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < _dtPackPrint_Insert.Columns.Count; i++)
                    {
                        drNew[i] = drInserts[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    //drNew["IS_USED"] = "Y";
                    drNew["WORK_ORDER_KEY"] = txt_Work_Order_No.Text;
                    drNew["EDIT_TIME"] = DateTime.Now.ToString();
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    //把新增的行插入到dtInsert中
                    dtInsert.Rows.Add(drNew);
                }
                dtInsert.TableName = "POR_WO_PLASH_AUTOPRINT_INSERT";
                dsSave.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (_dtPackPrint_Update != null && _dtPackPrint_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = _dtPackPrint_Update.Clone();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtPackPrint_Update.Rows)
                {
                    //获取选中的行的信息
                    DataRow[] drUpdates = _dtPackPrint_Update.Select(string.Format(" PART_NUMBER ='{0}' ", dr["PART_NUMBER"].ToString()));

                    //对临时表dtUpdate的信息进行的遍历更新
                    DataRow drNew = dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    //对需要进行更新的列信息进行补充更新
                    drNew["WORK_ORDER_KEY"] = txt_Work_Order_No.Text;
                    drNew["EDIT_TIME"] = DateTime.Now.ToString();
                    drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    //把新增的行插入到dtUpdate中
                    dtUpdate.Rows.Add(drNew);
                }
                dtUpdate.TableName = "POR_WO_PLASH_AUTOPRINT_UPDATE";
                dsSave.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            //if (_dtPackPrint_Delete != null && _dtPackPrint_Delete.Rows.Count > 0)
            //{
            //    DataTable dtDelete = _dtPackPrint_Delete.Clone();
            //    _dtProDecay_Delete.RejectChanges();
            //    //遍历把更新的信息写入到临时表dtUpdate中
            //    foreach (DataRow dr in _dtPackPrint_Delete.Rows)
            //    {
            //        //获取选中的行的信息
            //        DataRow[] drDeletes = _dtPackPrint_Delete.Select(string.Format(" PART_NUMBER ='{0}' ", dr["PART_NUMBER"].ToString()));

            //        //对临时表dtUpdate的信息进行的遍历更新
            //        DataRow drNew = dtDelete.NewRow();
            //        for (int i = 0; i < dtDelete.Columns.Count; i++)
            //        {
            //            drNew[i] = drDeletes[0][i];
            //        }
            //        //对需要进行更新的列信息进行补充更新
            //        drNew["IS_USED"] = "N";
            //        drNew["EDIT_TIME"] = DateTime.Now.ToString();
            //        drNew["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            //        //把新增的行插入到dtUpdate中
            //        dtDelete.Rows.Add(drNew);
            //    }
            //    dtDelete.TableName = "POR_WO_PLASH_AUTOPRINT_DELETE";
            //    dsSave.Merge(dtDelete, true, MissingSchemaAction.Add);
            //}
            #endregion

            #region EL 图片测试规则设定 
            DataTable dtElPicRule = new DataTable();
            dtElPicRule.Columns.Add("FOR_WO_ELESTRULE_KEY");
            dtElPicRule.Columns.Add("WORK_ORDER_KEY");
            dtElPicRule.Columns.Add("ELTESTRULE");
            dtElPicRule.Columns.Add("RULE_TYPE");
            dtElPicRule.Columns.Add("CREATE_USER_ID");
            dtElPicRule.Columns.Add("EDIT_USER_ID");
            DataRow drElPicRule = dtElPicRule.NewRow();
            DataRow drElAftRule = dtElPicRule.NewRow();
            if (!string.IsNullOrEmpty(this.lstBefLamRule.Text.ToString()) && !string.IsNullOrEmpty(this.lstAfterLamRule.Text.ToString()))
            {
                drElPicRule["WORK_ORDER_KEY"] = workOrderKey;
                drElPicRule["ELTESTRULE"] = this.lstBefLamRule.Text.ToString();
                drElPicRule["RULE_TYPE"] = "befLam";
                drElPicRule["CREATE_USER_ID"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                drElPicRule["EDIT_USER_ID"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dtElPicRule.Rows.Add(drElPicRule);

                drElAftRule["WORK_ORDER_KEY"] = workOrderKey;
                drElAftRule["ELTESTRULE"] = this.lstAfterLamRule.Text.ToString();
                drElAftRule["RULE_TYPE"] = "aftLam";
                drElAftRule["CREATE_USER_ID"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                drElAftRule["EDIT_USER_ID"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dtElPicRule.Rows.Add(drElAftRule);
            }
            dtElPicRule.TableName = "POR_WO_ELTESTRULE";
            dsSave.Merge(dtElPicRule, true, MissingSchemaAction.Add);
            #endregion

            #region 打印信息
            DataTable printTable = new DataTable();
            printTable.Columns.Add("F001");
            printTable.Columns.Add("F002");
            printTable.Columns.Add("F003");
            printTable.Columns.Add("F004");
            printTable.Columns.Add("F005");
            printTable.Columns.Add("F006");
            printTable.Columns.Add("F007");
            printTable.Columns.Add("F008");
            printTable.Columns.Add("F101");
            printTable.Columns.Add("ORDER_NUMBER");
            printTable.TableName = "POR_WO_PRINT";
            DataRow drPrint = printTable.NewRow();
            drPrint["ORDER_NUMBER"] = txtWorkOrder.Text;
            drPrint["F001"] = cobBatteryType.Text;
            drPrint["F002"] = txtBatteryQty.Text;
            drPrint["F003"] = cobBatteryGG.Text;
            drPrint["F004"] = cobBatteryQty.Text;
            drPrint["F005"] = cobBoLiType.Text;
            drPrint["F006"] = txtWorkDate.Text;
            drPrint["F007"] = txtFacCode.Text;
            drPrint["F008"] = txtWorkSerial.Text;
            drPrint["F101"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            printTable.Rows.Add(drPrint);
            dsSave.Merge(printTable);

            #endregion
            DataSet dsReturn = _workOrdersEntity.SaveWorkOrderProInfo(dsSave);
            if (!string.IsNullOrEmpty(_workOrdersEntity.ErrorMsg))
            {
                MessageService.ShowMessage(_workOrdersEntity.ErrorMsg);
                return;
            }
            else
            {
                //MessageService.ShowMessage("保存成功!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0035}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.State = ControlState.ReadOnly;
                #region //对信息进行重新绑定
                //清空原来绑定信息
                ClearWorkOrderPro();
                //获取工单产品信息
                GetWOProData(workOrderKey);
                //获取工单基本信息
                GetWorkOrderData(workOrderNumber);
                //绑定获取的工单产品信息
                BindProduct();
                //绑定产品基本信息
                BindProBaseInfo();
                //获取工单属性信息并进行绑定
                GetWorkOrderAttrData(workOrderNumber);
                //对 工单线别信息进行绑定
                BindFactoryLine();
                //对工单功率上下线进行绑定
                BindUPLOWRule();
                BindWoCtm("", workOrderKey);
                BindPackPrint(workOrderNumber);
                #endregion
            }

        }


        //绑定料号实际标称功率和显示的标称功率设置的信息fyb
        private void BindPowerShow()
        {
            DataView dv = this._dtProPShow.DefaultView;

            this.gcPowerShow.DataSource = dv;
            this.gvPowerShow.FocusedRowHandle = -1;
        }

        private void BindWoPrintRule(string workOrderKey)
        {
            string code = string.Empty;
            DataSet dsPrintRule = _entity.GetPrintRuleData(workOrderKey);
            if (dsPrintRule.Tables.Contains("PRO_WO_PRD_PRINTRULE_DETAIL") && dsPrintRule.Tables["PRO_WO_PRD_PRINTRULE_DETAIL"].Rows.Count > 0)
            {
                this.gcRuleDetail.DataSource = dsPrintRule.Tables[0];
            }
            if (dsPrintRule.Tables.Contains("PRO_WO_PRD_PRINTRULE") && dsPrintRule.Tables["PRO_WO_PRD_PRINTRULE"].Rows.Count > 0)
            {
                this.lblRule.Text = dsPrintRule.Tables["PRO_WO_PRD_PRINTRULE"].Rows[0]["PRINT_NAME"].ToString() + ":" + dsPrintRule.Tables["PRO_WO_PRD_PRINTRULE"].Rows[0]["PRINT_DESC"].ToString();
                //this.picEdit.Image = ??
                code = dsPrintRule.Tables["PRO_WO_PRD_PRINTRULE"].Rows[0]["PRINT_CODE"].ToString();
            }
            if (code == "5")
            {
                DataTable dtRule1 = new DataTable();
                dtRule1.Columns.Add("PRODUCT_MODEL_CODE");
                dtRule1.Columns.Add("PRODUCT_MODEL_NAME");
                DataRow dr = dtRule1.NewRow();
                dr["PRODUCT_MODEL_CODE"] = "0";
                dr["PRODUCT_MODEL_NAME"] = "True";
                dtRule1.Rows.Add(dr);
                rilLupProductModel.DisplayMember = "PRODUCT_MODEL_CODE";
                rilLupProductModel.ValueMember = "PRODUCT_MODEL_CODE";
                rilLupProductModel.DataSource = dtRule1;
            }
            else
                BindProductModel();
            //BindProductModel();

        }

        private void BindProductModel()
        {
            DataSet dsProductModel = _entity.GetProductModel();
            rilLupProductModel.DisplayMember = "PRODUCT_MODEL_CODE";
            rilLupProductModel.ValueMember = "PRODUCT_MODEL_CODE";
            rilLupProductModel.DataSource = dsProductModel.Tables[0];
        }

        /// <summary>
        /// 在对衰减系数序号进行更新的时候对衰减序号对应的衰减范围进行更新
        /// </summary>
        private void riglueDECAY_KEY_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            if (this.gvProPrintSet.State == GridState.Editing && this.gvProPrintSet.IsEditorFocused)
            {
                string currentPrintDecayKey = Convert.ToString(this.gvProPrintSet.EditingValue);
                string newPrintDecayKey = Convert.ToString(e.Value);
                if (currentPrintDecayKey != newPrintDecayKey)
                {
                    this.gvProduct.SetFocusedRowCellValue(this.gvProPrintSet.FocusedColumn, newPrintDecayKey);

                    for (int i = 0; i < _dtProDecay.Rows.Count; i++)
                    {
                        if (_dtProDecay.Rows[i].RowState != DataRowState.Deleted && _dtProDecay.Rows[i].RowState != DataRowState.Detached)
                        {
                            if (newPrintDecayKey == _dtProDecay.Rows[i]["DECAY_NEWKEY"].ToString())
                            {
                                string minMaxPower = _dtProDecay.Rows[i]["DECAY_POWER_MIN"].ToString() + "-" + _dtProDecay.Rows[i]["DECAY_POWER_MAX"].ToString();
                                this.gvProPrintSet.SetFocusedRowCellValue("MinMaxPower", minMaxPower);
                            }
                        }
                    }

                    this.gvProduct.UpdateCurrentRow();
                }
            }
        }

        /// <summary>
        /// 在选择标签打印的时候对衰减列表进行重新刷新绑定
        /// </summary>
        private void xtabProParamControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtabProParamControl.SelectedTabPageIndex == 4)
            {
                BindPrintDeacy();
            }
        }

        private void tsbtnEdit_Click(object sender, EventArgs e)
        {
            //isFirstLoad = false;
            this.State = ControlState.Edit;
        }

        private void tsbtnCancel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(workOrderKey)) return;
            if (string.IsNullOrEmpty(workOrderNumber)) return;


            //更新状态在产品选中行变更后可进行更新
            //isFirstLoad = true;

            //把控件设置为只读状态
            this.State = ControlState.ReadOnly;

            //清除信息
            ClearWorkOrderPro();
            //获取工单产品信息
            GetWOProData(workOrderKey);
            //获取工单基本信息
            GetWorkOrderData(workOrderNumber);
            //绑定获取的工单产品信息
            BindProduct();
            //获取工单属性信息并进行绑定
            GetWorkOrderAttrData(workOrderNumber);
            //绑定工单功率上下限
            //BindUPLOWRule();
            BindWoPrintRule(workOrderKey);
            BindPrintRule();
            ////绑定Ctm值
            BindWoCtm(gvProduct.GetRowCellValue(0, "PRODUCT_KEY").ToString(), workOrderKey);
            //清楚包装清单打印信息
            BindPackPrint(workOrderNumber);
            this.State = ControlState.ReadOnly;

        }

        private void riceLableChenk_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void teStepName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OperationHelpDialog dlg = new OperationHelpDialog();
            dlg.FactoryRoom = string.Empty;
            dlg.ProductType = string.Empty;
            dlg.EnterpriseName = beRouteEnterprise;
            dlg.RouteName = teRouteName;
            dlg.StepName = teStepName;
            dlg.dtWorkOrderRoute = _dtWorkOrderRoute;
            dlg.IsRework = false;
            Point i = teStepName.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + teStepName.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X, i.Y - dlg.Height);
                }
            }
            else
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X + teStepName.Width - dlg.Width, i.Y + teStepName.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + teStepName.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }

        #region 工单线别的绑定

        /// <summary>
        /// 绑定工厂线别
        /// </summary>
        private void BindFactoryLine()
        {

            DataView dv = this._dtWorkOrderLine.DefaultView;

            this.gcWorkOrderLine.DataSource = dv;
            this.gvWorkOrderLine.FocusedRowHandle = -1;
        }
        /// <summary>
        /// 绑定工单功率上下限
        /// </summary>
        private void BindUPLOWRule()
        {

            DataView dv = this._dtUpAndLow.DefaultView;

            this.gcUPLOWRule.DataSource = dv;
            this.gvUPLOWRule.FocusedRowHandle = -1;
        }


        /// <summary>
        /// 新增工单线别绑定
        /// </summary>
        private void sbtnLineAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drNew = _dtWorkOrderLine.NewRow();

                drNew["WORK_ORDER_KEY"] = workOrderKey;
                drNew["ORDER_NUMBER"] = partNumber;
                drNew["WORK_ORDER_LINE_KEY"] = CommonUtils.GenerateNewKey(0);

                _dtWorkOrderLine.Rows.Add(drNew);
            }
            catch //(Exception ex) 
            { }
        }

        /// <summary>
        /// 删除工单线别
        /// </summary>
        private void sbtnLineDelete_Click(object sender, EventArgs e)
        {
            DataRow dr = gvWorkOrderLine.GetFocusedDataRow();
            dr.Delete();
        }

        /// <summary>
        /// 下拉关闭时对选择线别的判断
        /// </summary>
        private void rpgleFactoryLine_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            if (this.gvWorkOrderLine.State == GridState.Editing && this.gvWorkOrderLine.IsEditorFocused)
            {
                string currentLineKey = Convert.ToString(this.gvWorkOrderLine.EditingValue);
                string newLineKey = Convert.ToString(e.Value);
                if (currentLineKey != newLineKey)
                {
                    DataRow[] drs = _dtWorkOrderLine.Select(string.Format(" LINE_KEY = '{0}'", newLineKey));

                    if (drs.Length == 0)
                    {
                        drs = _dtFactoryLineList.Select(string.Format(" LINE_KEY = '{0}'", newLineKey));
                        foreach (DataRow dr in drs)
                        {
                            this.gvWorkOrderLine.SetFocusedRowCellValue(this.gvWorkOrderLine.FocusedColumn, newLineKey);
                            this.gvWorkOrderLine.SetFocusedRowCellValue("FACTORY_KEY", dr["FACTORY_KEY"]);
                            this.gvWorkOrderLine.SetFocusedRowCellValue("FATORY_NAME", dr["FACTORY_NAME"]);
                            this.gvWorkOrderLine.SetFocusedRowCellValue("LINE_NAME", dr["LINE_NAME"]);
                            this.gvWorkOrderLine.SetFocusedRowCellValue("LINE_CODE", dr["LINE_CODE"]);

                            //对打印设置的衰减序号信息进行绑定
                            //BindPrintDeacy();

                        }
                    }
                    else
                    {
                        e.Value = "";

                        //MessageBox.Show("选择线别已存在，不能重复选择！");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0036}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    }

                    this.gvProduct.UpdateCurrentRow();
                }
            }
        }
        /// <summary>
        /// 新增工单功率上下限卡控打印标签行信息
        /// </summary>
        private void sbtnUpAndLowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drNew = _dtUpAndLow.NewRow();

                drNew["WORK_ORDER_KEY"] = workOrderKey;
                drNew["ORDER_NUMBER"] = partNumber;
                drNew["WORK_ORDER_LINE_KEY"] = CommonUtils.GenerateNewKey(0);

                _dtWorkOrderLine.Rows.Add(drNew);
            }
            catch //(Exception ex) 
            { }
        }
        #endregion

        private void sbtUPLowRuleAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow drNew = _dtUpAndLow.NewRow();

                drNew["WORK_ORDER_KEY"] = workOrderKey;
                drNew["WORK_ORDER"] = workOrderNumber;
                drNew["UPLOW_RULE_UPLINE"] = "0";
                drNew["UPLOW_RULE_LOWLINE"] = "0";

                _dtUpAndLow.Rows.Add(drNew);
            }
            catch //(Exception ex) 
            { }
        }

        private void sbtUPLowRuleRemove_Click(object sender, EventArgs e)
        {
            DataRow dr = gvUPLOWRule.GetFocusedDataRow();
            dr.Delete();
        }

        private void sbtAddCtm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(gvProduct.GetRowCellValue(0, "PRODUCT_KEY").ToString()))
            {
                //MessageBox.Show("未设定主产品id，请先设定！");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0037}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            DataTable dtEffCtm = new DataTable();
            dtEffCtm.Columns.Add("EFF_UP", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("EFF_LOW", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("CTM_UP", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("CTM_LOW", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("PRO_KEY", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("WORK_ORDER_KEY", Type.GetType("System.String"));
            dtEffCtm.Columns.Add("ischecked", typeof(bool));
            if (string.IsNullOrEmpty(txtInitialize.Text.ToString()))
            {
                //MessageBox.Show("未设定效率档位的初始值信息，请设定！");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0038}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            if (string.IsNullOrEmpty(txtEffValue.Text.ToString()))
            {
                ////MessageBox.Show("未设定自增长幅度信息，请设定！");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0039}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            if (string.IsNullOrEmpty(txtRows.Text.ToString()))
            {
                //MessageBox.Show("未设定自动生成行数信息，请设定！");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0040}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            if (this.gcEffCtm.DataSource != null)
                dtEffCtm = ((DataView)this.gvEffCtm.DataSource).Table;

            decimal initialize = Convert.ToDecimal(txtInitialize.Text.ToString());
            decimal effValue = Convert.ToDecimal(txtEffValue.Text.ToString());
            for (int i = 1; i <= Convert.ToInt32(txtRows.Text.ToString()); i++)
            {
                DataRow drEffctm = dtEffCtm.NewRow();
                drEffctm["EFF_LOW"] = initialize.ToString("0.00");
                initialize = initialize + effValue;
                drEffctm["EFF_UP"] = initialize.ToString("0.00");
                drEffctm["PRO_KEY"] = gvProduct.GetRowCellValue(0, "PRODUCT_KEY").ToString();
                drEffctm["WORK_ORDER_KEY"] = workOrderKey;
                DataRow[] drrow = dtEffCtm.Select("EFF_UP = '" + drEffctm["EFF_UP"] + "' AND EFF_LOW = '" + drEffctm["EFF_LOW"] + "'");
                if (drrow.Length > 0)
                {
                    //MessageBox.Show("已经存在行信息:效率上限-【" + drEffctm["EFF_UP"] + "】|效率下限-【" + drEffctm["EFF_LOW"] + "】", "系统提示");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0041}")
                        + drEffctm["EFF_UP"]
                        + StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0042}")
                        + drEffctm["EFF_LOW"]
                        + StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0043}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    continue;
                }
                dtEffCtm.Rows.Add(drEffctm);
            }
            gcEffCtm.DataSource = dtEffCtm;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataTable dtEffCtm = new DataTable();
            if (this.gcEffCtm.DataSource != null)
                dtEffCtm = ((DataView)this.gvEffCtm.DataSource).Table;
            else
            {
                //MessageBox.Show("列表信息为空没有需要删除的数据！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0044}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            DataRow[] dr = dtEffCtm.Select("ischecked = true");
            if (dr.Length > 0)
            {
                foreach (DataRow dr01 in dr)
                    dtEffCtm.Rows.Remove(dr01);
                this.gcEffCtm.DataSource = dtEffCtm;
            }
            else
            {
                //MessageBox.Show("请选择需要删除的数据信息！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0045}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
        }

        private void smpCtmDelete_Click(object sender, EventArgs e)
        {
            this.gcEffCtm.DataSource = null;
        }
        /// <summary>
        /// 通过产品id获取ctm上下限卡控值  add by chao.pang
        /// </summary>
        /// <param name="proKey">产品id值</param>
        private void BindCtmInf(string proKey, string workOrder)
        {
            DataSet dSSetCtm = _entity.GetCtmInf(proKey, workOrder);
            DataTable dtProCtm = new DataTable();
            DataTable dtProWoCtm = new DataTable();

            if (dSSetCtm != null)
            {
                dtProCtm = dSSetCtm.Tables["BASE_PRODUCTMODEL_CTM"];
                dtProWoCtm = dSSetCtm.Tables["POR_WO_PRD_CTM"];
            }
            else return;
            if (dtProWoCtm.Rows.Count > 0)
            {
                gcEffCtm.DataSource = dtProWoCtm;
            }
            else
            {
                gcEffCtm.DataSource = dtProCtm;
            }
        }
        private void BindWoCtm(string proid, string workorderKey)
        {
            DataSet dSSetCtm = _entity.GetCtmInf(proid, workorderKey);
            DataTable dtProWoCtm = new DataTable();
            if (dSSetCtm != null)
            {
                dtProWoCtm = dSSetCtm.Tables["POR_WO_PRD_CTM"];
            }
            else return;
            gcEffCtm.DataSource = dtProWoCtm;
        }
        /// <summary>
        /// 打印规则代码变更时触发,绑定规则的配置参数信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lupPrintRule_EditValueChanged(object sender, EventArgs e)
        {

            printRuleCode = this.lupPrintRule.Text.ToString();
            if (printRuleCode.Trim() == "")
            {
                return;
            }
            string[] l_s = new string[] { "PRINT_CODE", "PRINT_COLUM_CODE", "PRINT_COLUM_NAME" };
            string category = "BASE_PRINT_RULE";
            DataTable dtCommon = BaseData.Get(l_s, category);
            dtCommon.Columns.Add("PRINT_VALUE");
            DataTable dtPrintRule = dtCommon.Clone();
            dtPrintRule.TableName = "Print_Rule";
            DataRow[] drs = dtCommon.Select(string.Format("PRINT_CODE='{0}'", printRuleCode));
            foreach (DataRow dr in drs)
                dtPrintRule.ImportRow(dr);
            DataView dview = dtPrintRule.DefaultView;
            dview.Sort = "PRINT_CODE asc";

            gcRuleDetail.DataSource = dtPrintRule;
            this.lblRule.Text = this.lupPrintRule.GetColumnValue("PRINT_DESC").ToString();
            //this.picEdit.Image = ??

            if (printRuleCode.Trim() == "4")
            {

                //打印规则设定中的打印规则列值默认与主产品产品代码相匹配
                rilLupProductModel.DataSource = null;
                //gcRuleDetail.DataSource = null;
                DataTable dtProduct = gcProduct.DataSource as DataTable;
                DataTable dtProductModel = _entity.GetProductModel().Tables[0];
                string model_name = string.Empty;
                if (dtProduct.Rows[0]["PRODUCT_CODE"].ToString().Length > 9)
                {
                    model_name = dtProduct.Rows[0]["PRODUCT_CODE"].ToString().Substring(0, 9);
                }
                DataRow[] drProductModel = dtProductModel.Select(string.Format("PRODUCT_MODEL_NAME='{0}'", model_name));
                if (drProductModel.Count() > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PRODUCT_MODEL_CODE");
                    dt.Columns.Add("PRODUCT_MODEL_NAME");
                    DataRow dr = dt.NewRow();
                    dr["PRODUCT_MODEL_CODE"] = drProductModel[0]["PRODUCT_MODEL_CODE"];
                    dr["PRODUCT_MODEL_NAME"] = drProductModel[0]["PRODUCT_MODEL_NAME"];
                    dt.Rows.Add(dr);
                    rilLupProductModel.DisplayMember = "PRODUCT_MODEL_CODE";
                    rilLupProductModel.ValueMember = "PRODUCT_MODEL_CODE";
                    rilLupProductModel.DataSource = dt;
                }
                else
                {
                    BindProductModel();
                }
            }
            else if (printRuleCode.Trim() == "5" || printRuleCode.Trim() == "6")
            {
                DataTable dtRule1 = new DataTable();
                dtRule1.Columns.Add("PRODUCT_MODEL_CODE");
                dtRule1.Columns.Add("PRODUCT_MODEL_NAME");
                DataRow dr = dtRule1.NewRow();
                dr["PRODUCT_MODEL_CODE"] = "0";
                dr["PRODUCT_MODEL_NAME"] = "True";
                dtRule1.Rows.Add(dr);
                rilLupProductModel.DisplayMember = "PRODUCT_MODEL_CODE";
                rilLupProductModel.ValueMember = "PRODUCT_MODEL_CODE";
                rilLupProductModel.DataSource = dtRule1;
            }
            else
                BindProductModel();
        }

        /// <summary>
        /// 未绑定时的设定
        /// </summary>
        private void setPackPrint()
        {

            DataTable dt_Print = new DataTable();
            dt_Print.Columns.Add("PART_NUMBER");
            dt_Print.Columns.Add("PRINT_TYPE");
            dt_Print.Columns.Add("PRINT_COPY");
            foreach (DataRow row in _dtPartNumber.Rows)
            {
                if (!row.IsNull("PART_NUMBER"))
                {
                    DataRow row1 = dt_Print.NewRow();
                    row1["PART_NUMBER"] = row["PART_NUMBER"];
                    //row1["PRINT_COPY"] = 1;
                    dt_Print.Rows.Add(row1);
                }
            }
            gcPackPrint.DataSource = dt_Print;


        }

        /// <summary>
        ///   绑定打印模板
        /// </summary>
        /// <returns></returns>
        private void BindPrint_Type()
        {
            string[] l_s = new string[] { "Print_Type", "Print_Type_Des" };
            string category = "Packing_List_Print_Setting";

            DataTable dt_Print_Type = BaseData.Get(l_s, category);
            lu_Print_Types.ValueMember = "Print_Type";
            lu_Print_Types.DisplayMember = "Print_Type_Des";
            lu_Print_Types.DataSource = dt_Print_Type;
        }

        private void BindPackPrint(string workOrderNo)
        {
            DataSet dsFlashAutoPrint = _entity.GetFlashAutoPrintData(workOrderNo);
            if (dsFlashAutoPrint.Tables[0].Rows.Count > 0)
            {
                gcPackPrint.DataSource = dsFlashAutoPrint.Tables[0];

            }
            else
            {
                setPackPrint();
            }
        }

        private void xtrabLabelNameplate_Paint(object sender, PaintEventArgs e)
        {

        }
        //新增标签/铭牌/清单的体现功率设置fyb
        private void spbtnAddPower_Click(object sender, EventArgs e)
        {
            WorkOrderPSettingFormPower proPowerForm = new WorkOrderPSettingFormPower();

            if (DialogResult.OK == proPowerForm.ShowDialog())
            {
                if (IsExist(partNumber, proPowerForm.por_before_power, _dtProPShow, "PART_NUMBER", "BEFORE_POWER"))
                {
                    //MessageBox.Show("该料号的显示档位特殊规则已经添加", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0046}"),
                        StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string powerBefore = string.Empty;
                    string powerAfter = string.Empty;

                    powerBefore = proPowerForm.por_before_power.ToString();
                    powerAfter = proPowerForm.por_after_power.ToString();

                    PowerShowAdd(powerBefore, powerAfter, proPowerForm.drProPowerSet);
                }
            }
        }

        private void spbtnDelPower_Click(object sender, EventArgs e)
        {
            if (this.gvPowerShow.FocusedRowHandle > -1)
            {
                //获取选中的行并进行移除
                DataRow dr = this.gvPowerShow.GetFocusedDataRow();
                dr.Delete();

                //判断
                if (this.gvPowerShow.FocusedRowHandle > -1)
                {
                    //获取选择的功率分档的Key值
                    proRuleCode = gvPowerShow.GetRowCellValue(gvPowerShow.FocusedRowHandle, "RULE_CODE").ToString();
                }
            }
        }

        private void PowerShowAdd(string powerBefore, string powerAfter, DataRow drProPowerSet)
        {

            ProPShowAdd(drProPowerSet);

            //对功率分档进行绑定
            BindProPShow();

            //把新增的行设定为聚焦行
            if (!string.IsNullOrEmpty(proRuleCode))
            {
                for (int i = 0; i < gvPowerShow.RowCount; i++)
                {
                    string sk = Convert.ToString(((DataRowView)(this.gvPowerShow.GetRow(i))).Row["RULE_CODE"]);
                    if (proRuleCode.Equals(sk.Trim()))
                    {
                        this.gvPowerShow.FocusedRowHandle = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 对标签/铭牌/清单体现功率表进行新增
        /// </summary>
        /// <param name="dr">需要新增的行信息</param>
        private void ProPShowAdd(DataRow dr)
        {
            DataRow drPShow = _dtProPShow.NewRow();

            drPShow["WORK_ORDER_KEY"] = workOrderKey;
            drPShow["WORK_ORDER"] = workOrderNumber;
            drPShow["RULE_CODE"] = dr["RULE_CODE"];
            drPShow["BEFORE_POWER"] = dr["BEFORE_POWER"];
            drPShow["AFTER_POWER"] = dr["AFTER_POWER"];
            drPShow["PART_NUMBER"] = partNumber;

            _dtProPShow.Rows.Add(drPShow);
        }


        /// <summary>
        /// 对标签/铭牌/清单体现功率进行绑定fyb
        /// </summary>
        /// <param name="dr">对应的数据表</param>
        private void BindProPShow()
        {

            proRuleCode = "";

            DataView dv = this._dtProPShow.DefaultView;
            dv.RowFilter = string.Format(" WORK_ORDER_KEY='{0}' AND PART_NUMBER ='{1}'", workOrderKey, partNumber);
            dv.Sort = "BEFORE_POWER ASC";
            //对获取进行绑定
            this.gcPowerShow.DataSource = dv;


            //获取视图要绑定的行数如果存在信息聚焦到第一行
            if (dv.Count > 0)
            {
                //清除聚焦行
                gvPowerShow.FocusedRowHandle = -1;
            }
        }

        private void tsbtnQuery_Click(object sender, EventArgs e)
        {
            GetAllWorkList();
            //WorkOrderSettingForm workOrderSelect = new WorkOrderSettingForm();
            //if (DialogResult.OK == workOrderSelect.ShowDialog())
            //{
            //    //每次查询完后清空缓存
            //    gcRuleDetail.DataSource = null;
            //    lupPrintRule.EditValue = "";

            //    workOrderNumber = workOrderSelect.por_work_order_Number;
            //    workOrderKey = workOrderSelect.por_work_order_key;

            //    if (!string.IsNullOrEmpty(workOrderSelect.por_ProID))
            //    {
            //        //更新状态在产品选中行变更后可进行更新
            //        //isFirstLoad = false;

            //        //把控件设置为只读状态
            //        this.State = ControlState.ReadOnly;

            //        //清空原来绑定信息
            //        ClearWorkOrderPro();

            //        //获取工单产品信息
            //        GetWOProData(workOrderKey);
            //        //获取工单基本信息
            //        GetWorkOrderData(workOrderNumber);
            //        //绑定获取的工单产品信息
            //        BindProduct();
            //        //绑定产品基本信息
            //        BindProBaseInfo();
            //        //获取工单属性信息并进行绑定
            //        GetWorkOrderAttrData(workOrderNumber);
            //        //绑定工单线别
            //        BindFactoryLine();
            //        //获取工单对应的接线盒信息
            //        GetWOJunctionBox(workOrderNumber);
            //        //绑定工单功率上下限
            //        BindUPLOWRule();
            //        //绑定工单cTM数据信息
            //        BindWoCtm(workOrderSelect.por_ProID, workOrderKey);
            //        //绑定工单的打印规则信息
            //        BindWoPrintRule(workOrderKey);
            //        //绑定包装清单打印信息
            //        BindPackPrint(workOrderNumber);
            //        //绑定料号实际标称功率和显示的标称功率设置的信息fyb
            //      BindPowerShow();
            //    }
            //    else
            //    {
            //        //isFirstLoad = true;
            //        //获取工单产品信息
            //        GetWorkOrderData(workOrderNumber);
            //        //获取工单对应的接线盒信息
            //        GetWOJunctionBox(workOrderNumber);

            //        //工单变更后对控件对应数据进行清除添加主产品
            //        BindWorOrderPro();

            //        //绑定工单线别
            //        BindFactoryLine();

            //        //添加IV和EL默认属性
            //        AddELIVCheck();

            //        //绑定工单功率上下限
            //        BindUPLOWRule();

            //        //绑定工单cTM数据信息
            //        BindWoCtm("", workOrderKey);


            //        //绑定工单的打印规则信息
            //        BindWoPrintRule(workOrderKey);

            //        //绑定料号实际标称功率和显示的标称功率设置的信息fyb
            //        BindPowerShow();

            //        //对界面视图进行绑定
            //        this.State = ControlState.Edit;
            //        //把修改和取消置为不可用
            //        this.tsbtnEdit.Enabled = false;
            //        this.tsbtnCancel.Enabled = false;

            //        setPackPrint();
            //    }
            //    txt_Work_Order_No.Text = teOrderNumber.Text;
            //    BindPrint_Type();
            //}
        }

        private void gvProduct_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            if (e.PrevFocusedColumn != null && e.PrevFocusedColumn.FieldName == "PRODUCT_KEY")
            {
                string partNum = gvProduct.GetFocusedDataRow()["PART_NUMBER"].ToString();

                string partDesc = partNumberBind.Select(string.Format("PART_NUMBER='{0}'", partNum))[0]["PART_DESC"].ToString();
                string proKey = Convert.ToString(gvProduct.GetFocusedRowCellValue("PRODUCT_KEY"));
                if (string.IsNullOrEmpty(proKey))
                {
                    return;
                }
                string isKingLine = _dtProListBind.Select(string.Format("PRODUCT_KEY='{0}'", proKey))[0]["ISKINGLING"].ToString();

                //判断物料号信息、产品ID是否有金刚线，给出提示
                if (partDesc.Contains("金刚线"))
                {
                    if (isKingLine.Equals("0"))
                    {
                        //MessageBox.Show("该产品ID为非金刚线，请确认！");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0047}"));
                    }
                }
                else if (!(partDesc.Contains("金刚线")))
                {
                    if (isKingLine.Equals("1"))
                    {
                        //MessageBox.Show("该产品ID为金刚线，请确认！");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0048}"));
                    }

                }
                //判断物料号信息、产品ID是否有黑硅，给出提示 2018.5.11
                if (partDesc.Contains("黑硅"))
                {
                    if (!isKingLine.Equals("2"))
                    {
                        //MessageBox.Show("该产品ID为非黑硅片，请确认！");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0049}"));
                    }
                }
                else if (!(partDesc.Contains("黑硅")))
                {
                    if (isKingLine.Equals("2"))
                    {
                        //MessageBox.Show("该产品ID为黑硅片，请确认！");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0050}"));
                    }

                }

            }
        }
        /// <summary>
        /// 更新清单打印设置 2018.4.16
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnUpDataPrint_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(workOrderNumber))
            {
                if (_workOrdersEntity.isUpDataPrint(workOrderNumber))
                {
                    //MessageBox.Show(string.Format("【{0}】工单清单打印设置更新完毕", workOrderNumber));
                    MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0051}"), workOrderNumber));
                }
                else
                {
                    //MessageBox.Show( string.Format("【{0}】工单清单打印设置更新失败，请重试", workOrderNumber));
                    MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.WorkOrderProductSetting.msg.0052}"), workOrderNumber));
                }
                BindPackPrint(workOrderNumber);
            }

        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBOM_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvProduct_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvEffCtm_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvProLevel_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvProPrintSet_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvWorkOrderLine_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDecay_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPowerSetSub_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPowerSetColor_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPowerSet_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderProperties_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvPackPrint_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvPowerShow_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvRuleDetail_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvUPLOWRule_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvWorkOrder_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gcWorkOrderList_DoubleClick(object sender, EventArgs e)
        {

            Hashtable hstable = new Hashtable();
            if (!string.IsNullOrEmpty(txtWorkOrder.Text.Trim()))
            {
                hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = txtWorkOrder.Text.Trim();
            }
            else
            {
                return;
            }

            DataSet dsReturn = _workOrdersEntity.GetWorkOrderByNoOrProid(hstable);

            DataTable dtWorkOrder = dsReturn.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];

            if (dtWorkOrder.Rows.Count == 0)
            {
                MessageService.ShowMessage("请先建立工单，再设置属性");
                return;
            };

            DataRow drFocused = dtWorkOrder.Rows[0];
            workOrderKey = drFocused[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
            workOrderNumber = drFocused[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString();
            string por_ProID = drFocused[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString();
            if (string.IsNullOrEmpty(workOrderKey))
            {
                MessageService.ShowMessage("未读取到工单ID，请与管理员联系!");
                return;
            }

            gcRuleDetail.DataSource = null;
            lupPrintRule.EditValue = "";
            if (!string.IsNullOrEmpty(por_ProID))
            {
                //更新状态在产品选中行变更后可进行更新
                //isFirstLoad = false;

                //把控件设置为只读状态
                this.State = ControlState.ReadOnly;

                //清空原来绑定信息
                ClearWorkOrderPro();

                //获取工单产品信息
                GetWOProData(workOrderKey);
                //获取工单基本信息
                GetWorkOrderData(workOrderNumber);
                //绑定获取的工单产品信息
                BindProduct();
                //绑定产品基本信息
                BindProBaseInfo();
                //获取工单属性信息并进行绑定
                GetWorkOrderAttrData(workOrderNumber);
                //绑定工单线别
                BindFactoryLine();
                //获取工单对应的接线盒信息
                GetWOJunctionBox(workOrderNumber);
                //绑定工单功率上下限
                BindUPLOWRule();
                //绑定工单cTM数据信息
                BindWoCtm(por_ProID, workOrderKey);
                //绑定工单的打印规则信息
                BindWoPrintRule(workOrderKey);
                //绑定包装清单打印信息
                BindPackPrint(workOrderNumber);
                //绑定料号实际标称功率和显示的标称功率设置的信息fyb
                BindPowerShow();
           
            }
            else
            {
                //isFirstLoad = true;
                //获取工单产品信息
                GetWorkOrderData(workOrderNumber);
                //获取工单对应的接线盒信息
                GetWOJunctionBox(workOrderNumber);

                //工单变更后对控件对应数据进行清除添加主产品
                BindWorOrderPro();

                //绑定工单线别
                BindFactoryLine();

                //添加IV和EL默认属性
                AddELIVCheck();

                //绑定工单功率上下限
                BindUPLOWRule();

                //绑定工单cTM数据信息
                BindWoCtm("", workOrderKey);


                //绑定工单的打印规则信息
                BindWoPrintRule(workOrderKey);

                //绑定料号实际标称功率和显示的标称功率设置的信息fyb
                BindPowerShow();

                //对界面视图进行绑定
                this.State = ControlState.Edit;
                //把修改和取消置为不可用
                this.tsbtnEdit.Enabled = false;
                this.tsbtnCancel.Enabled = false;

                setPackPrint();
            }
            txt_Work_Order_No.Text = teOrderNumber.Text;
            BindPrint_Type();
            //打印信息
            BindWoPrint(workOrderNumber);
        }
        private void BindWoPrint(string orderNumber)
        {
            cobBatteryType.Text = "";
            txtBatteryQty.Text = "";
            cobBatteryGG.Text = "";
            cobBatteryQty.Text = "";
            cobBoLiType.Text = "";
            txtWorkDate.Text = "";
            txtFacCode.Text = "";
            txtWorkSerial.Text = "";

            DataTable dtWorkPrint = _workOrdersEntity.GetWoPrint(orderNumber);
            if (dtWorkPrint.Rows.Count > 0)
            {
                DataRow dr = dtWorkPrint.Rows[0];
                cobBatteryType.Text = dr["f001"].ToString();
                txtBatteryQty.Text = dr["f002"].ToString();
                cobBatteryGG.Text = dr["f003"].ToString();
                cobBatteryQty.Text = dr["f004"].ToString();
                cobBoLiType.Text = dr["f005"].ToString();
                txtWorkDate.Text = dr["f006"].ToString();
                txtFacCode.Text = dr["f007"].ToString();
                txtWorkSerial.Text = dr["f008"].ToString();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
