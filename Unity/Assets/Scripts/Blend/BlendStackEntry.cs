using UnityEngine;
using System.Collections;

namespace Engine
{
	public class BlendStackEntry<ObjectType>
	{
		public delegate void OnExpiredDelegate( BlendStackEntry<ObjectType> entry );
		public event OnExpiredDelegate Expired;
		
		public ObjectType ActualObject
		{
			get
			{
				return m_actualObject;
			}
		}
		
		public IBlendFactor DesiredBlendContribution
		{
			set { m_desiredBlend = value; }				
			get { return m_desiredBlend; }
		}
		
		public float ActualBlendContribution
		{
			get { return m_actualBlend; }
			set 
			{ 
				m_actualBlend = value;
				
				if( m_previousBlendWeight > 0.0f && m_actualBlend == 0.0f )
				{
					if( Expired != null )
					{
						Expired( this );
					}
				}
				
				m_previousBlendWeight = m_actualBlend;		
			}
		}
		
		public BlendStackEntry( ObjectType contents )
		{
			m_actualObject = contents;
		}
		
		public BlendStackEntry( ObjectType contents, IBlendFactor blend )
		{
			m_actualObject = contents;
			DesiredBlendContribution = blend;
		}
		
		private ObjectType m_actualObject;
		private IBlendFactor m_desiredBlend;	
		private float m_actualBlend;	
		private float m_previousBlendWeight = 1.0f;
	}
}
