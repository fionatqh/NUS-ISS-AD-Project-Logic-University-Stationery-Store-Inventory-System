using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.StoreSupervisorViews
{
    public partial class AdjustmentVouchers : System.Web.UI.Page
    {
        StoreSupervisorLibrary ssl = new StoreSupervisorLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storesupervisor") || Context.User.IsInRole("storemanager")))
            {
                Response.Redirect("~/Account/Login");
            }
        }

        // Click this button to view list of adjustment voucher
        protected void btn_HistoryAdjustment_Click(object sender, EventArgs e)
        {
            List<AdjustmentVoucher> listAdjust = ssl.GetAllAdjustmentVoucher();
            grdView_ListAdjustment.DataSource = ssl.GetAllAdjustmentVoucher();
            grdView_ListAdjustment.DataBind();
           
        }

        // this event handler makes the gridview rows clickable and returns the rowindex. 
        protected void grdView_ListAdjustment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_ListAdjustment, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select Adjustment Voucher";
                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        // Choose 1 row in grdView_ListAdjustment which we would like to show details of adjustment voucher
        protected void grdView_ListAdjustment_SelectedIndexChanged(object sender, EventArgs e)
        {
            int adjID = Convert.ToInt32(grdView_ListAdjustment.SelectedRow.Cells[0].Text);

            List<AdjustmentVoucherDetail> adjlist = ssl.GetAdjustmentVoucherDetail(adjID);
            grdView_DetailsAdjustment.DataSource = adjlist;
            grdView_DetailsAdjustment.DataBind();
            lbl_DetailsAdjustment.Visible = true;
        }
    }
}