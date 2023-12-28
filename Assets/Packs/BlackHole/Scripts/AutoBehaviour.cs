using UnityEngine;

public class AutoBehaviour : MonoBehaviour
{
	protected new Transform transform
	{
		get
		{
			return _transform ?? (_transform = GetComponent<Transform>());
		}
	}

	private Transform _transform;
}
