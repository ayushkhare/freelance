
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
    [Activity(Theme = "@style/MyTheme.Login", Label = "AddUserActivity", ScreenOrientation = ScreenOrientation.Portrait)]    
    public class AddUserActivity : Activity, DatePickerDialog.IOnDateSetListener
    {
        private EditText username;
        private EditText password;
        private TextView expiry;
        private Button changeExpiryBtn;
        private Button saveBtn;        
        private int bundleUserId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddUserLayout);

            username = FindViewById<EditText>(Resource.Id.addUsernameEt);
            password = FindViewById<EditText>(Resource.Id.addPasswordEt);
            expiry = FindViewById<TextView>(Resource.Id.addExpiryTv);
            changeExpiryBtn = FindViewById<Button>(Resource.Id.changeExpiryBtn);
            saveBtn = FindViewById<Button>(Resource.Id.saveBtn);

            var bundleUsername = Intent.GetStringExtra("Username");
            var bundlePassword = Intent.GetStringExtra("Password");
            var bundleExpiry = Intent.GetStringExtra("Expiry");
            bundleUserId = Intent.GetIntExtra("UserId", -1);

            if (!String.IsNullOrEmpty(bundleUsername))
            {
                username.Text = bundleUsername;                
            }
            if (!String.IsNullOrEmpty(bundlePassword))
            {
                password.Text = bundlePassword;
            }
            if (!String.IsNullOrEmpty(bundleExpiry))
            {
                expiry.Text = DateTime.Parse(bundleExpiry).ToShortDateString();
            } else
            {
                string defaultExpiry = DateTime.Now.AddDays(30).ToShortDateString();
                expiry.Text = defaultExpiry;
            }

            changeExpiryBtn.Click += this.ChangeExpiry;
            saveBtn.Click += this.SaveUser;

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

        private void SaveUser(object sender, EventArgs e)
        {
            string usernameString = username.Text;
            string passwordString = password.Text;

            DateTime parsedDateTime = DateTime.Parse(expiry.Text);            

            if (String.IsNullOrEmpty(usernameString))
            {
                Toast.MakeText(Application.Context,
                    $"Username cannot be empty!", ToastLength.Short).Show();
            }
            else if (String.IsNullOrEmpty(passwordString))
            {
                Toast.MakeText(Application.Context,
                    $"Password cannot be empty!", ToastLength.Short).Show();
            } else
            {
                User user = new User(usernameString, passwordString,
                    parsedDateTime, Role.USER);

                int userId= -1;
                if (bundleUserId != -1)
                {
                    user.Id = bundleUserId;
                    userId = DatabaseManager.UpdateUser(user);
                } else
                {
                    userId = DatabaseManager.SaveUser(user);
                }
                
                var loggedInUser = DatabaseManager.GetUser(userId);
                if (loggedInUser == null)
                {
                    Toast.MakeText(Application.Context, $"User not saved",
                        ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(Application.Context,
                        $"User details saved successfully!",
                        ToastLength.Short).Show();
                    Finish();
                }
            }
        }

        private void ChangeExpiry(object sender, EventArgs e)
        {
            DateTime dateTimeNow = DateTime.Parse(expiry.Text);
            dateTimeNow = dateTimeNow.AddMonths(-1);
            var dialog = new DatePickerDialog(this, this, dateTimeNow.Year,
                dateTimeNow.Month, dateTimeNow.Day);
            dialog.Show();
        }

        public void OnDateSet(DatePicker view, int year, int month,
            int dayOfMonth)
        {
            expiry.Text = (month + 1) + "/" + dayOfMonth + "/" + year;
        }
    }
}
