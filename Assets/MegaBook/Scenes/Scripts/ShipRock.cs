using UnityEngine;

namespace MegaBook
{
	public class ShipRock : MonoBehaviour
	{
		Vector3 extrarot;

		[Range(0.0f, 1.0f)]
		public float amount			= 1.0f;
		public float wobble			= 1.0f;
		public float amp			= 1.0f;
		public float wave			= 0.0f;
		public MegaBookAxis axis	= MegaBookAxis.Y;

		public float wobble1		= 1.0f;
		public float amp1			= 1.0f;
		public float wave1			= 0.0f;
		public MegaBookAxis axis1	= MegaBookAxis.Y;

		void Start()
		{
			extrarot = transform.localEulerAngles;
		}

		void Update()
		{
			wave += Time.deltaTime * wobble;

			float ang = Mathf.Sin(wave) * amp;
			Vector3 rot = extrarot;
			rot[(int)axis] += ang * amount;

			wave1 += Time.deltaTime * wobble1;

			ang = Mathf.Sin(wave1) * amp1;
			rot[(int)axis1] += ang * amount;
			transform.localEulerAngles = rot;
		}
	}
}