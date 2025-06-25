using UnityEngine;
using System.Collections;

namespace MegaBook
{
	[ExecuteInEditMode]
	public class FireCannon : MonoBehaviour
	{
		public ParticleSystem	CannonMuzzleFlash;
		public Light			MuzzleFlashLight;
		public ParticleSystem	SparkParticles;
		public ParticleSystem	SmokeParticles;
		public ParticleSystem	SmokeFireParticles;
		public AudioSource		CannonFireAudio;
		public AudioSource		BurningFuseAudio;
		float					fadeStart			= 3.0f;
		float					fadeEnd				= 0.0f;
		public float			fadeTime			= 1.0f;
		float					t					= 0.0f;

		void Start()
		{
			MuzzleFlashLight.intensity = 0.0f;
		}  
  
		public void LightFuse()
		{
			ResetTheCannon();
		}

		public void FireTheCannon()
		{
			CannonMuzzleFlash.Play();
			SparkParticles.Play();
			SmokeParticles.Play();
			SmokeFireParticles.Play();
			CannonFireAudio.Play();
			StartCoroutine("FadeLight");
		}

		public void ResetTheCannon()
		{
			SparkParticles.Clear();
			MuzzleFlashLight.intensity = 0.0f;
		}

		IEnumerator FadeLight()
		{
			while ( t < fadeTime )
			{
    			t += Time.deltaTime;
				MuzzleFlashLight.intensity = Mathf.Lerp(fadeStart, fadeEnd, t / fadeTime);
				yield return 0;  
			}              
            
			t = 0.0f;  
		}
	}
}