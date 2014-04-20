using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class VagabondController : MonoBehaviour {

	[SerializeField]
	private Animator _animator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool moveDesired = Input.GetKey (KeyCode.W);
		float moveImpulse = moveDesired ? 1.0f : 0.0f;
		//_animator.SetFloat ("speed", moveImpulse);
		if(moveDesired)
		{
			_animator.SetBool("run", true);
			_animator.SetBool("stop", false);
		}
		else
		{
			_animator.SetBool("run", false);
			_animator.SetBool("stop", true);
		}

		if(Input.GetKeyDown(KeyCode.Space))
			_animator.SetTrigger("slide");
		
		if(Input.GetKeyDown(KeyCode.Q))
			_animator.SetTrigger("lunge");
		
		if(Input.GetKeyDown(KeyCode.E))
			_animator.SetTrigger("slice");

		bool isLunging = _animator.GetCurrentAnimatorStateInfo (0).IsName ("dive-lung-attack");
		UpperBodyWeight = isLunging ? 0.0f : 1.0f;

		_animator.SetLayerWeight(1, m_weight);
	}

	public float m_weight;
	public float m_targetWeight;

	public float UpperBodyWeight
	{
		set
		{
			if(m_targetWeight != value)
			{
				m_targetWeight = value;
				HOTween.Pause(this);
				HOTween.To(this, 1.0f, new TweenParms().Prop("m_weight", value, false).Ease(EaseType.EaseOutCubic));
			}
		}
	}
}
