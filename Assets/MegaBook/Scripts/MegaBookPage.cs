using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Video;

namespace MegaBook
{
	[System.Flags]
	public enum LayerID
	{
		None = 0,
		Layer1	= 1,
		Layer2	= 2,
		Layer3	= 4,
		Layer4	= 8,
		Layer5	= 16,
		Layer6	= 32,
		Layer7	= 64,
		Layer8	= 128,
		All		= 255,
	}

	[System.Serializable]
	public class MegaBookPageObject
	{
		public Vector3			pos;
		public Vector3			rot;
		public float			offset;
		public GameObject		obj;
		public Vector3			BaryCoord		= Vector3.zero;
		public int[]			BaryVerts		= new int[3];
		public bool				attached		= false;
		public Vector3			BaryCoord1		= Vector3.zero;
		public int[]			BaryVerts1		= new int[3];
		public Vector3			attachforward	= new Vector3(0.005f, 0.0f, 0.0f);
		public bool				overridevisi	= false;
		public float			visilow			= -1.0f;
		public float			visihigh		= 1.0f;
		public bool				message			= false;
		public List<GameObject>	messageObjs		= new List<GameObject>();
		public Vector3			appearScale		= Vector3.one;
		public Vector3			vanishScale		= Vector3.one;
		public Vector3			scale			= Vector3.one;
		public float			visiScaleLow	= -1.0f;
		public float			visiScaleHigh	= 1.0f;
		public bool				scaleObj		= false;
		public Vector3			appearRot		= Vector3.zero;
		public Vector3			vanishRot		= Vector3.zero;
		public Vector3			appearPos		= Vector3.zero;
		public Vector3			vanishPos		= Vector3.zero;
		public AnimationCurve	appearCrv		= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve	vanishCrv		= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve	offsetCrv		= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public bool				backOfPage		= false;
		public float			appearAlpha;
		public float			vanishAlpha;
		public float			alpha;
		// Used with canvas group to fade UI in and out, will add deforming UI support, also add to MF2
		public float			appearFade		= 1.0f;
		public float			fade			= 1.0f;
		public float			vanishFade		= 1.0f;
		public AnimationCurve	fadeAppearCrv	= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public AnimationCurve	fadeVanishCrv	= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public CanvasGroup		canvasGroup;
		public bool				active			= true;
		public LayerID			layerId			= LayerID.Layer1;
	}

	public enum VideoReduceSize
	{
		Original	= 1,
		Half		= 2,
		Quarter		= 4,
		Eighth		= 8,
	}

	public enum ColliderOverride
	{
		UseBook,
		NoCollider,
		Collider,
	}

	public enum TMProSubdiv
	{
		UseBook,
		None,
		Auto,
		Twice,
		Triple,
		Quad,
	}

	[System.Serializable]
	public class MegaBookPageParams
	{
		public Texture2D				front;
		public Texture2D				back;
		public Texture2D				frontMask;
		public Texture2D				backMask;
		public Rect						copyarea;
		public Texture2D				background;
		public Rect						copyarea1;
		public Texture2D				background1;
		public Texture2D				madefront;
		public Texture2D				madeback;
		public Material					frontmat;
		public Material					backmat;
		public Material					edgemat;
		public Mesh						pagemesh;
		public Mesh						holemesh;
		public GameObject				pageobj;
		public GameObject				holeobj;
		public int						frontmatindex		= -1;
		public int						backmatindex		= -1;
		public bool						usebackground		= true;
		public bool						usebackground1		= true;
		public Vector3					rotate				= Vector3.zero;
		public Vector3					pivot				= Vector3.zero;
		public bool						swapsides			= false;
		public bool						stiff				= false;
		public Mesh						extramesh;
		public List<MegaBookPageObject>	objects				= new List<MegaBookPageObject>();
		public float					visobjlow			= -1.0f;
		public float					visobjhigh			= 1.0f;
		public bool						alphatexturefront	= false;
		public bool						alphatextureback	= false;
		public int						objectIndex;
		public bool						useFrontColor		= false;
		public Color					frontColor			= Color.white;
		public bool						useBackColor		= false;
		public Color					backColor			= Color.white;
		public float					scale				= 1.0f;
		public VideoClip				videoFrontClip;
		public VideoClip				videoBackClip;
		public float					videoFrontVol		= 0.0f;
		public float					videoBackVol		= 0.0f;
		public bool						videoFrontPlayVis	= true;
		public bool						videoBackPlayVis	= true;
		public VideoReduceSize			videoFrontReduceSize	= VideoReduceSize.Original;
		public VideoReduceSize			videoBackReduceSize		= VideoReduceSize.Original;
		public int						videoFrontMatID		= -1;
		public int						videoBackMatID		= -1;
		public bool						meshFront			= false;
		public GameObject				meshFrontRoot;
		public Vector3					meshFrontOffset;
		public Vector3					meshFrontScale		= Vector3.one;
		public Vector3					meshFrontRot;
		public bool						meshBack			= false;
		public GameObject				meshBackRoot;
		public Vector3					meshBackOffset;
		public Vector3					meshBackScale		= Vector3.one;
		public Vector3					meshBackRot;
		public int						widthSegs			= -1;
		public int						lengthSegs			= -1;
		public bool						texturesDirty		= false;
		public ColliderOverride			colliderOverride	= ColliderOverride.UseBook;
		public TMProSubdiv				frontTMProSubdiv	= TMProSubdiv.UseBook;
		public TMProSubdiv				backTMProSubdiv		= TMProSubdiv.UseBook;
		public LayerID					frontLayerID		= LayerID.Layer1;
		public LayerID					backLayerID			= LayerID.Layer1;
	}

