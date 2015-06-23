  1. After deployment of Sample Project **SampleProject Event Receiver Example** feature will be added in the Site Collection features.<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S31.png' />
<ol><li>On Activation of this feature new list will be added in current Site and ItemAdding event will be associated with the list <br>Goto Site Actions -> View All Site Content<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S32.png' />
</li><li>To verify the attached events on list refer below screen shots. These steps is not provided by the SharePoint 2010. But you can refer another open source <a href='http://code.google.com/p/sharepoint-eventhandlers-manager/'>SharePoint Event handlers Manager</a> to validate or configure the event handler. Download the <a href='http://code.google.com/p/sharepoint-eventhandlers-manager/downloads/detail?name=sharepoint-eventhandlers-manager-2010-1.0.zip'>Event Handler solution</a> from here<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S38.png' /><br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S39.png' /><br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S40.png' />
</li><li>Open List <b>Sample Event Handler List</b>  Click -> Add new Item<br>
</li><li>Enter some special character<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S33.png' />
</li><li>Click save will show you error message generated by Event handler attached to the list<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S34.png' /></li></ol>

<blockquote><h3>Steps to Add or Modify Event Handler</h3></blockquote>

<ol><li>Goto SampleProject.EventHandlers-> Right Click on Project Name -> Goto Add -> Class from the context menu as shown in below screen shot -> Provide the name of the class and click Add<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S25.png' /><br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S30.png' />
</li><li>Refer <b>SampleEventHandler.cs</b> for modifying existing code used for Event Handler sample in <a href='http://code.google.com/p/visual-studio-sharepoint-project-template/downloads/detail?name=Sample%20Project%20C%23%201.0.zip'>Sample project</a><br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S35.png' /></li></ol>
