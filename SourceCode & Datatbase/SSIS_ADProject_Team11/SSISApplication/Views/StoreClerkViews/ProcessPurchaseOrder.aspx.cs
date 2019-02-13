using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.StoreClerkViews
{
    public partial class ProcessPurchaseOrder : System.Web.UI.Page
    {
        StoreClerkLibrary scl = new StoreClerkLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storeclerk")))
            {
                Response.Redirect("~/Account/Login");
            }
            //get the low quantity inventory (from View Inventory)
            var bind = scl.ViewInventoryBelowReorderAsListInventory();

            //bind low qty inv to data grid
            Session["LowStockList"] = bind;

            RemovePendingPurchases();
            grdView_lowStockItems.DataSource = (List<Inventory>)Session["LowStockListWithoutPending"];
            grdView_lowStockItems.DataBind();

            //bind suppliers name to dropdown boundfield
            //onrowdatabound attached to the ddl itself liao?

        }

        public void RemovePendingPurchases()
        {
            List<PurchaseOrder> pendingPurchaseOrders = scl.pendingPurchaseOrder();
            List<PurchaseOrderDetail> pendingPurchaseOrderDetails = new List<PurchaseOrderDetail>();

            foreach (var item in pendingPurchaseOrders)
            {
                List<PurchaseOrderDetail> thisPurchaseOrderDetails = scl.PendingPurchaseById(item.PurchaseOrderID);
                pendingPurchaseOrderDetails.AddRange(thisPurchaseOrderDetails);
            }

            List<Inventory> lowstocklist = (List<Inventory>)Session["LowStockList"];
            List<Inventory> listToRemove = new List<Inventory>();

            foreach (var item in pendingPurchaseOrderDetails)
            {
                listToRemove.AddRange(lowstocklist.Where(p => p.InventoryID == item.InventoryID).ToList());
                // Inventory inventory = ctx.Inventory.Where(p => p.InventoryID == item.InventoryID).FirstOrDefault();
            }
            List<Inventory> lowStockListWithoutPending = lowstocklist.Except(listToRemove).ToList();
            Session["LowStockListWithoutPending"] = lowStockListWithoutPending;
        }

        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //create a new dropdownlsit
                DropDownList ddl = new DropDownList();


                //bind the source and define the values
                List<String> suppliercodes = scl.getsuppliercodes(e.Row.Cells[0].Text);

                foreach (var item in suppliercodes)
                {
                    ddl.Items.Add(new ListItem { Text = item, Value = item });
                }


                //add the dropdownlist to the gridview in column 1
                PlaceHolder placeholder1 = (PlaceHolder)e.Row.FindControl("placeholder1");
                placeholder1.Controls.Add(ddl);
            }
        }

        protected void btn_Order_Click(object sender, EventArgs e)
        {
            MakePurchaseOrder();
            RemovePendingPurchases();
            Response.Redirect(Request.RawUrl);
        }

        public void MakePurchaseOrder()
        {
            List<String> selectedSuppliers = new List<String>();
            List<PlaceHolder> placeHolder = new List<PlaceHolder>();
            List<DropDownList> ddl = new List<DropDownList>();
            List<string> stringList = new List<string>();
            List<string> lowStockGetFromSupplier = new List<string>();

            for (int i = 0; i < grdView_lowStockItems.Rows.Count; i++)
            {
                placeHolder.Add((PlaceHolder)grdView_lowStockItems.Rows[i].Cells[5].FindControl("placeholder1"));
                foreach (Control item in placeHolder[i].Controls)
                {
                    if (item is DropDownList)
                    {
                        if (!selectedSuppliers.Contains(((DropDownList)item).Text))
                        {
                            ddl.Add((DropDownList)item);
                            selectedSuppliers.Add(ddl[i].SelectedValue);
                        }
                        lowStockGetFromSupplier.Add(((DropDownList)item).SelectedValue);
                    }
                }
                stringList.Add(grdView_lowStockItems.Rows[i].Cells[0].Text);
            }

            foreach (var supplier in selectedSuppliers)
            {
                int purchaseOrderID = scl.CreatePurchaseOrder(supplier);

                //PurchaseOrder purchaseOrder = new PurchaseOrder();
                //purchaseOrder.PurchaseOrderID = scl.GetMaxPurchaseOrderId() + 1;
                //purchaseOrder.SupplierID = ctx.Supplier.Where(p => p.SupplierCode == supplier).First().SupplierID;
                //purchaseOrder.PurchaseOrderDate = DateTime.Now;
                //purchaseOrder.DeliveryStatus = "Pending";
                //ctx.PurchaseOrder.Add(purchaseOrder);

                int counter = 0;
                foreach (var lowStockGetFromSupplierSupplierCode in lowStockGetFromSupplier)
                {
                    if (lowStockGetFromSupplierSupplierCode == supplier)
                    {
                        string itemname = stringList[counter];

                        scl.CreatePurchaseOrderDetails(purchaseOrderID, Convert.ToInt32(grdView_lowStockItems.Rows[counter].Cells[3].Text), itemname);

                        //PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
                        //purchaseOrderDetail.PurchaseOrderID = purchaseOrder.PurchaseOrderID;

                        //Inventory inventory = ctx.Inventory.Where(p => p.ItemName == itemname).First();
                        //purchaseOrderDetail.InventoryID = inventory.InventoryID;
                        //purchaseOrderDetail.Quantity = Convert.ToInt32(grdView_lowStockItems.Rows[counter].Cells[3].Text);

                        //ctx.PurchaseOrderDetail.Add(purchaseOrderDetail);
                    }
                    counter++;
                }
                //ctx.SaveChanges();
            }

        }
    }
}