Public Class GetEventHandlerAssemblyDetail
    Public Shared Function GetAssemblyDetail() As [String]
        Return GetType(GetEventHandlerAssemblyDetail).Assembly.ToString()
    End Function
End Class

