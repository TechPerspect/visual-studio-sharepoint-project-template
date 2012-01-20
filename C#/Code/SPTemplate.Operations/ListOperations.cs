using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Collections;
using System.IO;
using Microsoft.SharePoint.Utilities;
using System.Xml;
using System.Web;
using System.Net;
using Microsoft.SharePoint.Administration;

namespace $safeprojectname$
{
    internal class ListOperations : BaseOperation
    {
        public ListOperations(SPWeb objSPWeb) : base(objSPWeb) { }

        public Guid CreateList(out Boolean IsExisting, String strListName, String strDescription, ArrayList xmlFieldCollection, SPListTemplateType listType, Boolean requireCheckout)
        {
            SPWeb objSPWeb = this.CurrentWeb;
            SPList objList;
            Guid objListGUID = default(Guid);
            IsExisting = false;
            try
            {
                if (!IsListExisting(strListName))
                {
                    //create list here
                    objListGUID = objSPWeb.Lists.Add(strListName, strDescription, listType);
                    objList = objSPWeb.Lists.GetList(objListGUID, false);
                    if (requireCheckout)
                    {
                        objSPWeb.AllowUnsafeUpdates = true;
                        objList.EnableVersioning = true;
                        objList.EnableMinorVersions = true;
                        objList.ForceCheckout = true;
                        objList.EnableFolderCreation = false;
                        objList.Update();
                    }
                }
                else
                {
                    IsExisting = true;
                    objList = objSPWeb.Lists.TryGetList(strListName);
                    objListGUID = objList.ID;
                }
                foreach (String strfieldXML in xmlFieldCollection)
                {
                    try
                    {
                        objList.Fields.AddFieldAsXml(strfieldXML, true, SPAddFieldOptions.AddFieldCheckDisplayName);
                    }
                    catch { }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return objListGUID;
        }

        public Guid CreateList(out Boolean IsExisting, String strListName, String strDescription, ArrayList xmlFieldCollection, SPListTemplateType listType)
        {
            return CreateList(out IsExisting, strListName,  strDescription, xmlFieldCollection, listType, false);
        }

        public SPField CreateSiteColumn(out Boolean IsExisting, String xmlSiteColXml)
        {
            SPField objSPField = null;
            SPWeb objSPRootWeb = this.CurrentWeb.Site.RootWeb;
            IsExisting = false;
            XmlDocument objFieldXml = null;
            String strFieldStaticName = String.Empty;
            String strColTitle = String.Empty;
            try
            {
                objFieldXml = new XmlDocument();
                objFieldXml.LoadXml(xmlSiteColXml);
                if (objFieldXml != null && objFieldXml.HasChildNodes)
                {
                    strFieldStaticName = objFieldXml.ChildNodes[0].Attributes["StaticName"].Value;
                    if (!String.IsNullOrEmpty(strFieldStaticName))
                    {
                        objSPField = objSPRootWeb.Fields.TryGetFieldByStaticName(strFieldStaticName);
                        if (objSPField == null)
                        {
                            strFieldStaticName = objSPRootWeb.Fields.AddFieldAsXml(xmlSiteColXml, true, SPAddFieldOptions.AddFieldCheckDisplayName);
                            if (!String.IsNullOrEmpty(strFieldStaticName))
                            {
                                objSPField = objSPRootWeb.Fields.GetFieldByInternalName(strFieldStaticName);
                            }
                        }
                        else
                        {
                            IsExisting = true;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return objSPField;
        }

        public Boolean IsListExisting(String listName)
        {
            Boolean listExists = false;
            SPList objSPList = null;
            SPWeb objSPWeb;
            objSPWeb = this.CurrentWeb;

            objSPList = objSPWeb.Lists.TryGetList(listName);
            if (objSPList != null)
            {
                listExists = true;
            }
            return listExists;
        }

        
        public void GetListItem(String strListName, String strQueryXml, out SPListItem objSPListItem, SPWeb objSPWeb)
        {
            SPListItemCollection objSPListItemCollection = null;

            try
            {
                objSPListItem = null;
                objSPListItemCollection = GetListItems(strListName, strQueryXml, objSPWeb);
                if (objSPListItemCollection != null && objSPListItemCollection.Count > 0)
                {
                    objSPListItem = objSPListItemCollection[0];
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public SPListItemCollection GetListItems(String strListName, String strQueryXml)
        {
            SPWeb objSPWeb = this.CurrentWeb;
            return GetListItems(strListName, strQueryXml, objSPWeb);
        }

        public SPListItemCollection GetListItems(String strListName, String strQueryXml, SPWeb objSPWeb)
        {
            SPList objSPList = null;
            SPQuery objSPQuery;
            SPListItemCollection objSPListItemCollection = null;

            try
            {

                objSPList = objSPWeb.Lists.TryGetList(strListName);

                if (objSPList != null)
                {
                    if (!String.IsNullOrEmpty(strQueryXml))
                    {
                        objSPQuery = new SPQuery();
                        objSPQuery.ViewAttributes = "Scope='Recursive'";
                        objSPQuery.Query = strQueryXml;
                        objSPListItemCollection = objSPList.GetItems(objSPQuery);
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            return objSPListItemCollection;
        }

        public SPListItem GetListItemByID(String strListName, Int32 ID)
        {
            SPWeb objSPWeb = this.CurrentWeb.Site.RootWeb;
            return GetListItemByID(strListName, ID, objSPWeb);
        }

        public SPListItem GetListItemByListID(String ListID, Int32 ItemID)
        {
            String strListName = String.Empty;
            SPWeb objSPWeb = this.CurrentWeb;
            strListName = GetListNameByListID(ListID);
            return GetListItemByID(strListName, ItemID, objSPWeb);
        }

        public SPListItem GetListItemByListID(String ListID, Int32 ItemID, SPWeb objSPWeb)
        {
            String strListName = String.Empty;
            strListName = GetListNameByListID(ListID);
            return GetListItemByID(strListName, ItemID, objSPWeb);
        }

      
        public String GetListNameByListID(String ListID)
        {
            String strListName = String.Empty;
            SPWeb objSPWeb = this.CurrentWeb;
            SPList objSPList = null;
            try
            {
                objSPList = objSPWeb.Lists.GetList(new Guid(ListID), false, false);
                if (objSPList != null)
                {
                    strListName = objSPList.Title;
                }
            }
            catch (Exception Ex)
            {
                return String.Empty;
            }
            return strListName;
        }

        public SPListItem GetListItemByID(String strListName, Int32 ID, SPWeb objSPWeb)
        {
            SPList objSPList = null;
            SPQuery objSPQuery;
            SPListItemCollection objSPListItemCollection = null;
            SPListItem objSPListItem = null;
            String strQueryXml = String.Empty;
            try
            {
                objSPList = objSPWeb.Lists.TryGetList(strListName);
                if (objSPList != null)
                {
                    strQueryXml = @"<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + ID + "</Value></Eq></Where>";
                    objSPQuery = new SPQuery();
                    objSPQuery.ViewAttributes = "Scope='Recursive'";
                    objSPQuery.Query = strQueryXml;
                    objSPListItemCollection = objSPList.GetItems(objSPQuery);
                    if (objSPListItemCollection.Count > 0)
                    {
                        objSPListItem = objSPListItemCollection[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return objSPListItem;
        }

        public void AddItem(string strListName, out SPListItem objSPListItem)
        {
            SPList objSPList = null;
            SPWeb objSPWeb = null;
            objSPListItem = null;
            try
            {
                objSPWeb = this.CurrentWeb;
                objSPList = objSPWeb.Lists.TryGetList(strListName);
                if (objSPList != null)
                {
                    objSPListItem = objSPList.Items.Add();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public SPField GetSPFieldByName(String strListName, String strfieldName)
        {
            SPField objSPField = null;
            SPList objSPList = null;
            try
            {
                objSPList = this.CurrentWeb.Lists[strListName];
                if (objSPList != null)
                {
                    objSPField = objSPList.Fields.GetField(strfieldName);
                }
            }
            catch (Exception Ex)
            {
                return null;
            }
            return objSPField;
        }

        public SPField GetSPFieldByName(SPFieldCollection fieldColl, String strfieldName)
        {
            SPField objSPField = null;

            try
            {
                if (fieldColl != null)
                {
                    objSPField = fieldColl.GetField(strfieldName);
                }
            }
            catch (Exception Ex)
            {
                return null;
            }

            return objSPField;
        }

        public void DeleteList(String strListName)
        {
            SPList objSPList = null;
            SPWeb objSPWeb = null;
            try
            {
                objSPWeb = this.CurrentWeb;
                if (IsListExisting(strListName))
                {
                    objSPWeb.AllowUnsafeUpdates = true;
                    objSPList = objSPWeb.Lists.TryGetList(strListName);
                    objSPList.Delete();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public Boolean DeleteItemByID(String strListName, Int32 ItemId)
        {
            SPWeb objSPWeb = this.CurrentWeb;
            return DeleteItemByID(strListName, ItemId, objSPWeb);
        }

        public Boolean DeleteItemByTitle(String strListName, String strTitle)
        {
            String strqueryXml = String.Empty;
            Boolean IsSucess = false;
            SPListItem objSPListItem = null;
            try
            {
                strqueryXml = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + strTitle + "</Value></Eq></Where>";
                GetListItem(strListName, strqueryXml, out objSPListItem, this.CurrentWeb);
                if (objSPListItem != null)
                {
                    objSPListItem.Delete();
                }
            }
            catch (Exception Ex)
            {
            }
            return IsSucess;
        }

        public Boolean DeleteItemByID(String strListName, Int32 ItemId, SPWeb objSPWeb)
        {
            Boolean isSucess = false;
            SPListItem objSPListItem = null;
            try
            {
                objSPListItem = GetListItemByID(strListName, ItemId);
                if (objSPListItem != null)
                {
                    objSPListItem.Recycle();
                    isSucess = true;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return isSucess;
        }

     
        public bool IsPageExists(String strPath)
        {
            Boolean IsExists = false;
            SPWeb objSPWeb;
            try
            {
                objSPWeb = this.CurrentWeb;
                if (objSPWeb.GetFile(strPath).Exists)
                {
                    IsExists = true;
                }
            }
            catch (Exception Ex)
            {

            }
            return IsExists;
        }

        public bool IsListItemExists(string strListName, string strFileNameWithoutextension)
        {
            SPWeb web = this.CurrentWeb.Site.RootWeb;
            SPList objSPList = null;
            SPListItemCollection objSPListItemCollection = null;
            Boolean boolReturn = false;
            try
            {
                objSPList = web.Lists[strListName];
                objSPListItemCollection = objSPList.Items;
                foreach (SPListItem item in objSPListItemCollection)
                {
                    if (item.DisplayName.ToLower().Equals(strFileNameWithoutextension.ToLower()))
                    {
                        boolReturn = true;
                        break;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return boolReturn;
        }

        public int GetNewListItemID(string strListName, string strFileNameWithoutextension)
        {
            SPWeb web = this.CurrentWeb.Site.RootWeb;
            SPList objSPList = null;
            SPListItemCollection objSPListItemCollection = null;
            int intListItemID = 0;
            try
            {
                objSPList = web.Lists[strListName];
                objSPListItemCollection = objSPList.Items;
                foreach (SPListItem item in objSPListItemCollection)
                {
                    if (item.DisplayName.Equals(strFileNameWithoutextension))
                    {
                        intListItemID = item.ID;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intListItemID;
        }

        public String GetFileNameById(Int32 ItemID, String strListName)
        {
            SPWeb objSPRootWeb = this.CurrentWeb.Site.RootWeb;
            SPListItem objSPListItem = null;
            String strFileName = String.Empty;
            try
            {
                objSPListItem = GetListItemByID(strListName, ItemID, objSPRootWeb);
                if (objSPListItem != null)
                {
                    strFileName = objSPListItem.File.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strFileName;
        }

        public Boolean DeleteItemPermanentlyByID(String strListName, Int32 ItemId)
        {
            Boolean isSucess = false;
            SPListItem objSPListItem = null;
            try
            {
                objSPListItem = GetListItemByID(strListName, ItemId);
                if (objSPListItem != null)
                {
                    objSPListItem.Delete();
                    isSucess = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSucess;
        }

        public SPList GetListByListID(String ListID)
        {
            SPWeb objSPWeb = this.CurrentWeb;
            SPList objSPList = null;
            try
            {
                objSPList = objSPWeb.Lists.GetList(new Guid(ListID), true);

            }
            catch (Exception Ex)
            {
                return null;
            }
            return objSPList;
        }

        public SPFieldCollection GetListFieldsByListName(String listName)
        {
            SPWeb objSPWeb = this.CurrentWeb;
            SPFieldCollection fieldColl = null;
            try
            {
                SPList list = objSPWeb.Lists.TryGetList(listName);
                if (list != null)
                {
                    fieldColl = list.Fields;
                }
            }
            catch (Exception Ex)
            {
                return null;
            }
            return fieldColl;
        }

        public SPFieldCollection GetContentTypeFieldsByContentTypeID(String contentTypeID)
        {
            SPWeb objSPWeb = this.CurrentWeb;
            SPFieldCollection fieldColl = null;

            try
            {
                SPContentTypeId cTypeID = new SPContentTypeId(contentTypeID);

                SPContentType contentType = objSPWeb.AvailableContentTypes[cTypeID];

                if (contentType != null)
                {
                    fieldColl = contentType.Fields;
                }
            }
            catch (Exception Ex)
            {
                return null;
            }
            return fieldColl;
        }


      
        public SPList GetListByName(String listName)
        {
            if (!IsListExisting(listName)) return null;

            SPWeb objSPWeb;
            objSPWeb = this.CurrentWeb;

            return objSPWeb.Lists.TryGetList(listName);
        }

        public String GetListIDByListName(String listName)
        {
            String listID = string.Empty;
            SPList objSPList = null;
            SPWeb objSPWeb;
            objSPWeb = this.CurrentWeb;

            objSPList = objSPWeb.Lists.TryGetList(listName);
            if (objSPList != null)
            {
                listID = objSPList.ID.ToString();
            }
            return listID;
        }

        public String GetListURL(String ListName, bool serverRelative = false)
        {
            SPList list = GetListByName(ListName);
            if (null == list) return String.Empty;

            var listUrl = list.DefaultViewUrl;
            if (list is SPDocumentLibrary)
            {
                var idx = listUrl.IndexOf("Forms");
                listUrl = listUrl.Remove(idx);
                if (listUrl.EndsWith("/"))
                    listUrl = listUrl.Remove(listUrl.LastIndexOf("/"));
            }
            else
            {
                var idx = listUrl.LastIndexOf("/");
                listUrl = listUrl.Remove(idx);
            }
            if (!serverRelative)
            {
                var serverUrl = list.ParentWeb.Url;
                serverUrl = serverUrl.Substring(0, serverUrl.IndexOf("/", 7));
                listUrl = serverUrl + listUrl;
            }

            return listUrl;

        }

   
      
        public void PerformMinorCheckin(String strListName, Int32 ItemID, String strMessage)
        {
            SPListItem objListItem = null;
            ListOperations objListOperations = new ListOperations(this.CurrentWeb);
            try
            {
                objListItem = objListOperations.GetListItemByID(strListName, ItemID);
                if (objListItem.ParentList.EnableVersioning)
                {
                    if (objListItem.File.CheckOutType == SPFile.SPCheckOutType.Online)
                    {
                        objListItem.File.CheckIn(strMessage, SPCheckinType.MinorCheckIn);
                    }
                }
            }
            catch { }
        }
    }
}
