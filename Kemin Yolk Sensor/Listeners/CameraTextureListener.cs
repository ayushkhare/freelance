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
using Android.Util;

namespace Kemin_Yolk_Sensor.Listeners
{
    public class CameraTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        public CameraFunction Manager { get; set; }

        public CameraTextureListener(CameraFunction manager)
        {
            Manager = manager;
        }

        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int width, int height)
        {

            Manager.OpenCamera(width, height);

        }

        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
        {
            return true;
        }

        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
        {

        }

        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
        {

        }

    }
}