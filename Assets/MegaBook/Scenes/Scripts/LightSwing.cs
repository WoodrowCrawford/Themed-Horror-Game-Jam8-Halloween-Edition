using UnityEngine;

namespace MegaBook
{
	public class LightSwing : MonoBehaviour
	{
		public float		speed		= 1.0f;
		public MegaBookAxis	axis;
		public float		maxAngle	= 30.0f;
		public MegaBookAxis axis1;
		public float		maxAngle1	= 30.0f;
		public float		speed1		= 1.0f;
		Vector3				locrot;
		Vector3				ang;
		float				t;
		float				t1;

		void Start()
		{
			locrot = transform.localEulerAngles;
		}

		void Update()
		{
			t += Time.deltaTime * speed;
			float a = Mathf.Sin(t * Mathf.PI * 2.0f) * maxAngle;

			ang[(int)axis] = a;

			t1 += Time.deltaTime * speed1;
			a = Mathf.Sin(t1 * Mathf.PI * 2.0f) * maxAngle1;

			ang[(int)axis1] = a;

			transform.localEulerAngles = locrot + ang;
		}
	}
}