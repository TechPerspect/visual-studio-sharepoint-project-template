Imports System.Web.UI
Imports Microsoft.SharePoint.WebControls
Namespace WebParts
    Public Class BaseWebPart
        Inherits Microsoft.SharePoint.WebPartPages.WebPart
#Region "WebPart Methods"
        Protected Overrides Sub CreateChildControls()
            MyBase.CreateChildControls()
        End Sub


        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            MyBase.OnPreRender(e)
        End Sub

        Protected Overrides Sub RenderWebPart(ByVal output As HtmlTextWriter)
            MyBase.RenderWebPart(output)
        End Sub

#End Region
    End Class
End Namespace
