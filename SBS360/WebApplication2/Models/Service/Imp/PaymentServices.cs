using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.Service.Interface;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository paymentRepository;
        public PaymentServices(IPaymentRepository _paymentRepository)
        {
            paymentRepository = _paymentRepository;
        }


        public int CreateReceivable(PaymentReceivableViewModel receivable)
        {
            return paymentRepository.CreateReceivable(receivable);
        }

        public int CreatePayable(PaymentPayableViewModel payable)
        {
            return paymentRepository.CreatePayable(payable);
        }

        public int SaveReceivable(PaymentReceivableViewModel receivable)
        {
            return paymentRepository.SaveReceivable(receivable);
        }

        public int SavePayable(PaymentPayableViewModel payable)
        {
            return paymentRepository.SavePayable(payable);
        }

        public PaymentReceivableViewModel getReceivable(int pymtID)
        {
            return paymentRepository.getReceivable(pymtID);
        }

        public PaymentPayableViewModel getPayable(int pymtID)
        {
            return paymentRepository.getPayable(pymtID);
        }

        public List<PaymentReceivableViewModel> getAllReceivables()
        {
            return paymentRepository.getAllReceivables();
        }

        public List<PaymentPayableViewModel> getAllPayables()
        {
            return paymentRepository.getAllPayables();

        }

        public List<PaymentStatusViewModel> getAllPaymentStatus()
        {
            return paymentRepository.getAllPaymentStatus();
        }

        public int DeleteReceivable(int pymtID)
        {
            return paymentRepository.DeleteReceivable(pymtID);
        }

        public int DeletePayable(int pymtID)
        {
            return paymentRepository.DeletePayable(pymtID);
        }

        public decimal GetInvoiceAmount(string invno, string invfrom)
        {
            return paymentRepository.GetInvoiceAmount(invno, invfrom);
        }

    }
}