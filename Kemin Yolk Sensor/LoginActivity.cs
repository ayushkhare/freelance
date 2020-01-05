
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Kemin_Yolk_Sensor.Database;
using Kemin_Yolk_Sensor.Model;

namespace Kemin_Yolk_Sensor
{
    [Activity(Theme = "@style/MyTheme.Login", Label = "8SCAN™", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        private EditText username;
        private EditText password;
        private Button userLoginBtn;
        private ImageView adminLoginBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);

            username = FindViewById<EditText>(Resource.Id.usernameEditText);
            password = FindViewById<EditText>(Resource.Id.passwordEditText);
            userLoginBtn = FindViewById<Button>(Resource.Id.userLoginBtn);
            adminLoginBtn = FindViewById<ImageView>(Resource.Id.adminLoginBtn);
            userLoginBtn.Click += this.UserLogin;
            adminLoginBtn.Click += this.AdminLogin;          

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetIcon(Android.Resource.Color.Transparent);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void AdminLogin(object sender, EventArgs e)
        {
            AdminLoginDialog adminLoginDialog = new AdminLoginDialog(this);
            adminLoginDialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            adminLoginDialog.Show();
        }

        private void UserLogin(object sender, EventArgs e)
        {
            string usernameString = username.Text;
            string passwordString = password.Text;

            if (String.IsNullOrEmpty(usernameString))
            {
                Toast.MakeText(Application.Context, 
                    $"Username cannot be empty!", ToastLength.Short).Show();
            } else if (String.IsNullOrEmpty(passwordString))
            {
                Toast.MakeText(Application.Context,
                    $"Password cannot be empty!", ToastLength.Short).Show();
            } else
            {                                
                User validUser = DatabaseManager.FindUser(usernameString,
                    passwordString);                
                if(validUser != null)
                {
                    if (validUser.ExpiryDate < DateTime.Now)
                    {
                        Toast.MakeText(Application.Context,
                        $"Account has expired",
                        ToastLength.Short).Show();
                    } else
                    {
                        StartActivity(new Intent(this, typeof(MainActivity)));
                        Finish();
                    }                    
                } else
                {
                    Toast.MakeText(Application.Context, 
                        $"Invalid credentials or account has expired", 
                        ToastLength.Short).Show();
                }
            }
        }
    }
}
