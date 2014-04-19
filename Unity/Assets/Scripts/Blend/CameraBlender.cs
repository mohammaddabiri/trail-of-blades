using UnityEngine;
using System.Collections;
using Engine;

namespace Engine
{
	public class CameraBlender : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			m_blendedCameras.Update ();
			//POV = m_blendedCameras.GetContribution ();
		}

		public POV POV {
			get;
			private set;
		}

		public void TransitionTo(CameraBase camera, IBlendFactor transitionBlend)
		{
			m_blendedCameras.Push (camera, transitionBlend);
		}

		BlendStack<CameraBase> m_blendedCameras = new BlendStack<CameraBase>();
		private POV m_pov = new POV();
	}
}