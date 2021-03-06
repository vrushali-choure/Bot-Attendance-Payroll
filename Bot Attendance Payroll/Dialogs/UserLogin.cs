﻿using Autofac;
using Chronic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using Zest_Client;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class UserLogin : IDialog<object>
    {
        protected string username { get; set; }
        protected string password { get; set; }
        protected string AuthenticationType { get; set; }

        
        public async Task StartAsync(IDialogContext context)
        {

            context.PostAsync("Please Enter your username>>..");

            context.Wait(abc);
           
           
         
        }
        private async Task abc(IDialogContext context, IAwaitable<object> result)
        {

            var user = await result as Activity;
            username = (user.Text);

            await context.PostAsync("Enter password:");
            context.Wait(abc2);

            

        }
        private async Task abc2(IDialogContext context, IAwaitable<object> result)
        {
            var pass = await result as Activity;
            password = (pass.Text);
            var ac = new AuthenticationCalling();
            string t = await ac.TokenCalling(username, password);
            if (t == null)
            {
                context.PostAsync("you have enterd wrong credentials<br>"+" re-login<br>"+"Enter your username");
                context.Wait(abc);
            }

            if(t!=null)
            {
                await context.PostAsync($"Response is {t}");
                context.UserData.SetValue("token", t);
                context.Done(true);
            }
            /*Conversation.UpdateContainer(
               builder =>
               {
                   var store = new InMemoryDataStore();
                   builder.Register(c => store)
                          .Keyed<IBotDataStore<BotData>>(t)
                          .AsSelf()
                          .SingleInstance();

               });*/
           
        }
       
    }
   
}
    