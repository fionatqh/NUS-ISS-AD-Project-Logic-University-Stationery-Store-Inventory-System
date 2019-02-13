using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.CommonViews
{
    public partial class RequestStationary : System.Web.UI.Page
    {

        //public List<NewRequestQuantityDetails> RequestQuantityDetailsList = new List<NewRequestQuantityDetails>();

        EmployeeLibrary obj = new EmployeeLibrary();
        CommonFunctionLibrary cf = new CommonFunctionLibrary();
        int deptId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("employee") || Context.User.IsInRole("deptrep") || Context.User.IsInRole("storeclerk") || Context.User.IsInRole("storesupervisor")))
            {
                Response.Redirect("~/Account/Login");
            }
            if (!IsPostBack)
            {
                BindGridviewData();
            }
            deptId = cf.GetDeptID(HttpContext.Current.User.Identity.Name);
        }

        //To get all inventory
        protected void BindGridviewData()
        {

            List<Inventory> lst = new List<Inventory>();
            lst = obj.GetInventory();

            Gridview_GetInventory.DataSource = lst;

            Gridview_GetInventory.DataBind();
        }

        //TO request stationery
        protected void RequestButton_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (GridViewRow row1 in Gridview_GetInventory.Rows)
            {
                
                TextBox qty = (TextBox)row1.FindControl("txt_quantity");


                if (string.IsNullOrWhiteSpace(qty.Text))
                {
                    count++;
                }
            }
            if (count >= 89)
            {


                string display = "No items are selected to request stationary.Inorder to place stationery order enter quantity";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                //Console.WriteLine(display);

            }
            else
            {
                NewRequestMaster aa = new NewRequestMaster
                {

                    Email = System.Web.HttpContext.Current.User.Identity.Name,
                    RequestDate = System.DateTime.Now,
                    RequestStatusDate = System.DateTime.Now,
                    RequestStatus = 1,
                    RequestStatusComment = null
                };

                List<NewRequestQuantityDetails> lst = new List<NewRequestQuantityDetails>();

                foreach (GridViewRow row in Gridview_GetInventory.Rows)
                {

                    NewRequestQuantityDetails objNQD = new NewRequestQuantityDetails();
                    Label invenID = row.FindControl("lbl_InventoryID") as Label;
                    TextBox reqqty = row.FindControl("txt_quantity") as TextBox;
                    if (!string.IsNullOrWhiteSpace(reqqty.Text))
                    {
                        int rQty = Convert.ToInt32(reqqty.Text);
                        objNQD.RequestQuantity = rQty;
                    }

                    objNQD.InventoryID = Convert.ToInt32(invenID.Text);
                    if (!string.IsNullOrWhiteSpace(reqqty.Text))
                    {
                        lst.Add(objNQD);
                    }
                }


                NewRequest ob = new NewRequest
                {
                    NewRequestMaster = aa,
                    NewRequestQuantityDetails = lst
                };

                obj.InsertRequest(ob, deptId);
                lbl_Comment.Text = "Request processed sucessfully";
               
               
            }

        }

        //To get totals of all the items wrt the quantity entered 
        protected void GetTotals_Click(object sender, EventArgs e)
        {

            //int sum = 0;
            foreach (GridViewRow row in Gridview_GetInventory.Rows)
            {
               
                TextBox qty = (TextBox)row.FindControl("txt_quantity");


                if (!string.IsNullOrWhiteSpace(qty.Text))
                {
                    Label price = (Label)row.FindControl("lbl_price");

                    Label totalAmt = (Label)row.FindControl("lbl_TotalAmount");
                    totalAmt.Text = ((Convert.ToDecimal(price.Text)) * (Convert.ToInt32(qty.Text))).ToString();
                    //int amt = int.Parse(totalAmt.Text);
                    //sum = ((sum) + (amt));
                }
            }

            //lbl_totalamount.Text = sum.ToString();



        }

    }   
}


     
