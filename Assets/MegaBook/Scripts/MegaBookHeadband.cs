using UnityEngine;

namespace MegaBook
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class MegaBookHeadband : MonoBehaviour
	{
		public float		length	= 1.0f;
		public float		radius	= 0.01f;
		public int			segs	= 8;
		public int			sides	= 8;
		public float		uvtwist	= 0.0f;
		public Vector2		uvoffset	= Vector3.zero;
		public Vector2		uvsize		= Vector2.one;
		Mesh				mesh;
		Vector3[]			verts;
		Vector3[]			sverts;
		Vector3[]			cross;

		void Start()
		{
		}

		void InitMesh()
		{
			MeshFilter mf = GetComponent<MeshFilter>();
			if ( mf )
			{
				mesh = new Mesh();
				BuildMesh();
				mf.sharedMesh = mesh;
			}
		}

		public void DeformMesh(float h, float r)
		{
			if ( mesh == null )
				InitMesh();

			if ( r != 0.0f )
			{
				float rr = (h * 0.5f) / -r;

				float off = 0.0f;

				float vy = Mathf.Clamp((h * 0.5f) / rr, -1.0f, 1.0f);
				float theta = Mathf.Asin(vy);
				float maxx = rr - (Mathf.Cos(theta) * rr);

				for ( int i = 0; i < verts.Length; i++ )
				{
					Vector3 v = verts[i];
					v.y *= -h;

					vy = Mathf.Clamp(v.y / rr, -1.0f, 1.0f);
					theta = Mathf.Asin(vy);
					float px = rr - (Mathf.Cos(theta) * rr);

					if ( i == 0 && rr < 0.0f )
						off = px;
					px -= maxx;	//off;

					v.x += px;
					sverts[i] = v;
				}
			}
			else
			{
				for ( int i = 0; i < verts.Length; i++ )
				{
					Vector3 v = verts[i];
					v.y *= -h;
					sverts[i] = v;
				}
			}
			mesh.vertices = sverts;
			mesh.RecalculateNormals();
		}

		public void DeformMesh1(float h, float r)
		{
			if ( mesh == null )
				InitMesh();

			if ( r != 0.0f )
			{
				float rr = (h * 0.5f) / -r;

				float off = 0.0f;

				for ( int i = 0; i < verts.Length; i++ )
				{
					Vector3 v = verts[i];
					v.y *= -h;

					float vy = Mathf.Clamp(v.y / rr, -1.0f, 1.0f);
					float theta = Mathf.Asin(vy);
					float px = rr - (Mathf.Cos(theta) * rr);

					if ( i == 0 && rr < 0.0f )
						off = px;
					px -= off;

					v.x += px;
					sverts[i] = v;
				}
			}
			else
			{
				for ( int i = 0; i < verts.Length; i++ )
				{
					Vector3 v = verts[i];
					v.y *= h;
					sverts[i] = v;
				}
			}
			mesh.vertices = sverts;
			mesh.RecalculateNormals();
		}

		public void BuildMesh()
		{
			if ( mesh == null )
				InitMesh();

			int segments = segs;
			if ( segments < 1 )
				segments = 1;

			int vcount = ((segments + 1) * (sides + 1));
			int tcount = ((sides * 2) * segments);
			tcount += (sides - 2) * 2;

			verts = new Vector3[vcount];
			sverts = new Vector3[vcount];

			Vector2[] uvs = new Vector2[vcount];
			int[] tris = new int[tcount * 3];

			int vi = 0;
			int ti = 0;

			BuildCrossSection(1.0f);

			Vector2 uv = Vector2.zero;

			float startAlpha = -0.5f;
			float endAlpha = 0.5f;

			int vo = vi;

			for ( int i = 0; i <= segments; i++ )
			{
				float alpha = Mathf.Lerp(startAlpha, endAlpha, ((float)i / (float)segments));
				float uvt = alpha * uvtwist;

				for ( int v = 0; v <= cross.Length; v++ )
				{
					Vector3 p = cross[v % cross.Length];
					verts[vi] = p * radius;
					verts[vi].y = alpha * length;

					uv.x = uvoffset.y + ((alpha + 0.5f) * uvsize.y);    // * length)lengthuvtile;
					uv.y = uvoffset.x + (((float)v / (float)cross.Length) * uvsize.x) + uvt;

					uvs[vi] = uv;

					vi++;
				}
			}

			int sc = sides + 1;
			for ( int i = 0; i < segments; i++ )
			{
				for ( int v = 0; v < cross.Length; v++ )
				{
					int v1 = ((i + 1) * sc) + v + vo;
					int v2 = ((i + 1) * sc) + ((v + 1) % sc) + vo;
					int v3 = (i * sc) + v + vo;
					int v4 = (i * sc) + ((v + 1) % sc) + vo;

					tris[ti++] = v3;
					tris[ti++] = v2;
					tris[ti++] = v1;

					tris[ti++] = v3;
					tris[ti++] = v4;
					tris[ti++] = v2;
				}
			}

			// cap mesh
			int ci = 2;
			for ( int i = 0; i < cross.Length - 2; i++ )
			{
				tris[ti++] = 0;
				tris[ti++] = ci++;
				tris[ti++] = ci - 2;
			}

			ci = verts.Length - cross.Length + 2;
			for ( int i = 0; i < cross.Length - 2; i++ )
			{
				tris[ti++] = verts.Length - cross.Length;
				tris[ti++] = ci - 1;
				tris[ti++] = ci++;
			}

			mesh.Clear(false);
			mesh.vertices = verts;
			mesh.uv = uvs;
			mesh.triangles = tris;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
		}

		void BuildCrossSection(float rad)
		{
			float rotation = 0.0f;

			if ( cross == null || cross.Length != sides )
				cross = new Vector3[sides];

			Vector3 p = Vector3.zero;
			for ( int i = 0; i < sides; i++ )
			{
				float ang = (((float)i / (float)sides) * Mathf.PI * 2.0f) + (rotation * Mathf.Deg2Rad);

				ang += 45.0f * Mathf.Deg2Rad;

				p.x = Mathf.Sin(ang);
				p.z = Mathf.Cos(ang);
				cross[i] = p;
			}
		}
	}
}