Imports Microsoft.SharePoint
Imports System.Web

Public Class ElevatedCodeHelper
    Public Delegate Sub PrivilegedCodeDelegate(ByVal elevatedWeb As SPWeb)

    Public Shared Sub ExecuteElevatedCode(ByVal siteId As Guid, ByVal webId As Guid, ByVal webCode As PrivilegedCodeDelegate)
        Using site As New SPSite(siteId)
            Dim normalWeb As SPWeb = site.OpenWeb(webId)
            Try
                ExecuteElevatedCode(normalWeb, webCode)
            Finally
                ' Release web resources.
                normalWeb.Dispose()
            End Try
        End Using
    End Sub

    Public Shared Sub ExecuteElevatedCode(ByVal normalWeb As SPWeb, ByVal webCode As PrivilegedCodeDelegate, ByVal permissionToCheck As SPBasePermissions)
        If normalWeb.DoesUserHavePermissions(permissionToCheck) Then
            webCode(normalWeb)
        Else
            ExecuteElevatedCode(normalWeb, webCode)
        End If
    End Sub

    Public Shared Sub ExecuteElevatedCode(ByVal normalWeb As SPWeb, ByVal webCode As PrivilegedCodeDelegate)
        If Not normalWeb.UserIsSiteAdmin Then
            SPSecurity.RunWithElevatedPrivileges(Sub()
                                                     Using site As New SPSite(normalWeb.Site.Url)
                                                         site.AllowUnsafeUpdates = True
                                                         Dim elevatedWeb As SPWeb = site.OpenWeb()
                                                         Try
                                                             elevatedWeb.AllowUnsafeUpdates = True
                                                             webCode(elevatedWeb)
                                                         Finally
                                                             elevatedWeb.Dispose()
                                                         End Try
                                                     End Using
                                                 End Sub
                                                 )

        Else
            webCode(normalWeb)
        End If
    End Sub
End Class

