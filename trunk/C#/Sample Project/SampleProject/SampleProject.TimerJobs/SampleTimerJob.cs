using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using SampleProject;
using SampleProject.Common;
using SampleProject.Exceptions;

namespace SampleProject.TimerJobs
{
    public class SampleTimerJob : SPJobDefinition
    {
        # region -- Constructors --

        public SampleTimerJob()
            : base()
        {
        }

        public SampleTimerJob(string jobName, SPService service, SPServer server)
            : base(jobName, service, server, SPJobLockType.Job)
        {
        }

        public SampleTimerJob(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        #endregion

        #region Public Method

        public override void Execute(Guid contentDbId)
        {
            try
            {
                if (this.WebApplication != null)
                {
                    if (this.WebApplication.Sites.Count > 0)
                    {
                        foreach (SPSite objSPSite in this.WebApplication.Sites)
                        {
                            ExecuteMethod(objSPSite.RootWeb);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                new CustomExceptions(Ex,Enums.ShowException.EventViewer);
            }
        }
        #endregion

        #region Private Methods
        private void ExecuteMethod(SPWeb objWeb)
        {
            //Change List item Title to "Timer Job" if 
            //list item title is test then change to Time Job
            ElevatedCodeHelper.ExecuteElevatedCode(objWeb, delegate(SPWeb elevatedWeb)
            {
                foreach (SPList list in elevatedWeb.Lists)
                {
                    if (!list.Hidden && !list.IsApplicationList && !list.IsSiteAssetsLibrary)
                    {
                        foreach (SPListItem ltItem in list.Items)
                        {
                            if (String.Compare(ltItem.Title, "test", true) == 0)
                            {
                                ltItem[SPBuiltInFieldId.Title] = "Time Job";
                                ltItem.SystemUpdate(false);
                            }
                        }
                    }
                }
            });
        }
        #endregion
    }
}
