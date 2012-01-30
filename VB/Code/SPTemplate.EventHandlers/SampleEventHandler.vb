Imports System.Collections.Generic
Imports System.Text
Imports Microsoft.SharePoint
Imports System.Web 
Imports $customUsing$.Common
Imports $customUsing$.Exceptions


Public Class SampleEventHandler
    Inherits SPItemEventReceiver

    Private current As HttpContext
    Public Sub New()
        current = HttpContext.Current
    End Sub

    Public Overrides Sub ItemAdding(ByVal properties As SPItemEventProperties)
        MyBase.ItemAdding(properties)
    End Sub

    Public Overrides Sub ItemAdded(ByVal properties As SPItemEventProperties)
        MyBase.ItemAdded(properties)
    End Sub

    Public Overrides Sub ItemUpdating(ByVal properties As SPItemEventProperties)
        MyBase.ItemUpdating(properties)
    End Sub

    Public Overrides Sub ItemUpdated(ByVal properties As SPItemEventProperties)
        MyBase.ItemUpdated(properties)
    End Sub

End Class
