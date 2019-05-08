using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Views;
using System;
using Android.Content.PM;

namespace Kemin_Yolk_Sensor
{
    [Activity(Label = "Kemin_Yolk_Sensor", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            SetContentView(Resource.Layout.Main);

            if (bundle == null)
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.mainframe, CameraFunction.NewInstance()).Commit();
            }
        }
    }
}

