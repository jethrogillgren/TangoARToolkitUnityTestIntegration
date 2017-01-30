using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShoutStuff : MonoBehaviour {

	private static PluginFunctions.LogCallback logCallback = null;

	// Use this for initialization
	void Start () {
		PluginFunctions.arwSetVideoDebugMode (true);
		PluginFunctions.arwSetLogLevel (5);

		PluginFunctions.LogCallback c = new PluginFunctions.LogCallback (this.JLog);
		PluginFunctions.arwRegisterLogCallback (c);
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {

		StringBuilder builder = new StringBuilder();

		builder.AppendLine ("AR Toolkit Version " + PluginFunctions.arwGetARToolKitVersion ());
		builder.AppendLine ("Running?: " + PluginFunctions.arwIsRunning ());

		int width;
		int height;
		int pixelSize;
		string pixelFormatString;
		PluginFunctions.arwGetVideoParams(out width, out height, out pixelSize, out pixelFormatString);
		builder.AppendLine ("VideoParams: w:" + width + " h:"+height + " pixelSize:" + pixelSize + " formatStr:" + pixelFormatString);

		builder.AppendLine ("Current ErrCode: " + PluginFunctions.arwGetError ());

		debugAndToast( builder.ToString() );

	}

	private void debugAndToast(string str) {
		JLog ( str );
		AndroidHelper.ShowAndroidToastMessage ( str);
	}

	protected virtual void JLog(string val) {
		Debug.Log ("J# " + val);
	}
	protected virtual void JLogErr(string val) {
		Debug.LogError ("J# " + val);
	}
}
