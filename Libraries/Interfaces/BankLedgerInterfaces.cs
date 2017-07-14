using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libraries.Models;

namespace Libraries.Interfaces
{
    public interface IBankLedgerDataAccessLayer
    {
        bool RegisterNewUser( UserViewModel model );
        bool VerifyPasswordEmailComboExists( UserViewModel model );
        bool EmailExists( string searchEmail );
        bool SubmitTransaction( TransactionViewModel model );
        TransactionViewModel[] GetTransactionHistory( string userId );
    }
}