using UnityEngine;

namespace MegaBook
{
	public class ShipControl : MonoBehaviour
	{
		public float			amount			= 0.0f;
		public float			damp			= 0.25f;
		public AnimationCurve	crv				= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public bool				doFireCannons	= true;
		public float			firetime		= 1.0f;
		ShipRock[]				ships;
		FireCannon[]			cannons;
		Light[]					lights;
		AudioSource[]			audiosrcs;
		float[]					ct;
		float[]					lightintens;
		float[]					volumes;
		float					vel;

		void Start()
		{
			ships = GetComponentsInChildren<ShipRock>();
			cannons = GetComponentsInChildren<FireCannon>();
			lights = GetComponentsInChildren<Light>();
			audiosrcs = GetComponentsInChildren<AudioSource>();

			if ( audiosrcs.Length > 0 )
			{
				volumes = new float[audiosrcs.Length];

				for ( int i = 0; i < volumes.Length; i++ )
					volumes[i] = audiosrcs[i].volume;
			}

			if ( cannons.Length > 0 )
			{
				ct = new float[cannons.Length];

				for ( int i = 0; i < ct.Length; i++ )
					ct[i] = Random.Range(0.0f, firetime);
			}

			if ( lights.Length > 0 )
			{
				lightintens = new float[lights.Length];

				for ( int i = 0; i < lights.Length; i++ )
					lightintens[i] = lights[i].intensity;
			}
		}

		void Update()
		{
			for ( int i = 0; i < ships.Length; i++ )
				ships[i].amount = amount;

			for ( int i = 0; i < lights.Length; i++ )
				lights[i].intensity = lightintens[i] * amount;

			for ( int i = 0; i < audiosrcs.Length; i++ )
				audiosrcs[i].volume = volumes[i] * amount;

			if ( cannons.Length > 0 && doFireCannons )
			{
				if ( amount > 0.75f )
				{
					for ( int i = 0; i < ct.Length; i++ )
					{
						ct[i] -= Time.deltaTime;
						if ( ct[i] < 0.0f )
						{
							cannons[i].FireTheCannon();

							ct[i] = Random.Range(firetime - (firetime * 0.5f), firetime * 2.0f);
						}
					}
				}
			}
		}

		public void BookVisibility(float val)
		{
			amount = crv.Evaluate(val);
		}

		public void BookAppeared()
		{
		}

		public void BookVanished()
		{
		}

		public void BookAppear(float alpha)
		{
		}

		public void BookVanish(float alpha)
		{
		}
	}
}