using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Kemin_Yolk_Sensor.Database;
using Kemin_Yolk_Sensor.Model;

namespace Kemin_Yolk_Sensor
{
    public class ViewHolder: Java.Lang.Object
    {
        public TextView username { get; set; }

        public TextView dateOfExpiry { get; set; }

        public TextView daysLeft { get; set; }
    }

    public class UserManagementAdapter : BaseAdapter
    {
        private Activity activity;
        private List<User> users;

        public UserManagementAdapter(Activity activity, List<User> users)
        {
            this.activity = activity;
            this.users = users;
        }

        public override int Count => users.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return users[position].Id;
        }

        public override View GetView(int position, View convertView,
            ViewGroup parent)
        {
            var view = convertView;                        
            ImageView deleteBtn;            
            if (view == null)
            {
                view = activity.LayoutInflater.Inflate(
                Resource.Layout.UserManagementItemLayout, parent, false);                
                deleteBtn = view.FindViewById<ImageView>(Resource.Id.deleteBtn);
                deleteBtn.Click += delegate
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(activity);
                    alert.SetTitle("Confirm Delete");
                    alert.SetMessage("Are you sure you want to delete this user record?");

                    alert.SetNegativeButton("CANCEL", (c, ev) => { });
                    alert.SetPositiveButton("DELETE", (c, ev) => {
                        DatabaseManager.DeleteUser(users[position].Id);
                        users.RemoveAt(position);
                        SetData(users);
                        Toast.MakeText(activity, "User has been deleted",
                            ToastLength.Short).Show();
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                };
            }

            var username = view.FindViewById<TextView>(Resource.Id.username);
            var dateOfExpiry = view.FindViewById<TextView>(
                Resource.Id.dateOfExpiry);
            var daysLeft = view.FindViewById<TextView>(Resource.Id.daysLeft);
            username.Text = users[position].Username;
            dateOfExpiry.Text = users[position].ExpiryDate.ToString();
            var totalDays = (users[position].ExpiryDate.Date - DateTime.Now.Date).Days;
            if(totalDays <= 0)
            {
                daysLeft.Text = "Expired";
            } else
            {
                daysLeft.Text = totalDays.ToString();
            }
            

            return view;
        }

        public void SetData(List<User> userList)
        {         
            users = userList;
            this.NotifyDataSetChanged();
        }
    }
}
