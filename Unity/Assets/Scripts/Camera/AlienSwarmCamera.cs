using UnityEngine;
using System.Collections;
using Engine;

public class AlienSwarmCamera : IsometricCamera
{
	public Vector2 MaxDistanceToTarget = new Vector2( 5.0f, 5.0f );
	public float MaxFriction = 0.2f;
	public float MinFriction = 0.2f;
	public float MaxMouseDistance = 0.707f;
	
	public Vector3 FocalPoint
	{
		get { return m_focalPoint; }
	}
	
	protected Vector3 IdealFocalPoint ()
	{
		Vector3 mouseVS = camera.ScreenToViewportPoint( Input.mousePosition );
		mouseVS.x = Mathf.Clamp01( mouseVS.x );
		mouseVS.y = Mathf.Clamp01( mouseVS.y );
		
		return CalcFocalPoint( mouseVS );
	}
		
	private Vector3 CalcFocalPoint( Vector3 cursorPositionSS )
	{
		// Determine how many percent of the maximum offset we have moved from the centre of the screen.
		Vector3 dragOffset = cursorPositionSS - s_screenMidpointVS;
		float dragRadius = dragOffset.magnitude;
		
		float dragPct = dragRadius / MaxMouseDistance;
		dragPct = Mathf.Min( dragPct, 1.0f );
		
		// EaseInOut
		float distanceFactor = Mathfx.Hermite( 0.0f, 1.0f, dragPct );
		distanceFactor = Mathf.Min( 1.0f, distanceFactor );
		
		// Convert screen space offset to local space 
		Vector3 targetOffsetLS = new Vector3( dragOffset.x, 0.0f, dragOffset.y );
		
		if (TargetTM == null)
			return Vector3.zero;
		
		// Take into account the horizontal and vertical offset distances separately. 
		Vector3 offsetFactor = targetOffsetLS.normalized;
		offsetFactor.x *= distanceFactor * MaxDistanceToTarget.x;
		offsetFactor.z *= distanceFactor * MaxDistanceToTarget.y;
		
		Vector3 idealFocalPoint = TargetTM.position + ViewOffset + offsetFactor;		
		return idealFocalPoint;
	}
	
	protected override Vector3 GetFocalPoint ()
	{
		return m_focalPoint;
	}
	
	protected override float Friction ()
	{	
		if (null == TargetTM)
			return 0;
		
		Vector3 offsetToDesiredFocalPoint = ( m_focalPoint - TargetTM.position );
		offsetToDesiredFocalPoint.y = 0.0f;
		
		float focalPointDeltaDistance = ( offsetToDesiredFocalPoint ).magnitude;
		float pctOfMaxDeltaDistance = focalPointDeltaDistance / ( MaxDistanceToTarget.magnitude  );
		pctOfMaxDeltaDistance = Mathf.Min( pctOfMaxDeltaDistance, 1.0f );
		
		return Mathf.Lerp( MinFriction, MaxFriction, pctOfMaxDeltaDistance );
	}
		
	protected override void OnInitialised ()
	{
		base.OnInitialised ();
		
		if( TargetTM != null )
		{
			m_focalPoint = IdealFocalPoint();
		}
	}
		
	private void DrawDebugX( Vector3 _point, Color _color, float _length )
	{
		Debug.DrawLine( _point, _point + ( Vector3.right * ( _length / 2.0f ) ), _color, 0.0f );
		Debug.DrawLine( _point, _point - ( Vector3.right * ( _length / 2.0f ) ), _color, 0.0f );
		Debug.DrawLine( _point, _point + ( Vector3.forward * ( _length / 2.0f ) ), _color, 0.0f );
		Debug.DrawLine( _point, _point - ( Vector3.forward * ( _length / 2.0f ) ), _color, 0.0f );
	}
	
	public void SetDesiredFOV( float fov, float time )
	{	
		m_previousDesiredCameraFOV = POV.FOV;
		m_desiredCameraFOV = fov;
		m_cameraFOVTimer = new TimedBlendFactor( new BlendParams( time ) );
	}
	
	private float CalcFOV()
	{		
		return Mathf.Lerp( m_previousDesiredCameraFOV, m_desiredCameraFOV, m_cameraFOVTimer.BlendWeight );
	}
	
	protected override POV CalculatePOV ()
	{
		POV pov = base.CalculatePOV();
		pov.FOV = CalcFOV();
		
		return pov;
	}
	
	protected override void OnUpdate ()
	{		
		base.OnUpdate ();
		 
		m_focalPoint = IdealFocalPoint();
		m_cameraFOVTimer.Update( Time.deltaTime );
		
#if UNITY_EDITOR
		DrawDebugX( m_focalPoint, Color.red, 2.0f );
#endif
	}
	
	private static Vector3 s_screenMidpointVS = new Vector3( 0.5f, 0.5f, 0.0f );
	private Vector3 m_focalPoint; 
	
	private Engine.TimedBlendFactor m_cameraFOVTimer = new Engine.TimedBlendFactor ();
	private float m_desiredCameraFOV = 55.0f;
	private float m_previousDesiredCameraFOV = 55.0f;
}