
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class OnEditorInitialise
{
	
	static OnEditorInitialise()
	{
		EditorApplication.update += RunOnce;
	}

	static void RunOnce()
	{
		Debug.Log("RunOnce!");

		// pick a random color
		Color newColor = new Color( UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1.0f );
		// apply it on current object's material
		if( Object.FindObjectOfType<ShoutStuff>() != null ) 
			Object.FindObjectOfType<ShoutStuff>().gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = newColor;

		EditorApplication.update -= RunOnce;
	}
}
