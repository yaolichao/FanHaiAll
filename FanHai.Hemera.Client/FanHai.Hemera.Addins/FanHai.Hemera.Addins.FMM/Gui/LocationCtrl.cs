using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Data;
using System.Windows.Forms;


namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示区域管理界面的用户自定义控件类。
    /// </summary>
    public partial class LocationCtrl : BaseUserCtrl
    {
        #region variable define
        private LocationEntity _locationEntity = new LocationEntity();
        private new delegate void AfterStateChanged(ControlState controlState);
        private new AfterStateChanged afterStateChanged = null;
        private ControlState _locationCtrlState = ControlState.Empty;
        #endregion

        #region construtor with no paramter
        public LocationCtrl()
        {
            InitializeComponent();
        }
        #endregion

        #region State Change
        private new ControlState State
        {
            get
            {
                return _locationCtrlState;
            }
            set
            {
                _locationCtrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Deal with state change event
        /// </summary>
        /// <param name="state"></param>
        private void OnAfterStateChanged(ControlState state)
        {
            this.tsbSearch.Enabled = this.tsbSetLine.Enabled = true;
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:
                    this.txtName.Text = "";
                    this.txtDescription.Text = "";
                    this.leWorkShop.EditValue = "";
                    //this.cbType.SelectedIndex = 0;
                    this.cbTypeEdit.SelectedIndex = 0;
                    this.cbTypeEdit.Properties.ReadOnly = true;
                    this.tsbSetLine.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbSearch.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    this.tsbSetLine.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbSearch.Enabled = false;
                    break;
                #endregion

                #region case state of read
                case ControlState.Read:
                    this.txtName.Enabled=false;
                    this.txtDescription.Enabled = false;
                    this.leWorkShop.Enabled = false;
                    //this.cbType.Enabled = false;
                    this.cbTypeEdit.Properties.ReadOnly = true;
                    this.tsbSetLine.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbSearch.Enabled = false;
                    break;
                #endregion

                #region case state of edit
                case ControlState.Edit:
                    this.txtName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.leWorkShop.Enabled = true;
                    //this.cbType.Enabled = true;
                    this.cbTypeEdit.Properties.ReadOnly = true;
                    this.tsbSetLine.Enabled = true;
                    this.tsbDelete.Enabled = true;
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    this.txtName.Text = "";
                    this.txtDescription.Text = "";
                    this.leWorkShop.EditValue = "";
                    //this.cbType.SelectedIndex = 0;
                    this.cbTypeEdit.SelectedIndex = 0;
                    this.txtName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.leWorkShop.Enabled = true;
                    //this.cbType.Enabled = true;
                    this.cbTypeEdit.Properties.ReadOnly = false;
                    this.tsbSetLine.Enabled = false;
                    this.tsbDelete.Enabled = false;
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = true;
                    break;
                #endregion
            }
        }
        #endregion

        #region construtor with entity----Location
        /// <summary>
        ///  construtor with entity----Location
        /// </summary>
        /// <param name="location"></param>
        public LocationCtrl(LocationEntity location)
        {
            InitializeComponent();
            SetLanguageInfoToControl();
            //initialize afterstatechanged 
            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);
            //bind data to look up edit control

            int parentLevel = 2;
            if (location != null)
            {
                Int32.TryParse(location.ParentLocationLevel, out parentLevel);
            }
            if (!BindDataToLookUpEditContol(parentLevel))
            {
               
                if (this.ParentForm != null)
                {
                    this.ParentForm.Close();
                }
               
            }
            //check
            if(location==null)
            {
                //set control state
                State = ControlState.New;
            }
            else
            {
                _locationEntity = location;
                //map information to control
                MapEntityToControl(_locationEntity);
                //set control state
                //set control state
                State = ControlState.Edit;
            }
        }

        private void SetLanguageInfoToControl()
        {
            this.Content.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.TxtGroupCtrlName}");
            this.lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.lblName}");
            this.lblType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.lblType}");
            this.lblDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.lblDescription}");
            this.lblBelongedLocation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.lblDirectBelongedLocation}");//FanHai.Hemera.Addins.EMS.LocationCtrl.lblDirectBelongedLocation  //lblBelongedLocation
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbSearch.Text = StringParser.Parse("${res:Global.Query}");
            this.tsbSetLine.Text = StringParser.Parse("${res:Global.SetLine}");
            //this.lblApplicationTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationViewContent.ViewContentPartTitle}");
        }
       
        #endregion

        #region bind data to workshop
        /// <summary>
        /// BIND DATA TO WORKSHOP
        /// </summary>
        private bool BindDataToLookUpEditContol(int parentLevel)
        {

            DataSet dataSet = new DataSet();

            
            #region leWorkShop() List

            this.leWorkShop.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;     //LOCATION_NAME
            this.leWorkShop.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;        //LOCATION_KEY 

            #endregion

            //get bind data 获取绑定数据,传入参数值parentLevel为0初始值 1工厂 2楼层 5车间 
            dataSet = _locationEntity.GetWorkshopBindData(parentLevel);  
            

            if (_locationEntity.ErrorMsg != "")
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgLocationInitializeFailure}"));
                return false;
            }
            else
            {
                if (dataSet.Tables.Contains(FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME))
                {
                    leWorkShop.Properties.DataSource = dataSet.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME];
                }
            }
            return true;
        }

        #endregion

        #region map information to control
        /// <summary>
        /// map information to control
        /// </summary>
        /// <param name="_locationEntity">entity of location</param>
        private void MapEntityToControl(LocationEntity locaEntity)
        {
            //set name
            this.txtName.Text = locaEntity.LocationName;
            //set description
            this.txtDescription.Text = locaEntity.Descriptions;
            //set type
            //for (int i = 0; i < this.cbType.Items.Count; i++)
            //{
            //    if (this.cbType.Items[i].ToString().Substring(0, 1) == locaEntity.LocalLevel)
            //    {
            //        this.cbType.SelectedIndex = i;
            //        break;
            //    }
            //}
            for (int i = 0; i < this.cbTypeEdit.Properties.Items.Count; i++)
            {
                if (this.cbTypeEdit.Properties.Items[i].ToString().Substring(0, 1) == locaEntity.LocalLevel)
                {
                    this.cbTypeEdit.SelectedIndex = i;
                    break;
                }
            }
                //set workshop
                if (locaEntity.ParentLocationKey == "")
                {
                    this.leWorkShop.EditValue = null;
                }
                else
                {
                    this.leWorkShop.EditValue = locaEntity.ParentLocationKey;
                }
        }

        #endregion

        #region search location
        private void tsbSearch_Click(object sender, EventArgs e)
        {
            #region variable define
            string locationName = "";   //location name
            string locationLevel = "";
            LocationEntity locationEntity = new LocationEntity(); //Location entity
            DataSet _returnDs = new DataSet(); 
            #endregion

            #region search deal
            //get locationName 将当前txtName的值赋值给locationName 
            locationName = this.txtName.Text.Trim().ToString();
            //cmbLocationLevel下拉列表值不为空 locationName 
            if (this.cbTypeEdit.Text != "")
            {
                //将cmbLocationLevel中选择的值取第一个字符赋值给变量locationLevel 
                locationLevel = this.cbTypeEdit.Text.Substring(0, 1);
            }

            //set value to entity of LotNumber 判断locationName不为空 
            if (locationName != "")
            {
                locationEntity.LocationName = locationName;
            }

            locationEntity.LocalLevel = locationLevel;

            //call search action and get dataset
            _returnDs = locationEntity.SearchLocation();

            if (locationEntity.ErrorMsg != "")
            {
                //show message “查询数据出错！” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.MsgQueryDataFailure}"));
            }
            else
            {
                //gridView1中的数据用返回数据集的FMM_LOCATION表填充 
                this.gridView1.GridControl.DataSource = _returnDs.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME];
            }

            #endregion
           
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {

            //获取行焦点 
            int rowHandle = gridView1.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                //获取行单元格LOCATION_KEY数据 
                string pubLocationKey = gridView1.GetRowCellValue(rowHandle, FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY).ToString();
                //获取行单元格LOCATION_NAME数据 
                string pubLocationName = gridView1.GetRowCellValue(rowHandle, FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME).ToString();
                DataSet dsGet = new DataSet();
                if (pubLocationKey != "")
                {
                    LocationEntity locationEntity = new LocationEntity();

                    locationEntity.LocationKey = pubLocationKey;
                    locationEntity.LocationName = pubLocationName;
                    dsGet = locationEntity.GetLocation();

                    if (locationEntity.ErrorMsg == "")
                    {
                        if (dsGet.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
                        {
                            
                            locationEntity.LocationName = dsGet.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows[0][FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME].ToString();           //获取表集的FMM_LOCATION表的LOCATION_NAME值到文本框
                            locationEntity.LocalLevel = dsGet.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows[0][FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL].ToString();            //获取表集的FMM_LOCATION表的LOCATION_LEVEL类型值到下拉框
                            locationEntity.Descriptions = dsGet.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows[0][FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS].ToString();            //获取表集的FMM_LOCATION表的DESCRIPTIONS描述值到文本框
                            locationEntity.ParentLocationKey = dsGet.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows[0][FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY].ToString(); //获取表集的FMM_LOCATION表的PARENT_LOC_KEY父位置主键值到下拉框
                            _locationEntity = locationEntity;
                            //赋值数据到控件中 
                            MapEntityToControl(_locationEntity);
                            State = ControlState.Edit;
                            OnAfterStateChanged(State);
                        }
                    }
                    else
                    {
                        MessageBox.Show(locationEntity.ErrorMsg);
                    }
                }
            }
        }

        #endregion

        #region MapDataSetToEntity
        /// <summary>
        /// MapDataSetToEntity
        /// </summary>
        /// <param name="dsGet"></param>
        private void MapDataSetToEntity(DataSet dataset)
        {
            if(dataset.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows.Count>0)
            {
                _locationEntity.LocationName = dataset.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows[0][FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME].ToString();
                _locationEntity.LocalLevel = dataset.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows[0][FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL].ToString();
                _locationEntity.Descriptions = dataset.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows[0][FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS].ToString();
                _locationEntity.ParentLocationKey = dataset.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME].Rows[0][FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY].ToString();
            }
        }
        #endregion

        #region add new location
        private void tsbNew_Click(object sender, EventArgs e)
        {
            //foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            //{
            //    //判定视图名称是否为“区域管理”
            //    if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.ViewContentPartTile}"))
            //    {
            //        viewContent.WorkbenchWindow.SelectWindow();
            //        return;
            //    }
            //}
            LocationViewContent locationViewContent = new LocationViewContent("", null);
            WorkbenchSingleton.Workbench.ShowView(locationViewContent);
        }
        #endregion

        #region ClearEntityValue
        /// <summary>
        /// ClearEntityValue
        /// </summary>
        private void ClearEntityValue()
        {
            _locationEntity.LocationKey = "";
            _locationEntity.LocationName = "";
            _locationEntity.ParentLocationKey = "";
            _locationEntity.SiteNumber = "";
            _locationEntity.LocalLevel = "";
            _locationEntity.ParentLocationKey = "";
            _locationEntity.ParentLocationLevel = "";
        }
        #endregion

        #region save location
        private void tsbSave_Click(object sender, EventArgs e)
        {
            //提示确定要保存吗？如果是执行下面操作 
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                DataSet dsReturn = new DataSet();                           
                try
                {

                    //check--if ok 输入信息判定
                    if (CheckSaveInfo())
                    {
                        //set data to entity 
                        MapDataToEntity();
                        //判定状态如果为新增执行下面的操作
                        if (State == ControlState.New)
                        {
                            //call method of saveData of entity
                            dsReturn = _locationEntity.SaveNewLocation();
                        }
                        //判定状态为修改Edit时执行下面修改操作
                        else if (State == ControlState.Edit)
                        {
                            //call method of saveData of entity 调用保存修改方法 
                            dsReturn = _locationEntity.SaveUpdateLocation();
                        }
                        //check result
                        if (_locationEntity.ErrorMsg != "")
                        {
                            //ClearEntityValue();
                            if (State == ControlState.New)
                            {
                                ClearEntityValue();
                            }
                            MessageBox.Show(StringParser.Parse(_locationEntity.ErrorMsg));
                        }
                        else
                        {
                            //set title
                            if (WorkbenchSingleton.Workbench.ActiveViewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.ViewContentPartTile}"))
                            {
                                State = ControlState.Edit;
                                //set title
                                WorkbenchSingleton.Workbench.ActiveViewContent.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.ViewContentPartTile}") + "_" + _locationEntity.LocationName;
                            }
                            //bind data again
                            int parentLevel = 5;
                            Int32.TryParse(_locationEntity.ParentLocationLevel, out parentLevel);

                            BindDataToLookUpEditContol(parentLevel);
                            //提示“数据保存成功”
                            MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgDataSaveSuccessfully}"));
                            tsbSearch_Click(sender, e);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #endregion

        #region map data to location entity
        /// <summary>
        ///  map data to location entity 将值都传入到location entity 中
        /// </summary>
        private void MapDataToEntity()
        {
            string locationKeyAndLevel = this.leWorkShop.Text.ToString(); ;
            //判定状态为新增时执行下面操作 
            if (State==ControlState.New) 
            {
                _locationEntity.LocationKey =  CommonUtils.GenerateNewKey(0);
                
            }
            _locationEntity.LocationName = this.txtName.Text.Trim().ToString();                  //位置名称        
            _locationEntity.Descriptions = this.txtDescription.Text.Trim().ToString();           //描述
            //_locationEntity.LocalLevel = this.cbType.Text.Substring(0, 1);
            _locationEntity.LocalLevel = this.cbTypeEdit.Text.Substring(0, 1);                   //1:厂区,2:楼层,5:车间,9:区域 
            _locationEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);            //创建者 
            _locationEntity.CreateTime = "";                                                     //创建时间 
            _locationEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);      //创建时区 
            _locationEntity.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);             //编辑者 
            _locationEntity.EditTime = "";                                                       //编辑时间 
            _locationEntity.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);        //编辑时区 
            //if (_locationEntity.LocalLevel == "9")判定2:楼层,5:车间,9:区域 
            if (_locationEntity.LocalLevel == "9" || _locationEntity.LocalLevel == "5" || _locationEntity.LocalLevel == "2")
            {
                //获取2:楼层,5:车间,9:区域 值 
                _locationEntity.ParentLocationLevel = locationKeyAndLevel.Substring(0, locationKeyAndLevel.IndexOf('_'));
                //获取主键值  
                _locationEntity.ParentLocationKey = locationKeyAndLevel.Substring(locationKeyAndLevel.IndexOf('_') + 1, locationKeyAndLevel.Length - locationKeyAndLevel.IndexOf('_') - 1);
            }
            else
            {
                _locationEntity.ParentLocationKey = "";
                _locationEntity.ParentLocationLevel = "";
            }
        }

        #endregion

        #region check save info
        /// <summary>
        /// CheckSaveInfo
        /// </summary>
        private bool CheckSaveInfo()
        {
            #region variable define 
            string locationName = "";   //location name
            string description = "";    //location's description
            string workShop = "";       //work shop
            string locationLevel="";            //location level
            #endregion

            #region check
            locationName = this.txtName.Text.Trim().ToString();
            description = this.txtDescription.Text.Trim().ToString();
            workShop = this.leWorkShop.Text;
            //locationLevel = this.cbType.Text;
            locationLevel = this.cbTypeEdit.Text;
            //验证名称是否为空
            if (locationName == "")
            {
                //提示"名称不可以为空"
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgNameIsNullCheck}"));
                return false;
            }
            //验证描述是否为空
            else if (description == "")
            {
                //提示“描述不可以为空”
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgDescriptionIsNullCheck}"));
                return false;
            }
            //名称不为空并且为描述为区域 
            else if (locationLevel!="" && locationLevel.Substring(0, 1) == "9")
            {
                //父节点为空 
                if (workShop == "")
                {
                    //提示“所属车间不可以为空” 
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgLocationIsNullCheck}"));
                    return false;
                }
                else 
                {
                    if (workShop == locationName)
                    {
                        //不可以选择自身车间作为自己的所属车间！ 
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgLocationIsSelfCheck}"));
                        return false;
                    }
                }
            }

            return true;
            #endregion
        }
        #endregion

        #region delete location
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //系统提示“确定要删除吗”
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                //判定LocationKey是否为空 
                if (_locationEntity.LocationKey == "")
                {
                    return;
                }
                else
                {
                    _locationEntity.DeleteLocation();
                    if (_locationEntity.ErrorMsg == "")
                    {
                        //foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                        //{
                        //    //如果视图名称为“区域管理”执行 
                        //    if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.ViewContentPartTile}"))
                        //    {
                        //        //参数值为true则关闭当前操作视图 
                        //        WorkbenchSingleton.Workbench.ActiveViewContent.WorkbenchWindow.CloseWindow(true);
                        //        viewContent.WorkbenchWindow.SelectWindow();
                        //        return;
                        //    }
                        //}
                        ////if there are no window named "区域管理" is opened
                        ////set control state
                        //State = ControlState.New;
                        ////change title 区域管理 
                        //WorkbenchSingleton.Workbench.ActiveViewContent.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.ViewContentPartTile}");
                        //提示“删除成功” 
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgDeleteDataSuccessfully}"));
                        LocationViewContent locationViewContent = new LocationViewContent("", null);
                        WorkbenchSingleton.Workbench.ShowView(locationViewContent);
                    }
                    else
                    {
                        MessageBox.Show(StringParser.Parse(_locationEntity.ErrorMsg));
                    }
                }
            }
        }
        #endregion

        #region set  line which location contains
        private void tsbSetLine_Click(object sender, EventArgs e)
        {
            #region check
            //检查类型 
            if (_locationEntity.LocationKey == "")
            {
                //提示“该数据还未保存！” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgNotSaveData}"));
                return;
            }
            else if (_locationEntity.LocalLevel == "5")
            {
                //提示“不可以给车间分配线别！” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgCanGiveLineToLocation}"));
                return;
            }
            else if (_locationEntity.LocalLevel == "2")
            {
                //提示“只能给区域分配线别” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgCanGiveLineToLocationOnly}"));
                return;
            }
            else if (_locationEntity.LocalLevel == "1")
            {
                //提示“只能给区域分配线别” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationCtrl.MsgCanGiveLineToLocationOnly}"));
                return;
            }
            #endregion

            #region detail deal
            LineToAreaDialog lineToAreaDialog = new LineToAreaDialog(_locationEntity.LocationKey);
            lineToAreaDialog.ShowDialog();

            #endregion

        }
        #endregion

        #region cbType_SelectedValueChanged
        private void cbType_SelectedValueChanged(object sender, EventArgs e)
        {

           
        }
        #endregion

        /// <summary>
        /// 根据描述的值的改变获取相应的值到父节点下拉列表中  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTypeEdit_SelectedValueChanged(object sender, EventArgs e)
        {
            int parentLevel = 0;                    //作为传入的参数值1:工厂 2:楼层 5:车间                       

            //判断描述中的值是否为空
            if (cbTypeEdit.Text != "")
            {
                //截取描述(下拉列表)选择项的第一个字符判断其值 
                switch (cbTypeEdit.Text.Substring(0, 1))
                {
                    case "1":
                        parentLevel = 0;
                        //截取值为1时即用户选择工厂时,隐藏父节点lblBelongedLocation控件和leWorkShop控件 
                        this.lblBelongedLocation.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        this.leWorkShop.Visible = false;
                        break;

                    case "2":
                        parentLevel = 1;
                        //截取值为2时即用户选择楼层时,显示父节点lblBelongedLocation控件和leWorkShop控件
                        this.lblBelongedLocation.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        this.leWorkShop.Visible = true;

                        string factoryCode = PropertyService.Get(PROPERTY_FIELDS.FACTORY_CODE);   //传入参数值为FACTORY_CODE  
                        this.leWorkShop.Text = factoryCode;
                        BindDataToLookUpEditContol(parentLevel);                                  //传入参数parentLevel获取数据绑定到leWorkShop控件中                         
                        break;

                    case "5":
                        parentLevel = 2;
                        //截取值为5时即用户选择车间时,显示父节点lblBelongedLocation控件和leWorkShop控件 
                        this.lblBelongedLocation.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        this.leWorkShop.Visible = true;
                        BindDataToLookUpEditContol(parentLevel);                                  //传入参数parentLevel获取数据绑定到leWorkShop控件中 
                        break;

                    case "9":
                        parentLevel = 5;
                        //截取值为9时即用户选择区域时,显示父节点lblBelongedLocation控件和leWorkShop控件 
                        this.lblBelongedLocation.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        this.leWorkShop.Visible = true;
                        BindDataToLookUpEditContol(parentLevel);                                  //传入参数parentLevel获取数据绑定到leWorkShop控件中 

                        break;

                    default:
                        break;
                }
               
              
                
            }

           
        }



        private void LoadParentLocationData(int parentLevel)
        {
            #region Variables

            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateILocationEngine().GetParentLocations(parentLevel);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                BindDataToParentLocationList(resDS.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
            

        }

        private void BindDataToParentLocationList(DataTable dataTable)
        {
            this.leWorkShop.Properties.DataSource = dataTable;
        }
    }
}
