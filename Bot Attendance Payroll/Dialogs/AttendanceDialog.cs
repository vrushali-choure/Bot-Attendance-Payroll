
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Connector;
using System.Web.Http;

namespace Bot_Attendance_Payroll.Dialogs
{
    [LuisModel("8fa30d4f-9134-4dad-bc66-014adb8f2f79", "42bc0b5e5d4a4515b9d1c4ef4319c673")]
    [Serializable]

     public class AttendanceDialog : LuisDialog<object>
    {
        //await context.Forward(new AttendanceDialog(), ResumeAfterCallingAnyMethod, context.Activity, CancellationToken.None);

       
        // Greeting Intent
        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, IAwaitable<object> activity,LuisResult result)
        {
            var msg = await activity as Activity;
            

            if (msg.Text.Equals("hello", StringComparison.InvariantCultureIgnoreCase))
              {

                context.PostAsync("Authenticating user...");
                context.Call(new UserLogin(), ResumeAfterUserLogin);
            }

            else if (msg.Text.Equals("good morning", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Good Morning Friend");

            }

            else if (msg.Text.Equals("hi", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("hi how can I help you");
            }

                      
        }

        public async Task ResumeAfterUserLogin(IDialogContext context, IAwaitable<object> result)
        {

            await context.PostAsync("You have access to various details..");
            var formFLow = FormDialog.FromForm(EmployeeDetailsForm.BuildForm, FormOptions.PromptInStart);
            context.Call(formFLow, Formloaded);

        }


        

        //v1.Leave Encashment
        [LuisIntent("Leave_Encashment")]
        public async Task LeaveEncashment(IDialogContext context, LuisResult result)
        {
                   context.Call(new LeaveDialog(), this.ResumeAfterTaskDialog);

        }

       
        [LuisIntent("Leave_Encashment_types")]
        public async Task Leave_Encashment_types(IDialogContext context, LuisResult result)
        {

            context.Call(new Leave_Encashment_types(), this.ResumeAfterTaskDialog);

        }


        ////2.Tour
        [LuisIntent("Tour")]
        public async Task CallingTourMethod(IDialogContext context, LuisResult result)
        {
                       
            
            context.Call(new Tour(), this.ResumeAfterTaskDialog);


        }


       
        // v2.5 Tour_Details
        [LuisIntent("Tour_Details ")]
        public async Task Tour_Details(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Your tour deatils are as follows<br>" + "1.Tour StartDate:18-03-2018<br>"+"2.Tour End: 20-03-2018<br>"+"3.Place:Delhi<br>");

        }



        // 3.OutdoorDuty
        [LuisIntent("Outdoor")]
        public async Task CallingOutdoorMethod(IDialogContext context, LuisResult result)
        {
            
            context.Call(new OutdoorDuty(), this.ResumeAfterTaskDialog);
        }
        [LuisIntent("Outdoor_Details")]
        public async Task Outdoor_Details(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Your outdoor duty deatils are as follows<br>" + "1.Tour StartDate:18-03-2018<br>" + "2.Tour End: 20-03-2018<br>" + "3.Place:Delhi<br>");

        }


        //v.4 Work From Home
        [LuisIntent("WorkFromHome")]
        public async Task CallingWorkFromHome(IDialogContext context, LuisResult result)
        {

            context.Call(new WorkFromHome(), this.ResumeAfterTaskDialog);

        }

      
         
              
        //6.Misspunch
        [LuisIntent("Mispunch")]
        private async Task CallingMisspunchMethod(IDialogContext context, LuisResult result)
       {
            
            context.Call(new Misspunch(), ResumeAfterCallingMisspunch);
        }

        private async Task ResumeAfterCallingMisspunch(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync(" Your equest send for approval");
        }

        // 7.Compoff
        [LuisIntent("Compoff_apply ")]
        private async Task CallingCompOffMethod(IDialogContext context, LuisResult result)
        {

            context.Call(new Compoff(), ResumeAfterCallingCompoff);
        }

        private async Task ResumeAfterCallingCompoff(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Your request is forwared");
        }


        
        
        //6.Apply
        [LuisIntent("Apply_leave ")]
        public async Task Apply_Leave(IDialogContext context, LuisResult result)
        {
           context.Call(new ApplyingLeave(),ResumeAfterLeaveApply);
        }

        private async Task ResumeAfterLeaveApply(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Your leave has been  granted by your manager");
        }

        [LuisIntent("Lop")]
        public async Task Lop(IDialogContext context, LuisResult result)
        {
            context.Call(new LopDetails(), this.ResumeAfterTaskDialog);
        }

        [LuisIntent("Late_Comings")]
        public async Task Late_Comings(IDialogContext context, LuisResult result)
        {
            context.Call(new Late_Comings(), this.ResumeAfterTaskDialog);
        }

        [LuisIntent("Early_leavings")]
        public async Task Early_leavings(IDialogContext context, LuisResult result)
        {
            context.Call(new Net_Hrs(), this.ResumeAfterTaskDialog);
        }


        // 2. Calling Profile
        [LuisIntent("Profile")]
        public async Task CallingProfileMethod(IDialogContext context,LuisResult result)
        {
            await context.PostAsync("I am profile");
            context.Call(new Profile(),ResumeAfterCallingProfile);
        }
        
        // Resume After Calling  Profile
        public async Task ResumeAfterCallingProfile(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(" Your profile request send for approval");
        }


        //3. Calling Payroll
        [LuisIntent("Payroll")]
        public async Task CallingPayrollMethod(IDialogContext context,LuisResult result)
        {
            await context.PostAsync("i am Payroll");
            var payrollform = FormDialog.FromForm(Payroll.PayrollForm, FormOptions.PromptInStart);
            context.Call(payrollform, ResumeAfterCallingPayroll);


        }

        public async Task ResumeAfterCallingPayroll(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Your Request is sent for approval");
        }

        



        
        // Calling Default None Intent

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry I dont know what you wanted.....");
            await context.PostAsync("### What are you looking for? <br>" +
                "1.Leave Encashment:"+"Try message like *leave encashment*<br>"+
                "2.Tour:" + "Type *tour*<br>"+
                "3.Outdoor Duty:" + "Type *Outdoor Duty*<br>"+
                "4.Work From Home:" + "Type *Work From Home*<br>"+
                "5.Compoff:" + "Type *Compoff*<br>"+
                "6.Mispunch:" + "Type *Mispunch*<br>"+
                "7.Working Hrs:" + "Type *Working Hrs*<br>"+
                "8.Holidays:" + "Type *Holidays*<br>"+
                "9.Payroll:" + "Type *Payroll*<br>"+
                "10.Profile:" + "Type *Profile*<br>"+
                "You can get information about above mention categories:"

                );       
        context.Wait(MessageReceived);
        }

      
        public async Task FormFlowResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
           await context.PostAsync("Form Flow Finished");
            context.Wait(MessageReceived);
        }

