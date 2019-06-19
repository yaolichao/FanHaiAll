using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraGrid.Localization;
using System.Resources;
using System.Threading;

namespace Astronergy.AMES.Resource
{
    /// <summary>
    /// DevExpress XtraGrid控件本地化资源类。
    /// </summary>
    public class XtraGridLocalizer : GridLocalizer
    {
        public override string GetLocalizedString(GridStringId id)
        {
            string ret = "";
            try
            {
                ResourceManager rsManager = new ResourceManager("Astronergy.AMES.Resource.DevExpress.XtraGridResource", typeof(XtraGridLocalizer).Assembly);
                string resourceName = "GridStringId." + Convert.ToString(id);
                object ob = rsManager.GetObject(resourceName, Thread.CurrentThread.CurrentUICulture);
                if (ob != null)
                {
                    ret = Convert.ToString(ob);
                }
            }
            catch (MissingManifestResourceException)
            {
                return base.GetLocalizedString(id);
            }
            return ret;
        }
    }
}
