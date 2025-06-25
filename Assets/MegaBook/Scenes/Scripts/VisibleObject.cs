using UnityEngine;

namespace MegaBook
{
	[ExecuteInEditMode]
	public class VisibleObject : MonoBehaviour
	{
		ParticleSystem parts;

		void Start()
		{
			parts = gameObject.GetComponentInChildren<ParticleSystem>();
		}

		public void BookVisibility(float val)
		{
		}

		public void BookAppeared()
		{
			//Debug.Log("appeared " + Time.frameCount);
			if ( parts )
			{
				if ( !parts.isPlaying )
					parts.Play();
			}
		}

		public void BookVanished()
		{
			//Debug.Log("vanished " + Time.frameCount);
			if ( parts )
			{
				if ( parts.isPlaying )
					parts.Stop();
			}
		}

		public void BookAppear(float alpha)
		{
			//Debug.Log("appear " + alpha);
		}

		public void BookVanish(float alpha)
		{
			//Debug.Log("vanish " + alpha);
		}
	}
}