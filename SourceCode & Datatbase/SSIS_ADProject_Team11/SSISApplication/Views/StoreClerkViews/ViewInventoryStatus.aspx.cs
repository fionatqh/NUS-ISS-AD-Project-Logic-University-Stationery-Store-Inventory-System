using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.StoreClerkViews
{
    public partial class ViewInventoryStatus : System.Web.UI.Page
    {
        StoreClerkLibrary scl = new StoreClerkLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storeclerk")))
            {
                Response.Redirect("~/Account/Login");
            }

            BindGridviewData(grdView_inventory);
        }

        // BINDING VIEW INVENTORY GRIDVIEW
        public void BindGridviewData(System.Web.UI.WebControls.GridView gridview1)
        {
            //ItemNumber, ItemName, UOM, ReOrder Lvl, Qty

            List<Inventory> lst = new List<Inventory>();

            lst = scl.ViewAllInventory();

            gridview1.DataSource = lst.Select(o => new
            { ItemNumber = o.ItemNumber, ItemName = o.ItemName, UOM = o.UnitOfMeasure, ReorderLvl = o.ReorderLevel, CurrentQuantity = o.Quantity }).ToList(); ;
            gridview1.DataBind();
        }
        protected void grdView_inventory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToInt32(e.Row.Cells[4].Text) < Convert.ToInt32(e.Row.Cells[3].Text))
                {
                    e.Row.Cells[4].ForeColor = Color.Red;
                }

            }
        }
        protected void btn_PurchaseOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProcessPurchaseOrder.aspx");
        }
    }
}