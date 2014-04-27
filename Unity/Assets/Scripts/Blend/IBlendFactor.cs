using UnityEngine;
using System.Collections;

public interface IBlendFactor
{
	void Update( float _deltaTime );
	float BlendWeight
	{
		get;
	}
}

