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

namespace Kemin_Yolk_Sensor.Listeners
{
    public class CameraPreview : CameraCaptureSession.StateCallback
    {
        public CameraFunction Manager { get; set; }

        public CameraPreview(CameraFunction manager)
        {
            Manager = manager;
        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {
            Manager.ShowToast("Failed");
        }

        public override void OnConfigured(CameraCaptureSession session)
        {
            // The camera is already closed
            if (null == Manager.cameraDevice)
            {
                return;
            }

            // When the session is ready, we start displaying the preview.
            Manager.cameraCaptureSession = session;
            try
            {
                //Manager
                // Auto focus should be continuous for camera preview.

                //Set the properties to Fluorescent white balance, auto exposure off, ISO to 125,
                //and shutter speed to 1/45
                Manager.PreviewRequestBuilder.Set(CaptureRequest.ControlAwbMode, (int)ControlAwbMode.Fluorescent);
                Manager.PreviewRequestBuilder.Set(CaptureRequest.ControlAeMode, (int)ControlAEMode.Off);
                Manager.PreviewRequestBuilder.Set(CaptureRequest.SensorSensitivity, 125);
                Manager.PreviewRequestBuilder.Set(CaptureRequest.SensorExposureTime, (long)2.222e+7);

                // Finally, we start displaying the camera preview.
                Manager.PreviewRequest = Manager.PreviewRequestBuilder.Build();
                Manager.cameraCaptureSession.SetRepeatingRequest(Manager.PreviewRequest,
                        Manager.captureCallBack, Manager.backHandler);

            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }
    }
}