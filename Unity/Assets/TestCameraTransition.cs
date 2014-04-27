using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine;

public class TestCameraTransition : MonoBehaviour {

	public List<CameraBase> Cameras;
	public float TransitionDuration;

	public CameraManager CameraManager;

	[SerializeField]
	private int m_selectedCamIndex;
	private int m_lastSelectedCamIndex;

	// Use this for initialization
	void Start () {		
		m_lastSelectedCamIndex = m_selectedCamIndex;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_selectedCamIndex != m_lastSelectedCamIndex)
		{
			var newCamera = Cameras[m_selectedCamIndex];
			var newCameraGO = newCamera.gameObject;

			var clonedCameraGO = (GameObject)Object.Instantiate(newCameraGO);
			var clonedCamera = clonedCameraGO.GetComponent<CameraBase>();

			CameraManager.Transition (clonedCamera, new TimedBlendFactor(new BlendParams(TransitionDuration, BlendFunctions.EaseInOut)));
		}
						//CameraManager.Transition (Camera2, new TimedBlendFactor(new BlendParams(5.0f, BlendFunctions.EaseInOut)));
						//CameraManager.Transition (Camera2, new StaticBlendFactor (0.5f));
		
		m_lastSelectedCamIndex = m_selectedCamIndex;
	}
}
