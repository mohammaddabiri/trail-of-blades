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
//			if( ActiveCamera != null )
//			{
//				ActiveCamera.InitCamera();
//			}
			Transition (ActiveCamera, new StaticBlendFactor (0.2f));
		}
		
		void OnGUI () {
			// Make a background box
			GUI.Box(new Rect(10,10,100,90), "Active Cameras\n");
			//			

			GUI.Label (new Rect (10, 30, 50, 50), m_blendStack.ToList ().Count.ToString ());
			//			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			//			if(GUI.Button(new Rect(20,40,80,20), "Level 1")) {
			//				Application.LoadLevel(1);
			//			}
			//			
			//			// Make the second button.
            //            if(GUI.Button(new Rect(20,70,80,20), "Level 2")) {
            //                Application.LoadLevel(2);
			//			}
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


//
//			if( ActiveCamera != null )
//			{
//				ActiveCamera.UpdateCamera();
//				ActiveCamera.UpdateView();
//				
				//POV finalPOV = ActiveCamera.POV;
				POV finalPOV = CalculatePOV();
				if(finalPOV != null)
				{
					m_actualCamera.fov = finalPOV.FOV;
					m_actualCamera.transform.localPosition = finalPOV.Position;
					m_actualCamera.transform.localRotation = finalPOV.Orientation;
				}
				
//				if( ShakeTM != null )
//				{
//					m_actualCamera.transform.localPosition += ShakeTM.localPosition;
//					m_actualCamera.transform.localRotation *= ShakeTM.localRotation;
//				}
//			}
		}

		public void Transition(CameraBase destination, IBlendFactor blend)
		{
			m_blendStack.Push (new BlendStackEntry<CameraBase> (destination, blend));
		}
		//	
	//	protected POV CalculatePOV()
	//	{
	//		return ActiveCamera.POV;
	//	}

		[SerializeField]
		private Camera m_actualCamera;
		
		BlendStack<CameraBase> m_blendStack = new BlendStack<CameraBase>();
		//private Engine.CameraBlender m_blender = new CameraBlender();
	}
}