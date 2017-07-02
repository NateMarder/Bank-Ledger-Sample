using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleBanking.Classes
{
    public class TransactionHandler
    {
        private WebRequestHelper _requestHelper;
        public WebRequestHelper RequestHelper => _requestHelper ?? ( _requestHelper = new WebRequestHelper() );

        public TransactionHandler( WebRequestHelper requestHelper )
        {
            _requestHelper = requestHelper;
        }

        public TransactionHandler()
        {
        }

        public void WithdrawFunds()
        {
        }

        public void AddFunds()
        {
        }

        public double GetBalance()
        {
            return 0.0;
        }
    }
}