	[System.Serializable]
	public class MegaBookPage
	{
		public MegaBookBendMod			flexer;
		public MegaBookBendMod			turner;
		public MegaBookBendMod			lander;
		public Vector3					pivot;
		public float					width;
		public float					length;
		public Vector3[]				verts;
		public Vector3[]				sverts;
		public GameObject				obj;
		public Mesh						mesh;
		public Bounds					bbox;
		public AnimationCurve			landerangcon	= new AnimationCurve();
		public AnimationCurve			flexangcon		= new AnimationCurve();
		public AnimationCurve			turnerangcon	= new AnimationCurve();
		public AnimationCurve			turnerfromcon	= new AnimationCurve();
		public AnimationCurve			turnerfromcon1	= new AnimationCurve();
		public bool						deform			= false;
		public float					turnangle		= 0.0f;
		public bool						stiff			= false;
		public Mesh						holemesh;
		public Vector3[]				verts1;
		public Vector3[]				sverts1;
		public MeshFilter				mf;
		public bool						showinghole;
		public bool						showobjects;
		public List<MegaBookPageObject>	objects			= new List<MegaBookPageObject>();
		public float					visobjlow		= -1.0f;
		public float					visobjhigh		= 1.0f;
		public MeshCollider				collider;
		public int						pnum;
		public NativeArray<Vector3>		jverts;
		public NativeArray<Vector3>		jsverts;
		public NativeArray<Vector3>		jverts1;
		public NativeArray<Vector3>		jsverts1;
		public VideoPlayer				videoFront;
		public VideoPlayer				videoBack;
		public bool						noHole			= false;
		//public LayerID					frontLayerID	= LayerID.Layer1;
		//public LayerID					backLayerID		= LayerID.Layer1;

		bool Equals(float v1, float v2)
		{
			if ( Mathf.Abs(v1 - v2) > 0.00001f )
				return true;

			return false;
		}

		public Vector3 MapPoint(MegaBookBuilder book, Vector3 p)
		{
			Vector3 mp = p; //Vector3.zero;

			if ( book.enableflexer )
				mp = flexer.Map(0, mp); //.DeformFirst(this, jverts, jsverts);

			if ( book.enablelander )
				mp = lander.Map(0, mp);

			if ( book.enableturner )
				mp = turner.Map(0, mp);

			return mp;
		}

		public Vector3 MapPoint(MegaBookBuilder book, float u, float v)
		{
			Vector3 mp = new Vector3(u * book.pageWidth, 0.0f, book.pageLength);
			return MapPoint(book, mp);
		}

