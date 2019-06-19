using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Addins.BasicData;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Common;


namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicDataSettingTree : BaseUserCtrl
    {
        #region variable define
        //define treenode
        TreeNode tn = new TreeNode();
        ContextMenu contextMenu = null;
        public BaseAttributeCategory baseAttributeCategoryEntity = new BaseAttributeCategory();
        public string categoryName = string.Empty;  //category name
        #endregion

        #region constructor
        public BasicDataSettingTree()
        {
            InitializeComponent();
            SetLanguageInfoToControl();             //获取该列表的名称 
        }
        #endregion

        #region SetLanguageInfoToControl
        private void SetLanguageInfoToControl()
        {
            //列表名称为“基础数据表类型” 
            this.lblTableType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.lblTableType}");
        }
        #endregion

        #region treeview mouse up event
        /// <summary>
        /// mouse up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvBasicSettings_MouseUp(object sender, MouseEventArgs e)
        {
            #region show basic datatable's viewcontent
            if(tvBasicSettings.Nodes.Count==0)
            {
                //define context menu
                contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.PopMenuAddTableType}"), new EventHandler(AddDataType));
                contextMenu.MenuItems[0].Enabled = true;
                //set the context menu's show position
                Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                //指定屏幕点的位置设置为工作点,参数值p为屏幕点位置坐标 modi by chao.pang
                p = this.PointToClient(p);
                //show context menu
                contextMenu.Show(this, p);
            }
            else
            {
                //if left button down and treeview has nodes 
                if (tvBasicSettings.GetNodeAt(e.X, e.Y) != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        //get right click node
                        tn = tvBasicSettings.GetNodeAt(e.X, e.Y);
                        //set selected node
                        tvBasicSettings.SelectedNode = tn;
                        //define context menu
                        contextMenu = new ContextMenu();
                        //add menu items to context menu
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.PopMenuEditColumn}"), new EventHandler(EditColumn));              //编辑列
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.PopMenuEditData}"), new EventHandler(EditData));                  //编辑数据
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.PopMenuAddTableType}"), new EventHandler(AddDataType));           //新增表类型
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.PopMenuDeleteTableType}"), new EventHandler(DeleteDataType));     //删除表类型
                        CheckPrivilege();
                        //set the context menu's show position
                        Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                        p = this.PointToClient(p);
                        //show context menu
                        contextMenu.Show(this, p);
                    }
                }
            }
            #endregion

        }
        #endregion

        #region CheckPrivilege add by rayna 2010-8-2
        private void CheckPrivilege()
        {
            contextMenu.MenuItems[0].Enabled = true;
            contextMenu.MenuItems[1].Enabled = true;
            contextMenu.MenuItems[2].Enabled = true;
            contextMenu.MenuItems[3].Enabled = true;
        }
        #endregion

        #region show EditColumn view content
        /// <summary>
        /// show EditColumn view content
        /// </summary>
        private void EditColumn(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == tn.Text + StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.ViewContentColumnTitle}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //pass the treenode information to viewcontent
            BasicSettingsViewContent basicSettingsViewContent = new BasicSettingsViewContent(tn);
            //if the viewcontent is opened, do not open again
            IsOpenedViewContent(basicSettingsViewContent, "column");
        }

        #endregion

        #region show EditData view content
        /// <summary>
        /// show EditData view content
        /// </summary>
        private void EditData(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == tn.Text + StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.ViewContentDataTitle}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //pass the treenode information to viewcontent
            BasicSettingsDatViewContent basicSettingsDatViewContent = new BasicSettingsDatViewContent(tn);
            //if the viewcontent is opened, do not open again
            IsOpenedViewContent(basicSettingsDatViewContent, "data");
        }

        #endregion

        #region show Add Data Type view content
        /// <summary>
        /// show Add Data Type view content
        /// </summary>
        private void AddDataType(object sender, EventArgs e)
        {
            DataSet dataDsCategoryKey = new DataSet();
            //show add data type window
            BasicDataTypeDeal basicDataTypeDeal = new BasicDataTypeDeal("add");
            basicDataTypeDeal.ShowDialog();

            #region insert new group name to table and add node to treeview
            if (basicDataTypeDeal.typeName != "")
            {
                //get category name
                categoryName = basicDataTypeDeal.typeName;  
                try
                {
                    //set value to entity
                    MapValueToEntity();

                    //save data  
                    baseAttributeCategoryEntity.SaveBaseCategory();
                    //check result
                    if (baseAttributeCategoryEntity.ErrorMsg == "")
                    {
                        //add treenode to treeview
                        //set category name
                        this.tvBasicSettings.Nodes.Add(baseAttributeCategoryEntity.CategoryName);
                        //set category key
                        this.tvBasicSettings.Nodes[tvBasicSettings.Nodes.Count - 1].Tag = baseAttributeCategoryEntity.CategoryKey;
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.MsgDataAddSuccessfully}"));
                    }
                    else
                    {
                        MessageBox.Show(StringParser.Parse(baseAttributeCategoryEntity.ErrorMsg));
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
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
            //set category key
            baseAttributeCategoryEntity.CategoryKey =  CommonUtils.GenerateNewKey(0);
            //set category name
            baseAttributeCategoryEntity.CategoryName = categoryName;
            //set descriptions
            baseAttributeCategoryEntity.Descriptions = "";
            //set creator
            baseAttributeCategoryEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            //set create time
            baseAttributeCategoryEntity.CreateTime = "";
            //set create time zone
            baseAttributeCategoryEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
        }
        #endregion

        #region DeleteDataType
        /// <summary>
        /// DeleteDataType
        /// </summary>
        private void DeleteDataType(object sender, EventArgs e)
        {
            DataSet dataDsCategoryKey = new DataSet();

            #region insert new group name to table and add node to treeview
            if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.MsgDeleteTableTypeText}"), StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.MsgDeleteTableTypeCaption}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    //set value to entity
                    baseAttributeCategoryEntity.CategoryKey = tvBasicSettings.SelectedNode.Tag.ToString();

                    //save data  
                    baseAttributeCategoryEntity.DeleteBaseCategory();
                    //check result
                    if (baseAttributeCategoryEntity.ErrorMsg == "")
                    {
                        //remove node
                        //add by yanrong liu 2010-08-27
                        foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                        {
                            if (viewContent.TitleName ==
                                tvBasicSettings.SelectedNode.Text + StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsViewContent.ViewContentPartTitle}"))
                            {                               
                                viewContent.WorkbenchWindow.CloseWindow(true);
                                continue;
                            }
                            if (viewContent.TitleName ==
                                tvBasicSettings.SelectedNode.Text + StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicSettingsDatViewContent.ViewContentPartTitle}"))
                            {
                                viewContent.WorkbenchWindow.CloseWindow(true);
                                continue;
                            }
                        }                       
                        //end
                        tvBasicSettings.Nodes.Remove(tvBasicSettings.SelectedNode);
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicData.BasicDataSettingTree.MsgDeleteTableTypeSuccessfully}"));
                                              
                    }
                    else
                    {
                        MessageBox.Show(StringParser.Parse(baseAttributeCategoryEntity.ErrorMsg));
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }            
           
            }
            #endregion

        }

        #endregion

        #region treeview initialize
        /// <summary>
        /// load tree node data 载入列表数据 modi by chao.pang
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasicDataSettingTree_Load(object sender, EventArgs e)
        {

            DataSet dataDS = new DataSet(); //dataset to receive category information
          
            try
            {
                //get data  
                dataDS=baseAttributeCategoryEntity.GetBaseCategory();
                //check result 
                if (baseAttributeCategoryEntity.ErrorMsg == "")
                {
                    //if there ara datas,initial tree 返回值dataDS中行数进行循环，获取值到节点下 modi by chao.pang
                    for (int i = 0; i < dataDS.Tables[0].Rows.Count; i++)
                    {
                        //set value to tree's node dataDS中CATEGORY_NAME列的值插到节点下 modi by chao.pang
                        tvBasicSettings.Nodes.Add(dataDS.Tables[0].Rows[i]["CATEGORY_NAME"].ToString());
                        //add tag 将节点对象对应到相应的CATEGORY_KEY值  modi by chao.pang
                        tvBasicSettings.Nodes[i].Tag = dataDS.Tables[0].Rows[i]["CATEGORY_KEY"].ToString();
                    }

                    //this.tvBasicSettings.Sort();
                }
                else
                {
                    MessageBox.Show(baseAttributeCategoryEntity.ErrorMsg);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        #endregion

        #region check the viewcontent is opened or not
        /// <summary>
        /// if the viewcontent need to be opened is opened do not open again
        /// </summary>
        /// <param name="titleName">viewcontent need to be opened</param>
        private void IsOpenedViewContent(IViewContent iViewContent, string columnOrData)
        {
            foreach (IViewContent item in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (columnOrData == "column")
                {
                    if ((iViewContent.TitleName.ToString()) == item.TitleName)
                    {
                        return;
                    }
                }
                else if (columnOrData == "data")
                {
                    //compare titlename
                    if ((iViewContent.TitleName.ToString()) == item.TitleName)
                    {
                        return;
                    }
                }
            }
            //show view content
            WorkbenchSingleton.Workbench.ShowView(iViewContent);
        }     
   
        #endregion 

 
    }
}

