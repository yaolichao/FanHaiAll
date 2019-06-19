using System;
using System.Windows.Forms;


using FanHai.Gui.Core;

namespace FanHai.Gui.Framework.Gui.XmlForms
{
    public abstract class BaseSolarViewerFrameworkpForm : XmlForm
    {
        public BaseSolarViewerFrameworkpForm()
        {
        }

        protected override void SetupXmlLoader()
        {
            xmlLoader.StringValueFilter = new SolarViewerFrameworkStringValueFilter();
            xmlLoader.PropertyValueCreator = new SolarViewerFrameworkPropertyValueCreator();
        }

        public void SetEnabledStatus(bool enabled, params string[] controlNames)
        {
            foreach (string controlName in controlNames)
            {
                Control control = ControlDictionary[controlName];
                if (control == null)
                {
                    MessageService.ShowError(controlName + " not found!");
                }
                else
                {
                    control.Enabled = enabled;
                }
            }
        }


    }
}
