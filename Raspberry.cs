using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;


namespace sqlconnectionapp
{
    public class MainProgram
    {
        public static void Main(string[] args)
        {
            int i = 0;
            while (i < 4)
            {
                //This Loop runs every 10 minutes and checks the check flag and set flag is high or no
                //checkflag = 1 when patient opens pill box atleast once in allocated time and checkflag = 0 when pill box is not opened 
                //skipflag = 1 when this program checks the check flag and decides to skip this for next iterations
                int CheckFlag = 0;
                int SkipFlag = 0;
                //make query strings for calling method
                string query1 = "select skip_flag from dbo.flags where row_id = 1";
                string query2 = "select chk_flg from dbo.flags where row_id = 1";
                //create an object to call SelectDb method
                //This method queries db for flags status i.e to check status of CheckFlag and SkipFlag
                MainProgram d = new MainProgram();
                SkipFlag = d.SelectDbMethod(query1);
                CheckFlag = d.SelectDbMethod(query2);
                if (SkipFlag == 1)
                {
                    i = 5;
                    Environment.Exit(1);
                    //if skipflag is 1 it skips the iteration run
                }
                else
                {
                    if (CheckFlag == 1)
                    {
                        //after checking the status of checkflag 
                        //if checkflag = 1 then it makes switch case for i to decide how many alerts did user made use for consuming the medication
                        // i = 0 : user takes 0 alerts from the system and consumes medication before the first alert hence the user type = P(Proactive)
                        // i = 1 : user takes 1 alerts from the system and consumes medication before the first alert hence the user type = R(Reactive)
                        // i = 2 : user takes 2 alerts from the system and consumes medication before the first alert hence the user type = L(Lazy)
                        // i = 3 : user takes 3 alerts from the system and consumes medication before the first alert hence the user type = S(SuperLazy)
                        //and sends a text message if not taken even after 4 alerts

                        MainProgram A = new MainProgram();
                        switch (i)
                        {
                            case 0:
                                A.UpdateDbMethod('P', 1, 0);
                                break;
                            case 1:
                                A.UpdateDbMethod('R', 1, 0);
                                break;
                            case 2:
                                A.UpdateDbMethod('L', 1, 0);
                                break;
                            case 3:
                                //A.UpdateDbMethod('S', 1, 0);
                                //MainProgram sms = new MainProgram();
                                //sms.SmsMethod();
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Its Time to take medication");
                        if (i == 3)
                        {
                            //DbUpdate A = new DbUpdate();
                            //A.UpdateDbMethod('S', 1, 0);
                            //Program sms = new Program();
                            //sms.SmsMethod();
                            //break;
                            Console.WriteLine("Sending SMS for the caretaker");
                        }
                    }
                }
                i++;
                System.Threading.Thread.Sleep(10);
            }
        }
        public void SmsMethod()
        {
            // Find your Account Sid and Auth Token at twilio.com/user/account
            string AccountSid = "{{ AC0f7585feaf972f1fc2c04ddd7f8596e0 }}";
            string AuthToken = "{{ e8416e31ae6fc614d2e119aec701903a }}";

            TwilioClient.Init(AccountSid, AuthToken);

            var message = MessageResource.Create(
                body: "Patient skipped to take medication.",
                from: new Twilio.Types.PhoneNumber("+2562425369"),
                to: new Twilio.Types.PhoneNumber("+640225620291")
            );

            Console.WriteLine(message.Sid);
        }
        public int SelectDbMethod(string query)
        {
            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=flagdata;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            int flag = (Int32)cmd.ExecuteScalar();
            conn.Close();
            return flag;
        }
        public void UpdateDbMethod(char user_type, int skip_flag, int chk_flg)
        {
            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=flagdata;Integrated Security=True");
            conn.Open();
            string cmd1 = "update [dbo].[flags] set user_type = '" + user_type;
            string cmd2 = "', skip_flag = " + skip_flag;
            string cmd3 = ", chk_flg = " + chk_flg;
            string cmd4 = " where row_id = 1 ";
            string Sqlcmd = cmd1 + cmd2 + cmd3 + cmd4;
            //Console.WriteLine(Sqlcmd);
            SqlCommand cmd = new SqlCommand(Sqlcmd, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
