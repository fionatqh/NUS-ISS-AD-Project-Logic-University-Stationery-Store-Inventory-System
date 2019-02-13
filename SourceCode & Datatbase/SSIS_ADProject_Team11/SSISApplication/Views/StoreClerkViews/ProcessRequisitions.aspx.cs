using SSISLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;

namespace SSISApplication.Views.StoreClerkViews
{
    public partial class ProcessRequisitions : System.Web.UI.Page
    {
        StoreClerkLibrary scl = new StoreClerkLibrary();
        CommonFunctionLibrary cf = new CommonFunctionLibrary();
        string loginUser = string.Empty;

        // BIND RADIOBUTTON with COLLECTION POINTS
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storeclerk")))
            {
                Response.Redirect("~/Account/Login");
            }

            loginUser = HttpContext.Current.User.Identity.Name;

            grdView_processRequ.Columns[7].Visible = false; // hide quantity text box
            btn_MakeRetrievalAndDisbursement.Visible = false;
            btn_RetrieveItems.Visible = false;
            if (!IsPostBack)
            {
                PopulateRadioButtonList(rdb_colpoint);
            }
        }

        public void PopulateRadioButtonList(System.Web.UI.WebControls.RadioButtonList rdb_colpoint)
        {
            rdb_colpoint.Items.Clear();

            List<string> listCollectionPoint = scl.CollectionPoints(loginUser);

            foreach (var item in listCollectionPoint)
            {
                rdb_colpoint.Items.Add(item);
                //rdb_colpoint.Items.Add(new ListItem() { Text = item.CollectionPointName, Value = item.CollectionPointName });
                rdb_colpoint.DataBind();
            }

        }
        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_processRequ, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Please do not retrieve more than the requested quantity";

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

