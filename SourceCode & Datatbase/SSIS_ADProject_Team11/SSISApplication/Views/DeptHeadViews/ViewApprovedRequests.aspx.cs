using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;
using System.Configuration;

namespace SSISApplication.Views.DeptHeadViews
{
    public partial class ViewApprovedRequests : System.Web.UI.Page
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
            grdView_ViewApprovedRequests.DataSource = dhl.GetApprovedRequest(deptId);
            grdView_ViewApprovedRequests.DataBind();
        }
        protected void grdView_ViewApprovedRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdView_ViewApprovedRequests.PageIndex = e.NewPageIndex;
            grdView_ViewApprovedRequests.DataSource = dhl.GetApprovedRequest(deptId);
            grdView_ViewApprovedRequests.DataBind(); //bind data
        }
        protected void grdView_ViewApprovedRequests_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_ViewApprovedRequests, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select pending request";

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }
        protected void grdView_ViewApprovedRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userRequestID = Convert.ToInt32(grdView_ViewApprovedRequests.SelectedRow.Cells[0].Text);
            using (SSISDbModelContext ctx = new SSISDbModelContext())
            {
                grdview_ViewApprovedRequestDetails.DataSource = dhl.GetApprovedRequestDetailByUserRequestID(userRequestID);
                grdview_ViewApprovedRequestDetails.DataBind();
            }
        }
    }

}