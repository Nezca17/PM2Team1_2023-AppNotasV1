using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Java.Security;
using PM2Team1_2023_AppNotasV1.Droid.Activities;
using PM2Team1_2023_AppNotasV1.Droid.Services;
using PM2Team1_2023_AppNotasV1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthService))]
namespace PM2Team1_2023_AppNotasV1.Droid.Services
{
    public class FirebaseAuthService : IFirebaseAuthService

    {
        public static int REQ_AUTH = 9999;
        public static String KEY_AUTH = "auth";

        public string getAuthKey()
        {
            return KEY_AUTH;
        }

        public bool IsUserSigned()
        {
            var user = Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser;
            var signedIn = user != null;

            return signedIn;
        }

        public async Task<bool> Logout()
        {
            try
            {
                FirebaseAuth.GetInstance(MainActivity.app).SignOut();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SignIn(string email, string password)
        {
            try
            {
                await FirebaseAuth.GetInstance(MainActivity.app).SignInWithEmailAndPasswordAsync(email, password);
                var user = Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser;

                if (user.IsEmailVerified)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SignInWithGoogle()
        {
            var googleIntent = new Intent(Forms.Context, typeof(GoogleLoginActivity));
            ((Activity)Forms.Context).StartActivityForResult(googleIntent, REQ_AUTH);
        }

        public async Task<bool> SignInWithGoogle(string token)
        {
            try
            {
                var credential = GoogleAuthProvider.GetCredential(token, null);
                await FirebaseAuth.GetInstance(MainActivity.app).SignInWithCredentialAsync(credential);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> SignUp(string email, string password)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CreateUserWithEmailAndPasswordAsync(email, password);

                await SignIn (email, password);

                var user = Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser;
                var actionCode = ActionCodeSettings.NewBuilder()
                       .SetUrl("https://pm2team1-2023.firebaseapp.com/__/auth/action?mode=action&oobCode=code")
                       .SetHandleCodeInApp(true).Build();
                await user.SendEmailVerificationAsync(actionCode);

                await Logout();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**********************************************{ex}");
                return false;
               
            }
        }

        public async Task<bool> ResetPassword(string email)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).SendPasswordResetEmailAsync(email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UserAutentication(string email)
        {
            try
            {
                var actionCode = ActionCodeSettings.NewBuilder()
                       .SetHandleCodeInApp(true).Build();
                await Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).SendSignInLinkToEmail(email, actionCode);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UserVerify()
        {
            var user = Firebase.Auth.FirebaseAuth.GetInstance(MainActivity.app).CurrentUser;

            return  user.IsEmailVerified;


        }
    }
}