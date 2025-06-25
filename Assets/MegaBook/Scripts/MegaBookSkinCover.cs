using UnityEngine;

namespace MegaBook
{
	[ExecuteInEditMode]
	public class MegaBookSkinCover : MonoBehaviour
	{
		public Transform		frontBone;
		public Transform		spineBone;
		public Transform		backBone;
		public MegaBookAxis		upAxis			= MegaBookAxis.X;
		//public bool				flip			= false;
		public float			frontFalloff	= 0.01f;
		public float			backFalloff		= 0.01f;
		public AnimationCurve	frontFallCurve	= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve	backFallCurve	= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public float			size			= 0.001f;
		public string			savePath		= "MegaBook/Cover Styles";

		Transform FindChild(Transform parent, string name)
		{
			if ( parent )
			{
				for ( int i = 0; i < parent.childCount; i++ )
				{
					Transform tm = parent.GetChild(i);
					if ( tm.name == name )
						return tm;
				}
			}

			return null;
		}

		private void Awake()
		{
			Bounds b = GetComponent<MeshFilter>().sharedMesh.bounds;

			frontBone = FindChild(transform, "Front");
			if ( !frontBone )
			{
				frontBone = new GameObject("Front").transform;
				frontBone.SetParent(transform);
				frontBone.localPosition = new Vector3(b.max.x, b.max.y, b.center.z);
			}

			backBone = FindChild(transform, "Back");
			if ( !backBone )
			{
				backBone = new GameObject("Back").transform;
				backBone.SetParent(transform);
				backBone.localPosition = new Vector3(b.max.x, b.min.y, b.center.z);
			}

			spineBone = FindChild(transform, "Spine");

			if ( !spineBone )
			{
				spineBone = new GameObject("Spine").transform;
				spineBone.SetParent(transform);
				spineBone.localPosition = new Vector3(b.max.x, b.center.y, b.center.z);
			}
		}
	}
}