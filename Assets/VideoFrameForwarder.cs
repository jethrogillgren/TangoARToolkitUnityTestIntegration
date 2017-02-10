using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Tango;

//WIP   Forward tango video frames to the ARToolkit NDK.
public class VideoFrameForwarder : MonoBehaviour, ITangoVideoOverlay, ITangoLifecycle {

	public string m_activityTangoARClass = "com.google.unity.UnityTangoARPlayer";

	private TangoApplication m_tangoApplication;
	private long fCount = 0;

	private System.IO.FileStream _FileStream;

	// Use this for initialization
	void Start () {
		JLog ("ITango Video Overlay Frame Forwarder  start()");

		// Open file for reading
		_FileStream = 
			new System.IO.FileStream(Application.persistentDataPath + "data.yv12", System.IO.FileMode.Create,
				System.IO.FileAccess.Write);
		// Writes a block of bytes to this stream using data from
		// a byte array.

		// close file stream


		m_tangoApplication = FindObjectOfType<TangoApplication>();
//		m_tangoApplication.m_enableVideoOverlay = true;
//		m_tangoApplication.m_videoOverlayUseByteBufferMethod = true;

		m_tangoApplication.Register(this);

		//        TangoEnvironmentalLighting t;
	}
		

	// Update is called once per frame
	void Update () {
		if( PluginFunctions.arwGetError() != 0 ) {
			JLogErr ("NDK ARToolkit Has errcode: " + PluginFunctions.arwGetError ());
		}

	}

//	void OnGUI() {
//		if(GUI.Button(new Rect(20,70,80,100), "Call Android Method"))
//		{
//			using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.google.unity.UnityTangoARPlayer"))
//			{
//				using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
//				{
//					obj_Activity .CallStatic("foo");
//				}
//			}
//		}
//	}

	/// <summary>
	/// This is called when the permission granting process is finished.
	/// </summary>
	/// <param name="permissionsGranted"><c>true</c> if permissions were granted, otherwise <c>false</c>.</param>
	public void OnTangoPermissions(bool permissionsGranted)
	{
	}
	/// <summary>
	/// This is called when successfully connected to the Tango service.
	/// </summary>
	public void OnTangoServiceConnected()
	{
	}

	/// <summary>
	/// This is called when disconnected from the Tango service.
	/// </summary>
	public void OnTangoServiceDisconnected()
	{
	}

	public void OnExperimentalTangoImageAvailable(Tango.TangoEnums.TangoCameraId id) {
		if(fCount == 0)
			JLog ( "ExperimentalTangoImageAvailable on " + id.ToString() );
		fCount++;
	}

	public void OnTangoImageMultithreadedAvailable(Tango.TangoEnums.TangoCameraId id, Tango.TangoImageBuffer buf) {
		if(fCount == 0)
			JLog ( "ExperimentalTangoImageAvailable on " + id.ToString() );
		fCount++;
	}

