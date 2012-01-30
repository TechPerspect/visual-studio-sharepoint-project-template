Imports System.Linq
Imports System.Text
Imports System.Web.UI.WebControls
Imports System.Collections
Imports Microsoft.SharePoint
Imports Microsoft.SharePoint.WebControls
Imports System.Web.UI
Imports System.Web
Imports $customUsing$.Common
Imports $customUsing$.Exceptions

Namespace WebControls
    Public Class ErrorCaptureControl
        Inherits WebControl


#Region "Render Message"
        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
        End Sub

        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            Dim sMessage As [String] = Convert.ToString(HttpContext.Current.Cache(CommonFunctions.GetErrorCodeKey(HttpContext.Current, SPContext.Current.Web)))
            If Not [String].IsNullOrEmpty(sMessage) Then
                DisplayMessage(sMessage)
                ClearMessageFromUI()

                If sMessage.Contains(Constants.UNIQUE_KEY_CODE_SUCESS) Then
                    'unique will determine that the message needs to be displayed on second post back; case with long operations
                    sMessage = sMessage.Replace(Constants.UNIQUE_KEY_CODE_SUCESS, [String].Empty)
                    SetSuccessMessageforUI(sMessage)
                ElseIf sMessage.Contains(Constants.UNIQUE_KEY_CODE_FAILURE) Then
                    sMessage = sMessage.Replace(Constants.UNIQUE_KEY_CODE_FAILURE, [String].Empty)
                    SetErrorMessageforUI(sMessage)
                End If
            End If
        End Sub
#End Region

#Region "Set Message in UI"
        Public Shared Sub ClearMessageFromUI()
            If HttpContext.Current IsNot Nothing Then
                CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, [String].Empty)
            End If
        End Sub

        Public Shared Sub ClearMessageFromUI(ByVal web As SPWeb)
            If HttpContext.Current IsNot Nothing Then
                CustomExceptions.SetMessageVariable(HttpContext.Current, web, [String].Empty)
            End If
        End Sub

        Public Shared Sub SetErrorMessageforUI(ByVal sErrorMessage As [String])
            If HttpContext.Current IsNot Nothing Then
                If Not [String].IsNullOrEmpty(sErrorMessage) Then
                    Dim sMessage As [String] = Constants.ERROR_MESSAGE_SYMBOL + sErrorMessage
                    CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, sMessage)
                End If
            End If
        End Sub

        Public Shared Sub SetErrorMessageforUI(ByVal web As SPWeb, ByVal sErrorMessage As [String])
            If HttpContext.Current IsNot Nothing Then
                If Not [String].IsNullOrEmpty(sErrorMessage) Then
                    Dim sMessage As [String] = Constants.ERROR_MESSAGE_SYMBOL + sErrorMessage
                    CustomExceptions.SetMessageVariable(HttpContext.Current, web, sMessage)
                End If
            End If
        End Sub

        Public Shared Sub SetSuccessMessageforUI(ByVal sErrorMessage As [String])
            If HttpContext.Current IsNot Nothing Then
                If Not [String].IsNullOrEmpty(sErrorMessage) Then
                    Dim sMessage As [String] = Constants.SUCCESS_MESSAGE_SYMBOL + sErrorMessage
                    CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, sMessage)
                End If
            End If
        End Sub

        Public Shared Sub SetSuccessMessageforUI(ByVal web As SPWeb, ByVal sErrorMessage As [String])
            If HttpContext.Current IsNot Nothing Then
                If Not [String].IsNullOrEmpty(sErrorMessage) Then
                    Dim sMessage As [String] = Constants.SUCCESS_MESSAGE_SYMBOL + sErrorMessage
                    CustomExceptions.SetMessageVariable(HttpContext.Current, web, sMessage)
                End If
            End If
        End Sub

        Public Shared Sub SetSuccessMessageforLongOperations(ByVal sMessage As [String])
            If HttpContext.Current IsNot Nothing Then
                CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, sMessage + Constants.UNIQUE_KEY_CODE_SUCESS)
            End If
        End Sub

        Public Shared Sub SetErrorMessageforLongOperations(ByVal sMessage As [String])
            If HttpContext.Current IsNot Nothing Then
                CustomExceptions.SetMessageVariable(HttpContext.Current, SPContext.Current.Web, sMessage + Constants.UNIQUE_KEY_CODE_FAILURE)
            End If
        End Sub
#End Region

#Region "Private Methods"


        Private Sub DisplayMessage(ByVal strMessage As [String])
            If Not [String].IsNullOrEmpty(strMessage) AndAlso strMessage.Length > 1 Then
                Dim sSymbol As [String] = strMessage.Substring(0, 1)
                strMessage = strMessage.Substring(1)
                strMessage = strMessage.Replace("'", "&#39;")

                Select Case sSymbol
                    Case Constants.ERROR_MESSAGE_SYMBOL
                        ShowFailureMessage(strMessage)
                        Exit Select
                    Case Constants.SUCCESS_MESSAGE_SYMBOL
                        ShowSuccessMessage(strMessage)
                        Exit Select
                End Select
            End If
        End Sub


        Private Sub ShowFailureMessage(ByVal strMessage As [String])
            Dim strFormatString As [String] = [String].Empty
            Try
                strMessage = strMessage.Replace("\r", "\ r")
                strFormatString = [String].Format("javascript:ShowFailureStatusBarMessage('{0}','{1}');", "Error :", strMessage)
                RegisterScript(strFormatString)

            Catch ex As Exception
            End Try
        End Sub

        Private Sub ShowSuccessMessage(ByVal strMessage As [String])
            Dim strFormatString As [String] = [String].Empty
            Try
                strFormatString = [String].Format("javascript:ShowSuccessStatusBarMessage('{0}','{1}');", "Note :", strMessage)
                RegisterScript(strFormatString)

            Catch ex As Exception
            End Try
        End Sub

        Private Sub RegisterScript(ByVal strFormatString As [String])
            Dim strJavaScript As [String] = [String].Empty
            strJavaScript = "<script type=""text/javascript"">ExecuteOrDelayUntilScriptLoaded(Initialize, ""sp.js"");function Initialize(){ " & Convert.ToString(strFormatString) & " }</script>"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "opennotification", strJavaScript, False)
        End Sub

#End Region
    End Class
End Namespace
