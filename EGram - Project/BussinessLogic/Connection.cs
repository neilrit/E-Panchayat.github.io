using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Gram.Services.Models
{
    public class Connection
    {
       
        public static CrmServiceClient CRMService()
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
                
            }

            return service;
        }

        private static string GetConnectionStringFromAppConfig(string name)
        {
            // return @"AuthType=ClientSecret;url=https://org3b6d186f.crm.dynamics.com;ClientId=8dd1d69e7-e0a2-4323-b335-e9893ff2dfee;ClientSecret=9Rk8Q~xIJCM-YVTRAMeQYM7QH~4DZJotbgco7aW1";
            //   return "AuthType=OAuth;Username=NilamSapkal@Value121.onmicrosoft.com;Url=https://org5ecedb1b.crm.dynamics.com;"
            //+ "Password=Pa##w0rd;AppId=84761ae2-5c7b-4455-8454-a8697b415906;"
            //+ "RedirectUri=https://localhost/d365;TokenCacheStorePath=d:\\MyTokenCache;LoginPrompt=Auto";

            return "AuthType=OAuth;Username=testtest@infy188.onmicrosoft.com;Url=https://org3b6d186f.crm.dynamics.com;"
            + "Password=Pa##w0rd;AppId=0c31d8f6-24b7-4ed6-abbc-d073c2c31ce2;"
            + "RedirectUri=https://localhost/d365;LoginPrompt=Never";
        }
    }
}