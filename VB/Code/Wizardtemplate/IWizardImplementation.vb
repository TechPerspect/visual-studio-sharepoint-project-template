Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TemplateWizard
Imports EnvDTE
Imports System.IO
Imports Microsoft.Build.BuildEngine
Imports System.Collections
 
Public Class IWizardImplementation
    Implements IWizard

#Region "Private Variables"
    Private dte As EnvDTE.DTE = Nothing
    Private PROJECT_FOLDER_KEY As [String] = "ProjectFolder"
    Private REPLACE_PROJECTNAME As [String]() = {"$safeprojectname$", "ProjectFolder", "replaceprojectname"}

    Private REPLACE_FILENAME As [String] = "replaceprojectname"
    Private REPLACE_USING As [String] = "$customUsing$"
    Private PCOMMON As [String] = "Common"
    Private PUI As [String] = "UI"
    Private PTIMER As [String] = "TimerJobs"
    Private POPERATIONS As [String] = "Operations"
    Private PEXCEPTION As [String] = "Exceptions"
    Private PEVENTHANDLER As [String] = "EventHandlers"

    Private REPLACE_GUIDS As [String]() = {"b9e30a4e-6552-40e4-aab1-4022bfaec9e2", "938cda6c-d4b8-46ea-9855-343d2e2c42e0", "06ac2b37-491e-43ed-b8c2-be3c96377af8", "216ab41c-169e-4d90-9395-09b8be2fc7bd", "1906b937-9a39-4dae-b1ad-2b04a2e285f6", "d832f612-81d0-4e08-83ee-28a4d1e45f55"}
    Private commonProject As EnvDTE.Project = Nothing
    Private UIProject As EnvDTE.Project = Nothing
    Private TimerProject As EnvDTE.Project = Nothing
    Private OperationProject As EnvDTE.Project = Nothing
    Private ExceptionProject As EnvDTE.Project = Nothing
    Private EventHandlerProject As EnvDTE.Project = Nothing
    Private BlankProject As EnvDTE.Project = Nothing

#End Region

