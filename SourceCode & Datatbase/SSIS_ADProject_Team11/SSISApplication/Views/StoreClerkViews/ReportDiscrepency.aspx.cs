using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using SSISLibrary;

namespace SSISWebSiteApplication.StoreClerkViews
{
    public partial class ReportDiscrepency : System.Web.UI.Page
    {
        StoreClerkLibrary scl = new StoreClerkLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storeclerk")))
            {
                Response.Redirect("~/Account/Login");
            }
        }
        protected void grdView_Reportdiscrepency_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdView_Reportdiscrepency, "Select$" + e.Row.RowIndex);


                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");


                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            }
        }
        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            //NewEditIndex property used to determine the index of the row being edited.  
            grdView_Reportdiscrepency.EditIndex = e.NewEditIndex;
            //GridView1.DataBind();

        }
        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            
            //Finding the controls from Gridview for the row which is going to update  
            Label InventoryID = grdView_Reportdiscrepency.Rows[e.RowIndex].FindControl("InventoryID") as Label;
            Label Price = grdView_Reportdiscrepency.Rows[e.RowIndex].FindControl("lbl_Price") as Label;
            Label InventoryQuantity = grdView_Reportdiscrepency.Rows[e.RowIndex].FindControl("lbl_InventoryQuantity") as Label;
            TextBox ActualQuantity = grdView_Reportdiscrepency.Rows[e.RowIndex].FindControl("txt_lbl_ActualQuantity") as TextBox;
            TextBox DiscrepancyReason = grdView_Reportdiscrepency.Rows[e.RowIndex].FindControl("txt_DiscrepancyReason") as TextBox;

            SqlConnection con = new SqlConnection(@"Data Source=(local);Initial Catalog=SSIS;Integrated Security=True");
            con.Open();
            //updating the record  
            SqlCommand cmd = new SqlCommand("Update DiscrepancyDetail set ActualQuantity='" + ActualQuantity.Text + "',DiscrepancyReason='" + DiscrepancyReason.Text + "' where InventoryID=" + Convert.ToInt32(InventoryID.Text), con);
            cmd.ExecuteNonQuery();
            con.Close();
            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
            grdView_Reportdiscrepency.EditIndex = -1;
            //Call ShowData method for displaying updated data  
            grdView_Reportdiscrepency.DataBind();
        }

        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
            grdView_Reportdiscrepency.EditIndex = -1;
            grdView_Reportdiscrepency.DataBind();

        }

        protected void btn_Generate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CrystalReports/ViewDiscrepancyReport.aspx");
        }
    }
}