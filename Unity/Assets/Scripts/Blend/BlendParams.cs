using UnityEngine;
using System.Collections;

namespace Engine
{
	public enum BlendFunctions
	{
		Linear,
		Cubic,
		EaseIn,
		EaseOut,
		EaseInOut,
	}

	[System.Serializable]
	public class BlendParams
	{
		public float Duration = 0.2f;
		public BlendFunctions BlendFunction;
		public static BlendParams Immediate = new BlendParams( 0.0f );
		
		public BlendParams()
		{
		}
		
		public BlendParams( float _duration )
		{
			Duration = _duration;
		}
		
		public BlendParams( float _duration, BlendFunctions _blendFunc )
		{
			Duration =_duration;
			BlendFunction = _blendFunc;
		}
	}
}