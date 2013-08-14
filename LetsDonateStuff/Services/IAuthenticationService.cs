using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetsDonateStuff.Services
{
    public interface IAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }
}
