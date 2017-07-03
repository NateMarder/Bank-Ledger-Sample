using System;
using System.Net.Http;
using System.Threading.Tasks;
using ConsoleBanking.Interfaces;
using ConsoleBanking.Models;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleBanking
{
    public class WebRequestHelper : IWebRequestHelper
    {
        private XDocument _sharedSettingsDocument;

        public XDocument SharedSettingsDocument
            => _sharedSettingsDocument 
            ?? ( _sharedSettingsDocument = XDocument.Load( "../SharedResources/Settings.xml" ) );

        private static HttpClient _httpClient;

        public static HttpClient HttpClient
            => _httpClient ?? ( _httpClient = new HttpClient() );

        public WebRequestHelper()
        {
        }

        public WebRequestHelper( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        public async Task<int> UserSignIn( LoginFromConsoleViewModel model )
        {
            string stringResponse;

            var signInUrl = SharedSettingsDocument.Descendants()
                .ToList()
                .Find( node => node.Value.ToString() == "signin" )
                .NextNode
                .ToString()
                .Replace( "<LinkValue>", "" )
                .Replace( "</LinkValue>", "" );

            Console.WriteLine( "Attempting to login at: " + signInUrl );
            Console.WriteLine( "  email: " + model.Email );
            Console.WriteLine( "  password: " + model.Password );
            Console.WriteLine( "  remember me: " + model.RememberMe );

            signInUrl += "?Email=" + model.Email + "&Password=" + model.Password + "&RememberMe=false";

            try
            {
                var response = await HttpClient.GetAsync( signInUrl );
                response.EnsureSuccessStatusCode();
                stringResponse = await response.Content.ReadAsStringAsync();
            }
            catch ( Exception )
            {
                stringResponse = Enum.GetName( typeof( SignInStatus ), SignInStatus.Failure );
            }

            HttpClient.Dispose();
            return int.Parse( stringResponse );
        }

        public async Task<int> RegisterNewUser( LoginFromConsoleViewModel model )
        {
            string stringResponse;

            var signInUrl = SharedSettingsDocument.Descendants()
                .ToList()
                .Find( node => node.Value.ToString() == "signin" )
                .NextNode
                .ToString()
                .Replace( "<LinkValue>", "" )
                .Replace( "</LinkValue>", "" );

            Console.WriteLine( "Attempting to login at: " + signInUrl );
            Console.WriteLine( "  email: " + model.Email );
            Console.WriteLine( "  password: " + model.Password );
            Console.WriteLine( "  remember me: " + model.RememberMe );

            signInUrl += "?Email=" + model.Email + "&Password=" + model.Password + "&RememberMe=false";

            try
            {
                var response = await HttpClient.GetAsync( signInUrl );
                response.EnsureSuccessStatusCode();
                stringResponse = await response.Content.ReadAsStringAsync();
            }
            catch ( Exception )
            {
                stringResponse = Enum.GetName( typeof( SignInStatus ), SignInStatus.Failure );
            }

            HttpClient.Dispose();
            return int.Parse( stringResponse );
        }
    }
}