using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.BillDTO
{
    public class PublicBillDTO
    {
        public int Id { get; set; }
        public DateTime BillCreated { get; set; }

        public PublicBillDTO ()
        { }

        public PublicBillDTO(BillModel bill)
        {
            Id = bill.Id;
            BillCreated = bill.BillCreated;
        }
    }
}