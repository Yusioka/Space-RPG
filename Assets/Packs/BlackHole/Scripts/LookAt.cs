using UnityEngine;

public class LookAt : AutoBehaviour
{
	public Transform Target;

	void Update()
	{
		transform.LookAt(Target.position);
	}
}
