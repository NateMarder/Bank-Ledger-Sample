using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libraries.Models;

namespace Libraries.Dal
{
    public class UserAccount
    {
        public UserAccountModel Data { get; set; }
    }

    public static class LocalDataStore
    {
        private static readonly object PadLock = new object();

        private static Dictionary<string, UserAccount> _dataStore;
        public static Dictionary<string, UserAccount> DataStore
        {
            get
            {
                lock ( PadLock )
                {
                    return _dataStore ?? ( _dataStore = new Dictionary<string, UserAccount>() );
                }
            }
        }
    }
}