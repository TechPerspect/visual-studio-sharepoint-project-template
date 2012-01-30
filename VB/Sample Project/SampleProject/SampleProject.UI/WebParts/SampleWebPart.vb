Imports System.Web.UI

Namespace WebParts
    Public Class SampleWebPart
        Inherits BaseWebPart
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
