using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Engine{
	[RequireComponent( typeof(Camera) )]
	[ExecuteInEditMode]
	public class CameraManager : MonoBehaviour
	{	
		public CameraBase ActiveCamera;
		public Transform ShakeTM;
		
		void Awake()
		{
			m_actualCamera = GetComponent<Camera>();
		}
		
		// Use this for initialization
		void Start ()
		{
			Transition (ActiveCamera, new StaticBlendFactor (0.2f));
		}
		
		void OnGUI () {
			// Make a background box
			GUI.Box(new Rect(10,10,500,400), "Active Cameras\n");
			//			
			int row = 0;
			foreach( var camera in m_blendStack )
			{
				var contribution = m_blendStack.GetContribution(camera);
				GUI.Label (new Rect (10, 30 + (row * 40), 50, 50), string.Concat( camera.gameObject.name + ": " + contribution.ToString()));
				row++;
			}
			//GUI.Label (new Rect (10, 30, 50, 50), m_blendStack.ToList ().Count.ToString ());
		
		}

		POV CalculatePOV()
		{
			m_blendStack.Update ();

			CameraBlender blender = new CameraBlender ();

			foreach(var camera in m_blendStack )
			{
				camera.UpdateCamera();
				camera.UpdateView();
				
				var cameraContribution = m_blendStack.GetContribution(camera);
				blender.AddChild(camera, cameraContribution );
			}
			
			blender.Evaluate ();
			return blender.POV;
		}

		// Update is called once per frame
		void Update ()
		{
			POV finalPOV = CalculatePOV();
			if(finalPOV != null)
			{
				m_actualCamera.fov = finalPOV.FOV;
				m_actualCamera.transform.localPosition = finalPOV.Position;
				m_actualCamera.transform.localRotation = finalPOV.Orientation;
			}
			
//			if( ShakeTM != null )
//			{
//				m_actualCamera.transform.localPosition += ShakeTM.localPosition;
//				m_actualCamera.transform.localRotation *= ShakeTM.localRotation;
//			}
		}

		public void Transition(CameraBase destination, IBlendFactor blend)
		{
			m_blendStack.Push (new BlendStackEntry<CameraBase> (destination, blend));
		}

		[SerializeField]
		private Camera m_actualCamera;
		
		BlendStack<CameraBase> m_blendStack = new BlendStack<CameraBase>();
	}
}