        public async Task Formloaded(IDialogContext context, IAwaitable<EmployeeDetailsForm> result)
        {
            var token = await result;
            if(token.requestTypes.Equals("Leave Encashment"))
            {
                context.Wait(MessageReceived);
            }
            else if(token.requestTypes.Equals("Tour"))
            {
                context.Wait(MessageReceived);
            }

            else if(token.requestTypes.Equals("Outdoor Duty"))
            {
                context.Wait(MessageReceived);
            }

            else if(token.requestTypes.Equals("Work From Home"))
            {
                context.Wait(MessageReceived);
            }
            else if(token.requestTypes.Equals("CompOff"))
            {
                context.Wait(MessageReceived);
            }
            else if(token.requestTypes.Equals("Mispunch"))
            {
                context.Wait(MessageReceived);
            }

            else if(token.requestTypes.Equals("Apply For Leave"))
            {
                context.Wait(MessageReceived);
            }

            else if(token.requestTypes.Equals("WorkTime"))
            {
                context.Wait(MessageReceived);
            }
            else if(token.requestTypes.Equals("Holidays"))
            {
                context.Wait(MessageReceived);
            }

            else if(token.requestTypes.Equals("Payroll"))
            {
                context.Wait(MessageReceived);
            }
            else if(token.requestTypes.Equals("Profile"))
            {
                context.Wait(MessageReceived);
            }     
        }

        
        [LuisIntent("LeaveEncashmentType ")]
        public async Task LeaveEncashmentType(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("yes it is encashable");
        }

