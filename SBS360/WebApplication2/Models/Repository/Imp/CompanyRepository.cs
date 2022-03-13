using AutoMapper;
using Eng360Web.Models.Domain;
using Eng360Web.Models.Repository.Interface;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public class CompanyRepository : ICompanyRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
        Logger logger = LogManager.GetCurrentClassLogger();

        public CompanyViewModel getCompany()
        {

            var company = Eng360DB.eng_company.FirstOrDefault();

            if (company == null)
            {
                CompanyViewModel cmp = new CompanyViewModel();
                return cmp;
            }
            else
            {
                CompanyViewModel cmp = new CompanyViewModel()
                {
                    CompanyID = company.CompanyID,
                    CompanyName = company.CompanyName,
                    Auth_InvoiceName = company.Auth_InvoiceName,
                    InvoiceTerms = company.InvoiceTerms,
                    City = company.City,
                    Address1 = company.Address1,
                    Address2 = company.Address2,
                    Pincode = company.Pincode,
                    Tel = company.Tel,
                    Fax = company.Fax,
                    Email = company.Email,
                    RegNo = company.RegNo,
                    GstRegNo = company.GstRegNo,
                    LogoPath = company.LogoPath,
                    Country = company.Country,
                    Normal_Work_Hours = (decimal) company.Normal_Work_Hours,
                    Weekend_Work_Hours = (decimal) company.Weekend_Work_Hours,
                    Lunch_Break_Hours = (decimal) company. Lunch_Break_Hours,
                    GST = (decimal) company.GST



                };
                return cmp;
            }
            
       }


        public int CreateAndUpdate(CompanyViewModel company)
        {

            if (company.CompanyID != 0)
            {
                var domainCompany = Eng360DB.eng_company.Find(company.CompanyID);

                    domainCompany.CompanyName = company.CompanyName;

                domainCompany.Auth_InvoiceName = company.Auth_InvoiceName;
                domainCompany.InvoiceTerms = company.InvoiceTerms;
                domainCompany.Address1 = company.Address1;
                domainCompany.Address2 = company.Address2;
                domainCompany.City = company.City;
                domainCompany.Country = company.Country;
                domainCompany.Pincode = company.Pincode;
                domainCompany.Tel = company.Tel;
                domainCompany.Fax = company.Fax;
                domainCompany.Email = company.Email;
                domainCompany.RegNo = company.RegNo;
                domainCompany.GstRegNo = company.GstRegNo;
                domainCompany.LogoPath = company.LogoPath;
                domainCompany.Normal_Work_Hours = company.Normal_Work_Hours;
                domainCompany.Weekend_Work_Hours = company.Weekend_Work_Hours;
                domainCompany.Lunch_Break_Hours = company.Lunch_Break_Hours;
                domainCompany.GST = company.GST;


                Eng360DB.Entry(domainCompany).State = System.Data.Entity.EntityState.Modified;
                return Eng360DB.SaveChanges();
                    }
            else
            {
                var domainCompany = new eng_company();
                          


               domainCompany.CompanyName = company.CompanyName;

                domainCompany.Auth_InvoiceName = company.Auth_InvoiceName;
                domainCompany.InvoiceTerms = company.InvoiceTerms;
                domainCompany.Address1 = company.Address1;
                domainCompany.Address2 = company.Address2;
                domainCompany.City = company.City;
                domainCompany.Country = company.Country;
                domainCompany.Pincode = company.Pincode;
                domainCompany.Tel = company.Tel;
                domainCompany.Fax = company.Fax;
                domainCompany.Email = company.Email;
                domainCompany.RegNo = company.RegNo;
                domainCompany.GstRegNo = company.GstRegNo;
                domainCompany.LogoPath = company.LogoPath;
                domainCompany.Normal_Work_Hours = company.Normal_Work_Hours;
                domainCompany.Weekend_Work_Hours = company.Weekend_Work_Hours;
                domainCompany.Lunch_Break_Hours = company.Lunch_Break_Hours;
                domainCompany.GST = company.GST;


                Eng360DB.eng_company.Add(domainCompany);
                return Eng360DB.SaveChanges();

            }

            return -1;



        }
    }
}