using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_GramProject.Models
{
    public class CustomerCRM
    {
        public string id { get; set; }
        public string guid { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string IdentityProof { get; set; }
        public string IdentityNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string District { get; set; }
        public string State { get; set; }

        public string[] Salutation { get; set; }
        
        //public string CreateCustomer()
        //{
        //    CrmServiceClient service = Connect("Connect");
        //    if (service.IsReady)
        //    {
        //        Entity caseEntity = new Entity("incident");
        //        caseEntity.Attributes["title"] = "case title";
        //        caseEntity.Attributes["subjectid"] = "subject";
        //        caseEntity.Attributes["description"] = "description";
        //        service.Create(caseEntity);

        //    }

        //        return "";
        //}

        public static CrmServiceClient Connect(string name)
        {
            CrmServiceClient service = null;
            string constr = GetConnectionStringFromAppConfig("Connect");
            //You can specify connection information in cds/App.config to run this sample without the login dialog
            if (string.IsNullOrEmpty(constr))
            {
                //// Failed to find a connection string... Show login Dialog. 
                //ExampleLoginForm loginFrm = new ExampleLoginForm();
                //// Login process is Async, thus we need to detect when login is completed and close the form. 
                //loginFrm.ConnectionToCrmCompleted += LoginFrm_ConnectionToCrmCompleted;
                //// Show the dialog here. 
                //loginFrm.ShowDialog();

                //// If the login process completed, assign the connected service to the CRMServiceClient var 
                //if (loginFrm.CrmConnectionMgr != null && loginFrm.CrmConnectionMgr.CrmSvc != null && loginFrm.CrmConnectionMgr.CrmSvc.IsReady)
                //    service = loginFrm.CrmConnectionMgr.CrmSvc;


            }
            else
            {
                // Try to create via connection string. 
                service = new CrmServiceClient(constr);
                var s = service.IsReady;
            }

            return service;

        }

        private static string GetConnectionStringFromAppConfig(string name)
        {
            return "AuthType=OAuth;Username=AMARSAP@ISVPractice95.onmicrosoft.com;Url=https://org9023a361.crm.dynamics.com;"
                 + "Password=Pa##w0rd;AppId=fc1f45df-d10b-4f45-b22d-0f393e1bba1f;"
                 + "RedirectUri=https://localhost/d365;LoginPrompt=Never";
        }

        internal Note RetrieveAttachmentFromCRM(string noteid)
        {
            CrmServiceClient service = Connect("Connect");
            if (service.IsReady)
            {
                Note nt=new Note();
                Entity note = service.Retrieve("annotation", new Guid(noteid), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                nt.FileName = note.GetAttributeValue<string>("filename");
                nt.doc_content = note.GetAttributeValue<string>("documentbody");
                return nt;
                //String Location = @"Imag";
                //String filename = note.GetAttributeValue<String>("filename");
                //String noteBody = note.GetAttributeValue<String>("documentbody");

                //string outputFileName = @ "" + Location + "\\" + filename;
                //System.IO.File.WriteAllBytes(outputFileName, fileContent);
            }
            return null;
        }
    }

    public class Salutation
    {
        public string title;
        public int value;
    }
    public class Note
    {
        public string FileName { get; set; }
        public string doc_content { get; set; }
    }
}