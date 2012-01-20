using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using System.IO;
using Microsoft.Build.BuildEngine;
using System.Collections;

namespace WizardFunctionality
{
    public class IWizardImplementation : IWizard
    {

        #region Private Variables
        private EnvDTE.DTE dte = null;
        private String PROJECT_FOLDER_KEY = "ProjectFolder";
        private String[] REPLACE_PROJECTNAME = {"$safeprojectname$",  
                                                "ProjectFolder" ,
                                                "replaceprojectname"
                                               };

        private String REPLACE_FILENAME = "replaceprojectname";
        private String REPLACE_USING = "$customUsing$";
        private String PCOMMON = "Common";
        private String PUI = "UI";
        private String PTIMER = "TimerJobs";
        private String POPERATIONS = "Operations";
        private String PEXCEPTION = "Exceptions";
        private String PEVENTHANDLER = "EventHandlers";

        private String[] REPLACE_GUIDS = {"b9e30a4e-6552-40e4-aab1-4022bfaec9e2", //Package ID
                                  "938cda6c-d4b8-46ea-9855-343d2e2c42e0", //Feature ID
                                  "06ac2b37-491e-43ed-b8c2-be3c96377af8"  //Feature ID
                                  };
        //ArrayList arProjectPath = new ArrayList();
        //ArrayList arProjectName = new ArrayList();
        EnvDTE.Project commonProject = null;
        EnvDTE.Project UIProject = null;
        EnvDTE.Project TimerProject = null;
        EnvDTE.Project OperationProject = null;
        EnvDTE.Project ExceptionProject = null;
        EnvDTE.Project EventHandlerProject = null;
        EnvDTE.Project BlankProject = null;

        #endregion

        #region Interface Method
        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
            bool bSharePointProject;
            foreach (EnvDTE.Project projects in dte.Solution.Projects)
            {
                bSharePointProject = true;
                //Update File Content
                if (projects.Name.EndsWith(PCOMMON))
                {
                    bSharePointProject = false;
                    commonProject = projects;
                }

                if (projects.Name.EndsWith(PTIMER))
                {
                    bSharePointProject = false;
                    TimerProject = projects;
                }

                if (projects.Name.EndsWith(POPERATIONS))
                {
                    bSharePointProject = false;
                    OperationProject = projects;
                }

                if (projects.Name.EndsWith(PEXCEPTION))
                {
                    bSharePointProject = false;
                    ExceptionProject = projects;
                }

                if (projects.Name.EndsWith(PEVENTHANDLER))
                {
                    bSharePointProject = false;
                    EventHandlerProject = projects;
                }

                if (projects.Name.EndsWith(PUI))
                {
                    bSharePointProject = false;
                    UIProject = projects;
                }

                if (bSharePointProject)
                {
                    BlankProject = projects;
                    bSharePointProject = false;
                }
            }

            foreach (EnvDTE.Project projects in dte.Solution.Projects)
            {
                UpdateFileContents(projects.ProjectItems, BlankProject.Name);
            }

            //Adding refernce of Common Project in all solutions
            
            foreach (EnvDTE.Project projects in dte.Solution.Projects)
            {
                if (projects.Name.EndsWith(PCOMMON))
                {
                }

 
                if (projects.Name.EndsWith(PTIMER))
                {
                    VSLangProj.VSProject vsProj = (VSLangProj.VSProject)projects.Object;
                    vsProj.References.AddProject(commonProject);
                    vsProj.References.AddProject(ExceptionProject);
                    vsProj.References.AddProject(OperationProject);
                    projects.Save();
                }

                if (projects.Name.EndsWith(POPERATIONS))
                {
                    VSLangProj.VSProject vsProj = (VSLangProj.VSProject)projects.Object;
                    vsProj.References.AddProject(commonProject);
                    vsProj.References.AddProject(ExceptionProject);
                    projects.Save();
                }

                if (projects.Name.EndsWith(PEXCEPTION))
                {
                    VSLangProj.VSProject vsProj = (VSLangProj.VSProject)projects.Object;
                    vsProj.References.AddProject(commonProject);
                    projects.Save();
                }

                if (projects.Name.EndsWith(PEVENTHANDLER))
                {
                    VSLangProj.VSProject vsProj = (VSLangProj.VSProject)projects.Object;
                    vsProj.References.AddProject(commonProject);
                    vsProj.References.AddProject(ExceptionProject);
                    vsProj.References.AddProject(OperationProject);
                    projects.Save();
                }

                if (projects.Name.EndsWith(PUI))
                {
                    VSLangProj.VSProject vsProj = (VSLangProj.VSProject)projects.Object;
                    vsProj.References.AddProject(commonProject);
                    vsProj.References.AddProject(ExceptionProject);
                    vsProj.References.AddProject(OperationProject);
                    vsProj.References.AddProject(TimerProject);
                    projects.Save();
                }
            }
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            // throw new NotImplementedException();
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            // throw new NotImplementedException();
        }