        public async Task ResumeAfterCallingAnyMethod(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("your request is under process");
            context.Wait(MessageReceived);
        }

        ///////////k////////////////
        //k1:Pf_number
        [LuisIntent("Pf_number")]
        public async Task Pfnumber(IDialogContext context, LuisResult result)
        {
            // await context.PostAsync("Pf number");
            context.Call(new Pf_number(), this.ResumeAfterTaskDialog);
            //context.Wait(MessageReceivedAsync);
        }
        private async Task ResumeAfterTaskDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("how else can i help you");
            await context.PostAsync("### What are you looking for? <br>" +
                "1.Leave Encashment:" + "Try message like *leave encashment*<br>" +
                "2.Tour:" + "Type *tour*<br>" +
                "3.Outdoor Duty:" + "Type *Outdoor Duty*<br>" +
                "4.Work From Home:" + "Type *Work From Home*<br>" +
                "5.Compoff:" + "Type *Compoff*<br>" +
                "6.Mispunch:" + "Type *Mispunch*<br>" +
                "7.Working Hrs:" + "Type *Working Hrs*<br>" +
                "8.Holidays:" + "Type *Holidays*<br>" +
                "9.Payroll:" + "Type *Payroll*<br>" +
                "10.Profile:" + "Type *Profile*<br>"
                );

