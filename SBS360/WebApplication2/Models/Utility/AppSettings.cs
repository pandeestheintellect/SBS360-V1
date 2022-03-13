using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace Eng360Web.Models.Utility
{
    public static class AppSettings
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static string DateTimeFormat = "DateTimeFormat";
        private static string MAIL_SETTINGS_SECTION = "system.net/mailSettings";
        private static string DEFAULT_FROM_MAIL_ADDRESS = ConfigurationManager.AppSettings["defaultEmail"];
        private static string WEB_CONFIG_FILE = "~\\Web.config";

        private static string DEFAULT_FROM = ConfigurationManager.AppSettings["defaultFrom"];



        public static string GetDateFormat()
        {
            return ConfigurationManager.AppSettings[DateTimeFormat];
        }

        public static void sendEmail(string subject, string body, string toEmailAddress, string ccEmailAddress, string attachments, byte[] pdfStream)
        {

            MailMessage mail = new MailMessage();

            try
            {

                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.UTF8;

                var mConfigurationFile = WebConfigurationManager.OpenWebConfiguration(WEB_CONFIG_FILE);
                MailSettingsSectionGroup mMailSettings = mConfigurationFile.GetSectionGroup(MAIL_SETTINGS_SECTION) as MailSettingsSectionGroup;

                MemoryStream ms = new MemoryStream(pdfStream);
                // Email attachment
                Attachment emailAttachment = new Attachment(ms, new System.Net.Mime.ContentType("application/pdf"));
                emailAttachment.ContentDisposition.FileName = subject + "_" + DateTime.Now.ToString("yyyyMMdd");


                //set the addresses
                if (mMailSettings != null)
                {
                    mail.From = new MailAddress(mMailSettings.Smtp.From, DEFAULT_FROM);
                }
                else
                {
                    mail.From = new MailAddress(DEFAULT_FROM_MAIL_ADDRESS, DEFAULT_FROM);
                }
                mail.To.Add(toEmailAddress);
                

                string[] CCId = ccEmailAddress.Split(',');

                foreach (string CCEmail in CCId)
                {
                    mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                }

                string[] AttId = attachments.Split(',');

                //foreach (string attach in AttId)
                //{
                //    if(attach != "" && attach != null)
                //    mail.Attachments.Add(new Attachment(attach)); //Adding Multiple Attachements
                //}

                mail.Attachments.Add(emailAttachment); //Adding Multiple Attachements

                mail.Subject = subject;
                
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(body, null, "text/plain");
                mail.AlternateViews.Add(plainView);

                using (SmtpClient client = new SmtpClient())
                {
                     var emailuser = ConfigurationManager.AppSettings["sEmailuser"];
                     var emailpass = ConfigurationManager.AppSettings["sEmailpass"];

                    client.Credentials = new System.Net.NetworkCredential(emailuser, emailpass);
                    client.EnableSsl = true;
                    
                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                
                logger.Error("Error occured in SendMail():");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                throw ex;
                
                
            }
            finally
            {
                mail.Dispose();
            }
        }


        public static MemoryStream ConvertHTMLToPDF(string htmlContent)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    using (var doc = new iTextSharp.text.Document(PageSize.A4.Rotate()))
                    {
                        
                        //Create a writer that's bound to our PDF abstraction and our stream
                        using (var writer = PdfWriter.GetInstance(doc, ms))
                        {
                            //Open the document for writing
                            doc.Open();

                            //Create a new HTMLWorker bound to our document
                            //using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                            using (var htmlWorker = new HtmlWorkerExtended(doc))
                            {
                                writer.CloseStream = false;
                                //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                                using (var sr = new StringReader(htmlContent))
                                {
                                    //Parse the HTML
                                    htmlWorker.Parse(sr);
                                }
                            }
                            doc.Close();
                        }
                    }
                    return ms;
                }

            }
            catch (Exception excep)
            {
                logger.Error(excep.StackTrace + "\n\n" + excep.Message);
                return null;
            }

        }

        public static void getWorkTimeHours(decimal nwh, decimal wwh, decimal lbh, string startTime, string endTime, bool wehFlag, bool lbFlag, out decimal workHours, out decimal otHours)
        {
            var normal_work_hours = nwh;
            var weekend_work_hours = wwh;
            var lunch_break_hours = lbh;
            decimal normal_ot_work_hours = normal_work_hours;
            decimal weekend_ot_work_hours = weekend_work_hours;

            TimeSpan workhours = Convert.ToDateTime(endTime) - Convert.ToDateTime(startTime);

            var totWorkHours = Convert.ToDecimal(workhours.TotalHours);

            if(wehFlag)
            {
                normal_work_hours = wwh;

            }

            if(totWorkHours <= normal_work_hours)
            {
                workHours = totWorkHours;
                otHours = 0;
            }
            else
            {
                if (lbFlag)
                {
                    workHours = normal_work_hours;
                    otHours = totWorkHours - normal_work_hours - lunch_break_hours;
                   
                }
                else
                {
                    workHours = normal_work_hours;
                    otHours = totWorkHours - normal_work_hours;
                }

            }

            //if (wehFlag)
            //{

            //    if (totWorkHours >= weekend_work_hours)
            //    {
            //        workHours = weekend_work_hours;
            //        otHours = totWorkHours - weekend_ot_work_hours;
            //        if (otHours < 0)
            //            otHours = 0;
            //    }
            //    else
            //    {
            //        workHours = totWorkHours;
            //        otHours = 0;
            //    }


            //}
            //else
            //{
            //    if (totWorkHours >= normal_work_hours)
            //    {
            //        workHours = normal_work_hours;
            //        otHours = totWorkHours - normal_ot_work_hours;
            //        if (otHours < 0)
            //            otHours = 0;
            //    }
            //    else
            //    {
            //        workHours = totWorkHours;
            //        otHours = 0;
            //    }

            //}

        }
        

        public static string NumberToCurrencyText(decimal number, MidpointRounding midpointRounding)
        {
            // Round the value just in case the decimal value is longer than two digits
            number = decimal.Round(number, 2, midpointRounding);

            string wordNumber = string.Empty;

            // Divide the number into the whole and fractional part strings
            string[] arrNumber = number.ToString().Split('.');

            // Get the whole number text
            long wholePart = long.Parse(arrNumber[0]);
            string strWholePart = NumberToText(wholePart);

            // For amounts of zero dollars show 'No Dollars...' instead of 'Zero Dollars...'
            //wordNumber = (wholePart == 0 ? "No" : strWholePart) + (wholePart == 1 ? " Singapore Dollar and " : " Singapore Dollars and ");
           // wordNumber = (wholePart == 0 ? "No" : strWholePart) + (wholePart == 1 ? " and " : " and ");

            wordNumber = (wholePart == 0 ? "No" : strWholePart) + (wholePart == 1 ? " " : " "); ;

            // If the array has more than one element then there is a fractional part otherwise there isn't
            // just add 'No Cents' to the end
            if (arrNumber.Length > 1)
            {
                // If the length of the fractional element is only 1, add a 0 so that the text returned isn't,
                // 'One', 'Two', etc but 'Ten', 'Twenty', etc.
                long fractionPart = long.Parse((arrNumber[1].Length == 1 ? arrNumber[1] + "0" : arrNumber[1]));
                string strFarctionPart = NumberToText(fractionPart);
                //wordNumber += (fractionPart == 0 ? " No " : strFarctionPart) + (fractionPart == 1 ? " Cent" : " Cents");

                if (fractionPart != 0)
                wordNumber += " and " + (fractionPart == 0 ? "" : strFarctionPart) + (fractionPart == 1 ? " Cent" : " Cents");
            }
            else
                wordNumber += "";
            //wordNumber += "No Cents";

            return wordNumber;
        }


        public static string NumberToText(long number)
        {
            StringBuilder wordNumber = new StringBuilder();

            string[] powers = new string[] { "Thousand ", "Million ", "Billion " };
            string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] ones = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                                       "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

            if (number == 0) { return "Zero"; }
            if (number < 0)
            {
                wordNumber.Append("Negative ");
                number = -number;
            }

            long[] groupedNumber = new long[] { 0, 0, 0, 0 };
            int groupIndex = 0;

            while (number > 0)
            {
                groupedNumber[groupIndex++] = number % 1000;
                number /= 1000;
            }

            for (int i = 3; i >= 0; i--)
            {
                long group = groupedNumber[i];

                if (group >= 100)
                {
                    wordNumber.Append(ones[group / 100 - 1] + " Hundred ");
                    group %= 100;

                    if (group == 0 && i > 0)
                        wordNumber.Append(powers[i - 1]);
                }

                if (group >= 20)
                {
                    if ((group % 10) != 0)
                        wordNumber.Append(tens[group / 10 - 2] + " " + ones[group % 10 - 1] + " ");
                    else
                        wordNumber.Append(tens[group / 10 - 2] + " ");
                }
                else if (group > 0)
                    wordNumber.Append(ones[group - 1] + " ");

                if (group != 0 && i > 0)
                    wordNumber.Append(powers[i - 1]);
            }

            return wordNumber.ToString().Trim();
        }
    }


}

    

    public class HtmlWorkerExtended : HTMLWorker
    {
        public HtmlWorkerExtended(IDocListener document)
            : base(document)
        {

        }
        public override void StartElement(string tag, IDictionary<string, string> str)
        {
            if (tag.Equals("newpage"))
                document.Add(Chunk.NEXTPAGE);
            else
            {
                if (!tag.ToLower().Trim().Equals("hr"))
                    base.StartElement(tag, str);
            }

        }

    }


