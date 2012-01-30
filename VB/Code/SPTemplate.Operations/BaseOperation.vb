Imports Microsoft.SharePoint

Public MustInherit Class BaseOperation
    Public CurrentWeb As SPWeb

    Public Sub New(ByVal objSPWeb As SPWeb)
        CurrentWeb = objSPWeb
    End Sub
End Class