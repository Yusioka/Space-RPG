using UnityEngine;

[ExecuteInEditMode]
public class BlackHoleRenderer : AutoBehaviour
{
	protected Material material
	{
		get
		{
			if (_material == null)
			{
				_material = new Material(shader);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}
			return _material;
		}
	}

	[HideInInspector] public float ratio = 0.7f; // The height to the length ratio of the screen to display shader properly
	[HideInInspector] public float radius = 0.5f; // The radius of the black hole measured in the same units as the other objects in the scene
	[HideInInspector] public bool EinsteinRadiusCompliance;
	[HideInInspector] public Transform BH; // The object whose position is taken as the position of the black hole

	private Shader shader;
	private int outOfScreen;
	private Material _material; // Material in which the shader is located

	protected virtual void OnDisable()
	{
		if (_material)
		{
			DestroyImmediate(_material);
		}
	}

	private void Start()
	{
		shader = Resources.Load<Shader>("BlackHoleDistortion");
		material.SetInt("_EinsteinR", EinsteinRadiusCompliance ? 1 : 0);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (shader && material && BH != null)
		{
			Camera currentCamera = Camera.current;

			// Find the position of the black hole in screen coordinates
			Vector3 screenPos = currentCamera.WorldToScreenPoint(BH.position);
			Vector2 pos = new Vector2(screenPos.x / currentCamera.pixelWidth, screenPos.y / currentCamera.pixelHeight);
			Vector3 lookDir = BH.position - currentCamera.transform.position;

			float lerp = Mathf.Clamp01(Remap(Vector3.Angle(lookDir, currentCamera.transform.forward),
				currentCamera.fieldOfView - (currentCamera.fieldOfView / 3),
				0f,
				currentCamera.fieldOfView + (currentCamera.fieldOfView / 3),
				1f
				));

			// Install all the required parameters for the shader
			material.SetVector("_Position", new Vector2(pos.x, 1 - pos.y));
			material.SetFloat("_Ratio", ratio);
			material.SetFloat("_Rad", Mathf.Lerp(radius, 0, lerp));
			material.SetFloat("_Distance", Vector3.Distance(BH.position, transform.position));

			// Apply to the resulting image.
			Graphics.Blit(source, destination, material);
		}
	}

	private float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (from2 - from1) * (to2 - to1) + to1;
	}
}
