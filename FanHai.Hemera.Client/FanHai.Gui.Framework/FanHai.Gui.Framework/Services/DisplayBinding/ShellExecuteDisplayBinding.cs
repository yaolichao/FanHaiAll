using System;
using System.IO;
using System.Diagnostics;

using FanHai.Gui.Core;


namespace FanHai.Gui.Framework
{
	/// <summary>
	/// Opens files with the default Windows application for them.
	/// </summary>
	public class ShellExecuteDisplayBinding : IDisplayBinding
	{
		public bool CanCreateContentForFile(string fileName)
		{
			return !FileUtility.IsUrl(fileName);
		}
		
		
	}
}
