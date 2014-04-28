using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine;

namespace Engine
{
	public class CameraBlender 
	{
		private class Entry
		{
			public CameraBase Camera;
			public float Weight;
		}

		public POV POV {
			get;
			private set;
		}
		
		public void AddChild( CameraBase _camera, float _blendWeight )
		{
			Entry newChild = new Entry();

			newChild.Camera = _camera;
			newChild.Weight = _blendWeight;

			Children.Add(newChild);
		}
		
		static POV BlendPOVs( POV _a, POV _b, float _alpha )
		{
			POV	pov = new POV();
			
			pov.Position = Vector3.Lerp( _a.Position, _b.Position, _alpha );
			pov.FOV			= Mathf.Lerp( _a.FOV, _b.FOV, _alpha );
			pov.Orientation	= Quaternion.Lerp( _a.Orientation, _b.Orientation, _alpha );
			
			return pov;
		}
		
		public void Evaluate()
		{
			if( Children.Count != 0 )
			{
				var lastChild = Children[Children.Count - 1];
				
				var localPOV = lastChild.Camera.POV;
				var finalPOV = localPOV;
				
				//( finalPOV * Children[0].BlendWeight ) + ( 1 - Children[0].BlendWeight ) * otherChildren.POV;
				for( int childIndex = Children.Count - 2; childIndex >= 0; --childIndex )
				{
					var currentChild = Children[childIndex];
					localPOV = currentChild.Camera.POV;
					
					finalPOV = BlendPOVs( finalPOV, localPOV, currentChild.Weight );
				}
				POV = finalPOV;
			}
		}

		[SerializeField]
		private List<CameraBase> m_activeCameras = new List<CameraBase> ();
		private List<Entry> Children = new List<Entry>();
	}
}