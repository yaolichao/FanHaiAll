using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Common
{
    public class ADValidate
    {
        //LDAP Path
        private const string ADPath = "LDAP://FanHai.com";

        /// <summary>
        /// User AD validate
        /// </summary>
        /// <param name="userName">UserAccount</param>
        /// <param name="passWord">PassWord</param>
        /// <returns>True or False</returns>
        public static bool ADUserValidate(string userName, string passWord)
        {
            DirectoryEntry directoryEntry = GetDirectoryObject(userName, passWord);

            if (GetSearchResultByAccount(directoryEntry, userName) == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get user info to dictionary
        /// </summary>
        /// <param name="sAMAccountName">UserAccount</param>
        /// <returns>IDictionary Collection</returns>
        public static IDictionary GetDirectoryEntryByAccount(string sAMAccountName)
        {
            Dictionary<string, string> userInfo = new Dictionary<string, string>();
            DirectoryEntry directoryEntry = GetDirectoryObject();
            SearchResult result = GetSearchResultByAccount(directoryEntry, sAMAccountName);
            if (result != null)
            {
                foreach (string propertyName in result.Properties.PropertyNames)
                {
                    userInfo.Add(propertyName, result.Properties[propertyName][0].ToString());
                }

                return userInfo;
            }

            return null;
        }

        /// <summary>
        /// Via user account get user info
        /// </summary>
        /// <param name="directoryEntry">User DirectoryEntry</param>
        /// <param name="sAMAccountName">User Account</param>
        /// <returns>User Info</returns>
        public static SearchResult GetSearchResultByAccount(DirectoryEntry directoryEntry, string sAMAccountName)
        {
            DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
            directorySearcher.Filter = "(&(&(objectCategory=person)(objectClass=user))(sAMAccountName=" + sAMAccountName + "))";
            directorySearcher.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = directorySearcher.FindOne();
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Get user property via property name
        /// </summary>
        /// <param name="searchResult">SearchResult</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>User Property</returns>
        public static string GetProperty(SearchResult searchResult, string propertyName)
        {
            if (searchResult.Properties.Contains(propertyName))
            {
                return searchResult.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Via the domain get this  DirectoryEntry
        /// </summary>
        /// <returns>DirectoryEntry</returns>
        private static DirectoryEntry GetDirectoryObject()
        {
            DirectoryEntry entry = new DirectoryEntry(ADPath);
            return entry;
        }

        /// <summary>
        /// Via the domain, username and password get this DirectoryEntry 
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="passWord">PassWord</param>
        /// <returns>DirectoryEntry</returns>
        private static DirectoryEntry GetDirectoryObject(string userName, string passWord)
        {
            DirectoryEntry entry = new DirectoryEntry(ADPath, userName, passWord, AuthenticationTypes.Secure);
            return entry;
        }
    }
}
