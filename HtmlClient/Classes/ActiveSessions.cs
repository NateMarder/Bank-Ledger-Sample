using System;
using System.Collections.Generic;

namespace HtmlClient.Classes
{
    public static class ActiveSessions
    {
        private static readonly object PadLock = new object();

        private static List<Tuple<bool, string, string>> _sessions;
        public static List<Tuple<bool, string, string>> Sessions
        {
            get
            {
                lock ( PadLock )
                {
                    return _sessions ?? ( _sessions = new List<Tuple<bool, string, string>>() );
                }
            }
        }

        public static void MarkSessionAsAuthenticated( string sessionId, string userId = null)
        {
            var email = userId ?? string.Empty;
            for ( var i = 0; i < Sessions.Count; i++ )
            {
                if ( Sessions[i].Item2 == sessionId )
                {
                    email = string.IsNullOrEmpty( email ) ? Sessions[i].Item3 : email;
                    Sessions.RemoveAt( i );
                    break;
                }
            }

            var authenticatedSession = Tuple.Create( true, sessionId, email );
            Sessions.Add( authenticatedSession );
        }

        public static void RemoveSession( string sessionId )
        {
            for ( var i = 0; i < Sessions.Count; i++ )
            {
                if ( Sessions[i].Item2 == sessionId )
                {
                    Sessions.RemoveAt( i );
                    break;
                }
            }
        }

        public static bool SessionIsAuthenticated( string sessionId, string email )
        {
            foreach ( var nextTuple in Sessions )
            {
                if ( nextTuple.Item2 == sessionId  && nextTuple.Item3 == email)
                {
                    return nextTuple.Item1;
                }
            }
            return false;
        }
    }
}