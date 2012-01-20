using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web.UI;
using SampleProject.UI.WebControls;
using SampleProject.Common;
using SampleProject.Operations;
using System.Web.UI.WebControls;
using SampleProject.EventHandlers;
using System.Collections;

namespace SampleProject.UI.WebParts
{
    public class SampleWebPart : BaseWebPart
    {
        #region WebPart Methods
        Button btnClick;
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            btnClick = new Button();
            btnClick.ID = "ltControl";
            btnClick.Text = "Create List and Attach event handler";
            btnClick.Click += new EventHandler(btnClick_Click); 
            this.Controls.Add(btnClick);  
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void RenderWebPart(HtmlTextWriter output)
        {
            base.RenderWebPart(output);
            output.Write("<table>");
            output.Write("<tr>");
            output.Write("<td>");
            output.Write("Click on this button wil create new list in SharePoint and add ITEMADDING event to the list");
            output.Write("</td>");
            output.Write("</tr>");
            output.Write("</table>");
        }

        protected void btnClick_Click(object sender, EventArgs e)
        {
            ElevatedCodeHelper.ExecuteElevatedCode(SPContext.Current.Web, delegate(SPWeb elevatedWeb)
            {
                ListOperations objList = new ListOperations(SPContext.Current.Web);
                bool isExist ;
                Guid listID = objList.CreateList(out isExist,"Test List","List created by code",new ArrayList(),SPListTemplateType.GenericList);

                SPList list = objList.GetListByListID(listID.ToString());

                list.EventReceivers.Add(SPEventReceiverType.ItemAdding, GetEventHandlerAssemblyDetail.GetAssemblyDetail(), "SampleProject.EventHandlers.SampleEventHandler");    
            });
  
        }

        #endregion
    }
}
