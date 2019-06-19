using System;
using System.Drawing;

using FanHai.Gui.Core.WinForms;

namespace FanHai.Gui.Framework.Gui.XmlForms
{
    public class SolarViewerFrameworkPropertyValueCreator : IPropertyValueCreator
    {
        public bool CanCreateValueForType(Type propertyType)
        {
            return propertyType == typeof(Icon) || propertyType == typeof(Image);
        }

        public object CreateValue(Type propertyType, string valueString)
        {

            if (propertyType == typeof(Icon))
            {
                return WinFormsResourceService.GetIcon(valueString);
            }

            if (propertyType == typeof(Image))
            {
                return WinFormsResourceService.GetBitmap(valueString);
            }

            return null;
        }
    }
}
