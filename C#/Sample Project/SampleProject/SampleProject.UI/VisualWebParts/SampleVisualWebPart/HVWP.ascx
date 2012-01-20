<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HVWP.ascx.cs" Inherits="SampleProject.UI.UserControls.SampleVisualWebPartUserControl" %>


<table>
    <tr>
        <td><asp:Button ID="btnSuccess"  runat="server" Text="Show Success Message" onclick="btnSuccess_Click"/></td>
        <td>On click of this button will show the success message on top of the page</td>
    </tr>
    <tr>
        <td><asp:Button ID="btnError"  runat="server" Text="Show Error Message"  onclick="btnError_Click"/></td>
        <td>On click of this button will show the error message on top of the page</td>
    </tr>
    <tr>
        <td><asp:Button ID="btnException"  runat="server" Text="Show Handled Exception Message"  onclick="btnException_Click"/></td>
        <td>On click of this button will show the handled exception message on top of the page</td>
    </tr>
    <tr>
        <td><asp:Button ID="btnListData"  runat="server" Text="Show List Data with Elevated Code"  onclick="btnListData_Click"/></td>
        <td>On click of this button will show all the list name exists in the current web</td>
    </tr>
    <tr>
        <td colspan ="2"><asp:Literal ID="Literal1" runat="server"></asp:Literal></td>
    </tr>
</table>




