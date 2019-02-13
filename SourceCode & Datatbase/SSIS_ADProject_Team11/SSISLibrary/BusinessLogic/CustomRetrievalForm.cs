using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SSISLibrary
{
    public partial class RetrievalForm
    {
        //DeptRequest dr = new DeptRequest();
        //deptrequeststatus = "Approved";
        //deptrequestid
        //DeptRequestDate
        //DepartmentID

        public int DeptRequestID { get; set; }

        public int DepartmentID { get; set; }

        public DateTime DeptRequestDate { get; set; }

        public int DeptRequestStatus { get; set; }

        //DeptRequestDetail drd = new DeptRequestDetail();
        //InventoryID
        //DeptRequestID
        //DeptRequestQuantity

        public int InventoryID { get; set; }
        public int DeptRequestID_drd { get; set; }
        public int DeptRequestQuantity { get; set; }

        // Department dept = new Department();
        // CollectionPointID
        // DepartmentID
        // DeptName

        public int DepartmentID_dept { get; set; }
        public string DeptName { get; set; }
        public int CollectionPointID { get; set; }

        //Inventory inv = new Inventory();
        //itemname
        //inventoryid
        //UnitOfMeasure
        public int InventoryID_inv { get; set; }
        public string ItemName { get; set; }
        public string UnitOfMeasure { get; set; }

        //CollectionPoint cp = new CollectionPoint();
        //CollectionPointID
        //CollectionPointName

        public int CollectionPointID_cp { get; set; }
        public string CollectionPointName { get; set; }
    }
}
