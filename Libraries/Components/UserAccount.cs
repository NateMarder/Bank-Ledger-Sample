using Libraries.Models;

namespace Libraries.Components
{
    public class UserAccount
    {
        private UserAccountModel _model;
        public UserAccountModel Model => _model ?? ( _model = new UserAccountModel() );

        public UserAccount(UserAccountModel model)
        {
            _model = model;
        }

        public UserAccount()
        {
        }



    }
}