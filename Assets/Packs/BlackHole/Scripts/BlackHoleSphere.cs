using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BlackHoleSphere : MonoBehaviour
{
	private Material mat;
	private Transform m_tansform;

	void Start()
	{
		mat = GetComponent<MeshRenderer>().sharedMaterial;
		m_tansform = transform;
	}

	void Update()
	{
		mat.SetVector("_Center", new Vector4(m_tansform.position.x, m_tansform.position.y, m_tansform.position.z, 1f));
	}
}
