
using E_GramProject.Bussiness_Logic;
using E_GramProject.Models;
using EGram.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace E_GramProject.Views.Home
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }

        //To create complaint
        public ActionResult CreateComplaint()
        {
            List<Salutation> listoftitle = new List<Salutation>();
            Salutation obj = new Salutation();
            //    obj.CreateCustomer();
            obj.title = "Mr";
            obj.value = 1;
            Salutation obj1 = new Salutation()
            {
                title = "Ms",
                value = 2

            };
            listoftitle.Add(obj);
            listoftitle.Add(obj1);
            ViewBag.titlearray = listoftitle;
            return View();
        }

        public ActionResult UpdateComplaint()
        {
            return View();
        }

        public ActionResult DeleteComplaint()
        {
            return View();
        }
      
        public ActionResult CreateResidents(CustomerCRM cust)
        {
            ViewBag.Response = "";
            CustomerCurd custcurd = new CustomerCurd();
            if (cust.FirstName!=null)
            {
                string returnval = custcurd.CreateCustomer(cust);
                ViewBag.Response = returnval;
                using (var db = new Entities())
                {
                    db.Customers.Add(new Customer
                    {
                      GUID=returnval,
                      FirstName=cust.FirstName,
                      LastName=cust.LastName,
                      MiddleName=cust.MiddleName,
                      MobilePhone=cust.MobilePhone,
                      Email=cust.Email,
                      State =cust.State,
                      City=cust.City,
                      District=cust.District,
                    // Address=cust.Address,
                     ID ="a",
                     IdentityNo =  cust.IdentityNo,
                     IdentityProof=cust.IdentityProof  

                        
                    });
                    db.SaveChanges();
                }
            }
            
            ViewBag.titlearray = new SelectList(GetSalutationData(), "Value", "Text");
            string[] cityArray = new string[]
                     {
                        "cr3cc_name",
                        "cr3cc_state"
                     };
            ViewBag.cityArray = new SelectList(custcurd.retrieveMasterData("cr3cc_city", cityArray, "cr3cc_city"),"Value","Text");
            ViewBag.stateArray = new SelectList(custcurd.retrieveMasterData("cr3cc_state", null , "cr3cc_state"), "Value", "Text");

           
                return View();
        }
      
        public ActionResult ViewResident()
        {
            
            //IList<Customer> entities = new List<Customer>();
            //try
            //{
            //    CustomerCurd custcurd = new CustomerCurd();
            //    entities = custcurd.ViewAllResident();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return View();
        }
        public ActionResult GetListCust()
        {
            IList<CustomerCRM> entities = new List<CustomerCRM>();
            try
            {
                CustomerCurd custcurd = new CustomerCurd();
                entities = custcurd.ViewAllResident();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return Json(new { data = entities }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public ActionResult updateCustomer(string id)
        {
            CustomerCurd custcurd = new CustomerCurd();
            string number = id;
            Customer customerObj = null;
            using (var db = new Entities())
            {

                var schemaquery = from b in db.Customers
                                  where b.GUID == id
                                  select b;

                foreach (var item in schemaquery)
                {
                    customerObj = new Customer
                    {
                      ID = item.ID,
                      FirstName = item.FirstName,
                      LastName = item.LastName,
                      MiddleName = item.MiddleName,
                      Salutation = item.Salutation, 
                      Email  =item.Email,
                      MobilePhone = item.MobilePhone,
                //      State = item.State,
                      Address = item.Address,
              //        City = item.City,//ConverttoText(item.City, "cr3cc_city"),
                      District   =item.District,
                      IdentityNo   =item.IdentityNo,
                      IdentityProof =item.IdentityProof,
                      GUID = item.GUID
                    };
                }
            }
               ViewBag.titlearray = new SelectList(GetSalutationData(), "Value", "Text");
            string[] cityArray = new string[]
                     {
                        "cr3cc_name",
                        "cr3cc_state"
                     };
            ViewBag.cityArray = new SelectList(custcurd.retrieveMasterData("cr3cc_city", cityArray, "cr3cc_city"),"Value","Text");
            ViewBag.stateArray = new SelectList(custcurd.retrieveMasterData("cr3cc_state", null , "cr3cc_state"), "Value", "Text");

            return View(customerObj);
        }
        public ActionResult DeleteResourcebyid(dynamic id)
        {

            CustomerCurd custcurd = new CustomerCurd();
            int returnVal=custcurd.DeleteResourcebyID("contact",id[0]);
            
            return View("ViewResident");
        }

        public ActionResult ViewCustomerDetails(string id)
        {
            string number = id;
            Customer customerObj = null;
            using (var db = new Entities())
            {

                var schemaquery = from b in db.Customers
                                  where b.GUID == id
                                  select b;

                foreach (var item in schemaquery)
                {
                    customerObj = new Customer
                    {
                        ID = item.ID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleName = item.MiddleName,
                        Salutation = item.Salutation,
                        Email = item.Email,
                        MobilePhone = item.MobilePhone,
                        State = item.State,
                        Address = item.Address,
                        City = item.City,
                        District = item.District,
                        IdentityNo = item.IdentityNo,
                        IdentityProof = item.IdentityProof,
                        GUID = item.GUID
                    };
                }
            }
            return View(customerObj);
        }
        public ActionResult SPAPage()
        {
            return View("SPAPage");
        }

        [NonAction]
        public List<SelectListItem> GetSalutationData()
        {
            List<SelectListItem> list = new List<SelectListItem>();


            list.Add(new SelectListItem()
            {
                Text = "Mr",
                Value = "0"
            });

            list.Add(new SelectListItem()
            {
                Text = "Mrs",
                Value = "1"
            });
            return list;
        }

    }
}