#Region "Interface Method"
    Public Sub ProjectFinishedGenerating(ByVal project As EnvDTE.Project) Implements IWizard.ProjectFinishedGenerating

        Dim bSharePointProject As Boolean
        For Each projects As EnvDTE.Project In dte.Solution.Projects
            bSharePointProject = True
            'Update File Content
            If projects.Name.EndsWith(PCOMMON) Then
                bSharePointProject = False
                commonProject = projects
            End If

            If projects.Name.EndsWith(PTIMER) Then
                bSharePointProject = False
                TimerProject = projects
            End If

            If projects.Name.EndsWith(POPERATIONS) Then
                bSharePointProject = False
                OperationProject = projects
            End If

            If projects.Name.EndsWith(PEXCEPTION) Then
                bSharePointProject = False
                ExceptionProject = projects
            End If

            If projects.Name.EndsWith(PEVENTHANDLER) Then
                bSharePointProject = False
                EventHandlerProject = projects
            End If

            If projects.Name.EndsWith(PUI) Then
                bSharePointProject = False
                UIProject = projects
            End If

            If bSharePointProject Then
                BlankProject = projects
                bSharePointProject = False
            End If
        Next

        For Each projects As EnvDTE.Project In dte.Solution.Projects
            UpdateFileContents(projects.ProjectItems, BlankProject.Name)
        Next

        'Adding refernce of Common Project in all solutions

        For Each projects As EnvDTE.Project In dte.Solution.Projects
            If projects.Name.EndsWith(PCOMMON) Then
            End If


            If projects.Name.EndsWith(PTIMER) Then
                Dim vsProj As VSLangProj.VSProject = DirectCast(projects.[Object], VSLangProj.VSProject)
                vsProj.References.AddProject(commonProject)
                vsProj.References.AddProject(ExceptionProject)
                vsProj.References.AddProject(OperationProject)
                projects.Save()
            End If

            If projects.Name.EndsWith(POPERATIONS) Then
                Dim vsProj As VSLangProj.VSProject = DirectCast(projects.[Object], VSLangProj.VSProject)
                vsProj.References.AddProject(commonProject)
                vsProj.References.AddProject(ExceptionProject)
                projects.Save()
            End If

            If projects.Name.EndsWith(PEXCEPTION) Then
                Dim vsProj As VSLangProj.VSProject = DirectCast(projects.[Object], VSLangProj.VSProject)
                vsProj.References.AddProject(commonProject)
                projects.Save()
            End If

            If projects.Name.EndsWith(PEVENTHANDLER) Then
                Dim vsProj As VSLangProj.VSProject = DirectCast(projects.[Object], VSLangProj.VSProject)
                vsProj.References.AddProject(commonProject)
                vsProj.References.AddProject(ExceptionProject)
                vsProj.References.AddProject(OperationProject)
                projects.Save()
            End If

            If projects.Name.EndsWith(PUI) Then
                Dim vsProj As VSLangProj.VSProject = DirectCast(projects.[Object], VSLangProj.VSProject)
                vsProj.References.AddProject(commonProject)
                vsProj.References.AddProject(ExceptionProject)
                vsProj.References.AddProject(OperationProject)
                vsProj.References.AddProject(TimerProject)
                projects.Save()
            End If
        Next
    End Sub

    Public Sub BeforeOpeningFile(ByVal projectItem As ProjectItem) Implements IWizard.BeforeOpeningFile
        ' throw new NotImplementedException();
    End Sub

    Public Sub ProjectItemFinishedGenerating(ByVal projectItem As ProjectItem) Implements IWizard.ProjectItemFinishedGenerating
        ' throw new NotImplementedException();
    End Sub

    Public Sub RunFinished() Implements IWizard.RunFinished
        Dim sPath As [String] = BlankProject.FullName
        Dim sName As [String] = BlankProject.Name
        Dim sReplace As [String] = sName & "\" & sName & "\" & sName & ".vbproj"

        Try

            dte.Solution.Remove(BlankProject)
        Catch
        End Try

        sPath = sPath.Replace(sReplace, sName & ".sln")
        dte.ExecuteCommand("File.SaveAll")
        dte.ExecuteCommand("File.CloseSolution")
        dte.Solution.Open(sPath)
        Try
            sPath = sPath.Replace(".sln", "\" & sName)
            If Directory.Exists(sPath) Then
                ' Create the directory.
                Directory.Delete(sPath, True)
            End If
        Catch
        End Try
    End Sub

    Public Sub RunStarted(ByVal automationObject As Object, ByVal replacementsDictionary As Dictionary(Of String, String), ByVal runKind As WizardRunKind, ByVal customParams As Object()) Implements IWizard.RunStarted
        dte = TryCast(automationObject, DTE)
    End Sub

    Public Function ShouldAddProjectItem(ByVal filePath As String) As Boolean Implements IWizard.ShouldAddProjectItem
        Return True
    End Function
#End Region

#Region "Private Methods"
    Private Sub UpdateFileContents(ByVal items As ProjectItems, ByVal name As String)
        Dim sFilePath As [String]
        Dim sExtension As [String]
        For Each item As ProjectItem In items
            'Renaming Folder Name
            If item.Name = PROJECT_FOLDER_KEY Then
                item.Name = name
            End If
            If item.Name.IndexOf(REPLACE_FILENAME) > -1 Then
                item.Name = item.Name.Replace(REPLACE_FILENAME, name)
            End If

            sFilePath = item.FileNames(0)
            sExtension = Path.GetExtension(sFilePath)

            If Not [String].IsNullOrEmpty(sExtension) Then
                ReplaceKeysinFileContent(name, item)
            End If

            If item.ProjectItems.Count <> 0 Then
                UpdateFileContents(item.ProjectItems, name)
            End If
        Next
    End Sub

    Private Function GetFirstProjectName(ByVal sName As [String]) As [String]
        sName = sName.Replace("." & PCOMMON, "")
        sName = sName.Replace("." & PEVENTHANDLER, "")
        sName = sName.Replace("." & PTIMER, "")
        sName = sName.Replace("." & POPERATIONS, "")
        sName = sName.Replace("." & PEXCEPTION, "")
        Return sName
    End Function

    Private Sub ReplaceKeysinFileContent(ByVal name As [String], ByVal item As ProjectItem)
        Dim sFilePath As [String] = item.FileNames(0)
        Dim sExtension As [String] = Path.GetExtension(sFilePath)

        If sFilePath.EndsWith("key.snk") Then
            sFilePath = sFilePath.Replace("key.snk", name & ".UI.vbproj")
            sExtension = Path.GetExtension(sFilePath)
        End If


        If sExtension.ToLower().IndexOf(".snk") = -1 Then
            Dim textReader As TextReader = New StreamReader(sFilePath)
            Dim sFileContent As [String] = textReader.ReadToEnd()
            textReader.Close()

            sFileContent = sFileContent.Replace(REPLACE_USING, GetFirstProjectName(name))

            For Each sKey As [String] In REPLACE_PROJECTNAME
                sFileContent = sFileContent.Replace(sKey, name)
            Next

            For Each sGUID As [String] In REPLACE_GUIDS
                sFileContent = sFileContent.Replace(sGUID, Guid.NewGuid().ToString())
            Next

            Dim writer As New StreamWriter(File.Create(sFilePath))
            writer.Write(sFileContent)
            writer.Close()
        End If
    End Sub

#End Region

End Class

