using UnityEngine;
using System.Collections;
using Engine;

public class OrbitCamera : Engine.CameraBase
{
	[Range( 0.0f, 100.0f) ]
	public float FOV = 70.0f;
	
	public float Zoom = 10.0f;
	public Vector3 ViewOffset = Vector3.zero;
	public float Speed = 1.0f;
	
	public Vector3 ViewAngle
	{
		get
		{
			return m_orientationEA;
		}
		set
		{
			m_orientationEA = value;
		}			
	}
	
	protected virtual float Friction()
	{
		return m_friction;
	}
	
	protected virtual float GetSpeed()
	{
		return Speed;
	}
	
	protected override POV InitalPOV ()
	{
		return IdealPOV();
	}
	
	protected virtual POV IdealPOV()
	{
		POV idealPOV = new POV();
		
		Vector3 focalPoint = GetFocalPoint();
		
		idealPOV.Position = focalPoint + ( Quaternion.Euler( m_orientationEA ) * ( Vector3.forward * -Zoom ) );
		idealPOV.Orientation = Quaternion.Euler( m_orientationEA );	
		idealPOV.FOV = FOV;
			
		return idealPOV;
	}
	
	protected override void OnInitialised ()
	{
		m_previousFocalPoint = POV.Position + ( POV.Orientation * ( Vector3.forward * Zoom ) );
	}
	
	protected override POV CalculatePOV ()
	{
		POV pov = new POV();
				
		float speed = GetSpeed ();
		speed -= ( Friction() * speed );
		
		POV idealPOV = IdealPOV();
		
		pov.Position = CriticallyDampedSpring( idealPOV.Position, POV.Position, ref m_smoothVelocity, Time.deltaTime * speed );
		pov.Orientation = Quaternion.Euler( m_orientationEA );
		pov.FOV = FOV;
		
		m_previousFocalPoint = pov.Position + ( pov.Orientation * ( Vector3.forward * Zoom ) );
		
		return pov;
	}
	
	Vector3 CriticallyDampedSpring( Vector3 _target, Vector3 _current, ref Vector3 _velocity, float _timeStep )
	{
		return Vector3.SmoothDamp( _target, _current, ref _velocity, _timeStep );
	}
	
	protected virtual Vector3 GetFocalPoint()
	{
		return TargetTM.TransformPoint( ViewOffset );
	}
	
	protected override void OnUpdate ()
	{
		base.OnUpdate();
				
	}
	
	public void Pitch( float _deltaAngle )
	{
		float actualDeltaAngle = Mathf.Repeat( _deltaAngle, 360.0f );
		m_orientationEA.x += actualDeltaAngle;
	}
	
	protected Vector3 m_previousFocalPoint = Vector3.zero;
	 
	[SerializeField]
	protected Vector3 m_orientationEA = new Vector3( 0.0f, 0.0f, 0.0f );
	
	private Vector3 m_smoothVelocity = Vector3.zero;
	private float m_friction = 0.25f;
}

