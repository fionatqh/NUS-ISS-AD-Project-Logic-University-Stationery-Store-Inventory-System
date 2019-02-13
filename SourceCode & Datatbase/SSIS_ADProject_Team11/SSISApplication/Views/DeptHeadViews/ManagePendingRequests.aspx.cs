using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.DeptHeadViews
{
    public partial class ManagePendingRequests : System.Web.UI.Page
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
                btn_Approve.Visible = false;
                btn_Reject.Visible = false;
                txtbox_RejectComments.Visible = false;
                lbl_Reason.Visible = false;
            }
        }

        protected void BindGridviewData()
        {
            grdView_PendingRequests.DataSource = dhl.GetPendingRequest(deptId);
            grdView_PendingRequests.DataBind();
        }

        protected void grdView_PendingRequests_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // enable autopostback for first 3 columns onclick
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_PendingRequests, "Select$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_PendingRequests, "Select$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_PendingRequests, "Select$" + e.Row.RowIndex);
                // disable autopostback for column 4 to be able to type rejection remarks in
                // e.Row.Cells[3].Attributes["onclick"] = null;
                e.Row.ToolTip = "Click to select pending request";

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }
        protected void grdView_PendingRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
                int userRequestID = Convert.ToInt32(grdView_PendingRequests.SelectedRow.Cells[0].Text);
                using (SSISDbModelContext ctx = new SSISDbModelContext())
                {
                    grdView_RequestDetails.DataSource = dhl.GetPendingRequestDetailByUserRequestID(userRequestID);
                    grdView_RequestDetails.DataBind();
                }

            btn_Approve.Visible = true;
            btn_Reject.Visible = true;
            txtbox_RejectComments.Visible = true;
            lbl_Reason.Visible = true;
        }

        protected void btn_Approve_Click(object sender, EventArgs e)
        {
            try
            {
                int userRequestID = Convert.ToInt32(grdView_PendingRequests.SelectedRow.Cells[0].Text);
                dhl.ApproveRequest(userRequestID, loginUser);

                ScriptManager.RegisterStartupScript(this, this.GetType(),
                "alert", "alert('Request is approved.');window.location ='ManagePendingRequests.aspx';", true);
            }

            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                "alert", "alert('Please select a request.');window.location ='ManagePendingRequests.aspx';", true);
            }
        }

        protected void btn_Reject_Click(object sender, EventArgs e)
        {
            try
            {
                int userRequestID = Convert.ToInt32(grdView_PendingRequests.SelectedRow.Cells[0].Text);
                string comments = txtbox_RejectComments.Text;
                dhl.RejectRequest(userRequestID, loginUser, comments);
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                "alert", "alert('Request is rejected.');window.location ='ManagePendingRequests.aspx';",true);
            }

            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
               "alert", "alert('Please select a request.');window.location ='ManagePendingRequests.aspx';", true);
            }
        }
    }
}