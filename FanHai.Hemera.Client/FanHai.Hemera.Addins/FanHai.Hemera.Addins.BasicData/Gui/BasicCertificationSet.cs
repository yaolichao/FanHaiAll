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

namespace FanHai.Hemera.Addins.BasicData.Gui
{
    /// <summary>
    /// <para>Description : 产品认证维护</para>
    /// <para>Author      : weixian.lu</para>
    /// <para>Create Date : 2017-12-19</para>
    /// </summary>
    public partial class BasicCertificationSet : BaseUserCtrl
    {
        BasicCertificationEntity _baseCertificationEntity = new BasicCertificationEntity();

        private DataTable _dt = null;

        private delegate void AfterStateChanged(ControlState controlState);
        private AfterStateChanged _afterStateChanged = null;

        private ControlState _controlState = ControlState.Empty;
        private ControlState State
        {
            get
            {
                return _controlState;
            }
            set
            {
                _controlState = value;

                if (_afterStateChanged != null)
                {
                    _afterStateChanged(value);
                }
            }
        }

        public BasicCertificationSet()
        {
            InitializeComponent();
        }

        private void BasicCertificationSet_Load(object sender, EventArgs e)
        {
            this._afterStateChanged += OnAfterStateChanged;
            this.State = ControlState.ReadOnly;
            
            BindCertificationType();
            
            if (_dt == null)
            {
                _dt = new DataTable();
                _dt.Columns.Add("CERTIFICATION_KEY", typeof(string));
                _dt.Columns.Add("CERTIFICATION_TYPE", typeof(string));
                _dt.Columns.Add("CERTIFICATION_DATE", typeof(DateTime));
                _dt.Columns.Add("VERSION", typeof(int));
                _dt.Columns.Add("CREATOR", typeof(string));
                _dt.Columns.Add("CREATE_TIME", typeof(DateTime));
                _dt.Columns.Add("EDITOR", typeof(string));
                _dt.Columns.Add("EDIT_TIME", typeof(DateTime));
                _dt.Columns.Add("IS_USED", typeof(string));
                gcCertification.DataSource = _dt;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            this.State = ControlState.Edit;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.State = ControlState.ReadOnly;

            if (_dt != null) _dt.RejectChanges();
            //LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_dt == null) LoadData();

            string certificationKey = "";
            string certificationType = this.cboCertificationType.Text;
            DateTime certificationDate = this.dtpCertificationDate.DateTime;

            if (!CheckInsertValid(certificationType, certificationDate, ref certificationKey)) return;

            if (_dt != null)
            {
                DataRow dr = _dt.NewRow();
                dr["CERTIFICATION_KEY"] = certificationKey;
                dr["CERTIFICATION_TYPE"] = certificationType;
                dr["CERTIFICATION_DATE"] = certificationDate == DateTime.MinValue ? (object)DBNull.Value : certificationDate;//允许认证时间为空
                dr["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dr["CREATE_TIME"] = FanHai.Hemera.Utils.Common.Utils.GetCurrentDateTime(); ;
                _dt.Rows.Add(dr);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow dr = gvCertification.GetFocusedDataRow();
            if (dr != null) dr.Delete();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_dt != null)
            {
                DataTable dtInsert = _dt.GetChanges(DataRowState.Added);
                DataTable dtDelete = _dt.GetChanges(DataRowState.Deleted);
                DataTable dtUpdate = _dt.GetChanges(DataRowState.Modified);

                #region 数据库操作
                DataSet ds = new DataSet();
                if (dtInsert != null && dtInsert.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtInsert.Rows)
                    {
                        dr["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        //dr["VERSION"] = 1;
                        //dr["IS_USED"] = "Y";
                    }

                    dtInsert.TableName = BaseCertification.TableForInsert;
                    ds.Merge(dtInsert, true, MissingSchemaAction.Add);
                }
                if (dtDelete != null && dtDelete.Rows.Count > 0)
                {
                    dtDelete.RejectChanges();

                    foreach (DataRow dr in dtDelete.Rows)
                    {
                        dr["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    }

                    dtDelete.TableName = BaseCertification.TableForDelete;
                    ds.Merge(dtDelete, true, MissingSchemaAction.Add);
                }
                if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUpdate.Rows)
                    {
                        dr["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    }

                    dtUpdate.TableName = BaseCertification.TableForUpdate;
                    ds.Merge(dtUpdate, true, MissingSchemaAction.Add);
                }

                if (ds.Tables.Count <= 0)
                {
                    MessageBox.Show("没有更改内容，不需要保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataSet dsReturn = _baseCertificationEntity.SaveCertification(ds);
                if (!string.IsNullOrEmpty(_baseCertificationEntity.ErrorMsg))
                {
                    MessageService.ShowMessage(_baseCertificationEntity.ErrorMsg);
                    return;
                }
                else
                {
                    MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.State = ControlState.ReadOnly;

                    LoadData();
                }
                #endregion
            }
        }

        /// <summary>
        /// 控件状态变更事件
        /// </summary>
        /// <param name="controlState"></param>
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {
                case ControlState.ReadOnly:
                    this.btnQuery.Enabled = true;
                    this.btnAdd.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.btnModify.Enabled = true;
                    this.gvCertification.OptionsBehavior.ReadOnly = true;
                    break;
                case ControlState.Edit:
                    this.btnQuery.Enabled = false;
                    this.btnAdd.Enabled = true;
                    this.btnDelete.Enabled = true;
                    this.btnSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.btnModify.Enabled = false;
                    this.gvCertification.OptionsBehavior.ReadOnly = true;
                    break;
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            DataSet ds = _baseCertificationEntity.GetValidCertification();
            if (ds != null && ds.Tables.Count > 0)
            {
                _dt = ds.Tables[0];
                gcCertification.DataSource = _dt;
            }
        }

        /// <summary>
        /// 绑定认证类型数据
        /// </summary>
        private void BindCertificationType()
        {
            DataSet ds = _baseCertificationEntity.GetValidCertificationType();
            if (ds != null && ds.Tables.Count > 0)
            {
                this.cboCertificationType.Properties.Items.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    this.cboCertificationType.Properties.Items.Add(row["CERTIFICATION_TYPE"]);
                }
            }
        }

        /// <summary>
        /// 检查插入数据是否有效
        /// </summary>
        /// <param name="certificationType">认证类型</param>
        /// <param name="certificationDate">认证时间</param>
        /// <param name="certificationKey">主键（认证类型+认证时间）</param>
        /// <returns></returns>
        private bool CheckInsertValid(string certificationType, DateTime certificationDate, ref string certificationKey)
        {
            if (string.IsNullOrEmpty(certificationType))
            {
                MessageBox.Show("认证类型不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #region 检查记录是否已存在（双重验证）
            #region 检查表格中是否已存在记录
            if (_dt != null)
            {
                DataRow[] dataRows = _dt.Select(string.Format(@"CERTIFICATION_KEY<>'{0}' AND CERTIFICATION_TYPE='{1}' AND {2}", certificationKey
                                                                                                                              , certificationType
                                                                                                                              , certificationDate == DateTime.MinValue ? "CERTIFICATION_DATE IS NULL"
                                                                                                                                                                       : string.Format("CERTIFICATION_DATE>='{0}' AND CERTIFICATION_DATE<'{1}'", certificationDate.ToString("yyyy-MM-dd 00:00:00")
                                                                                                                                                                                                                                               , certificationDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00"))));
                if (dataRows.Length > 0)
                {
                    MessageBox.Show(string.Format("该记录已添加，请勿重复操作！\r\n认证类型：{0}\r\n认证时间：{1}", certificationType, certificationDate == DateTime.MinValue ? "空" : certificationDate.ToString("yyyy-MM-dd")), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            #endregion

            #region 检查数据库中是否已存在记录
            DataSet ds = _baseCertificationEntity.GetCertification(certificationType, certificationDate);
            if (ds == null && ds.Tables.Count <= 0) return false;
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drsValid = dt.Select("IS_USED = 'Y'");
                //DataRow[] drsInvalid = dt.Select("IS_USED = 'N'");
                if (drsValid.Length > 0)
                {
                    MessageBox.Show(string.Format("该记录已存在，不允许插入重复记录！\r\n认证类型：{0}\r\n认证时间：{1}", certificationType, certificationDate == DateTime.MinValue ? "空" : certificationDate.ToString("yyyy-MM-dd")), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                certificationKey = dt.Compute("max(CERTIFICATION_KEY)", "").ToString();
            }
            #endregion
            #endregion

            if (string.IsNullOrEmpty(certificationKey)) certificationKey = CommonUtils.GenerateNewKey(0);

            return true;
        }
        /// <summary>
        /// 检查修改数据是否有效
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <returns></returns>
        private bool CheckModifyValid(DataRow dr)
        {
            /*if (dr.RowState != DataRowState.Added)
            {
                MessageBox.Show("只允许修改新增的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }*/

            string certificationKey = Convert.ToString(dr["CERTIFICATION_KEY"]); //主键（认证类型+认证时间）
            string certificationType = Convert.ToString(dr["CERTIFICATION_TYPE"]); //认证类型
            DateTime certificationDate = dr["CERTIFICATION_DATE"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["CERTIFICATION_DATE"]); //认证时间

            if (!CheckInsertValid(certificationType, certificationDate, ref certificationKey)) return false;

            if (dr.RowState == DataRowState.Added)
                dr["CERTIFICATION_KEY"] = certificationKey;

            return true;
        }
    }
}
