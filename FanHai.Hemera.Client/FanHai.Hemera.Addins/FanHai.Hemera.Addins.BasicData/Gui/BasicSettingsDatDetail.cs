using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.StaticFuncs;
using System;
using System.Data;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicSettingsDatDetail : BaseUserCtrl
    {

        TreeNode tnode = null;  //reveive the paramter---treeNode
        //define dataset to receive category key's ds
        DataSet categoryDs = new DataSet();
        //define dataset to receive all columns of some  category key 
        DataSet columnsDs = new DataSet();
        //define entity
        CrmAttribute crmAttributeEntity = new CrmAttribute();




        public BasicSettingsDatDetail(TreeNode treeNode)
        {
            InitializeComponent();
            SetLanguageInfoToControl();
            //get paramter treenode
            tnode = treeNode;
        }

        private void SetLanguageInfoToControl()
        {
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.groupControlBasic}");
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbEdit.Text = StringParser.Parse("${res:Global.Edit}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
        }


        /// <summary>
        /// page_load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasicSettingsDetail_Load(object sender, EventArgs e)
        {
            this.topPanel.Visible = false;
            CheckPrivilege();
            //if there are columns in table
            if (ConstructDynamicDatagrid(dgvData))//do dynamic column in datagrid view 
            {
                DataSet ds = new DataSet(); //define new dataset
                //get data from db  
                ds = GetDataFromDb();
                //set data to grid
                SetDataToDataGrid(ds);
            }

        }

        private void CheckPrivilege()
        {
            tsbDelete.Enabled = true;
            tsbNew.Enabled = true;
            tsbEdit.Enabled = true;
            tsbSave.Enabled = true;
        }



        /// <summary>
        /// Construct Dynamic Datagridview's columns
        /// </summary>
        /// <param name="dgv">datagridview</param>
        private bool ConstructDynamicDatagrid(DataGridView dgv)
        {

            DataSet dataDsBack = new DataSet();//dataset to receive result of adding new category info

            //get all columns from db
            try
            {
                //set value to entity
                crmAttributeEntity.CategoryKey = tnode.Tag.ToString();
                crmAttributeEntity.CategoryName = tnode.Text.ToString();
                //call method of entity---GetGroupColumns
                dataDsBack = crmAttributeEntity.GetGroupColumns();
                //get columnsds---very important is used in other deal
                columnsDs = dataDsBack;
                //check result of GetGroupColumns
                if (crmAttributeEntity.ErrorMsg == "" && dataDsBack.Tables[0].Rows.Count!=0)
                {
                    //add line number
                    dataDsBack.Tables[0].Rows.Add();
                    dataDsBack.Tables[0].Rows[dataDsBack.Tables[0].Rows.Count - 1]["ATTRIBUTE_NAME"] = "ITEM_ORDER";
                    dataDsBack.Tables[0].Rows[dataDsBack.Tables[0].Rows.Count - 1]["ATTRIBUTE_KEY"] = -1;
                    //add columns to datagrid
                    for (int i = 0; i < dataDsBack.Tables[0].Rows.Count - 1; i++)
                    {
                        dgv.Columns.Add(dataDsBack.Tables[0].Rows[i]["ATTRIBUTE_NAME"].ToString(), dataDsBack.Tables[0].Rows[i]["ATTRIBUTE_NAME"].ToString());
                    }
                    //line number
                    dgv.Columns.Add("ITEM_ORDER", StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.GridColumnItemOrder}"));
                    //flag of dealing
                    dgv.Columns.Add("FLAG", StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.GridColumnFlag}"));
                    //set line number and flag's visible
                    dgv.Columns["ITEM_ORDER"].Visible = false;
                    dgv.Columns["FLAG"].Visible = false;
                    //return value
                    return true;
                }
                else
                {
                    //MessageBox.Show("该表中没有相关列，请先添加该组别的列！");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.MsgColumnsNullCheck}"));
                    return false;
                }
            }
            catch (Exception ee)
            {  
                MessageBox.Show(ee.Message);
                return false;
            }
        }



        private DataSet GetDataFromDb()
        {

            #region variable define

            DataSet dataSetFromAll = new DataSet(); //all data to pass
            DataSet dataSetBackAll = new DataSet(); //all data to receive
            DataSet dataSetFromGroupInfo = new DataSet();   //group info to pass
            DataSet dataSetBackGroupInfo = new DataSet();   //group info to receive
            dataSetFromAll = AddinCommonStaticFunction.GetTwoColumnsCommonDs();
            dataSetFromAll.Tables[0].Rows.Add();
            dataSetFromAll.Tables[0].Rows[0][0] = "CATEGORY_KEY";
            dataSetFromAll.Tables[0].Rows[0][1] = tnode.Tag.ToString();
            #endregion

            try
            {
                //set value to entity
                crmAttributeEntity.CategoryKey = tnode.Tag.ToString();
                //call method of entity----GetAllData
                dataSetBackAll = crmAttributeEntity.GetAllData();
                //check result of GetAllData
                if (crmAttributeEntity.ErrorMsg == "")
                {
                    if (dataSetBackAll.Tables[0].Rows.Count > 0)
                    {
                        //get result of excute sql
                        dataSetBackAll = crmAttributeEntity.GetGruopBasicData();
                        if (crmAttributeEntity.ErrorMsg != "")
                        {
                            MessageBox.Show(crmAttributeEntity.ErrorMsg);
                            return new DataSet();
                        }
                        //remove parameter table 
                        dataSetBackAll.Tables.Remove(dataSetBackAll.Tables[1]);
                        //add parameter to datasetfromall
                        dataSetBackAll.Merge(dataSetFromAll.Tables[0], false, MissingSchemaAction.Add);
                        //get distinct column's data of some group
                        dataSetBackAll.Tables[0].TableName = "Columns";
                        dataSetBackAll.Tables[1].TableName = "Category";
                        dataSetBackGroupInfo = crmAttributeEntity.GetDistinctColumnsData(dataSetBackAll);
                        if (crmAttributeEntity.ErrorMsg != "")
                        {
                            MessageBox.Show(crmAttributeEntity.ErrorMsg);
                            return new DataSet();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(crmAttributeEntity.ErrorMsg);
                    return new DataSet();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dataSetBackGroupInfo;
        }



        /// <summary>
        /// set data to datagridview
        /// </summary>
        private void SetDataToDataGrid(DataSet dataSet)
        {
            try
            {
                //if there are datas table
                if (dataSet.Tables.Count == 2)
                {
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        //if there are columns tables
                        if (columnsDs.Tables.Count == 2)
                        {
                            dgvData.Rows.Add();
                            for (int j = 0; j < columnsDs.Tables[0].Rows.Count; j++)
                            {
                                dgvData.Rows[i].Cells[columnsDs.Tables[0].Rows[j][1].ToString()].Value = dataSet.Tables[0].Rows[i][columnsDs.Tables[0].Rows[j][1].ToString()];
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.Message.ToString()); ;
            }

            
        }



        /// <summary>
        /// get max item order from datagridview
        /// </summary>
        /// <param name="dgv">datagridview</param>
        /// <returns></returns>
        private int GetMaxItemOrder(DataGridView dgv)
        {
            //get datagridview's row count
            int rowCount = dgvData.Rows.Count - 1;
            //variable to receive the item order
            int rowItemOrder = 0;
            if (rowCount != 0)
            {
                try
                {
                    //initial rowItemOrder;
                    rowItemOrder = Convert.ToInt32(dgvData.Rows[0].Cells["ITEM_ORDER"].Value.ToString());
                    for (int i = 0; i < dgvData.Rows.Count; i++)
                    {
                        if (dgvData.Rows[i].Cells["ITEM_ORDER"].Value == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (Convert.ToInt32(dgvData.Rows[i].Cells["ITEM_ORDER"].Value.ToString()) > rowItemOrder)
                            {
                                rowItemOrder = Convert.ToInt32(dgvData.Rows[i].Cells["ITEM_ORDER"].Value.ToString());
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                
            }
            //return value
            return rowItemOrder + 1;
        }




        /// <summary>
        /// get dataset for delete
        /// </summary>
        /// <returns>dataset</returns>
        private DataSet GetDataSetForDelete()
        {
            DataSet paraDataSet = new DataSet();    //paramter dataset
            //new DataTable columnNameDt
            DataTable columnDt = new DataTable("columnDt");
            columnDt.Columns.Add("columnName");
            columnDt.Columns.Add("columnValue");
            columnDt.Columns.Add("columnKey");
            columnDt.Columns.Add("columnItemOrder");
            //add datatable to dataset
            paraDataSet.Tables.Add(columnDt);
            //return dataset
            return paraDataSet;
        }

        /// <summary>
        /// get dataset for inserting
        /// </summary>
        /// <returns>dataset</returns>
        private DataSet GetDataSetForInsert()
        {
            DataSet paraDataSet = new DataSet();    //paramter dataset
            //new DataTable columnNameDt
            DataTable columnDt = new DataTable("columnDt");
            columnDt.Columns.Add("columnName");
            columnDt.Columns.Add("columnValue");
            columnDt.Columns.Add("columnKey");
            columnDt.Columns.Add("columnItemOrder");
            columnDt.Columns.Add("deal");
            paraDataSet.Tables.Add(columnDt);
            //return dataset
            return paraDataSet;
        }




        /// <summary>
        /// initial the status of every row after save the value
        /// </summary>
        private void ClearRowStatus()
        {
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                dgvData.Rows[i].Cells["FLAG"].Value = "";
            }
        }


        /// <summary>
        /// Delete Data From Db And Grid
        /// </summary>
        /// <param name="rowNum"></param>
        private void DeleteDataFromDbAndGrid(int rowNum)
        {
            if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.MsgDeleteDataText}"), StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.MsgDeleteDataCaption}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                #region variable define
                //get dataset for inserting
                DataSet paraDataSet = GetDataSetForDelete();
                #endregion

                //get added row's item order
                int rowCount = Convert.ToInt32(dgvData.SelectedRows[0].Cells["ITEM_ORDER"].Value.ToString());
                //get all value need to be inserted
                for (int j = 0; j < columnsDs.Tables[0].Rows.Count-1; j++)
                {
                    //add columnName
                    if (dgvData.Rows[rowNum].Cells[columnsDs.Tables[0].Rows[j][1].ToString()].Value != null)
                    {
                        paraDataSet.Tables["columnDt"].Rows.Add();
                        paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnName"] = columnsDs.Tables[0].Rows[j][1].ToString() ;
                        //add columnValue     


                        paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnValue"] =  dgvData.Rows[rowNum].Cells[columnsDs.Tables[0].Rows[j][1].ToString()].Value.ToString() ;

                        //add columnKey                      
                        paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnKey"] = columnsDs.Tables[0].Rows[j][0].ToString();
                        //add columnItemOrder

                        paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnItemOrder"] = rowCount.ToString();
                    }
                }
                DataSet dataSetBack = new DataSet(); //all data to receive
                try
                {
                    //set value to dataset of entity
                    crmAttributeEntity.DatasetValue = paraDataSet;
                    //call method of entity----DeleteBasicData
                    //get result of excute sql
                    dataSetBack = crmAttributeEntity.DeleteBasicData();
                    //check result
                    if (crmAttributeEntity.ErrorMsg == "")
                    {
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.MsgDeleteDataSuccessfully}"));
                        //delete data from datagridview
                        this.dgvData.Rows.RemoveAt(rowNum);
                    }
                    else
                    {
                        MessageBox.Show(crmAttributeEntity.ErrorMsg);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } 
        }



        /// <summary>
        /// mouse_down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvData_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvData.Rows.Count != 0)
            {
                if (e.RowIndex != -1)
                {
                    if (dgvData.Rows[e.RowIndex].Cells["FLAG"].Value != null && (dgvData.Rows[e.RowIndex].Cells["FLAG"].Value.ToString() == "EDIT" || dgvData.Rows[e.RowIndex].Cells["FLAG"].Value.ToString() == "ADD"))
                    {
                        //set datagridview can be edited
                        dgvData.ReadOnly = false;
                        if (e.ColumnIndex != -1)
                        {
                            this.dgvData.CurrentCell = dgvData.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        }
                    }
                    else
                    {
                        //set datagridview can not be edited
                        dgvData.ReadOnly = true;
                    }
                }
            }
        }



        /// <summary>
        /// add new row data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            
            //判断改组中有没有添加列,count=0表示没有添加该列 
            if (dgvData.Columns.Count == 0)
            {
                //提示“该表中没有相关列,请先添加该组别的列” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.MsgColumnsNullCheck}"));
                return;
            }
            //如果count!=0,表示已有列信息可添加数据 
            else
            {
                //数据表添加一行用于编辑 
                dgvData.Rows.Add();
                //set Flag
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["FLAG"].Value = "ADD";
                //set item order
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["ITEM_ORDER"].Value = GetMaxItemOrder(dgvData).ToString();

            }
        }



        /// <summary>
        /// edit row data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            //get row number
            int rowNumber = -1;
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                if (dgvData.Rows[i].Selected == true)
                {
                    rowNumber = i;
                    break;
                }
            }
            //if select some row
            if (rowNumber != -1)
            {
                //if is not added row
                if (dgvData.Rows[rowNumber].Cells["FLAG"].Value == null)
                {
                    dgvData.Rows[rowNumber].Cells["FLAG"].Value = "EDIT";
                }
                else
                {
                    if (dgvData.Rows[rowNumber].Cells["FLAG"].Value.ToString() == "ADD")
                    {
                        //MessageBox.Show("该行是新增行！");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatDetail.MsgCheckIsNewRow}"));
                        return;
                    }
                    else
                    {
                        dgvData.Rows[rowNumber].Cells["FLAG"].Value = "EDIT";
                    }
                }
            }
        }



        /// <summary>
        /// save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            //if the data is added or edited
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                for (int i = 0; i < this.dgvData.Rows.Count; i++)
                {
                    bool haveData = false;
                    if (this.dgvData.Rows[i].Cells["FLAG"].Value != null)
                    {
                        if (this.dgvData.Rows[i].Cells["FLAG"].Value.ToString() == "ADD" || dgvData.Rows[i].Cells["FLAG"].Value.ToString() == "EDIT")
                        {
                            #region variable define
                            this.dgvData.CurrentCell = null;
                            //get dataset for inserting
                            DataSet paraDataSet = GetDataSetForInsert();
                            #endregion

                            //get added row's item order
                            int rowCount = Convert.ToInt32(dgvData.Rows[i].Cells["ITEM_ORDER"].Value.ToString());
                            //get all value need to be inserted
                            for (int j = 0; j < columnsDs.Tables[0].Rows.Count - 1; j++)
                            {
                                string columnName = Convert.ToString(columnsDs.Tables[0].Rows[j][1].ToString());
                                string columnValue =Convert.ToString(dgvData.Rows[i].Cells[columnName].Value);
                                string columnKey = Convert.ToString(columnsDs.Tables[0].Rows[j][0]);
                                columnName = columnName ?? string.Empty;
                                columnValue = columnValue ?? string.Empty;
                                columnKey = columnKey ?? string.Empty;
                                //add columnName
                                paraDataSet.Tables["columnDt"].Rows.Add();
                                if (columnsDs.Tables[0].Rows[j][1] != null)
                                {
                                    paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnName"] =columnName;
                                }
                                else
                                {
                                    paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnName"] = "";
                                }

                                //add columnValue
                                //paraDataSet.Tables["columnDt"].Rows.Add();
                                if (dgvData.Rows[i].Cells[columnsDs.Tables[0].Rows[j][1].ToString()].Value != null)
                                {
                                    haveData = true;
                                    paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnValue"] = columnValue ;
                                    paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnKey"] =  columnKey;
                                    //add columnItemOrder                           
                                    paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["columnItemOrder"] = rowCount.ToString();

                                    if (this.dgvData.Rows[i].Cells["FLAG"].Value.ToString() == "ADD")
                                    {
                                        paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["deal"] = "ADD";
                                    }
                                    else if (this.dgvData.Rows[i].Cells["FLAG"].Value.ToString() == "EDIT")
                                    {
                                        paraDataSet.Tables["columnDt"].Rows[paraDataSet.Tables["columnDt"].Rows.Count - 1]["deal"] = "EDIT";
                                    }
                                }
                            }

                            if (haveData)
                            {
                                DataSet dataSetBack = new DataSet(); //all data to receive
                                //set dataset to entity's method
                                crmAttributeEntity.DatasetValue = paraDataSet;
                                //call entity's method
                                dataSetBack = crmAttributeEntity.SaveCrmAttribute();
                                //check result
                                if (crmAttributeEntity.ErrorMsg != "")
                                {
                                    MessageBox.Show(crmAttributeEntity.ErrorMsg);
                                    return;
                                }
                            }
                            else
                            {
                                dgvData.Rows.RemoveAt(i);
                            }
                        }
                    }
                }
                //initial the status of every row after save the value
                ClearRowStatus();
            }
        }



        /// <summary>
        /// delete row data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //get row number
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                int rowNumber = -1;
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (dgvData.Rows[i].Selected == true)
                    {
                        rowNumber = i;
                        break;
                    }
                }
                //if select some row
                if (rowNumber != -1)
                {
                    //if is not added row
                    if (dgvData.Rows[rowNumber].Cells["FLAG"].Value == null)
                    {
                        //delete data from db and datagridview
                        DeleteDataFromDbAndGrid(rowNumber);
                    }
                    else
                    {
                        if (dgvData.Rows[rowNumber].Cells["FLAG"].Value.ToString() == "ADD")
                        {
                            //delete row in datagridview
                            dgvData.Rows.RemoveAt(rowNumber);
                        }
                        else
                        {
                            //delete data from db and datagridview
                            DeleteDataFromDbAndGrid(rowNumber);
                        }
                    }

                }
            }
        }
    }
}
