using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.DeptHeadViews
{
    public partial class ModifyDepartmentDetails : System.Web.UI.Page
    {
        DeptHeadLibrary dhl = new DeptHeadLibrary();
        CommonFunctionLibrary cf = new CommonFunctionLibrary();
        int deptID = 0;
        string loginUser = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("depthead") || Context.User.IsInRole("delegatehead")))
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                loginUser = HttpContext.Current.User.Identity.Name;
                deptID = cf.GetDeptID(loginUser);

                rbl_CollPt.DataSource = dhl.GetAllCollectionPoint();
                rbl_CollPt.DataTextField = "CollectionPointName";
                rbl_CollPt.DataValueField = "CollectionPointID";
                rbl_CollPt.DataBind();

                ddl_Employee_Name2.DataSource = dhl.GetSSISList(deptID);
                ddl_Employee_Name2.DataBind();

                ddl_Employee_Name.DataSource = dhl.GetSSISList(deptID);
                ddl_Employee_Name.DataTextField = "PersonName";
                ddl_Employee_Name.DataValueField = "Email";
                ddl_Employee_Name.DataBind();

                ddl_Employee_Name2.DataSource = dhl.GetSSISList(deptID);
                ddl_Employee_Name2.DataTextField = "PersonName";
                ddl_Employee_Name2.DataValueField = "Email";
                ddl_Employee_Name2.DataBind();

                lbl_CurrentDeptRep.Text = dhl.GetCurrentDeptRep(deptID);
                lbl_CurrentColPt.Text = dhl.GetCurrentCollectionPoint(deptID);
                lbl_CurrentDeptHead.Text = dhl.GetCurrentDeptHead(deptID);

            }
        }

        public void StartDate_Selected(object sender, EventArgs e)
        {
            txt_StartDate.Text = cldr_StartDate.SelectedDate.ToShortDateString();
            cldr_StartDate.Visible = false;

        }
        public void Show_StartDate(object sender, EventArgs e)
        {
            if (cldr_StartDate.Visible == true)
            {
               cldr_StartDate.Visible = false;
            }
            else
                cldr_StartDate.Visible = true;
                txt_StartDate.Text = "";
        }

        public void EndDate_Selected(object sender, EventArgs e)
        {
            txt_EndDate.Text = cldr_EndDate.SelectedDate.ToShortDateString();
            cldr_EndDate.Visible = false;

        }
        public void Show_EndDate(object sender, EventArgs e)
        {
            if (cldr_StartDate.Visible == true)
            {
                cldr_StartDate.Visible = false;
            }
            else
            cldr_EndDate.Visible = true;
            txt_EndDate.Text = "";
        }

        protected void saveChanges_Click(object sender, EventArgs e)
        {

            loginUser = HttpContext.Current.User.Identity.Name;
            deptID = cf.GetDeptID(loginUser);
            string repEmail, delHeadEmail;

            //change dept rep

            repEmail = ddl_Employee_Name.SelectedItem.Value;
            dhl.ChangeDeptRep(deptID, repEmail, loginUser);

            //change collection point
            
            int collectionPt = 0;

            var hasSelection = (rbl_CollPt.SelectedItem != null);
            if (hasSelection) collectionPt = Convert.ToInt32(rbl_CollPt.SelectedItem.Value);
            if (hasSelection) dhl.ChangeCollectionPoint(deptID, collectionPt);

            //delegate employee
            
            delHeadEmail = ddl_Employee_Name2.SelectedItem.Value;
            DateTime startDate = cldr_StartDate.SelectedDate.Date;
            DateTime endDate = cldr_EndDate.SelectedDate.Date;

            if (endDate < startDate)
            {
                string display = "End date should be greater than start date.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + " ');", true);
            }

            else
            {
                dhl.AddDelegation(loginUser, delHeadEmail, startDate, endDate);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(),
               "alert", "alert('Changes saved.');window.location ='ManagePendingRequests.aspx';", true);
        }
    }
}