using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using SampleProject.Exceptions;
using SampleProject.Common;
using System.Web;
using SampleProject.Operations;


namespace SampleProject.UI.Features
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    public class ErrorCapturingEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            if (properties != null)
            {
                using (SPWeb objSPWeb = (properties.Feature.Parent as SPSite).OpenWeb())
                {
                    if (objSPWeb != null)
                    {
                        try
                        {
                            InstallCustomActions objCustomAction = new InstallCustomActions(objSPWeb);
                            objCustomAction.RegisterCustomCoreInScriptLink();
                        }
                        catch (Exception ex)
                        {
                            if (HttpContext.Current != null)
                            {
                                new CustomExceptions(ex, HttpContext.Current, objSPWeb);
                            }
                            else
                            {
                                new CustomExceptions(ex,Enums.ShowException.EventViewer);
                            }
                        }
                    }
                }
            }

        }

       
        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
