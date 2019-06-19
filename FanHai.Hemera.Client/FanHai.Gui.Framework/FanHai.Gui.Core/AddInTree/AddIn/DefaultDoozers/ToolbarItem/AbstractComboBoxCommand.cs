// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Hao.Zhang" email=""/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;

namespace FanHai.Gui.Core
{
	public abstract class AbstractComboBoxCommand : AbstractCommand, IComboBoxCommand
	{
		bool isEnabled = true;
		
		public virtual bool IsEnabled {
			get {
				return isEnabled;
			}
			set {
				isEnabled = value;
			}
		}
		
		public override void Run()
		{
			
		}
		
	}
}
