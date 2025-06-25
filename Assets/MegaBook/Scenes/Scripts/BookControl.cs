using UnityEngine;

namespace MegaBook
{
	[ExecuteInEditMode]
	public class BookControl : MonoBehaviour
	{
		public MegaBookBuilder	book;
		public Vector3			offset;
		public Vector3			rot;
		public Transform		parent;

		void Update()
		{
			if ( book )
			{
				transform.position = parent.position + parent.TransformVector(offset);
				transform.rotation = parent.rotation * Quaternion.Euler(rot);
			}
		}

		public void SetPage(float a)
		{
			if ( book )
				book.SetPage(a);
		}
	}
}