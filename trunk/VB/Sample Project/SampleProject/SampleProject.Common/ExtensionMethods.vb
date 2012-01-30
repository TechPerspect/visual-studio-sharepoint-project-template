Imports System.Runtime.CompilerServices
Imports Microsoft.SharePoint
Imports System.Web
Imports Microsoft.SharePoint.Utilities
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO
Imports System.Reflection

Module ExtensionMethods
#Region "Extension Methods"
    <Extension()> _
    Public Function HasSpecialCharacterForFileandFolder(ByVal sValue As [String]) As [Boolean]
        Dim sChars As [Char]() = {"~"c, "#"c, "%"c, "&"c, "*"c, "{"c, _
         "}"c, "\"c, ":"c, "<"c, ">"c, "?"c, _
         "/"c, "|"c, """"c}

        If sValue.IndexOfAny(sChars) > -1 Then
            Return True
        End If

        If sValue.IndexOf("..") > -1 Then
            Return True
        End If

        If sValue.IndexOf("'") > -1 Then
            Return True
        End If

        If sValue.StartsWith(".") Then
            Return True
        End If

        If sValue.EndsWith(".") Then
            Return True
        End If

        Dim array As [String]() = {".files", "_files", "-Dateien", "_fichiers", "_bestanden", "_file", _
         "_archivos", "-filer", "_tiedostot", "_pliki", "_soubory", "_elemei", _
         "_ficheiros", "_arquivos", "_dosyalar", "_datoteke", "_fitxers", "_failid", _
         "_fails", "_bylos", "_fajlovi", "_fitxategiak"}

        For iLoop As Integer = 0 To array.Length - 1
            If sValue.EndsWith(array(iLoop)) Then
                Return True
            End If
        Next

        Return False
    End Function

    <Extension()> _
    Public Function HasSpecialCharacterForSiteandGroup(ByVal sValue As [String]) As [Boolean]
        Dim sChars As [Char]() = {"~"c, "#"c, "%"c, "&"c, "*"c, "{"c, _
         "}"c, "\"c, ":"c, "<"c, ">"c, "?"c, _
         "/"c, "+"c, "|"c, """"c}

        If sValue.IndexOfAny(sChars) > -1 Then
            Return True
        End If

        If sValue.StartsWith("_") Then
            Return True
        End If

        If sValue.IndexOf("..") > -1 Then
            Return True
        End If

        If sValue.EndsWith(".") Then
            Return True
        End If

        If sValue.StartsWith(".") Then
            Return True
        End If
        Return False
    End Function

    <Extension()> _
    Public Function GetResourceString(ByVal Key As [String], ByVal sResourceFileName As [String]) As [String]
        Dim currentWeb As SPWeb = Nothing
        If SPContext.Current IsNot Nothing AndAlso SPContext.Current.Web IsNot Nothing Then
            currentWeb = SPContext.Current.Web
        End If
        Dim LanguageCode As UInteger = If(currentWeb IsNot Nothing, currentWeb.Language, 1033)
        Return SPUtility.GetLocalizedString("$Resources:" & Convert.ToString(Key), sResourceFileName, LanguageCode)
    End Function

    <Extension()> _
    Public Function GetResourceString(ByVal Key As [String], ByVal currentWeb As SPWeb, ByVal sResourceFileName As [String]) As [String]
        Dim LanguageCode As UInteger = If(currentWeb IsNot Nothing, currentWeb.Language, 1033)
        Return Convert.ToString(SPUtility.GetLocalizedString("$Resources:" & Convert.ToString(Key), sResourceFileName, LanguageCode))
    End Function

    <Extension()> _
    Public Function Serialise(ByVal objEntity As Object) As [String]
        Dim xmlDoc As New XmlDocument()
        Try
            Dim xmlSerializer As New XmlSerializer(objEntity.[GetType]())
            Using xmlStream As New MemoryStream()
                xmlSerializer.Serialize(xmlStream, objEntity)
                xmlStream.Position = 0
                xmlDoc.Load(xmlStream)
            End Using
        Catch Ex As Exception
        End Try
        Return xmlDoc.InnerXml
    End Function

    <Extension()> _
    Public Function DeSerialise(ByVal XMLString As String, ByVal objEntity As [Object]) As [Object]
        Try
            Dim oXmlSerializer As New XmlSerializer(objEntity.[GetType]())
            objEntity = oXmlSerializer.Deserialize(New StringReader(XMLString))
        Catch
        End Try
        Return objEntity
    End Function

    <Extension()> _
    Public Sub SaveCustomAttribute(ByVal sField As SPField, ByVal propertyName As String, ByVal propertyValue As Object)
        If sField IsNot Nothing Then
            Try
                Dim type As Type = GetType(SPField)
                If propertyValue IsNot Nothing Then
                    Dim [set] As MethodInfo = type.GetMethod("SetFieldAttributeValue", BindingFlags.NonPublic Or BindingFlags.Instance)
                    [set].Invoke(sField, New Object() {propertyName, propertyValue.ToString()})
                    sField.Update()
                Else
                    Dim remove As MethodInfo = type.GetMethod("RemoveFieldAttributeValue", BindingFlags.NonPublic Or BindingFlags.Instance)
                    remove.Invoke(sField, New Object() {propertyName})
                    sField.Update()
                End If
            Catch
            End Try
        End If
    End Sub

    <Extension()> _
    Public Sub SetCustomAttribute(ByVal sField As SPField, ByVal propertyName As String, ByVal propertyValue As Object)
        If sField IsNot Nothing Then
            Try
                Dim type As Type = GetType(SPField)
                If propertyValue IsNot Nothing Then
                    Dim [set] As MethodInfo = type.GetMethod("SetFieldAttributeValue", BindingFlags.NonPublic Or BindingFlags.Instance)
                    [set].Invoke(sField, New Object() {propertyName, propertyValue.ToString()})
                Else
                    Dim remove As MethodInfo = type.GetMethod("RemoveFieldAttributeValue", BindingFlags.NonPublic Or BindingFlags.Instance)
                    remove.Invoke(sField, New Object() {propertyName})
                End If
            Catch
            End Try
        End If
    End Sub

    <Extension()> _
    Public Function GetCustomAttribue(ByVal sField As SPField, ByVal propertyName As String) As String
        Dim sReturn As [String] = [String].Empty
        If sField IsNot Nothing Then
            Dim type As Type = GetType(SPField)
            Dim getField As MethodInfo = type.GetMethod("GetFieldAttributeValue", BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, New Type() {GetType([String])}, Nothing)
            Dim objValue As Object = getField.Invoke(sField, New Object() {propertyName})
            sReturn = Convert.ToString(objValue)
        End If
        Return sReturn
    End Function

    <Extension()> _
    Public Function GetFieldNamebyEscapeCharacter(ByVal sName As String) As [String]
        Dim sReturn As [String] = sName
        Dim sChar As [String] = "_x00{0}_"
        sReturn = sReturn.Replace("_", [String].Format(sChar, "2d"))
        sReturn = sReturn.Replace(" ", [String].Format(sChar, "20"))
        sReturn = sReturn.Replace("-", [String].Format(sChar, "27"))
        sReturn = sReturn.Replace("#", [String].Format(sChar, "23"))
        sReturn = sReturn.Replace("%", [String].Format(sChar, "25"))
        sReturn = sReturn.Replace("{", [String].Format(sChar, "7B"))
        sReturn = sReturn.Replace("}", [String].Format(sChar, "7D"))
        sReturn = sReturn.Replace("|", [String].Format(sChar, "7C"))
        sReturn = sReturn.Replace("\", [String].Format(sChar, "5C"))
        sReturn = sReturn.Replace("^", [String].Format(sChar, "5E"))
        sReturn = sReturn.Replace("~", [String].Format(sChar, "7E"))
        sReturn = sReturn.Replace("[", [String].Format(sChar, "5B"))
        sReturn = sReturn.Replace("]", [String].Format(sChar, "5D"))
        sReturn = sReturn.Replace("`", [String].Format(sChar, "60"))
        sReturn = sReturn.Replace(";", [String].Format(sChar, "3B"))
        sReturn = sReturn.Replace("/", [String].Format(sChar, "2F"))
        sReturn = sReturn.Replace("?", [String].Format(sChar, "3F"))
        sReturn = sReturn.Replace(":", [String].Format(sChar, "3A"))
        sReturn = sReturn.Replace("@", [String].Format(sChar, "40"))
        sReturn = sReturn.Replace("=", [String].Format(sChar, "3D"))
        sReturn = sReturn.Replace("&", [String].Format(sChar, "26"))
        sReturn = sReturn.Replace("$", [String].Format(sChar, "24"))
        Return sReturn
    End Function

#End Region
End Module

