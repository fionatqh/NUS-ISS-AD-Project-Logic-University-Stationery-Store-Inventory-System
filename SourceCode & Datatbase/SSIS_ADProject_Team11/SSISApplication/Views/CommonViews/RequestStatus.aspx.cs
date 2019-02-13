using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.CommonViews
{
    public partial class RequestStatus : System.Web.UI.Page
    {
        EmployeeLibrary obj = new EmployeeLibrary();
        string userEmail = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("employee") || Context.User.IsInRole("deptrep") || Context.User.IsInRole("storeclerk") || Context.User.IsInRole("storesupervisor")))
            {
                Response.Redirect("~/Account/Login");
            }

            userEmail = HttpContext.Current.User.Identity.Name;
            if (!IsPostBack)
            {
                BindGridviewData();
                btn_save.Visible = false;
                btn_cancel.Visible = false;
                
            }
            lbl_Comment.Visible = false;
        }

        protected void BindGridviewData()
        {
            List<CustomUserRequestDetails> lst = new List<CustomUserRequestDetails>();

            lst = obj.GetUserRequest(userEmail).ToList();
            Gridview_Allrequestsandstatus.DataSource = lst;
            Gridview_Allrequestsandstatus.DataBind();

        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            sender = btn_search;
           
            DateTime startDate = Convert.ToDateTime(cldr_Startdate.SelectedDate.ToShortDateString());
            DateTime endDate = Convert.ToDateTime(cldr_Enddate.SelectedDate.ToShortDateString());
            if (endDate<startDate)
            {
                string display = "End date should be greather than the startdate";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }
            List<CustomUserRequestDetails> lst = new List<CustomUserRequestDetails>();

            lst = obj.GetUserRequest(startDate, endDate, userEmail).ToList();
            Gridview_Allrequestsandstatus.DataSource = lst;

            Gridview_Allrequestsandstatus.DataBind();

        }

        
        protected void Gridview_Allrequestsandstatus_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(Gridview_Allrequestsandstatus, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select request details";

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        protected void Gridview_Allrequestsandstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            int urId = Convert.ToInt32(Gridview_Allrequestsandstatus.SelectedRow.Cells[0].Text);
            GetRequestAndStatus lst = new GetRequestAndStatus();
            lst = obj.GetRequestAndStatus(urId);
            GridView_RequestDetails.DataSource = lst.RequestDataDetails;
            GridView_RequestDetails.DataBind();
            GridView_RequestDetails.Visible = true;
          

        }

        protected void GridView_RequestDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            TextBox txt = (TextBox)e.Row.FindControl("txt_requestquantity");


            if (e.Row.Cells[0].Text == "Approved")
            {
                //compare first column of current row value
                if (txt != null)
                {
                    btn_save.Visible = false;
                    btn_cancel.Visible = false;
                    txt.Attributes.Add("readonly", "readonly");
                    // txt.Attributes.Remove("readonly"); To remove readonly attribute
                }
            }
            if (e.Row.Cells[0].Text == "Pending")
            {
                btn_save.Visible = true;
                btn_cancel.Visible = true;
            }

        }


        protected void SaveButton_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView_RequestDetails.Rows)
            {
                Label userReq = (Label)row.FindControl("lbl_userrequestID");
                Label invenID = (Label)row.FindControl("lbl_inventoryID");
                TextBox reqQty = (TextBox)row.FindControl("txt_requestquantity");
                obj.ChangeRequestQuantity(Convert.ToInt32(userReq.Text), Convert.ToInt32(invenID.Text), Convert.ToInt32(reqQty.Text));
            }
            if (lbl_Comment.Visible == false)
            {
                lbl_Comment.Visible = true;
                lbl_Comment.Text = "Request quantities changed sucessfully";
            
            }
            else
            {
                lbl_Comment.Visible = false;
            }
            
            
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            BindGridviewData();
            GridView_RequestDetails.Visible = false;
            btn_save.Visible = false;
            btn_cancel.Visible = false;

        }
        public void StartDate_Selected(object sender, EventArgs e)
        {
            txt_startdate.Text = cldr_Startdate.SelectedDate.ToShortDateString();
            cldr_Startdate.Visible = false;

        }
        public void StartShow_Date(object sender, EventArgs e)
        {
            if (cldr_Startdate.Visible == true)
            {
                cldr_Startdate.Visible = false;
            }
            else
                cldr_Startdate.Visible = true;
            txt_startdate.Text = "";
        }
        public void EndDate_Selected(object sender, EventArgs e)
        {
            txt_enddate.Text = cldr_Enddate.SelectedDate.ToShortDateString();
            cldr_Enddate.Visible = false;

        }
        public void EndShow_Date(object sender, EventArgs e)
        {
            if (cldr_Enddate.Visible == true)
            {
                cldr_Enddate.Visible = false;
            }
            else
                cldr_Enddate.Visible = true;
            txt_enddate.Text = "";
        }
    }
}