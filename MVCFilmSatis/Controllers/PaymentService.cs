using MVCFilmSatis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCFilmSatis.Controllers
{
    public abstract class PaymentService
    {
        public abstract bool MakePayment(IPaymentModel pm);
    }

    //concrete class : asıl işi yapana sınıf
    public class BankTransferService : PaymentService
    {
        public override bool MakePayment(IPaymentModel pm)
        {
            var info = (BankTransferPayment)pm;
            //1.Bankaya bağlanıp ödeme var mı kontrol et
            //2. Ödeme varsa true dön
            //3.Yoksa false dön
            return true;
        }
    }

    public class CreditCardService : PaymentService
    {
        public override bool MakePayment(IPaymentModel pm)
        {
            var info = (CreditCardPayment)pm;
            // 1. Kart Bilgileri geçerli mi ve tutar çekiliyor mu
            // 2. Ödeme alındıysa true dön
            // 3. Ödeme başarısız olursa false dön
            return true;
        }
    }
}