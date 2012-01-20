using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;
using $customUsing$.Operations;
using $customUsing$.TimerJobs;
using System.Web;
using $customUsing$.Exceptions;
using $customUsing$.Common; 

namespace $safeprojectname$.Features
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    public class InstallTimerJobEventReceiver : SPFeatureReceiver
    {
         
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                //Add Site Columns For Effectivity Functionality
                SPSite objSite = properties.Feature.Parent as SPSite;
                if (objSite != null)
                {
                    using (SPSite objSPSite = properties.Feature.Parent as SPSite)
                    {
                        InstallFirstTimerJob(objSPSite.RootWeb);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

     


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {

            //Add Site Columns For Effectivity Functionality
            SPSite objSite = properties.Feature.Parent as SPSite;
            if (objSite != null)
            {
                using (SPSite objSPSite = properties.Feature.Parent as SPSite)
                {
                    try
                    {
                        DeleteFirstTimerJob(objSPSite.RootWeb);
                    }
                    catch (Exception ex)
                    {
                        if (HttpContext.Current != null)
                        {
                            new CustomExceptions(ex, HttpContext.Current, objSPSite.RootWeb);
                        }
                        else
                        {
                            new CustomExceptions(ex, Enums.ShowException.EventViewer);
                        }
                    }
                }
            }

        }


        #region Install and Delete First Timer Job
        public void InstallFirstTimerJob(SPWeb web)
        {

            DeleteFirstTimerJob( web);

            try
            {
                // install the timer job
                SampleTimerJob objFirstTimer = new SampleTimerJob("$safeprojectname$ First Timer Job", web.Site.WebApplication);
                SPMinuteSchedule schedule = new SPMinuteSchedule();

                schedule.Interval = 15;
                schedule.BeginSecond = 0;
                schedule.EndSecond = 0;

                objFirstTimer.Schedule = schedule;
                objFirstTimer.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteFirstTimerJob(SPWeb web)
        {
            try
            {
                SPJobDefinition objFirstTimerJob = web.Site.WebApplication.JobDefinitions["$safeprojectname$ First Timer Job"];
                if (objFirstTimerJob != null)
                {
                    objFirstTimerJob.Delete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
