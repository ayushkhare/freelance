using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Hardware.Camera2;
using Android.Graphics;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Android.Support.V13.App;
using Android.Support.V4.Content;
using Kemin_Yolk_Sensor.Listeners;
using Java.IO;
using Java.Lang;
using Java.Util;
using Java.Util.Concurrent;
using Boolean = Java.Lang.Boolean;
using Math = Java.Lang.Math;
using Orientation = Android.Content.Res.Orientation;
using System.Threading.Tasks;
using Android.Preferences;
using Android.Hardware;
using Android.Runtime;
using System.Linq;

namespace Kemin_Yolk_Sensor
{
    public class CameraFunction : Fragment, View.IOnClickListener, FragmentCompat.IOnRequestPermissionsResultCallback
    {
        private static readonly SparseIntArray ORIENTATIONS = new SparseIntArray();
        public static readonly int REQUEST_CAMERA_PERMISSION = 1;
        private static readonly string FRAGMENT_DIALOG = "dialog";

        // Tag for the {@link Log}.
        private static readonly string TAG = "CameraFunction";

        //string used to hold the text that will be saved when the user hits the save button
        private string saveText;
        //shared preferences to store saved data
        private ISharedPreferences sharedPreferences;
        //camera characteristics
        private CameraCharacteristics camchar;

        // Camera state: Showing camera preview.
        public const int STATE_PREVIEW = 0;

        // Camera state: Waiting for the focus to be locked.
        public const int STATE_WAITING_LOCK = 1;

        // Camera state: Waiting for the exposure to be precapture state.
        public const int STATE_WAITING_PRECAPTURE = 2;

        //Camera state: Waiting for the exposure state to be something other than precapture.
        public const int STATE_WAITING_NON_PRECAPTURE = 3;

        // Camera state: Picture was taken.
        public const int STATE_PICTURE_TAKEN = 4;

        // Max preview width that is guaranteed by Camera2 API
        private static readonly int MAX_PREVIEW_WIDTH = 1920;

        // Max preview height that is guaranteed by Camera2 API
        private static readonly int MAX_PREVIEW_HEIGHT = 1080;

        // TextureView.ISurfaceTextureListener handles several lifecycle events on a TextureView
        private CameraTextureListener cameraTextureListener;

        //the square marker where the reading will be taken
        private View squareMarker;

        // ID of the current {@link CameraDevice}.
        private string cameraId;

        // An TextureView for camera preview
        private TextureView textureView;
        public Size imageDimensions;

        //layouts used to make sure camera preview is always at correct resolution
        private RelativeLayout relativeLayout;
        private FrameLayout frameLayout;


        //YolkScores object used to do comparison
        private YolkScores yolkScores;

        //Height and Width that will be used to determine resolution of camera
        private int DSI_Height;
        private int DSI_Width;

        //current ISO, SS and WB values used for testing only
        //public string currentISO;
        //public string currentShutterSpeed;
        //public string currentWhiteBalance;
        
        //gets the camera focus mode and state when the app starts to see what kind of focus the phone supports
        public string currentfocusmode;
        public string currentfocusstate;

        //not the actual phone model number, just to tell if the phone supports the camera focus in the code
        public string phonemodel;

        //bool to check the status of camera focus
        private bool focusLocked = false;

        //buttons to be used later
        //focus lock button
        private ImageButton focuslock_Btn;
        //button to take readings and get the score
        private ImageButton score_Btn;
        //button to open the list
        private ImageButton list_Btn;
        //button to save the reading
        private ImageButton save_Btn;
        //just a background for the score to be displayed on
        private Button scoreDisplay;


        // CameraCaptureSession for camera preview.
        public CameraCaptureSession cameraCaptureSession;

        // A reference to the opened CameraDevice
        public CameraDevice cameraDevice;

        // The size of the camera preview
        private Size size;
        private Size cameraSize;

        // CameraDevice.StateListener is called when a CameraDevice changes its state
        private CameraStateListener stateCallBack;

        // An additional thread for running tasks that shouldn't block the UI.
        private HandlerThread backThread;

        // A {@link Handler} for running tasks in the background.
        public Handler backHandler;

        // An {@link ImageReader} that handles still image capture.
        private ImageReader ImageReader;

        // This is the output file for our picture.
        public File mFile;



