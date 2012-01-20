using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web.UI;
using $customUsing$.UI.WebControls;
using $customUsing$.Common;

namespace $safeprojectname$.WebParts
{
    public class SampleWebPart : BaseWebPart
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
