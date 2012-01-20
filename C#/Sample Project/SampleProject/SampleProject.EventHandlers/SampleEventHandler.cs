using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using SampleProject.Common;
using SampleProject.Exceptions;
using System.Web;

namespace SampleProject.EventHandlers
{
    public class SampleEventHandler : SPItemEventReceiver
    {
       HttpContext current;

       public SampleEventHandler()
       {
           current = HttpContext.Current;
       }

       public override void ItemAdding(SPItemEventProperties properties)
       {
           base.ItemAdding(properties);
           //Cancel Event if title contain special character
           if (properties != null)
           {

               String sTitle = Convert.ToString(properties.AfterProperties["Title"]);
               if (!String.IsNullOrEmpty(sTitle))
               {
                   if (sTitle.HasSpecialCharacterForFileandFolder())
                   {
                       properties.Status = SPEventReceiverStatus.CancelWithRedirectUrl;
                       SPWeb web = properties.OpenWeb();
                       properties.RedirectUrl = current.Request.Url.ToString();
                       CustomExceptions.SetErrorMessageforUI(current, web, "Special Character is not allowed");
                   }
               }

           }
       }

        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
        }

        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
        }

    }
}
