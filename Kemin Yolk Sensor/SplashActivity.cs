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
using Android.Support.V7.App;
using Android.Support.V4;
using Android.Util;
using System.Threading.Tasks;

namespace Kemin_Yolk_Sensor
{
    //This activity just sets up the splash screen when the user starts the app

    //[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Sets up the splash screen
        async void SimulateStartup()
        {
            //A small delay for before the app starts
            await Task.Delay(2000);
            //Start the main activity of the app
            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
            //StartActivity(new Intent(this, typeof(MainActivity)));
        }
    }
}