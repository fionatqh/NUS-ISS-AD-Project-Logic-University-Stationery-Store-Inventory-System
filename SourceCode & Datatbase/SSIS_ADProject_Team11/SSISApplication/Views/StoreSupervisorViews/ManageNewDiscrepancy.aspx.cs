using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.StoreSupervisorViews
{
    public partial class ManageNewDiscrepancy : System.Web.UI.Page
    {
        StoreSupervisorLibrary ssl = new StoreSupervisorLibrary();
        CommonFunctionLibrary cfl = new CommonFunctionLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storesupervisor") || Context.User.IsInRole("storemanager")))
            {
                Response.Redirect("~/Account/Login");
            }

            lbl_ShowDetails.Visible = false;
            string login = HttpContext.Current.User.Identity.Name;
            int roleid= ssl.GetUserRoleID(login);
            if (roleid == 5)
            {
                btn_SendManager.Visible = false;
            }
            lbl_TotalAmount.Visible = false;
            lbl_SumAmount.Visible = false;

        }
        
       
        // Click this button to view list of new discrepancies
        protected void btn_CheckDiscrepancies_Click(object sender, EventArgs e)
        {
            List<Discrepancy> listDiscrepancy = ssl.GetPendingDiscrepancies();
            grdView_ListDiscrepancies.DataSource = listDiscrepancy;
            grdView_ListDiscrepancies.DataBind();
        }


        // this event handler makes the gridview rows clickable and returns the rowindex. 
        protected void grdView_ListDiscrepancies_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_ListDiscrepancies, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select discrepancy";
                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        // Choose 1 row in grdView_ListDiscrepancies which we would like to show details of discrepany
        protected void grdView_ListDiscrepancies_SelectedIndexChanged(object sender, EventArgs e)
        {
            int disID = Convert.ToInt32(grdView_ListDiscrepancies.SelectedRow.Cells[0].Text);
            List<DiscrepancyDetail> dd2list = ssl.GetDiscrepancyDetail(disID);
            grdView_DetailDiscrepancy.DataSource = dd2list;
            grdView_DetailDiscrepancy.DataBind();
            Session["listDiscrepancyDetail"] = dd2list;
            Session["grdView_ListDiscrepanciesSelected"] = disID;
            decimal sum1 = ssl.SumDiscrepancyAmount(dd2list);
            lbl_SumAmount.Text = " $" + sum1;
            lbl_TotalAmount.Visible = true;
            lbl_SumAmount.Visible = true;
            lbl_ShowDetails.Visible = true;
            if (sum1 < 250)
            {
                btn_IssueAdjustment.Visible = true;
            }
            else
            {
                btn_SendManager.Visible = true;
            }

        }



        // Click this button to show details of adjustment voucher will be created
        protected void btn_IssueAdjustment_Click(object sender, EventArgs e)    
        {
            List<DiscrepancyDetail> listDiscrepancyDetail = (List<DiscrepancyDetail>)Session["listDiscrepancyDetail"]; // get the discrepancydetaillist that is currently showing in the gridview

            List<AdjustmentVoucherDetail> listAdjustmentVoucherDetail = ssl.CreateAdjustmentVoucherDetail(listDiscrepancyDetail);

            grdView_DetailAdjustment.DataSource = listAdjustmentVoucherDetail;
            grdView_DetailAdjustment.DataBind();
            Session["listAdjustmentVoucherDetail"] = listAdjustmentVoucherDetail;
            btn_CreateAdjustment.Visible = true;
            lbl_ShowDetails.Visible = true;
            lbl_TotalAmount.Visible = true;
            lbl_SumAmount.Visible = true;
        }

        // Click this button to create and save new adjustment voucher in database
        protected void btn_CreateAdjustment_Click(object sender, EventArgs e)     // Create Button
        {

            int disID = Convert.ToInt32(Session["grdView_ListDiscrepanciesSelected"]);
            AdjustmentVoucher newAdjustmentVoucher = ssl.CreateAdjustmentVoucher(disID);
            if (newAdjustmentVoucher != null)
            {
                List<AdjustmentVoucherDetail> listAdjustmentVoucherDetail = (List<AdjustmentVoucherDetail>)Session["listAdjustmentVoucherDetail"];
                string loginUser = HttpContext.Current.User.Identity.Name;
                for (int i = 0; i < grdView_DetailAdjustment.Rows.Count; i++)
                {
                    foreach (var item in listAdjustmentVoucherDetail)
                    {
                        TextBox cmtTextBox = (TextBox)grdView_DetailAdjustment.Rows[i].Cells[3].FindControl("txt_AdjustedComment");
                        item.AdjustmentComments = cmtTextBox.Text;
                    }
                }
                ssl.SaveAdjustmentVoucherDetail(listAdjustmentVoucherDetail, loginUser);
            }

                grdView_ListDiscrepancies.DataSource = ssl.GetPendingDiscrepancies();
                grdView_ListDiscrepancies.DataBind();

                grdView_DetailDiscrepancy.DataSource = null;
                grdView_DetailDiscrepancy.DataBind();

                Session["listDiscrepancyDetail"] = null;
                Session["listAdjustmentVoucherDetail"] = null;
                grdView_DetailAdjustment.DataSource = null;
                grdView_DetailAdjustment.DataBind();
            
        }
        
    }
}