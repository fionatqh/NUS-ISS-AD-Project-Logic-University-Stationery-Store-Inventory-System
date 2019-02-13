using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSISApplication.Views.StoreSupervisorViews
{
    public partial class ViewReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Context.User.IsInRole("storesupervisor") || Context.User.IsInRole("storemanager")))
            {
                Response.Redirect("~/Account/Login");
            }
        }

        protected void btn_departmentOrderReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CrystalReports/DepartmentOrdersReport.aspx");
        }

        protected void btn_Report_Click(object sender, EventArgs e)
        {

        }

        protected void btn_StoreItemReport_Click(object sender, EventArgs e)
        {

        }
    }
}