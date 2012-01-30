Imports Microsoft.SharePoint
Imports System.Xml

Public Class ListOperations
    Inherits BaseOperation
    Public Sub New(ByVal objSPWeb As SPWeb)
        MyBase.New(objSPWeb)
    End Sub

    Public Sub AddItem(ByVal strListName As String, ByRef objSPListItem As SPListItem)
        Dim objSPList As SPList = Nothing
        Dim objSPWeb As SPWeb = Nothing
        objSPListItem = Nothing
        Try
            objSPWeb = Me.CurrentWeb
            objSPList = objSPWeb.Lists.TryGetList(strListName)
            If objSPList IsNot Nothing Then
                objSPListItem = objSPList.Items.Add()
            End If
        Catch Ex As Exception
            Throw Ex
        End Try
    End Sub

    Public Function CreateList(ByRef IsExisting As [Boolean], ByVal strListName As [String], ByVal strDescription As [String], ByVal xmlFieldCollection As ArrayList, ByVal listType As SPListTemplateType, ByVal requireCheckout As [Boolean]) As Guid
        Dim objSPWeb As SPWeb = Me.CurrentWeb
        Dim objList As SPList
        Dim objListGUID As Guid = Nothing
        IsExisting = False
        Try
            If Not IsListExisting(strListName) Then
                'create list here
                objListGUID = objSPWeb.Lists.Add(strListName, strDescription, listType)
                objList = objSPWeb.Lists.GetList(objListGUID, False)
                If requireCheckout Then
                    objSPWeb.AllowUnsafeUpdates = True
                    objList.EnableVersioning = True
                    objList.EnableMinorVersions = True
                    objList.ForceCheckout = True
                    objList.EnableFolderCreation = False
                    objList.Update()
                End If
            Else
                IsExisting = True
                objList = objSPWeb.Lists.TryGetList(strListName)
                objListGUID = objList.ID
            End If
            For Each strfieldXML As [String] In xmlFieldCollection
                Try
                    objList.Fields.AddFieldAsXml(strfieldXML, True, SPAddFieldOptions.AddFieldCheckDisplayName)
                Catch
                End Try
            Next
        Catch Ex As Exception
            Throw Ex
        End Try
        Return objListGUID
    End Function

    Public Function CreateList(ByRef IsExisting As [Boolean], ByVal strListName As [String], ByVal strDescription As [String], ByVal xmlFieldCollection As ArrayList, ByVal listType As SPListTemplateType) As Guid
        Return CreateList(IsExisting, strListName, strDescription, xmlFieldCollection, listType, False)
    End Function

    Public Function CreateSiteColumn(ByRef IsExisting As [Boolean], ByVal xmlSiteColXml As [String]) As SPField
        Dim objSPField As SPField = Nothing
        Dim objSPRootWeb As SPWeb = Me.CurrentWeb.Site.RootWeb
        IsExisting = False
        Dim objFieldXml As XmlDocument = Nothing
        Dim strFieldStaticName As [String] = [String].Empty
        Dim strColTitle As [String] = [String].Empty
        Try
            objFieldXml = New XmlDocument()
            objFieldXml.LoadXml(xmlSiteColXml)
            If objFieldXml IsNot Nothing AndAlso objFieldXml.HasChildNodes Then
                strFieldStaticName = objFieldXml.ChildNodes(0).Attributes("StaticName").Value
                If Not [String].IsNullOrEmpty(strFieldStaticName) Then
                    objSPField = objSPRootWeb.Fields.TryGetFieldByStaticName(strFieldStaticName)
                    If objSPField Is Nothing Then
                        strFieldStaticName = objSPRootWeb.Fields.AddFieldAsXml(xmlSiteColXml, True, SPAddFieldOptions.AddFieldCheckDisplayName)
                        If Not [String].IsNullOrEmpty(strFieldStaticName) Then
                            objSPField = objSPRootWeb.Fields.GetFieldByInternalName(strFieldStaticName)
                        End If
                    Else
                        IsExisting = True
                    End If
                End If
            End If
        Catch Ex As Exception
            Throw Ex
        End Try
        Return objSPField
    End Function

    Public Function DeleteItemByID(ByVal strListName As [String], ByVal ItemId As Int32, ByVal objSPWeb As SPWeb) As [Boolean]
        Dim isSucess As [Boolean] = False
        Dim objSPListItem As SPListItem = Nothing
        Try
            objSPListItem = GetListItemByID(strListName, ItemId)
            If objSPListItem IsNot Nothing Then
                objSPListItem.Recycle()
                isSucess = True
            End If
        Catch Ex As Exception
            Throw Ex
        End Try
        Return isSucess
    End Function

    Public Function DeleteItemByID(ByVal strListName As [String], ByVal ItemId As Int32) As [Boolean]
        Dim objSPWeb As SPWeb = Me.CurrentWeb
        Return DeleteItemByID(strListName, ItemId, objSPWeb)
    End Function

    Public Function DeleteItemByTitle(ByVal strListName As [String], ByVal strTitle As [String]) As [Boolean]
        Dim strqueryXml As [String] = [String].Empty
        Dim IsSucess As [Boolean] = False
        Dim objSPListItem As SPListItem = Nothing
        Try
            strqueryXml = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" & Convert.ToString(strTitle) & "</Value></Eq></Where>"
            GetListItem(strListName, strqueryXml, objSPListItem, Me.CurrentWeb)
            If objSPListItem IsNot Nothing Then
                objSPListItem.Delete()
            End If
        Catch Ex As Exception
        End Try
        Return IsSucess
    End Function

    Public Function DeleteItemPermanentlyByID(ByVal strListName As [String], ByVal ItemId As Int32) As [Boolean]
        Dim isSucess As [Boolean] = False
        Dim objSPListItem As SPListItem = Nothing
        Try
            objSPListItem = GetListItemByID(strListName, ItemId)
            If objSPListItem IsNot Nothing Then
                objSPListItem.Delete()
                isSucess = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return isSucess
    End Function

    Public Sub DeleteList(ByVal strListName As [String])
        Dim objSPList As SPList = Nothing
        Dim objSPWeb As SPWeb = Nothing
        Try
            objSPWeb = Me.CurrentWeb
            If IsListExisting(strListName) Then
                objSPWeb.AllowUnsafeUpdates = True
                objSPList = objSPWeb.Lists.TryGetList(strListName)
                objSPList.Delete()
            End If
        Catch Ex As Exception
            Throw Ex
        End Try
    End Sub

    Public Function GetContentTypeFieldsByContentTypeID(ByVal contentTypeID As [String]) As SPFieldCollection
        Dim objSPWeb As SPWeb = Me.CurrentWeb
        Dim fieldColl As SPFieldCollection = Nothing

        Try
            Dim cTypeID As New SPContentTypeId(contentTypeID)

            Dim contentType As SPContentType = objSPWeb.AvailableContentTypes(cTypeID)

            If contentType IsNot Nothing Then
                fieldColl = contentType.Fields
            End If
        Catch Ex As Exception
            Return Nothing
        End Try
        Return fieldColl
    End Function

    Public Function GetFileNameById(ByVal ItemID As Int32, ByVal strListName As [String]) As [String]
        Dim objSPRootWeb As SPWeb = Me.CurrentWeb.Site.RootWeb
        Dim objSPListItem As SPListItem = Nothing
        Dim strFileName As [String] = [String].Empty
        Try
            objSPListItem = GetListItemByID(strListName, ItemID, objSPRootWeb)
            If objSPListItem IsNot Nothing Then
                strFileName = objSPListItem.File.Name
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strFileName
    End Function

    Public Function GetListByListID(ByVal ListID As [String]) As SPList
        Dim objSPWeb As SPWeb = Me.CurrentWeb
        Dim objSPList As SPList = Nothing
        Try

            objSPList = objSPWeb.Lists.GetList(New Guid(ListID), True)
        Catch Ex As Exception
            Return Nothing
        End Try
        Return objSPList
    End Function



    Public Function GetListByName(ByVal listName As [String]) As SPList
        If Not IsListExisting(listName) Then
            Return Nothing
        End If

        Dim objSPWeb As SPWeb
        objSPWeb = Me.CurrentWeb

        Return objSPWeb.Lists.TryGetList(listName)
    End Function

    Public Function GetListFieldsByListName(ByVal listName As [String]) As SPFieldCollection
        Dim objSPWeb As SPWeb = Me.CurrentWeb
        Dim fieldColl As SPFieldCollection = Nothing
        Try
            Dim list As SPList = objSPWeb.Lists.TryGetList(listName)
            If list IsNot Nothing Then
                fieldColl = list.Fields
            End If
        Catch Ex As Exception
            Return Nothing
        End Try
        Return fieldColl
    End Function

    Public Function GetListIDByListName(ByVal listName As [String]) As [String]
        Dim listID As [String] = String.Empty
        Dim objSPList As SPList = Nothing
        Dim objSPWeb As SPWeb
        objSPWeb = Me.CurrentWeb

        objSPList = objSPWeb.Lists.TryGetList(listName)
        If objSPList IsNot Nothing Then
            listID = objSPList.ID.ToString()
        End If
        Return listID
    End Function


    Public Sub GetListItem(ByVal strListName As [String], ByVal strQueryXml As [String], ByRef objSPListItem As SPListItem, ByVal objSPWeb As SPWeb)
        Dim objSPListItemCollection As SPListItemCollection = Nothing

        Try
            objSPListItem = Nothing
            objSPListItemCollection = GetListItems(strListName, strQueryXml, objSPWeb)
            If objSPListItemCollection IsNot Nothing AndAlso objSPListItemCollection.Count > 0 Then
                objSPListItem = objSPListItemCollection(0)
            End If
        Catch Ex As Exception
            Throw Ex
        End Try
    End Sub

    Public Function GetListItemByID(ByVal strListName As [String], ByVal ID As Int32) As SPListItem
        Dim objSPWeb As SPWeb = Me.CurrentWeb.Site.RootWeb
        Return GetListItemByID(strListName, ID, objSPWeb)
    End Function

    Public Function GetListItemByID(ByVal strListName As [String], ByVal ID As Int32, ByVal objSPWeb As SPWeb) As SPListItem
        Dim objSPList As SPList = Nothing
        Dim objSPQuery As SPQuery
        Dim objSPListItemCollection As SPListItemCollection = Nothing
        Dim objSPListItem As SPListItem = Nothing
        Dim strQueryXml As [String] = [String].Empty
        Try
            objSPList = objSPWeb.Lists.TryGetList(strListName)
            If objSPList IsNot Nothing Then
                strQueryXml = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" & Convert.ToString(ID) & "</Value></Eq></Where>"
                objSPQuery = New SPQuery()
                objSPQuery.ViewAttributes = "Scope='Recursive'"
                objSPQuery.Query = strQueryXml
                objSPListItemCollection = objSPList.GetItems(objSPQuery)
                If objSPListItemCollection.Count > 0 Then
                    objSPListItem = objSPListItemCollection(0)
                End If
            End If
        Catch Ex As Exception
            Throw Ex
        End Try
        Return objSPListItem
    End Function

    Public Function GetListItemByListID(ByVal ListID As [String], ByVal ItemID As Int32, ByVal objSPWeb As SPWeb) As SPListItem
        Dim strListName As [String] = [String].Empty
        strListName = GetListNameByListID(ListID)
        Return GetListItemByID(strListName, ItemID, objSPWeb)
    End Function

    Public Function GetListItemByListID(ByVal ListID As [String], ByVal ItemID As Int32) As SPListItem
        Dim strListName As [String] = [String].Empty
        Dim objSPWeb As SPWeb = Me.CurrentWeb
        strListName = GetListNameByListID(ListID)
        Return GetListItemByID(strListName, ItemID, objSPWeb)
    End Function

    Public Function GetListItems(ByVal strListName As [String], ByVal strQueryXml As [String], ByVal objSPWeb As SPWeb) As SPListItemCollection
        Dim objSPList As SPList = Nothing
        Dim objSPQuery As SPQuery
        Dim objSPListItemCollection As SPListItemCollection = Nothing

        Try

            objSPList = objSPWeb.Lists.TryGetList(strListName)

            If objSPList IsNot Nothing Then
                If Not [String].IsNullOrEmpty(strQueryXml) Then
                    objSPQuery = New SPQuery()
                    objSPQuery.ViewAttributes = "Scope='Recursive'"
                    objSPQuery.Query = strQueryXml
                    objSPListItemCollection = objSPList.GetItems(objSPQuery)
                End If
            End If

        Catch Ex As Exception
        End Try
        Return objSPListItemCollection
    End Function

    Public Function GetListItems(ByVal strListName As [String], ByVal strQueryXml As [String]) As SPListItemCollection
        Dim objSPWeb As SPWeb = Me.CurrentWeb
        Return GetListItems(strListName, strQueryXml, objSPWeb)
    End Function


    Public Function GetListNameByListID(ByVal ListID As [String]) As [String]
        Dim strListName As [String] = [String].Empty
        Dim objSPWeb As SPWeb = Me.CurrentWeb
        Dim objSPList As SPList = Nothing
        Try
            objSPList = objSPWeb.Lists.GetList(New Guid(ListID), False, False)
            If objSPList IsNot Nothing Then
                strListName = objSPList.Title
            End If
        Catch Ex As Exception
            Return [String].Empty
        End Try
        Return strListName
    End Function

    Public Function GetListURL(ByVal ListName As [String], Optional ByVal serverRelative As Boolean = False) As [String]
        Dim list As SPList = GetListByName(ListName)
        If list Is Nothing Then
            Return [String].Empty
        End If

        Dim listUrl = list.DefaultViewUrl
        If TypeOf list Is SPDocumentLibrary Then
            Dim idx = listUrl.IndexOf("Forms")
            listUrl = listUrl.Remove(idx)
            If listUrl.EndsWith("/") Then
                listUrl = listUrl.Remove(listUrl.LastIndexOf("/"))
            End If
        Else
            Dim idx = listUrl.LastIndexOf("/")
            listUrl = listUrl.Remove(idx)
        End If
        If Not serverRelative Then
            Dim serverUrl = list.ParentWeb.Url
            serverUrl = serverUrl.Substring(0, serverUrl.IndexOf("/", 7))
            listUrl = serverUrl + listUrl
        End If

        Return listUrl

    End Function

    Public Function GetNewListItemID(ByVal strListName As String, ByVal strFileNameWithoutextension As String) As Integer
        Dim web As SPWeb = Me.CurrentWeb.Site.RootWeb
        Dim objSPList As SPList = Nothing
        Dim objSPListItemCollection As SPListItemCollection = Nothing
        Dim intListItemID As Integer = 0
        Try
            objSPList = web.Lists(strListName)
            objSPListItemCollection = objSPList.Items
            For Each item As SPListItem In objSPListItemCollection
                If item.DisplayName.Equals(strFileNameWithoutextension) Then
                    intListItemID = item.ID
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
        Return intListItemID
    End Function

    Public Function GetSPFieldByName(ByVal strListName As [String], ByVal strfieldName As [String]) As SPField
        Dim objSPField As SPField = Nothing
        Dim objSPList As SPList = Nothing
        Try
            objSPList = Me.CurrentWeb.Lists(strListName)
            If objSPList IsNot Nothing Then
                objSPField = objSPList.Fields.GetField(strfieldName)
            End If
        Catch Ex As Exception
            Return Nothing
        End Try
        Return objSPField
    End Function

    Public Function GetSPFieldByName(ByVal fieldColl As SPFieldCollection, ByVal strfieldName As [String]) As SPField
        Dim objSPField As SPField = Nothing

        Try
            If fieldColl IsNot Nothing Then
                objSPField = fieldColl.GetField(strfieldName)
            End If
        Catch Ex As Exception
            Return Nothing
        End Try

        Return objSPField
    End Function

    Public Function IsListExisting(ByVal listName As [String]) As [Boolean]
        Dim listExists As [Boolean] = False
        Dim objSPList As SPList = Nothing
        Dim objSPWeb As SPWeb
        objSPWeb = Me.CurrentWeb

        objSPList = objSPWeb.Lists.TryGetList(listName)
        If objSPList IsNot Nothing Then
            listExists = True
        End If
        Return listExists
    End Function

    Public Function IsListItemExists(ByVal strListName As String, ByVal strFileNameWithoutextension As String) As Boolean
        Dim web As SPWeb = Me.CurrentWeb.Site.RootWeb
        Dim objSPList As SPList = Nothing
        Dim objSPListItemCollection As SPListItemCollection = Nothing
        Dim boolReturn As [Boolean] = False
        Try
            objSPList = web.Lists(strListName)
            objSPListItemCollection = objSPList.Items
            For Each item As SPListItem In objSPListItemCollection
                If item.DisplayName.ToLower().Equals(strFileNameWithoutextension.ToLower()) Then
                    boolReturn = True
                    Exit For
                End If
            Next
        Catch Ex As Exception
            Throw Ex
        End Try
        Return boolReturn
    End Function


    Public Function IsPageExists(ByVal strPath As [String]) As Boolean
        Dim IsExists As [Boolean] = False
        Dim objSPWeb As SPWeb
        Try
            objSPWeb = Me.CurrentWeb
            If objSPWeb.GetFile(strPath).Exists Then
                IsExists = True
            End If

        Catch Ex As Exception
        End Try
        Return IsExists
    End Function



    Public Sub PerformMinorCheckin(ByVal strListName As [String], ByVal ItemID As Int32, ByVal strMessage As [String])
        Dim objListItem As SPListItem = Nothing
        Dim objListOperations As New ListOperations(Me.CurrentWeb)
        Try
            objListItem = objListOperations.GetListItemByID(strListName, ItemID)
            If objListItem.ParentList.EnableVersioning Then
                If objListItem.File.CheckOutType = SPFile.SPCheckOutType.Online Then
                    objListItem.File.CheckIn(strMessage, SPCheckinType.MinorCheckIn)
                End If
            End If
        Catch
        End Try
    End Sub
End Class