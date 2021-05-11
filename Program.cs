using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace ConsoleDogAge
{
    public class FollowupDiet
    {
        string API_Token = "0EB30887EC5606E71F31A0E416A39FAE";
        string API_URL = "https://redcap.dogagingproject.org/api/";

        private string responseHTTP(byte[] bytePostData)
        {
            string strResponse = "";

            try
            {
                HttpWebRequest webreqDogAge = (HttpWebRequest)WebRequest.Create(API_URL);

                webreqDogAge.Method = "POST";
                webreqDogAge.ContentType = "application/x-www-form-urlencoded";
                webreqDogAge.ContentLength = bytePostData.Length;

                // Get the request stream and read it
                Stream streamData = webreqDogAge.GetRequestStream();
                streamData.Write(bytePostData, 0, bytePostData.Length);
                streamData.Close();

                HttpWebResponse webrespDogAge = (HttpWebResponse)webreqDogAge.GetResponse();

                //Read the response and outout it
                Stream streamResponse = webrespDogAge.GetResponseStream();
                StreamReader readerResponse = new StreamReader(streamResponse);

                strResponse = readerResponse.ReadToEnd();
            }
            catch (WebException exWE)
            {
                Stream streamWE = exWE.Response.GetResponseStream();
                StringBuilder sbResponse = new StringBuilder("", 65536);

                try
                {
                    byte[] readBuffer = new byte[1000];
                    int intCnt = 0;

                    for (; ; )
                    {
                        intCnt = streamWE.Read(readBuffer, 0, readBuffer.Length);

                        if (intCnt == 0)
                        {
                            // EOF
                            break;
                        }

                        sbResponse.Append(System.Text.Encoding.UTF8.GetString(readBuffer, 0, intCnt));
                    }

                }
                finally
                {
                    streamWE.Close();

                    strResponse = sbResponse.ToString();
                }
            }
            catch (Exception ex)
            {
                strResponse = ex.Message.ToString();
                //throw new Exception ( "RC Error. " +  ex.Message.ToString(), ex);
            }

            return (strResponse);
        }

        public void GetData()
        {

            string strPostParameters = "";
            strPostParameters = "&content=record&format=csv&type=flat&eventName=unique";
            string strFields;
            string strForms;
            
            strFields = "subject_id,fu_df_frequency,fu_df_prim_org,fu_df_prim_brand, fu_df_overrweight";
            strForms = "Followup Diet";
            strPostParameters += "&fields=" + strFields;
            strPostParameters += "&forms=" + strForms;

            byte[] bytePostData = Encoding.UTF8.GetBytes("token=" + API_Token + strPostParameters);

            string strResponse = responseHTTP(bytePostData);

             //output the response
            Console.WriteLine(strResponse);
            Console.Write("Press ,Enter> to exit ...");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            { 
                //run loop until Enter is press
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            string sResponse = "";
            FollowupDiet fd = new FollowupDiet();
            fd.GetData();
            //output the response
            //Console.WriteLine(sResponse);
        }
    }
}
