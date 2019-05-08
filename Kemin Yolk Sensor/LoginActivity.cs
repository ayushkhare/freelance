
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Kemin_Yolk_Sensor.Database;
using Kemin_Yolk_Sensor.Model;

namespace Kemin_Yolk_Sensor
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private EditText username;
        private EditText password;
        private Button userLoginBtn;
        private Button adminLoginBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);

            username = FindViewById<EditText>(Resource.Id.usernameEditTex);
            password = FindViewById<EditText>(Resource.Id.passwordEditText);
            userLoginBtn = FindViewById<Button>(Resource.Id.userLoginBtn);
            adminLoginBtn = FindViewById<Button>(Resource.Id.adminLoginBtn);
            userLoginBtn.Click += this.UserLogin;
            adminLoginBtn.Click += this.AdminLogin;
        }

        private void AdminLogin(object sender, EventArgs e)
        {
            AdminLoginDialog adminLoginDialog = new AdminLoginDialog(this);
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
                DateTime expiryDate = DateTime.Today.AddMonths(1);
                User user = new User(usernameString, passwordString, expiryDate,
                     Role.USER);
                var id = DatabaseManager.SaveUser(user);
                var loggedInUser = DatabaseManager.GetUser(id);
                if(loggedInUser == null)
                {
                    Toast.MakeText(Application.Context, $"User not saved", 
                        ToastLength.Short).Show();
                } else
                {
                    Toast.MakeText(Application.Context, 
                        $"User saved, details: {user}", 
                        ToastLength.Short).Show();
                }
            }
        }
    }
}
