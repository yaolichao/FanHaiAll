using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System.Data;
using FanHai.Hemera.Share.Constants;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;

namespace FanHai.Hemera.Utils.Controls
{
    public class ControlUtils
    {
        public static void QueryCheckStateByValue(object sender, QueryCheckStateByValueEventArgs e)
        {
            string value = string.Empty;

            if (e.Value != null)
            {
                value = e.Value.ToString();
            }

            switch (value)
            {
                case COMMON_VALUES.VALUE_COMMON_ONE:
                    e.CheckState = CheckState.Checked;
                    break;
                case COMMON_VALUES.VALUE_COMMON_ZERO:
                    e.CheckState = CheckState.Unchecked;
                    break;
                case COMMON_VALUES.VALUE_COMMON_YES:
                    e.CheckState = CheckState.Checked;
                    break;
                case COMMON_VALUES.VALUE_COMMON_NO:
                    e.CheckState = CheckState.Unchecked;
                    break;
                case COMMON_VALUES.VALUE_COMMON_TRUE:
                    e.CheckState = CheckState.Checked;
                    break;
                case COMMON_VALUES.VALUE_COMMON_FALSE:
                    e.CheckState = CheckState.Unchecked;
                    break;
                default:
                    e.CheckState = CheckState.Unchecked;
                    break;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Initial Grid View Control
        /// </summary>
        /// <param name="grv"></param>
        /// <param name="dt"></param>
        public static void InitialGridView(GridView grv, DataTable dt)
        {
            if (grv == null || grv.GridControl == null || dt == null || dt.Columns.Count <= 0) return;

            grv.Columns.Clear();

            int index = 0;

            foreach (DataColumn dc in dt.Columns)
            {
                GridColumn col = new GridColumn();

                col.Name = dc.ColumnName;
                col.FieldName = dc.ColumnName;
                col.Caption = dc.Caption;
                col.OptionsColumn.ReadOnly = dc.ReadOnly;
                col.Visible = true;
                col.VisibleIndex = index++;

                grv.Columns.Add(col);
            }

            grv.GridControl.DataSource = dt;

            grv.BestFitColumns();
        }
    }
    /// <summary>
    /// 字体常量。
    /// </summary>
    internal static class FontConst{
        public const string MENU_FONT_NAME = "Tahoma";
        public const string TITLE_FONT_NAME = "Tahoma";
        public const string CONTENT_FONT_NAME = "Tahoma";
        public const string CONTENT_GRID_FONT_NAME = "Tahoma";
        public const int MENU_FONT_SIZE = 12;
        public const int TITLE_FONT_SIZE = 14;
        public const int CONTENT_FONT_SIZE = 12;
        public const int CONTENT_GRID_FONT_SIZE = 10;
    }
}
