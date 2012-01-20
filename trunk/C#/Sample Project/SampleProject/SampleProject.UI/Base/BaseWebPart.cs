using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;

namespace SampleProject.UI.WebParts
{
    public class BaseWebPart : Microsoft.SharePoint.WebPartPages.WebPart
    {
        #region WebPart Methods
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void RenderWebPart(HtmlTextWriter output)
        {
            base.RenderWebPart(output);
        }

        #endregion
    }
}
