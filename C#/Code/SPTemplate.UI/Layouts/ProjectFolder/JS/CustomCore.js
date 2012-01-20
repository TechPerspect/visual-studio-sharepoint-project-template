function openPopupControl(popupPage, siteURL, eventTarget, width, height) {
    var absoluteUrl = siteURL + popupPage;
    var appender = "";
    var options = SP.UI.$create_DialogOptions();
    options.url = absoluteUrl;
    options.width = width;
    options.height = height;
    options.dialogReturnValueCallback = Function.createDelegate(false, closePopupWithoutReturnParams);
    SP.UI.ModalDialog.showModalDialog(options);
    return false;
}

function openPopup(popupPage, siteURL, eventTarget) {
    return openPopupControl(popupPage, siteURL, eventTarget, 950, 700)
}

function openPopupWithQueryParams(itemid, listid, siteUrl, pageurl, eventTrgt) 
{
    var url = pageurl + '?ItemID=' + itemid + '&ListID=' + listid;
    return openPopup(url, siteUrl, eventTrgt);
}


function closePopupWithoutReturnParams(RetVal) 
{
    if (RetVal != null)
     {
         if (RetVal == "postBack") 
        {
            __doPostBack('Refresh', 'RefreshData');
        }
        else
         {
            return false;
         }
     }
    else {
           return false;
         }
   }


function ShowStatusBarMessage(title, message)
 {
    var statusId = SP.UI.Status.addStatus(title, message, true);
    SP.UI.Status.setStatusPriColor(statusId, 'yellow'); 
}

function ShowFailureStatusBarMessage(title, message) {
    var statusId = SP.UI.Status.addStatus(title, message, true);
    SP.UI.Status.setStatusPriColor(statusId, 'red');  
}

function ShowSuccessStatusBarMessage(title, message) {
    var statusId = SP.UI.Status.addStatus(title, message, true);
    SP.UI.Status.setStatusPriColor(statusId, 'green');
}

function ShowNotificationMessage(tooltip, message, sticky) 
{
   return SP.UI.Notify.addNotification(message, sticky, tooltip, null);
}

function removeNotificationMessage(nid) 
{
    SP.UI.Notify.removeNotification(nid);
}

 
function openConfirmDialog(eventTarget, eventArgs, message) {
    var retval = confirm(message);
    if (retval == true) {
        __doPostBack(eventTarget, eventArgs);
    }
    else {
        return false;
    }
}

 

function removeAllStatusMessages() {
    SP.UI.Status.removeAllStatus(true);
}

  