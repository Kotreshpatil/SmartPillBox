using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;


namespace sqlconnectionapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int i = 0;
            while (i < 4)
            {
                    int CheckFlag = 0;
                    //int SkipFlag = 0;
                    //string query1 = "select skip_flag from dbo.flags where row_id = 1";
                    string query2 = "select chk_flg from dbo.flags where row_id = 1";
                    Dbcheck d = new Dbcheck();
                    //SkipFlag = d.SelectDbMethod(query1);
                    CheckFlag = d.SelectDbMethod(query2);
                    if (CheckFlag == 1)
                    {

                        DbUpdate A = new DbUpdate();
                        switch (i)
                        {
                            case 0:
                                A.UpdateDbMethod('P', 0, 0);
                                Environment.Exit(1);
                                break;
                            case 1:
                                A.UpdateDbMethod('R', 0, 0);
                                Environment.Exit(1);
                                break;
                            case 2:
                                A.UpdateDbMethod('L', 0, 0);
                                Environment.Exit(1);
                                break;
                            case 3:
                                A.UpdateDbMethod('S', 0, 0);
                                Program sms = new Program();
                                sms.SmsMethod();
                                Environment.Exit(1);
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
                        }
                        
                    }
                i++;
                System.Threading.Thread.Sleep(60000);
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