		public void Update(MegaBookBuilder book, float flip, bool dohole, float alpha)
		{
			if ( book.runcontrollers )
			{
				float angle = -landerangcon.Evaluate(flip);
				if ( Equals(angle, lander.angle) )
				{
					lander.angle = angle;
					deform = true;
				}

				angle = -flexangcon.Evaluate(flip);
				if ( Equals(angle, flexer.angle) )
				{
					flexer.angle = angle;
					deform = true;
				}

				angle = -turnerangcon.Evaluate(flip) * book.Turn_maxAngleCrv.Evaluate(alpha);

				angle += book.GetBottomAngle();
				if ( Equals(angle, turner.angle) )
				{
					turner.angle = angle;
					deform = true;
				}

				float from = turnerfromcon.Evaluate(flip);
				float from1 = turnerfromcon1.Evaluate(flip);

				from = Mathf.Lerp(from, from1, flip);

				if ( Equals(from, turner.from) )
				{
					turner.from = from;
					deform = true;
				}
			}

			if ( flip <= (visobjlow * 14.0f) || flip >= (visobjhigh * 14.0f) )
				showobjects = false;
			else
				showobjects = true;

			if ( (flip < -14.0f || flip >= 28.0f) && holemesh && dohole )
			{
				if ( showinghole == false )
				{
					deform = true;
					showinghole = true;
					if ( noHole )
						showinghole = false;
				}
			}
			else
			{
				if ( showinghole )
				{
					deform = true;
					showinghole = false;
				}
			}

			if ( !book.useholepage )
			{
				showinghole = false;
			}

			if ( deform )
			{
				deform = false;

				if ( stiff )
				{
					//if ( book.enableturner )
					//	turner.Deform(this, verts, sverts);
				}
				else
				{
					if ( showinghole )
					{
						if ( book.enableflexer )
							flexer.DeformFirst(this, jverts1, jsverts1);
						else
						{
							for ( int i = 0; i < jverts1.Length; i++ )
								jsverts1[i] = jverts1[i];
						}

						if ( book.enablelander )
							lander.Deform(this, jsverts1);

						if ( book.enableturner )
							turner.Deform(this, jsverts1);

						holemesh.SetVertices(jsverts1);

						holemesh.RecalculateBounds();
						holemesh.RecalculateNormals();

						if ( book.recalcTangents )
							holemesh.RecalculateTangents();

						if ( mf.sharedMesh != holemesh )
							mf.sharedMesh = holemesh;

						if ( book.updatecollider && collider )
						{
							collider.sharedMesh = null;
							//collider.sharedMesh = holemesh;
							collider.enabled = false;
						}
					}
					else
					{
						if ( book.enableflexer )
							flexer.DeformFirst(this, jverts, jsverts);
						else
						{
							for ( int i = 0; i < jverts.Length; i++ )
								jsverts[i] = jverts[i];
						}

						if ( book.enablelander )
							lander.Deform(this, jsverts);

						if ( book.enableturner )
							turner.Deform(this, jsverts);

						mesh.SetVertices(jsverts);

						mesh.RecalculateBounds();
						mesh.RecalculateNormals();
						if ( book.recalcTangents )
							mesh.RecalculateTangents();
					
						if ( mf.sharedMesh != mesh )
							mf.sharedMesh = mesh;

						// Could do this only when show objects is on
						if ( book.updatecollider && collider )	//&& showobjects )
						{
							collider.enabled = true;
							//if ( collider.sharedMesh != null )
							{
								collider.sharedMesh = null;
								collider.sharedMesh = mesh;
							}
							//else
								//collider.sharedMesh = mesh;
						}
					}
				}
			}

			// Update attached objects
			for ( int i = 0; i < objects.Count; i++ )
			{
				UpdateAttached(book, objects[i], flip);
			}

			if ( videoFront )
			{
				bool playVideo = false;

				if ( flip > -14.0f && flip < 14.0f )
					playVideo = true;

				if ( playVideo )
				{
					if ( !videoFront.isPlaying )
						videoFront.Play();
				}
				else
				{
					if ( videoFront.isPlaying )
						videoFront.Stop();
				}
			}

			if ( videoBack )
			{
				bool playVideo = false;

				if ( flip > 0.0f && flip < 28.0f )
					playVideo = true;

				if ( playVideo )
				{
					if ( !videoBack.isPlaying )
						videoBack.Play();
				}
				else
				{
					if ( videoBack.isPlaying )
						videoBack.Stop();
				}
			}
		}

