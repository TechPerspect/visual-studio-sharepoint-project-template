using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Web;
using $customUsing$.Common;
using $customUsing$.Exceptions;

namespace  $safeprojectname$
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
