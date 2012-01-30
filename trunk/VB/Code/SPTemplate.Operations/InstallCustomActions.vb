Imports Microsoft.SharePoint
Imports $customUsing$.Common

Public Class InstallCustomActions
    Inherits BaseOperation
    Public Sub New(ByVal objSPWeb As SPWeb)
        MyBase.New(objSPWeb)
    End Sub

    Public Sub RegisterCustomCoreInScriptLink()
        Dim objSPUserCustomActionScript As SPUserCustomAction = Nothing
        objSPUserCustomActionScript = Me.CurrentWeb.UserCustomActions.Add()

        If Not CommonFunctions.IsExistsCustomAction("CustomCore.Js", "ScriptLink", [String].Empty, Me.CurrentWeb) Then
            objSPUserCustomActionScript.Location = "ScriptLink"
            objSPUserCustomActionScript.Name = "CustomCore.Js"
            objSPUserCustomActionScript.ScriptSrc = "/_layouts/replaceprojectname/JS/CustomCore.js"
            Me.CurrentWeb.AllowUnsafeUpdates = True
            objSPUserCustomActionScript.Update()
        End If
    End Sub
End Class
