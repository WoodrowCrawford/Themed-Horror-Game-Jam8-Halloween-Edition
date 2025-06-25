using UnityEngine;
using Unity.Mathematics;

namespace MegaBook
{
	[System.Serializable]
	public class CameraShake : MonoBehaviour
	{
		public Vector3			PositionScale	= Vector3.zero;
		public float			PositionFreq	= 1.0f;
		public float			PositionPhase	= 0.0f;
		public bool				PositionEnable	= true;
		public Vector3			RotationScale	= Vector3.zero;
		public float			RotationFreq	= 1.0f;
		public float			RotationPhase	= 0.0f;
		public bool				RotationEnable	= true;
		public AnimationCurve	amtCurve		= new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
		public float			amountphase		= 0.0f;
		public Vector3			posseed			= new Vector3(1.0f, 2.0f, 3.0f);
		public Vector3			rotseed			= new Vector3(2.0f, 4.0f, 6.0f);

		public void DoUpdate(ref Vector3 pos, ref Vector3 rot, float shakeamt)
		{
			float seed1 = 2.0f;
			float seed2 = 3.0f;
			amountphase += Time.deltaTime * 0.4f;

			float amt = (0.5f + noise.snoise(new float3(seed1, seed2, amountphase)));
			amt = amtCurve.Evaluate(amt);

			if ( PositionEnable )
			{
				PositionPhase += Time.deltaTime * PositionFreq * amt;
				Vector3 d = Vector3.zero;
				Vector3 sp = posseed;

				d.x = noise.snoise(new float3(sp.y, sp.z, PositionPhase));
				d.y = noise.snoise(new float3(sp.x, sp.z, PositionPhase));
				d.z = noise.snoise(new float3(sp.x, sp.y, PositionPhase));

				d *= shakeamt;
				pos += transform.localToWorldMatrix.MultiplyVector(Vector3.Scale(d, PositionScale));
			}

			if ( RotationEnable )
			{
				RotationPhase += Time.deltaTime * RotationFreq * amt;
				Vector3 d = Vector3.zero;
				Vector3 sp = rotseed;

				d.x = noise.snoise(new float3(sp.y, sp.z, RotationPhase));
				d.y = noise.snoise(new float3(sp.x, sp.z, RotationPhase));
				d.z = noise.snoise(new float3(sp.x, sp.y, RotationPhase));

				d *= shakeamt;
				rot += Vector3.Scale(d, RotationScale);
			}
		}
	}
}