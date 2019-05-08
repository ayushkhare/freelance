package md5fd90a3429e96b59e6ba4bef589d472ff;


public class ErrorDialog
	extends android.app.DialogFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateDialog:(Landroid/os/Bundle;)Landroid/app/Dialog;:GetOnCreateDialog_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Kemin_Yolk_Sensor.ErrorDialog, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ErrorDialog.class, __md_methods);
	}


	public ErrorDialog ()
	{
		super ();
		if (getClass () == ErrorDialog.class)
			mono.android.TypeManager.Activate ("Kemin_Yolk_Sensor.ErrorDialog, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public android.app.Dialog onCreateDialog (android.os.Bundle p0)
	{
		return n_onCreateDialog (p0);
	}

	private native android.app.Dialog n_onCreateDialog (android.os.Bundle p0);

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