        public void RunFinished()
        {
            String sPath = BlankProject.FullName;
            String sName = BlankProject.Name;
            String sReplace = sName + "\\" + sName + "\\" + sName + ".csproj";

            try
            {
                dte.Solution.Remove(BlankProject);

            }
            catch { }

            sPath = sPath.Replace(sReplace, sName + ".sln");
            dte.ExecuteCommand("File.SaveAll");
            dte.ExecuteCommand("File.CloseSolution");
            dte.Solution.Open(sPath);
            try
            {
                sPath = sPath.Replace(".sln", "\\" + sName);
                if (Directory.Exists(sPath))
                {
                    // Create the directory.
                    Directory.Delete(sPath, true);
                }
            }
            catch { }
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            dte = automationObject as DTE;
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
        #endregion

        #region Private Methods
        private void UpdateFileContents(ProjectItems items, string name)
        {
            String sFilePath;
             String sExtension;
            foreach (ProjectItem item in items)
            {
                //Renaming Folder Name
                if (item.Name == PROJECT_FOLDER_KEY)
                {
                    item.Name = name;
                }
                if (item.Name.IndexOf(REPLACE_FILENAME) > -1)
                {
                    item.Name = item.Name.Replace(REPLACE_FILENAME, name);
                }

                sFilePath = item.FileNames[0];
                sExtension = Path.GetExtension(sFilePath);

                if (!String.IsNullOrEmpty(sExtension))
                {
                    ReplaceKeysinFileContent(name, item);
                }

                if (item.ProjectItems.Count != 0)
                {
                    UpdateFileContents(item.ProjectItems, name);
                }
            }
        }

        private String GetFirstProjectName(String sName)
        {
            sName = sName.Replace("." + PCOMMON,"");
            sName = sName.Replace("." + PEVENTHANDLER, "");
            sName = sName.Replace("." + PTIMER, "");
            sName = sName.Replace("." + POPERATIONS, "");
            sName = sName.Replace("." + PEXCEPTION, "");
            return sName;
        }

        private void ReplaceKeysinFileContent(String name, ProjectItem item)
        {
            String sFilePath = item.FileNames[0];
            String sExtension = Path.GetExtension(sFilePath);

            if (sFilePath.EndsWith("key.snk"))
            {
                sFilePath = sFilePath.Replace("key.snk", name + ".UI.csproj");
                sExtension = Path.GetExtension(sFilePath);
            }


            if (sExtension.ToLower().IndexOf(".snk") == -1)
            {
                TextReader textReader = new StreamReader(sFilePath);
                String sFileContent = textReader.ReadToEnd();
                textReader.Close();

                sFileContent = sFileContent.Replace(REPLACE_USING, GetFirstProjectName(name));

                foreach (String sKey in REPLACE_PROJECTNAME)
                {
                    sFileContent = sFileContent.Replace(sKey, name);
                }

                foreach (String sGUID in REPLACE_GUIDS)
                {
                    sFileContent = sFileContent.Replace(sGUID, Guid.NewGuid().ToString());
                }

                StreamWriter writer = new StreamWriter(File.Create(sFilePath));
                writer.Write(sFileContent);
                writer.Close();
            }
        }

        #endregion
    }
}
