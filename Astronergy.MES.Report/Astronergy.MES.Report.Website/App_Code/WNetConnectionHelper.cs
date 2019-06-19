using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;

public static class WNetConnectionHelper
{
    [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2")]
    private static extern uint WNetAddConnection2(NetResource lpNetResource, string lpPassword, string lpUsername, uint dwFlags);
 
    [DllImport("Mpr.dll", EntryPoint = "WNetCancelConnection2")]
    private static extern uint WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);
 
    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public int dwScope;
        public int dwType;
        public int dwDisplayType;
        public int dwUsage;
        public string lpLocalName;
        public string lpRemoteName;
        public string lpComment;
        public string lpProvider;
    }
 
    public static uint WNetAddConnection(NetResource netResource,string username,string password)
    {
        uint result = WNetAddConnection2(netResource, password, username, 0);
        return result;
    }
 
    public static uint WNetAddConnection(string username, string password, string remoteName, string localName)
    {
        NetResource netResource = new NetResource();
        netResource.dwScope = 2;       //RESOURCE_GLOBALNET
        netResource.dwType = 1;       //RESOURCETYPE_ANY
        netResource.dwDisplayType = 3; //RESOURCEDISPLAYTYPE_GENERIC
        netResource.dwUsage = 1;       //RESOURCEUSAGE_CONNECTABLE
        netResource.lpLocalName = localName;
        netResource.lpRemoteName = remoteName.TrimEnd('\\');
        //netResource.lpRemoteName = lpComment;
        //netResource.lpProvider = null;
        uint result = WNetAddConnection2(netResource, password, username, 0);
        return result;
    }
 
    public static uint WNetCancelConnection(string name,uint flags,bool force)
    {
        uint nret = WNetCancelConnection2(name, flags, force);
        return nret;
    }
}