using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSISLibrary;

namespace SSISApplication.Views.DeptRepViews
{
    public partial class ModifyCollectionPoint : System.Web.UI.Page
    {
        DeptRepLibrary dr = new DeptRepLibrary();
        CommonFunctionLibrary cr = new CommonFunctionLibrary();

        private string currentUser = System.Web.HttpContext.Current.User.Identity.Name;

        protected void Page_Load(object sender, EventArgs e)

        {
            if (!(Context.User.IsInRole("deptrep") || Context.User.IsInRole("delegaterep") || Context.User.IsInRole("depthead") || Context.User.IsInRole("delegatehead")))
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                rdb_CollectionPoints.DataSource = dr.GetAllCollectionPoint();
                rdb_CollectionPoints.DataTextField = "CollectionPointName";
                rdb_CollectionPoints.DataValueField = "CollectionPointID";
                rdb_CollectionPoints.DataBind();
                for (int i = 0; i < 6; i++)
                {
                    rdb_CollectionPoints.Items[i].Attributes.Add("timings", "9.30-10.30");

                }

                selectCurrentColl();

            }

        }

        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            int COLLPOINT = 0;
            int i = 0;
            int deptID = cr.GetDeptID(currentUser);

            var hasSelection = (rdb_CollectionPoints.SelectedItem != null);
            if (hasSelection) COLLPOINT = Convert.ToInt32(rdb_CollectionPoints.SelectedItem.Value);
            i = dr.ChangeCollectionPoint(deptID, COLLPOINT);

            if (i == 1)
            {
                lbl_Status.Text = "Department Collection point successfully changed to " + dr.currentCollectionPointName(COLLPOINT);
            }
            else
            {
                lbl_Status.Text = "Department Collection point unable to change";
            }
            selectCurrentColl();

        }

        protected void selectCurrentColl()
        {
            int i = 0;
            ListItem srtItem = null;
            String collptid;
            int deptID = cr.GetDeptID(currentUser);
            collptid = dr.currentCollectionPoint(deptID).ToString();
            srtItem = rdb_CollectionPoints.Items.FindByValue(collptid);
            if (srtItem != null)
            {
                //srtItem.Enabled = false;
                rdb_CollectionPoints.Items.FindByValue(collptid).Selected = true;
            }
        }
    

            
            

        protected void rdb_CollectionPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            //selectCurrentColl();
        }
    }
}