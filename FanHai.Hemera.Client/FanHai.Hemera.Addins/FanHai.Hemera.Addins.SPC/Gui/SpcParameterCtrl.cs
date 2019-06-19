/*
<FileInfo>
  <Author>Rayna Liu, SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Hemera.Utils.Controls;
using SolarViewer.Hemera.Utils.Common;


namespace SolarViewer.Hemera.Addins.SPC
{
    public partial class SpcParameterCtrl :BaseUserCtrl
    {
        private DataTable mainParamTable = new DataTable();
        private SpcEntity spcEntity = null;
        public SpcParameterCtrl()
        {
            InitializeComponent();
            spcEntity = new SpcEntity();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            DataTable saveTable = new DataTable();
            saveTable.TableName = SPC_PARAMETER_FIELD.DATABASE_TABLE_NAME;
            saveTable.Columns.Add(SPC_PARAMETER_FIELD.PARAMETER_NAME);
            saveTable.Columns.Add(SPC_PARAMETER_FIELD.PARAMETER_KEY);
            saveTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION);
            DataTable paramTable = (DataTable)gcMainParam.DataSource;
            for (int i = 0; i < paramTable.Rows.Count; i++)
            {
                int j;
                string paramKey = string.Empty;
                for (j = 0; j < mainParamTable.Rows.Count; j++)
                {
                    paramKey = paramTable.Rows[i][SPC_PARAMETER_FIELD.PARAMETER_KEY].ToString();
                    if (mainParamTable.Rows[j][SPC_PARAMETER_FIELD.PARAMETER_KEY].ToString() == paramKey)
                        break;
                }
                if (j == mainParamTable.Rows.Count)
                {
                    //add new 
                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {SPC_PARAMETER_FIELD.PARAMETER_KEY,paramTable.Rows[i][SPC_PARAMETER_FIELD.PARAMETER_KEY].ToString()},
                                                                {SPC_PARAMETER_FIELD.PARAMETER_NAME,paramTable.Rows[i][SPC_PARAMETER_FIELD.PARAMETER_NAME].ToString()},                                                                
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,"New"}
                                                            };

                    SolarViewer.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref saveTable, rowData);
                }
            }

            for (int i = 0; i < mainParamTable.Rows.Count; i++)
            {
                int j;
                string paramKey = string.Empty;
                for (j = 0; j < paramTable.Rows.Count; j++)
                {
                    paramKey = mainParamTable.Rows[i][SPC_PARAMETER_FIELD.PARAMETER_KEY].ToString();
                    if (paramTable.Rows[j][SPC_PARAMETER_FIELD.PARAMETER_KEY].ToString() == paramKey)
                        break;
                }
                if (j == paramTable.Rows.Count)
                {
                    //add delete
                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                 {SPC_PARAMETER_FIELD.PARAMETER_KEY,mainParamTable.Rows[i][SPC_PARAMETER_FIELD.PARAMETER_KEY].ToString()},
                                                                {SPC_PARAMETER_FIELD.PARAMETER_NAME,mainParamTable.Rows[i][SPC_PARAMETER_FIELD.PARAMETER_NAME].ToString()},                                                                                                                         
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,"Delete"}
                                                            };

                    SolarViewer.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref saveTable, rowData);
                }
            }

            if (saveTable.Rows.Count > 0)
            {
                DataSet dsSave = new DataSet();
                dsSave.Tables.Add(saveTable);
                spcEntity = new SpcEntity();
                if (spcEntity.SaveParams(dsSave))
                {
                    InitialData();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ParamSearchDialog param = new ParamSearchDialog();
            if (DialogResult.OK == param.ShowDialog())
            {
                DataTable paramTable = (DataTable)gcMainParam.DataSource;

                for (int i = 0; i < paramTable.Rows.Count; i++)
                {
                    if (param.ParamKey == paramTable.Rows[i][SPC_PARAMETER_FIELD.PARAMETER_KEY].ToString())
                    {
                        return;
                    }
                }

                paramTable.Rows.Add(new object[] { param.ParamName,param.ParamKey });
                paramTable.AcceptChanges();               
            }
        }       

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow row = this.gvMainParam.GetDataRow(this.gvMainParam.FocusedRowHandle); 
            DataTable paramTable = (DataTable)gcMainParam.DataSource;
            paramTable.Rows.Remove(row);
            paramTable.AcceptChanges();
        }

        private void SpcParameterCtrl_Load(object sender, EventArgs e)
        {
            //Get params of spc
            InitialData();
        }

        private void InitialData()
        {
            DataSet dataSet = spcEntity.GetSpcParams();
            if (spcEntity.ErrorMsg.Length > 0)
            {
                MessageService.ShowError("get spc params error:" + spcEntity.ErrorMsg);
            }
            else
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables.Contains(SPC_PARAMETER_FIELD.DATABASE_TABLE_NAME))
                {
                    DataTable dataTable = dataSet.Tables[SPC_PARAMETER_FIELD.DATABASE_TABLE_NAME];
                    mainParamTable = dataTable.Copy();
                    this.gcMainParam.MainView = gvMainParam;
                    this.gcMainParam.DataSource = dataTable;
                }
            }
        }
    }
}
