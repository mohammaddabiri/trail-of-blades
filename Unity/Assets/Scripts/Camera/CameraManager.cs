using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Engine{
	[RequireComponent( typeof(Camera) )]
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
			if( ActiveCamera != null )
			{
				ActiveCamera.InitCamera();
			}	
		}
		
		// Update is called once per frame
		void Update ()
		{
			if( ActiveCamera != null )
			{
				ActiveCamera.UpdateCamera();
				ActiveCamera.UpdateView();
				
				POV finalPOV = ActiveCamera.POV;
				m_actualCamera.fov = finalPOV.FOV;
				m_actualCamera.transform.localPosition = finalPOV.Position;
				m_actualCamera.transform.localRotation = finalPOV.Orientation;
				
				if( ShakeTM != null )
				{
					m_actualCamera.transform.localPosition += ShakeTM.localPosition;
					m_actualCamera.transform.localRotation *= ShakeTM.localRotation;
				}
			}
		}
	//	
	//	protected POV CalculatePOV()
	//	{
	//		return ActiveCamera.POV;
	//	}
		
		private Camera m_actualCamera;
	}

}