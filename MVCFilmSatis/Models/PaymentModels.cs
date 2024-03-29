﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCFilmSatis.Models
{

    public interface IPaymentModel
    {
        int Id { get; set; }
        string TC { get; set; }
        string NameSurname { get; set; }
        bool IsApproved { get; set; }
    }
    public class BankTransferPayment : IPaymentModel
    {
        public int Id { get; set; }
        public string TC { get; set; }
        public string NameSurname { get; set; }
        public bool IsApproved { get; set; }
        public decimal Total { get; set; }
        public virtual Order Order { get; set; }
    }
    public class CreditCardPayment : IPaymentModel
    {
        public int Id { get; set; }
        public string TC { get; set; }
        public string NameSurname { get; set; }
        //[NotMapped] : veritabanına bu alan aktarılmaz
        public string CartNumber { get; set; }
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }
        public short CV2 { get; set; }
        public bool IsApproved { get; set; }
        public decimal Total { get; set; }
        public virtual Order Order { get; set; }
    }
}