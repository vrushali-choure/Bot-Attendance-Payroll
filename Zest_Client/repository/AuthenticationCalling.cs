﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zest_Client
{
    public class AuthenticationCalling
    {
      
        public  async Task <string>TokenCalling(string username, string password)
        {
           

            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri("http://localhost:57144/");
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (username.Equals("vrushali", StringComparison.InvariantCultureIgnoreCase) && password.Equals("1234", StringComparison.InvariantCultureIgnoreCase))
            {
                var tag = new AuthenticationRequest { UserName = username, Password = password, AuthenticationType = "form" };

                string req = JsonConvert.SerializeObject(tag);
                HttpContent content = new StringContent(tag.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage res = cons.PostAsync("http://localhost:57144/api/Authentication/UserLogin", new StringContent(@"{""RequestJSON"":" + req + "}", Encoding.Default, "application/json")).Result;
                var data = await res.Content.ReadAsAsync<ServiceResponse>();
                var token = data.ResponseJSON.AuthorizationToken;
                string t = token.ToString();
                return t;
            }
            else
            {
                return null;
            }
           
        }
    }
    public class AuthenticationResponse
    {
        public int EmpID { get; set; }
        public string AuthorizationToken { get; set; }
    }
    public class ServiceResponse
    {
        public string Status { get; set; }
        public string ServerDateTime { get; set; }
        public string ErrorList { get; set; }
        public AuthenticationResponse ResponseJSON { get; set; }
    }
    public class AuthenticationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthenticationType { get; set; }
    }
   
}
