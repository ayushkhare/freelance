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
using Android.Hardware.Camera2;
using Android.Util;

namespace Kemin_Yolk_Sensor.Listeners
{
    public class CameraStateListener : CameraDevice.StateCallback
    {
        public CameraFunction manager;
        public override void OnOpened(CameraDevice cameraDevice)
        {
            // This method is called when the camera is opened.  We start camera preview here.

            manager.cameraOpenCloseLock.Release();
            manager.cameraDevice = cameraDevice;
            manager.CreateCameraPreviewSession();
        }

        public override void OnDisconnected(CameraDevice cameraDevice)
        {
            manager.cameraOpenCloseLock.Release();
            cameraDevice.Close();
            manager.cameraDevice = null;
        }

        public override void OnError(CameraDevice cameraDevice, CameraError error)
        {
            manager.cameraOpenCloseLock.Release();
            cameraDevice.Close();
            manager.cameraDevice = null;
            if (manager == null)
                return;
            Activity activity = manager.Activity;
            if (activity != null)
            {
                activity.Finish();
            }

        }
    }
}