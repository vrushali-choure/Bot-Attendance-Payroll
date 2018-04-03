using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Holidays : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var Type = FormDialog.FromForm(HolidayFormFlow.HolidayForm, FormOptions.PromptInStart);
            context.Call(Type, DisplayHoliday);
        }

        private async Task DisplayHoliday(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("All holidays");

            using (HttpClient client = new HttpClient())
            {
                
                //Assuming that the api takes the user message as a query paramater
                string RequestURI = "http://localhost:62943/api/Holiday";
                HttpResponseMessage responsemMsg = await client.GetAsync(RequestURI);
                if (responsemMsg.IsSuccessStatusCode)
                {
                    var apiResponse = await responsemMsg.Content.ReadAsStringAsync();

                    //Post the API response to bot again
                    await context.PostAsync($"Response is {apiResponse}");

                }

                context.Done<object>(null);


                //context.Wait(MessageReceivedAsync);
            }
        }
    }
}