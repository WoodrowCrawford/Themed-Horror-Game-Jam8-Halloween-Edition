using UnityEngine;

namespace MegaBook
{
	public class MegaBookMatrix
	{
		static public void Translate(ref Matrix4x4 mat, Vector3 p)
		{
			Translate(ref mat, p.x, p.y, p.z);
		}

		static public void Translate(ref Matrix4x4 mat, float x, float y, float z)
		{
			Matrix4x4 tm = Matrix4x4.identity;

			tm[0, 3] = x;
			tm[1, 3] = y;
			tm[2, 3] = z;

			mat = tm * mat;
		}

		static public void RotateX(ref Matrix4x4 mat, float ang)
		{
			Matrix4x4 tm = Matrix4x4.identity;

			float c = Mathf.Cos(ang);
			float s = Mathf.Sin(ang);

			tm[1, 1] = c;
			tm[1, 2] = s;
			tm[2, 1] = -s;
			tm[2, 2] = c;

			mat = tm * mat;
		}

		static public void RotateY(ref Matrix4x4 mat, float ang)
		{
			Matrix4x4 tm = Matrix4x4.identity;

			float c = Mathf.Cos(ang);
			float s = Mathf.Sin(ang);

			tm[0, 0] = c;
			tm[0, 2] = -s;
			tm[2, 0] = s;
			tm[2, 2] = c;

			mat = tm * mat;
		}

		static public void RotateZ(ref Matrix4x4 mat, float ang)
		{
			Matrix4x4 tm = Matrix4x4.identity;

			float c = Mathf.Cos(ang);
			float s = Mathf.Sin(ang);

			tm[0, 0] = c;
			tm[0, 1] = s;
			tm[1, 0] = -s;
			tm[1, 1] = c;

			mat = tm * mat;
		}
	}
}