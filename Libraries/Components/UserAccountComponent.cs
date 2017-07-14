using Libraries.Models;

namespace Libraries.Components
{
    public class UserAccountComponent
    {
        private UserAccountModel _model;
        public UserAccountModel Model => _model ?? ( _model = new UserAccountModel() );

        public UserAccountComponent(UserAccountModel model)
        {
            _model = model;
        }

        public UserAccountComponent()
        {
        }



    }
}