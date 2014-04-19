using UnityEngine;
using System.Collections;

public class IsometricCamera : OrbitCamera
{
	public float TiltAngle = 53.130102354155978703144387440907f; //Mathf.Atan( 4.0f / 3.0f )
	
	protected override void OnStart ()
	{
		base.OnStart ();
		
		m_orientationEA.x = TiltAngle;
	}
	
	protected override void OnUpdate ()
	{
		base.OnUpdate ();
		
		m_orientationEA.x = TiltAngle;
	}	
}

