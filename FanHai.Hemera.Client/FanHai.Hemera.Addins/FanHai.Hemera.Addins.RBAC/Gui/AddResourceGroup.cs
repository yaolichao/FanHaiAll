using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.RBAC
{
    public partial class AddResourceGroup : BaseDialog
    {
        public ResourceGroup resourceGroupEntity = null;
        public AddResourceGroup()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.AddResourceGroup.TitleNew}"))
        {
            InitializeComponent();
            resourceGroupEntity = new ResourceGroup();
        }

        public AddResourceGroup(ResourceGroup resourceGroup)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.AddResourceGroup.TitleEdit}"))
        {
            InitializeComponent();
            resourceGroupEntity = resourceGroup;
            //Map Data To Control
            MapDataToControl();
        }
        private void MapDataToControl()
        {
            DataSet dataSet = new DataSet();
            dataSet=resourceGroupEntity.GetResourceGroup();
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    #region MapDataToEntity
                    resourceGroupEntity.GroupName=dataSet.Tables[0].Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_GROUP_NAME].ToString();
                    resourceGroupEntity.ResourceGroupCode=dataSet.Tables[0].Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_CODE].ToString();
                    resourceGroupEntity.Remark=dataSet.Tables[0].Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_REMARK].ToString();
                    resourceGroupEntity.Descriptions=dataSet.Tables[0].Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_DESCRIPTIONS].ToString();
                    resourceGroupEntity.Editor=dataSet.Tables[0].Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_EDITOR].ToString();
                    resourceGroupEntity.EditTimeZone=dataSet.Tables[0].Rows[0][RBAC_RESOURCE_GROUP_FIELDS.FIELD_EDIT_TIMEZONE].ToString();
                    resourceGroupEntity.IsInitializeFinished = true; 
                    #endregion

                    #region MapDataToControl
                    this.txtName.Text = resourceGroupEntity.GroupName;
                    this.txtCode.Text = resourceGroupEntity.ResourceGroupCode;
                    this.txtRemark.Text = resourceGroupEntity.Remark;
                    this.txtDescription.Text = resourceGroupEntity.Descriptions;
                    #endregion
                }
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}", "${res:Global.SystemInfo}"))
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = CreateDataTable();
                string groupName = string.Empty;
                bool bNew = false;
                groupName = this.txtName.Text.Trim();
                if (this.txtName.Text == string.Empty)
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.AddResourceGroup.Msg.NameOrCodeIsNull}", "${res:Global.SystemInfo}");
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(this.txtCode.Text, @"^\d{2}$"))
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.AddResourceGroup.CodeError}", "${res:Global.SystemInfo}");
                    return;
                }
                resourceGroupEntity.GroupName = groupName;
                resourceGroupEntity.Descriptions = this.txtDescription.Text;
                resourceGroupEntity.Remark = this.txtRemark.Text;
                resourceGroupEntity.ResourceGroupCode = this.txtCode.Text;
                resourceGroupEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                resourceGroupEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                resourceGroupEntity.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                resourceGroupEntity.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                if (resourceGroupEntity.GroupKey == "")
                {
                    resourceGroupEntity.GroupKey =  CommonUtils.GenerateNewKey(0);
                    bNew = true;
                }
                Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_KEY,resourceGroupEntity.GroupKey},
                                                                {RBAC_RESOURCE_GROUP_FIELDS.FIELD_GROUP_NAME,resourceGroupEntity.GroupName},
                                                                {RBAC_RESOURCE_GROUP_FIELDS.FIELD_DESCRIPTIONS,resourceGroupEntity.Descriptions},
                                                                {RBAC_RESOURCE_GROUP_FIELDS.FIELD_REMARK, resourceGroupEntity.Remark },
                                                                {RBAC_RESOURCE_GROUP_FIELDS.FIELD_CREATOR, resourceGroupEntity.Creator},
                                                                {RBAC_RESOURCE_GROUP_FIELDS.FIELD_CREATE_TIMEZONE,resourceGroupEntity.CreateTimeZone},
                                                                {RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_CODE,resourceGroupEntity.ResourceGroupCode}
                                                            };

                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                dataSet.Tables.Add(dataTable);
                resourceGroupEntity.AddResourceGroup(bNew, dataSet);
                if (resourceGroupEntity.ErrorMsg == "")
                {
                    DialogResult = DialogResult.OK;
                    this.Close();
                    resourceGroupEntity.ResetDirtyList();
                }
                else
                {
                    DialogResult = DialogResult.Retry;
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + resourceGroupEntity.ErrorMsg);
                }
            }
        }
        public static DataTable CreateDataTable()
        {            
            List<string> fields = new List<string>() { 
                                                        RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_KEY,
                                                        RBAC_RESOURCE_GROUP_FIELDS.FIELD_GROUP_NAME,
                                                        RBAC_RESOURCE_GROUP_FIELDS.FIELD_DESCRIPTIONS,
                                                        RBAC_RESOURCE_GROUP_FIELDS.FIELD_REMARK,
                                                        RBAC_RESOURCE_GROUP_FIELDS.FIELD_CREATOR,
                                                        RBAC_RESOURCE_GROUP_FIELDS.FIELD_CREATE_TIMEZONE,
                                                        RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_CODE
                                                       };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_RESOURCE_GROUP_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AddResourceGroup_Load(object sender, EventArgs e)
        {
            this.lblCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.Code}");
            this.lblDescription.Text = StringParser.Parse("${res:Global.Description}");
            this.lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.Name}");
            this.lblRemark.Text = StringParser.Parse("${res:Global.Remark}");
            this.layoutCtlGroupResource.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceGroup}");
            this.btnOk.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");
        }       
    }
}