        //{@link CaptureRequest.Builder} for the camera preview
        public CaptureRequest.Builder PreviewRequestBuilder;

        // {@link CaptureRequest} generated by {@link #mPreviewRequestBuilder}
        public CaptureRequest PreviewRequest;

        // The current state of camera state for taking pictures.
        public int State = STATE_PREVIEW;

        // A {@link Semaphore} to prevent the app from exiting before closing the camera.
        public Semaphore cameraOpenCloseLock = new Semaphore(1);

        // Whether the current camera device supports Flash or not.
        private bool FlashStatus;

        // Orientation of the camera sensor
        private int OrientationSensor;

        // A {@link CameraCaptureSession.CaptureCallback} that handles events related to JPEG capture.
        public CameraCaptureListener captureCallBack;


        // Shows a {@link Toast} on the UI thread.
        public void ShowToast(string text)
        {
            if (Activity != null)
            {
                Activity.RunOnUiThread(new ShowToastRunnable(Activity.ApplicationContext, text));
            }
        }

        private class ShowToastRunnable : Java.Lang.Object, IRunnable
        {
            private string text;
            private Context context;

            public ShowToastRunnable(Context context, string text)
            {
                this.context = context;
                this.text = text;
            }

            public void Run()
            {
                Toast.MakeText(context, text, ToastLength.Short).Show();
            }
        }

        //picks the optimal size for the camera preview
        private static Size ChooseOptimalSize(Size[] choices, int textureViewWidth,
            int textureViewHeight, int maxWidth, int maxHeight, Size aspectRatio)
        {
            // Collect the supported resolutions that are at least as big as the preview Surface
            var bigEnough = new List<Size>();
            // Collect the supported resolutions that are smaller than the preview Surface
            var notBigEnough = new List<Size>();
            int w = aspectRatio.Width;
            int h = aspectRatio.Height;


            for (var i = 0; i < choices.Length; i++)
            {
                Size option = choices[i];
                if ((option.Width <= maxWidth) && (option.Height <= maxHeight) &&
                       option.Height == option.Width * h / w)
                {
                    if (option.Width >= textureViewWidth &&
                        option.Height >= textureViewHeight)
                    {
                        bigEnough.Add(option);
                    }
                    else
                    {
                        notBigEnough.Add(option);
                    }
                }
            }

            // Pick the smallest of those big enough. If there is no one big enough, pick the
            // largest of those not big enough.
            if (bigEnough.Count > 0)
            {
                return (Size)Collections.Min(bigEnough, new GetSize());
            }
            else if (notBigEnough.Count > 0)
            {
                return (Size)Collections.Max(notBigEnough, new GetSize());
            }
            else
            {
                Log.Error(TAG, "Couldn't find any suitable preview size");
                return choices[0];
            }
        }

        public static CameraFunction NewInstance()
        {
            return new CameraFunction();
        }

        //to display a toast from the other classes
        public void FocusToast(string toasttext)
        {
            Toast.MakeText(this.Activity, toasttext, ToastLength.Short).Show();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            stateCallBack = new CameraStateListener() { manager = this };
            cameraTextureListener = new CameraTextureListener(this);
            //new YolkScores object
            yolkScores = new YolkScores();
            var activity = Activity;

            //DisplayMetrics to get the appropriate resolution
            DisplayMetrics displayMetrics = new DisplayMetrics();
            activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            DSI_Height = displayMetrics.HeightPixels;
            DSI_Width = displayMetrics.WidthPixels;
            imageDimensions = new Size(DSI_Width, DSI_Height);

            //sets up value for phonemodel
            phonemodel = "";
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.main_camera, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            //assigns the layouts and buttons created above to the ones in the layout in main_camera.axml
            relativeLayout = (RelativeLayout)view.FindViewById(Resource.Id.rellayout);
            frameLayout = new FrameLayout(this.Activity);
            textureView = new TextureView(this.Activity);
            squareMarker = (View)view.FindViewById(Resource.Id.squaremarker);
            focuslock_Btn = (ImageButton)view.FindViewById(Resource.Id.focuslock_btn);
            score_Btn = (ImageButton)view.FindViewById(Resource.Id.score);
            list_Btn = (ImageButton)view.FindViewById(Resource.Id.list_btn);
            save_Btn = (ImageButton)view.FindViewById(Resource.Id.save_btn);
            scoreDisplay = (Button)view.FindViewById(Resource.Id.scoredisplay);
            view.FindViewById(Resource.Id.score).SetOnClickListener(this);
            view.FindViewById(Resource.Id.focuslock_btn).SetOnClickListener(this);
            view.FindViewById(Resource.Id.save_btn).SetOnClickListener(this);
            view.FindViewById(Resource.Id.list_btn).SetOnClickListener(this);

            //set the aspect ratio of the camera preview
            Size newSize = GetDimensions();
            setAspectRatioTextureView(newSize.Height, newSize.Width);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            mFile = new File(Activity.GetExternalFilesDir(null), "pic.jpg");
        }

