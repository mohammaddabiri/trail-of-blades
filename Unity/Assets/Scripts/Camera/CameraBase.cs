using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Engine
{
	public class POV
	{
		public Vector3 Position;
		public Quaternion Orientation;
		public float FOV = 70.0f;
	}

	public abstract class CameraBase : MonoBehaviour, IEqualityComparer<CameraBase>
	{
		#region IEqualityComparer implementation

		bool IEqualityComparer<CameraBase>.Equals (CameraBase x, CameraBase y)
		{
			return x.name == y.name;
		}

		int IEqualityComparer<CameraBase>.GetHashCode (CameraBase obj)
		{
			return obj.GetHashCode ();
		}

		#endregion

		public Transform TargetTM;
		
		public POV POV 
		{
			get { return this.m_pov; }
		}

		private POV m_pov = new POV();
			
		void Update()
		{
			// Do nothing.
		}
		
		void Start()
		{
			// Do nothing.
			InitCamera ();
		}
		
		// Use this for initialization
		public void InitCamera ()
		{
			OnStart();
			
			m_pov = InitalPOV();
		}
		
		// Update is called once per frame
		public void UpdateCamera ()
		{		
			OnUpdate();
		}
		
		public void UpdateView()
		{		
			m_pov = CalculatePOV();
		}
		
		protected virtual void OnStart()
		{
			// Do Nothing.
		}
		
		protected virtual void OnUpdate()
		{
			// Do Nothing.
		}
		
		protected virtual void OnInitialised()
		{
			// Do Nothing.
		}
		
		protected abstract POV InitalPOV();
		protected abstract POV CalculatePOV();
	}

}