using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;



namespace SSISApplication.Views.DeptRepViews
{
    public partial class ViewDisbursments : System.Web.UI.Page
    {
        int DisbId, deptID;
        String loggedUser,deptReqStatus;
        
        DeptRepLibrary dr = new DeptRepLibrary();
        CommonFunctionLibrary cr = new CommonFunctionLibrary();

        
        protected void Page_Load(object sender, EventArgs e)
        {
            setDtls();
            if (!IsPostBack)
            {
                if (!(Context.User.IsInRole("deptrep") || Context.User.IsInRole("delegaterep")))
                {
                    Response.Redirect("~/Account/Login");
                }

                btn_ViewDisbList.Visible=false;  
                btn_Confirm0.Visible = false;
                //cldr_Startdate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //cldr_Enddate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
               

                //txt_startdate.Text = cldr_Startdate.SelectedDate.ToString();
                //txt_enddate.Text = cldr_Enddate.SelectedDate.ToString();

                //BindGridView();

            }
        }
        private void setDtls()
        {
            loggedUser = HttpContext.Current.User.Identity.Name;
            
                deptID = cr.GetDeptID(loggedUser);
            
        }

        private void BindGridView()
        {
            List<Disbursement> d = dr.GetDisbursementbyDate(cldr_Startdate.SelectedDate, cldr_Enddate.SelectedDate, deptID);
            
    
            grdView_ViewDisbursement.DataSource = d.ToList();
            grdView_ViewDisbursement.DataBind();
        }

        

        protected void grdView_ViewDisbursement_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisbId = Convert.ToInt32(grdView_ViewDisbursement.SelectedValue.ToString());
            Session["DisbId"] = DisbId;
            grdView_ViewDisbursementDetails.DataSource = dr.GetDisbursementDetail(DisbId);
            grdView_ViewDisbursementDetails.DataBind();
            btn_ViewDisbList.Visible = true;
            btn_Confirm0.Visible = true;
            grdView_ViewDisbursementDetails.Visible = true;
            deptReqStatus = dr.GetDeptDisbStatus(DisbId);
            if (deptReqStatus == "Disbursed") btn_Confirm0.Enabled = false;
            if (deptReqStatus == "Pending") btn_Confirm0.Enabled = true;
        }

        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            int i=dr.ConfirmDisbursement((int)Session["DisbId"], deptID, loggedUser);
            if (i == 1) lbl_Confirm.Text = "Disbursement Confirmed";
            if (i == 0) lbl_Confirm.Text = "Unable to confirm Disbursement";
            //Reflect changed status
            grdView_ViewDisbursement.DataSource = dr.GetDisbursementbyDate(cldr_Startdate.SelectedDate, cldr_Enddate.SelectedDate, deptID);
            grdView_ViewDisbursement.DataBind();
        }

        protected void btn_ViewDisbList_Click(object sender, EventArgs e)
        {
            grdView_ViewDisbursementDetails.Visible = false;
            btn_Confirm0.Visible = false;
            btn_ViewDisbList.Visible = false;
            grdView_ViewDisbursement.Visible = true;
            BindGridView();
        }

        public int GetReqQty(String ItemName)
        {
           
            return dr.getReqQuantity(ItemName);
                
        }

        public string GetCollectionName(string collId)
        {

            return dr.GetCollectionName(Convert.ToInt32(collId));
         

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

        protected void grdView_ViewDisbursement_Sorting(object sender, GridViewSortEventArgs e)
        {/*
            grdView_ViewDisbursement.DataSource = dr.GetDisbursementbyDate(cldr_Startdate.SelectedDate, cldr_Enddate.SelectedDate, deptID);

            System.Data.DataTable dataTable = grdView_ViewDisbursement.DataSource as System.Data.DataTable;
            List<Disbursement> data = grdView_ViewDisbursement.DataSource as List<Disbursement>;
            DataTable dt = grdView_ViewDisbursement.DataSource as DataTable;
            if (dt != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

                grdView_ViewDisbursement.DataSource = dataView;
                grdView_ViewDisbursement.DataBind();
            }*/
        }

        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }

            return newSortDirection;
        }

        protected void grdView_ViewDisbursement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdView_ViewDisbursement.PageIndex = e.NewPageIndex;
            grdView_ViewDisbursement.DataSource = dr.GetDisbursementbyDate(cldr_Startdate.SelectedDate, cldr_Enddate.SelectedDate, deptID);//get datasource (list or datatable)
            grdView_ViewDisbursement.DataBind(); //bind data
        }

        

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            txt_enddate.Text = cldr_Enddate.SelectedDate.ToShortDateString();
            txt_startdate.Text = cldr_Startdate.SelectedDate.ToShortDateString();


            DateTime startDate = Convert.ToDateTime(cldr_Startdate.SelectedDate.ToShortDateString());
            DateTime endDate = Convert.ToDateTime(cldr_Enddate.SelectedDate.ToShortDateString());
            //cmpVal1.Validate();
            if (startDate > endDate)
            {
                string display = "End date should be greather than the startdate";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }
            //if (cmpVal1.IsValid)
            {
                grdView_ViewDisbursement.DataSource = dr.GetDisbursementbyDate(startDate,endDate,deptID);
                grdView_ViewDisbursement.DataBind();
           }

        }

        

       

       


    }
}