        public override void OnResume()
        {
            base.OnResume();
            StartBackgroundThread();

            // When the screen is turned off and turned back on, the SurfaceTexture is already
            // available, and "onSurfaceTextureAvailable" will not be called. In that case, we can open
            // a camera and start preview from here (otherwise, we wait until the surface is ready in
            // the SurfaceTextureListener).
            if (textureView.IsAvailable)
            {
                OpenCamera(textureView.Width, textureView.Height);
            }
            else
            {
                textureView.SurfaceTextureListener = cameraTextureListener;
            }
        }

        public override void OnPause()
        {
            CloseCamera();
            StopBackgroundThread();
            base.OnPause();
        }

        private void RequestCameraPermission()
        {
            if (FragmentCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.Camera))
            {
                new PermissionsDialog().Show(ChildFragmentManager, FRAGMENT_DIALOG);
            }
            else
            {
                FragmentCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera },
                        REQUEST_CAMERA_PERMISSION);
            }
        }

        public void OnRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults)
        {
            if (requestCode != REQUEST_CAMERA_PERMISSION)
                return;

            if (grantResults.Length != 1 || grantResults[0] != (int)Permission.Granted)
            {
                ErrorDialog.NewInstance(GetString(Resource.String.get_permission))
                        .Show(ChildFragmentManager, FRAGMENT_DIALOG);
            }
        }


        // Sets up member variables related to camera.
        private void SetUpCameraOutputs(int width, int height)
        {
            var activity = Activity;
            var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            try
            {
                for (var i = 0; i < manager.GetCameraIdList().Length; i++)
                {
                    var thecameraId = manager.GetCameraIdList()[i];
                    CameraCharacteristics characteristics = manager.GetCameraCharacteristics(thecameraId);


                    // We don't use a front facing camera in this sample.
                    var facing = (Integer)characteristics.Get(CameraCharacteristics.LensFacing);
                    if (facing != null && facing == (Integer.ValueOf((int)LensFacing.Front)))
                    {
                        continue;
                    }

                    var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
                    if (map == null)
                    {
                        continue;
                    }

                    // For still image captures, we use the largest available size.
                    Size largest = (Size)Collections.Max(Arrays.AsList(map.GetOutputSizes((int)ImageFormatType.Jpeg)),
                        new GetSize());

                    //get the optimal highest quality size for the camera preview
                    Point displaySize = new Point();
                    activity.WindowManager.DefaultDisplay.GetSize(displaySize);
                    var rotatedPreviewWidth = height;
                    var rotatedPreviewHeight = width;
                    var maxPreviewWidth = displaySize.X;
                    var maxPreviewHeight = displaySize.Y;



                    if (maxPreviewWidth > MAX_PREVIEW_WIDTH)
                    {
                        maxPreviewWidth = MAX_PREVIEW_WIDTH;
                    }

                    if (maxPreviewHeight > MAX_PREVIEW_HEIGHT)
                    {
                        maxPreviewHeight = MAX_PREVIEW_HEIGHT;
                    }

                    // Danger, W.R.! Attempting to use too large a preview size could  exceed the camera
                    // bus' bandwidth limitation, resulting in gorgeous previews but the storage of
                    // garbage capture data.
                    Size[] testArray = map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture)));
                    //get the best size for the camera
                    cameraSize = testArray[0];

                    //assign the optimal size to the size
                    size = ChooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))),
                        rotatedPreviewWidth, rotatedPreviewHeight, maxPreviewWidth,
                        maxPreviewHeight, largest);

                    // We fit the aspect ratio of TextureView to the size of preview we picked.
                    var orientation = Resources.Configuration.Orientation;


                    // Check if the flash is supported.
                    var available = (Boolean)characteristics.Get(CameraCharacteristics.FlashInfoAvailable);
                    if (available == null)
                    {
                        FlashStatus = false;
                    }
                    else
                    {
                        FlashStatus = (bool)available;
                    }

                    cameraId = thecameraId;
                    return;
                }
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (NullPointerException e)
            {
                // Currently an NPE is thrown when the Camera2API is used but not supported on the
                // device this code runs.
                ErrorDialog.NewInstance(GetString(Resource.String.camera_error)).Show(ChildFragmentManager, FRAGMENT_DIALOG);
            }
        }



        // Opens the camera specified by {@link Camera2BasicFragment#cameraId}.
        public void OpenCamera(int width, int height)
        {
            if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) != Permission.Granted)
            {
                RequestCameraPermission();
                return;
            }
            SetUpCameraOutputs(width, height);
            var activity = Activity;
            var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            try
            {
                if (!cameraOpenCloseLock.TryAcquire(2500, TimeUnit.Microseconds))
                {
                    throw new RuntimeException("Time out waiting to lock camera opening.");
                }
                manager.OpenCamera(cameraId, stateCallBack, backHandler);


            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera opening.", e);
            }

        }

        // Closes the current {@link CameraDevice}.
        private void CloseCamera()
        {
            try
            {
                cameraOpenCloseLock.Acquire();
                if (null != cameraCaptureSession)
                {
                    cameraCaptureSession.Close();
                    cameraCaptureSession = null;
                }
                if (null != cameraDevice)
                {
                    cameraDevice.Close();
                    cameraDevice = null;
                }
                if (null != ImageReader)
                {
                    ImageReader.Close();
                    ImageReader = null;
                }
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera closing.", e);
            }
            finally
            {
                cameraOpenCloseLock.Release();
            }
        }

        // Starts a background thread and its {@link Handler}.
        private void StartBackgroundThread()
        {
            backThread = new HandlerThread("CameraBackground");
            backThread.Start();
            backHandler = new Handler(backThread.Looper);
        }

        // Stops the background thread and its {@link Handler}.
        private void StopBackgroundThread()
        {
            backThread.QuitSafely();
            try
            {
                backThread.Join();
                backThread = null;
                backHandler = null;
            }
            catch (InterruptedException e)
            {
                e.PrintStackTrace();
            }
        }

        // Creates a new {@link CameraCaptureSession} for camera preview.
        public void CreateCameraPreviewSession()
        {
            try
            {
                SurfaceTexture texture = textureView.SurfaceTexture;
                if (texture == null)
                {
                    throw new IllegalStateException("texture is null");
                }
                // We configure the size of default buffer to be the size of camera preview we want.
                textureView.SurfaceTexture.SetDefaultBufferSize(cameraSize.Width, cameraSize.Height);

                // This is the output Surface we need to start preview.
                Surface surface = new Surface(textureView.SurfaceTexture);

                
                // We set up a CaptureRequest.Builder with the output Surface.
                PreviewRequestBuilder = cameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
  
                //uses surface in the PreviewRequestBuilder
                PreviewRequestBuilder.AddTarget(surface);

                // Here, we create a CameraCaptureSession for camera preview.
                List<Surface> surfaces = new List<Surface>();
                surfaces.Add(surface);
                captureCallBack = new CameraCaptureListener(this);
                
                cameraDevice.CreateCaptureSession(surfaces, new CameraPreview(this), null);


            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        public static T Cast<T>(Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }



        public void OnClick(View v)
        {
            switch (v.Id)
            {
                //button to lock the focus
                case Resource.Id.focuslock_btn:
                    //lock the focus of the camera
                    if (focusLocked == false)
                    {
                        //set the focus status to locked
                        focusLocked = true;
                        //changes the image to look from unlocked to locked
                        Toast.MakeText(this.Activity, "Focus Locked", ToastLength.Short).Show();
                        focuslock_Btn.SetImageResource(Resource.Drawable.lockedimg);

                        //if the phone does not support the camera focus coded in the app, it will be assigned
                        //"old", otherwise it will be "new". These assignments will be used to decide how to
                        //focus the camera on that phone
                        if (phonemodel == "")
                        {
                            if (currentfocusstate == "0")
                            {
                                phonemodel = "old";
                            }
                            else
                                phonemodel = "new";
                        }



                        //For older phones, make it a manual focus by making the user tap the focus button until
                        //camera focus becomes clear
                        if (phonemodel == "old")
                        {
                            PreviewRequestBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.Auto);
                            PreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Start);
                            cameraCaptureSession.Capture(PreviewRequestBuilder.Build(), captureCallBack,
                                    backHandler);
                        }

                        //For newer phones, clicking the focus button freezes the focus when the autofocus gets
                        //clear
                        if (phonemodel == "new")
                        {
                            PreviewRequestBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.Off);
                            cameraCaptureSession.SetRepeatingRequest(PreviewRequestBuilder.Build(), captureCallBack,
                                    backHandler);
                        }
                        

                    }
                    //unlock the focus of the camera
                    else
                    {
                        //set the focus status to unlocked
                        focusLocked = false;

                        Toast.MakeText(this.Activity, "Focus Unlocked", ToastLength.Short).Show();
                        focuslock_Btn.SetImageResource(Resource.Drawable.unlockedimg);

                        //if the phone is "old", just make it so it's the same as above and manually focus
                        //with the button as it's the only thing the phone can do in this case
                        if (phonemodel == "old")
                        {
                            PreviewRequestBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.Auto);
                            PreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Start);
                            cameraCaptureSession.Capture(PreviewRequestBuilder.Build(), captureCallBack,
                                    backHandler);
                        }

                        //if the phone is "new", reverts it to a state of constant autofocus before they locked 
                        //the focus
                        if (phonemodel == "new")
                        {                            
                            PreviewRequestBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);
                            cameraCaptureSession.SetRepeatingRequest(PreviewRequestBuilder.Build(), captureCallBack,
                                    backHandler);
                        }
                    }

                    break;

                case Resource.Id.list_btn:
                    //opens the list of saved data
                    StartActivity(new Intent(Application.Context, typeof(SaveListActivity)));
                    break;

                //button to save data
                case Resource.Id.save_btn:
                    
                    //check if score has been taken yet
                        if (!scoreDisplay.Text.ToString().Equals(""))
                        {
                        //save data reading text into the list
                            Toast.MakeText(this.Activity, "Saved", ToastLength.Long).Show();
                            sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this.Activity);
                            ISharedPreferencesEditor editor = sharedPreferences.Edit();
                            var colours = sharedPreferences.GetString("scorelist", null);
                        //use a string builder
                            StringBuilder sb = new StringBuilder();

                        //separate the data by an !
                            sb.Append(colours).Append("!").Append(saveText);
                            if (colours == null)
                            {
                                sb.Delete(0, 5);
                            }
                            editor.PutString("scorelist", sb.ToString());
                            editor.Commit();
                        }
                        else
                        {
                        //if score has not been taken, alert the user to take a reading first
                            Toast.MakeText(this.Activity, "Error: No Score Taken", ToastLength.Long).Show();
                        }
                    
                    break;
                    
                    //take a reading and get the colour score of the egg yolk
                case Resource.Id.score:
                    string colorcode = "";

                    //get the pixels of the square marker that the reading will be taken in
                    int squarepixels = DpToPixels(45);

                    //get the bitmap (image) from the camera when a reading is taken
                    Bitmap bitmap = textureView.GetBitmap(textureView.Width, textureView.Height);

                    //red, green and blue values for RGB
                    double redColors = 0;
                    double greenColors = 0;
                    double blueColors = 0;

                    //to iterate through pixels
                    int pixelCount = 0;

                    //starting and end points for pixels of square marker
                    int markerwidthL = 0;
                    int markerwidthR = 0;
                    int markerheightUp = 0;
                    int markerheightDown = 0;

                    //set the start and end points of the marker
                       markerwidthL = ((textureView.Width / 2) - (squarepixels / 2));
                       markerwidthR = ((textureView.Width / 2) + (squarepixels / 2));
                       markerheightUp = ((textureView.Height / 2) - (squarepixels / 2));
                       markerheightDown = ((textureView.Height / 2) + (squarepixels / 2));

                   //for each pixel, get the red, green and blue values of the pixel
                   //add the square of each to get the total
                        for (int y = markerheightUp; y < markerheightDown; y++)
                        {
                            for (int x = markerwidthL; x < markerwidthR; x++)
                            {
                                int c = bitmap.GetPixel(x, y);
                                pixelCount++;
                                var colourtorgb = new Color(c);
                                redColors += (colourtorgb.R * colourtorgb.R);
                                greenColors += (colourtorgb.G * colourtorgb.G);
                                blueColors += (colourtorgb.B * colourtorgb.B);
                            }
                        }
                        //get the final RGB value from the reading by getting the square root of the average 
                        int red = Convert.ToInt32(Math.Sqrt(redColors / pixelCount));
                        int green = Convert.ToInt32(Math.Sqrt(greenColors / pixelCount));
                        int blue = Convert.ToInt32(Math.Sqrt(blueColors / pixelCount));

                        //get the RGB colour code, this part is only here if you want to use the RGB and color code in the
                        //future to reference the RGB values in the app to edit the calibration.
                        colorcode = "#" + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
                        string rgb = red + ", " + green + ", " + blue;

                        
                    //get the colour score off the egg yolk
                        string detectedcolour = yolkScores.GetColour(red, green, blue);
                        scoreDisplay.Text = detectedcolour;

                    string scoreText = "";
                    if (detectedcolour != "")
                    {
                        scoreText += "Score: " + detectedcolour + " ";
                    }

                    //for getting rgb and score
                    saveText = scoreText + "RGB:" + "(" + rgb + ") " + DateTime.Now.ToString("dd\\/MM\\/yyyy h\\:mm tt");

                    //for user
                    //saveText = "Score: " + detectedcolour + " (" + DateTime.Now.ToString("dd\\/MM\\/yyyy h\\:mm tt") + ")";


                    Toast.MakeText(this.Activity, saveText, ToastLength.Short).Show();

                   //delete the bitmap after doing the reading to make space for more readings
                    bitmap.Recycle();


                    break;

            }
        }

        //sets the correct aspect ratio for the camera preview
        public void setAspectRatioTextureView(int ResolutionWidth, int ResolutionHeight)
        {
            if (ResolutionWidth > ResolutionHeight)
            {
                int newWidth = imageDimensions.Width;
                int newHeight = ((imageDimensions.Width * ResolutionWidth) / ResolutionHeight);
                setTextureViewSize(newWidth, newHeight);

            }
            else
            {
                int newWidth = imageDimensions.Width;
                int newHeight = ((imageDimensions.Width * ResolutionHeight) / ResolutionWidth);
                setTextureViewSize(newWidth, newHeight);
            }

        }

        //sets the layouts to make sure the camera preview is not distorted
        public void setTextureViewSize(int viewWidth, int viewHeight)
        {
            textureView.LayoutParameters = (new FrameLayout.LayoutParams(viewWidth, viewHeight) { Gravity = GravityFlags.CenterVertical });
            relativeLayout.AddView(textureView, 0);
            RelativeLayout.LayoutParams newLayoutParams = (RelativeLayout.LayoutParams)textureView.LayoutParameters;
            newLayoutParams.AddRule(LayoutRules.CenterInParent);
        }

        //get the dimensions for the camera
        private Size GetDimensions()
        {
            var activity = Activity;
            var manager = (CameraManager)activity.GetSystemService(Context.CameraService);

            for (var i = 0; i < manager.GetCameraIdList().Length; i++)
            {
                var thecameraId = manager.GetCameraIdList()[i];
                CameraCharacteristics characteristics = manager.GetCameraCharacteristics(thecameraId);
                int[] focusmodes = (int[])characteristics.Get(CameraCharacteristics.ControlAfAvailableModes);
          
                

                var facing = (Integer)characteristics.Get(CameraCharacteristics.LensFacing);
                if (facing != null && facing == (Integer.ValueOf((int)LensFacing.Front)))
                {
                    continue;
                }
                var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
                if (map == null)
                {
                    continue;
                }

                Size[] arrayOfSizes = map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture)));
                if (arrayOfSizes[0].Height > arrayOfSizes[arrayOfSizes.Length -1].Height)
                {
                    cameraSize = arrayOfSizes[0];
                }
                else
                    cameraSize = arrayOfSizes[arrayOfSizes.Length - 1];

                string compvalues = (characteristics.Get(CameraCharacteristics.ControlAeCompensationRange)).ToString();
                

            }
            return cameraSize;
        }
        
        //convert dp to pixels (to match the size of the square marker used to take readings)
        public int DpToPixels(int dp)
        {
            int pixels = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Context.Resources.DisplayMetrics);
            return pixels;
        }

        
    }
}