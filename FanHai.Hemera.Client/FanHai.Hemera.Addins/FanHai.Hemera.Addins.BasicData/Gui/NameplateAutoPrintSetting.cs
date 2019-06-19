using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class NameplateAutoPrintSetting : UserControl
    {
        NameplateLabelAutoPrintEntity entity = new NameplateLabelAutoPrintEntity();

        DataSet dsInfo = null;
        string editor = string.Empty;


        public NameplateAutoPrintSetting()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            labelControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.lbl.0001}");//铭牌自动打印设置
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.lbl.0002}");//产品代码
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.lbl.0003}");//铭牌模板
            btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.lbl.0004}");//查询
            btnAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.lbl.0005}");//新增
            btnUpdate.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.lbl.0006}");//修改
            gridColumn1.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.GridControl.0001}");//产品代码
            gridColumn2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.GridControl.0002}");//铭牌模板
            gridColumn3.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.GridControl.0003}");//修改人
            gridColumn4.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.GridControl.0004}");//修改时间
        }

        private void NameplateAutoPrintSetting_Load(object sender, EventArgs e)
        {
            editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            gvNamePlateTemplate.FocusedRowHandle = -1;

            bindData();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable dtQuery = null;
            
            string prodId = txtProdId.Text.Trim().ToUpper();
            if ("".Equals(prodId))
            {
                dtQuery = dsInfo.Tables[0];
            }
            else
            {
                DataRow[] drInfos = dsInfo.Tables[0].Select(string.Format("PROD_ID='{0}'", prodId));
                if (drInfos.Count() > 0)
                {
                    dtQuery = drInfos[0].Table.Clone();
                }
                else
                {
                    //MessageBox.Show("未找到该产品代码，请确认是否输入正确");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.msg.0001}"));
                }
                foreach (DataRow row in drInfos)
                {
                    dtQuery.ImportRow(row);
                }
            }
            gcNamePlateTemplate.DataSource = dtQuery;
        }

        public void bindData()
        {
            string[] l_s = new string[] { "TEMPLATE", "TEMPLATE_DESC" };
            string category = "Name_Templates";
            DataTable dt = BaseData.Get(l_s, category);


            luNamePlateTemplate.Properties.DataSource = dt;
            luNamePlateTemplate.Properties.ValueMember = "TEMPLATE_DESC";
            luNamePlateTemplate.Properties.DisplayMember = "TEMPLATE";



            dsInfo = entity.GetInfoForTemplate();

            gcNamePlateTemplate.DataSource = dsInfo.Tables[0];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string prodId = txtProdId.Text.Trim().ToUpper();
            string template = luNamePlateTemplate.Text;
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(prodId) || !string.IsNullOrEmpty(template))
            {

                if (entity.addTemplate(prodId, template, editor, time))
                {
                    //MessageBox.Show("新增成功");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.msg.0002}"));
                    bindData();
                    txtClear();
                }
                else
                {
                    //MessageBox.Show("新增失败");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.msg.0003}"));
                }
            }
            else
            {
                //MessageBox.Show("请输入产品代码或者选择模板");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.msg.0004}"));
            }
        }

        private void gvNamePlateTemplate_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvNamePlateTemplate.RowCount == 1)
            {
                gvNamePlateTemplate.FocusedRowHandle = 0;
            }
            string prodId = gvNamePlateTemplate.GetRowCellValue(gvNamePlateTemplate.FocusedRowHandle, "PROD_ID").ToString();
            string template = gvNamePlateTemplate.GetRowCellValue(gvNamePlateTemplate.FocusedRowHandle, "TEMPLATE").ToString();

            txtProdId.Text = prodId;
            luNamePlateTemplate.Text = template;
 
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
            string prodId = txtProdId.Text.Trim().ToUpper();
            string template = luNamePlateTemplate.Text;
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(prodId) || !string.IsNullOrEmpty(template))
            {
                //if (MessageBox.Show("确认修改？", "提示",MessageBoxButtons.OKCancel) == DialogResult.OK)
                if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (!entity.updateTemplate(prodId, template, editor, time))
                    {

                        //MessageBox.Show("更新失败");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.msg.0006}"));
                    }
                    else
                    {
                        //MessageBox.Show("更新成功");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.msg.0007}"));
                        bindData();
                        txtClear();
                    }
                }
            }
            else
            {

                //MessageBox.Show("请输入产品代码或者选择模板");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.NameplateAutoPrintSetting.msg.0008}"));
            }
        }
        public void txtClear()
        {
            txtProdId.Text = "";
            luNamePlateTemplate.Text = "";
        }
    }
}
