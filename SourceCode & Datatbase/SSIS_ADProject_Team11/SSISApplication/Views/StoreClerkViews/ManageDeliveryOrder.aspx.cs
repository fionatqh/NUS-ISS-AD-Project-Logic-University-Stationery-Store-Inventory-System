using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISWebSiteApplication.StoreClerkViews
{
    public partial class ManageDeliveryOrder : System.Web.UI.Page
    {
        StoreClerkLibrary scl = new StoreClerkLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storeclerk")))
            {
                Response.Redirect("~/Account/Login");
            }

            grdView_Delivery.DataSource = scl.GetAllPurchaseOrder();
            grdView_Delivery.DataBind();
           
        }
        protected void grdView_Delivery_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_Delivery, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select Delivery Order";

                
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }
        protected void grdView_Delivery_SelectedIndexChanged(object sender, EventArgs e)
        {

            int PurchaseOrderID = Convert.ToInt32(grdView_Delivery.SelectedRow.Cells[0].Text);
            using (SSISDbModelContext ctx = new SSISDbModelContext())
            {
                grdview_DeliveryOrderDetail.DataSource = scl.GetDeliveryOrderDetailByPurchaseOrderID(PurchaseOrderID);
                grdview_DeliveryOrderDetail.DataBind();
            }
        }
    }
}