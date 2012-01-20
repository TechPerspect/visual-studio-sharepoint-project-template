using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace $safeprojectname$
{
    public static class ExtensionMethods
    {
       #region Extension Methods
        public static Boolean HasSpecialCharacterForFileandFolder(this String sValue)
        {
            Char[] sChars = { '~','#','%','&','*','{','}','\\',':','<','>','?','/','|','\"' };

            if (sValue.IndexOfAny(sChars) > -1)
            {
                return true;
            }

            if (sValue.IndexOf("..") > -1)
            {
                return true;
            }

            if (sValue.IndexOf("'") > -1)
            {
                return true;
            }

            if (sValue.StartsWith("."))
            {
                return true;
            }

            if (sValue.EndsWith("."))
            {
                return true;
            }

            String[] array = { ".files", "_files", "-Dateien", "_fichiers", "_bestanden","_file","_archivos",
                                 "-filer","_tiedostot","_pliki","_soubory","_elemei","_ficheiros","_arquivos",
                                 "_dosyalar","_datoteke","_fitxers","_failid","_fails","_bylos","_fajlovi","_fitxategiak" };

            for (int iLoop = 0; iLoop < array.Length; iLoop++)
            {
                if (sValue.EndsWith(array[iLoop]))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean HasSpecialCharacterForSiteandGroup(this String sValue)
        {
            Char[] sChars = { '~', '#', '%', '&', '*', '{', '}', '\\', ':', '<', '>', '?', '/', '+', '|', '"' };

            if (sValue.IndexOfAny(sChars) > -1)
            {
                return true;
            }

            if (sValue.StartsWith("_"))
            {
                return true;
            }

            if (sValue.IndexOf("..") > -1)
            {
                return true;
            }

            if (sValue.EndsWith("."))
            {
                return true;
            }

            if (sValue.StartsWith("."))
            {
                return true;
            }
            return false ;
        }

        public static String GetResourceString(this String Key,String sResourceFileName)
        {
            SPWeb currentWeb = null;
            if (SPContext.Current != null && SPContext.Current.Web != null)
            {
                currentWeb = SPContext.Current.Web;
            }
            uint LanguageCode = currentWeb != null ? currentWeb.Language : 1033;
            return SPUtility.GetLocalizedString("$Resources:" + Key, sResourceFileName, LanguageCode);
        }

        public static String GetResourceString(this String Key, SPWeb currentWeb,String sResourceFileName)
        {
            uint LanguageCode = currentWeb != null ? currentWeb.Language : 1033;
            return Convert.ToString(SPUtility.GetLocalizedString("$Resources:" + Key, sResourceFileName, LanguageCode));
        }

        public static String Serialise(this object objEntity)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(objEntity.GetType());
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, objEntity);
                    xmlStream.Position = 0;
                    xmlDoc.Load(xmlStream);
                }
            }
            catch (Exception Ex)
            {
            }
            return xmlDoc.InnerXml;
        }

        public static Object DeSerialise(this string XMLString, Object objEntity)
        {
            try
            {
                XmlSerializer oXmlSerializer = new XmlSerializer(objEntity.GetType());
                objEntity = oXmlSerializer.Deserialize(new StringReader(XMLString));
            }
            catch { }
            return objEntity;
        }
 
        public static void SaveCustomAttribute(this SPField sField, string propertyName, object propertyValue)
        {
            if (sField != null)
            {
                try
                {
                    Type type = typeof(SPField);
                    if (propertyValue != null)
                    {
                        MethodInfo set = type.GetMethod("SetFieldAttributeValue", BindingFlags.NonPublic | BindingFlags.Instance);
                        set.Invoke(sField, new object[] { propertyName, propertyValue.ToString() });
                        sField.Update();
                    }
                    else
                    {
                        MethodInfo remove = type.GetMethod("RemoveFieldAttributeValue", BindingFlags.NonPublic | BindingFlags.Instance);
                        remove.Invoke(sField, new object[] { propertyName });
                        sField.Update();
                    }
                }
                catch
                {
                }
            }
        }

        public static void SetCustomAttribute(this SPField sField, string propertyName, object propertyValue)
        {
            if (sField != null)
            {
                try
                {
                    Type type = typeof(SPField);
                    if (propertyValue != null)
                    {
                        MethodInfo set = type.GetMethod("SetFieldAttributeValue", BindingFlags.NonPublic | BindingFlags.Instance);
                        set.Invoke(sField, new object[] { propertyName, propertyValue.ToString() });
                    }
                    else
                    {
                        MethodInfo remove = type.GetMethod("RemoveFieldAttributeValue", BindingFlags.NonPublic | BindingFlags.Instance);
                        remove.Invoke(sField, new object[] { propertyName });
                    }
                }
                catch
                {
                }
            }
        }
     
        public static string GetCustomAttribue(this SPField sField, string propertyName)
        {
            String sReturn = String.Empty;
            if (sField != null)
            {
                Type type = typeof(SPField);
                MethodInfo getField = type.GetMethod("GetFieldAttributeValue", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(String) }, null);
                object objValue = getField.Invoke(sField, new object[] { propertyName });
                sReturn = Convert.ToString(objValue);
            }
            return sReturn;
        }
 
        public static String GetFieldNamebyEscapeCharacter(this string sName)
        {
            String sReturn = sName;
            String sChar = "_x00{0}_";
            sReturn = sReturn.Replace("_", String.Format(sChar, "2d"));
            sReturn = sReturn.Replace(" ", String.Format(sChar, "20"));
            sReturn = sReturn.Replace("-", String.Format(sChar, "27"));
            sReturn = sReturn.Replace("#", String.Format(sChar, "23"));
            sReturn = sReturn.Replace("%", String.Format(sChar, "25"));
            sReturn = sReturn.Replace("{", String.Format(sChar, "7B"));
            sReturn = sReturn.Replace("}", String.Format(sChar, "7D"));
            sReturn = sReturn.Replace("|", String.Format(sChar, "7C"));
            sReturn = sReturn.Replace("\\", String.Format(sChar, "5C"));
            sReturn = sReturn.Replace("^", String.Format(sChar, "5E"));
            sReturn = sReturn.Replace("~", String.Format(sChar, "7E"));
            sReturn = sReturn.Replace("[", String.Format(sChar, "5B"));
            sReturn = sReturn.Replace("]", String.Format(sChar, "5D"));
            sReturn = sReturn.Replace("`", String.Format(sChar, "60"));
            sReturn = sReturn.Replace(";", String.Format(sChar, "3B"));
            sReturn = sReturn.Replace("/", String.Format(sChar, "2F"));
            sReturn = sReturn.Replace("?", String.Format(sChar, "3F"));
            sReturn = sReturn.Replace(":", String.Format(sChar, "3A"));
            sReturn = sReturn.Replace("@", String.Format(sChar, "40"));
            sReturn = sReturn.Replace("=", String.Format(sChar, "3D"));
            sReturn = sReturn.Replace("&", String.Format(sChar, "26"));
            sReturn = sReturn.Replace("$", String.Format(sChar, "24"));
            return sReturn;
        }
 
        #endregion
    }
}
