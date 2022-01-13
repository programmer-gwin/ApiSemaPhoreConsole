using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class DataCentric
    {

        internal static string baseUrl = "https://corapay.widerpay.com/USSDCoraPay/";
        //My API CALLING METHOD 
        internal static async Task<string> PostUSSDAsyc(string actionName, string mrawData)
        {
            string mresult = "";
            try
            {
                using (var mclient = new HttpClient() { BaseAddress = new Uri(baseUrl) })
                {
                    //mclient.Timeout = TimeSpan.FromMinutes(1);
                    var response = await mclient.PostAsync($"{actionName}/", new StringContent(mrawData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                mresult = "";
            }
            return mresult;
        }


    }
}
