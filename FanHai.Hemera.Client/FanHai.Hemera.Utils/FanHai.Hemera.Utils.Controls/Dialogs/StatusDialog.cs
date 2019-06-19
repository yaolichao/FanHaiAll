/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
#endregion

namespace FanHai.Hemera.Share.CommonControls.Dialogs
{
    public partial class StatusDialog : BaseDialog
    {
        #region constructor
        public StatusDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// constructor
        /// </summary>
        public StatusDialog(EntityObject entityObject)
        {
            InitializeComponent();

            //set entity
            _entityObject = entityObject;
        }
        //public StatusDialog()
        //{
        //    InitializeComponent();
        //}
        #endregion

        #region page_load
        /// <summary>
        /// page_load
        /// </summary>
        private void StatusDialog_Load(object sender, EventArgs e)
        {
            try
            {
                //set control status according to old status
                switch (Convert.ToInt32(_entityObject.Status))
                {
                    case 0:
                        this.rbgStatus.SelectedIndex = 0;
                        this.rbgStatus.Properties.Items[1].Enabled = true;
                        this.rbgStatus.Properties.Items[2].Enabled = false;
                        break;
                    case 1:
                        this.rbgStatus.SelectedIndex = 1;
                        this.rbgStatus.Properties.Items[0].Enabled = false;
                        this.rbgStatus.Properties.Items[2].Enabled = true;
                        break;
                    case 2:
                        this.rbgStatus.SelectedIndex = 2;
                        this.rbgStatus.Properties.Items[0].Enabled = false;
                        this.rbgStatus.Properties.Items[1].Enabled = false;
                        this.btnOk.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
            catch
            { }
        }
        #endregion

        #region save new status
        /// <summary>
        /// save new status
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            //get new status
            int _newStatus = this.rbgStatus.SelectedIndex;

            _entityStatus = (EntityStatus)_newStatus;
            try
            {
                if (Convert.ToInt32(_entityObject.Status) != _newStatus)
                {
                    _entityObject.Status = (EntityStatus)_newStatus;
                    _entityObject.UpdateStatus();
                }
            }
            catch
            { }
            _dialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion

        #region cancel deal
        /// <summary>
        /// cancel deal
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region variable define
        public EntityObject _entityObject = null; //define entity

        public EntityStatus _entityStatus = new EntityStatus();
        public DialogResult _dialogResult = new DialogResult();
        #endregion
    }
}
