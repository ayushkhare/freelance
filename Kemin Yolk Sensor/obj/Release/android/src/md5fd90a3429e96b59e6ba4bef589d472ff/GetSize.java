package md5fd90a3429e96b59e6ba4bef589d472ff;


public class GetSize
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.util.Comparator
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_compare:(Ljava/lang/Object;Ljava/lang/Object;)I:GetCompare_Ljava_lang_Object_Ljava_lang_Object_Handler:Java.Util.IComparatorInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_equals:(Ljava/lang/Object;)Z:GetEquals_Ljava_lang_Object_Handler:Java.Util.IComparatorInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Kemin_Yolk_Sensor.GetSize, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GetSize.class, __md_methods);
	}


	public GetSize ()
	{
		super ();
		if (getClass () == GetSize.class)
			mono.android.TypeManager.Activate ("Kemin_Yolk_Sensor.GetSize, Kemin Yolk Sensor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public int compare (java.lang.Object p0, java.lang.Object p1)
	{
		return n_compare (p0, p1);
	}

	private native int n_compare (java.lang.Object p0, java.lang.Object p1);


	public boolean equals (java.lang.Object p0)
	{
		return n_equals (p0);
	}

	private native boolean n_equals (java.lang.Object p0);

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
