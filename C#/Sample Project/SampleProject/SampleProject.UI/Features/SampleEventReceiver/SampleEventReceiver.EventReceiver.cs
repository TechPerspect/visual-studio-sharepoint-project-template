using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using SampleProject.Common;
using SampleProject.Operations;
using SampleProject.EventHandlers;
using System.Collections;

namespace SampleProject.UI.Features
{

    [Guid("87a7796a-261d-4c84-94dc-03e50159d7c4")]
    public class SampleEventReceiverEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite objSite = properties.Feature.Parent as SPSite;
            if (objSite != null)
            {

                ElevatedCodeHelper.ExecuteElevatedCode(objSite.RootWeb, delegate(SPWeb elevatedWeb)
                {
                    ListOperations objList = new ListOperations(elevatedWeb);
                    bool isExist;

                    Guid listID = objList.CreateList(out isExist, "Sample Event Handler List", "List created by code", new ArrayList(), SPListTemplateType.GenericList);
                    SPList list = objList.GetListByListID(listID.ToString());

                    list.EventReceivers.Add(SPEventReceiverType.ItemAdding, GetEventHandlerAssemblyDetail.GetAssemblyDetail(), "SampleProject.EventHandlers.SampleEventHandler");
                });

            }
        }

    }
}
