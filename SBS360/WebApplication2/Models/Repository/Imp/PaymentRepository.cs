using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eng360Web.Models.Domain;
using AutoMapper;
using System.Data.Entity;
using Eng360Web.Models.Utility;

namespace Eng360Web.Models.Repository.Interface
{
    public class PaymentRepository : IPaymentRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

       public int CreateReceivable(PaymentReceivableViewModel receivable)
        {

           var _db_receivable = Mapper.Map<eng_pymt_receivable>(receivable);

           Eng360DB.eng_pymt_receivable.Add(_db_receivable);
           return Eng360DB.SaveChanges();
        }

        public int CreatePayable(PaymentPayableViewModel payable)
        {

            var _db_payable = Mapper.Map<eng_pymt_payable>(payable);

            Eng360DB.eng_pymt_payable.Add(_db_payable);
            return Eng360DB.SaveChanges();
        }

        public int SaveReceivable(PaymentReceivableViewModel receivable)
        {

            var _db_receivable = Mapper.Map<eng_pymt_receivable>(receivable);
            Eng360DB.Entry(_db_receivable).State = EntityState.Modified;
 
            return Eng360DB.SaveChanges();

        }

        public int SavePayable(PaymentPayableViewModel payable)
        {

            var _db_payable = Mapper.Map<eng_pymt_payable>(payable);
            Eng360DB.Entry(_db_payable).State = EntityState.Modified;

            return Eng360DB.SaveChanges();

        }

        public PaymentReceivableViewModel getReceivable(int pymtID)
        {
            eng_pymt_receivable eng_pymt_receivable = Eng360DB.eng_pymt_receivable.Find(pymtID);

            return Mapper.Map<PaymentReceivableViewModel>(eng_pymt_receivable);
        }

        public PaymentPayableViewModel getPayable(int pymtID)
        {
            eng_pymt_payable eng_pymt_payable = Eng360DB.eng_pymt_payable.Find(pymtID);

            return Mapper.Map<PaymentPayableViewModel>(eng_pymt_payable);
        }

        public decimal GetInvoiceAmount(string invno, string invfrom)
        {
            decimal invAmt = 0;
            if (invfrom == "Quotation")
            {
                var det = Eng360DB.eng_quote_master.Where(a => a.InvoiceNo == invno).ToList();
                invAmt = (decimal)det.FirstOrDefault().FinalAmount;
            }
            if (invfrom == "Project")
            {
                var det = Eng360DB.eng_project_master.Where(a => a.InvoiceNo == invno).ToList();
                invAmt = (decimal)det.FirstOrDefault().Project_Cost;

            }
            if (invfrom == "Custom")
            {
                var det = Eng360DB.eng_custom_invoice.Where(a => a.InvoiceNo == invno).ToList();
                invAmt = (decimal)det.FirstOrDefault().FinalInvoiceAmount;

            }

            return invAmt;

        }


        public List<PaymentReceivableViewModel> getAllReceivables()
        {
            var eng_pymt_receivable = Eng360DB.eng_pymt_receivable.ToList();
            var lstReceivableView = Mapper.Map<List<PaymentReceivableViewModel>>(eng_pymt_receivable);
            return lstReceivableView;
        }

        public List<PaymentPayableViewModel> getAllPayables()
        {
            var eng_pymt_payable = Eng360DB.eng_pymt_payable.ToList();
            var lstPayableView = Mapper.Map<List<PaymentPayableViewModel>>(eng_pymt_payable);
            return lstPayableView;
        }

        public List<PaymentStatusViewModel> getAllPaymentStatus()
        {
            var eng_sys_pymt_status  = Eng360DB.eng_sys_pymt_status.ToList();
            var lstPymtStatusView = Mapper.Map<List<PaymentStatusViewModel>>(eng_sys_pymt_status);
            return lstPymtStatusView;
        }

        public int DeleteReceivable(int pymtID)
        {

            var _db_receivable = Eng360DB.eng_pymt_receivable.First(a => a.PymtID == pymtID);

            Eng360DB.eng_pymt_receivable.Remove(_db_receivable);

            return Eng360DB.SaveChanges();

        }

        public int DeletePayable(int pymtID)
        {

            var _db_payable = Eng360DB.eng_pymt_payable.First(a => a.PymtID == pymtID);

            Eng360DB.eng_pymt_payable.Remove(_db_payable);

            return Eng360DB.SaveChanges();

        }

    }
}