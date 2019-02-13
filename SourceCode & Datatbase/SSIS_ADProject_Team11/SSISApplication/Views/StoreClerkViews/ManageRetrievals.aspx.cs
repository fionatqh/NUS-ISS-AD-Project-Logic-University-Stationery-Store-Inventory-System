using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;
using System.Data;
using System.Drawing;

namespace SSISApplication.Views.StoreClerkViews
{
    public partial class ManageRetrievals : System.Web.UI.Page
    {
        StoreClerkLibrary scl = new StoreClerkLibrary();
        string loginUser = string.Empty;
        CommonFunctionLibrary cfl = new CommonFunctionLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storeclerk")))
            {
                Response.Redirect("~/Account/Login");
            }

            loginUser = HttpContext.Current.User.Identity.Name;
            if (!IsPostBack)
            {
                //PopulateRadioButtonList(rdb_colpoint);
                string retriever = HttpContext.Current.User.Identity.Name;
                BindGridRetrieval(retriever);

                btn_EditQty.Visible = false;
                btn_Cancel.Visible = false;
                btn_SaveQty.Visible = false;
                // btn_SendForCollection.Visible = false;
                // btn_SaveDisbursement.Visible = false;
            }
        }

        public void BindGridRetrieval(string retriever)
        {
            DataTable dt = new DataTable();
            dt = scl.RetrievalList(retriever); // select * from Retrieval where RetrieverEmail = @Retriever
            grdView_retrieval.DataSource = dt;
            grdView_retrieval.DataBind();
        }

        protected void RowDataBound2(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_retrieval, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to view Retrieval details";

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        // MERGING DECORATOR
        public class GridDecorator
        {
            public static void MergeRows(GridView gridView)
            {
                for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                {
                    GridViewRow row = gridView.Rows[rowIndex];
                    GridViewRow previousRow = gridView.Rows[rowIndex + 1];


                    if (row.Cells[0].Text == previousRow.Cells[0].Text)
                    {
                        row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                               previousRow.Cells[0].RowSpan + 1;
                        previousRow.Cells[0].Visible = false;
                    }

                }
            }
        }
        protected void gridView_PreRender(object sender, EventArgs e)
        {
            GridDecorator.MergeRows(grdView_retrievalDetails);
        }



        protected void SelectedIndexChanged2(object sender, EventArgs e)
        {
            int retrievalID = Convert.ToInt32(grdView_retrieval.SelectedRow.Cells[0].Text); // get clicked row retrieval
            Session["SelectedRetrievalID"] = retrievalID; ;
            //string colpoint = rdb_colpoint.SelectedValue; // get selected radio button collection point
            grdView_retrievalDetails.DataSource = scl.GetRetrievalDetail(retrievalID); // get retrieval details
            Session["RetrievalDetailForSelectedRetrieval"] = scl.GetRetrievalDetail(retrievalID);

            List<Disbursement> DisbursedDisbList = scl.GetAllDisbursement();
           foreach (var item in DisbursedDisbList)
                // if the retrievalID in the disbursement has been disbursed, then cannot edit
                if (item.RetrievalID == retrievalID && item.DisbursementStatus == "Disbursed")
                {
                    string display = "This retrieval has been disbursed, quantity is no longer editable";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                    btn_SaveQty.Visible = false;
                    btn_Cancel.Visible = false;
                    btn_EditQty.Visible = false;
                    grdView_retrievalDetails.DataBind();
                    break;
                }
                else
                {
                    btn_SaveQty.Visible = false;
                    btn_Cancel.Visible = false;
                    btn_EditQty.Visible = true;
                    grdView_retrievalDetails.DataBind();
                }


            foreach (GridViewRow row in grdView_retrievalDetails.Rows)
            {
                TextBox qty = (TextBox)row.FindControl("txt_RetrievalQuantity");
                qty.BorderStyle = BorderStyle.None;
            }
        }

        protected void btn_EditQty_Click(object sender, EventArgs e)
        {
            //ALLOW USER TO EDIT THE QUANTITY
            foreach (GridViewRow row in grdView_retrievalDetails.Rows)
            {
                TextBox qty = (TextBox)row.FindControl("txt_RetrievalQuantity");
                qty.BorderStyle = BorderStyle.Groove;
                qty.BorderColor = Color.OrangeRed;
                qty.ReadOnly = false;

                btn_SaveQty.Visible = true;
                btn_EditQty.Visible = false;
                // btn_SendForCollection.Visible = false;
                btn_Cancel.Visible = true;
                // btn_SaveDisbursement.Visible = false;
            }

        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            //RESTRICT USER TO EDIT THE QUANTITY
            foreach (GridViewRow row in grdView_retrievalDetails.Rows)
            {
                TextBox qty = (TextBox)row.FindControl("txt_RetrievalQuantity");
                qty.BorderStyle = BorderStyle.None;
                qty.ReadOnly = true;

                btn_SaveQty.Visible = false;
                btn_EditQty.Visible = true;
                // btn_SendForCollection.Visible = true;
                btn_Cancel.Visible = false;
                // btn_SaveDisbursement.Visible = false;
            }
        }

        protected void btn_UpdateDisb_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/StoreClerkViews/ManageDisbursmentLists.aspx");
        }


