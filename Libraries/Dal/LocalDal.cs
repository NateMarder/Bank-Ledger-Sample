using Libraries.Interfaces;
using Libraries.Models;

namespace Libraries.Dal
{
    public class LocalDal : IBankLedgerDataAccessLayer
    {
        public virtual bool RegisterNewUser( UserViewModel model )
        {
            return false;
        }

        public bool VerifyPasswordEmailComboExists( UserViewModel model )
        {
            return false;
        }

        public bool EmailExists( string searchEmail )
        {
            return false;
        }

        public bool SubmitTransaction( TransactionViewModel model )
        {
            return false;
        }

        public TransactionViewModel[] GetTransactionHistory( string userId )
        {
            return null;
        }
    }
}