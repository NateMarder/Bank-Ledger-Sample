using System.Collections.Generic;

namespace Libraries.SessionManagement
{
    // singleton implementation of makeshift session
    public class ConsoleSession
    {
        private static ConsoleSession _instance;
        public static ConsoleSession Instance => _instance ?? ( _instance = new ConsoleSession() );
        public Dictionary<string, string> Data { get; }

        private ConsoleSession()
        {
            Data = new Dictionary<string, string>();
        }
    }
}