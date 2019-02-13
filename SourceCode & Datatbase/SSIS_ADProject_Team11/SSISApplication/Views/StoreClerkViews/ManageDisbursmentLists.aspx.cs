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
    public partial class ManageDisbursmentLists : System.Web.UI.Page
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
                PopulateRadioButtonList(rdb_colpoint);
                btn_EditQty.Visible = false;
                btn_SaveQty.Visible = false;
                btn_SendForCollection.Visible = false;
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

        public void showDisbursementGridBasedOnColPt(object sender, EventArgs e) // SHOW LIST OF DISBURSEMENTS
        {
            string colpoint = rdb_colpoint.SelectedItem.Value;
            BindGrid(colpoint);

        }

        public void BindGrid(string colpoint)
        {
            DataTable dt = new DataTable();
            dt = scl.PendingDisbursmentList(colpoint);
            grdView_disbursement.DataSource = dt;
            grdView_disbursement.DataBind();
            
        }

        protected void RowDataBound2(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_disbursement, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to view Disbursement details";

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }

        protected void SelectedIndexChanged2(object sender, EventArgs e)
        {
            int disbID = Convert.ToInt32(grdView_disbursement.SelectedRow.Cells[0].Text);
            Session["SelectedDisbursementID"] = disbID;
            grdView_disbursementDetails.DataSource = scl.GetDisbursementDetail(disbID);
            btn_SaveQty.Visible = false;
            btn_EditQty.Visible = true;
            grdView_disbursementDetails.DataBind();
            btn_SendForCollection.Visible = true;

            //VALIDATION HIGHLIGHT UNMATCHING QTY BETWEEN RETRIEVAL AND DISB
            
            int retrievalID = scl.GetRetrievalFromDisbursement(disbID).RetrievalID;
            List<RetrievalDetail> retrievalDetailList = scl.GetRetrievalDetailByRetrievalID(retrievalID);

            foreach (var item in retrievalDetailList)
            {
                int total = 0;
                for (int i = 0; i < grdView_disbursementDetails.Rows.Count; i++)
                {
                    if (grdView_disbursementDetails.Rows[i].Cells[1].Text.ToString() == item.InventoryID.ToString())
                    {
                        TextBox quantityTextBox = (TextBox)grdView_disbursementDetails.Rows[i].Cells[4].FindControl("TextBox2");
                        int newDisbQty = Convert.ToInt32(quantityTextBox.Text);
                        total += newDisbQty;

                        if (total > item.RetrievalQuantity)
                        {
                            quantityTextBox.ForeColor = Color.Red;
                            quantityTextBox.ToolTip = "Disbursement Quantity exceeds the retrieved quantity, please re-check & update!";
                            
                        }
                        else
                        {
                            quantityTextBox.ForeColor = Color.Black;
                        }
                    }
                }

                
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
            GridDecorator.MergeRows(grdView_disbursementDetails);
        }

        protected void btn_SaveQty_Click(object sender, EventArgs e) // update DisbursementDetail and update RetrievalDetail! 
        {
            int disbID = (int)Session["SelectedDisbursementID"];
            int retrievalID = scl.GetRetrievalFromDisbursement(disbID).RetrievalID;
            List<RetrievalDetail> retrievalDetailList = scl.GetRetrievalDetailByRetrievalID(retrievalID);

            int count = 0;
            foreach (var item in retrievalDetailList)
            {
                int total = 0;
                for (int i = 0; i < grdView_disbursementDetails.Rows.Count; i++)
                {
                    if (grdView_disbursementDetails.Rows[i].Cells[1].Text.ToString() == item.InventoryID.ToString())
                    {
                        TextBox quantityTextBox = (TextBox)grdView_disbursementDetails.Rows[i].Cells[4].FindControl("TextBox2");
                        int newDisbQty = Convert.ToInt32(quantityTextBox.Text);
                        total += newDisbQty;
                    }
                }

                if (total > item.RetrievalQuantity)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                string display = "Total disbursement quantity must be less than retrieval quantity! If you want to disburse more than retrieval quantity, please edit the Retrieval quantity in \"Manage Retrievals\".";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }


            else
            {

                for (int i = 0; i < grdView_disbursementDetails.Rows.Count; i++)
                {
                    int invID = Convert.ToInt32(grdView_disbursementDetails.Rows[i].Cells[1].Text);

                    TextBox quantityTextBox = (TextBox)grdView_disbursementDetails.Rows[i].Cells[4].FindControl("TextBox2");
                    int newDisbQty = Convert.ToInt32(quantityTextBox.Text);
                    // get old disbursement details for updating Retrieval
                    DisbursementDetail oldDisbursementDetail = scl.GetOneDisbursementDetail(disbID, invID);
                    int oldDisbursementQuantity = oldDisbursementDetail.DisbursementQuantity;
                    // must get old details before updating otherwise old details cannot be gotten anymore

                    scl.UpdateDisbursementQuantity(disbID, invID, newDisbQty); // update DisbursementDetail

                    // scl.UpdateRetrievalQuantity(retrievalID, invID, newDisbQty - oldDisbursementQuantity);
                    string display = "You have updated the disbursement.";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);

                }
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void btn_EditQty_Click(object sender, EventArgs e)
        {
            //ALLOW USER TO EDIT THE QUANTITY
            foreach (GridViewRow row in grdView_disbursementDetails.Rows)
            {
                TextBox qty = (TextBox)row.FindControl("TextBox2");
                qty.ReadOnly = false;
                qty.BorderStyle = BorderStyle.Groove;
                qty.BorderColor = Color.OrangeRed;
                btn_EditQty.Visible = false;
                btn_SaveQty.Visible = true;
                btn_Cancel.Visible = true;


            }
        }


        protected void btn_SendForCollection_Click(object sender, EventArgs e)
        {
            //VALIDATION
            int disbID = (int)Session["SelectedDisbursementID"];
            int retrievalID = scl.GetRetrievalFromDisbursement(disbID).RetrievalID;
            List<RetrievalDetail> retrievalDetailList = scl.GetRetrievalDetailByRetrievalID(retrievalID);

            int count = 0;
            foreach (var item in retrievalDetailList)
            {
                int total = 0;
                for (int i = 0; i < grdView_disbursementDetails.Rows.Count; i++)
                {
                    if (grdView_disbursementDetails.Rows[i].Cells[1].Text.ToString() == item.InventoryID.ToString())
                    {
                        TextBox quantityTextBox = (TextBox)grdView_disbursementDetails.Rows[i].Cells[4].FindControl("TextBox2");
                        int newDisbQty = Convert.ToInt32(quantityTextBox.Text);
                        total += newDisbQty;
                    }
                }

                if (total > item.RetrievalQuantity)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                string display = "Total disbursement quantity must be less than retrieval quantity! If you want to disburse more than retrieval quantity, please edit the Retrieval quantity in \"Manage Retrievals\".";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }
            else
            {
                int deptID = scl.GetDisbursement(disbID).DepartmentID;
                string toEmail = cfl.GetDeptRepEmail(deptID);
                string mailSubject = "Ready For Collection";
                string mailBody = "Your Disbursement with DisbursementID: " + disbID + " us now ready for collection.";
                cfl.SendEmail(loginUser, toEmail, @"123qwe!@#QWE", mailSubject, mailBody);

                string display = "Department Representative has been notified for collection.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }



        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdView_disbursementDetails.Rows)
            {
                TextBox qty = (TextBox)row.FindControl("TextBox2");
                qty.BorderStyle = BorderStyle.None;
                qty.ReadOnly = true;

                btn_SaveQty.Visible = false;
                btn_EditQty.Visible = true;
                btn_Cancel.Visible = false;
            }

        }
    }
}
