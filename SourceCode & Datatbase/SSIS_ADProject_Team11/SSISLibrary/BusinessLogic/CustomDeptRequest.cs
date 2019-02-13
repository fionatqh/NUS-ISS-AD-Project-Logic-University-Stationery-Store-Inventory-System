using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSISLibrary
{
    public class CustomDeptRequest
    {
        public int InventoryID { get; set; }

        public string ItemName { get; set; }

        public int? Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        // public int SumOfDeptRequestQuantity { get; set; }

        public string DeptName { get; set; }

        public int DeptRequestQuantity { get; set; }

        public DateTime DeptRequestDate { get; set; }




        // inv.InventoryID, inv.ItemName, inv.Quantity, inv.UnitOfMeasure, dept.DeptName, drd.DeptRequestQuantity, dr.DeptRequestDate 
    }
}