		void UpdateAttached(MegaBookBuilder book, MegaBookPageObject pobj, float flip)
		{
			GameObject target = pobj.obj;

			if ( target && pobj.active )
			{
				bool show = showobjects;

				if ( pobj.overridevisi )
				{
					if ( flip <= (pobj.visilow * 14.0f) || flip >= (pobj.visihigh * 14.0f) )
						show = false;
					else
						show = true;
				}

				bool showLayer = false;

				if ( pobj.backOfPage )
				{
					if ( (pobj.layerId & book.pageparams[pnum].backLayerID) != 0 )
						showLayer = true;
				}
				else
				{
					if ( (pobj.layerId & book.pageparams[pnum].frontLayerID) != 0 )
						showLayer = true;
				}

				if ( show && showLayer )	//objects )
				{
					if ( !target.activeInHierarchy )
					{
						target.SetActive(true);
						if ( Application.isPlaying )
							pobj.obj.SendMessage("BookAppeared", SendMessageOptions.DontRequireReceiver);
					}
				}
				else
				{
					if ( target.activeInHierarchy )
					{
						if ( Application.isPlaying )
							pobj.obj.SendMessage("BookVanished", SendMessageOptions.DontRequireReceiver);
						target.SetActive(false);
					}
					return;
				}

				// Curves for scale appear vanish
				// button to set the visi values from the current turn value
				// Get scale lerp
				float appearAlpha = 0.0f;
				float vanishAlpha = 0.0f;
				pobj.appearAlpha = 0.0f;
				pobj.vanishAlpha = 0.0f;

				Vector3 scl = pobj.scale;   //target.transform.localScale;
				Vector3 orot = Vector3.zero;   //target.transform.localScale;
				Vector3 opos = Vector3.zero;   //target.transform.localScale;
				float fade = pobj.fade;

				if ( pobj.scaleObj )
				{
					if ( flip >= (pobj.visilow * 14.0f) && flip <= (pobj.visiScaleLow * 14.0f) )
					{
						// scale the object
						pobj.appearAlpha = (flip - (pobj.visilow * 14.0f)) / ((pobj.visiScaleLow * 14.0f) - (pobj.visilow * 14.0f));
						appearAlpha = pobj.appearCrv.Evaluate(pobj.appearAlpha);
						scl = Vector3.Lerp(pobj.appearScale, pobj.scale, appearAlpha);
						orot += Vector3.Lerp(pobj.appearRot, Vector3.zero, pobj.appearCrv.Evaluate(appearAlpha));
						opos += Vector3.Lerp(pobj.appearPos, Vector3.zero, pobj.appearCrv.Evaluate(appearAlpha));
						fade = Mathf.Lerp(0.0f, fade * pobj.appearFade, pobj.fadeAppearCrv.Evaluate(appearAlpha));
					}
					else
					{
						if ( flip >= (pobj.visiScaleHigh * 14.0f) && flip <= (pobj.visihigh * 14.0f) )
						{
							// scale the object
							pobj.vanishAlpha = 1.0f - (((pobj.visihigh * 14.0f) - flip) / ((pobj.visihigh * 14.0f) - (pobj.visiScaleHigh * 14.0f)));
							vanishAlpha = pobj.vanishCrv.Evaluate(pobj.vanishAlpha);
							scl = Vector3.Lerp(pobj.scale, pobj.vanishScale, vanishAlpha);
							orot += Vector3.Lerp(Vector3.zero, pobj.vanishRot, pobj.vanishCrv.Evaluate(vanishAlpha));
							opos += Vector3.Lerp(Vector3.zero, pobj.vanishPos, pobj.vanishCrv.Evaluate(vanishAlpha));
							//fade = pobj.fadeVanishCrv.Evaluate(vanishAlpha);
							fade = Mathf.Lerp(pobj.fade, fade * pobj.vanishFade, pobj.fadeVanishCrv.Evaluate(vanishAlpha));
						}
						else
						{
							pobj.appearAlpha = 1.0f;
							pobj.vanishAlpha = 0.0f;
							appearAlpha = pobj.appearCrv.Evaluate(pobj.appearAlpha);
							vanishAlpha = pobj.appearCrv.Evaluate(pobj.vanishAlpha);
						}
					}

					target.transform.localScale = scl;
					if ( pobj.canvasGroup )
					{
						pobj.canvasGroup.alpha = fade;
					}
				}

				target.transform.localScale = scl;
#if false
				//Vector3 orot = Vector3.zero;   //target.transform.localScale;
				//if ( pobj.rotateObj )
				{
					if ( flip >= (pobj.visilow * 14.0f) && flip <= (pobj.visiScaleLow * 14.0f) )
					{
						// scale the object
						float a = (flip - (pobj.visilow * 14.0f)) / ((pobj.visiScaleLow * 14.0f) - (pobj.visilow * 14.0f));
						orot += Vector3.Lerp(pobj.appearRot, Vector3.zero, pobj.appearCrv.Evaluate(a));
					}
					else
					{
						if ( flip >= (pobj.visiScaleHigh * 14.0f) && flip <= (pobj.visihigh * 14.0f) )
						{
							// scale the object
							float a = 1.0f - (((pobj.visihigh * 14.0f) - flip) / ((pobj.visihigh * 14.0f) - (pobj.visiScaleHigh * 14.0f)));
							orot += Vector3.Lerp(Vector3.zero, pobj.vanishRot, pobj.vanishCrv.Evaluate(a));
						}
					}

					//target.transform.localEulerAngles = orot;
				}
#endif
				if ( Application.isPlaying )
				{
					if ( pobj.message )
					{
						pobj.obj.SendMessage("BookVisibility", ((flip / 14.0f) - pobj.visilow) / (pobj.visihigh - pobj.visilow), SendMessageOptions.DontRequireReceiver);
						pobj.obj.SendMessage("BookAppear", appearAlpha, SendMessageOptions.DontRequireReceiver);
						pobj.obj.SendMessage("BookVanish", vanishAlpha, SendMessageOptions.DontRequireReceiver);
					}
				}

				Vector3 v0 = jsverts[pobj.BaryVerts[0]];
				Vector3 v1 = jsverts[pobj.BaryVerts[1]];
				Vector3 v2 = jsverts[pobj.BaryVerts[2]];

				Vector3 pos = obj.transform.localToWorldMatrix.MultiplyPoint(GetCoordMine(v0, v1, v2, pobj.BaryCoord));

				// Rotation
				Vector3 va = v1 - v0;
				Vector3 vb = v2 - v1;

				Vector3 norm = obj.transform.TransformDirection(Vector3.Cross(va, vb).normalized);

				v0 = jsverts[pobj.BaryVerts1[0]];
				v1 = jsverts[pobj.BaryVerts1[1]];
				v2 = jsverts[pobj.BaryVerts1[2]];

				Vector3 fwd = obj.transform.localToWorldMatrix.MultiplyPoint(GetCoordMine(v0, v1, v2, pobj.BaryCoord1)) - pos;

				//norm = obj.transform.TransformDirection(norm);
				Quaternion erot = Quaternion.Euler(pobj.rot + orot);

				Quaternion rot = Quaternion.identity;
				if ( fwd == Vector3.zero )
					rot = erot;
				else
					rot = Quaternion.LookRotation(fwd, norm) * erot;

				pobj.alpha = ((flip / 14.0f) - pobj.visilow) / (pobj.visihigh - pobj.visilow);
				//Quaternion rot = Quaternion.LookRotation(fwd, norm) * erot;
				//Quaternion rot = Quaternion.FromToRotation(obj.transform.up, norm) * erot;
				target.transform.position = pos + opos + (pobj.offsetCrv.Evaluate(pobj.alpha) * pobj.offset * norm);	//.normalized);
				//target.transform.rotation = rot * Quaternion.Euler(orot);
				target.transform.rotation = rot;
			}
		}

