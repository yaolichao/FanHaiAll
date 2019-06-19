using System;
using System.Xml;

namespace FanHai.Gui.Framework.Gui.XmlForms
{
    /// <summary>
    /// This interface is used to create the objects which are given by name in 
    /// the xml definition.
    /// </summary>
    public interface IObjectCreator
    {
        /// <summary>
        /// Creates a new instance of object name. 
        /// </summary>
        object CreateObject(string name, XmlElement el);

        Type GetType(string name);
    }
}