        protected void btn_SaveQty_Click(object sender, EventArgs e) // saving editted retrieval form
        {

            List<Inventory> InventoryList = scl.ViewAllInventory();

            int count = 0;
            foreach (var item in InventoryList)
            {
                int total = 0;
                for (int i = 0; i < grdView_retrievalDetails.Rows.Count; i++)
                {
                    if (grdView_retrievalDetails.Rows[i].Cells[1].Text.ToString() == item.ItemNumber.ToString())
                    {
                        TextBox quantityTextBox = (TextBox)grdView_retrievalDetails.Rows[i].Cells[3].FindControl("txt_RetrievalQuantity");
                        int newRetrievedQty = Convert.ToInt32(quantityTextBox.Text);
                        total += newRetrievedQty;
                    }
                }

                if (total > item.Quantity)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                grdView_processRequ.Visible = false;
                string display = "Insufficient stock in inventory!";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }
            //List<int> disbIDListForThisRetrievalId = new List<int>();
            //if (!disbIDListForThisRetrievalId.Contains(scl.GetDisbursementIDFromRetrievalID(retrievalID).FirstOrDefault()))
            //{
            //    disbIDListForThisRetrievalId.AddRange(scl.GetDisbursementIDFromRetrievalID(retrievalID));
            //}
            //List<CustomDisbursementDetailForClerk> disbListForClerk = new List<CustomDisbursementDetailForClerk>();
            //foreach (var disbID in disbIDListForThisRetrievalId)
            //{
            //    disbListForClerk.Add(scl.GetDisbursementDetail(disbID));
            //}
            else
            {
                grdView_processRequ.Visible = true;
                int retrievalID = (int)Session["SelectedRetrievalID"];
                List<int> retrievalQuantities = scl.GetRetrievalQuantitiesByRetrievalID(retrievalID);

                for (int i = 0; i < grdView_retrievalDetails.Rows.Count; i++)
                {
                    // retrievalID = Convert.ToInt32(grdView_retrievalDetails.Rows[i].Cells[0].Text);
                    // int invID = Convert.ToInt32(grdView_retrievalDetails.Rows[i].Cells[1].Text);

                    string itemnumber = grdView_retrievalDetails.Rows[i].Cells[1].Text;

                    int invID = scl.GetInventoryIDFromItemNumber(itemnumber);

                    int oldRetrievalQuantity = retrievalQuantities[i];

                    TextBox quantityTextBox = (TextBox)grdView_retrievalDetails.Rows[i].Cells[3].FindControl("txt_RetrievalQuantity");
                    int newRetrievalQty = Convert.ToInt32(quantityTextBox.Text);
                    scl.UpdateRetrievalQuantity(retrievalID, invID, newRetrievalQty - oldRetrievalQuantity);

                    // make text box non-edittable
                    quantityTextBox.BorderStyle = BorderStyle.None;
                    quantityTextBox.ReadOnly = true;
                }

                grdView_processRequ.DataSource = scl.DisbursementDetailsFromRetrievalID(retrievalID);
                // grdView_processRequ.DataSource = scl.PendingDeptOrders(colpoint);
                grdView_processRequ.DataBind();
                grdView_processRequ.Columns[5].Visible = true;
                btn_SaveQty.Visible = false;
                btn_EditQty.Visible = false;
                btn_Cancel.Visible = false;
                lbl_disbursementNotif.Visible = true;
                btn_UpdateDisb.Visible = true;
            }
            // Response.Redirect(Request.RawUrl);           
        }











        //protected void btn_SaveDisbursement_Click(object sender, EventArgs e) // saving editted retrieval form
        //{
        //    int retrievalID = (int)Session["SelectedRetrievalID"];

        //    // VALIDATION: sum of Disbursement Quantities for this Retrieval must not exceed Retrieval Quantity
        //    //
        //    //


        //    List<CustomRetrievalDetailForClerk> retrievalDetailForSelectedRetreival = (List<CustomRetrievalDetailForClerk>)Session["RetrievalDetailForSelectedRetrieval"];

