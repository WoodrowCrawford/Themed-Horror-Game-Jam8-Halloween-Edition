using UnityEngine;

namespace MegaBook
{
	[ExecuteInEditMode]
	public class MegaBookCover : MonoBehaviour
	{
		public Transform		spine;
		public Transform		front;
		public Transform		back;
		public Vector3			frontpivot;
		public Vector3			frontoffset;
		public Vector3			backpivot;
		public Vector3			spinepivot;
		public MegaBookAxis		frontAxis		= MegaBookAxis.Z;
		public MegaBookAxis		backAxis		= MegaBookAxis.Z;
		public MegaBookAxis		spineAxis		= MegaBookAxis.Z;
		public MegaBookBuilder	book;
		public float			thickness		= 0.0f;
		public float			spineThickness	= 0.0f;
		public MegaBookAxis		coverThickAxis	= MegaBookAxis.Y;
		public MegaBookAxis		spineThickAxis	= MegaBookAxis.Y;
		public float			coverWidth		= 1.0f;
		Mesh					mesh;

		private void Start()
		{
		}

		public void UpdateBones(MegaBookBuilder book)
		{
			if ( mesh == null )
				mesh = GetComponent<MeshFilter>().sharedMesh;

			if ( front )
			{
				float ca = 0.0f;
				if ( book.Flip < 0.0f )
					ca = -book.Flip;

				float ang = book.FrontCoverAngle();
				Vector3 frot = Vector3.zero;
				frot[(int)frontAxis] = ang;

				Vector3 off = frontpivot + spinepivot;
				off[(int)coverThickAxis] = (book.bookthickness + (thickness * 0.5f)) * 0.5f;
				off.x += book.coverOffset.x;
				if ( book.autoFit )
					off[(int)coverThickAxis] *= book.autoFitSize.y;

				front.position = book.transform.TransformPoint(off);
				front.localEulerAngles = frot;

				Vector3 lscl = book.coverScale;

				if ( book.autoFit )
				{
					lscl.x *= (book.pageWidth / coverWidth) * book.autoFitSize.x;
					lscl.z *= (book.pageLength / mesh.bounds.size.z) * book.autoFitSize.z;
				}

				front.localScale = lscl;
			}

			if ( back )
			{
				float ca = 0.0f;
				if ( book.Flip > book.NumPages )
					ca = book.Flip - book.NumPages;

				float ang = book.BackCoverAngle();

				if ( !book.changespineangle )
					ang = 0.0f;

				Vector3 off = backpivot + spinepivot;
				off[(int)coverThickAxis] = -((book.bookthickness + (thickness * 0.5f)) * 0.5f);
				off.x += book.coverOffset.x;
				if ( book.autoFit )
					off[(int)coverThickAxis] *= book.autoFitSize.y;

				back.position = book.transform.TransformPoint(off);
				Vector3 brot = Vector3.zero;
				brot[(int)backAxis] = ang;
				back.localEulerAngles = brot;

				Vector3 lscl = book.coverScale;

				if ( book.autoFit )
				{
					lscl.x *= (book.pageWidth / coverWidth) * book.autoFitSize.x;
					lscl.z *= (book.pageLength / mesh.bounds.size.z) * book.autoFitSize.z;
				}

				back.localScale = lscl;
			}

			if ( spine )
			{
				Vector3 srot = Vector3.zero;
				Vector3 off = spinepivot;
				off.x += book.coverOffset.x;

				spine.position = book.transform.TransformPoint(off);	//spinepivot);
				Vector3 rot = book.transform.localEulerAngles;

				srot[(int)spineAxis] = -rot.z;
				spine.localEulerAngles = srot;
				Vector3 lscl = book.coverScale;	//Vector3.one;	//spine.localScale;	//Vector3.one;

				float h = frontpivot.y - backpivot.y;

				if ( h.Equals(0.0f) )
					h = 1.0f;

				lscl[(int)spineThickAxis] = (book.bookthickness + spineThickness) / h;
				lscl.x *= book.spineScale;

				if ( book.autoFit )
				{
					lscl.z *= (book.pageLength / mesh.bounds.size.z) * book.autoFitSize.z;
					lscl.y *= book.autoFitSize.y;
				}

				spine.localScale = lscl;
			}
		}
	}
}