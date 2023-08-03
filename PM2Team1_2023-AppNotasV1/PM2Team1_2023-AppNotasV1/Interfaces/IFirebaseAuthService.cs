using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM2Team1_2023_AppNotasV1.Interfaces
{
    public interface IFirebaseAuthService
    {
        String getAuthKey();
        bool IsUserSigned();
        Task<bool> SignUp(String email, String password);
        Task<bool> SignIn(String email, String password);
        void SignInWithGoogle();
        Task<bool> SignInWithGoogle(String token);
        Task<bool> Logout();
 
    }

}

