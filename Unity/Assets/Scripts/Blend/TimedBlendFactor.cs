using UnityEngine;
using System.Collections;

namespace Engine
{
	/// <summary>
	/// A blend which ramps up from 0 to 1.0 based on as time elapses..
	/// </summary>
	public class TimedBlendFactor : IBlendFactor
	{
		public delegate void TimedBlendEventDelgate( TimedBlendFactor source ); 
		public event TimedBlendEventDelgate OnPeaked;

		public float Phase
		{
			get 
			{
				if( Blend.Duration == 0.0f || m_blendTimeToGo == 0.0f )
				{
					return 1.0f;
				}
				
				return (Blend.Duration - m_blendTimeToGo) / Blend.Duration;
			}
			set 
			{					
				if( value < 0 || value > 1 )
				{
					throw new System.ArgumentOutOfRangeException( "Expected argument to be in range 0.0 and 1.0." );
				}
				
				m_blendTimeToGo = Blend.Duration * value;
			}
		}		
		
		public TimedBlendFactor()
		{
		}
		
		public TimedBlendFactor( BlendParams _blendParams )
		{
			Blend = _blendParams;
		}
		
		public TimedBlendFactor( BlendParams _blendParams, float _initialPhase )
		{
			
			Blend = _blendParams;
		}
		
		public float BlendWeight
		{
			set 
			{ 
				if( m_blendWeight != value )
				{
					m_previousBlendWeight = m_blendWeight;
					m_blendWeight = value;
					
					if( m_blendWeight == 1.0f && m_previousBlendWeight != 1.0f )
					{
						if( OnPeaked != null )
						{
							OnPeaked( this );
						}
					}
				}
			}				
			get { return m_blendWeight; }
		}
				
		public BlendParams Blend
		{
			set 
			{ 
				if( value == null )
				{
					throw new System.ArgumentNullException( "Blend cannot be null." );
				}
				m_blend = value; 
				m_blendTimeToGo = value.Duration;
				Update( 0.0f );
			}
			get
			{
				return m_blend;
			}
		}		
		
		public void Update( float _deltaTime )
		{
			m_blendTimeToGo = Mathf.Max( 0.0f, m_blendTimeToGo - _deltaTime );
			
			float blendPct = 1.0f;
			
			if( m_blendTimeToGo > 0 )
			{
				float durationPct = (Blend.Duration - m_blendTimeToGo) / Blend.Duration;
				
				
				//TODO: Consider integrating blend resolution to a lib like iTween which comes with support for much
				// more blend types out of the box.
				
				switch (Blend.BlendFunction)
				{
				case BlendFunctions.Linear:
					blendPct = Mathf.Lerp( 0.0f, 1.0f, durationPct);
					break;
				case BlendFunctions.Cubic:
					break;
				case BlendFunctions.EaseIn:
					blendPct = Mathfx.Sinerp( 0.0f, 1.0f, durationPct );
					break;
				case BlendFunctions.EaseOut:
					blendPct = Mathfx.Sinerp( 0.0f, 1.0f, -durationPct );
					break;
				case BlendFunctions.EaseInOut:
					blendPct = Mathfx.Hermite( 0.0f, 1.0f, durationPct );
					break;
				default:
					break;
				}
			}
			
			BlendWeight = blendPct;
		}
		
		private BlendParams m_blend;
		private float m_blendWeight = 0.0f;
		private float m_blendTimeToGo = 0.0f;
		private float m_previousBlendWeight = 0.0f;
	}
}