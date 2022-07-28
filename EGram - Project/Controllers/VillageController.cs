using E_GramProject.Bussiness_Logic;
using E_GramProject.Models;
using EGram.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EGram.Controllers
{
    public class VillageController : Controller
    {
        // GET: Village
        [Authorize]
        public ActionResult Index(Schema Sceme)
        {
            using (var db = new Entities())
            {

                //var query = from b in db.VillageConfigs
                //            orderby b.ID
                //            select b;

                //Console.WriteLine("All All student in the database:");

                //foreach (var item in query)
                //{
                //    Village vilobj=new Village
                //    {
                //        ID = item.ID,   
                //        NameofVillage = item.NameofVillage,
                //        Description   = item.Description,
                //        SirpanchName = item.SirpanchName,   
                //        Latitude = item.Latitude,   
                //        Longitude = item.Longitude
                //    };
                //}
                var schemaquery = from b in db.Schemata
                                  select b;

                foreach (var item in schemaquery)
                {
                    Schema vilobj = new Schema
                    {
                        Description = item.Description,
                        SchemaName = item.SchemaName,
                        SchemeID = item.SchemeID,
                        Condition = item.Condition,
                        URL = item.URL
                    };
                }

                if (Sceme != null && Sceme.SchemaName != null)
                {
                    if (ModelState.IsValid)
                    {

                        db.Schemata.Add(new Schema
                        {
                            SchemeID = Sceme.SchemeID,
                            SchemaName = Sceme.SchemaName,
                            Condition = Sceme.Condition,
                            Description = Sceme.Description,
                            URL = Sceme.URL
                        });
                        db.SaveChanges();
                        return RedirectToAction("Index");


                    }
                }

            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult UpdateSchema(int? id)
        {
            int number = Convert.ToInt16(id);
            Schema schemaObj = null;
            using (var db = new Entities())
            {

                var schemaquery = from b in db.Schemata
                                  where b.SchemeID == number
                                  select b;

                foreach (var item in schemaquery)
                {
                    schemaObj = new Schema
                    {
                        Description = item.Description,
                        SchemaName = item.SchemaName,
                        SchemeID = item.SchemeID,
                        Condition = item.Condition,
                        URL = item.URL
                    };
                }
            }
            return View(schemaObj);
        }

        // POST: PersonalDetails/Edit/5
        // To protect from overposting attacks, please enable the specific
        //properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult UpdateSchema(Schema model, HttpPostedFileBase file)
        {
            // It will redirect to 
            // the Read method
            //Use Namespace called :  System.IO  
            string FileName = Path.GetFileNameWithoutExtension(file.FileName);

            //To Get File Extension  
            string FileExtension = Path.GetExtension(file.FileName);

            //Add Current Date To Attached File Name  
            FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

            //Get Upload path from Web.Config file AppSettings.  
            string UploadPath = @"D:\nilam\EgramPanchayatV2\EGram - Copy\ImagePath\Schema\";
            //Its Create complete path to store in server.  
            model.ImagePath = UploadPath + FileName;

            using (var context = new Entities())
            {
                var data = context.Schemata.FirstOrDefault(x => x.SchemeID == model.SchemeID);
                if (data != null)
                {
                    data.SchemaName = model.SchemaName;
                    data.Description = model.Description;
                    data.Condition = model.Condition;
                    data.Condition = model.Condition;
                    data.ImagePath = FileName;

                    context.SaveChanges();
                    ViewBag.Message = "Update is successfully completed";

                    //To copy and save file into server.  
                    file.SaveAs(model.ImagePath);
                    return View("UpdateSchema", model);
                }
                else
                    return View();
            }

        }

       
        public ActionResult SchemaPage()
        {
            List<Schema> schemas = new List<Schema>();
            using (var db = new Entities())
            {
                var schemaquery = from b in db.Schemata
                                  select b;

                foreach (var item in schemaquery)
                {
                    Schema vilobj = new Schema
                    {
                        Description = item.Description,
                        SchemaName = item.SchemaName,
                        SchemeID = item.SchemeID,
                        Condition = item.Condition,
                        URL = item.URL
                    };
                    schemas.Add(vilobj);
                }
            }

            return View("ScemaSettings", schemas);
        }
        
        public FileResult downloadschematemplate(string path)
        {
            return null;
        }
        public string RetrieveSchemebyid(dynamic id)
        {
            int number = Convert.ToInt16(id[0]);
            Schema schemaObj = null;
            using (var db = new Entities())
            {

                var schemaquery = from b in db.Schemata
                                  where b.SchemeID == number
                                  select b;

                foreach (var item in schemaquery)
                {
                    schemaObj = new Schema
                    {
                        Description = item.Description,
                        SchemaName = item.SchemaName,
                        SchemeID = item.SchemeID,
                        Condition = item.Condition,
                        URL = item.URL,
                        ImagePath = item.ImagePath
                    };
                }
            }
            string schemaObjstr = JsonConvert.SerializeObject(schemaObj);
            return schemaObjstr;
        }
    
      /////////////////////////////////////////////////////////////
        //Complaint module started.
       public ActionResult complaint()
        {
            return View("Complaint");
        }
        public string GetCustomerDetails(string id,string onlycustomer)
        {
           
            Customer CustomerObj = null;
            using (var db = new Entities())
            {
               
                var customerquery = from b in db.Customers
                                  where b.MobilePhone == id
                                  select b;

                foreach (var item in customerquery)
                {
                    CustomerObj = new Customer
                    {
                        Salutation = item.Salutation,   
                        FirstName = item.FirstName,
                        LastName    =   item.LastName,
                        MobilePhone=item.MobilePhone,
                        Email=item.Email,
                        GUID = item.GUID,
                    };
                }
                if(CustomerObj!=null && (onlycustomer==null || !onlycustomer.Equals("true")))
                {
                    string otp = Generate_otp();
                    CustomerObj.OTP = otp;
                    string mobileNo = CustomerObj.MobilePhone;
                    string SMSContents = "", smsResult = "", emailResult = "";
                    SMSContents = otp + " is your One-Time Password, valid for 10 minutes only, Please do not share your OTP with anyone.";
                    //smsResult = SendSMS(mobileNo, SMSContents);
                    emailResult = sendEmail(CustomerObj.Email, SMSContents);

                    db.Messages.Add(new Message
                    {
                        MessageType = "OTP System",
                        IsEmailRequired = true,
                        EmailID = CustomerObj.Email,
                        IsSMSRequired = true,
                        MobileNumber = CustomerObj.MobilePhone,
                        MessageContent = SMSContents,
                        OTPText = otp,
                       MessageID=emailResult,
                       CreatedOn=DateTime.Now
                    });
                    db.SaveChanges();
                }
                    
                   
            }
            string schemaObjstr = JsonConvert.SerializeObject(CustomerObj);
            return schemaObjstr;
        }

        public String ValidateOTP(String otp,String mobileNumber)
        {
            if (otp == null)
                return "true";
            using (var db = new Entities())
            {
                DateTime dtbefore20 = DateTime.Now.AddMinutes(-20);
                Message messageObject=new Message();
                var messageQuery = from b in db.Messages
                                  where (b.MobileNumber == mobileNumber
                                  && b.OTPText == otp && b.CreatedOn >= dtbefore20)
                                  select b;

                foreach (var item in messageQuery)
                {
                    messageObject = new Message
                    {
                     MobileNumber=mobileNumber 
                    };
                }
                if(messageObject != null && messageObject.MobileNumber!=null)
                {
                    return "true";
                }
            }
            return "";
        }
        private string sendEmail(string email, string sMSContents)
        {
            //Create OTPMessage in CRM
            CustomerCurd otpobj = new CustomerCurd();
            return otpobj.createOTPmessagesinCRM(email, sMSContents);

            
        }



        // Start OTP Generation function
        [NonAction]
        protected string Generate_otp()
        {
            char[] charArr = "0123456789".ToCharArray();
            string strrandom = string.Empty;
            Random objran = new Random();
            for (int i = 0; i < 4; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);
                if (!strrandom.Contains(charArr.GetValue(pos).ToString())) strrandom += charArr.GetValue(pos);
                else i--;
            }
            return strrandom;
        }

        // End Send OTP code on button click
        // SMS Sending function
        public static string SendSMS(string MblNo, string Msg)
        {
           
            
            try
            {
          
                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "NmQ2NjY5NGM0MTZkNDI2OTc2NTk3ODVhNDEzMjU1N2E="},
                {"numbers" , "9767548791"},
                {"message" , "This is OTP"},
                {"sender" , "TXTLCL"}
                });
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    return result;
                }

            }
            catch (Exception e)
            {
               
                return "Error " + e;
            }
        }
        // End SMS Sending function
        // Get Response function
        public static string GetResponse(string smsURL)
        {
            try
            {
                WebClient objWebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(objWebClient.OpenRead(smsURL));
                string ResultHTML = reader.ReadToEnd();
                return ResultHTML;
            }
            catch (Exception)
            {
                return "Fail";
            }
        }

        [HttpPost]
        public ActionResult CreateDispute(Complaint complaint, HttpPostedFileBase file)
        {
            // It will redirect to 
            // the Read method
            //Use Namespace called :  System.IO  
            string FileName = String.Empty;
            if (file != null)
            {
                FileName = Path.GetFileNameWithoutExtension(file.FileName);

                //To Get File Extension  
                string FileExtension = Path.GetExtension(file.FileName);

                //Add Current Date To Attached File Name  
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
            }
            Random rand = new Random();
            int nextNum = rand.Next();
            using (var db = new Entities())
            {
                db.Complaints.Add(new Complaint
                {
                    Complaint_Number = nextNum,
                    Raised_By = complaint.Raised_By,
                    RaisedByUniqueID = complaint.RaisedByUniqueID.Split(':')[0],
                    Description = complaint.Description,
                    Subject = complaint.Subject,
                    Status = "Active",
                    AgainstParty = complaint.AgainstParty,
                    CreatedOn = DateTime.Now.ToString(),
                    FileName = complaint.FileName
                });

                if (file != null)
                {
                    //Get Upload path from Web.Config file AppSettings.  
                    string UploadPath = @"D:\nilam\EgramPanchayatV2\EGram - Copy\ImagePath\Complaint\" + nextNum + "\\";
                    //Its Create complete path to store in server.  
                    complaint.FileName = UploadPath + FileName;

                    if (!Directory.Exists(UploadPath))
                    {
                        Directory.CreateDirectory(UploadPath);

                    }
                }
                db.SaveChanges();
                if(file!=null)
                    file.SaveAs(complaint.FileName);
            }
        
           string d="call";
           return View("UpdateDispute",complaint.Complaint_Number);
        }
        // End Get Response function

        [HttpGet]
        public ActionResult GetAllDisputes(string OTP,string MobileNumber,string CustomerID)
        {
            if(ValidateOTP(OTP, MobileNumber)=="true")
            {
                List<Complaint> listComp = new List<Complaint>();
                using (var db = new Entities())
                {
                    if (MobileNumber != null)
                    {
                        // List<Complaint> complaintQuery = new List<Complaint>();
                        var complaintQuery = from b in db.Complaints
                                             where b.RaisedByUniqueID == MobileNumber
                                             select b;
                        foreach (var item in complaintQuery)
                        {
                            listComp.Add(new Complaint
                            {
                                AgainstParty = item.AgainstParty,
                                Complaint_Number = item.Complaint_Number,
                                CreatedOn = item.CreatedOn,
                                Reviewbycomitee = item.Reviewbycomitee,
                                Status = item.Status
                            });
                        }
                        return View("GetCases",listComp);
                    }
                    else
                    {
                        var complaintQuery = from b in db.Complaints
                                           //  where b.RaisedByUniqueID == CustomerID
                                             select b;
                        foreach (var item in complaintQuery)
                        {
                            listComp.Add(new Complaint
                            {
                                AgainstParty = item.AgainstParty,
                                Complaint_Number = item.Complaint_Number,
                                CreatedOn = item.CreatedOn,
                                Description = item.Description,
                                FileName = item.FileName,
                                ImageFile = item.ImageFile,
                                Meeting = item.Meeting,
                                RaisedByUniqueID = item.RaisedByUniqueID,
                                Raised_By = item.Raised_By,
                                Reviewbycomitee = item.Reviewbycomitee,
                                Status = item.Status,
                                Subject = item.Subject
                            });
                        }
                    }
                }
                return View(listComp);
            }
            return null;
        }


       
        [HttpGet]
        public ActionResult UpdateDispute(int? id)
        {
            int number = Convert.ToInt32(id);
            Complaint schemaObj = null;
            using (var db = new Entities())
            {

                var schemaquery = from b in db.Complaints
                                  where b.Complaint_Number == number
                                  select b;

                foreach (var item in schemaquery)
                {
                    schemaObj = new Complaint
                    {
                        Description = item.Description,
                        AgainstParty = item.AgainstParty,
                        Complaint_Number = item.Complaint_Number,
                        CreatedOn = item.CreatedOn,
                        FileName = item.FileName,
                        ImageFile=item.ImageFile,
                        Meeting     =item.Meeting,
                        RaisedByUniqueID    =item.RaisedByUniqueID,
                        Raised_By    =item.Raised_By,
                        Reviewbycomitee=    item.Reviewbycomitee,
                        Status=item.Status,
                        Subject=item.Subject
                    };
                }
            }
            return View("UpdateDispute1",schemaObj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult UpdateDispute(Complaint model, HttpPostedFileBase file)
        {
            string FileName = Path.GetFileNameWithoutExtension(file.FileName);
            //To Get File Extension  
            string FileExtension = Path.GetExtension(file.FileName);

            //Add Current Date To Attached File Name  
            FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

            //Get Upload path from Web.Config file AppSettings.  
            string UploadPath = @"~/ImagesInProj/Complaint/"+ model.Complaint_Number;
            //Its Create complete path to store in server.  
            //temp model.ImageFile = UploadPath + FileName;

            using (var context = new Entities())
            {
                var data = context.Complaints.FirstOrDefault(x => x.Complaint_Number == model.Complaint_Number);
                if (data != null)
                {
                    data.AgainstParty = model.AgainstParty;
                    data.Description = model.Description;
                    data.Status = model.Status;
                    data.Subject = model.Subject;
                   // data.ImagePath = FileName;

                    context.SaveChanges();
                    ViewBag.Message = "Update is successfully completed";

                    //To copy and save file into server.  
                    // file.SaveAs(model.ImagePath);
                    return View("UpdateSchema", model);
                }
                else
                    return View();
            }

        }

    }
}