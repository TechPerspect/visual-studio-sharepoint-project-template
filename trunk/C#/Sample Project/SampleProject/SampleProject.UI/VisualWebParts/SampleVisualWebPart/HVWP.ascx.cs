using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SampleProject.UI.WebControls;
using SampleProject.Exceptions;
using System.Web;
using Microsoft.SharePoint;
using SampleProject.Common;

namespace SampleProject.UI.UserControls
{
    public partial class SampleVisualWebPartUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSuccess_Click(object sender, EventArgs e)
        {
            ErrorCaptureControl.SetSuccessMessageforUI("This is test success message");
        }

        protected void btnError_Click(object sender, EventArgs e)
        {
            ErrorCaptureControl.SetErrorMessageforUI("This is test error message");
        }

        protected void btnException_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("This is custom exception");
            }
            catch (Exception ex)
            {
                new CustomExceptions(ex, HttpContext.Current, SPContext.Current.Web);
            }
        }


        protected void btnListData_Click(object sender, EventArgs e)
        {
            ElevatedCodeHelper.ExecuteElevatedCode(SPContext.Current.Web, delegate(SPWeb elevaedWeb)
            {
                String sListNames = String.Empty;
                foreach (SPList list in elevaedWeb.Lists)
                {
                    sListNames += list.Title + "<br>";
                }

                Literal1.Text = sListNames;
                //Display list or do list operation
            });
        }
    }
}
