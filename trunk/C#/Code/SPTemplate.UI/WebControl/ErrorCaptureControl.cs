using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Web;
using $customUsing$.Common;
using $customUsing$.Exceptions;

namespace $safeprojectname$.WebControls
{
    public class ErrorCaptureControl : WebControl
    {
       

        #region Render Message
        protected override void OnPreRender(EventArgs e)
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            String sMessage = Convert.ToString(HttpContext.Current.Cache[CommonFunctions.GetErrorCodeKey(HttpContext.Current, SPContext.Current.Web)]);
            if (!String.IsNullOrEmpty(sMessage))
            {
                DisplayMessage(sMessage);
                ClearMessageFromUI();

                if (sMessage.Contains(Constants.UNIQUE_KEY_CODE_SUCESS))
                {
                    //unique will determine that the message needs to be displayed on second post back; case with long operations
                    sMessage = sMessage.Replace(Constants.UNIQUE_KEY_CODE_SUCESS, String.Empty);
                    SetSuccessMessageforUI(sMessage);
                }
                else if (sMessage.Contains(Constants.UNIQUE_KEY_CODE_FAILURE))
                {
                    sMessage = sMessage.Replace(Constants.UNIQUE_KEY_CODE_FAILURE, String.Empty);
                    SetErrorMessageforUI(sMessage);
                }
            }
        }
        #endregion

       #region Set Message in UI
        public static void ClearMessageFromUI()
        {
            if (HttpContext.Current != null)
            {
                CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, String.Empty);
            }
        }

        public static void ClearMessageFromUI(SPWeb web)
        {
            if (HttpContext.Current != null)
            {
                CustomExceptions.SetMessageVariable(HttpContext.Current, web, String.Empty);
            }
        }

        public static void SetErrorMessageforUI(String sErrorMessage)
        {
            if (HttpContext.Current != null)
            {
                if (!String.IsNullOrEmpty(sErrorMessage))
                {
                    String sMessage = Constants.ERROR_MESSAGE_SYMBOL + sErrorMessage;
                    CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, sMessage);
                }
            }
        }

        public static void SetErrorMessageforUI(SPWeb web, String sErrorMessage)
        {
            if (HttpContext.Current != null)
            {
                if (!String.IsNullOrEmpty(sErrorMessage))
                {
                    String sMessage = Constants.ERROR_MESSAGE_SYMBOL + sErrorMessage;
                    CustomExceptions.SetMessageVariable(HttpContext.Current, web, sMessage);
                }
            }
        }

        public static void SetSuccessMessageforUI(String sErrorMessage)
        {
            if (HttpContext.Current != null)
            {
                if (!String.IsNullOrEmpty(sErrorMessage))
                {
                    String sMessage = Constants.SUCCESS_MESSAGE_SYMBOL + sErrorMessage;
                    CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, sMessage);
                }
            }
        }

        public static void SetSuccessMessageforUI(SPWeb web, String sErrorMessage)
        {
            if (HttpContext.Current != null)
            {
                if (!String.IsNullOrEmpty(sErrorMessage))
                {
                    String sMessage = Constants.SUCCESS_MESSAGE_SYMBOL + sErrorMessage;
                    CustomExceptions.SetMessageVariable(HttpContext.Current, web, sMessage);
                }
            }
        }

        public static void SetSuccessMessageforLongOperations(String sMessage)
        {
            if (HttpContext.Current != null)
            {
                CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, sMessage + Constants.UNIQUE_KEY_CODE_SUCESS);
            }
        }

        public static void SetErrorMessageforLongOperations(String sMessage)
        {
            if (HttpContext.Current != null)
            {
                CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, sMessage + Constants.UNIQUE_KEY_CODE_FAILURE);
            }
        }
	    #endregion

        #region Private Methods
 

        private void DisplayMessage(String strMessage)
        {
            if (!String.IsNullOrEmpty(strMessage) && strMessage.Length > 1)
            {
                String sSymbol = strMessage.Substring(0, 1);
                strMessage = strMessage.Substring(1);
                strMessage = strMessage.Replace("'", "&#39;");

                switch (sSymbol)
                {
                    case Constants.ERROR_MESSAGE_SYMBOL:
                        ShowFailureMessage(strMessage);
                        break;
                    case Constants.SUCCESS_MESSAGE_SYMBOL:
                        ShowSuccessMessage(strMessage);
                        break;
                }
            }
        }
      

        private void ShowFailureMessage(String strMessage)
        {
            String strFormatString = String.Empty;
            try
            {
                strMessage = strMessage.Replace("\\r", "\\ r");
                strFormatString = String.Format("javascript:ShowFailureStatusBarMessage('{0}','{1}');", "Error :", strMessage);
                RegisterScript(strFormatString);
            }
            catch (Exception ex)
            {

            }
        }

        private void ShowSuccessMessage(String strMessage)
        {
            String strFormatString = String.Empty;
            try
            {
                strFormatString = String.Format("javascript:ShowSuccessStatusBarMessage('{0}','{1}');", "Note :", strMessage);
                RegisterScript(strFormatString);
            }
            catch (Exception ex)
            {

            }
        }

        private void RegisterScript(String strFormatString)
        {
            String strJavaScript = String.Empty;
            strJavaScript = "<script type=\"text/javascript\">ExecuteOrDelayUntilScriptLoaded(Initialize, \"sp.js\");function Initialize(){ " + strFormatString + " }</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "opennotification", strJavaScript, false);
        }

        #endregion
    }
}