                //    TextBox txt = new TextBox();
                //    txt = (TextBox)e.Row.FindControl("txt_qtyRetrieved");
                //    if (txt != null)
                //    {
                //        txt.OnTextChange += new OnChangeEventHandler();
                //    }
            }

        }


        // BIND grdView_processRequ WITH PENDING DEPT REQUESTS
        protected void showGridViewBasedOnColPt(object sender, EventArgs e) // OnSelectedIndexChanged
        {
            #region//ASP ON RADIO BUTTON CHECKED 
            btn_RetrieveItems.Visible = true;
            string colpoint = rdb_colpoint.SelectedItem.Value;
            BindGrid(colpoint);
            #endregion

            #region //HIGHLIGHT INSUFFICIENT INVENTORY WITH REQUESTED QTY
            int count = 0;
            int totalQtyReq = 0;

            //VALIDATION (if Inventory < Requested Qty && Total Requested Qty of multiple departments < Retrieved Qty) 

            for (int rowIndex = grdView_processRequ.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row_ = grdView_processRequ.Rows[rowIndex];
                //GridViewRow previousRow = grdView_processRequ.Rows[rowIndex-1];

                Label row_qtyRequested = (Label)row_.FindControl("lbl_qtyRequested");
                int rowQuantityRequested = Convert.ToInt32(row_qtyRequested.Text);
                //Label previousrow_qtyRequested = (Label)previousRow.FindControl("lbl_qtyRequested");
                //int previousRowQuantityRequested = Convert.ToInt32(previousrow_qtyRequested.Text);

                Label lbl_qtyInventory = (Label)row_.FindControl("lbl_qtyInventory");
                int qtyInv = Convert.ToInt32(lbl_qtyInventory.Text);
                Label txt_qtyRequested = (Label)row_.FindControl("lbl_qtyRequested");
                int qtyRequested = Convert.ToInt32(txt_qtyRequested.Text);

                //if (row_.Cells[0].Text != previousRow.Cells[0].Text && row_.Cells[4].Text != previousRow.Cells[4].Text)
                //{
                // sum Quantity cells
                totalQtyReq = rowQuantityRequested /*+ previousRowQuantityRequested*/;
                // Highlight red
                if (totalQtyReq > qtyInv)
                {
                    count++;
                    //previousRow.ForeColor = Color.Red;
                    row_.ForeColor = Color.Red;
                    //}
                }
            }
        }
        #endregion

        public void BindGrid(string colpoint)
        {
            DataTable dt = new DataTable();
            dt = scl.PendingDeptOrdersSql(colpoint);
            grdView_processRequ.DataSource = dt;
            // grdView_processRequ.DataSource = scl.PendingDeptOrders(colpoint);
            grdView_processRequ.DataBind();
        }

        // MERGING ON GRIDVIEW COLUMN
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
                        // merge InventoryID cells
                        row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                                previousRow.Cells[0].RowSpan + 1;
                        previousRow.Cells[0].Visible = false;
                        // merge ItemName cells
                        row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                                previousRow.Cells[1].RowSpan + 1;
                        previousRow.Cells[1].Visible = false;
                        // merge Quantity cells
                        row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                                previousRow.Cells[2].RowSpan + 1;
                        previousRow.Cells[2].Visible = false;
                        #region TESTING FOR INVENTORY VALIDATION
                        //if (row.Cells[4].Text != previousRow.Cells[4].Text)
                        //{
                        //    int countReq = Convert.ToInt32(row.Cells[5].Text) + Convert.ToInt32(previousRow.Cells[5].Text);
                        //}
                        #endregion

                        // merge UOM cells
                        row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                                previousRow.Cells[3].RowSpan + 1;
                        previousRow.Cells[3].Visible = false;
                    }

                }

                //// for (int i = 0; i < row.Cells.Count; i++)
                //   for (int i = 0; i < 4; i++) // merge only InventoryID and ItemName rows
                //       // if (!(row.Cells[i].Text.Any(char.IsDigit)) && row.Cells[i].Text == previousRow.Cells[i].Text)
                //       // problem with the merging! all cells with templatefield are read as equal! which means all columns with templatefield will be merged and take only the first cell value! char.IsDigit is not working!
                //       // to solve, must manually remove templatefield columns from the if block.
                //       if (i!=2 && i !=5 && i!=7)

            }
        }
        protected void gridView_PreRender(object sender, EventArgs e)
        {
            GridDecorator.MergeRows(grdView_processRequ);
        }

        // RETRIEVE BUTTON
        protected void btn_RetrieveItems_Click(object sender, EventArgs e)
        {
            grdView_processRequ.Columns[7].Visible = true;
            btn_MakeRetrievalAndDisbursement.Visible = true;
            btn_RetrieveItems.Visible = false;
            validateInventoryQtyAndUpdate();

            //List<string> departmentsInGridView = new List<string>();
            //for (int i = 0; i < grdView_processRequ.Rows.Count; i++)
            //{
            //    if (!departmentsInGridView.Contains(grdView_processRequ.Rows[i].Cells[4].Text))
            //    {
            //        departmentsInGridView.Add(grdView_processRequ.Rows[i].Cells[4].Text);
            //    }
            //}
            // shouldnt update here, should update after the validation
            //foreach (string dept in departmentsInGridView)
            //    scl.UpdateDeptRequestStatusTo2Processing(dept);
        }

        public void validateInventoryQtyAndUpdate()
        {
            int count = 0;
            int totalQtyReq = 0;
            #region VALIDATION (if Inventory < Requested Qty && Total Requested Qty of multiple departments < Retrieved Qty) 

            for (int rowIndex = grdView_processRequ.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row_ = grdView_processRequ.Rows[rowIndex];
                GridViewRow previousRow = grdView_processRequ.Rows[rowIndex + 1];

                Label row_qtyRequested = (Label)row_.FindControl("lbl_qtyRequested");
                int rowQuantityRequested = Convert.ToInt32(row_qtyRequested.Text);
                Label previousrow_qtyRequested = (Label)previousRow.FindControl("lbl_qtyRequested");
                int previousRowQuantityRequested = Convert.ToInt32(previousrow_qtyRequested.Text);

                Label lbl_qtyInventory = (Label)row_.FindControl("lbl_qtyInventory");
                int qtyInv = Convert.ToInt32(lbl_qtyInventory.Text);
                Label txt_qtyRequested = (Label)row_.FindControl("lbl_qtyRequested");
                int qtyRequested = Convert.ToInt32(txt_qtyRequested.Text);

                //COUNT THE ACCUMULATION OF EACH INVENTORY REQUESTED QTY FROM MULTIPLE DEPTS 
                if (row_.Cells[0].Text == previousRow.Cells[0].Text && row_.Cells[4].Text != previousRow.Cells[4].Text) // if (inventory top row = inventory row below it && dept top row != dept row below it)
                {
                    // sum Quantity cells
                    totalQtyReq = rowQuantityRequested + previousRowQuantityRequested;

                    // Highlight red
                    if (totalQtyReq > qtyInv)
                    {
                        count++;
                        previousRow.ForeColor = Color.Red;
                        row_.ForeColor = Color.Red;
                    }
                }
            }
            if (count > 0)
            {
                for (int rowIndex = grdView_processRequ.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                {
                    GridViewRow row_ = grdView_processRequ.Rows[rowIndex];
                    GridViewRow previousRow = grdView_processRequ.Rows[rowIndex + 1];

                    Label row_qtyRequested = (Label)row_.FindControl("lbl_qtyRequested");
                    int rowQuantityRequested = Convert.ToInt32(row_qtyRequested.Text);
                    Label previousrow_qtyRequested = (Label)previousRow.FindControl("lbl_qtyRequested");
                    int previousRowQuantityRequested = Convert.ToInt32(previousrow_qtyRequested.Text);

                    Label lbl_qtyInventory = (Label)row_.FindControl("lbl_qtyInventory");
                    int qtyInv = Convert.ToInt32(lbl_qtyInventory.Text);
                    Label txt_qtyRequested = (Label)row_.FindControl("lbl_qtyRequested");
                    int qtyRequested = Convert.ToInt32(txt_qtyRequested.Text);

                    if (row_.Cells[0].Text == previousRow.Cells[0].Text && row_.Cells[4].Text != previousRow.Cells[4].Text)
                    {
                        // sum Quantity cells
                        totalQtyReq = rowQuantityRequested + previousRowQuantityRequested;
                        // Check user input that are still exceeding requested ammount
                        if (totalQtyReq > rowQuantityRequested)
                        {
                            string display = "Inventory quantity is not sufficient";
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                            grdView_processRequ.Columns[7].Visible = true; //quantity retrieved
                            btn_RetrieveItems.Visible = true;
                            btn_MakeRetrievalAndDisbursement.Visible = false;
                        }
                        else if (totalQtyReq <= rowQuantityRequested)
                        {
                            previousRow.ForeColor = Color.Black;
                            row_.ForeColor = Color.Black;
                            grdView_processRequ.Columns[7].Visible = true; //quantity retrieved
                            btn_RetrieveItems.Visible = false;
                        }
                        else
                        {
                            #endregion
                            #region // SAVING TO DB PART
                            grdView_processRequ.Columns[7].Visible = true; // show retrieved quantity text box
                            btn_MakeRetrievalAndDisbursement.Visible = true;

                            for (int i = 0; i < grdView_processRequ.Rows.Count; i++)
                            {
                                string deptName = grdView_processRequ.Rows[i].Cells[4].Text; //cell for deptname
                                scl.UpdateDeptRequestStatusTo2Processing(deptName);
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        // SENDFORCOLLECTION BUTTON
        protected void btn_MakeRetrievalAndDisbursement_Click(object sender, EventArgs e)

        {
            int count = 0;
            int condition = 0;
            #region // validation
            foreach (GridViewRow row in grdView_processRequ.Rows)
            {
                TextBox txt_qtyRetrieved = (TextBox)row.FindControl("txt_qtyRetrieved");
                int qtyRetrieved = Convert.ToInt32(txt_qtyRetrieved.Text);
                Label lbl_qtyRequested = (Label)row.FindControl("lbl_qtyRequested");
                int qtyRequested = Convert.ToInt32(lbl_qtyRequested.Text);
                Label lbl_qtyInventory = (Label)row.FindControl("lbl_qtyInventory");
                int qtyInv = Convert.ToInt32(lbl_qtyInventory.Text);

                if (qtyRetrieved > qtyRequested) //condition 1
                {
                    count++;
                    condition = 1;
                    row.ForeColor = Color.Red;
                }
                if (qtyRetrieved > qtyInv) //condition 2
                {
                    count++;
                    condition = 2;
                    row.ForeColor = Color.Red;
                }
            }

            if (count > 0)
            {
                if (condition == 1)
                {
                    string display = "Retrieved Quantity must not exceed the Requested Quantity";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                    grdView_processRequ.Columns[7].Visible = true; //quantity retrieved
                    btn_RetrieveItems.Visible = true;
                }
                else
                {
                    string display = "Insufficient Inventory Quantity";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                    grdView_processRequ.Columns[7].Visible = true; //quantity retrieved
                    btn_RetrieveItems.Visible = true;
                }

            }
            else
            {
                #endregion

                int retrievalID = MakeRetrievalList(); // makes new retrieval, new retrieval details, and returns the new max retrievalId

                #region// get the unique departments and inventories in the GridView
                List<string> departmentsInGridView = new List<string>();
                List<string> inventoriesInGridView = new List<string>();
                for (int i = 0; i < grdView_processRequ.Rows.Count; i++)
                {
                    if (!departmentsInGridView.Contains(grdView_processRequ.Rows[i].Cells[4].Text))
                    {
                        departmentsInGridView.Add(grdView_processRequ.Rows[i].Cells[4].Text);
                    }
                    if (!inventoriesInGridView.Contains(grdView_processRequ.Rows[i].Cells[0].Text))
                    {
                        inventoriesInGridView.Add(grdView_processRequ.Rows[i].Cells[0].Text);
                    }
                }
                #endregion

                #region Make Disbursement
                foreach (string department in departmentsInGridView) // for every unique department, make a new Disbursement
                {
                    scl.MakeDisbursementFromProcessRequisitions(department, retrievalID); // makes disbursement only

                    // for every unique inventory item, sum up all quantity and make 1 disbursement detail for this item
                    foreach (string inventoryID in inventoriesInGridView)
                    {
                        int thisInventoryTotalQuantity = 0;

                        // loop through all gridview rows
                        for (int i = 0; i < grdView_processRequ.Rows.Count; i++)
                        {
                            GridViewRow row = grdView_processRequ.Rows[i];

                            if (row.Cells[4].Text == department && row.Cells[0].Text == inventoryID) // if this row is for this dept and this inventoryID, add this quantity to sum
                            {
                                TextBox quantitytextbox = (TextBox)row.Cells[7].FindControl("txt_qtyRetrieved");
                                thisInventoryTotalQuantity += Convert.ToInt32(quantitytextbox.Text);
                            }
                        }

                        // add disbursement detail with disbursementID = current maximum disbursementID
                        scl.saveDisbursmentDetails(Convert.ToInt32(inventoryID), thisInventoryTotalQuantity);
                    }


                    //GridViewRow row = grdView_processRequ.Rows[i];
                    //if (row.Cells[4].Text == department || row.Cells[0].Text == inventoryID)
                    //{
                    //    MakeDisbursementDetails(department, inventoryID);
                    //}

                    scl.UpdateDeptRequestStatusTo3RdyForCollection(department);

                    // show confirmation on screen
                    int disbursementID = scl.getMaxDisbursmentId();
                    lbl_ShowDisbursementID.Text += " " + disbursementID.ToString() + ",";
                }
                //Response.Redirect("~/Views/StoreClerkViews/ManageDisbursmentLists");
                Response.Redirect(Request.RawUrl);
                #endregion
            }
        }

        public int MakeRetrievalList()
        {
            DateTime dt = DateTime.Now;
            int retrievalid = scl.getMaxRetrievalId(loginUser, dt); // makes a new Retrieval and returns the new max ID. need to return this new max retrievalID to put into new Disbursement later on

            //Show Confirmation on Retrieval Creation Label
            // lbl_ShowDisbursementID.Text += " " + retrievalid.ToString() + ",";
            // lbl_ShowDisbursementID.Visible = true;

            // make retrieval details
            List<RetrievalDetail> newret = new List<RetrievalDetail>();

            for (int j = 0; j < grdView_processRequ.Rows.Count; j++)
            {
                GridViewRow row = grdView_processRequ.Rows[j];
                int inventoryID = Convert.ToInt32(row.Cells[0].Text);

                TextBox quantitytextbox = (TextBox)row.Cells[7].FindControl("txt_qtyRetrieved");

                if (j == 0 || grdView_processRequ.Rows[j].Cells[1].Text != grdView_processRequ.Rows[j - 1].Cells[1].Text) // compare item name
                {
                    scl.saveRetrievalDetails(retrievalid, inventoryID, Convert.ToInt32(quantitytextbox.Text)); // save new retrieval if this row's itemname != previous row's itemname
                }

                else if (grdView_processRequ.Rows[j].Cells[1].Text == grdView_processRequ.Rows[j - 1].Cells[1].Text) // compare item name
                {
                    scl.saveSumRetrievalDetails(retrievalid, inventoryID, Convert.ToInt32(quantitytextbox.Text)); // update new retrieval if this row's itemname != previous row's itemname
                }

                // Update DeptRequestStatus from "1" to "2" 
                string deptName = grdView_processRequ.Rows[j].Cells[4].Text;
                scl.UpdateDeptRequestStatusTo2Processing(deptName);
            }

            return retrievalid;
        }
    }
}

