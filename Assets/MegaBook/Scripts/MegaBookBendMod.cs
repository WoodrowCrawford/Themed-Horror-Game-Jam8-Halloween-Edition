using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaBook
{
	[System.Serializable]
	public enum MegaBookAxis
	{
		X,
		Y,
		Z
	}

	[System.Serializable]
	public class MegaBookBendMod
	{
		public float		angle			= 0.0f;
		public Vector3		gizmo_pos		= Vector3.zero;
		public Vector3		gizmo_rotation	= Vector3.zero;
		public Vector3		center			= Vector3.zero;
		public float		from			= 0.0f;
		public float		to				= 0.0f;
		public bool			fromto			= false;
		public MegaBookAxis	axis			= MegaBookAxis.X;
		public Vector3		gizmo_scale		= Vector3.one;
		public float		dir				= 0.0f;
		Matrix4x4			tm				= Matrix4x4.identity;
		Matrix4x4			invtm			= Matrix4x4.identity;
		Matrix4x4			mat				= new Matrix4x4();
		Matrix4x4			tmAbove			= new Matrix4x4();
		Matrix4x4			tmBelow			= new Matrix4x4();
		float				r				= 0.0f;
		float				oor				= 0.0f;
		BendJob				bendJob;
		JobHandle			jobHandle;
		BendJobFirst		bendFirstJob;
		JobHandle			jobFirstHandle;
		BendJobDbl			bendJobDbl;
		JobHandle			jobHandleDbl;

		[BurstCompile]
		struct BendJobFirst : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<Vector3>	jvertices;
			public NativeArray<Vector3>	jsverts;
			public float				angle;
			public float				dir;
			public bool					doRegion;
			public float				from;
			public float				to;
			public float				r;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			tmBelow;
			public Matrix4x4			tmAbove;
			public float				oor;

			public void Execute(int i)
			{
				if ( r == 0.0f )
				{
					if ( !doRegion )
						jsverts[i] = jvertices[i];
					else
					{
						Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

						if ( doRegion )
						{
							if ( p.y <= from )
							{
								jsverts[i] = invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
								return;
							}
							else
							{
								if ( p.y >= to )
								{
									jsverts[i] = invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
									return;
								}
							}
						}

						jsverts[i] = invtm.MultiplyPoint3x4(p);
					}
				}
				else
				{
					if ( doRegion )
					{
						Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

						if ( p.y <= from )
						{
							jsverts[i] = invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
							return;
						}
						else
						{
							if ( p.y >= to )
							{
								jsverts[i] = invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
								return;
							}
							else
							{
								float x = p.x;
								float yr = math.PI - (p.y * oor);

								float c = math.cos(yr);
								float s = math.sin(yr);
								p.x = r * c + r - x * c;
								p.y = r * s - x * s;
								jsverts[i] = invtm.MultiplyPoint3x4(p);
							}
						}
					}
					else
					{
						Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

						float x = p.x;
						float yr = math.PI - (p.y * oor);

						float c = math.cos(yr);
						float s = math.sin(yr);
						p.x = r * c + r - x * c;
						p.y = r * s - x * s;
						jsverts[i] = invtm.MultiplyPoint3x4(p);
					}
				}
			}
		}

		[BurstCompile]
		struct BendJob : IJobParallelFor
		{
			public NativeArray<Vector3>	jsverts;
			public float				angle;
			public float				dir;
			public bool					doRegion;
			public float				from;
			public float				to;
			public float				r;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			tmBelow;
			public Matrix4x4			tmAbove;
			public float				oor;

			public void Execute(int i)
			{
				if ( r == 0.0f )
				{
					if ( !doRegion )
					{
					}
					else
					{
						Vector3 p = tm.MultiplyPoint3x4(jsverts[i]);

						if ( doRegion )
						{
							if ( p.y <= from )
							{
								jsverts[i] = invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
								return;
							}
							else
							{
								if ( p.y >= to )
								{
									jsverts[i] = invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
									return;
								}
							}
						}

						jsverts[i] = invtm.MultiplyPoint3x4(p);
					}
				}
				else
				{
					if ( doRegion )
					{
						Vector3 p = tm.MultiplyPoint3x4(jsverts[i]);

						if ( p.y <= from )
						{
							jsverts[i] = invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
							return;
						}
						else
						{
							if ( p.y >= to )
							{
								jsverts[i] = invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
								return;
							}
							else
							{
								float x = p.x;
								float yr = math.PI - (p.y * oor);

								float c = math.cos(yr);
								float s = math.sin(yr);
								p.x = r * c + r - x * c;
								p.y = r * s - x * s;
								p.x = (r * c + r - x * c);
								p.y = (r * s - x * s);

								jsverts[i] = invtm.MultiplyPoint3x4(p);
							}
						}
					}
					else
					{
						Vector3 p = tm.MultiplyPoint3x4(jsverts[i]);

						float x = p.x;
						float yr = math.PI - (p.y * oor);

						float c = math.cos(yr);
						float s = math.sin(yr);
						p.x = r * c + r - x * c;
						p.y = r * s - x * s;
						jsverts[i] = invtm.MultiplyPoint3x4(p);
					}
				}
			}
		}

		[BurstCompile]
		struct BendJobDbl : IJobParallelFor
		{
			public NativeArray<Vector3> jsverts;
			public float		angle;
			public float		dir;
			public bool			doRegion;
			public float		from;
			public float		to;
			public double		r;
			public Matrix4x4	tm;
			public Matrix4x4	invtm;
			public Matrix4x4	tmBelow;
			public Matrix4x4	tmAbove;
			public double		oor;

			public void Execute(int i)
			{
				if ( r == 0.0f )
				{
					if ( !doRegion )
					{
					}
					else
					{
						Vector3 p = tm.MultiplyPoint3x4(jsverts[i]);

						if ( doRegion )
						{
							if ( p.y <= from )
							{
								jsverts[i] = invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
								return;
							}
							else
							{
								if ( p.y >= to )
								{
									jsverts[i] = invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
									return;
								}
							}
						}

						jsverts[i] = invtm.MultiplyPoint3x4(p);
					}
				}
				else
				{
					if ( doRegion )
					{
						Vector3 p = tm.MultiplyPoint3x4(jsverts[i]);

						if ( p.y <= from )
						{
							jsverts[i] = invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
							return;
						}
						else
						{
							if ( p.y >= to )
							{
								jsverts[i] = invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
								return;
							}
							else
							{
								double x = p.x;
								double yr = math.PI_DBL - ((double)p.y * oor);

								double c = math.cos(yr);
								double s = math.sin(yr);

								p.x = (float)(r * c + r - x * c);
								p.y = (float)(r * s - x * s);
								jsverts[i] = invtm.MultiplyPoint3x4(p);
							}
						}
					}
					else
					{
						Vector3 p = tm.MultiplyPoint3x4(jsverts[i]);

						double x = p.x;
						double yr = math.PI_DBL - ((double)p.y * oor);

						double c = math.cos(yr);
						double s = math.sin(yr);
						p.x = (float)(r * c + r - x * c);
						p.y = (float)(r * s - x * s);
						jsverts[i] = invtm.MultiplyPoint3x4(p);
					}
				}
			}
		}

		public void SetAxis(Matrix4x4 tmAxis)
		{
			Matrix4x4 itm = tmAxis.inverse;
			tm = tmAxis * tm;
			invtm = invtm * itm;
		}

		public void SetTM()
		{
			tm = Matrix4x4.identity;
			Quaternion rot = Quaternion.Euler(-gizmo_rotation);

			tm.SetTRS(gizmo_pos + center, rot, gizmo_scale);
			invtm = tm.inverse;
		}

		void CalcR(MegaBookPage page, MegaBookAxis axis, float ang)
		{
			float len = 0.0f;

			if ( !fromto )
			{
				switch ( axis )
				{
					case MegaBookAxis.X: len = page.bbox.max.x - page.bbox.min.x; break;
					case MegaBookAxis.Z: len = page.bbox.max.y - page.bbox.min.y; break;
					case MegaBookAxis.Y: len = page.bbox.max.z - page.bbox.min.z; break;
				}
			}
			else
				len = to - from;

			if ( Mathf.Abs(ang) < 0.0001f )
				r = 0.0f;
			else
				r = len / ang;

			if ( r != 0.0f )
				oor = 1.0f / r;
		}

		public Vector3 Map(int i, Vector3 p)
		{
			if ( r == 0.0f && !fromto )
				return p;

			p = tm.MultiplyPoint3x4(p);	// tm may have an offset gizmo etc

			if ( fromto )
			{
				if ( p.y <= from )
					return invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
				else
				{
					if ( p.y >= to )
						return invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
				}
			}

			if ( r == 0.0f )
				return invtm.MultiplyPoint3x4(p);

			float x = p.x;
			float y = p.y;

			float yr = Mathf.PI - (y * oor);	/// r;

			float c = Mathf.Cos(yr);
			float s = Mathf.Sin(yr);
			float px = r * c + r - x * c;
			p.x = px;
			float pz = r * s - x * s;
			p.y = pz;
			p = invtm.MultiplyPoint3x4(p);
			return p;
		}

		public void DeformFirst(MegaBookPage page, NativeArray<Vector3> jverts, NativeArray<Vector3> jsverts)
		{
			Calc(page);

			if ( jverts != null )
			{
				bendFirstJob.oor		= this.oor;
				bendFirstJob.tmAbove	= this.tmAbove;
				bendFirstJob.tmBelow	= this.tmBelow;
				bendFirstJob.tm			= this.tm;
				bendFirstJob.invtm		= this.invtm;
				bendFirstJob.r			= this.r;
				bendFirstJob.angle		= this.angle;
				bendFirstJob.dir		= this.dir;
				bendFirstJob.doRegion	= this.fromto;
				bendFirstJob.from		= this.from;
				bendFirstJob.to			= this.to;
				bendFirstJob.jvertices	= jverts;
				bendFirstJob.jsverts	= jsverts;
				jobFirstHandle			= bendFirstJob.Schedule(jverts.Length, 64);
				jobFirstHandle.Complete();
			}
		}

		public void Deform(MegaBookPage page, NativeArray<Vector3> jsverts)
		{
			Calc(page);

			if ( jsverts != null )
			{
				if ( Mathf.Abs(r) > 10.0f )
				{
					bendJobDbl.oor		= this.oor;
					bendJobDbl.tmAbove	= this.tmAbove;
					bendJobDbl.tmBelow	= this.tmBelow;
					bendJobDbl.tm		= this.tm;
					bendJobDbl.invtm	= this.invtm;
					bendJobDbl.r		= this.r;
					bendJobDbl.angle	= this.angle;
					bendJobDbl.dir		= this.dir;
					bendJobDbl.doRegion	= this.fromto;
					bendJobDbl.from		= this.from;
					bendJobDbl.to		= this.to;
					bendJobDbl.jsverts	= jsverts;
					jobHandleDbl		= bendJobDbl.Schedule(jsverts.Length, 64);
					jobHandleDbl.Complete();
				}
				else
				{
					bendJob.oor = this.oor;
					bendJob.tmAbove = this.tmAbove;
					bendJob.tmBelow = this.tmBelow;
					bendJob.tm = this.tm;
					bendJob.invtm = this.invtm;
					bendJob.r = this.r;
					bendJob.angle = this.angle;
					bendJob.dir = this.dir;
					bendJob.doRegion = this.fromto;
					bendJob.from = this.from;
					bendJob.to = this.to;
					bendJob.jsverts = jsverts;
					jobHandle = bendJob.Schedule(jsverts.Length, 64);
					jobHandle.Complete();
				}
			}
		}

		void Calc(MegaBookPage page)
		{
			SetTM();
			if ( from > to ) from = to;
			if ( to < from ) to = from;

			mat = Matrix4x4.identity;

			switch ( axis )
			{
				case MegaBookAxis.X: MegaBookMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
				case MegaBookAxis.Y: MegaBookMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaBookAxis.Z: break;
			}

			MegaBookMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir);
			SetAxis(mat);

			CalcR(page, axis, Mathf.Deg2Rad * angle);

			if ( fromto )
			{
				fromto = false;
				float len  = to - from;
				float rat1, rat2;

				if ( len == 0.0f )
					rat1 = rat2 = 1.0f;
				else
				{
					rat1 = to / len;
					rat2 = from / len;
				}

				Vector3 pt;
				tmAbove = Matrix4x4.identity;
				MegaBookMatrix.Translate(ref tmAbove, 0.0f, -to, 0.0f);
				MegaBookMatrix.RotateZ(ref tmAbove, -Mathf.Deg2Rad * angle * rat1);
				MegaBookMatrix.Translate(ref tmAbove, 0.0f, to, 0.0f);
				pt = new Vector3(0.0f, to, 0.0f);
				MegaBookMatrix.Translate(ref tmAbove, tm.MultiplyPoint3x4(Map(0, invtm.MultiplyPoint3x4(pt))) - pt);

				tmBelow = Matrix4x4.identity;
				MegaBookMatrix.Translate(ref tmBelow, 0.0f, -from, 0.0f);
				MegaBookMatrix.RotateZ(ref tmBelow, Mathf.Deg2Rad * angle * rat2);
				MegaBookMatrix.Translate(ref tmBelow, 0.0f, from, 0.0f);
				pt = new Vector3(0.0f, from, 0.0f);
				MegaBookMatrix.Translate(ref tmBelow, tm.MultiplyPoint3x4(Map(0, invtm.MultiplyPoint3x4(pt))) - pt);

				fromto = true;
			}
		}
	}
}