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
// chao.pang          2012-02-09            添加注释 
// ==================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Addins.BasicData;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Common;


namespace FanHai.Hemera.Addins.BasicData
{
    public partial class ColumnAddOrEdit : BaseDialog
    {
        #region variable define

        string attributeId = "";    //define attributeId to accept attribute's Id
        string deal = "";           // add or edit
        string categoryKey = "";      //define groupName to accept group's key
        public DataSet outDataSet = new DataSet();  //dataset for storing the data to return
        List<string> lInformation = new List<string>();//define the list to store the information during the window
        public BaseAttribute baseAttributeEntity = new BaseAttribute();
        string columnName = ""; //column name
        string columnDataType = ""; //column data type
        string columnDesc = ""; //column decription

        #endregion

        #region constructor

        public ColumnAddOrEdit(string done, string cateKey, string attrId)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.Title}"))
        {
            InitializeComponent();
            SetLanguageInfoToControl();
            //get parameter
            attributeId = attrId;
            //get categoryKey
            categoryKey = cateKey;
            deal = done;
            //construct the structure of dataset which used to return
            StructDataSet();
        }

        private void SetLanguageInfoToControl()
        {
            this.baseInforGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.TabPageBasicAttributeCaption}");
            this.lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.LblName}");
            this.lblType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.lblType}");
            //this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.AddColumn}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.btnOk.Text = StringParser.Parse("${res:Global.OKButtonText}");
        }

        #endregion

        #region page load
        /// <summary>
        /// page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnAddOrEdit_Load(object sender, EventArgs e)
        {
            #region variable define
            DataSet dataDsBack = new DataSet();//dataset to receive result of adding new category info
            DataSet dataDsFrom = new DataSet();//dataset to pass to remoting method
            #endregion

            //set group name and status
            this.tbType.Enabled = false;
            this.tbType.Text = categoryKey;
          
            //if edit the data change the title of window
            if (deal == "edit")
            {
                //set title when edit the column
                this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.GroupControlText}");
                //initial data which need to be changed
                //get data from db
                DataSet dataDS = new DataSet(); //dataset to receive category information
                //set attribute_key of entity
                baseAttributeEntity.AttributeKey = attributeId;
                try
                {
                    //get data  
                    dataDS = baseAttributeEntity.GetColumnInfoByAttributeKey();
                    //check result
                    if (baseAttributeEntity.ErrorMsg == "")
                    {
                        //insert data to controls
                        this.tbColumnKey.Text = baseAttributeEntity.AttributeKey;    //Column Key
                        this.tbName.Text = dataDS.Tables[0].Rows[0]["ATTRIBUTE_NAME"].ToString();//column name
                        //this.cbbDataType.SelectedText = GetDataType(dataDS.Tables[0].Rows[0]["DATA_TYPE"].ToString());//DATA_TYPE name
                        this.cbbDataType.Text = GetDataType(dataDS.Tables[0].Rows[0]["DATA_TYPE"].ToString());//DATA_TYPE name
                        this.tbDesc.Text = dataDS.Tables[0].Rows[0]["DESCRIPTION"].ToString(); //add by vicky
                    }
                    else
                    {
                        MessageService.ShowError(baseAttributeEntity.ErrorMsg);
                        this.Close();
                    }
                }
                catch (Exception ee)
                {
                    MessageService.ShowError(ee.Message);
                    this.Close();
                }
            }
        }
        #endregion

        #region information's detail deal

        #region store information
        /// <summary>
        /// store information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 基础表新增列视图单击确定事件    modi by chao.pang
        private void btnOk_Click(object sender, EventArgs e)
        {
            //判定弹出窗体“是否保存”的返回值
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                DataSet dsColumnCheck = new DataSet();
                DataSet dsReturn = new DataSet();
                //check data 检查填入数据是否为空 modi by chao.pang
                if (!CheckNewColumnInfo())
                {
                    return;
                }

                #region detail deal
                try
                {
                    //set value to entity
                    MapValueToEntity();
                    //判定变量deal的值为add则是添加,为edit则是编辑  modi by chao.pang
                    if (deal == "add")
                    {
                        //save data  调用保存数据SaveBaseAttribute方法  modi by chao.pang
                        baseAttributeEntity.SaveBaseAttribute();
                        //check result 错误信息为空执行 modi by chao.pang
                        if (baseAttributeEntity.ErrorMsg == "")
                        {
                            //set outdataset value  将值添加到数据表中显示  modi by chao.pang
                            outDataSet.Tables[0].Rows.Add();
                            outDataSet.Tables[0].Rows[0]["ATTRIBUTE_KEY"] = baseAttributeEntity.AttributeKey;
                            outDataSet.Tables[0].Rows[0]["ATTRIBUTE_NAME"] = baseAttributeEntity.AttributeName;
                            outDataSet.Tables[0].Rows[0]["DATA_TYPE"] = baseAttributeEntity.DataType;
                            outDataSet.Tables[0].Rows[0]["DESCRIPTION"] = baseAttributeEntity.Descriptions;
                            //提示添加成功  modi by chao.pang
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.MsgEditDataSuccessfully}");
                            this.Close();
                        }
                        //错误信息返回值不为空，提示错误信息 modi by chao.pang 
                        else
                        {
                            MessageService.ShowError(baseAttributeEntity.ErrorMsg);
                        }
                    }
                    else
                    {
                        //set entity's attribute_key
                        baseAttributeEntity.AttributeKey = attributeId;
                        //save data  调用修改数据UpdateBaseAttribute的方法 modi by chao.pang
                        baseAttributeEntity.UpdateBaseAttribute();
                        //check result 返回错误信息为空 modi by chao.pang
                        if (baseAttributeEntity.ErrorMsg == "")
                        {
                            //set outdataset value to return
                            outDataSet.Tables[0].Rows.Add();
                            outDataSet.Tables[0].Rows[0]["ATTRIBUTE_KEY"] = baseAttributeEntity.AttributeKey;
                            outDataSet.Tables[0].Rows[0]["ATTRIBUTE_NAME"] = baseAttributeEntity.AttributeName;
                            outDataSet.Tables[0].Rows[0]["DATA_TYPE"] = baseAttributeEntity.DataType;
                            outDataSet.Tables[0].Rows[0]["DESCRIPTION"] = baseAttributeEntity.Descriptions; 
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.MsgEditDataSuccessfully}");
                            this.Close();
                        }
                        else
                        {
                            MessageService.ShowError(baseAttributeEntity.ErrorMsg);
                        }
                    }
                }
                catch (Exception ee)
                {
                    MessageService.ShowError(ee.Message);
                }
            }            
            #endregion
        }
        #endregion

        #region MapValueToEntity
        /// <summary>
        /// MapValueToEntity
        /// </summary>
        private void MapValueToEntity()
        {
            //set attribute key
            baseAttributeEntity.AttributeKey =  CommonUtils.GenerateNewKey(0);
            //set attribute name
            baseAttributeEntity.AttributeName = columnName;
            //set data type
            baseAttributeEntity.DataType = columnDataType.Substring(0,1);
            //set category key
            baseAttributeEntity.CategoryKey = this.categoryKey;
            //set descriptions
            baseAttributeEntity.Descriptions = this.columnDesc;
            //set creator
            baseAttributeEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            //set create time
            baseAttributeEntity.CreateTime = "";
            //set create time zone
            baseAttributeEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            //set editor
            baseAttributeEntity.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            //set edit time
            baseAttributeEntity.EditTime = "";
            //set edit time zone
            baseAttributeEntity.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            //set attribute order
            baseAttributeEntity.AttributeOrder = "0";
            //set IS_PRIMARY_KEY
            baseAttributeEntity.IsPriaryKey = "0";
            //set ATTRIBUTE_UNIT
            baseAttributeEntity.AttributeUnit = "";

        }
        #endregion

        #region cancel deal
        /// <summary>
        /// cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //if select ok
            if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.MsgCancelText}"), StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.MsgCancelCaption}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
            return;
        }
        #endregion

        #region get string value of data type according int value
        /// <summary>
        /// get string value of data type according int value
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns></returns>
        private string GetDataType(string intValue)
        {
            switch (intValue)
            {
                case "1":
                    return "1-INTEGER";
                case "2":
                    return "2-DATA";
                case "3":
                    return "3-DATATIME";
                case "4":
                    return "4-BOOLEAN";
                case "5":
                    return "5-STRING";
                case "6":
                    return "6-FLOAT";
                case "7":
                    return "7-SETTING";
                case "8":
                    return "8-LINKED";
                default:
                    return "";
            }
        }
        #endregion

        #endregion

        #region construct the structure of dataset which used to return
        /// <summary>
        /// construct the structure of dataset which used to return
        /// </summary>
        private void StructDataSet()
        {
            //construct the structure of dataset which used to return
            DataTable dt = new DataTable();
            DataColumn dcKey = new DataColumn("ATTRIBUTE_KEY");
            DataColumn dcName = new DataColumn("ATTRIBUTE_NAME");
            DataColumn dcDataType = new DataColumn("DATA_TYPE");
            DataColumn dcDataDesc = new DataColumn("DESCRIPTION");
            //add DataColumn to datatable
            dt.Columns.Add(dcKey);
            dt.Columns.Add(dcName);
            dt.Columns.Add(dcDataType);
            dt.Columns.Add(dcDataDesc);
            //add datatable to dataset
            outDataSet.Tables.Add(dt);
        }
        #endregion

        #region check new column data
        /// <summary>
        /// CheckNewColumnInfo
        /// </summary>
        /// <returns>true false</returns>
        private bool CheckNewColumnInfo()
        {
            columnName = ""; //column name
            columnDataType = ""; //column data type
            //get column name
            columnName = this.tbName.Text.Trim().ToString();
            //get column data type
            columnDataType = this.cbbDataType.Text.ToString();
            columnDesc = this.tbDesc.Text.ToString();

            if (columnName == "")
            {
               MessageService.ShowMessage("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.MsgColumnIsNullCheck}");
                return false;
            }
            if (columnDataType == "")
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.BasicData.ColumnAddOrEdit.MsgColumnTypeIsNullCheck}");
                return false;
            }
            return true;
        }
        #endregion

    }
}
