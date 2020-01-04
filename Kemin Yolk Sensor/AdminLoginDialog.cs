
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

namespace Kemin_Yolk_Sensor
{
    public class AdminLoginDialog : Dialog
    {
        private EditText username;
        private EditText password;
        private Button loginBtn;
        private Context context;

        public AdminLoginDialog(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AdminLoginDialogLayout);
                        
            username = FindViewById<EditText>(Resource.Id.adminUsernameEditText);
            password = FindViewById<EditText>(Resource.Id.adminPasswordEditText);
            loginBtn = FindViewById<Button>(Resource.Id.adminLoginBtn);
            loginBtn.Click += this.AdminLogin;
        }

        private void AdminLogin(object sender, EventArgs e)
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
            } else if (!Constants.Constants.ADMIN_USERNAME.Equals(usernameString)
                || !Constants.Constants.ADMIN_PASSWORD.Equals(passwordString))
            {
                Toast.MakeText(Application.Context,
                    $"Invalid admin credentials!", ToastLength.Short).Show();
            } else
            {                
                var intent = new Intent(context, typeof(UserManagementActivity));
                context.StartActivity(intent);
                this.Dismiss();
            }
        }
    }
}
