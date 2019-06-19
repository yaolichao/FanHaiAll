﻿using System;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Gui.Framework
{
	/// <summary>
    /// This class defines the SolarViewerFramework display binding interface, it is a factory
	/// structure, which creates IViewContents.
	/// </summary>
	public interface ISecondaryDisplayBinding
	{
		/// <summary>
		/// Gets if the display binding can attach to the specified view content.
		/// </summary>
		bool CanAttachTo(IViewContent content);
		
		/// <summary>
		/// When you return true for this property, the CreateSecondaryViewContent method
		/// is called again after the LoadSolutionProjects thread has finished.
		/// </summary>
		bool ReattachWhenParserServiceIsReady { get; }
		
		/// <summary>
		/// Creates the secondary view contents for the given view content.
		/// If ReattachWhenParserServiceIsReady is used, the implementation is responsible
		/// for checking that no duplicate secondary view contents are added.
		/// </summary>
		IViewContent[] CreateSecondaryViewContent(IViewContent viewContent);
	}
}
