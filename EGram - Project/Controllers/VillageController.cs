using CRM.Gram.Services.Models;
using E_GramProject.Bussiness_Logic;
using E_GramProject.Models;
using EGram.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
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
       
        public ActionResult Index(Schema Sceme)
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Schema Sceme, HttpPostedFileBase file)
        {
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
                        URL = item.URL,
                        ActiveTill=item.ActiveTill,
                        CreatedOn=item.CreatedOn,
                        FileName=item.FileName,
                        ImagePath=item.ImagePath,
                        Status=item.Status,
                        Village=item.Village
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
                            URL = Sceme.URL,
                          //  ImagePath = Sceme.FileName,
                           Status = Sceme.Status,
                            Village = Sceme.Village,
                            CreatedOn = Sceme.CreatedOn,
                            ActiveTill = Sceme.ActiveTill,
                            FileName = Sceme.FileName


                    });
                    //    db.SaveChanges();
                        return RedirectToAction("Index");


                    }
                }

            }
            return RedirectToAction("UpdateSchema",Sceme.SchemeID);
            
        }

        [Authorize]
        [HttpGet]
        public ActionResult UpdateSchema(int? id)
        {
            int number = Convert.ToInt32(id);
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
                        CreatedOn = item.CreatedOn,
                        ActiveTill = item.ActiveTill,   
                        Status  =item.Status,
                        Village = item.Village,
                        ImagePath = item.ImagePath,
                        FileName = item.FileName
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
            string FileName = String.Empty;
            string UploadPath=String.Empty;
            // It will redirect to 
            // the Read method
            //Use Namespace called :  System.IO  
            //if (file != null)
            //{
            //    FileName = Path.GetFileNameWithoutExtension(file.FileName);

            //    //To Get File Extension  
            //    string FileExtension = Path.GetExtension(file.FileName);

            //    //Add Current Date To Attached File Name  
            //    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

            //    //Get Upload path from Web.Config file AppSettings.  
            //    UploadPath = @"D:\nilam\EgramPanchayatV2\EGram - Copy\ImagePath\Schema\";
            //    //Its Create complete path to store in server.  
            //    model.ImagePath = UploadPath + FileName;
            //}
            using (var context = new Entities())
            {
                var data = context.Schemata.FirstOrDefault(x => x.SchemeID == model.SchemeID);
                if (data != null)
                {
                    data.SchemaName = model.SchemaName;
                    data.Description = model.Description;
                    data.Condition = model.Condition;
                    data.ImagePath = FileName;
                    data.Status = model.Status;
                    data.Village = model.Village;
                    data.CreatedOn = model.CreatedOn;
                    data.ActiveTill = model.ActiveTill;
                    data.FileName = FileName;
                    data.URL = model.URL;   
                    
                    context.SaveChanges();
                    ViewBag.Message = "Update is successfully completed";

                    //To copy and save file into server.
                    if (file != null)
                    {
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data1 = target.ToArray();

                        CrmServiceClient svc = Connection.CRMService();
                        string entitytype = "incident";
                        Entity Note = new Entity("annotation");
                        Guid EntityToAttachTo = Guid.Parse("34eb15ed-f565-475d-9456-a568d1387464"); // The GUID of the incident
                        Note["objectid"] = new Microsoft.Xrm.Sdk.EntityReference(entitytype, EntityToAttachTo);
                        Note["objecttypecode"] = entitytype;
                        Note["subject"] = "Village-Eksar";
                        Note["notetext"] = "Schema/" + model.SchemeID+"_"+model.SchemaName;
                        Note["filename"] = file.FileName;
                        Note["mimetype"] = file.ContentType;
                        Note["documentbody"] = Convert.ToBase64String(data1); //crm like us to store attachments as base64 strings

                        Guid note = svc.Create(Note);

                        context.ImageConfigs.Add(new ImageConfig
                        {
                            ID = note.ToString(),
                            ImageName = file.FileName,
                            SPPath = "Schema/" + model.SchemeID + "_" + model.SchemaName,
                            RecordType = "Complaint",
                            RecordNo = model.SchemeID.ToString(),
                            Purpose = model.SchemaName,
                            Date = DateTime.Now,
                            Note = note.ToString()

                        }); ;
                        context.SaveChanges();
                        //   file.SaveAs(model.ImagePath);
                    }
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
                        URL = item.URL,
                        CreatedOn =   item.CreatedOn,
                        ActiveTill = item.ActiveTill,
                        Status = item.Status,
                        Village = item.Village,
                        ImagePath = item.ImagePath,
                        FileName = item.FileName
                        
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
        
           //string d="call";
            complaint.Complaint_Number = nextNum;
            return View("CreateDispute", complaint);
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
                              //  ImageFile = item.ImageFile,
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
            if(id == null)
                return View();
            int number = Convert.ToInt32(id);
            ComplaintModal schemaObj = null;
            using (var db = new Entities())
            {
                List<SelectListItem> list = GetAllStatus();


                ViewBag.StatusList = list;
                var schemaquery = from b in db.Complaints
                                  where b.Complaint_Number == number
                                  select b;

                foreach (var item in schemaquery)
                {
                    schemaObj = new ComplaintModal
                    {
                        Description = item.Description,
                        AgainstParty = item.AgainstParty,
                        Complaint_Number = item.Complaint_Number,
                        CreatedOn = item.CreatedOn,
                        FileName = item.FileName,
                       // ImageFile=item.ImageFile,
                        Meeting     =item.Meeting,
                        RaisedByUniqueID    =item.RaisedByUniqueID,
                        Raised_By    =item.Raised_By,
                        Reviewbycomitee=    item.Reviewbycomitee,
                        Status=GetSelectedList(list,item.Status),
                        Subject=item.Subject,
                        StatusText= item.Status
                    };
                }
            }

          
            return View("UpdateDispute1",schemaObj);
        }

        private IEnumerable<SelectListItem> GetSelectedList(List<SelectListItem> list, string status)
        {
           foreach(var item in list)
            {
                if(item.Value==status)
                {
                    item.Selected = true;
                }
            }
           return list;
        }

        private List<SelectListItem> GetAllStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "Active",
                Value = "1"
            });
            list.Add(new SelectListItem()
            {
                Text = "Inactive",
                Value = "2"
            });
            return list;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDispute(ComplaintModal model, HttpPostedFileBase file)
        {
            string FileName = String.Empty;
            string UploadPath = String.Empty;
            model.Status = GetAllStatus();
            //if (file != null)
            //{
            //    FileName = Path.GetFileNameWithoutExtension(file.FileName);
            //    //To Get File Extension  
            //    string FileExtension = Path.GetExtension(file.FileName);

            //    //Add Current Date To Attached File Name  
            //    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

            //    //Get Upload path from Web.Config file AppSettings.  
            //    UploadPath = @"C:\Users\AMAR\OneDrive\Desktop\GIT Copy\EGram - Project\ImagesInProj\Complaint\" + model.Complaint_Number + "\\";

            //}
            //Its Create complete path to store in server.  
            //temp model.ImageFile = UploadPath + FileName;
            List<SelectListItem> list = GetAllStatus();

            using (var context = new Entities())
            {
                var data = context.Complaints.FirstOrDefault(x => x.Complaint_Number == model.Complaint_Number);
                if (data != null)
                {
                    data.AgainstParty = model.AgainstParty;
                    data.Description = model.Description;
                    data.Status = GetSelectedOne(list,model.StatusText)  ;
                    data.Subject = model.Subject;
                    data.Reviewbycomitee = model.Reviewbycomitee;
                    data.Raised_By = model.Raised_By;
                    data.Complaint_Number = model.Complaint_Number;
                    data.CreatedOn= model.CreatedOn;
                    data.FileName =  FileName;
                    data.Image_Purpose = model.Purpose;
                    context.SaveChanges();
                    ViewBag.Message = "Update is successfully completed";
                    if (file != null)
                    {
                        //if (!Directory.Exists(UploadPath))
                        //{
                        //    Directory.CreateDirectory(UploadPath);
                        //}
                        //file.SaveAs(UploadPath + FileName);
                       

                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data1 = target.ToArray();

                        CrmServiceClient svc= Connection.CRMService();
                        string entitytype = "incident";
                        Entity Note = new Entity("annotation");
                        Guid EntityToAttachTo = Guid.Parse("0C9F62A8-90DF-E311-9565-A45D36FC5FE8"); // The GUID of the incident
                        Note["objectid"] = new Microsoft.Xrm.Sdk.EntityReference(entitytype, EntityToAttachTo);
                        Note["objecttypecode"] = entitytype;
                        Note["subject"] = "Village-Eksar";
                        Note["notetext"] = "Complaint/"+model.Raised_By+"("+model.RaisedByUniqueID+")/"+model.Complaint_Number;
                        Note["filename"] = file.FileName;
                        Note["mimetype"] = file.ContentType;
                        Note["documentbody"] = Convert.ToBase64String(data1); //crm like us to store attachments as base64 strings

                        Guid note = svc.Create(Note);

                        context.ImageConfigs.Add(new ImageConfig
                        {
                            ID = note.ToString(),
                            ImageName = file.FileName,
                            SPPath = "Complaint/" + model.Raised_By + "(" + model.RaisedByUniqueID + ")/" + model.Complaint_Number,
                            RecordType = "Complaint",
                            RecordNo = model.Complaint_Number.ToString(),
                            Purpose=model.Purpose,
                            Date=DateTime.Now,
                            Note=note.ToString()
                            
                        }); ;
                              context.SaveChanges();

                    }


                    //return RedirectToAction("UpdateDispute/" + model.Complaint_Number);
                    return View("UpdateDispute1",model);
                }
                else
                    return View();
            }
        }

        public ActionResult GetAttachmentConfig(string imagetype,string number)
        {
            IList<ImageConfig> entities = new List<ImageConfig>();
            try 
            { 
              using (var db = new Entities())
              {

                var attachmentQuery = from b in db.ImageConfigs
                                  where b.RecordType == imagetype &&
                                         b.RecordNo == number    
                                  select b;
               
                foreach (var item in attachmentQuery)
                {
                        entities.Add(item);
                }
                    return Json(new { data = entities }, JsonRequestBehavior.AllowGet);

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public string downloadfile(string id)
        {
            
            using (var db = new Entities())
            {
                CustomerCRM CRM = new CustomerCRM();
                Note nt = CRM.RetrieveAttachmentFromCRM(id);
                return JsonConvert.SerializeObject(nt);
                //var attachmentQuery = from b1 in db.ImageConfigs
                //                      where b1.ID == id
                //                      select b1;
                //foreach (var item in attachmentQuery)
                //{
                //    //return "success"+item.SPPath;
                //}

            }

            return null;
        }
        private string GetSelectedOne(IEnumerable<SelectListItem> status,string s)
        {
            foreach (var item in status)
            {
                if (item.Text == s)
                {
                    return item.Text;
                }
            }
           return string.Empty;
        }

       
        public ActionResult CreateDashboardObject()
        {
            return View();
        }
            [HttpPost]
        public ActionResult CreateDashboardObject(Dashboardobj dash, HttpPostedFileBase file)
        {
            byte[] data;
            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            using (var db = new Entities())
            {
              
                db.Dashboardobjs.Add(new Dashboardobj() 
                { 
                    Name = dash.Name, 
                    Description = dash.Description, 
                    Guid = dash.Guid,
                    ImageName= dash.ImageName,
                  //  Image=data,
                    Isaplace=dash.Isaplace,
                    Isaresident=dash.Isaresident,
                    FilePath=dash.FilePath
                });
                db.SaveChanges();
            }
            ViewBag.Message = "Record Created SuccessFully.";
                return View();
        }

    }
}