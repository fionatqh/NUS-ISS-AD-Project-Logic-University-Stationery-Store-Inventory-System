using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SSISLibrary
{
    public class CustomPendingRequest
    {
        public int UserRequestID { get; set; }

        public DateTime RequestDate { get; set; }

        public string PersonName { get; set; }
    }
    
}
