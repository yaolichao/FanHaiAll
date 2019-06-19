
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class PorLineHelpDialog : Form
    {
        #region variable define
        private static PorLineHelpDialog instance;
        public string _returnLineName = "";   //LineName
        public string _returnLineKey = "";    //LineKey 
        public static DevExpress.XtraEditors.ButtonEdit _becontrolName;
        public static DevExpress.XtraEditors.TextEdit _tecontrolName;
        #endregion

        #region constructor
        public PorLineHelpDialog()
        {
            InitializeComponent();
            this.gridControl1.MainView = gvpnhelp;
            SetLanguageInfoToControl();
            //set gridcontrol's main view
           
        }

        private void SetLanguageInfoToControl()
        {
            this.lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PorLineHelpDialog.lblName}");
            this.lblLine.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PorLineHelpDialog.lblLine}");
            this.lblShow.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PorLineHelpDialog.lblShow}");
            this.btnsearch.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PorLineHelpDialog.btnsearch}");

            this.LINE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PorLineHelpDialog.gridColumnLineName}");
            this.LINE_CODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PorLineHelpDialog.gridColumnLineCode}");
        }
        #endregion

        #region btnsearch_Click
        /// <summary>
        /// search click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsearch_Click(object sender, EventArgs e)
        {
            #region variable define
            string _lineName = ""; //line name
            string _showNumber = "";    //show number
            #endregion

            #region detail deal
            //get part number
            _lineName = this.txtpartnumber.Text.Trim().ToString();
            //get show number
            _showNumber = this.txtshownumber.Text.Trim().ToString();
            //new ds
            DataSet _ds = new DataSet();
            DataSet _dataDsFrom=new DataSet();
            //check

            if (_showNumber != "")
            {
                if (Convert.ToInt32(_showNumber) < 0)
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PorLineHelpDialog.MsgSearchShowLineCheck}"));
                    return;
                }
                else
                {
                    try
                    {
                        //clear data in grid
                        this.gvpnhelp.GridControl.DataSource = null;
                        //UnregisterChannel
                        CallRemotingService.UnregisterChannel();
                        //get server object factory
                        IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                        //get parameter dataset
                        _dataDsFrom = AddinCommonStaticFunction.GetTwoColumnsCommonDs();
                        //add data to dataset
                        _dataDsFrom.Tables[0].Rows.Add();
                        _dataDsFrom.Tables[0].Rows[0][0] = "LINE_NAME";
                        _dataDsFrom.Tables[0].Rows[0][1] = _lineName;
                        _dataDsFrom.Tables[0].Rows.Add();
                        _dataDsFrom.Tables[0].Rows[1][0] = "rownum";
                        _dataDsFrom.Tables[0].Rows[1][1] = _showNumber;
                        //get return dataset
                        _ds = iServerObjFactory.CreateILineManageEngine().GetHelpInfoForLineHelpForm(_dataDsFrom);
                        string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(_ds);
                        //check return dataset
                        if (string.IsNullOrEmpty(msg))
                        {
                            //bind data to grid
                            this.gvpnhelp.GridControl.DataSource = _ds.Tables[0];
                        }
                        else
                        {
                            MessageBox.Show(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        //UnregisterChannel
                        CallRemotingService.UnregisterChannel();
                    }
                }
            }
            #endregion

        }
        #endregion

        #region gridControl1_DoubleClick
        /// <summary>
        /// gridControl1_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gvpnhelp.FocusedRowHandle >= 0)
            {
                //get LineName
                _returnLineName = gvpnhelp.GetRowCellValue(gvpnhelp.FocusedRowHandle, "LINE_NAME").ToString();
                //get LineKey
                _returnLineKey = gvpnhelp.GetRowCellValue(gvpnhelp.FocusedRowHandle, "PRODUCTION_LINE_KEY").ToString();

                this.gridView1.GridControl.DataSource = null;

                //hide form
                this.Visible = false;
                //set value
                _becontrolName.Text = _returnLineName;
                _tecontrolName.Text = _returnLineKey;

                
                
            }


        }
        #endregion

        #region singleton get instance
        /// <summary>
        /// get instance
        /// </summary>
        public static PorLineHelpDialog Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PorLineHelpDialog();
                }
                return instance;
            }
        }
        #endregion

        #region singleton get instance BeControlName
        /// <summary>
        /// get instance
        /// </summary>
        public static DevExpress.XtraEditors.ButtonEdit BeControlName
        {
            set
            {
                _becontrolName = value;
            }
        }
        #endregion

        #region singleton get instance
        /// <summary>
        /// get instance
        /// </summary>
        public static DevExpress.XtraEditors.ButtonEdit TeControlName
        {
            set
            {
                _tecontrolName = value;
            }
        }
        #endregion

        private void PorLineHelpDialog_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
