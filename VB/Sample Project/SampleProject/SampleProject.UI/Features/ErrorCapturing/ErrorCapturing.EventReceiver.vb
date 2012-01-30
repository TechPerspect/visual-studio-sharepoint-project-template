Option Explicit On
Option Strict On

Imports System
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports Microsoft.SharePoint
Imports Microsoft.SharePoint.Security
Imports SampleProject.Operations
Imports System.Web
Imports SampleProject.Exceptions
Imports SampleProject.Common

Namespace Features
    Public Class ErrorCapturingEventReceiver
        Inherits SPFeatureReceiver
      
        Public Overrides Sub FeatureActivated(ByVal properties As SPFeatureReceiverProperties)

            If properties IsNot Nothing Then
                Using objSPWeb As SPWeb = TryCast(properties.Feature.Parent, SPSite).OpenWeb()
                    If objSPWeb IsNot Nothing Then
                        Try
                            Dim objCustomAction As New InstallCustomActions(objSPWeb)
                            objCustomAction.RegisterCustomCoreInScriptLink()
                        Catch ex As Exception
                            Dim custome As CustomExceptions
                            If HttpContext.Current IsNot Nothing Then
                                custome = New CustomExceptions(ex, HttpContext.Current, objSPWeb)
                            Else
                                custome = New CustomExceptions(ex, Enums.ShowException.EventViewer)
                            End If
                        End Try
                    End If
                End Using
            End If

        End Sub


        ' Uncomment the method below to handle the event raised before a feature is deactivated.

        'public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        '{
        '}


        ' Uncomment the method below to handle the event raised after a feature has been installed.

        'public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        '{
        '}


        ' Uncomment the method below to handle the event raised before a feature is uninstalled.

        'public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        '{
        '}

        ' Uncomment the method below to handle the event raised when a feature is upgrading.

        'public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        '{
        '}
    End Class
End Namespace

