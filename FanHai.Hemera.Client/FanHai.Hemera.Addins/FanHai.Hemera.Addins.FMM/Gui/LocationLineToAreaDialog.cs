
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class LineToAreaDialog: BaseDialog
    {
        #region variable define
        private string _locationKey = "";   //location key
        private LocationLine _locationLineEntity = new LocationLine();
        #endregion

        #region constructor with no parameter
        /// <summary>
        /// 构造函数 
        /// </summary>
        public LineToAreaDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LineToAreaDialog.Title}"))   //区域分配线别 
        {
            InitializeComponent();
        }
        #endregion

        #region constructor with paramter--location key
        public LineToAreaDialog(string locationKey)
        {
            InitializeComponent();
            SetLanguageInfoToControl();
            //set location key to entity of locationlineEntity
            _locationLineEntity.LocationKey = locationKey;  
            //set value to private variable
            _locationKey = locationKey;
            //get lines that the area has and set it to control
            GetLineAndSetToControl(_locationKey);
        }

        /// <summary>
        ///设定控件名称 
        /// </summary>
        private void SetLanguageInfoToControl()
        {
            this.btnSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LineToAreaDialog.btnSave}");                  //button名 保存 
            this.btnClose.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LineToAreaDialog.btnClose}");                //button名 关闭 
            this.selectGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LineToAreaDialog.lblHaveLine}");          //已有线别 
            this.UnSelectGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LineToAreaDialog.lblUnSelectedLines}"); //未选线别 
        }
        #endregion

        #region get lines that the area has and set it to control
        /// <summary>
        /// get lines that the area has and set it to control
        /// </summary>
        /// <param name="locationKey">locationKey</param>
        private void GetLineAndSetToControl(string locationKey)
        {
            DataSet dsReturn = new DataSet();
            //excute entity's method
            dsReturn = _locationLineEntity.GetLinesAndLocationLine();
            //check result
            if (_locationLineEntity.ErrorMsg != "")
            {
                MessageBox.Show(_locationLineEntity.ErrorMsg);
            }
            else
            {
                //set value to control----unselected
                BindDataToClbUnSelectLine(dsReturn.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME]);
                //set value to control----selected
                BindDataToClbSelectLine(dsReturn.Tables[FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME]);
            }
        }
        #endregion

        #region save data
        private void btnSave_Click(object sender, EventArgs e)
        {
           //提示“确定要保存吗” 如果是返回值为true 
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                #region variable define
                DataSet dsSave = new DataSet();           //创建表集对象  
                DataSet dataSet = new DataSet();          //创建表集对象  
                DataTable dataTable = new DataTable();    //创建表对象  
                Hashtable hashTable = new Hashtable();    //创建表对象  
                DataTable mainTable = new DataTable();    //创建表对象  
                #endregion

                #region detail deal
                dataTable.Columns.Add(FMM_LOCATION_LINE_FIELDS.FIELD_LINE_KEY);       //添加LINE_KEY为第一列     
                dataTable.Columns.Add(FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY);   //添加LOCATION_KEY为第一列  
                dataTable.TableName = FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME;   //表名为FMM_LOCATION_LINE 
                //get data needed to updated to db 
                for (int i = 0; i < clbSelectLine.Items.Count; i++)
                {
                    dataTable.Rows.Add();
                    dataTable.Rows[i][FMM_LOCATION_LINE_FIELDS.FIELD_LINE_KEY] = ((LocationLine)clbSelectLine.Items[i]).LineKey;    //添加左边权限对应的LINE_KEY值到table中 
                    dataTable.Rows[i][FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY] = _locationLineEntity.LocationKey;               //添加左边权限对应的LOCATION_KEY值到table中 
                }
                //get main table parameter
                hashTable.Add(FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY, _locationLineEntity.LocationKey);
                mainTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                mainTable.TableName = FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME + "main";
                dataSet.Tables.Add(mainTable);
                dataSet.Tables.Add(dataTable);
                //excute new or insert
                dsSave = _locationLineEntity.SaveLocationLineRelation(dataSet);
                //check result
                if (_locationLineEntity.ErrorMsg != "")
                {
                    MessageBox.Show(_locationLineEntity.ErrorMsg);
                }
                else
                {
                    this.Close();
                    //提示“数据保存成功！” 
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LineToAreaDialog.MsgSaveDataSuccessfully}"));
                }
                #endregion
            }
        }
        #endregion

        #region close form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region delete line
        /// <summary>
        /// delete line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteLine_Click(object sender, EventArgs e)
        {
            //判断有无选择左侧记录 
            if (clbSelectLine.CheckedItems.Count <= 0)
            {
                //提示“请选择要删除的记录！” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LineToAreaDialog.MsgWetherSelectOneDeleteRow}"));
            }
            //选择了相应的记录 
            else
            {
                //循环左侧clbSelectLine中选中项数量的次数，o为每次获取的值 
                foreach (object o in clbSelectLine.CheckedItems)
                {
                    //将获取的值添加到clbUnSelectLine中 
                    clbUnSelectLine.Items.Add(o);
                }
                for (int i = clbSelectLine.Items.Count - 1; i >= 0; i--)
                {
                    //确定指定项是否位于集合内，如果项在集合中，则为 true；否则为 false。 
                    if (clbSelectLine.CheckedItems.Contains(clbSelectLine.Items[i]))
                    {
                        //将选择项从clbSelectLine中删除 
                        clbSelectLine.Items.Remove(clbSelectLine.Items[i]);
                    }
                }
            }
        }

        #endregion

        #region add line
        /// <summary>
        /// add line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLine_Click(object sender, EventArgs e)
        {
            //判断有无选择右侧记录 
            if (this.clbUnSelectLine.CheckedItems.Count <= 0)
            {
                // //提示“请选择记录再添加！” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LineToAreaDialog.MsgSelectNoRecordWhenAdd}"));
            }
            //选择了相应的记录 
            else
            {
                //循环左侧clbUnSelectLine中选中项数量的次数，o为每次获取的值 
                foreach (object o in this.clbUnSelectLine.CheckedItems)
                {

                    //将获取的值添加到clbSelectLine中 
                    clbSelectLine.Items.Add(o);
                }
                for (int i = clbUnSelectLine.Items.Count - 1; i >= 0; i--)
                {
                    //确定指定项是否位于集合内，如果项在集合中，则为 true；否则为 false。 
                    if (clbUnSelectLine.CheckedItems.Contains(clbUnSelectLine.Items[i]))
                    {
                        //将选择项从clbUnSelectLine中删除 
                        clbUnSelectLine.Items.Remove(clbUnSelectLine.Items[i]);
                    }
                }
            }
        }
        #endregion

        #region page_load

        private void ContentPrivilege_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region BindDataToUnSelectLine control
        /// <summary>
        /// BindDataToUnSelectLine control
        /// </summary>
        /// <param name="dataTable">unselected data</param>
        private void BindDataToClbUnSelectLine(DataTable dataTable)
        {
            if (clbUnSelectLine.Items.Count > 0)
            {
                clbUnSelectLine.Items.Clear();
            }
            List<LocationLine> roleList = new List<LocationLine>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                LocationLine lEntity = new LocationLine();
                lEntity.LineKey = dataTable.Rows[i][FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY].ToString();
                lEntity.LineName = dataTable.Rows[i][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString();
                roleList.Add(lEntity);
            }
            clbUnSelectLine.Items.AddRange(roleList.ToArray());
        }

        #endregion

        #region BindDataToSelectLine control
        /// <summary>
        /// BindDataToSelectLine control
        /// </summary>
        /// <param name="dataTable">selected data</param>
        private void BindDataToClbSelectLine(DataTable dataTable)
        {
            if (clbSelectLine.Items.Count > 0)
            {
                clbSelectLine.Items.Clear();
            }
            List<LocationLine> roleList = new List<LocationLine>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                LocationLine lEntity = new LocationLine();
                lEntity.LineKey = dataTable.Rows[i][FMM_LOCATION_LINE_FIELDS.FIELD_LINE_KEY].ToString();
                lEntity.LineName = dataTable.Rows[i][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString();
                roleList.Add(lEntity);
            }
            clbSelectLine.Items.AddRange(roleList.ToArray());
        }

        #endregion
    }
}
