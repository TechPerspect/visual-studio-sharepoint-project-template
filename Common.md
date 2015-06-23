## ProjectName.UI Detail ##

This project contains all the common methods or functionality which will be used in all projects like Constants, Common Methods, Enumarators etc.Below Class Diagram show the classes used in ProjectName.Common project.  <br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/Common.Class2.png' />


Below is the description of classes shown in above class diagram<br>
<br>
<ol><li><b>CommonFunctions</b> - This class contains all the static methods which is generally used in every SharePoint project.<br>
<h3>Methods:</h3></li></ol>

<ol><li><b>GetColumnTypeByFieldGUID</b> -This method will return column type based on the GUID of that field<br>
</li><li><b>RemoveSpecialNames</b> - This method will remove all the special names which is not allowed in file names<br>
</li><li><b>EscapeCAMLChars</b> - This method will escape CAML character from the XML<br>
</li><li><b>RevertCAMLChars</b> - This method will revert CAML character from the XML<br>
</li><li><b>IsExistsCustomAction</b> - This method will check whether specific custom exists in the web or not.<br>
</li><li><b>GetErrorCodeKey</b> - This method will return error key which is used to display error message on UI<br>
<br>
<br>
<hr/><br>
<br>
<br>
</li><li><b>Enums</b> - This class all the enumerators used in the solution<br>
<br>
<br>
<hr/><br>
<br>
<br>
</li><li><b>Constants</b> - This class contains all the Constants used in the solution<br>
<br>
<br>
<hr/><br>
<br>
<br>
</li><li><b>ElevatedCodeHelper</b> - This class is used to overwrite the SPSecurity.RunWithElevatedPrivileges. You need to call ExecuteElevatedCode method to override SPSecurity. You can take reference of this in Visual WebPart code in <a href='http://code.google.com/p/visual-studio-sharepoint-project-template/wiki/Sample'>Sample</a><br />used <b>PrivilegedCodeDelegate</b> delegate to use above function anonymously. You can directly pass the method by using anonymous method in .Net.<br>
<br>
<br>
<hr/><br>
<br>
<br>
</li><li><b>ExtensionMethods</b> - This is static class and contains all the extended method which is used to bind with different objects<br>
<h3>Methods:</h3></li></ol>

<ol><li><b>Serialise</b>  - This method is extension of Object class. This will serialize respective object in XML string<br>
</li><li><b>DeSerialise</b> - This method is extension of String class, this will return respective XML to the Object<br>
</li><li><b>SaveCustomAttribute, SetCustomAttribute and GetCustomAttribue</b> - These methods are extensions of SPField class. These methods are used to save and get the SPField attributes via reflection<br>
</li><li><b>GetResourceString</b> - This method is extension of String class. This method will get the resource string based on the respective key for that string<br>
</li><li><b>GetFieldNamebyEscapeCharacter</b> - This method is extension of String class. This method will return the sharepoint field name by replacing escape character for SharePoint<br>
</li><li><b>HasSpecialCharacterForFileandFolder and HasSpecialCharacterForSiteandGroups</b> - This method is extension of String class. This method will return the boolean value based on the validating the special character in current string</li></ol>


