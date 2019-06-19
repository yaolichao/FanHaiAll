using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Controls;


namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicSettingsDetail : BaseUserCtrl
    {

        TreeNode treeNode = null;   //used to receive the parameter
        public BaseAttribute baseAttributeEntity = new BaseAttribute();



        public BasicSettingsDetail(TreeNode tn)
        {
            InitializeComponent();
            SetLanguageInfoToControl();               
            //get tree node
            treeNode = tn;
        }
        /// <summary>
        /// 将相关信息载入到控件中  
        /// </summary>
        private void SetLanguageInfoToControl()
        {
            this.dgvData.Columns["ColumnKey"].HeaderText = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.GridColumnKeyCaption}");
            this.dgvData.Columns["ColumnName"].HeaderText = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.GridColumnNameCaption}");
            this.dgvData.Columns["ColumnType"].HeaderText = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.GridColumnTypeCaption}");
            this.dgvData.Columns["ColumnGroup"].HeaderText = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.GridColumnGroupCaption}");

            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbEdit.Text = StringParser.Parse("${res:Global.Edit}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
        }



        /// <summary>
        /// data initialize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 页面载入操作  
        private void BasicSettingsDetail_Load(object sender, EventArgs e)
        {
            this.lblMenu.Text = "基础数据 > 配置管理 > 列信息";

            this.topPanel.Visible = false;

            CheckPrivilege();   
            DataSet dataDS = new DataSet(); //dataset to receive category information
            //set category key to entity
            baseAttributeEntity.CategoryKey = treeNode.Tag.ToString();
            try
            {
                //get data  
                dataDS = baseAttributeEntity.GetBaseAttribute();
                //check result
                if (baseAttributeEntity.ErrorMsg == "")
                {
                    //insert data to grid view
                    SetDataToGrid(dataDS, this.dgvData);
                }
                else
                {
                    MessageBox.Show(baseAttributeEntity.ErrorMsg);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }



        private void CheckPrivilege()
        {
            this.tsbNew.Enabled = true;
            this.tsbEdit.Enabled = true;
            this.tsbDelete.Enabled = true;
        }



        /// <summary>
        /// get selected row in datagrid
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private int GetSelectedRowIndex(DataGridView dgv)
        {
            for(int i=0 ;i<dgv.Rows.Count;i++)
            {
                if (dgv.Rows[i].Selected == true)
                {
                    return i;
                }
            }
            return -1;
        }


        /// <summary>
        /// set all data to datagrid
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="dgv"></param>
        private void SetDataToGrid(DataSet ds,DataGridView dgv)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
                    {
                        dgv.Rows.Add();
                        dgv.Rows[i].Cells["ColumnKey"].Value = ds.Tables[0].Rows[i]["ATTRIBUTE_KEY"];   //ATTRIBUTE_KEY
                        dgv.Rows[i].Cells["ColumnName"].Value = ds.Tables[0].Rows[i]["ATTRIBUTE_NAME"]; //ATTRIBUTE_NAME
                        dgv.Rows[i].Cells["ColumnType"].Value = GetDataType(ds.Tables[0].Rows[i]["DATA_TYPE"].ToString()); ;      //DATA_TYPE
                        dgv.Rows[i].Cells["ColumnGroup"].Value = ds.Tables[0].Rows[i]["CATEGORY_NAME"]; //CATEGORY_Key
                    }
                }
            }
        }
        
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
                    return "INTEGER";
                case "2":
                    return "DATA";
                case "3":
                    return "DATATIME";
                case "4":
                    return "BOOLEAN";
                case "5":
                    return "STRING";
                case "6":
                    return "FLOAT";
                case "7":
                    return "SETTING";
                case "8":
                    return "LINKED";
                default:
                    return "";
            }
        }

        /// <summary>
        /// update datagridview
        /// </summary>
        /// <param name="addOrEdit">"add" or "edit"</param>
        /// <param name="ds">new data's ds</param>
        private void UpdateDataGridView(string addOrEdit,DataSet ds)
        {
            try
            {
                //check the deal is add or edit
                if (addOrEdit == "add")
                {
                    //add new row to datagridview
                    dgvData.Rows.Add();
                    //get rows index of datagridview
                    int rowCount = dgvData.Rows.Count - 1;
                    //set data to new row
                    SetOneDataToGrid(rowCount, ds);
                }
                else if (addOrEdit == "edit")
                {
                    for (int i = 0; i < dgvData.Rows.Count; i++)
                    {
                        if (dgvData.Rows[i].Cells["ColumnKey"].Value.ToString() == ds.Tables[0].Rows[0]["ATTRIBUTE_KEY"].ToString())
                        {
                            //update data to grid
                            SetOneDataToGrid(i, ds);
                            break;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                
                MessageBox.Show(ee.Message);
            }
            
        }

        /// <summary>
        /// set one  data to grid
        /// </summary>
        /// <param name="rowNumber">datagrid's rownumber which need to be updated or added</param>
        /// <param name="ds">new data</param>
        private void SetOneDataToGrid(int rowNumber,DataSet ds)
        {
            //set ATTRIBUTE_KEY
            dgvData.Rows[rowNumber].Cells["ColumnKey"].Value = ds.Tables[0].Rows[0]["ATTRIBUTE_KEY"].ToString();
            //set ATTRIBUTE_NAME
            dgvData.Rows[rowNumber].Cells["ColumnName"].Value = ds.Tables[0].Rows[0]["ATTRIBUTE_NAME"].ToString();
            //set DATA_TYPE
            dgvData.Rows[rowNumber].Cells["ColumnType"].Value = GetDataType(ds.Tables[0].Rows[0]["DATA_TYPE"].ToString());
            //set COLUMN GROUP
            dgvData.Rows[rowNumber].Cells["ColumnGroup"].Value = treeNode.Text.ToString();
        }




        /// <summary>
        /// delete column 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 删除列 
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //add rows
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                if (this.dgvData.Rows.Count > 0)
                {
                    //get selected row in datagrid
                    int selectedLineNumber = GetSelectedRowIndex(this.dgvData);
                    //if there are no selected rows
                    if (selectedLineNumber == -1)
                    {
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.MsgWetherSelectOneEditedRow}"));
                        return;
                    }
                    //if select ok
                    if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.MsgDeleteDataText}"), StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.MsgDeleteDataCaption}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //get attribute key
                        string attributeKey = dgvData.Rows[selectedLineNumber].Cells["ColumnKey"].Value.ToString();
                        //set entity's attribute
                        baseAttributeEntity.AttributeKey = attributeKey;

                        try
                        {
                            //delete data  
                            baseAttributeEntity.DeleteBaseAttribute();
                            //check result
                            if (baseAttributeEntity.ErrorMsg == "")
                            {
                                //delete row from datagridview
                                this.dgvData.Rows.RemoveAt(selectedLineNumber);
                                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.MsgDeleteDataSuccessfully}"));
                            }
                            else
                            {
                                MessageBox.Show(StringParser.Parse(baseAttributeEntity.ErrorMsg));
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// edit column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvData.Rows.Count > 0)
            {
                //get selected row in datagrid
                int selectedLineNumber = GetSelectedRowIndex(this.dgvData);
                //if there are no selected rows
                if (selectedLineNumber == -1)
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDetail.MsgWetherSelectOneDeletedRow}"));
                    return;
                }
                //call window of edit column
                //call the window of add column
                ColumnAddOrEdit columnAddOrEdit = new ColumnAddOrEdit("edit", treeNode.Tag.ToString(), this.dgvData.Rows[selectedLineNumber].Cells[0].Value.ToString());
                //Show Dialog
                columnAddOrEdit.ShowDialog();
                if (columnAddOrEdit.outDataSet.Tables.Count > 0)
                {
                    if (columnAddOrEdit.outDataSet.Tables[0].Rows.Count > 0)
                    {
                        //update datagridview
                        UpdateDataGridView("edit", columnAddOrEdit.outDataSet);
                    }
                }
            }
        }

        /// <summary>
        /// add column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            //call the window of add column
            ColumnAddOrEdit columnAddOrEdit = new ColumnAddOrEdit("add", treeNode.Tag.ToString(), "");
            //Show Dialog
            columnAddOrEdit.ShowDialog();
            //update datagridview
            if (columnAddOrEdit.outDataSet.Tables.Count > 0)
            {
                if (columnAddOrEdit.outDataSet.Tables[0].Rows.Count > 0)
                {
                    UpdateDataGridView("add", columnAddOrEdit.outDataSet);

                }
            }
        }
    }
}
