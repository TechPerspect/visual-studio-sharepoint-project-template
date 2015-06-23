## ProjectName.UI Detail ##

This project is main component of SharePoint solution which will be used to deploy or create the SharePoint Solution. As name implies this project contains all the SharePoint deploy-able files like Web Part, User Control, JS files, CSS files.Below Class Diagram show the classes used in ProjectName.UI project.  <br>    <img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/UI.Class2.png' />  <br> Below is the description of classes shown in above class diagram<br>
<br>
<br>
<ol><li><b>BaseWebPart</b> - This is the base class for all the webparts (both webpart and visual webpart). Currently no common functionality is implemented in this class. But at any point of time developer needs to expose one common public property of the web parts then developer needs to implement that public property in this class.<br>
</li><li><b>SampleVisualWebPart</b> - This is visual web part class inheriting from BaseWebPart<br>
</li><li><b>SampleWebPart</b> - This is standard web part class inheriting from BaseWebPart<br>
</li><li><b>ErrorCaptureControl</b> - This is custom webcontrol class inheriting from System.Web.UI.WebControls.WebControl class. This class is used to display error or succss message on UI depends on the method call<br><br>
<h3>Methods:</h3></li></ol>


<ol><li><b>ClearMessageFromUI</b> - This method will clear message from UI<br />
</li><li><b>SetErrorMessageforUI</b>  - This method will set the Error message on    UI. It will display message as per below screen shot <br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/ErrorMessage.png' />
</li><li><b>SetSuccessMessageforUI</b> - This method will set the Success message on    UI. It will display message as per below screen shot <br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/SuccessMessage.png' /></li></ol>