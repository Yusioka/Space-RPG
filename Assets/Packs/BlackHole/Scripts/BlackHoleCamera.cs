using UnityEngine;

public class BlackHoleCamera : AutoBehaviour
{
	public Transform BlackHoleTransform;
	public bool EinsteinRadiusCompliance;
	public float Radius;

	private Camera blackHoleCamera;
	private Camera newCamera;
	private BlackHoleRenderer blackHoleRenderer;

	private float farClip;

	private void Awake()
	{
		newCamera = GetComponent<Camera>();
		farClip = newCamera.farClipPlane;

		var cam = new GameObject("BHC");

		Transform tr = cam.transform;
		tr.SetParent(transform);
		tr.localPosition = Vector3.zero;
		tr.localRotation = Quaternion.identity;
		tr.localScale = Vector3.one;

		blackHoleCamera = cam.AddComponent<Camera>();
		blackHoleCamera.CopyFrom(newCamera);
		blackHoleCamera.renderingPath = RenderingPath.Forward;
		blackHoleCamera.depth = -10;
		blackHoleCamera.clearFlags = CameraClearFlags.Skybox;

		newCamera.clearFlags = CameraClearFlags.Depth;
		newCamera.renderingPath = RenderingPath.Forward;

		blackHoleRenderer = cam.AddComponent<BlackHoleRenderer>();
		blackHoleRenderer.BH = BlackHoleTransform;
		blackHoleRenderer.EinsteinRadiusCompliance = EinsteinRadiusCompliance;
		blackHoleRenderer.ratio = Screen.height / Screen.width;
		blackHoleRenderer.radius = Radius;
	}

	private void Update()
	{
		blackHoleRenderer.radius = Radius;
		blackHoleRenderer.ratio = Screen.height / (float)Screen.width;
		blackHoleCamera.fieldOfView = newCamera.fieldOfView;

		float clip = Mathf.Abs(Vector3.Distance(BlackHoleTransform.position, transform.position) * Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(transform.forward, transform.position - BlackHoleTransform.position)));
		blackHoleCamera.nearClipPlane = clip;
		blackHoleCamera.farClipPlane = farClip;
		newCamera.farClipPlane = clip;
	}
}
