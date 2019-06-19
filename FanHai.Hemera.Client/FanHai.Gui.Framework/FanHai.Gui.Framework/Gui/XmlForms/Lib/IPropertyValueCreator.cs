using System;

namespace FanHai.Gui.Framework.Gui.XmlForms
{
    public interface IPropertyValueCreator
    {
        bool CanCreateValueForType(Type propertyType);

        object CreateValue(Type propertyType, string valueString);

    }
}
