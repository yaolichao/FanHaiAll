using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Utils.Controls.Common
{
    public class GridViewHelper
    {
        /// <summary>
        /// 设置GridView格式
        /// </summary>
        /// <param name="gridView"></param>
        public static void SetGridView(GridView gridView)
        {
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                gridView.Columns[i].AppearanceHeader.BackColor = Color.FromArgb(251, 248, 240);
                gridView.PaintStyleName = "Web";//这一条必须要有
            }
            gridView.IndicatorWidth = 60;
            gridView.OptionsView.ShowIndicator = true;
            gridView.OptionsView.ShowAutoFilterRow = true;
            gridView.OptionsView.EnableAppearanceEvenRow = true;
            gridView.OptionsView.EnableAppearanceOddRow = true;
            gridView.Appearance.EvenRow.BackColor = Color.FromArgb(224, 224, 224);
            gridView.Appearance.OddRow.BackColor = Color.White;
        }
    }
}
