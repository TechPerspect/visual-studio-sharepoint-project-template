# Sample Guide #

  1. Open Visual Studio 2010 -> File -> New -> Project<br />![http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S1.png](http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S1.png)<br>
<ol><li>Selct Visual C#-> SharePoint -> 2010 -> Select SharePoint Project Template -> Provide Name and select Location and click OK. It will create project as per you input as mentioned in below screen shot <br /><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S2.png' /><br>
</li><li>After successful creation your solution explorer will look like below screen shot <br /><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S3.png' /><br>
</li><li>Set your Site URL in {ProjectName}.UI properties as in below screen shots<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S23.png' /><br>
</li><li>Right click on solution name and click on Deploy from the context menu to deploy the solution<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S24.png' /><br>
</li><li>After deployment it will show Deploy succeeded message in Visual Studio Status Bar as mentioned in below screen shot<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S22.png' />
</li><li>After Successful deployment of your solution. Below are the screen shots to validate the deployed solution in SharePoint environment.<br>All the physical files used in SharePoint solution will be deployed in C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14<br><br>Deployed File type with location<br>

<BR>

	Feature Files<b>- C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\FEATURES</b><br><br>	User Controls<b>- C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\CONTROLTEMPLATES\{ProjectName}</b><br><br>	JS and CSS Files<b>- C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\{ProjectName}</b><br><br>	CSharp File<b>- All custom c# class deployed in GAC as mentioned in below screen shots</b><br><br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S20.png' /><br><br>
</li><li>SharePoint maintain all WSP solution in Solution store in SharePoint<br>Go to SharePoint Central Administration -> System Settings -> Manager Farm Solution and validate your solution status<br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/S7.png' /><br></li></ol>

<h3>Example of Sample Project Deployed Items</h3>
<a href='http://code.google.com/p/visual-studio-sharepoint-project-template/wiki/SampleProjectWebPart'>Example of WebPart</a><br>
<a href='http://code.google.com/p/visual-studio-sharepoint-project-template/wiki/SampleEventHandler'>Example of Event Handler</a><br>
<a href='http://code.google.com/p/visual-studio-sharepoint-project-template/wiki/SampleTimerJob'>Example of Timer Jobs</a><br>