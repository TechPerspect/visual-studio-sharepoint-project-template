Imports Microsoft.SharePoint.Administration
Imports Microsoft.SharePoint
Imports $customUsing$.Exceptions
Imports $customUsing$.Common

Public Class SampleTimerJob
    Inherits SPJobDefinition
#Region "-- Constructors --"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal jobName As String, ByVal service As SPService, ByVal server As SPServer)
        MyBase.New(jobName, service, server, SPJobLockType.Job)
    End Sub

    Public Sub New(ByVal jobName As String, ByVal webApplication As SPWebApplication)
        MyBase.New(jobName, webApplication, Nothing, SPJobLockType.Job)
        Me.Title = jobName
    End Sub
#End Region

#Region "Public Method"
    Public Overrides Sub Execute(ByVal contentDbId As Guid)
        Try
            If Me.WebApplication IsNot Nothing Then
                If Me.WebApplication.Sites.Count > 0 Then
                    For Each objSPSite As SPSite In Me.WebApplication.Sites
                        ExecuteMethod(objSPSite.RootWeb)
                    Next
                End If
            End If
        Catch Ex As Exception
            Dim custom As CustomExceptions
            custom = New CustomExceptions(Ex, Enums.ShowException.EventViewer)
        End Try
    End Sub
#End Region

#Region "Private Methods"
    Private Sub ExecuteMethod(ByVal objWeb As SPWeb)
        'do your processing
    End Sub
#End Region
End Class
