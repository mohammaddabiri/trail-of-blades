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

//		// Use this for initialization
//		void Start () {
//		
//		}


		public POV POV {
			get;
			private set;
		}
		
		public void AddChild( CameraBase _camera, float _blendWeight )
		{
			Entry newChild = new Entry();

			newChild.Camera = _camera;
			newChild.Weight = _blendWeight;

			Children.Add( newChild );
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
			POV finalPOV;
			POV localPOV;
			Entry currentChild;
			int childIndex;
			Entry lastChild;
			
			if( Children.Count != 0 )
			{
				lastChild = Children[Children.Count - 1];
				
				localPOV = lastChild.Camera.POV;
				finalPOV = localPOV;
				
				//( finalPOV * Children[0].BlendWeight ) + ( 1 - Children[0].BlendWeight ) * otherChildren.POV;
				for( childIndex = Children.Count - 2; childIndex >= 0; --childIndex )
				{
					currentChild = Children[childIndex];
					localPOV = currentChild.Camera.POV;
					
					finalPOV = BlendPOVs( finalPOV, localPOV, currentChild.Weight );
				}
				
				POV = finalPOV;
			}
		}

//			void Update ()
//			{			
//				m_blendedCameras.Update ();
//				
//			foreach(var activeCamera in m_activeCameras)
//			{
//				float contribution = m_blendedCameras.GetContribution(activeCamera);
//			}
////				if( ActiveCamera != null )
////				{
////					ActiveCamera.UpdateCamera();
////					ActiveCamera.UpdateView();
////					
////					POV finalPOV = ActiveCamera.POV;
////					m_actualCamera.fov = finalPOV.FOV;
////					m_actualCamera.transform.localPosition = finalPOV.Position;
////					m_actualCamera.transform.localRotation = finalPOV.Orientation;
////					
////					if( ShakeTM != null )
////					{
////						m_actualCamera.transform.localPosition += ShakeTM.localPosition;
////						m_actualCamera.transform.localRotation *= ShakeTM.localRotation;
////					}
////				}
////			}
//
//		public void TransitionTo(CameraBase camera, IBlendFactor transitionBlend)
//		{
//			m_activeCameras.Add (camera);
//			m_blendedCameras.Push (camera, transitionBlend);
//		}

		[SerializeField]
		private List<CameraBase> m_activeCameras = new List<CameraBase> ();

		//private POV m_pov = new POV();

		private List<Entry> Children = new List<Entry>();
	}
}