	/// <summary>
	/// This will be called when a new frame is available from the camera.
	/// </summary>
	/// <param name="cameraId">Camera identifier.</param>
	/// <param name="imageBuffer">Tango camera image buffer.</param>
	public void OnTangoImageAvailableEventHandler(TangoEnums.TangoCameraId cameraId, TangoUnityImageData imageBuffer)
	{


		if (fCount == 0) {
			TangoEnums.TangoImageFormatType enumDisplayStatus = (TangoEnums.TangoImageFormatType)imageBuffer.format;
			string stringValue = enumDisplayStatus.ToString ();
			JLog ("TangoImageAvailable on cameraId: " + cameraId + "  Format: " + imageBuffer.format + "(" + stringValue + "),  length: " + imageBuffer.data.Length + "  (" + imageBuffer.width + "x" + imageBuffer.height + "),  timestamp: " + imageBuffer.timestamp + ",  Stride: " + imageBuffer.stride);
		}

		fCount++;
//		if (_FileStream!= null && _FileStream.Length >= 600464512) {
//			JLog ("Closed File: " + _FileStream.Name);
//			_FileStream.Close();
//			_FileStream = null;
//		} else if ( _FileStream.CanWrite) {
//			JLog ("Write File: " + _FileStream.Length);
//			_FileStream.Write (imageBuffer.data, 0, imageBuffer.data.Length);
//		}

//		//For each YV12 frame  BROKEN
//		for (int i = 0; i < imageBuffer.width * imageBuffer.height; i+=36) {
//			for (int y = 0; y < 24; y++)
//				JLog ("Y: " + imageBuffer.data[i+y]);
//			for (int u = 24; u < 30; u++)
//				JLog ("U: " + imageBuffer.data[i+u]);
//			for (int v = 30; v < 36; v++)
//				JLog ("V: " + imageBuffer.data[i+v]);
//			JLog ("--------------");
			
//			size.total = size.width * size.height;
//			y = yuv [position.y * size.width + position.x];
//			u = yuv [(position.y / 2) * (size.width / 2) + (position.x / 2) + size.total];
//			v = yuv [(position.y / 2) * (size.width / 2) + (position.x / 2) + size.total + (size.total / 4)];
//		}

		if(fCount%100==0)
			callUnityTangoARPlayer ( "arwAcceptVideoImage", new object[] {imageBuffer.data} );

		//We are calling ARToolkit Library native function, through JNI, to feed in a frame of Tango video to the ARToolkit processing.
		/**
	     * Passes a video frame to the native library for processing.
	     *
	     * @param image               Buffer containing the video frame
	     * @param width               Width of the video frame in pixels
	     * @param height              Height of the video frame in pixels
	     * @param cameraIndex         Zero-based index of the camera in use. If only one camera is present, will be 0.
	     * @param cameraIsFrontFacing false if camera is rear-facing (the default) or true if camera is facing toward the user.
	     * @return true if no error occurred, otherwise false
	     */
		//public static native boolean arwAcceptVideoImage(byte[] image, int width, int height, int cameraIndex, boolean cameraIsFrontFacing);
		//public static native boolean arwAcceptVideoImage(byte[] image);

	}


	public struct RGB
	{
		private byte _r;
		private byte _g;
		private byte _b;

		public RGB(byte r, byte g, byte b)
		{
			this._r = r;
			this._g = g;
			this._b = b;
		}

		public byte R
		{
			get { return this._r; }
			set { this._r = value; }
		}

		public byte G
		{
			get { return this._g; }
			set { this._g = value; }
		}

		public byte B
		{
			get { return this._b; }
			set { this._b = value; }
		}

		public bool Equals(RGB rgb)
		{
			return (this.R == rgb.R) && (this.G == rgb.G) && (this.B == rgb.B);
		}
	}

	public struct YUV
	{
		private double _y;
		private double _u;
		private double _v;

		public YUV(double y, double u, double v)
		{
			this._y = y;
			this._u = u;
			this._v = v;
		}

		public double Y
		{
			get { return this._y; }
			set { this._y = value; }
		}

		public double U
		{
			get { return this._u; }
			set { this._u = value; }
		}

		public double V
		{
			get { return this._v; }
			set { this._v = value; }
		}

		public bool Equals(YUV yuv)
		{
			return (this.Y == yuv.Y) && (this.U == yuv.U) && (this.V == yuv.V);
		}
	}

	public static RGB YUVToRGB(YUV yuv)
	{
		byte r = (byte)(yuv.Y + 1.4075 * (yuv.V - 128));
		byte g = (byte)(yuv.Y - 0.3455 * (yuv.U - 128) - (0.7169 * (yuv.V - 128)));
		byte b = (byte)(yuv.Y + 1.7790 * (yuv.U - 128));

		return new RGB(r, g, b);
	}



//	public static Color YUV2Color(float y_value, float u_value, float v_value)
//	{
//		float r = y_value + 1.370705f * (v_value - 0.5f);
//		float g = y_value - 0.698001f * (v_value - 0.5f) - (0.337633f * (u_value - 0.5f));
//		float b = y_value + 1.732446f * (u_value - 0.5f);
//
//		return new Color(r, g, b, 1f);
//	}

	private void callUnityTangoARPlayer(string func, object[] args ) {
		using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass(m_activityTangoARClass))
		{
//			JLog ("Calling " + m_activityTangoARClass + "." + func + "   with args: " + args.ToString() ) ;
//			cls_UnityPlayer.CallStatic( func, args );
			cls_UnityPlayer.CallStatic(func, args);
		}
	}

	protected virtual void JLog(string val) {
		Debug.Log ("J# " + val);
	}
	protected virtual void JLogErr(string val) {
		Debug.LogError ("J# " + val);
	}
}