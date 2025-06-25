using UnityEngine;

namespace MegaBook
{
	public class LightFlicker : MonoBehaviour
	{
		public float	scale		= 2.0f;
		public Light	lightObj;
		public float	minIntens	= 0.5f;
		public float	maxIntens	= 1.0f;
		public float	speed		= 1.0f;
		float			t;
		float			intens;

		private void Start()
		{
			if ( !lightObj )
				lightObj = GetComponent<Light>();

			intens = lightObj.intensity;
		}

		void Update()
		{
			if ( lightObj )
			{
				t += Time.deltaTime * speed;
				float a = Mathf.PerlinNoise(t, scale);
				lightObj.intensity = intens * Mathf.Lerp(minIntens, maxIntens, a);
			}
		}
	}
}