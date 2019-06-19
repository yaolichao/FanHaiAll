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
    public partial class AddOperationGroup : BaseDialog
    {
        public RBACOperationGroup operationGroupEntity = new RBACOperationGroup();
        public AddOperationGroup()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.AddOperationGroup.Title}"))
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}","${res:Global.SystemInfo}"))
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = CreateDataTable();
                string groupName = string.Empty;
                groupName = this.txtGroupName.Text.Trim();
                if (groupName == string.Empty)
                {
                    this.txtGroupName.Focus();
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.AddOperationGroup.Msg.NameIsNull}", "${res:Global.SystemInfo}");
                    return;
                }
                else
                {
                    operationGroupEntity.OperationGroupKey =  CommonUtils.GenerateNewKey(0);
                    operationGroupEntity.GroupName = groupName;
                    operationGroupEntity.Descriptions = this.txtGroupDescription.Text;
                    operationGroupEntity.Remark = this.txtRemark.Text;
                    operationGroupEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    operationGroupEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_OPERATION_GROUP_FIELDS.FIELD_OPERATION_GROUP_KEY, operationGroupEntity.OperationGroupKey},
                                                                {RBAC_OPERATION_GROUP_FIELDS.FIELD_GROUP_NAME,operationGroupEntity.GroupName},
                                                                {RBAC_OPERATION_GROUP_FIELDS.FIELD_DESCRIPTIONS,operationGroupEntity.Descriptions},
                                                                {RBAC_OPERATION_GROUP_FIELDS.FIELD_REMARK, operationGroupEntity.Remark},
                                                                {RBAC_OPERATION_GROUP_FIELDS.FIELD_CREATOR, operationGroupEntity.Creator},
                                                                {RBAC_OPERATION_GROUP_FIELDS.FIELD_CREATE_TIMEZONE,operationGroupEntity.CreateTimeZone}
                                                            };

                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    dataSet.Tables.Add(dataTable);
                    operationGroupEntity.AddOperationGroup(dataSet);
                    if (operationGroupEntity.ErrorMsg == "")
                    {
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        DialogResult = DialogResult.Retry;
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + operationGroupEntity.ErrorMsg);
                    }

                }
            }
        }
        public static DataTable CreateDataTable()
        {
            List<string> fields = new List<string>() { 
                                                        RBAC_OPERATION_GROUP_FIELDS.FIELD_OPERATION_GROUP_KEY,
                                                        RBAC_OPERATION_GROUP_FIELDS.FIELD_GROUP_NAME,
                                                        RBAC_OPERATION_GROUP_FIELDS.FIELD_DESCRIPTIONS,
                                                        RBAC_OPERATION_GROUP_FIELDS.FIELD_REMARK,
                                                        RBAC_OPERATION_GROUP_FIELDS.FIELD_CREATOR,
                                                        RBAC_OPERATION_GROUP_FIELDS.FIELD_CREATE_TIMEZONE};

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_OPERATION_GROUP_FIELDS.DATABASE_TABLE_NAME, fields);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AddOperationGroup_Load(object sender, EventArgs e)
        {
            this.lblGroupDescription.Text = StringParser.Parse("${res:Global.Description}");
            this.lblGroupName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.Name}");
            this.lblRemark.Text = StringParser.Parse("${res:Global.Remark}");
            this.layoutCtlGrpOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationGroup}");
            this.btnOk.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");
        }
    }
}
