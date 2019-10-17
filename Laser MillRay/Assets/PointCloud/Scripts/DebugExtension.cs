using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUnity {
	public static bool IsEditor
	{
		get
		{
		#if UNITY_EDITOR
					return true;
		#else
				return false;
		#endif
		}
	}
	public static void Log(object message)
	{
	#if UNITY_EDITOR
		Debug.Log(message);
	#endif
	}

}
