using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.VoucherDTO
{
    public class PublicVoucherDTO
    {
        public PublicVoucherDTO()
        { }

        public PublicVoucherDTO(VoucherModel voucher)
        {
            Id = voucher.Id;
            ExpirationDate = voucher.ExpirationDate;
        }
        public int Id { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}