using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour
{
	private Vector3 rot;

	void Update ()
	{
		rot += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime*5);
	}
}
