using UnityEngine;
using System.Collections;

public class AsteroidRotation : MonoBehaviour
{
	public Vector3 Rot;

	void Update()
	{
		transform.Rotate(Rot * Time.deltaTime);
	}
}
