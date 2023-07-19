using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{




	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	
		
	private void Awake()
	{

	}

	private void FixedUpdate()
	{
	}
}
