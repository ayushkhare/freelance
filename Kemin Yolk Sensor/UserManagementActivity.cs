
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
    [Activity(Theme = "@style/MyTheme.Login", Label = "User Management", ScreenOrientation = ScreenOrientation.Portrait)]    
    public class UserManagementActivity : Activity
    {
        private ListView listView;
        private List<User> users;
        private Button addUserBtn;
        private UserManagementAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserManagementLayout);

            listView = FindViewById<ListView>(Resource.Id.listView);
            addUserBtn = FindViewById<Button>(Resource.Id.addUserBtn);
            addUserBtn.Click += this.AddUser;
            users = new List<User>();            

            adapter = new UserManagementAdapter(this, users);
            listView.Adapter = adapter;
            listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                User user = users[e.Position];
                var intent = new Intent(this, typeof(AddUserActivity));
                intent.PutExtra("UserId", user.Id);
                intent.PutExtra("Username", user.Username);
                intent.PutExtra("Password", user.Password);
                intent.PutExtra("Expiry", user.ExpiryDate.ToString());
                StartActivity(intent);
            };

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetIcon(Android.Resource.Color.Transparent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            users = LoadDataFromDB();
            adapter.SetData(users);
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

        private void AddUser(object sender, EventArgs e)
        {
            this.StartActivity(new Intent(this, typeof(AddUserActivity)));
        }

        private List<User> LoadDataFromDB()
        {
            return DatabaseManager.GetUsers();

        }
    }
}
