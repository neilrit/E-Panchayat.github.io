using CRM.Gram.Services.Models;
using E_GramProject.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace E_GramProject.Bussiness_Logic
{
    public class masterclass
    {
        static public List<SelectListItem> citylist = new List<SelectListItem>();
        static public List<SelectListItem> statelist = new List<SelectListItem>();
        static public List<SelectListItem> ctrylist = new List<SelectListItem>();


    }
    public class CustomerCurd
    {
        
       
        static CrmServiceClient service= Connection.CRMService();
        public string CreateCustomer(CustomerCRM customer)
        {
            string response= string.Empty;
            try
            {
                if (!service.IsReady) {
                    service = Connection.CRMService();
                }

                if (service.IsReady)
                {
                    Entity caseEntity = new Entity("contact");
                    caseEntity.Attributes["salutation"] = "";
                    caseEntity.Attributes["firstname"] = customer.FirstName;
                    caseEntity.Attributes["lastname"] = customer.LastName;
                    caseEntity.Attributes["middlename"] = customer.MiddleName;
                    caseEntity.Attributes["mobilephone"] = customer.MobilePhone;
                    caseEntity.Attributes["emailaddress1"] = customer.Email;
                    Guid response1= service.Create(caseEntity);
                    response = response1.ToString();
                }
               
                return response;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        internal List<SelectListItem> retrieveMasterData(string entity,string[] FielNames,string EntityName)
        {
            try
            {
                if(EntityName== "cr3cc_city" && masterclass.citylist.Count !=0)
                {
                    return masterclass.citylist;
                }
                else if(EntityName == "cr3cc_state")
                {
                    return masterclass.statelist;
                }
                if (!service.IsReady)
                {
                    service = Connection.CRMService();
                }

                if (service.IsReady)
                {
                    
                    QueryExpression query = new QueryExpression(EntityName);
                    query.ColumnSet.AddColumns(FielNames);
                    if (EntityName == "cr3cc_city")
                    {
                        EntityCollection cityCollection = new EntityCollection();
                        cityCollection = service.RetrieveMultiple(query);
                        foreach(Entity city in cityCollection.Entities)
                        {
                            masterclass.citylist.Add(new SelectListItem()
                            {
                                Text = city.GetAttributeValue<string>("cr3cc_name"),
                                Value = city.Id.ToString()
                            });
                            if(masterclass.statelist.Count==0 && city.Attributes.Contains("cr3cc_state") && city.Attributes["cr3cc_state"] !=null
                                && city.GetAttributeValue<EntityReference>("cr3cc_state") !=null)
                            {
                                masterclass.statelist.Add(new SelectListItem()
                                {
                                    Text = city.GetAttributeValue<EntityReference>("cr3cc_state").Name,
                                    Value = city.GetAttributeValue<EntityReference>("cr3cc_state").Id.ToString()
                                });
                            }
                           else if(   city.Attributes.Contains("cr3cc_state")
                                && city.Attributes["cr3cc_state"] != null
                                && city.GetAttributeValue<EntityReference>("cr3cc_state") != null
                                && city.GetAttributeValue<EntityReference>("cr3cc_state").Name!="")
                            {
                                bool isadded = true;
                                foreach(var item in masterclass.statelist)
                                {
                                    if (item.Text == city.GetAttributeValue<EntityReference>("cr3cc_state").Name) 
                                    {
                                        isadded = false;
                                        break; }
                                }
                                if(isadded)
                                    masterclass.statelist.Add(new SelectListItem()
                                    {
                                        Text = city.GetAttributeValue<EntityReference>("cr3cc_state").Name,
                                        Value = city.GetAttributeValue<EntityReference>("cr3cc_state").Id.ToString()
                                    });

                            }
                        }
                        
                        return masterclass.citylist;
                      }
                }
                return null;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        internal IList<CustomerCRM> ViewAllResident()
        {
            IList<CustomerCRM> customerCollection = new List<CustomerCRM>();
            if (!service.IsReady)
            {
                service = Connection.CRMService();
            }

            if (service.IsReady)
            {
                try
                {
                    QueryExpression qe = new QueryExpression("contact");
                    qe.ColumnSet = new ColumnSet("salutation", "firstname", "lastname", "mobilephone", "emailaddress1", "cr3cc_contactid");
                    EntityCollection ec = service.RetrieveMultiple(qe);
                    foreach (Entity contact in ec.Entities)
                    {
                        CustomerCRM customer = new CustomerCRM
                        {
                            id= (contact.Contains("cr3cc_contactid") &&
                                    contact.Attributes["cr3cc_contactid"] != null) ? contact.GetAttributeValue<string>("cr3cc_contactid") : "",
                            Title = (contact.Contains("salutation") &&
                                    contact.Attributes["salutation"]!=null)? contact.GetAttributeValue<string>("salutation"):"",
                            FirstName = (contact.Contains("firstname") &&
                                    contact.Attributes["firstname"] != null) ? contact.GetAttributeValue<string>("firstname"):"",
                            LastName = (contact.Contains("lastname") &&
                                    contact.Attributes["lastname"] != null) ? contact.GetAttributeValue<string>("lastname"):"",
                            MobilePhone = (contact.Contains("mobilephone") &&
                                    contact.Attributes["mobilephone"] != null) ? contact.GetAttributeValue<string>("mobilephone"):"",
                            Email = (contact.Contains("emailaddress1") &&
                                    contact.Attributes["emailaddress1"] != null) ? contact.GetAttributeValue<string>("emailaddress1"):"",
                            guid = (contact.Contains("contactid")) ? contact.GetAttributeValue<Guid>("contactid").ToString() : "",

                        };

                        customerCollection.Add(customer);
                    }
                }
                catch(FaultException e)
                {
                    throw e;
                }
                catch(Exception e)
                {
                    throw e;
                }
                return customerCollection;
            }
            return null;
        }

        internal int DeleteResourcebyID(string entityName,string Recordid)
        {

            if (!service.IsReady)
            {
                service = Connection.CRMService();
            }

            if (service.IsReady)
            {
                service.Delete(entityName, new Guid(Recordid));
                return 200;
            }
            return -1;
        }

        public string createOTPmessagesinCRM(string Email,string SMSContent)
        {
            if (!service.IsReady)
            {
                service = Connection.CRMService();
            }

            if (service.IsReady)
            {
                Entity OTPmessage = new Entity("cr3cc_otpmessages");
                OTPmessage.Attributes["cr3cc_email"] = Email;
                OTPmessage.Attributes["cr3cc_content"] = SMSContent;
                return (service.Create(OTPmessage)).ToString();
            }
                return "done";
        }
    }
}