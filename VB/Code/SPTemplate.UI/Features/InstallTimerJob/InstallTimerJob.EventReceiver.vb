Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports Microsoft.SharePoint
Imports Microsoft.SharePoint.Security
Imports Microsoft.SharePoint.Administration
Imports  $customUsing$.Operations
Imports  $customUsing$.TimerJobs
Imports System.Web
Imports  $customUsing$.Exceptions
Imports  $customUsing$.Common

Namespace Features
    Public Class InstallTimerJobEventReceiver
        Inherits SPFeatureReceiver

        Public Overrides Sub FeatureActivated(properties As SPFeatureReceiverProperties)
            Try
                'Add Site Columns For Effectivity Functionality
                Dim objSite As SPSite = TryCast(properties.Feature.Parent, SPSite)
                If objSite IsNot Nothing Then
                    Using objSPSite As SPSite = TryCast(properties.Feature.Parent, SPSite)
                        InstallFirstTimerJob(objSPSite.RootWeb)
                    End Using
                End If
            Catch ex As Exception
            End Try
        End Sub
 
        Public Overrides Sub FeatureDeactivating(properties As SPFeatureReceiverProperties)

            'Add Site Columns For Effectivity Functionality
            Dim objSite As SPSite = TryCast(properties.Feature.Parent, SPSite)
            If objSite IsNot Nothing Then
                Using objSPSite As SPSite = TryCast(properties.Feature.Parent, SPSite)
                    Try
                        DeleteFirstTimerJob(objSPSite.RootWeb)
                    Catch ex As Exception
                        Dim custome As CustomExceptions
                        If HttpContext.Current IsNot Nothing Then
                            custome = New CustomExceptions(ex, HttpContext.Current, objSPWeb)
                        Else
                            custome = New CustomExceptions(ex, Enums.ShowException.EventViewer)
                        End If
                    End Try
                End Using
            End If

        End Sub


#Region "Install and Delete First Timer Job"
        Public Sub InstallFirstTimerJob(web As SPWeb)

            DeleteFirstTimerJob(web)

            Try
                ' install the timer job
                Dim objFirstTimer As New SampleTimerJob("$safeprojectname$ First Timer Job", web.Site.WebApplication)
                Dim schedule As New SPMinuteSchedule()

                schedule.Interval = 15
                schedule.BeginSecond = 0
                schedule.EndSecond = 0

                objFirstTimer.Schedule = schedule
                objFirstTimer.Update()
            Catch ex As Exception
                Throw ex
            End Try
        End Sub


        Public Sub DeleteFirstTimerJob(web As SPWeb)
            Try
                Dim objFirstTimerJob As SPJobDefinition = web.Site.WebApplication.JobDefinitions("$safeprojectname$ First Timer Job")
                If objFirstTimerJob IsNot Nothing Then
                    objFirstTimerJob.Delete()
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region
    End Class

End Namespace

