## ProjectName.Operations Detail ##

> This project contains all the operational classes used to perform an operation in SharePoint like adding an item, creating a list, installing a custom action.Below Class Diagram show the classes used in ProjectName.Operations project.  <br><img src='http://visual-studio-sharepoint-project-template.googlecode.com/svn/wiki/Images/Operations.Class.png' />
<br>
<blockquote>Below is the description of classes shown in above class diagram</blockquote>

<ol><li><b>InstallCustomAction</b> - This class is used to install all custom actions in SharePoint. It is inheriting from BaseOperation class<br>
<h3>Method:</h3></li></ol>

<ol><li><b>RegisterCustomCoreInScriptLink</b> - This method is used to register custom javascript file in Script Link<br>
<br>
<br>
<hr/><br>
<br>
<br>
</li><li><b>ListOperations</b> - This class is used to perform all list level operations in SharePoint. It is inheriting from BaseOperation class<br><br>
<h3>Method:</h3>
</li><li><b>AddItem</b> - This method is used to create list item in list<br>
</li><li><b>CreateList</b> - This method is used to create list in Site<br>
</li><li><b>CreateSiteColumn</b> - This method is used to create site column in site<br>
</li><li><b>DeleteItemByID</b> - This method is used to delete list item based on ID from the list<br>
</li><li><b>DeleteItemByTitle</b> -  This method is used to delete list item based on Title from the list<br>
</li><li><b>DeleteList</b> - This method is used to delete list from site<br>
</li><li><b>GetFileNameById</b> - This method is used get the file name from the ID<br>
</li><li><b>GetListByListID</b> - This method is used to get the list based on the List ID<br>
</li><li><b>GetListByName</b> - This method is used to get the list based on the list name<br>
</li><li><b>DeleteItemPermanentlyByID</b> - This method is used to delete the item permanently from the system. User will not be able to restore the item from the recycle bin<br>
</li><li><b>GetContentTypeFieldsByContentTypeID</b> - This method will return collection of fields of content type based on the content type id<br>
</li><li><b>GetListFieldsByListName</b> - This method will return the field collection for the specified list<br>
</li><li><b>GetListItem</b> - This method will return the list item<br>
</li><li><b>GetListItemByID</b> - This method will return the list item based on the list item id<br>
</li><li><b>GetListURL</b> - This method will return the URL of the specified list<br>
</li><li><b>GetSPFieldByName</b> - This method will return the sharepoint field based on the name of the field<br>
</li><li><b>IsListExisting</b> - This method will return true or false whether list is exists or not in the specified web<br>
</li><li><b>IsListItemExists</b> - This method will return true or false whether list item is exists or not in the specified list<br>
</li><li><b>IsPageExists</b> - This method will return true or false whether the page is exists or not in the specified url<br>
</li><li><b>PerformMinorCheckin</b> - This method will check-in the document or page in minor version<br>