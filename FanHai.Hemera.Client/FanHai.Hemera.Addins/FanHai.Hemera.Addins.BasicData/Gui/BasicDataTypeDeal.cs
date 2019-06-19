/*
<FileInfo>
  <Author>ZhangHao FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
// ----------------------------------------------------------------------------------
// Copyright (c) FanHai
// ----------------------------------------------------------------------------------
// ==================================================================================
// 修改人               修改时间              说明
// ----------------------------------------------------------------------------------
// chao.pang          2012-02-13            添加注释 
// ==================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicDataTypeDeal : BaseDialog
    {
        #region variable define
        public string typeName = "";    //type name
        public string addOrEdit = "";   //add or edit 

        #endregion

        #region initialize 
        public BasicDataTypeDeal(string done)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataTypeDeal.Title}"))
        {
            InitializeComponent();
            SetLanguageInfoToControl();
            //get parameter
            addOrEdit = done;
        }

        private void SetLanguageInfoToControl()
        {
           // this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataTypeDeal.FormTitle}");
            this.lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataTypeDeal.lblName}");
            this.btnOk.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataTypeDeal.btnOk}");
            this.btnCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataTypeDeal.btnCancel}");
        }
        #endregion

        #region ok  button deal
        /// <summary>
        /// press ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            string typeN = "";  //type name
            typeN = this.tbTypeName.Text.Trim().ToString();
            //check type name and return type name
            if (typeN == "")
            {
                //set focus 获取焦点 modi by chao.pang
                this.tbTypeName.Focus();
                //MessageBox.Show("名称不可以为空！");
                MessageService.ShowWarning("${res:FanHai.Hemera.Addins.BasicData.BasicDataTypeDeal.MsgNameNullCheck}");
                return;
            }
            //need to check repeat
            //need to check repeat
            //need to check repeat
            else
            {
                //set type name typeN!="" 
                typeName = typeN;
                this.Close();              //关闭当前视图 modi by chao.pang
            }
        }

        #endregion

        #region cancel button deal
        /// <summary>
        /// cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