            context.Wait(MessageReceived);
        }

        //PROFILE

        //k1:Probation_Period
        [LuisIntent("Probation_Period")]
        public async Task Probation_period(IDialogContext context, LuisResult result)
        {
            context.Call(new Probation_Period(), this.ResumeAfterTaskDialog);
        }

        //k2:Join_date
        [LuisIntent("Join_date")]
        public async Task Join_date(IDialogContext context, LuisResult result)
        {
            context.Call(new Join_date(), this.ResumeAfterTaskDialog);
        }

        //k3:Experience
        [LuisIntent("Experience")]
        public async Task Experience(IDialogContext context, LuisResult result)
        {
            context.Call(new Experience(), this.ResumeAfterTaskDialog);
        }

        //k4:Bank_Account
        [LuisIntent("Bank_Account")]
        public async Task Bank_Account(IDialogContext context, LuisResult result)
        {
            context.Call(new Bank_Account(), this.ResumeAfterTaskDialog);
        }


        //HOLIDAYS
        [LuisIntent("Holidays")]
       
        public async Task Holidays(IDialogContext context, LuisResult result)
        {
            context.Call(new Holidays(), this.ResumeAfterTaskDialog);
        }
        //k5:Work_on_holiday
        [LuisIntent("Work_on_holiday")]
        public async Task Work_on_holiday(IDialogContext context, LuisResult result)
        {
            context.Call(new Work_on_holiday(), this.ResumeAfterTaskDialog);

        }
        //k6:Hrs_work_holiday
        [LuisIntent("Hrs_work_holiday")]
        public async Task Hrs_work_holiday(IDialogContext context, LuisResult result)
        {
            context.Call(new Hrs_work_holiday(), this.ResumeAfterTaskDialog);
        }





        //PAYROLL

        //k7:House_rent_allowance
        [LuisIntent("House_rent_allowance")]
        public async Task House_rent_allowance(IDialogContext context, LuisResult result)
        {
            context.Call(new Hrs_work_holiday(), this.ResumeAfterTaskDialog);
        }
        //k8:Dearness_allowance
        [LuisIntent("Dearness_allowance")]
        public async Task Dearness_allowance(IDialogContext context, LuisResult result)
        {
            context.Call(new Dearness_allowance(), this.ResumeAfterTaskDialog);
        }
        //k9:Medical_allowance
        [LuisIntent("Medical_allowance")]
        public async Task Medical_allowance(IDialogContext context, LuisResult result)
        {
            context.Call(new Medical_allowance(), this.ResumeAfterTaskDialog);
        }
        //k18:Lta_allowance 
        [LuisIntent("Lta_allowance")]
        public async Task Lta_allowance(IDialogContext context, LuisResult result)
        {
            context.Call(new Lta_allowance(), this.ResumeAfterTaskDialog);
        }
        //k10:Tax_deductions
        [LuisIntent("Tax_deductions")]
        public async Task Tax_deductions(IDialogContext context, LuisResult result)
        {
            context.Call(new Tax_deductions(), this.ResumeAfterTaskDialog);
        }
        //k11:Payslip
        [LuisIntent("Payslip")]
        public async Task Payslip(IDialogContext context, LuisResult result)
        {
            context.Call(new Payslip(), this.ResumeAfterTaskDialog);
        }
        //k12:Base_pay
        [LuisIntent("Base_pay")]
        public async Task Base_pay(IDialogContext context, LuisResult result)
        {
            context.Call(new Base_pay(), this.ResumeAfterTaskDialog);
        }
        //k16:Gross_pay
        [LuisIntent("Gross_pay")]
        public async Task Gross_pay(IDialogContext context, LuisResult result)
        {
            context.Call(new Gross_pay(), this.ResumeAfterTaskDialog);
        }
        //k17:Net_pay
        [LuisIntent("Net_pay")]
        public async Task Net_pay(IDialogContext context, LuisResult result)
        {
            context.Call(new Net_pay(), this.ResumeAfterTaskDialog);
        }
        //k13:Pf_contribution
        [LuisIntent("Pf_contribution")]
        public async Task Pf_contribution(IDialogContext context, LuisResult result)
        {
            context.Call(new Pf_contribution(), this.ResumeAfterTaskDialog);
        }
        //k14:Professional_tax_deducted
        [LuisIntent("Professional_tax_deducted")]
        public async Task Professional_tax_deducted(IDialogContext context, LuisResult result)
        {
            context.Call(new Professional_tax_deducted(), this.ResumeAfterTaskDialog);
        }
        //k15:Esi_tax
        [LuisIntent("Esi_tax")]
        public async Task Esi_tax(IDialogContext context, LuisResult result)
        {
            context.Call(new Esi_tax(), this.ResumeAfterTaskDialog);
        }

             
        ////k20:Investment_details 
        [LuisIntent("Investment_details")]
        public async Task Investment_details(IDialogContext context, LuisResult result)
        {
            context.Call(new Hrs_work_holiday(), this.ResumeAfterTaskDialog);
        }

        //k21:Section 80d,80c
        [LuisIntent("80c")]
        public async Task Section_80_C(IDialogContext context, LuisResult result)
        {
            context.Call(new Section80C(), this.ResumeAfterTaskDialog);
        }

        [LuisIntent("80d")]
        public async Task Section_80_D(IDialogContext context, LuisResult result)
        {
            context.Call(new Section80D(), this.ResumeAfterTaskDialog);
        }
        //k22:Tds_deduction 
        [LuisIntent("Tds_deduction")]
        public async Task Tds_deduction(IDialogContext context, LuisResult result)
        {
            context.Call(new Tds_deduction(), this.ResumeAfterTaskDialog);
        }


        //k23:Allowances 
        [LuisIntent("Allowances")]
        public async Task Allowances(IDialogContext context, LuisResult result)
        {
            context.Call(new Allowances(), this.ResumeAfterTaskDialog);
        }

        [LuisIntent("Half_day")]
        public async Task Half_day(IDialogContext context, LuisResult result)
        {
            context.Call(new HalfDay(), this.ResumeAfterTaskDialog);
        }

        //k25:gross ,net and avg hrs of month
        [LuisIntent("Working_Hrs")]
        public async Task Working_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Working_Hrs(), this.ResumeAfterTaskDialog);
        }
        //k26:in/out time for month
        [LuisIntent("In_Out_Time")]
        public async Task In_Out_Time(IDialogContext context, LuisResult result)
        {
            context.Call(new In_Out_Time(), this.ResumeAfterTaskDialog);
        }



        //ATTENDANCE
        //k27:Late comings of the month
       
        [LuisIntent("Gross_Hrs")]
        public async Task Gross_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Gross_Hrs(), this.ResumeAfterTaskDialog);
        }


        [LuisIntent("Avg_Hrs")]
        public async Task Avg_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Avg_Hrs(), this.ResumeAfterTaskDialog);
        }

        [LuisIntent("Net_Hrs")]
        public async Task Net_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Net_Hrs(), this.ResumeAfterTaskDialog);
        }


    }


}
