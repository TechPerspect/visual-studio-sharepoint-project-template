using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace $safeprojectname$
{
    public class ElevatedCodeHelper
    {
        public delegate void PrivilegedCodeDelegate(SPWeb elevatedWeb);
      
        public static void ExecuteElevatedCode(Guid siteId, Guid webId, PrivilegedCodeDelegate webCode)
        {
            using (SPSite site = new SPSite(siteId))
            {
                SPWeb normalWeb = site.OpenWeb(webId);
                try
                {
                    ExecuteElevatedCode(normalWeb, webCode);
                }
                finally
                {
                    // Release web resources.
                    normalWeb.Dispose();
                }
            }
        }

        public static void ExecuteElevatedCode(SPWeb normalWeb, PrivilegedCodeDelegate webCode, SPBasePermissions permissionToCheck)
        {
            if (normalWeb.DoesUserHavePermissions(permissionToCheck))
            {
                webCode(normalWeb);
            }
            else
            {
                ExecuteElevatedCode(normalWeb, webCode);
            }
        }

        public static void ExecuteElevatedCode(SPWeb normalWeb, PrivilegedCodeDelegate webCode)
        {
            if (!normalWeb.UserIsSiteAdmin)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(normalWeb.Site.Url))
                    {
                        site.AllowUnsafeUpdates = true;
                        SPWeb elevatedWeb = site.OpenWeb();
                        try
                        {
                            elevatedWeb.AllowUnsafeUpdates = true;
                            webCode(elevatedWeb);
                        }
                        finally
                        {
                            elevatedWeb.Dispose();
                        }
                    }
                });
            }
            else
            {
                webCode(normalWeb);
            }
        }
    }
}
