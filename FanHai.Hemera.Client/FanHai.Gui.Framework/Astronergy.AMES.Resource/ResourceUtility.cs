using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraGrid.Localization;

namespace Astronergy.AMES.Resource
{
    public class ResourceUtility
    {
        public static void InitResources()
        {
            GridLocalizer.Active = new XtraGridLocalizer();
        }
    }
}