		Vector3 GetCoordMine(Vector3 A, Vector3 B, Vector3 C, Vector3 bary)
		{
			Vector3 p = Vector3.zero;
			p.x = (bary.x * A.x) + (bary.y * B.x) + (bary.z * C.x);
			p.y = (bary.x * A.y) + (bary.y * B.y) + (bary.z * C.y);
			p.z = (bary.x * A.z) + (bary.y * B.z) + (bary.z * C.z);

			return p;
		}

		public void GetVerts(bool force)
		{
			if ( force || jsverts.Length == 0 )	//|| mesh == null )
			{
				DisposeArrays();
				jverts = new NativeArray<Vector3>(mesh.vertices, Allocator.Persistent);
				jsverts = new NativeArray<Vector3>(mesh.vertices, Allocator.Persistent);

				if ( holemesh != null )
				{
					jverts1 = new NativeArray<Vector3>(holemesh.vertices, Allocator.Persistent);
					jsverts1 = new NativeArray<Vector3>(holemesh.vertices, Allocator.Persistent);
					holemesh.MarkDynamic();
				}
				mesh.MarkDynamic();
			}
		}

		public void DisposeArrays()
		{
			if ( jverts.IsCreated ) jverts.Dispose();
			if ( jsverts.IsCreated ) jsverts.Dispose();
			if ( jverts1.IsCreated ) jverts1.Dispose();
			if ( jsverts1.IsCreated ) jsverts1.Dispose();

			jverts = default;
			jsverts = default;
			jverts1 = default;
			jsverts1 = default;
		}
	}
}