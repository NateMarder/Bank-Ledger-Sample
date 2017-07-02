using System;
using Microsoft.VisualBasic.CompilerServices;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ConsoleBanking.Interfaces;


namespace ConsoleBanking
{
    public class WebRequestHelper : IWebRequestHelper
    {
        private static HttpClient _httpClient;
        public static HttpClient HttpClient => _httpClient ?? ( _httpClient = new HttpClient() );

        public WebRequestHelper()
        {
        }

        public WebRequestHelper( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        public async Task<string> TestRequestAsync()
        {
            string responseBody;

            try
            {
                var response = await HttpClient.GetAsync( "http://www.contoso.com/" );
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch ( HttpRequestException ex )
            {
                responseBody = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            // clean up
            HttpClient.Dispose();

            return responseBody;
        }


        public async Task<string> Post(Uri uri)
        {
            string responseBody;
            try
            {
                var response = await HttpClient.GetAsync( uri );
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch ( HttpRequestException ex )
            {
                responseBody = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            // clean up
            HttpClient.Dispose();

            return responseBody;
        }
    }
}