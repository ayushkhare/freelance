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
    public class ErrorDialog : DialogFragment
    {
        private static readonly string MESSAGE = "";
        private static Activity activity;

        private class PositiveListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            public void OnClick(IDialogInterface dialog, int which)
            {
                activity.Finish();
            }
        }

        public static ErrorDialog NewInstance(string message)
        {
            var args = new Bundle();
            args.PutString(MESSAGE, message);
            return new ErrorDialog { Arguments = args };
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            activity = Activity;
            return new AlertDialog.Builder(activity)
                .SetMessage(Arguments.GetString(MESSAGE))
                .SetPositiveButton(Android.Resource.String.Ok, new PositiveListener())
                .Create();
        }
    }
}