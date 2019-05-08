package md5fd90a3429e96b59e6ba4bef589d472ff;


public class CameraFunction_ShowToastRunnable
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.Runnable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_run:()V:GetRunHandler:Java.Lang.IRunnableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Kemin_Yolk_Sensor.CameraFunction+ShowToastRunnable, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CameraFunction_ShowToastRunnable.class, __md_methods);
	}


	public CameraFunction_ShowToastRunnable ()
	{
		super ();
		if (getClass () == CameraFunction_ShowToastRunnable.class)
			mono.android.TypeManager.Activate ("Kemin_Yolk_Sensor.CameraFunction+ShowToastRunnable, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CameraFunction_ShowToastRunnable (android.content.Context p0, java.lang.String p1)
	{
		super ();
		if (getClass () == CameraFunction_ShowToastRunnable.class)
			mono.android.TypeManager.Activate ("Kemin_Yolk_Sensor.CameraFunction+ShowToastRunnable, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1 });
	}


	public void run ()
	{
		n_run ();
	}

	private native void n_run ();

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
