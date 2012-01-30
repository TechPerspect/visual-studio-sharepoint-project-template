Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web
Imports Microsoft.SharePoint
Imports Microsoft.SharePoint.Administration
Imports SampleProject.Common

Public Class CustomExceptions
    Inherits Exception
    Private innerExceptionNew As Exception

    Public Sub New(ByVal exception As Exception, ByVal showException As Enums.ShowException)
        Me.innerExceptionNew = exception
        Select Case showException
            Case Enums.ShowException.EventViewer
                ShowMessageonEventViewer()
                Exit Select
            Case Enums.ShowException.LogFile
                ShowMessageonLogFile()
                Exit Select
            Case Else
                Exit Select
        End Select
    End Sub

    Public Sub New(ByVal exception As Exception, ByVal current As HttpContext, ByVal web As SPWeb)
        Me.innerExceptionNew = exception
        ShowMessageonUI(current, web)
    End Sub

#Region "Private Methods"
    Private Sub ShowMessageonEventViewer()
        System.Diagnostics.EventLog.WriteEntry(Constants.ERROR_KEY, Me.innerExceptionNew.Message & ": " & Me.innerExceptionNew.StackTrace, System.Diagnostics.EventLogEntryType.[Error])
    End Sub

    Private Sub ShowMessageonLogFile()
        SPDiagnosticsService.Local.WriteTrace(0, New SPDiagnosticsCategory(Constants.ERROR_KEY, TraceSeverity.Medium, EventSeverity.[Error]), TraceSeverity.Monitorable, Me.innerExceptionNew.Message, Me.innerExceptionNew.StackTrace)
    End Sub

    Private Sub ShowMessageonUI(ByVal current As HttpContext, ByVal web As SPWeb)
        SetErrorMessageforUI(current, web, Me.innerExceptionNew.Message)
    End Sub
#End Region

#Region "UI Message"

#Region "Public Clear Message"

    Public Shared Sub ClearMessageFromUI(ByVal current As HttpContext, ByVal web As SPWeb)
        If current IsNot Nothing Then
            SetMessageVariable(current, web, [String].Empty)
        End If
    End Sub
#End Region

#Region "Public Set Error Message"
    Public Shared Sub SetErrorMessageforUI(ByVal current As HttpContext, ByVal web As SPWeb, ByVal sErrorMessage As [String])
        If current IsNot Nothing Then
            If Not [String].IsNullOrEmpty(sErrorMessage) Then
                Dim sMessage As [String] = Constants.ERROR_MESSAGE_SYMBOL + sErrorMessage
                SetMessageVariable(current, web, sMessage)
            End If
        End If
    End Sub
#End Region

#Region "Public Set Success Message"
    Public Shared Sub SetSuccessMessageforUI(ByVal current As HttpContext, ByVal web As SPWeb, ByVal sErrorMessage As [String])
        If current IsNot Nothing Then
            If Not [String].IsNullOrEmpty(sErrorMessage) Then
                Dim sMessage As [String] = Constants.SUCCESS_MESSAGE_SYMBOL + sErrorMessage
                SetMessageVariable(current, web, sMessage)
            End If
        End If
    End Sub

#End Region

    Public Shared Sub SetMessageVariable(ByVal current As HttpContext, ByVal web As SPWeb, ByVal sErrorMessage As [String])
        If current IsNot Nothing Then
            current.Cache(CommonFunctions.GetErrorCodeKey(current, web)) = sErrorMessage
        End If
    End Sub
#End Region
End Class
