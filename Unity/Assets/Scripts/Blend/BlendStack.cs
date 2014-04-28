using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{	
	public class BlendStack<ObjectType> : IEnumerable<ObjectType> where ObjectType: IEqualityComparer<ObjectType>
	{		
		public delegate void ItemExpiredDelegate( ObjectType item );
		public event ItemExpiredDelegate OnItemExpired;

		#region IEnumerable implementation

		public IEnumerator<ObjectType> GetEnumerator ()
		{
			foreach (var node in m_blendNodes)
				yield return node.ActualObject;
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion
				
		public ObjectType this[int index]
		{
			get
			{
				return m_blendNodes[index].ActualObject;
			}
		}

		public int Count
		{
			get
			{
				return this.m_blendNodes.Count;
			}
		}

		protected void OnStackEntryExpired( BlendStackEntry<ObjectType> entry )
		{
			entry.Expired -= OnStackEntryExpired;
			
			ObjectType expiredObject = entry.ActualObject;
			
			Remove( entry );
			
			if( OnItemExpired != null )
			{
				OnItemExpired( expiredObject );
			}
		}
		
		public void Push( BlendStackEntry<ObjectType> newEntry )			
		{
			newEntry.Expired += OnStackEntryExpired;
			m_blendNodes.Insert( 0, newEntry );
			m_objToEntryMap.Add( newEntry.ActualObject, newEntry );
		}
		
		public void Push( ObjectType item, IBlendFactor blend )
		{
			BlendStackEntry<ObjectType> newEntry = new BlendStackEntry<ObjectType>( item, blend );
			Push( newEntry );
		}
		 
		protected void Blend()
		{			
			float remainingBlend = 1.0f;
					 	
			List<BlendStackEntry<ObjectType>> toRemove = new List<BlendStackEntry<ObjectType>> ();

			for( int index = 0; index < m_blendNodes.Count; ++index )
			{			
				if(m_blendNodes.Count > 1)
				{
					int boo = 2;
				}

				BlendStackEntry<ObjectType> entry = m_blendNodes[index] as BlendStackEntry<ObjectType>;
				entry.Update();

				float desiredBlendContribution = entry.DesiredBlendContribution.BlendWeight;
				float actualBlendContribution = Mathf.Min( remainingBlend, desiredBlendContribution );
				
				remainingBlend -= actualBlendContribution;
						
				entry.ActualBlendContribution = actualBlendContribution;
//
//				if(actualBlendContribution == 0.0f)
//					toRemove.Add(entry);
			}			
//
//			for(int i = 0; i < toRemove.Count; ++i)
//			{
//				Remove(toRemove[i]);
//			}
		}
		
		public bool Contains( ObjectType entryObj )
		{
			return entryObj != null && m_objToEntryMap.ContainsKey( entryObj );
		}
		
		public float GetContribution( ObjectType entryObj )
		{
			if( entryObj == null || !m_objToEntryMap.ContainsKey( entryObj ) )
			{
				return 0.0f;
			}
			
			BlendStackEntry<ObjectType> foundEntry = m_objToEntryMap[entryObj];
			
			if( foundEntry != null )
			{
				return foundEntry.ActualBlendContribution;	
			}	
			
			return 0.0f;
		}
						
		public void Remove( BlendStackEntry<ObjectType> entryToRemove )
		{
			//m_objToEntryMap.Remove( entryToRemove.ActualObject );
			//m_blendNodes.Remove( entryToRemove );
		}
		
		public void Remove( ObjectType entryToRemove )
		{
//			BlendStackEntry<ObjectType> foundEntry = m_objToEntryMap[entryToRemove];
//			
//			if( foundEntry != null )
//			{ 
//				Remove( foundEntry );
//			}
		}
		
		public void Update ()
		{
			Blend();
		}

		private List<BlendStackEntry<ObjectType>> m_blendNodes = new List<BlendStackEntry<ObjectType>>();
		private Dictionary<ObjectType, BlendStackEntry<ObjectType>> m_objToEntryMap = new Dictionary<ObjectType, BlendStackEntry<ObjectType>>();
	}
}