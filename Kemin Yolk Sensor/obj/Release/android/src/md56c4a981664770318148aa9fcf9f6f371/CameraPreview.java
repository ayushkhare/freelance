package md56c4a981664770318148aa9fcf9f6f371;


public class CameraPreview
	extends android.hardware.camera2.CameraCaptureSession.StateCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onConfigureFailed:(Landroid/hardware/camera2/CameraCaptureSession;)V:GetOnConfigureFailed_Landroid_hardware_camera2_CameraCaptureSession_Handler\n" +
			"n_onConfigured:(Landroid/hardware/camera2/CameraCaptureSession;)V:GetOnConfigured_Landroid_hardware_camera2_CameraCaptureSession_Handler\n" +
			"";
		mono.android.Runtime.register ("Kemin_Yolk_Sensor.Listeners.CameraPreview, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CameraPreview.class, __md_methods);
	}


	public CameraPreview ()
	{
		super ();
		if (getClass () == CameraPreview.class)
			mono.android.TypeManager.Activate ("Kemin_Yolk_Sensor.Listeners.CameraPreview, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CameraPreview (md5fd90a3429e96b59e6ba4bef589d472ff.CameraFunction p0)
	{
		super ();
		if (getClass () == CameraPreview.class)
			mono.android.TypeManager.Activate ("Kemin_Yolk_Sensor.Listeners.CameraPreview, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Kemin_Yolk_Sensor.CameraFunction, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onConfigureFailed (android.hardware.camera2.CameraCaptureSession p0)
	{
		n_onConfigureFailed (p0);
	}

	private native void n_onConfigureFailed (android.hardware.camera2.CameraCaptureSession p0);


	public void onConfigured (android.hardware.camera2.CameraCaptureSession p0)
	{
		n_onConfigured (p0);
	}

	private native void n_onConfigured (android.hardware.camera2.CameraCaptureSession p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
