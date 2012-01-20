using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SampleProject.Common;

namespace SampleProject.Exceptions
{
    public class CustomExceptions : Exception
    {
        private Exception innerException;

        public CustomExceptions(Exception exception, Enums.ShowException showException)
        {
            this.innerException = exception;
            switch (showException)
            {
                case Enums.ShowException.EventViewer:
                    ShowMessageonEventViewer();
                    break;
                case Enums.ShowException.LogFile:
                    ShowMessageonLogFile();
                    break;
                default:
                    break;
            }
        }

        public CustomExceptions(Exception exception, HttpContext current, SPWeb web)
        {
            this.innerException = exception;
            ShowMessageonUI(current, web);
        }

        #region Private Methods
        private void ShowMessageonEventViewer()
        {
            System.Diagnostics.EventLog.WriteEntry(Constants.ERROR_KEY , this.innerException.Message + ": " + this.innerException.StackTrace, System.Diagnostics.EventLogEntryType.Error);
        }

        private void ShowMessageonLogFile()
        {
            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(Constants.ERROR_KEY, TraceSeverity.Medium, EventSeverity.Error), TraceSeverity.Monitorable, this.innerException.Message, this.innerException.StackTrace);
        }

        private void ShowMessageonUI(HttpContext current, SPWeb web)
        {
            SetErrorMessageforUI(current, web, this.innerException.Message);
        }
        #endregion
      
        #region UI Message
       
        #region Public Clear Message

        public static void ClearMessageFromUI(HttpContext current, SPWeb web)
        {
            if (current != null)
            {
                SetMessageVariable(current, web, String.Empty);
            }
        }
        #endregion

        #region Public Set Error Message
        public static void SetErrorMessageforUI(HttpContext current, SPWeb web, String sErrorMessage)
        {
            if (current != null)
            {
                if (!String.IsNullOrEmpty(sErrorMessage))
                {
                    String sMessage = Constants.ERROR_MESSAGE_SYMBOL + sErrorMessage;
                    SetMessageVariable(current, web, sMessage);
                }
            }
        }
        #endregion

        #region Public Set Success Message
        public static void SetSuccessMessageforUI(HttpContext current, SPWeb web, String sErrorMessage)
        {
            if (current != null)
            {
                if (!String.IsNullOrEmpty(sErrorMessage))
                {
                    String sMessage = Constants.SUCCESS_MESSAGE_SYMBOL + sErrorMessage;
                    SetMessageVariable(current, web, sMessage);
                }
            }
        }

        #endregion

        public static void SetMessageVariable(HttpContext current, SPWeb web, String sErrorMessage)
        {
            if (current != null)
            {
                current.Cache[CommonFunctions.GetErrorCodeKey(current, web)] = sErrorMessage;
            }
        }
        #endregion
    }
}
