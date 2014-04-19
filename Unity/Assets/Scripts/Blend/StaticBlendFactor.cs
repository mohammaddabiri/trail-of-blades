using UnityEngine;
using System.Collections;

namespace Engine
{
	public class StaticBlendFactor : IBlendFactor
	{		
		public float BlendWeight
		{
			get{ return m_blendWeight; }
			set{ m_blendWeight = value; }
		}
		
		public StaticBlendFactor( float _weight )
		{
			BlendWeight = _weight;
		}
		
		private float m_blendWeight;
	}
}