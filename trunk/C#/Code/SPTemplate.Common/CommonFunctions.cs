using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web;

namespace $safeprojectname$
{
    public class CommonFunctions
    {
        #region Normal Static Method

        public static String GetColumnTypeByFieldGUID(String strfieldGuid)
        {
            SPField objSPField;
            SPWeb objSPWeb = null;
            String strRetVal = String.Empty;

            try
            {
                objSPWeb = SPContext.Current.Web;
                objSPField = objSPWeb.Fields[new Guid(strfieldGuid)];

                if (objSPField != null)
                {
                    strRetVal = objSPField.TypeAsString;
                }
                return strRetVal;
            }
            catch (Exception Ex)
            {
                return strRetVal;
            }
        }

        public static String GetSiteUrlByID(String strWebID)
        {
            String strWebUrl = String.Empty;
            try
            {
                using (SPSite objSPSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb objCurrentWeb = objSPSite.OpenWeb(new Guid(strWebID)))
                    {
                        if (objCurrentWeb != null)
                        {
                            strWebUrl = objCurrentWeb.Url;
                        }
                    }
                }
                return strWebUrl;
            }
            catch (Exception Ex)
            {
                return strWebUrl;
            }
        }


        public static string RemoveSpecialNames(String sFilename)
        {
            sFilename = sFilename.Replace(".files", ".files_");
            sFilename = sFilename.Replace("_files", "_files_");
            sFilename = sFilename.Replace("-Dateien", "-Dateien_");
            sFilename = sFilename.Replace("_fichiers", "_fichiers_");
            sFilename = sFilename.Replace("_bestanden", "_bestanden_");
            sFilename = sFilename.Replace("_file", "_file_");
            sFilename = sFilename.Replace("_archivos", "_archivos_");
            sFilename = sFilename.Replace("-filer", "-filer_");
            sFilename = sFilename.Replace("_tiedostot", "_tiedostot_");
            sFilename = sFilename.Replace("_pliki", "_pliki_");
            sFilename = sFilename.Replace("_soubory", "_soubory_");
            sFilename = sFilename.Replace("_elemei", "_elemei_");
            sFilename = sFilename.Replace("_ficheiros", "_ficheiros_");
            sFilename = sFilename.Replace("_arquivos", "_arquivos_");
            sFilename = sFilename.Replace("_dosyalar", "_dosyalar_");
            sFilename = sFilename.Replace("_datoteke", "_datoteke_");
            sFilename = sFilename.Replace("_fitxers", "_fitxers_");
            sFilename = sFilename.Replace("_failid", "_failid_");
            sFilename = sFilename.Replace("_fails", "_fails_");
            sFilename = sFilename.Replace("_bylos", "_bylos_");
            sFilename = sFilename.Replace("_fajlovi", "_fajlovi_");
            sFilename = sFilename.Replace("_fitxategiak", "_fitxategiak_");
            return sFilename;
        }

        public static String EscapeCAMLChars(String sName)
        {
            String sReturn = sName;
            sReturn = sReturn.Replace("&", "&amp;");
            sReturn = sReturn.Replace("<", "&lt;");
            sReturn = sReturn.Replace(">", "&gt;");
            return sReturn;
        }

        public static String RevertCAMLChars(String sText)
        {
            String sReturn = sText;
            sReturn = sReturn.Replace("&amp;", "&");
            sReturn = sReturn.Replace("&lt;", "<");
            sReturn = sReturn.Replace("&gt;", ">");
            return sReturn;
        }

        public static Boolean IsExistsCustomAction(String strCustomAction, String strLocation, String strRegistrationID, SPWeb objRootWeb)
        {
            Boolean IsExists = false;
            //objRootWeb = this.CurrentWeb.Site.RootWeb;
            foreach (SPUserCustomAction objSPUserCustomAction in objRootWeb.UserCustomActions)
            {
                if (objSPUserCustomAction.Location.Equals(strLocation)
                    && objSPUserCustomAction.Name != null
                    && objSPUserCustomAction.Name.Equals(strCustomAction))
                {
                    if (!String.IsNullOrEmpty(strRegistrationID))
                    {
                        if (objSPUserCustomAction.RegistrationId != null && objSPUserCustomAction.RegistrationId.Equals(strRegistrationID))
                        {
                            IsExists = true;
                            break;
                        }
                    }
                    else
                    {
                        IsExists = true;
                        break;
                    }
                }
            }
            return IsExists;
        }

        public  static String GetErrorCodeKey(HttpContext current, SPWeb web)
        {

            return Convert.ToString(current.Request.UserHostAddress) + "_" + web.CurrentUser.LoginName + "_" + Constants.KEY_SUFFIX;
        }
        #endregion
    }
}
