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
using Java.IO;
using Java.Lang;


namespace Kemin_Yolk_Sensor.Listeners
{
    public class CameraCaptureListener : CameraCaptureSession.CaptureCallback
    {
        //create an instance of CameraFunction to be used here to set values in CameraFunction.cs
        public CameraFunction Manager { get; set; }
        public CameraCaptureListener(CameraFunction manager)
        {
            Manager = manager;
        }
        public File File { get; set; }
        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            //sets the values for the current focus mode and state to be used in CameraFunction.cs
            Manager.currentfocusmode = result.Get(CaptureResult.ControlAfMode).ToString();
            Manager.currentfocusstate = result.Get(CaptureResult.ControlAfState).ToString();
            Process(result);
        }

        public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult)
        {
            Process(partialResult);
        }

        private void Process(CaptureResult result)
        {
            switch (Manager.State)
            {
                case CameraFunction.STATE_WAITING_LOCK:
                    {
                        Integer afState = (Integer)result.Get(CaptureResult.ControlAfState);
                        if (afState == null)
                        {
                        }

                        else if ((((int)ControlAFState.FocusedLocked) == afState.IntValue()) ||
                                   (((int)ControlAFState.NotFocusedLocked) == afState.IntValue()))
                        {
                            // ControlAeState can be null on some devices
                            Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                            if (aeState == null ||
                                    aeState.IntValue() == ((int)ControlAEState.Converged))
                            {
                                Manager.State = CameraFunction.STATE_PICTURE_TAKEN;
                            }
                            else
                            {
                            }
                        }
                        break;
                    }
                case CameraFunction.STATE_WAITING_PRECAPTURE:
                    {
                        // ControlAeState can be null on some devices
                        Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                        if (aeState == null ||
                                aeState.IntValue() == ((int)ControlAEState.Precapture) ||
                                aeState.IntValue() == ((int)ControlAEState.FlashRequired))
                        {
                            Manager.State = CameraFunction.STATE_WAITING_NON_PRECAPTURE;
                        }
                        break;
                    }
                case CameraFunction.STATE_WAITING_NON_PRECAPTURE:
                    {
                        // ControlAeState can be null on some devices
                        Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                        if (aeState == null || aeState.IntValue() != ((int)ControlAEState.Precapture))
                        {
                            Manager.State = CameraFunction.STATE_PICTURE_TAKEN;
                        }
                        break;
                    }
            }
        }
    }
}