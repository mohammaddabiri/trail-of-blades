using UnityEngine;
using System.Collections;

public class GroundEntity : MonoBehaviour {
	public Transform TargetTM;
	public LayerMask GroundLayer;
	public Transform GroundCentre;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var gravityDirection = TargetTM.position - GroundCentre.position;
		RaycastHit hitInfo;
		if(Physics.Raycast (TargetTM.position, -gravityDirection, out hitInfo, GroundLayer.value))
		{
			TargetTM.position = hitInfo.point;

			//TargetTM.rotation = Quaternion.Euler( gravityDirection);
		}
	}
}
