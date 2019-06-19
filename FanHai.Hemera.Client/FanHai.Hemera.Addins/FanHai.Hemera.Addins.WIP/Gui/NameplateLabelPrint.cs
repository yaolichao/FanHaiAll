using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.WIP.Report;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class NameplateLabelPrint : BaseUserCtrl
    {
        public string _orderNumner = string.Empty;
        public string _partName = string.Empty;
        public string _power = string.Empty;
        public string _noct = string.Empty;
        public string _cellType = string.Empty;
        public string _voc = string.Empty;
        public string _vmp = string.Empty;
        public string _isc = string.Empty;
        public string _imp = string.Empty;
        public string _fuse = string.Empty;
        public string _max = string.Empty;
        public string _pscode = string.Empty;

        public string _orderNumnerAsm = string.Empty;
        public string _partNameAsm = string.Empty;
        public string _powerAsm = string.Empty;
        public string _noctAsm = string.Empty;
        public string _cellTypeAsm = string.Empty;
        public string _vocAsm = string.Empty;
        public string _vmpAsm = string.Empty;
        public string _iscAsm = string.Empty;
        public string _impAsm = string.Empty;
        public string _fuseAsm = string.Empty;
        public string _maxAsm = string.Empty;
        public string _pscodeAsm = string.Empty;

        public string _orderNumnerCQC = string.Empty;
        public string _partNameCQC = string.Empty;
        public string _powerCQC = string.Empty;
        public string _noctCQC = string.Empty;
        public string _cellTypeCQC = string.Empty;
        public string _vocCQC = string.Empty;
        public string _vmpCQC = string.Empty;
        public string _iscCQC = string.Empty;
        public string _impCQC = string.Empty;
        public string _fuseCQC = string.Empty;
        public string _maxCQC = string.Empty;
        public string _pscodeCQC = string.Empty;


        //澳洲CEC
        public string _orderNumnerCEC = string.Empty;
        public string _partNameCEC = string.Empty;
        public string _powerCEC = string.Empty;
        public string _noctCEC = string.Empty;
        public string _cellTypeCEC = string.Empty;
        public string _vocCEC = string.Empty;
        public string _vmpCEC = string.Empty;
        public string _iscCEC = string.Empty;
        public string _impCEC = string.Empty;
        public string _fuseCEC = string.Empty;
        public string _maxCEC = string.Empty;
        public string _pscodeCEC = string.Empty;

        public string _orderNumnerCQClpz = string.Empty;
        public string _partNameCQClpz = string.Empty;
        public string _powerCQClpz = string.Empty;
        public string _cellTypeCQClpz = string.Empty;
        public string _effCQClpz = string.Empty;
        public string _mianjiCQClpz = string.Empty;
        public string _qtyCQClpz = string.Empty;
        public string _anthorCQClpz = string.Empty;

        public string _orderNumnerNe = string.Empty;
        public string _partNameNe = string.Empty;
        public string _powerNe = string.Empty;
        public string _noctNe = string.Empty;
        public string _cellTypeNe = string.Empty;
        public string _maxNe = string.Empty;
        public string _vocNe = string.Empty;
        public string _vmpNe = string.Empty;
        public string _iscNe = string.Empty;
        public string _impNe = string.Empty;
        public string _fuseNe = string.Empty;

        public string _orderNumnerCSA = string.Empty;
        public string _partNameCSA = string.Empty;
        public string _powerCSA = string.Empty;
        public string _noctCSA = string.Empty;
        public string _cellTypeCSA = string.Empty;
        public string _vocCSA = string.Empty;
        public string _vmpCSA = string.Empty;
        public string _iscCSA = string.Empty;
        public string _impCSA = string.Empty;
        public string _fuseCSA = string.Empty;
        public string _maxCSA = string.Empty;
        public string _pscodeCSA = string.Empty;

        public string _orderNumnerQT = string.Empty;
        public string _partNameQT = string.Empty;
        public string _powerQT = string.Empty;
        public string _noctQT = string.Empty;
        public string _cellTypeQT = string.Empty;
        public string _vocQT = string.Empty;
        public string _vmpQT = string.Empty;
        public string _iscQT = string.Empty;
        public string _impQT = string.Empty;
        public string _fuseQT = string.Empty;
        public string _maxQT = string.Empty;
        public string _pscodeQT = string.Empty;

        public string _orderNumnerTUVCSA = string.Empty;
        public string _partNameTUVCSA = string.Empty;
        public string _powerTUVCSA = string.Empty;
        public string _noctTUVCSA = string.Empty;
        public string _cellTypeTUVCSA = string.Empty;
        public string _vocTUVCSA = string.Empty;
        public string _vmpTUVCSA = string.Empty;
        public string _iscTUVCSA = string.Empty;
        public string _impTUVCSA = string.Empty;
        public string _fuseTUVCSA = string.Empty;
        public string _maxTUVCSA = string.Empty;
        public string _pscodeTUVCSA = string.Empty;

        public string _template = string.Empty;//铭牌模板名称
        public string _pordId = string.Empty;//产品代码

        DataTable dtNameplateLabelPrint = new DataTable();//存放型号温度信息

        NameplateLabelAutoPrintEntity namePlateLabelAutoPrint = new NameplateLabelAutoPrintEntity();//获取模板名称

        //此部分使用委托在修改原先代码的情况下解除因为工单不同料号不同而档位相同时的Bug jiabao.liu 2017-04-24
        private delegate void OnPartNumberChange(object sender,EventArgs e);
        private OnPartNumberChange _OnPartNumberChange;
        private OnPartNumberChange _OnASMPartNumberChange;
        private OnPartNumberChange _OnNormalCQCPartNumberChange;
        private OnPartNumberChange _OnNormalCECPartNumberChange;

        private OnPartNumberChange _OnNormalCQC_ANPartNumberChange;

        private OnPartNumberChange _OnRunCQCPartNumberChange;
        private OnPartNumberChange _OnNEPartNumberChange;
        private OnPartNumberChange _OnKoreaPartNumberChange;
        private OnPartNumberChange _OnCSAPartNumberChange;
        private OnPartNumberChange _OnQXandQTPartNumberChange;
        private OnPartNumberChange _OnTUVCSAPartNumberChange;

        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示

        public NameplateLabelPrint()
        {
            InitializeComponent();
            _OnPartNumberChange          += this.luPower_EditValueChanged;
            _OnASMPartNumberChange       += this.luPowerAsm_EditValueChanged;
            _OnNormalCQCPartNumberChange += this.luPowerCQC_EditValueChanged;
            _OnNormalCECPartNumberChange += this.luPowerCEC_EditValueChanged;
            _OnNormalCQC_ANPartNumberChange += this.luPowerCQC_AN_EditValueChanged;
            _OnRunCQCPartNumberChange    += this.luPowerCQClpz_EditValueChanged;
            _OnNEPartNumberChange        += this.luPowerNe_EditValueChanged;
            _OnKoreaPartNumberChange     += this.luPowerKorea_EditValueChanged;
            _OnCSAPartNumberChange       += this.luPowerCSA_EditValueChanged;
            _OnQXandQTPartNumberChange   += this.luPowerQT_EditValueChanged;
            _OnTUVCSAPartNumberChange    += this.luPowerTUVCSA_EditValueChanged;


            InitializeLanguage();
        }
        private void InitializeLanguage()
        {
            this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.101}");// "铭牌打印";
            this.xtraTabPage4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.102}");// "常规CQC/CGC领跑者";
            this.btnPrintC.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.103}");// "打印";
            this.labelControl3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.104}");// "请设置条码：组成规则（产品类型[6610P]+档位[250]+认证[CQC]+版本[01]  如6610P230CQC01）";
            this.layoutControlItem47.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.105}");// "工单号";
            this.layoutControlItem48.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.106}");// "额定最大功率";
            this.layoutControlItem49.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.107}");// "物料号";
            this.layoutControlItem51.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.108}");// "电池片类型";
            this.layoutControlItem52.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.109}");// "最大系统电压";
            this.layoutControlItem53.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.110}");// "开路电压";
            this.layoutControlItem54.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.111}");// "额定电压";
            this.layoutControlItem55.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.112}");// "短路电流";
            this.layoutControlItem56.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.113}");// "额定电流";
            this.layoutControlItem58.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.114}");// "分档方式";
            this.layoutControlItem59.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.115}");// "产品类型";
            this.layoutControlItem61.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.116}");// "档位";
            this.layoutControlItem62.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.117}");// "认证";
            this.layoutControlItem63.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.118}");// "版本第一位(组件类型)";
            this.layoutControlItem64.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.119}");// "版本第二位(Tolerance)";
            this.layoutControlItem212.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.120}");// "铭牌类型";
            this.xtraTabPage2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.121}");// "基础设置";
            this.layoutControlItem20.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.122}");// "横向X坐标";
            this.layoutControlItem24.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.123}");// "纵向Y坐标";
            this.btnPrint.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.124}");// "打印";
            this.labelControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.125}");// "请设置条码：组成规则（产品类型[6610P]+档位[250]+认证[TUV]+版本[01]  如6610P230TUV01）";
            this.layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.126}");// "工单号";
            this.layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.127}");// "开路电压";
            this.layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.128}");// "额定电流";
            this.layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.129}");// "物料号";
            this.layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.130}");// "额定最大功率";
            this.layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.131}");// "电池片类型";
            this.layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.132}");// "最大系统电压";
            this.layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.133}");// "产品类型";
            this.layoutControlItem14.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.134}");// "档位";
            this.layoutControlItem15.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.135}");// "认证";
            this.layoutControlItem16.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.136}");// "版本第一位（组件类型）";
            this.layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.137}");// "额定电压";
            this.layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.138}");// "短路电流";
            this.layoutControlItem17.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.139}");// "版本第二位（Tolerance）";
            this.layoutControlItem21.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.140}");// "分档方式";
            this.layoutControlItem165.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.141}");// "铭牌类型";
            this.labelControl2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.142}");// "请设置条码：组成规则（产品类型[6610P]+C+档位[250]+认证[TUV]+版本[01]  如6610PC230TUV01）";
            this.layoutControlItem26.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.143}");// "物料号";
            this.layoutControlItem27.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.144}");// "额定最大功率";
            this.layoutControlItem25.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.145}");// "工单号";
            this.layoutControlItem29.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.146}");// "电池片类型";
            this.layoutControlItem30.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.147}");// "最大系统电压";
            this.layoutControlItem31.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.148}");// "开路电压";
            this.layoutControlItem32.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.149}");// "额定电流";
            this.layoutControlItem33.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.150}");// "额定电压";
            this.layoutControlItem35.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.151}");// "短路电流";
            this.layoutControlItem36.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.152}");// "分档方式";
            this.layoutControlItem38.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.153}");// "产品类型";
            this.layoutControlItem39.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.154}");// "档位";
            this.layoutControlItem40.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.155}");// "认证";
            this.layoutControlItem41.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.156}");// "版本第一位(组件类型)";
            this.layoutControlItem42.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.157}");// "版本第二位（Tolerance）";
            this.layoutControlItem210.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.158}");// "铭牌类型";
            this.xtraTabPage5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.159}");// "CQC领跑者";
            this.btnPrintClpz.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.160}");// "打印";
            this.layoutControlItem69.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.161}");// "工单号";
            this.layoutControlItem70.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.162}");// "料号";
            this.layoutControlItem71.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.163}");// "额定最大功率";
            this.layoutControlItem72.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.164}");// "产品类型";
            this.layoutControlItem73.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.165}");// "转换效率";
            this.layoutControlItem74.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.166}");// "电池片数量(片)";
            this.lblMianj.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.167}");// "组件面积";
            this.btnNeJapan.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.168}");// "NE日本";
            this.btnNe.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.169}");// "打印";
            this.lcNeWord.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.170}");// "工单号";
            this.layoutControlItem79.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.171}");// "物料号";
            this.layoutControlItem80.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.172}");// "额定最大功率";
            this.layoutControlItem82.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.173}");// "种类";
            this.layoutControlItem83.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.174}");// "最大系统电压";
            this.layoutControlItem84.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.175}");// "开路电压";
            this.layoutControlItem85.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.176}");// "额定电压";
            this.layoutControlItem86.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.177}");// "短路电流";
            this.layoutControlItem78.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.178}");// "额定电流";
            this.layoutControlItem88.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.179}");// "分档方式";
            this.layoutControlItem91.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.180}");// "重量";
            this.layoutControlItem89.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.181}");// "产品类型";
            this.layoutControlItem178.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.182}");// "长*宽*高";
            this.layoutControlItem92.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.183}");// "长*宽*高";
            this.xtraTabPage6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.184}");// "韩国";
            this.btnPrintKorea.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.185}");// "打印";
            this.layoutControlItem93.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.186}");// "工单号";
            this.layoutControlItem94.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.187}");// "物料号";
            this.layoutControlItem95.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.188}");// "额定最大功率";
            this.layoutControlItem97.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.189}");// "电池片类型";
            this.layoutControlItem98.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.190}");// "最大系统电压";
            this.layoutControlItem99.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.191}");// "开路电压";
            this.layoutControlItem100.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.192}");// "额定电压";
            this.layoutControlItem101.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.193}");// "短路电流";
            this.layoutControlItem102.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.194}");// "额定电流";
            this.layoutControlItem104.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.195}");// "分档方式";
            this.layoutControlItem110.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.196}");// "产品类型";
            this.layoutControlItem111.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.197}");// "型号后缀";
            this.layoutControlItem113.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.198}");// "认证类型";
            this.layoutControlItem112.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.199}");// "版本第一位（组件类型）";
            this.layoutControlItem114.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.200}");// "版本第二位（Tolerance）";
            this.layoutControlItem209.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.201}");// "铭牌类型";
            this.btnPrintCSA.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.202}");// "打印";
            this.labelControl4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.203}");// "请设置条码：组成规则（产品类型[6610P]+档位[250]+认证[TUV]+版本[01]  如6610P230TUV01）";
            this.layoutControlItem119.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.204}");// "工单号";
            this.layoutControlItem120.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.205}");// "物料号";
            this.layoutControlItem121.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.206}");// "额定最大功率";
            this.layoutControlItem123.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.207}");// "电池片类型";
            this.layoutControlItem124.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.208}");// "最大系统电压";
            this.layoutControlItem125.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.209}");// "开路电压";
            this.layoutControlItem126.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.210}");// "额定电压";
            this.layoutControlItem127.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.211}");// "短路电流";
            this.layoutControlItem128.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.212}");// "额定电流";
            this.layoutControlItem130.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.213}");// "分档方式";
            this.layoutControlItem132.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.214}");// "产品型号";
            this.layoutControlItem133.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.215}");// "档位";
            this.layoutControlItem134.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.216}");// "认证";
            this.layoutControlItem135.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.217}");// "版本第一位（组件类型）";
            this.layoutControlItem136.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.218}");// "版本第二位（Tolerance）";
            this.xtraTabPage8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.219}");// "晴天/启鑫";
            this.btnPrintQT.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.220}");// "打印";
            this.labelControl5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.221}");// "请设置条码：组成规则（QTSM+产品类型[6610P]+档位[250]+认证[CQC]+版本[01]  如QTSM6610P230CQC01）";
            this.layoutControlItem141.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.222}");// "工单号";
            this.layoutControlItem142.CustomizationFormText = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.223}");// "物料号";
            this.layoutControlItem147.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.224}");// "开路电压";
            this.layoutControlItem148.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.225}");// "额定电压";
            this.layoutControlItem150.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.226}");// "额定电流";
            this.layoutControlItem152.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.227}");// "分档方式";
            this.layoutControlItem154.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.228}");// "产品型号";
            this.layoutControlItem155.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.229}");// "档位";
            this.layoutControlItem156.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.230}");// "认证";
            this.layoutControlItem157.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.231}");// "版本第一位(组件类型)";
            this.layoutControlItem158.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.232}");// "版本第二位(Tolerance)";
            this.layoutControlItem163.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.233}");// "铭牌类型";
            this.lyciNoct.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.234}");// "额定电池工作温度";
            this.lyciweight.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.235}");// "组件重量";
            this.lycicc.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.236}");// "组件尺寸";
            this.btnPrintTUVCSA.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.237}");// "打印";
            this.layoutControlItem166.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.238}");// "工单号";
            this.layoutControlItem167.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.239}");// "物料号";
            this.layoutControlItem168.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.240}");// "额定最大功率";
            this.layoutControlItem170.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.241}");// "电池片类型";
            this.layoutControlItem171.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.242}");// "系统最大电压";
            this.layoutControlItem172.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.243}");// "开路电压";
            this.layoutControlItem173.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.244}");// "额定电压";
            this.layoutControlItem149.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.245}");// "短路电流";
            this.layoutControlItem175.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.246}");// "额定电流";
            this.layoutControlItem177.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.247}");// "分档方式";
            this.layoutControlItem179.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.248}");// "产品型号";
            this.layoutControlItem180.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.249}");// "档位";
            this.layoutControlItem109.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.250}");// "铭牌类型";
            this.xtraTabPage9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.251}");// "安能";
            this.labelControl6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.252}");// "请设置条码：组成规则（产品类型[6610P]+档位[250]+认证[CQC]+版本[01]  如6610P230CQC01）";
            this.layoutControlItem189.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.253}");// "工单号";
            this.layoutControlItem188.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.254}");// "物料号";
            this.layoutControlItem190.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.255}");// "额定最大功率";
            this.layoutControlItem182.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.256}");// "最大系统电压";
            this.layoutControlItem183.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.257}");// "电池片类型";
            this.layoutControlItem195.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.258}");// "开路电压";
            this.layoutControlItem192.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.259}");// "额定电压";
            this.layoutControlItem197.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.260}");// "短路电流";
            this.layoutControlItem193.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.261}");// "额定电流";
            this.layoutControlItem194.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.262}");// "分档方式";
            this.layoutControlItem200.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.263}");// "档位";
            this.layoutControlItem201.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.264}");// "产品类型";
            this.layoutControlItem199.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.265}");// "认证";
            this.layoutControlItem202.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.266}");// "版本第一位(组件类型)";
            this.layoutControlItem203.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.267}");// "版本第二位(Tolerance)";
            this.tsmSetDefult.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.268}");// "设为有效数据";
            this.VC_CONTROLToolStripMenuItem.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.269}");// "更新卡控数据";
            this.layoutControlItem211.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.270}");// "铭牌类型";



        }



        public void BindWorkNumber()
        {
            teOrderNo.Properties.Items.Clear();
            WarehouseEngine whe = new WarehouseEngine();
            DataSet ds = whe.GetWorkNumber();
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    teOrderNo.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoAsm.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoCQC.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoCEC.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoCQC_AN.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoCQClpz.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoNe.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoKorea.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoCSA.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoQT.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                    teOrderNoTUVCSA.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                }
            }
        }

        private void teOrderNo_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumner = this.teOrderNo.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumner))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                //                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumner);
                try
                {
                    this.luPartNumber.Properties.DataSource = ds.Tables[0];
                    this.luPartNumber.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumber.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumber.ItemIndex = 0;
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void teOrderNoAsm_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerAsm = this.teOrderNoAsm.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerAsm))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                //                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerAsm);
                try
                {
                    this.luPartNumberAsm.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberAsm.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberAsm.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberAsm.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void teOrderNoCQC_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerCQC = this.teOrderNoCQC.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                //                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerCQC);
                try
                {
                    this.luPartNumberCQC.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberCQC.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberCQC.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberCQC.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void teOrderNoCQClpz_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerCQClpz = this.teOrderNoCQClpz.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQClpz))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                //                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerCQClpz);
                try
                {
                    this.luPartNumberCQClpz.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberCQClpz.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberCQClpz.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberCQClpz.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void teOrderNoNe_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerNe = this.teOrderNoNe.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerNe))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                //                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerNe);
                try
                {
                    this.luPartNumberNe.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberNe.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberNe.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberNe.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void teOrderNoKorea_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.teOrderNoKorea.EditValue.ToString()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                //                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(this.teOrderNoKorea.EditValue.ToString());
                try
                {
                    this.luPartNumberKorea.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberKorea.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberKorea.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberKorea.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void teOrderNoCSA_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerCSA = this.teOrderNoCSA.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCSA))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerCSA);
                try
                {
                    this.luPartNumberCSA.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberCSA.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberCSA.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberCSA.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void teOrderNoQT_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerQT = this.teOrderNoQT.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerQT))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerQT);
                try
                {
                    this.LuPartNumberQT.Properties.DataSource = ds.Tables[0];
                    this.LuPartNumberQT.Properties.DisplayMember = "PART_NUMBER";
                    this.LuPartNumberQT.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.LuPartNumberQT.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void teOrderNoTUVCSA_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerTUVCSA = this.teOrderNoTUVCSA.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerTUVCSA))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerTUVCSA);
                try
                {
                    this.luPartNumberTUVCSA.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberTUVCSA.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberTUVCSA.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberTUVCSA.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        private void luPartNumber_EditValueChanged(object sender, EventArgs e)
        {
            _partName = this.luPartNumber.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumner))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partName))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumner, _partName);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumner, _partName);
                try
                {
                    this.luPower.Properties.DataSource = ds.Tables[0];
                    this.luPower.Properties.DisplayMember = "PMAXSTAB";
                    this.luPower.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPower.ItemIndex = 0;
                    }
                    this.txtDIf.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtCertification.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();
                    this.txtType.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //根据质量需求温度根据产品型号锁死。jiabao.liu 2016/09/14
                        //this.comNoct.Properties.Items.Clear();
                        //this.comNoct.Properties.Items.Add("43");
                        //this.comNoct.Properties.Items.Add("46");

                        this.cmbCellType.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoct.Properties.Items.Clear();
                        //this.comNoct.Properties.Items.Add("47");
                        //this.comNoct.Properties.Items.Add("48");
                        this.cmbCellType.Text = "Mono-Si";
                    }
                    _OnPartNumberChange(luPower, e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtType, comNoct);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPartNumberAsm_EditValueChanged(object sender, EventArgs e)
        {
            _partNameAsm = this.luPartNumberAsm.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerAsm))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameAsm))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerAsm, _partNameAsm);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerAsm, _partNameAsm);
                try
                {
                    this.luPowerAsm.Properties.DataSource = ds.Tables[0];
                    this.luPowerAsm.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerAsm.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerAsm.ItemIndex = 0;
                    }
                    this.txtDIfAsm.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtCertificationAsm.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();
                    this.txtTypeAsm.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //this.comNoctAsm.Properties.Items.Clear();
                        //this.comNoctAsm.Properties.Items.Add("43");
                        //this.comNoctAsm.Properties.Items.Add("46");
                        this.cmbCellTypeAsm.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoctAsm.Properties.Items.Clear();
                        //this.comNoctAsm.Properties.Items.Add("47");
                        //this.comNoctAsm.Properties.Items.Add("48");
                        this.cmbCellTypeAsm.Text = "Mono-Si";
                    }
                    _OnASMPartNumberChange(luPowerAsm,e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeAsm, comNoctAsm);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPartNumberCQC_EditValueChanged(object sender, EventArgs e)
        {
            _partNameCQC = this.luPartNumberCQC.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQC))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerCQC, _partNameCQC);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerCQC, _partNameCQC);
                try
                {
                    this.luPowerCQC.Properties.DataSource = ds.Tables[0];
                    this.luPowerCQC.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerCQC.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerCQC.ItemIndex = 0;
                    }
                    this.txtDIfCQC.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtCertificationCQC.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();
                    this.txtTypeCQC.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //this.comNoctCQC.Properties.Items.Clear();
                        //this.comNoctCQC.Properties.Items.Add("43");
                        //this.comNoctCQC.Properties.Items.Add("46");
                        this.cmbCellTypeCQC.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoctCQC.Properties.Items.Clear();
                        //this.comNoctCQC.Properties.Items.Add("47");
                        //this.comNoctCQC.Properties.Items.Add("48");
                        this.cmbCellTypeCQC.Text = "Mono-Si";
                    }
                    _OnNormalCQCPartNumberChange(luPowerCQC,e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeCQC, comNoctCQC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
                if (luPartNumberCQC.EditValue.ToString().Contains("HV"))
                {
                    txtMaxCQC.Text = "1500";
                }
                else
                {
                    txtMaxCQC.Text = "1000";
                }
            }
        }
        private void luPartNumberCQClpz_EditValueChanged(object sender, EventArgs e)
        {
            _partNameCQClpz = this.luPartNumberCQClpz.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQClpz))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQClpz))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerCQClpz, _partNameCQClpz);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerCQClpz, _partNameCQClpz);
                try
                {
                    this.txtTypeCQClpz.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    this.luPowerCQClpz.Properties.DataSource = ds.Tables[0];
                    this.luPowerCQClpz.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerCQClpz.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerCQClpz.ItemIndex = 0;
                    }
                    _OnRunCQCPartNumberChange(luPowerCQClpz,e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPartNumberNe_EditValueChanged(object sender, EventArgs e)
        {
            _partNameNe = this.luPartNumberNe.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerNe))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameNe))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerNe, _partNameNe);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerNe, _partNameNe);
                try
                {
                    this.luPowerNe.Properties.DataSource = ds.Tables[0];
                    this.luPowerNe.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerNe.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerNe.ItemIndex = 0;
                    }
                    this.txtDIfNe.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtTypeNe.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //this.comNoctNe.Properties.Items.Clear();
                        //this.comNoctNe.Properties.Items.Add("43");
                        //this.comNoctNe.Properties.Items.Add("46");
                        this.cmbCellTypeNe.Text = "多結晶Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoctNe.Properties.Items.Clear();
                        //this.comNoctNe.Properties.Items.Add("47");
                        //this.comNoctNe.Properties.Items.Add("48");
                        this.cmbCellTypeNe.Text = "単結晶Si";
                    }
                    _OnNEPartNumberChange(luPowerNe,e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeNe, comNoctNe);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPartNumberKorea_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.teOrderNoKorea.EditValue.ToString()))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(this.luPartNumberKorea.Text.ToString()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(this.teOrderNoKorea.EditValue.ToString(), this.luPartNumberKorea.Text.ToString());
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(this.teOrderNoKorea.EditValue.ToString(), this.luPartNumberKorea.Text.ToString());
                this.txtTypeC.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                try
                {
                    this.luPowerKorea.Properties.DataSource = ds.Tables[0];
                    this.luPowerKorea.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerKorea.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerKorea.ItemIndex = 0;
                    }
                    this.txtDIfKorea.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    //this.txtTol.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();

                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //this.comNoctKorea.Properties.Items.Clear();
                        //this.comNoctKorea.Properties.Items.Add("43");
                        //this.comNoctKorea.Properties.Items.Add("46");
                        this.cmbCellTypeKorea.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoctKorea.Properties.Items.Clear();
                        //this.comNoctKorea.Properties.Items.Add("47");
                        //this.comNoctKorea.Properties.Items.Add("48");
                        this.cmbCellTypeKorea.Text = "Mono-Si";
                    }
                    _OnKoreaPartNumberChange(luPowerKorea,e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeC, comNoctKorea);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPartNumberCSA_EditValueChanged(object sender, EventArgs e)
        {
            _partNameCSA = this.luPartNumberCSA.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCSA))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerCSA, _partNameCSA);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerCSA, _partNameCSA);
                try
                {
                    this.luPowerCSA.Properties.DataSource = ds.Tables[0];
                    this.luPowerCSA.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerCSA.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerCSA.ItemIndex = 0;
                    }
                    this.txtDIfCSA.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtCertificationCSA.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();
                    this.txtTypeCSA.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //this.comNoctCSA.Properties.Items.Clear();
                        //this.comNoctCSA.Properties.Items.Add("43");
                        //this.comNoctCSA.Properties.Items.Add("46");
                        this.cmbCellTypeCSA.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoctCSA.Properties.Items.Clear();
                        //this.comNoctCSA.Properties.Items.Add("47");
                        //this.comNoctCSA.Properties.Items.Add("48");
                        this.cmbCellTypeCSA.Text = "Mono-Si";
                    }
                    _OnCSAPartNumberChange(luPowerCSA,e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeCSA, comNoctCSA);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void LuPartNumberQT_EditValueChanged(object sender, EventArgs e)
        {
            _partNameQT = this.LuPartNumberQT.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerQT))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerQT, _partNameQT);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerQT, _partNameQT);
                try
                {
                    this.luPowerQT.Properties.DataSource = ds.Tables[0];
                    this.luPowerQT.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerQT.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerQT.ItemIndex = 0;
                    }
                    this.txtDIfQT.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtCertificationQT.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();
                    this.txtTypeQT.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //this.comNoctQT.Properties.Items.Clear();
                        //this.comNoctQT.Properties.Items.Add("43");
                        //this.comNoctQT.Properties.Items.Add("46");
                        this.cmbCellTypeQT.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoctQT.Properties.Items.Clear();
                        //this.comNoctQT.Properties.Items.Add("47");
                        //this.comNoctQT.Properties.Items.Add("48");
                        this.cmbCellTypeQT.Text = "Mono-Si";
                    }
                    _OnQXandQTPartNumberChange(luPowerQT,e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeQT, comNoctQT);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPartNumberTUVCSA_EditValueChanged(object sender, EventArgs e)
        {
            _partNameTUVCSA = this.luPartNumberTUVCSA.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerTUVCSA))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameTUVCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerTUVCSA, _partNameTUVCSA);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerTUVCSA, _partNameTUVCSA);
                try
                {
                    this.luPowerTUVCSA.Properties.DataSource = ds.Tables[0];
                    this.luPowerTUVCSA.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerTUVCSA.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerTUVCSA.ItemIndex = 0;
                    }
                    this.txtDIfTUVCSA.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtCertificationTUVCSA.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();
                    this.txtTypeTUVCSA.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        this.cmbCellTypeTUVCSA.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        this.cmbCellTypeTUVCSA.Text = "Mono-Si";
                    }
                    _OnTUVCSAPartNumberChange(luPowerTUVCSA, e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeTUVCSA, comNoctTUVCSA);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        private void luPower_EditValueChanged(object sender, EventArgs e)
        {
            _power = this.luPower.EditValue.ToString();
            _partName = this.luPartNumber.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumner))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partName))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_power))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                //MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumner, _partName, _power);
                try
                {
                    this.txtVoc.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                    this.txtVmp.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                    this.txtIsc.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                    this.txtIpm.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                    this.txtFuse.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
                    this.txtPower.Text = this.luPower.EditValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }

        }
        private void luPowerAsm_EditValueChanged(object sender, EventArgs e)
        {
            _powerAsm = this.luPowerAsm.EditValue.ToString();
            _partNameAsm = this.luPartNumberAsm.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerAsm))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameAsm))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerAsm))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumnerAsm, _partNameAsm, _powerAsm);
                try
                {
                    this.txtVocAsm.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                    this.txtVmpAsm.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                    this.txtIscAsm.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                    this.txtIpmAsm.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                    this.txtFuseAsm.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
                    this.txtPowerAsm.Text = this.luPowerAsm.EditValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPowerCQC_EditValueChanged(object sender, EventArgs e)
        {
            _powerCQC = this.luPowerCQC.EditValue.ToString();
            _partNameCQC = this.luPartNumberCQC.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQC))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumnerCQC, _partNameCQC, _powerCQC);
                try
                {
                    this.txtVocCQC.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                    this.txtVmpCQC.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                    this.txtIscCQC.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                    this.txtIpmCQC.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                    this.txtFuseCQC.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
                    this.txtPowerCQC.Text = this.luPowerCQC.EditValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPowerCQClpz_EditValueChanged(object sender, EventArgs e)
        {
            _powerCQClpz = this.luPowerCQClpz.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQClpz))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQClpz))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCQClpz))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(this.txtTypeCQClpz.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            else
            {
                if (this.txtTypeCQClpz.Text.ToUpper().Trim().Equals("6612P"))
                {
                    if (_powerCQClpz.Equals("320"))
                    {
                        this.txtEffCQClpz.Text = "16.58";
                        this.txtEffCQClpz.Properties.ReadOnly = true;
                    }
                    if (_powerCQClpz.Equals("325"))
                    {
                        this.txtEffCQClpz.Text = "16.84";
                        this.txtEffCQClpz.Properties.ReadOnly = true;
                    }
                    if (_powerCQClpz.Equals("330"))
                    {
                        this.txtEffCQClpz.Text = "17.26";
                        this.txtEffCQClpz.Properties.ReadOnly = true;
                    }
                    else
                    {
                        this.txtEffCQClpz.Properties.ReadOnly = false;
                        this.txtEffCQClpz.Properties.Items.Clear();
                        this.txtEffCQClpz.Properties.Items.Add("16.58");
                        this.txtEffCQClpz.Properties.Items.Add("16.84");
                        this.txtEffCQClpz.Properties.Items.Add("17.26");
                        this.txtEffCQClpz.Properties.Items.Add("17.20");
                    }
                    this.txtMianjilpz.Properties.ReadOnly = true;
                    this.txtCellNumberCQClpz.Properties.ReadOnly = true;
                    this.txtMianjilpz.Text = "1.93";
                    this.txtCellNumberCQClpz.Text = "72";
                }
                else if (this.txtTypeCQClpz.Text.ToUpper().Trim().Equals("6610P"))
                {
                    if (_powerCQClpz.Equals("280"))
                    {
                        this.txtEffCQClpz.Text = "17.20";
                        this.txtEffCQClpz.Properties.ReadOnly = true;
                    }
                    else
                    {
                        this.txtEffCQClpz.Properties.ReadOnly = false;
                        this.txtEffCQClpz.Properties.Items.Clear();
                        this.txtEffCQClpz.Properties.Items.Add("16.58");
                        this.txtEffCQClpz.Properties.Items.Add("16.84");
                        this.txtEffCQClpz.Properties.Items.Add("17.26");
                        this.txtEffCQClpz.Properties.Items.Add("17.20");
                    }
                    this.txtMianjilpz.Text = "1.63";
                    this.txtCellNumberCQClpz.Text = "60";
                    this.txtMianjilpz.Properties.ReadOnly = true;
                    this.txtCellNumberCQClpz.Properties.ReadOnly = true;
                }
                else
                {
                    this.txtEffCQClpz.Properties.ReadOnly = false;
                    this.txtMianjilpz.Properties.ReadOnly = false;
                    this.txtCellNumberCQClpz.Properties.ReadOnly = false;
                    this.txtEffCQClpz.Properties.Items.Clear();
                    this.txtEffCQClpz.Properties.Items.Add("16.58");
                    this.txtEffCQClpz.Properties.Items.Add("16.84");
                    this.txtEffCQClpz.Properties.Items.Add("17.26");
                    this.txtEffCQClpz.Properties.Items.Add("17.20");
                    this.txtMianjilpz.Properties.Items.Clear();
                    this.txtMianjilpz.Properties.Items.Add("1.93");
                    this.txtMianjilpz.Properties.Items.Add("1.63");
                    this.txtCellNumberCQClpz.Properties.Items.Clear();
                    this.txtCellNumberCQClpz.Properties.Items.Add("72");
                    this.txtCellNumberCQClpz.Properties.Items.Add("60");
                }
            }
        }
        private void luPowerNe_EditValueChanged(object sender, EventArgs e)
        {
            _powerNe = this.luPowerNe.EditValue.ToString();
            _partNameNe = this.luPartNumberNe.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerNe))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameNe))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerNe))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }

            NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
            DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumnerNe, _partNameNe, _powerNe);
            try
            {
                this.txtVocNe.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                this.txtVmpNe.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                this.txtIscNe.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                this.txtIpmNe.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                this.txtFuseNe.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System error info");
                return;
            }

        }
        private void luPowerKorea_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.teOrderNoKorea.EditValue.ToString()))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(this.luPartNumberKorea.Text.ToString()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(this.luPowerKorea.EditValue.ToString()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }

            NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
            DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(this.teOrderNoKorea.EditValue.ToString(), this.luPartNumberKorea.Text.ToString(), this.luPowerKorea.EditValue.ToString());
            try
            {
                this.txtVocKorea.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                this.txtVmpKorea.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                this.txtIscKorea.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                this.txtIpmKorea.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                this.txtFuseKorea.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();

                string type = this.txtTypeC.Text.ToString();
                string power = this.luPowerKorea.Text.ToString();
                string[] l_s = new string[] { "Certificate_Date", "Type", "Certificate_No" };
                string category = "NameplateLabelPrint-01";
                DataTable dtCommon = BaseData.Get(l_s, category);
                DataTable dttype = dtCommon.Clone();

                DataRow[] drs = dtCommon.Select(string.Format("Type like '%{0}%' and Type like '%{1}%'",
                    type, power));
                foreach (DataRow dr in drs)
                    dttype.ImportRow(dr);
                this.textEdit1.Properties.DataSource = dttype;
                this.textEdit1.Properties.DisplayMember = "Type";
                this.textEdit1.Properties.ValueMember = "Type";
                if (dttype.Rows.Count > 0)
                {
                    this.textEdit1.ItemIndex = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System error info");
                return;
            }

        }
        private void luPowerCSA_EditValueChanged(object sender, EventArgs e)
        {
            _powerCSA = this.luPowerCSA.EditValue.ToString();
            _partNameCSA = this.luPartNumberCSA.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCSA))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumnerCSA, _partNameCSA, _powerCSA);
                try
                {
                    this.txtVocCSA.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                    this.txtVmpCSA.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                    this.txtIscCSA.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                    this.txtIpmCSA.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                    this.txtFuseCSA.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
                    this.txtPowerCSA.Text = this.luPowerCSA.EditValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPowerQT_EditValueChanged(object sender, EventArgs e)
        {
            _powerQT = this.luPowerQT.EditValue.ToString();
            _partNameQT = this.LuPartNumberQT.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerQT))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumnerQT, _partNameQT, _powerQT);
                try
                {
                    this.txtVocQT.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                    this.txtVmpQT.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                    this.txtIscQT.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                    this.txtIpmQT.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                    this.txtFuseQT.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
                    this.txtPowerQT.Text = this.luPowerQT.EditValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }
        private void luPowerTUVCSA_EditValueChanged(object sender, EventArgs e)
        {
            _powerTUVCSA = this.luPowerTUVCSA.EditValue.ToString();
            _partNameTUVCSA = this.luPartNumberTUVCSA.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerTUVCSA))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameTUVCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerTUVCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumnerTUVCSA, _partNameTUVCSA, _powerTUVCSA);
                try
                {
                    this.txtVocTUVCSA.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                    this.txtVmpTUVCSA.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                    this.txtIscTUVCSA.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                    this.txtIpmTUVCSA.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                    this.txtFuseTUVCSA.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
                    this.txtPowerTUVCSA.Text = this.luPowerTUVCSA.EditValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region 标签打印 new create by chao.pang 2015.10.19
            string typeTUV = radioGroup.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumner))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partName))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_power))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoct.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noct = this.comNoct.Text.Trim();
            }
            if (string.IsNullOrEmpty(cmbCellType.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            else
            {
                _cellType = cmbCellType.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMax.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _max = txtMax.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVoc.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _voc = txtVoc.Text.Trim();

            if (string.IsNullOrEmpty(txtVmp.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmp = txtVmp.Text.Trim();

            if (string.IsNullOrEmpty(txtIsc.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _isc = txtIsc.Text.Trim();
            if (string.IsNullOrEmpty(txtIpm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _imp = txtIpm.Text.Trim();
            if (string.IsNullOrEmpty(txtFuse.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuse = this.txtFuse.Text.Trim();
            if (string.IsNullOrEmpty(txtType.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPower.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtCertification.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtModuleType.Text.Trim()))
            {
                MessageBox.Show("组件类型不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtTolerance.Text.Trim()))
            {
                MessageBox.Show("Tolerance不能为空！", "System error info");
                return;
            }

            string bl = "";
            //if (txtModuleType.Text.Trim() == "1" || txtModuleType.Text.Trim() == "3")
            //{
            //    bl = " (BL)";
            //}
            string pscode = this.luPartNumber.EditValue.ToString();
            
            int i = pscode.LastIndexOf("-");
           
            pscode = pscode.Substring(0, i) + bl;

            string code = this.txtType.Text.ToUpper().Trim()
                + this.txtPower.Text.ToUpper().Trim()
                + this.txtCertification.Text.ToUpper().Trim()
                + this.txtModuleType.Text.ToUpper().Trim()
                + this.txtTolerance.Text.ToUpper().Trim();
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27

       
            _pordId = luPartNumber.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count > 0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();

                if (_template.Equals("PVLINE"))
                {
                    if (!typeTUV.Equals("PVLINE1000"))
                    {
                        DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为PVLINE，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (dr != DialogResult.Yes)
                        {
                            return;
                        }

                        
                    }
                }
                else if (_template.Equals("SOLAR"))//2018.04.24
                {
                    if (!typeTUV.Equals("SOLAR_JUICE1000") && !typeTUV.Equals("SOLAR_JUICE1500"))
                    {
                        DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为SOLAR_JUICE，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (dr != DialogResult.Yes)
                        {
                            return;
                        }
                    }
 
                }
                else if (_template.Equals("TUV_OLD"))//2018.04.24
                {
                    if (!typeTUV.Equals("常规1000(无产地标识)"))
                    {
                        DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为常规1000(无产地标识)，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (dr != DialogResult.Yes)
                        {
                            return;
                        }
                    }

                }
                else if (_template.Equals("TUV"))
                {
                    if (_pordId.Contains("HV"))
                    {
                        if (!typeTUV.Equals("常规1500"))
                        {
                            DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为常规1500，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                        }
                    }
                    else if (_pordId.Contains("HC"))
                    {
                        if (!typeTUV.Equals("常规1500"))
                        {
                            DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为常规1500，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                        }
                    }
                    else if (_pordId.Contains("DG") || _pordId.Contains("DGT"))
                    {
                        if (!typeTUV.Equals("双玻"))
                        {
                            DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为双玻，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                        }

                    }

                     //澳洲防火铭牌取消，TUV铭牌更新为新的防火铭牌 2018.5.25 应NPI韩苗、邵华华要求
                    //else if (_pordId.Contains("FR"))
                    //{
                    //    if (!typeTUV.Equals("澳洲防火"))
                    //    {

                    //        DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为澳洲防火，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    //        if (dr != DialogResult.Yes)
                    //        {
                    //            return;
                    //        }

                    //    }
                    //}
                    else
                    {
                        if (!typeTUV.Equals("常规1000(新)"))
                        {
                            DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为常规1000(新)，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                        }
                    }
                }


                else
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            if (!ModulePrint.PrintLabel(code, _voc, _isc, _vmp, _imp, _fuse, _max, _noct, _cellType, pscode, _power, this.txtDIf.Text, x, y,typeTUV))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
            #endregion
        }
        private void btnPrintAsm_Click(object sender, EventArgs e)
        {
            #region 标签打印 new create by chao.pang 2015.10.19
            if (string.IsNullOrEmpty(_orderNumnerAsm))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameAsm))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerAsm))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctAsm = this.comNoctAsm.Text.Trim();
            }
            if (string.IsNullOrEmpty(cmbCellTypeAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            else
            {
                _cellTypeAsm = cmbCellTypeAsm.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMaxAsm.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxAsm = txtMaxAsm.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocAsm = txtVocAsm.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpAsm = txtVmpAsm.Text.Trim();

            if (string.IsNullOrEmpty(txtIscAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscAsm = txtIscAsm.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impAsm = txtIpmAsm.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseAsm = this.txtFuseAsm.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerAsm.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtCertificationAsm.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtModuleTypeAsm.Text.Trim()))
            {
                MessageBox.Show("组件类型不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtToleranceAsm.Text.Trim()))
            {
                MessageBox.Show("Tolerance不能为空！", "System error info");
                return;
            }

            string pscode = "ASM" + this.txtTypeAsm.Text.ToUpper().Trim() + "C-" + this.txtPowerAsm.Text.ToUpper().Trim();

            string code = this.txtTypeAsm.Text.ToUpper().Trim() + "C"
                + this.txtPowerAsm.Text.ToUpper().Trim()
                + this.txtCertificationAsm.Text.ToUpper().Trim()
                + this.txtModuleTypeAsm.Text.ToUpper().Trim()
                + this.txtToleranceAsm.Text.ToUpper().Trim();
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            string namePlate = radioGroup1.EditValue.ToString();

            _pordId = luPartNumberAsm.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count > 0 )
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();

                //提示铭牌模板选错 yibin.fei 2017.11.27
                if (!_template.Equals(namePlate))
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            if (!ModulePrint.PrintLabel(code, _vocAsm, _iscAsm, _vmpAsm, _impAsm, _fuseAsm, _maxAsm, _noctAsm, _cellTypeAsm, pscode, _powerAsm, this.txtDIfAsm.Text, x, y, namePlate))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
          

            #endregion
        }
        private void btnPrintC_Click(object sender, EventArgs e)
        {
            #region 标签打印 new create by chao.pang 2016.03.25
            if (string.IsNullOrEmpty(_orderNumnerCQC))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctCQC = this.comNoctCQC.Text.Trim();
            }
            if (string.IsNullOrEmpty(cmbCellTypeCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            else
            {
                _cellTypeCQC = cmbCellTypeCQC.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMaxCQC.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxCQC = txtMaxCQC.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocCQC = txtVocCQC.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpCQC = txtVmpCQC.Text.Trim();

            if (string.IsNullOrEmpty(txtIscCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscCQC = txtIscCQC.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impCQC = txtIpmCQC.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseCQC = this.txtFuseCQC.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerCQC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtCertificationCQC.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtModuleTypeCQC.Text.Trim()))
            {
                MessageBox.Show("组件类型不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtToleranceCQC.Text.Trim()))
            {
                MessageBox.Show("Tolerance不能为空！", "System error info");
                return;
            }
            string pscode = this.luPartNumberCQC.EditValue.ToString();
            int i = pscode.LastIndexOf("-");
            pscode =  pscode.Substring(0, i) + "-" + this.txtPowerCQC.Text.ToUpper().Trim();

            string code = this.txtTypeCQC.Text.ToUpper().Trim()
                + this.txtPowerCQC.Text.ToUpper().Trim()
                + this.txtCertificationCQC.Text.ToUpper().Trim()
                + this.txtModuleTypeCQC.Text.ToUpper().Trim()
                + this.txtToleranceCQC.Text.ToUpper().Trim();
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }


            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = luPartNumberCQC.EditValue.ToString();
            string namePlate = radioGroupCQC.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count > 0 )
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals(namePlate))
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            if (!ModulePrint.PrintLabel(code, _vocCQC, _iscCQC, _vmpCQC, _impCQC, _fuseCQC, _maxCQC, _noctCQC, _cellTypeCQC, pscode, _powerCQC,
                this.txtDIfCQC.Text, x, y, namePlate))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
            #endregion
        }
        private void btnPrintClpz_Click(object sender, EventArgs e)
        {
            #region 标签打印 new create by chao.pang 2016.03.25
            if (string.IsNullOrEmpty(_orderNumnerCQClpz))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQClpz))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCQClpz))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtEffCQClpz.Text.Trim()))
            {
                MessageBox.Show("转换效率不能为空！", "System error info");
                return;
            }
            else
                _effCQClpz = txtEffCQClpz.Text.Trim();
            if (string.IsNullOrEmpty(txtMianjilpz.Text.Trim()))
            {
                MessageBox.Show("面积不能为空！", "System error info");
                return;
            }
            else
                _mianjiCQClpz = txtMianjilpz.Text.Trim();
            if (string.IsNullOrEmpty(txtCellNumberCQClpz.Text.Trim()))
            {
                MessageBox.Show("数量不能为空！", "System error info");
                return;
            }
            else
                _qtyCQClpz = txtCellNumberCQClpz.Text.Trim();
            string pscode = "CHSM" + this.txtTypeCQClpz.Text.ToUpper().Trim() + "-" + _powerCQClpz;

            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = luPartNumberCQClpz.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count >0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals("CQCLPZ"))
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            if (!ModulePrint.PrintLabel(pscode, _powerCQClpz, _effCQClpz, _mianjiCQClpz, _qtyCQClpz, x, y, "CQCLPZ"))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
            #endregion
        }
        private void btnNe_Click(object sender, EventArgs e)
        {
            #region 标签打印 new create by chao.pang 2016.05.04
            if (string.IsNullOrEmpty(_orderNumnerNe))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameNe))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerNe))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            //if (string.IsNullOrEmpty(comNoctNe.Text.Trim()))
            //{
            //    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
            //    return;
            //}
            //else
            //{
            //    _noctNe = this.comNoctNe.Text.Trim();
            //}
            if (string.IsNullOrEmpty(cmbCellTypeNe.Text.Trim()))
            {
                MessageBox.Show("种类不能为空！", "System error info");
                return;
            }
            else
            {
                _cellTypeNe = cmbCellTypeNe.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMaxNe.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxNe = txtMaxNe.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocNe.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocNe = txtVocNe.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpNe.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpNe = txtVmpNe.Text.Trim();

            if (string.IsNullOrEmpty(txtIscNe.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscNe = txtIscNe.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmNe.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impNe = txtIpmNe.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseNe.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseNe = this.txtFuseNe.Text.Trim();
            if (string.IsNullOrEmpty(txtDIfNe.Text.Trim()))
            {
                MessageBox.Show("分档方式不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtTypeNe.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            //if (string.IsNullOrEmpty(LupWeNe.Text.Trim()))
            //{
            //    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg011}"), MESSAGEBOX_CAPTION);//重量不能为空！
            //    return;
            //}
            //if (string.IsNullOrEmpty(lueCKGNe.Text.Trim()))
            //{
            //    MessageBox.Show("长宽高不能为空！", "System error info");
            //    return;
            //}
            if (string.IsNullOrEmpty(txtCKGNe.Text.Trim()))
            {
                MessageBox.Show("长宽高不能为空！", "System error info");
                return;
            }
            string flagType = string.Empty;
            string code = string.Empty;
            NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
            DataSet ds = namePlateLabelPrint.GetCellTypeByWorkOrderNumber(teOrderNoNe.Text.Trim());//通过工单号获取电池类型通过-N,-P区别是否是背钝化
            if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CELL_TYPE"].ToString().Contains("-N"))
                {
                    flagType = "-N";
                }
                if (ds.Tables[0].Rows[0]["CELL_TYPE"].ToString().Contains("-P"))
                {
                    flagType = "-P";
                }
            }
            if (string.IsNullOrEmpty(flagType))
            {
                DialogResult dr = MessageBox.Show("当前工单中没有体现电池是否为PERC,请选择是否为PERC电池组件！", "系统提示", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button3);
                if (dr == DialogResult.Yes)
                {
                    flagType = "-P";
                }
                else if (dr == DialogResult.No)
                {
                    flagType = "-N";
                }
                else {}

            }
            if (flagType == "-N")
            {
                if (this.txtTypeNe.Text.Trim().Contains("6610"))
                {
                    if (this.txtTypeNe.Text.Trim().Contains("M"))
                    {
                        code = "NERM156×156- -M SI "+ this.luPowerNe.Text.ToUpper().Trim()+"W";
                    }
                    else if (this.txtTypeNe.Text.Trim().Contains("P"))
                    {
                        code = "NERP156×156-60-P SI " + this.luPowerNe.Text.ToUpper().Trim() + "W";
                    }
                    else { MessageBox.Show("产品类型不对，为匹配单晶或多晶"); return; }
                    
                }
                else if (this.txtTypeNe.Text.Trim().Contains("6612"))
                {
                    if (this.txtTypeNe.Text.Trim().Contains("M"))
                    {
                        code = "NERM-CS6612M-" + this.luPowerNe.Text.ToUpper().Trim() + "W";
                    }
                    else if (this.txtTypeNe.Text.Trim().Contains("P"))
                    {
                        code = "NERP-CS6612P-" + this.luPowerNe.Text.ToUpper().Trim() + "W";
                    }
                    else { MessageBox.Show("产品类型不对，为匹配单晶或多晶"); return; }

                }
                else { MessageBox.Show("产品类型不对，为匹配6610或6612"); return; }
            } 
            else if (flagType == "-P")
            {
                if (this.txtTypeNe.Text.Trim().Contains("6610"))
                {
                    if (this.txtTypeNe.Text.Trim().Contains("M"))
                    {
                        code = "NER660M" + this.luPowerNe.Text.ToUpper().Trim();
                    }
                    else if (this.txtTypeNe.Text.Trim().Contains("P"))
                    {
                        code = "NER660P" + this.luPowerNe.Text.ToUpper().Trim() ;
                    }
                    else { MessageBox.Show("产品类型不对，为匹配单晶或多晶"); return; }

                }
                else if (this.txtTypeNe.Text.Trim().Contains("6612"))
                {
                    if (this.txtTypeNe.Text.Trim().Contains("M"))
                    {
                        code = "NER672M" + this.luPowerNe.Text.ToUpper().Trim();
                    }
                    else if (this.txtTypeNe.Text.Trim().Contains("P"))
                    {
                        code = "NER672P" + this.luPowerNe.Text.ToUpper().Trim() ;
                    }
                    else { MessageBox.Show("产品类型不对，为匹配单晶或多晶"); return; }

                }
                else { MessageBox.Show("产品类型不对，为匹配6610或6612"); return; }
            }
            
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = luPartNumberNe.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count >0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals("NE01"))
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            //if (!ModulePrint.PrintLabel(code, _powerNe, _impNe, _vmpNe, _iscNe, _vocNe, _noctNe, txtDIfNe.Text.Trim(), LupWeNe.Text.Trim(), lueCKGNe.Text.Trim(), _cellTypeNe, x, y, "NE"))
            //{
            //    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
            //    return;
            //}
            if (!ModulePrint.PrintLabel(code, _powerNe, _impNe, _vmpNe, _iscNe, _vocNe, txtDIfNe.Text.Trim(), txtCKGNe.Text.Trim(), txtMaxNe.Text.Trim(), _cellTypeNe, x, y, "NE01"))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
            #endregion
        }
        private void btnPrintKorea_Click(object sender, EventArgs e)
        {
            #region 标签打印 new create by chao.pang 2016.05.04
            string typeTUV = txtTol.Text.ToString();
            if (string.IsNullOrEmpty(teOrderNoKorea.Text.Trim()))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(luPartNumberKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(luPowerKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            if (string.IsNullOrEmpty(cmbCellTypeKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtMaxKorea.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtVocKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtVmpKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtIscKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtIpmKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtFuseKorea.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtDIfKorea.Text.Trim()))
            {
                MessageBox.Show("分档方式不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtCNo.Text.Trim()))
            {
                MessageBox.Show("证书号不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtCDate.Text.Trim()))
            {
                MessageBox.Show("证书日期不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtModuleTypeC.Text.Trim()))
            {
                MessageBox.Show("版本第一位不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtToleranceC.Text.Trim()))
            {
                MessageBox.Show("版本第二位不能为空！", "System error info");
                return;
            }


            string pscode = txtTypeC.Text.Trim() + textEdit2.Text.Trim() + luPowerKorea.Text.Trim() + txtTol.Text.Trim() + txtModuleTypeC.Text.Trim() + txtToleranceC.Text.Trim();
            //int i = pscode.IndexOf("-");
            //pscode = pscode.Substring(0, i);

            string code = textEdit1.Text.Trim();
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = luPartNumberKorea.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count >0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (_template.Equals("Korea"))
                {

                    if (!typeTUV.Equals("KEMCO"))
                    {
                        DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为KEMCO，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (dr != DialogResult.Yes)
                        {
                            return;
                        }
                    }
                  

                }
                else if (_template.Equals("KoreaKS"))
                {
                    if (!typeTUV.Equals("KS"))
                    {
                        DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为KS，是否继续打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (dr != DialogResult.Yes)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！请检查是否选择正确的铭牌模板。是否继续当前打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.Yes)
                    {

                        _template = typeTUV;

                    }
                    else 
                    {
                        return;
                    }
                }
            }
            if (!ModulePrint.PrintLabel(pscode, luPowerKorea.Text.Trim(), txtVocKorea.Text.Trim(), txtIscKorea.Text.Trim(), txtVmpKorea.Text.Trim(), txtIpmKorea.Text.Trim()
                , txtMaxKorea.Text.Trim(), txtDIfKorea.Text.Trim(), cmbCellTypeKorea.Text.Trim(), txtCNo.Text.Trim(), txtCDate.Text.Trim(), txtFuseKorea.Text.Trim(),
                x, y, _template, code))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
            #endregion
        }
        private void btnPrintCSA_Click(object sender, EventArgs e)
        {
            #region 标签打印 new create by chao.pang 2016.05.10
            if (string.IsNullOrEmpty(_orderNumnerCSA))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctCSA = this.comNoctCSA.Text.Trim();
            }
            if (string.IsNullOrEmpty(cmbCellTypeCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            else
            {
                _cellTypeCSA = cmbCellTypeCSA.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMaxCSA.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxCSA = txtMaxCSA.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocCSA = txtVocCSA.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpCSA = txtVmpCSA.Text.Trim();

            if (string.IsNullOrEmpty(txtIscCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscCSA = txtIscCSA.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impCSA = txtIpmCSA.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseCSA = this.txtFuseCSA.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtCertificationCSA.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtModuleTypeCSA.Text.Trim()))
            {
                MessageBox.Show("组件类型不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtToleranceCSA.Text.Trim()))
            {
                MessageBox.Show("Tolerance不能为空！", "System error info");
                return;
            }

            string pscode = this.luPartNumberCSA.EditValue.ToString();
            int i = pscode.IndexOf("-");
            pscode = pscode.Substring(0, i);

            string code = this.txtTypeCSA.Text.ToUpper().Trim()
                + this.txtPowerCSA.Text.ToUpper().Trim()
                + this.txtCertificationCSA.Text.ToUpper().Trim()
                + this.txtModuleTypeCSA.Text.ToUpper().Trim()
                + this.txtToleranceCSA.Text.ToUpper().Trim();
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = luPartNumberCSA.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count  >0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals("CSA"))
                {
                  DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            if (!ModulePrint.PrintLabel(code, _vocCSA, _iscCSA, _vmpCSA, _impCSA, _fuseCSA, _maxCSA, _noctCSA, _cellTypeCSA, pscode, _powerCSA, this.txtDIfCSA.Text, x, y, "CSA"))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
            #endregion
        }
        private void btnPrintTUVCSA_Click(object sender, EventArgs e)
        {
            #region 标签打印 new create by chao.pang 2017.07.25
            string typeTUVCSA = radioGroupTUVCSA.EditValue.ToString();
            if (string.IsNullOrEmpty(typeTUVCSA))
            {
                MessageBox.Show("请选择铭牌类型！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(_orderNumnerTUVCSA))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameTUVCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerTUVCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctTUVCSA = this.comNoctTUVCSA.Text.Trim();
            }
            if (string.IsNullOrEmpty(cmbCellTypeTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            else
            {
                _cellTypeTUVCSA = cmbCellTypeTUVCSA.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMaxTUVCSA.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxTUVCSA = txtMaxTUVCSA.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocTUVCSA = txtVocTUVCSA.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpTUVCSA = txtVmpTUVCSA.Text.Trim();

            if (string.IsNullOrEmpty(txtIscTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscTUVCSA = txtIscTUVCSA.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impTUVCSA = txtIpmTUVCSA.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseTUVCSA = this.txtFuseTUVCSA.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerTUVCSA.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtCertificationTUVCSA.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }

            string pscode = this.luPartNumberTUVCSA.EditValue.ToString();
            int i = pscode.IndexOf("-");
            pscode = pscode.Substring(0, i);

            string code = string.Empty;
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = luPartNumberTUVCSA.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count >0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals("TUV/CSA"))
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
                else
                {
                    if (_pordId.Contains("HV"))
                    {
                        if (!typeTUVCSA.Equals("TUVCSA1500"))
                        {

                            DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为1500V。是否继续当前打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!typeTUVCSA.Equals("TUVCSA1000"))
                        {
                            DialogResult dr = MessageBox.Show("打印铭牌模板选择错误！该铭牌应为1000V。是否继续当前打印？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                        }
                        
                    }
                }
            }
            if (!ModulePrint.PrintLabel(code, _vocTUVCSA, _iscTUVCSA, _vmpTUVCSA, _impTUVCSA, _fuseTUVCSA, _maxTUVCSA, _noctTUVCSA, _cellTypeTUVCSA, pscode, _powerTUVCSA, this.txtDIfTUVCSA.Text, x, y, typeTUVCSA))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
            #endregion
        }


        //add by Cheney Yang 
        Dictionary<string, string> map = new Dictionary<string, string>
       {
           {"6610P","30"},
           {"6612P","36"}
       };
        private void PrintQX()
        {
            if (string.IsNullOrEmpty(_orderNumnerQT))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                //MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctQT = this.comNoctQT.Text.Trim();
            }

            if (string.IsNullOrEmpty(txtMaxQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                //MessageBox.Show("最大系统电压不能为空！", "System error info");
                return;
            }
            else
            {
                _maxQT = txtMaxQT.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                //MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocQT = txtVocQT.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpQT = txtVmpQT.Text.Trim();

            if (string.IsNullOrEmpty(txtIscQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscQT = txtIscQT.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！              
                return;
            }
            else
                _impQT = txtIpmQT.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                
                return;
            }
            else
                _fuseQT = this.txtFuseQT.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtModuleTypeQT.Text.Trim()))
            {
                MessageBox.Show("组件类型不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtCertificationQT.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }

            string pscode = string.Format("SL{0}TU-{1}P", txtPowerQT.Text.Trim(), map[txtTypeQT.Text.Trim()]);


            string code = this.txtTypeQT.Text.ToUpper().Trim()
                + this.txtPowerQT.Text.ToUpper().Trim()
                + this.txtCertificationQT.Text.ToUpper().Trim()
                + this.txtModuleTypeQT.Text.ToUpper().Trim()
                + this.txtToleranceQT.Text.ToUpper().Trim();


            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = LuPartNumberQT.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count > 0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals("QX"))
                {
                   DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
               
            }

            if (!ModulePrint.PrintLabel(code, _vocQT, _iscQT, _vmpQT, _impQT, _fuseQT, _maxQT, _noctQT, _cellTypeQT, pscode, _powerQT,
                this.txtDIfQT.Text, x, y, "QX"))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }

        }
        private void PrintQTX()
        {
            if (string.IsNullOrEmpty(_orderNumnerQT))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctQT = this.comNoctQT.Text.Trim();
            }

            if (string.IsNullOrEmpty(txtMaxQT.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxQT = txtMaxQT.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocQT = txtVocQT.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpQT = txtVmpQT.Text.Trim();

            if (string.IsNullOrEmpty(txtIscQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscQT = txtIscQT.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impQT = txtIpmQT.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseQT = this.txtFuseQT.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            
            if (string.IsNullOrEmpty(cmdNoct.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            if (string.IsNullOrEmpty(cmdWeight.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg011}"), MESSAGEBOX_CAPTION);//重量不能为空！
                return;
            }
            if (string.IsNullOrEmpty(cmdcc.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg012}"), MESSAGEBOX_CAPTION);//尺寸不能为空！
                return;
            }

            string pscode = "QTSM" + this.txtTypeQT.Text.ToUpper().Trim() + "-" + this.txtPowerQT.Text.ToUpper().Trim();

            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = LuPartNumberQT.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count > 0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals("QTX"))
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            if (!ModulePrint.PrintLabel(_vocQT, _iscQT, _vmpQT, _impQT, _fuseQT, _maxQT, _noctQT, _cellTypeQT, pscode, _powerQT,cmdNoct.Text.Trim(),cmdWeight.Text.Trim(),cmdcc.Text.Trim(),
                this.txtDIfQT.Text, x, y, "QTX"))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                //MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }

        }
        private void btnPrintQT_Click(object sender, EventArgs e)
        {
            if (rbType.EditValue.ToString().Trim() == "QX")
            {
                PrintQX();
                return;
            }
            if (rbType.EditValue.ToString().Trim() == "QTX")
            {
                PrintQTX();
                return;
            }

            #region 标签打印 new create by chao.pang 2016.03.25
            if (string.IsNullOrEmpty(_orderNumnerQT))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerQT))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctQT = this.comNoctQT.Text.Trim();
            }
            if (string.IsNullOrEmpty(cmbCellTypeQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            else
            {
                _cellTypeQT = cmbCellTypeQT.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMaxQT.Text.Trim()))
            {
MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxQT = txtMaxQT.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocQT = txtVocQT.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpQT = txtVmpQT.Text.Trim();

            if (string.IsNullOrEmpty(txtIscQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscQT = txtIscQT.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impQT = txtIpmQT.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseQT = this.txtFuseQT.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerQT.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtCertificationQT.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtModuleTypeQT.Text.Trim()))
            {
                MessageBox.Show("组件类型不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtToleranceQT.Text.Trim()))
            {
                MessageBox.Show("Tolerance不能为空！", "System error info");
                return;
            }
            //带不出Type，貌似异常代码
            //string pscode = "QTSM" + this.Text.ToUpper().Trim() + "-" + this.txtPowerQT.Text.ToUpper().Trim();
            //jiabao.liu 2016-09-08
            string pscode = "QTSM" + this.txtTypeQT.Text.ToUpper().Trim() + "-" + this.txtPowerQT.Text.ToUpper().Trim();

            string code = "QTSM" + this.txtTypeQT.Text.ToUpper().Trim()
                + this.txtPowerQT.Text.ToUpper().Trim()
                + this.txtCertificationQT.Text.ToUpper().Trim()
                + this.txtModuleTypeQT.Text.ToUpper().Trim()
                + this.txtToleranceQT.Text.ToUpper().Trim();
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = LuPartNumberQT.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count >0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals("QT") )
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    //DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            
            if (!ModulePrint.PrintLabel(code, _vocQT, _iscQT, _vmpQT, _impQT, _fuseQT, _maxQT, _noctQT, _cellTypeQT, pscode, _powerQT,
                this.txtDIfQT.Text, x, y, "QT"))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
            #endregion
        }

        private void NameplateLabelPrint_Load(object sender, EventArgs e)
        {
            BindWorkNumber();
            BindNameplateLabelPrintTem();
        }

        //private void txtTypeNe_EditValueChanged(object sender, EventArgs e)
        //{
        //    string type = this.txtTypeNe.Text.ToString();
        //    string[] l_s = new string[] { "Type", "WEIGHT", "CODE", "Volume" };
        //    string category = "NameplateLabelPrint";
        //    DataTable dtCommon = BaseData.Get(l_s, category);
        //    DataTable dttype = dtCommon.Clone();
        //    string flag = "0";
        //    if (this.checkTrueOrFalse.Checked == true)
        //        flag = "1";

        //    DataRow[] drs = dtCommon.Select(string.Format("Type = '{0}' and CODE = '{1}'",
        //        type.Substring(0, type.Length - 1), flag));
        //    foreach (DataRow dr in drs)
        //        dttype.ImportRow(dr);
        //    this.LupWeNe.Properties.DataSource = dttype;
        //    this.LupWeNe.Properties.DisplayMember = "WEIGHT";
        //    this.LupWeNe.Properties.ValueMember = "WEIGHT";
        //    if (dttype.Rows.Count > 0)
        //    {
        //        this.LupWeNe.ItemIndex = 0;
        //    }
        //    this.lueCKGNe.Properties.DataSource = dttype;
        //    this.lueCKGNe.Properties.DisplayMember = "Volume";
        //    this.lueCKGNe.Properties.ValueMember = "Volume";
        //    if (dttype.Rows.Count > 0)
        //    {
        //        this.lueCKGNe.ItemIndex = 0;
        //    }
        //}
        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            string text = textEdit1.Text.Trim();
            int i = text.IndexOf("PW", 0, text.Length);
            int J = text.IndexOf("K1", 0, text.Length);
            int K = text.IndexOf("K2", 0, text.Length);
            int M = text.IndexOf("K3", 0, text.Length);

            if (i > 0) textEdit2.Text = text.Substring(i, 2);
            else if (J > 0) textEdit2.Text = text.Substring(J, 2);
            else if (K > 0) textEdit2.Text = text.Substring(K, 2);
            else if (M > 0) textEdit2.Text = text.Substring(M, 2);
            else textEdit2.Text = "";

            string type = this.textEdit1.Text.ToString();
            string[] l_s = new string[] { "Certificate_Date", "Type", "Certificate_No" };
            string category = "NameplateLabelPrint-01";
            DataTable dtCommon = BaseData.Get(l_s, category);
            DataTable dttype = dtCommon.Clone();

            DataRow[] drs = dtCommon.Select(string.Format("Type = '{0}'", type
                ));
            foreach (DataRow dr in drs)
                dttype.ImportRow(dr);
            this.txtCDate.Properties.DataSource = dttype;
            this.txtCDate.Properties.DisplayMember = "Certificate_Date";
            this.txtCDate.Properties.ValueMember = "Certificate_Date";
            if (dttype.Rows.Count > 0)
            {
                this.txtCDate.ItemIndex = 0;
            }
            this.txtCNo.Properties.DataSource = dttype;
            this.txtCNo.Properties.DisplayMember = "Certificate_No";
            this.txtCNo.Properties.ValueMember = "Certificate_No";
            if (dttype.Rows.Count > 0)
            {
                this.txtCNo.ItemIndex = 0;
            }
        }

        /// <summary>
        /// 根据产品型号绑定温度
        /// </summary>
        private void BindNameplateLabelPrintTem()
        {
            string[] l_s = new string[] { "ProductType", "TemNoct" };
            string category = "NameplateLabelPrint_Tem";
            dtNameplateLabelPrint = BaseData.Get(l_s, category);
        }

        /// <summary>
        /// 统一根据型号获取温度，jiabao 2016-09-14
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="txt"></param>
        /// <param name="cmb"></param>
        private void GetNotcTemByType(DataTable dt, TextEdit txt, ComboBoxEdit cmb)
        {
            //根据配置数据匹配温度
            DataTable dtTem = new DataTable();
            DataRow[] drTem = dt.Select(string.Format("ProductType='{0}'", txt.Text));
            try
            {
                dtTem = drTem.CopyToDataTable();
            }
            catch
            {
                cmb.Properties.Items.Clear();
                MessageBox.Show(string.Format("没有找到【{0}】对应的维护，请联系IT到基础数据NameplateLabelPrint_Tem添加对应的数据", txt.Text));
                return;
            }
            int TemCount = dtTem.Rows.Count;
            //此处加循环加入值防止以后一个产品型号对应多个温度，这里做了一个防呆
            cmb.Properties.Items.Clear();
            for (int i = 0; i < TemCount; i++)
            {
                cmb.Properties.Items.Add(dtTem.Rows[i]["TemNoct"].ToString());
            }
            cmb.SelectedIndex = 0;

        }

        private void rbType_Properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbType.EditValue.ToString() == "QTX")
            {
                lyciNoct.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lyciweight.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lycicc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem157.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem158.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else
            {
                lyciNoct.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lyciweight.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lycicc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem157.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem158.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
        }

        private void radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup.EditValue.ToString() == "常规1000(新)")
            {
                this.txtMax.Text = "1000";
            }
            if (this.radioGroup.EditValue.ToString() == "常规1000(无产地标识)")
            {
                this.txtMax.Text = "1000";
            }
            if (this.radioGroup.EditValue.ToString() == "常规1500")
            {
                this.txtMax.Text = "1500";
            }
            if (this.radioGroup.EditValue.ToString() == "双玻")
            {
                this.txtMax.Text = "1500";
            }
            if (this.radioGroup.EditValue.ToString() == "澳洲防火")
            {
                this.txtMax.Text = "1000";
            }
            if (this.radioGroup.EditValue.ToString() == "SOLAR_JUICE1500")
            {
                this.txtMax.Text = "1500";
            }
            if (this.radioGroup.EditValue.ToString() == "SOLAR_JUICE1000")
            {
                this.txtMax.Text = "1000";
            }
           
            
        }

        private void txtTypeNe_EditValueChanged(object sender, EventArgs e)
        {
            if (txtTypeNe.Text.Contains("6610"))
            {
                txtCKGNe.Text = "1650mm×991mm×40mm";
            }
            else if (txtTypeNe.Text.Contains("6612"))
            {
                txtCKGNe.Text = "1956mm×991mm×45mm";
            }
        }

        private void radioGroupTUVCSA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroupTUVCSA.EditValue.ToString() == "TUVCSA1000")
            {
                this.txtMaxTUVCSA.Text = "1000";
            }
            if (this.radioGroupTUVCSA.EditValue.ToString() == "TUVCSA1500")
            {
                this.txtMaxTUVCSA.Text = "1500";
            }            
        }

        //安能铭牌打印 yibin.fei
        #region
        private void teOrderNoCQC_AN_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerCQC = this.teOrderNoCQC_AN.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQC))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerCQC);
                try
                {
                    this.luPartNumberCQC_AN.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberCQC_AN.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberCQC_AN.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberCQC_AN.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        private void luPartNumberCQC_AN_EditValueChanged(object sender, EventArgs e)
        {
            _partNameCQC = this.luPartNumberCQC_AN.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQC))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerCQC, _partNameCQC);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerCQC, _partNameCQC);
                try
                {
                    this.luPowerCQC_AN.Properties.DataSource = ds.Tables[0];
                    this.luPowerCQC_AN.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerCQC_AN.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerCQC_AN.ItemIndex = 0;
                    }
                    this.txtDIfCQC_AN.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtCertificationCQC_AN.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();
                    this.txtTypeCQC_AN.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //this.comNoctCQC.Properties.Items.Clear();
                        //this.comNoctCQC.Properties.Items.Add("43");
                        //this.comNoctCQC.Properties.Items.Add("46");
                        this.cmbCellTypeCQC_AN.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoctCQC.Properties.Items.Clear();
                        //this.comNoctCQC.Properties.Items.Add("47");
                        //this.comNoctCQC.Properties.Items.Add("48");
                        this.cmbCellTypeCQC_AN.Text = "Mono-Si";
                    }
                    _OnNormalCQC_ANPartNumberChange(luPowerCQC_AN, e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeCQC_AN, comNoctCQC_AN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        private void luPowerCQC_AN_EditValueChanged(object sender, EventArgs e)
        {
            _powerCQC = this.luPowerCQC_AN.EditValue.ToString();
            _partNameCQC = this.luPartNumberCQC_AN.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCQC))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumnerCQC, _partNameCQC, _powerCQC);
                try
                {
                    this.txtVocCQC_AN.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                    this.txtVmpCQC_AN.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                    this.txtIscCQC_AN.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                    this.txtIpmCQC_AN.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                    this.txtFuseCQC_AN.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
                    this.txtPowerCQC_AN.Text = this.luPowerCQC_AN.EditValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        private void btnPrintC_AN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_orderNumnerCQC))
            {
                                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCQC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctCQC = this.comNoctCQC_AN.Text.Trim();
            }
            if (string.IsNullOrEmpty(cmbCellTypeCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                //MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            else
            {
                _cellTypeCQC = cmbCellTypeCQC_AN.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMaxCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxCQC = txtMaxCQC_AN.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocCQC = txtVocCQC_AN.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpCQC = txtVmpCQC_AN.Text.Trim();

            if (string.IsNullOrEmpty(txtIscCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscCQC = txtIscCQC_AN.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impCQC = txtIpmCQC_AN.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseCQC = this.txtFuseCQC_AN.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerCQC_AN.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtCertificationCQC_AN.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtModuleTypeCQC_AN.Text.Trim()))
            {
                MessageBox.Show("组件类型不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtToleranceCQC_AN.Text.Trim()))
            {
                MessageBox.Show("Tolerance不能为空！", "System error info");
                return;
            }

            string pscode = "CHSM" + this.txtTypeCQC_AN.Text.ToUpper().Trim() + "-" + this.txtPowerCQC_AN.Text.ToUpper().Trim();

            string code = this.txtTypeCQC_AN.Text.ToUpper().Trim()
                + this.txtPowerCQC_AN.Text.ToUpper().Trim()
                + this.txtCertificationCQC_AN.Text.ToUpper().Trim()
                + this.txtModuleTypeCQC_AN.Text.ToUpper().Trim()
                + this.txtToleranceCQC_AN.Text.ToUpper().Trim();
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }

            _pordId = luPartNumberCQC_AN.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count > 0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();

                //提示铭牌模板选错 yibin.fei 2017.11.27
                if (!_template.Equals("CQC_AN"))
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            if (!ModulePrint.PrintLabel(code, _vocCQC, _iscCQC, _vmpCQC, _impCQC, _fuseCQC, _maxCQC, _noctCQC, _cellTypeCQC, pscode, _powerCQC,
                this.txtDIfCQC_AN.Text, x, y, "CQC_AN"))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                //MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
        }

     

        private void radioGroupKorea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroupKorea.EditValue.ToString() == "Korea")
            {
                this.txtTol.Text = "KEMCO";
            }
            else if (this.radioGroupKorea.EditValue.ToString() == "KoreaKS")
            {
                this.txtTol.Text = "KS";
            }
        }

        private void txtCertificationCQC_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void radioGroupCQC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroupCQC.EditValue.ToString() == "CEC1500")
            {
                txtMaxCQC.Text = "1500";
            }
            else
            {
                txtMaxCQC.Text = "1000";
            }
        }

        private void teOrderNoCEC_EditValueChanged(object sender, EventArgs e)
        {
            _orderNumnerCEC = this.teOrderNoCEC.EditValue.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                //                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetMatrialByWorkOrderNumber(_orderNumnerCEC);
                try
                {
                    this.luPartNumberCEC.Properties.DataSource = ds.Tables[0];
                    this.luPartNumberCEC.Properties.DisplayMember = "PART_NUMBER";
                    this.luPartNumberCEC.Properties.ValueMember = "PRODUCT_CODE";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPartNumberCEC.ItemIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        private void luPartNumberCEC_EditValueChanged(object sender, EventArgs e)
        {
            _partNameCEC = this.luPartNumberCEC.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetPowerByWOnumberAndPartID(_orderNumnerCEC, _partNameCEC);
                DataSet ds01 = namePlateLabelPrint.GetInfByWOnumberAndPartID(_orderNumnerCEC, _partNameCEC);
                try
                {
                    this.luPowerCEC.Properties.DataSource = ds.Tables[0];
                    this.luPowerCEC.Properties.DisplayMember = "PMAXSTAB";
                    this.luPowerCEC.Properties.ValueMember = "PMAXSTAB";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.luPowerCEC.ItemIndex = 0;
                    }
                    this.txtDIfCEC.Text = ds01.Tables[0].Rows[0]["TOLERANCE"].ToString();
                    this.txtCertificationCEC.Text = ds01.Tables[0].Rows[0]["LABELTYPE"].ToString();
                    this.txtTypeCEC.Text = ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                    if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("P"))
                    {
                        //this.comNoctCQC.Properties.Items.Clear();
                        //this.comNoctCQC.Properties.Items.Add("43");
                        //this.comNoctCQC.Properties.Items.Add("46");
                        this.cmbCellTypeCEC.Text = "Poly-Si";
                    }
                    else if (ds01.Tables[0].Rows[0]["PROMODEL_NAME"].ToString().ToUpper().Contains("M"))
                    {
                        //this.comNoctCQC.Properties.Items.Clear();
                        //this.comNoctCQC.Properties.Items.Add("47");
                        //this.comNoctCQC.Properties.Items.Add("48");
                        this.cmbCellTypeCEC.Text = "Mono-Si";
                    }
                    _OnNormalCECPartNumberChange(luPowerCEC, e);
                    GetNotcTemByType(dtNameplateLabelPrint, txtTypeCEC, comNoctCEC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
                if (luPartNumberCEC.EditValue.ToString().Contains("HV"))
                {
                    txtMaxCEC.Text = "1500";
                }
                else
                {
                    txtMaxCEC.Text = "1000";
                }
            }
        }

        private void luPowerCEC_EditValueChanged(object sender, EventArgs e)
        {
            _powerCEC = this.luPowerCEC.EditValue.ToString();
            _partNameCEC = this.luPartNumberCEC.Text.ToString();
            if (string.IsNullOrEmpty(_orderNumnerCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            else
            {
                NameplateLabelPrintEngine namePlateLabelPrint = new NameplateLabelPrintEngine();
                DataSet ds = namePlateLabelPrint.GetInfByWOnumberAndPartIDAndPower(_orderNumnerCEC, _partNameCEC, _powerCEC);
                try
                {
                    this.txtVocCEC.Text = ds.Tables[0].Rows[0]["VOCSTAB"].ToString();
                    this.txtVmpCEC.Text = ds.Tables[0].Rows[0]["VMPPSTAB"].ToString();
                    this.txtIscCEC.Text = ds.Tables[0].Rows[0]["ISCSTAB"].ToString();
                    this.txtIpmCEC.Text = ds.Tables[0].Rows[0]["IMPPSTAB"].ToString();
                    this.txtFuseCEC.Text = ds.Tables[0].Rows[0]["FUSE"].ToString();
                    this.txtPowerCEC.Text = this.luPowerCEC.EditValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        private void radioGroupCEC_EditValueChanged(object sender, EventArgs e)
        {
            if (this.radioGroupCEC.EditValue.ToString() == "CEC1500")
            {
                txtMaxCEC.Text = "1500";
            }
            else
            {
                txtMaxCEC.Text = "1000";
            }
        }

        private void btnPrintCEC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_orderNumnerCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                return;
            }
            if (string.IsNullOrEmpty(_partNameCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PackingListPrint.Msg010}"), MESSAGEBOX_CAPTION);//料号不能为空，请确认！
                return;
            }
            if (string.IsNullOrEmpty(_powerCEC))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg001}"), MESSAGEBOX_CAPTION);//额定最大功率不能为空！
                return;
            }
            if (string.IsNullOrEmpty(comNoctCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg002}"), MESSAGEBOX_CAPTION);//温度不能为空！
                return;
            }
            else
            {
                _noctCEC = this.comNoctCEC.Text.Trim();
            }
            if (string.IsNullOrEmpty(cmbCellTypeCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg015}"), MESSAGEBOX_CAPTION);//电池片类型不能为空！
                return;
            }
            else
            {
                _cellTypeCEC = cmbCellTypeCEC.Text.Trim();

            }
            if (string.IsNullOrEmpty(txtMaxCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg003}"), MESSAGEBOX_CAPTION);//最大系统电压不能为空！
                return;
            }
            else
            {
                _maxCEC = txtMaxCEC.Text.Trim();
            }
            if (string.IsNullOrEmpty(txtVocCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg004}"), MESSAGEBOX_CAPTION);//开路电压不能为空！
                return;
            }
            else
                _vocCEC = txtVocCEC.Text.Trim();

            if (string.IsNullOrEmpty(txtVmpCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg005}"), MESSAGEBOX_CAPTION);//额定电压不能为空！
                return;
            }
            else
                _vmpCEC = txtVmpCEC.Text.Trim();

            if (string.IsNullOrEmpty(txtIscCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg006}"), MESSAGEBOX_CAPTION);//短路电流不能为空！
                return;
            }
            else
                _iscCEC = txtIscCEC.Text.Trim();
            if (string.IsNullOrEmpty(txtIpmCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg007}"), MESSAGEBOX_CAPTION);//额定电流不能为空！
                return;
            }
            else
                _impCEC = txtIpmCEC.Text.Trim();
            if (string.IsNullOrEmpty(txtFuseCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg008}"), MESSAGEBOX_CAPTION);//Fuse不能为空！
                return;
            }
            else
                _fuseCEC = this.txtFuseCEC.Text.Trim();
            if (string.IsNullOrEmpty(txtTypeCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg009}"), MESSAGEBOX_CAPTION);//产品类型不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtPowerCEC.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg010}"), MESSAGEBOX_CAPTION);//档位不能为空！
                return;
            }
            if (string.IsNullOrEmpty(txtCertificationCEC.Text.Trim()))
            {
                MessageBox.Show("认证不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtModuleTypeCEC.Text.Trim()))
            {
                MessageBox.Show("组件类型不能为空！", "System error info");
                return;
            }
            if (string.IsNullOrEmpty(txtToleranceCEC.Text.Trim()))
            {
                MessageBox.Show("Tolerance不能为空！", "System error info");
                return;
            }
            string pscode = this.luPartNumberCEC.EditValue.ToString();
            int i = pscode.LastIndexOf("-");
            pscode = pscode.Substring(0, i) + "-" + this.txtPowerCEC.Text.ToUpper().Trim();

            string code = this.txtTypeCEC.Text.ToUpper().Trim()
                + this.txtPowerCEC.Text.ToUpper().Trim()
                + this.txtCertificationCEC.Text.ToUpper().Trim()
                + this.txtModuleTypeCEC.Text.ToUpper().Trim()
                + this.txtToleranceCEC.Text.ToUpper().Trim();
            int x = 0;
            int y = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.ToString()))
            {
                x = Convert.ToInt32(this.txtX.Text);
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.ToString()))
            {
                y = Convert.ToInt32(this.txtY.Text);
            }


            //提示铭牌模板选错 yibin.fei 2017.11.27
            _pordId = luPartNumberCEC.EditValue.ToString();
            string namePlate = radioGroupCEC.EditValue.ToString();
            DataSet dsTemplate = namePlateLabelAutoPrint.getTemplateByProdId(_pordId);
            if (dsTemplate.Tables[0].Rows.Count > 0)
            {
                _template = dsTemplate.Tables[0].Rows[0]["NAME_TEMPLATE"].ToString();
                if (!_template.Equals(namePlate))
                {
                    DialogResult dr = MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg013}"), MESSAGEBOX_CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);//打印铭牌模板选择错误！请检查物料号是否选择正确、选择正确的铭牌模板。是否继续当前打印？
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            if (!ModulePrint.PrintLabel(code, _vocCEC, _iscCEC, _vmpCEC, _impCEC, _fuseCEC, _maxCEC, _noctCEC, _cellTypeCEC, pscode, _powerCEC,
                this.txtDIfCEC.Text, x, y, namePlate))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.NameplateLabelPrint.Msg014}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                return;
            }
        }

     

       



    }
        #endregion
}
