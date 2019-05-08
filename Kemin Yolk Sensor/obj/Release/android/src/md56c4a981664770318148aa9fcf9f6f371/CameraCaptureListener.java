package md56c4a981664770318148aa9fcf9f6f371;


public class CameraCaptureListener
	extends android.hardware.camera2.CameraCaptureSession.CaptureCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCaptureCompleted:(Landroid/hardware/camera2/CameraCaptureSession;Landroid/hardware/camera2/CaptureRequest;Landroid/hardware/camera2/TotalCaptureResult;)V:GetOnCaptureCompleted_Landroid_hardware_camera2_CameraCaptureSession_Landroid_hardware_camera2_CaptureRequest_Landroid_hardware_camera2_TotalCaptureResult_Handler\n" +
			"n_onCaptureProgressed:(Landroid/hardware/camera2/CameraCaptureSession;Landroid/hardware/camera2/CaptureRequest;Landroid/hardware/camera2/CaptureResult;)V:GetOnCaptureProgressed_Landroid_hardware_camera2_CameraCaptureSession_Landroid_hardware_camera2_CaptureRequest_Landroid_hardware_camera2_CaptureResult_Handler\n" +
			"";
		mono.android.Runtime.register ("Kemin_Yolk_Sensor.Listeners.CameraCaptureListener, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CameraCaptureListener.class, __md_methods);
	}


	public CameraCaptureListener ()
	{
		super ();
		if (getClass () == CameraCaptureListener.class)
			mono.android.TypeManager.Activate ("Kemin_Yolk_Sensor.Listeners.CameraCaptureListener, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CameraCaptureListener (md5fd90a3429e96b59e6ba4bef589d472ff.CameraFunction p0)
	{
		super ();
		if (getClass () == CameraCaptureListener.class)
			mono.android.TypeManager.Activate ("Kemin_Yolk_Sensor.Listeners.CameraCaptureListener, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Kemin_Yolk_Sensor.CameraFunction, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onCaptureCompleted (android.hardware.camera2.CameraCaptureSession p0, android.hardware.camera2.CaptureRequest p1, android.hardware.camera2.TotalCaptureResult p2)
	{
		n_onCaptureCompleted (p0, p1, p2);
	}

	private native void n_onCaptureCompleted (android.hardware.camera2.CameraCaptureSession p0, android.hardware.camera2.CaptureRequest p1, android.hardware.camera2.TotalCaptureResult p2);


	public void onCaptureProgressed (android.hardware.camera2.CameraCaptureSession p0, android.hardware.camera2.CaptureRequest p1, android.hardware.camera2.CaptureResult p2)
	{
		n_onCaptureProgressed (p0, p1, p2);
	}

	private native void n_onCaptureProgressed (android.hardware.camera2.CameraCaptureSession p0, android.hardware.camera2.CaptureRequest p1, android.hardware.camera2.CaptureResult p2);

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
