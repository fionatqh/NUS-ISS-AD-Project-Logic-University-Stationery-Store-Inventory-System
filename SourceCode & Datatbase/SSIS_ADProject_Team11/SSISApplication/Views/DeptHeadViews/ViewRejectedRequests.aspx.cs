using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.DeptHeadViews
{
    public partial class ViewRejectedRequests : System.Web.UI.Page
    {
        DeptHeadLibrary dhl = new DeptHeadLibrary();
        string loginUser = string.Empty;
        CommonFunctionLibrary cf = new CommonFunctionLibrary();
        int deptId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("depthead") || Context.User.IsInRole("delegatehead")))
            {
                Response.Redirect("~/Account/Login");
            }

            loginUser = HttpContext.Current.User.Identity.Name;
            deptId = cf.GetDeptID(loginUser);

            if (!IsPostBack)
            {
                BindGridviewData();

            }
        }

        protected void BindGridviewData()
        {
            grdView_ViewRejectedRequests.DataSource = dhl.GetRejectedRequest(deptId);
            grdView_ViewRejectedRequests.DataBind();
        }
        protected void grdView_ViewRejectedRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdView_ViewRejectedRequests.PageIndex = e.NewPageIndex;
            grdView_ViewRejectedRequests.DataSource = dhl.GetRejectedRequest(deptId);
            grdView_ViewRejectedRequests.DataBind(); //bind data
        }
        protected void grdView_ViewRejectedRequests_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_ViewRejectedRequests, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select pending request";

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }
        protected void grdView_ViewRejectedRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userRequestID = Convert.ToInt32(grdView_ViewRejectedRequests.SelectedRow.Cells[0].Text);
            using (SSISDbModelContext ctx = new SSISDbModelContext())
            {
                grdview_ViewRejectedRequestDetails.DataSource = dhl.GetRejectedRequestDetailByUserRequestID(userRequestID);
                grdview_ViewRejectedRequestDetails.DataBind();
            }
        }
    }
}