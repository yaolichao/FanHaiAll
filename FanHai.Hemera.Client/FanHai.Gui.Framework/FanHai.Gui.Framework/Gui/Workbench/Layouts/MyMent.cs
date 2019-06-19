using DevExpress.XtraNavBar;
using FanHai.Gui.Core;
using FanHai.Gui.Core.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FanHai.Gui.Framework.Gui.Workbench.Layouts
{
    public class MyMent
    {
        MenuStrip menuStrip;
        NavBarControl navBarControl;
        public delegate void OnClickDel(MenuCommand command);
        public MyMent(MenuStrip _menuStrip)
        {
            menuStrip = _menuStrip;
        }
        public NavBarControl CreateControl()
        {
            if (navBarControl == null)
            {
                navBarControl = new NavBarControl();
                navBarControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
                navBarControl.PaintStyleName = "SkinNav:Office 2007 Blue";
                navBarControl.PaintStyleKind = NavBarViewKind.NavigationPane;
                navBarControl.Dock = DockStyle.Left;
                navBarControl.Width = 200;
                navBarControl.BeginInit();
                navBarControl.SuspendLayout();
            }

            if (menuStrip != null)
            {
                try
                {
                    foreach (ToolStripMenuItem item in menuStrip.Items)
                    {
                        NavBarGroup group = CreateNavBarGroup1(item);
                        navBarControl.Groups.Add(group);
                        //navBarControl.Groups.Add(CreateNavBarGroup(command));

                        //foreach (ToolStripMenuItem item1 in item.DropDownItems)
                        //{
                        //    foreach (ToolStripMenuItem item2 in item1.DropDownItems)
                        //    {
                        //        command = (MenuCommand)item2;

                        //        break;
                        //    }
                        //    break;
                        //}
                        //break;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            navBarControl.EndInit();
            navBarControl.ResumeLayout(false);
            return navBarControl;
        }
        private NavBarGroup CreateNavBarGroup1(ToolStripMenuItem items)
        {
            try
            {
                NavBarGroup navBarGroup = new NavBarGroup();
                navBarGroup.Caption = items.Text;
                navBarGroup.Expanded = true;
                navBarGroup.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;

                NavBarControl navBar = CreateNavBarGroup2(items);
                if (navBar != null)
                {
                    NavBarGroupControlContainer navBarGroupControl = new NavBarGroupControlContainer();
                    navBarGroupControl.Appearance.BackColor = Color.Red;
                    navBarGroupControl.Appearance.Options.UseBackColor = true;
                    navBarGroupControl.Controls.Add(navBar);

                    navBarGroup.ControlContainer = navBarGroupControl;
                    navBarControl.Controls.Add(navBarGroupControl);                
                }
                return navBarGroup;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private NavBarGroup CreateNavBarGroup(ToolStripMenuItem items)
        {
            NavBarGroup navBarGroup = new NavBarGroup();

            try
            {
                navBarGroup.Caption = items.Text;
                foreach (ToolStripMenuItem item in items.DropDownItems)
                {
                    navBarGroup.ItemLinks.Add(CreateNavBarItem((MenuCommand)item));
                }
                return navBarGroup;
            }
            catch (Exception ex)
            {
                return navBarGroup;
            }


        }
        private NavBarItem CreateNavBarItem(MenuCommand command)
        {
            NavBarItem navBarItem = new NavBarItem();
            navBarItem.Caption = command.Text;

            //navBarItem.LinkClicked +=new NavBarLinkEventHandler(OnClickEvent(command));
            navBarItem.LinkClicked += (e, a) => OnClickEvent(command);
            return navBarItem;
        }
        public event OnClickDel OnClick;
        private void OnClickEvent(MenuCommand command)
        {
            if (command != null)
            {
                ICommand cmd = command.Command;
                LoggingService.Info("Run command " + cmd.GetType().FullName);
                cmd.Run();
            }
        }
        private NavBarControl CreateNavBarGroup2(ToolStripMenuItem items)
        {
            NavBarControl navBarControl2lenvel = new NavBarControl();

            try
            {
                //navBarContro.ActiveGroup = this.navBarGroup2;
                navBarControl2lenvel.Dock = System.Windows.Forms.DockStyle.Fill;
                //navBarContro.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
                //navBarGroup2});
                foreach (object item in items.DropDownItems)
                {                   
                    navBarControl2lenvel.Groups.Add(CreateNavBarGroup((ToolStripMenuItem)item));
                }

                navBarControl2lenvel.Location = new System.Drawing.Point(0, 0);
                //navBarControl2lenvel.Name = "navBarControl2";
                navBarControl.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
                //navBarControl.Size = new System.Drawing.Size(224, 385);
                navBarControl2lenvel.TabIndex = 0;
                navBarControl2lenvel.Text = items.Text;
                navBarControl2lenvel.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinExplorerBarViewInfoRegistrator("Caramel");
                navBarControl2lenvel.Dock = DockStyle.Fill;
                return navBarControl2lenvel;
            }
            catch (Exception ex)
            {
                return navBarControl2lenvel;
            }

        }

    }
}
