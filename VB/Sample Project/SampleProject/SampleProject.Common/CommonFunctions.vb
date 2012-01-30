Imports Microsoft.SharePoint
Imports System.Web


Public Class CommonFunctions
#Region "Normal Static Method"

    Public Shared Function GetColumnTypeByFieldGUID(ByVal strfieldGuid As [String]) As [String]
        Dim objSPField As SPField
        Dim objSPWeb As SPWeb = Nothing
        Dim strRetVal As [String] = [String].Empty

        Try
            objSPWeb = SPContext.Current.Web
            objSPField = objSPWeb.Fields(New Guid(strfieldGuid))

            If objSPField IsNot Nothing Then
                strRetVal = objSPField.TypeAsString
            End If
            Return strRetVal
        Catch Ex As Exception
            Return strRetVal
        End Try
    End Function

    Public Shared Function GetSiteUrlByID(ByVal strWebID As [String]) As [String]
        Dim strWebUrl As [String] = [String].Empty
        Try
            Using objSPSite As New SPSite(SPContext.Current.Web.Url)
                Using objCurrentWeb As SPWeb = objSPSite.OpenWeb(New Guid(strWebID))
                    If objCurrentWeb IsNot Nothing Then
                        strWebUrl = objCurrentWeb.Url
                    End If
                End Using
            End Using
            Return strWebUrl
        Catch Ex As Exception
            Return strWebUrl
        End Try
    End Function


    Public Shared Function RemoveSpecialNames(ByVal sFilename As [String]) As String
        sFilename = sFilename.Replace(".files", ".files_")
        sFilename = sFilename.Replace("_files", "_files_")
        sFilename = sFilename.Replace("-Dateien", "-Dateien_")
        sFilename = sFilename.Replace("_fichiers", "_fichiers_")
        sFilename = sFilename.Replace("_bestanden", "_bestanden_")
        sFilename = sFilename.Replace("_file", "_file_")
        sFilename = sFilename.Replace("_archivos", "_archivos_")
        sFilename = sFilename.Replace("-filer", "-filer_")
        sFilename = sFilename.Replace("_tiedostot", "_tiedostot_")
        sFilename = sFilename.Replace("_pliki", "_pliki_")
        sFilename = sFilename.Replace("_soubory", "_soubory_")
        sFilename = sFilename.Replace("_elemei", "_elemei_")
        sFilename = sFilename.Replace("_ficheiros", "_ficheiros_")
        sFilename = sFilename.Replace("_arquivos", "_arquivos_")
        sFilename = sFilename.Replace("_dosyalar", "_dosyalar_")
        sFilename = sFilename.Replace("_datoteke", "_datoteke_")
        sFilename = sFilename.Replace("_fitxers", "_fitxers_")
        sFilename = sFilename.Replace("_failid", "_failid_")
        sFilename = sFilename.Replace("_fails", "_fails_")
        sFilename = sFilename.Replace("_bylos", "_bylos_")
        sFilename = sFilename.Replace("_fajlovi", "_fajlovi_")
        sFilename = sFilename.Replace("_fitxategiak", "_fitxategiak_")
        Return sFilename
    End Function

    Public Shared Function EscapeCAMLChars(ByVal sName As [String]) As [String]
        Dim sReturn As [String] = sName
        sReturn = sReturn.Replace("&", "&amp;")
        sReturn = sReturn.Replace("<", "&lt;")
        sReturn = sReturn.Replace(">", "&gt;")
        Return sReturn
    End Function

    Public Shared Function RevertCAMLChars(ByVal sText As [String]) As [String]
        Dim sReturn As [String] = sText
        sReturn = sReturn.Replace("&amp;", "&")
        sReturn = sReturn.Replace("&lt;", "<")
        sReturn = sReturn.Replace("&gt;", ">")
        Return sReturn
    End Function

    Public Shared Function IsExistsCustomAction(ByVal strCustomAction As [String], ByVal strLocation As [String], ByVal strRegistrationID As [String], ByVal objRootWeb As SPWeb) As [Boolean]
        Dim IsExists As [Boolean] = False
        'objRootWeb = this.CurrentWeb.Site.RootWeb;
        For Each objSPUserCustomAction As SPUserCustomAction In objRootWeb.UserCustomActions
            If objSPUserCustomAction.Location.Equals(strLocation) AndAlso objSPUserCustomAction.Name IsNot Nothing AndAlso objSPUserCustomAction.Name.Equals(strCustomAction) Then
                If Not [String].IsNullOrEmpty(strRegistrationID) Then
                    If objSPUserCustomAction.RegistrationId IsNot Nothing AndAlso objSPUserCustomAction.RegistrationId.Equals(strRegistrationID) Then
                        IsExists = True
                        Exit For
                    End If
                Else
                    IsExists = True
                    Exit For
                End If
            End If
        Next
        Return IsExists
    End Function

    Public Shared Function GetErrorCodeKey(ByVal current As HttpContext, ByVal web As SPWeb) As [String]

        Return (Convert.ToString(current.Request.UserHostAddress) & "_" & Convert.ToString(web.CurrentUser.LoginName) & "_") + Constants.KEY_SUFFIX
    End Function
#End Region
End Class