        //    int count = 0;
        //    foreach (var item in retrievalDetailForSelectedRetreival)
        //    {
        //        int totalQuantity = 0;
        //        for (int i = 0; i < grdView_processRequ.Rows.Count; i++)
        //        {
        //            if (grdView_processRequ.Rows[i].Cells[0].Text == item.ItemNumber.ToString())
        //            {
        //                TextBox quantityTextBox = (TextBox)grdView_processRequ.Rows[i].Cells[6].FindControl("txt_DisbursementQuantity");
        //                int newDisbursementQuantity = Convert.ToInt32(quantityTextBox.Text);
        //                totalQuantity += newDisbursementQuantity;
        //            }
        //        }

        //        if (totalQuantity != item.RetrievalQuantity)
        //        {
        //            count++;
        //        }
        //    }

        //    if (count != 0)
        //    {
        //        string display = "Total disbursement quantity must be equal to retrieval quantity at this step!";
        //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
        //    }

        //    else
        //    {
        //        // update Disbursement Details !!!!!!
        //        for (int i = 0; i < grdView_processRequ.Rows.Count; i++)
        //        {
        //            int invID = Convert.ToInt32(grdView_processRequ.Rows[i].Cells[0].Text);
        //            int disbID = Convert.ToInt32(grdView_processRequ.Rows[i].Cells[5].Text);
        //            TextBox quantityTextBox = (TextBox)grdView_processRequ.Rows[i].Cells[6].FindControl("txt_DisbursementQuantity");
        //            int newDisbursementQuantity = Convert.ToInt32(quantityTextBox.Text);
        //            scl.UpdateDisbursementQuantity(disbID, invID, newDisbursementQuantity);
        //        }
        //        Response.Redirect(Request.RawUrl);
        //    }

        //}


        protected void btn_SendForCollection_Click(object sender, EventArgs e)
        {
            //lbl_ShowDisbursementID.Visible = true;

            //#region // get the unique retrievalID and inventories in the GridView
            //List<int> retrievalsInGridView = new List<int>();
            //List<string> inventoriesInGridView = new List<string>();
            //for (int i = 0; i < grdView_retrievalDetails.Rows.Count; i++)
            //{
            //    if (!inventoriesInGridView.Contains(grdView_retrievalDetails.Rows[i].Cells[1].Text))
            //    {
            //        inventoriesInGridView.Add(grdView_retrievalDetails.Rows[i].Cells[1].Text);
            //    }
            //}
            //for (int i = 0; i < grdView_retrieval.Rows.Count; i++)
            //{
            //    if (!retrievalsInGridView.Contains(Convert.ToInt32(grdView_retrieval.Rows[i].Cells[0].Text))) //cell RetrievalID
            //    {
            //        retrievalsInGridView.Add(Convert.ToInt32(grdView_retrieval.Rows[i].Cells[0].Text));
            //    }
            //}
            //#endregion


            //foreach (int retrievalId in retrievalsInGridView) // for every retrieval, make a new Disbursement
            //{
            //    scl.MakeDisbursementFromManageRetrievals(retrievalId);

            //    // for every unique inventory item, sum up all quantity and make 1 disbursement detail for this item
            //    foreach (string inventoryID in inventoriesInGridView)
            //    {
            //        int thisInventoryTotalQuantity = 0;

            //        // loop through all gridviewdetails rows for inventoryID && gridview rows for retrievalID

            //        for (int i = 0, j = 0; i < grdView_retrievalDetails.Rows.Count && j < grdView_retrieval.Rows.Count; i++, j++)
            //        {
            //            GridViewRow row = grdView_retrievalDetails.Rows[i];
            //            GridViewRow row_ = grdView_retrieval.Rows[j];

            //            if (Convert.ToInt32(row_.Cells[1].Text) == retrievalId && row.Cells[1].Text == inventoryID)
            //            {
            //                TextBox quantitytextbox = (TextBox)row.Cells[6].FindControl("txt_RetrievalQuantity");
            //                thisInventoryTotalQuantity += Convert.ToInt32(quantitytextbox.Text);
            //            }
            //        }

            //        // add disbursement detail with disbursementID = current maximum disbursementID
            //        scl.saveDisbursmentDetails(Convert.ToInt32(inventoryID), thisInventoryTotalQuantity);
            //    }
            //    scl.UpdateDeptRequestStatusTo3RdyForCollectionFetchByRetrievalID(retrievalId);

            //    // show confirmation on screen
            //    int disbursementID = scl.getMaxDisbursmentId();
            //    lbl_ShowDisbursementID.Text += " " + disbursementID.ToString() + ",";
            //}
        }
    }
}