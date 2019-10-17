using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyExtensions
{
	public static class ExtensionMethods{
		public static Vector3 ToVector3(this Quaternion q)
		{
			
			return new Vector3(q.x,q.y,q.z);
		}
	}
}
