/*
<FileInfo>
  <Author>rayna liu FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 冯旭                 2012-02-10            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Gui.Core
{
    public static class PrivilegeService
    {
        /// <summary>
        /// 判断当前用户是否具有指定的权限
        /// </summary>
        /// <param name="privilegeCode">要确认的权限号</param>
        /// <returns>有指定的权限时，返回true</returns>
        public static bool HavePrivilege(string privilegeCode)
        {
            string privilegeStr = PropertyService.Get("UserPrivilege");
            if (privilegeStr.IndexOf(privilegeCode) == -1)
            {
                return false;
            }
            else
            {
                return true;
            } 
        }
    }
}
