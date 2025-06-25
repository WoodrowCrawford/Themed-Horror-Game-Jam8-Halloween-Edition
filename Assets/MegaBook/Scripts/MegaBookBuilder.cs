using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;
using Unity.Collections;
using UnityEngine.Video;

namespace MegaBook
{
	[System.Flags]
	public enum RenderLayers
	{
		Nothing				= 0,
		RenderingLayer1		= 1,
		LightLayerDefault	= 1,
		RenderingLayer2		= 2,
		LightLayer1			= 2,
		RenderingLayer3		= 4,
		LightLayer2			= 4,
		RenderingLayer4		= 8,
		LightLayer3			= 8,
		RenderingLayer5		= 16,
		LightLayer4			= 16,
		LightLayer5			= 32,
		RenderingLayer6		= 32,
		RenderingLayer7		= 64,
		LightLayer6			= 64,
		RenderingLayer8		= 128,
		LightLayer7			= 128,
		RenderingLayer9		= 256,
		DecalLayerDefault	= 256,
		Default				= 257,
		DecalLayer1			= 512,
		RenderingLayer10	= 512,
		RenderingLayer11	= 1024,
		DecalLayer2			= 1024,
		RenderingLayer12	= 2048,
		DecalLayer3			= 2048,
		RenderingLayer13	= 4096,
		DecalLayer4			= 4096,
		RenderingLayer14	= 8192,
		DecalLayer5			= 8192,
		RenderingLayer15	= 16384,
		DecalLayer6			= 16384,
		DecalLayer7			= 32768,
		RenderingLayer16	= 32768,
		Everything			= 65535,
	}

	[System.Serializable]
	public class MegaBookBuiltEvent : UnityEvent<MegaBookBuilder> {}
	[System.Serializable]
	public class MegaBookPageTurnEvent : UnityEvent<MegaBookBuilder, float> { }

	[ExecuteInEditMode]
	public class MegaBookBuilder : MonoBehaviour
	{
		public List<MegaBookPage>			pages				= new List<MegaBookPage>();
		public List<MegaBookPageParams>		pageparams			= new List<MegaBookPageParams>();
		public int							seed				= 0;
		public int							NumPages			= 10;
		float								BottomAngle			= 0.0f;
		public int							Flex_RandomSeed		= 0;
		public float						Flex_CCenter		= 0.652f;
		public float						Flex_CArea			= 0.7f;
		public float						Flex_MaxAngle		= 190.0f;
		public float						Flex_MinAngle		= 0.0f;
		public bool							Flex_Random			= true;
		public float						Flex_RandomDegree	= 25.0f;
		public float						pageWidth			= 1.0f;
		public float						pageLength			= 0.7f;
		public float						pageHeight			= 0.0025f;
		public float						pageGap				= 0.0025f;

		// Noise
		public bool							addNoise			= false;
		public Vector3						noiseStrength		= Vector3.zero;
		public float						noiseScale			= 1.0f;
		public float						noiseScaleVert		= 1.0f;
		public float						noisePhase			= 1.0f;
		public float						noisePhaseVert		= 1.0f;
		public bool							noiseSame			= false;
		public bool							noiseSameVert		= false;
		public bool							noiseApply			= false;
		public bool							noiseRealtime		= true;
		public bool							useNoiseBook		= true;
		public AnimationCurve				noiseBook			= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public AnimationCurve				noiseCurveX			= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve				noiseCurveY			= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		// falloff curves for noise
		public bool							showPageMesh		= false;
		public Color						highlightColor		= Color.white;
		public int							LengthSegs			= 8;
		public int							WidthSegs			= 30;
		public int							HeightSegs			= 1;
		public float						Turn_CCenter		= 0.0f;
		public float						Turn_CArea			= 0.05f;
		public float						Turn_maxAngle		= 180.0f;
		public AnimationCurve				Turn_maxAngleCrv	= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public float						Turn_minAngle		= 0.0f;
		public AnimationCurve				Turn_minAngleCrv	= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
		public float						Turn_CLevel			= 0.0f;
		public float						Turn_Spread			= 5.0f;
		public float						Turn_Spread1		= 5.0f;
		public float						Turn_SpreadRead		= 5.0f;
		public float						Turn_SpreadRead1	= 5.0f;
		public AnimationCurve				Turn_SpreadCrv		= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public float						Land_CCenter		= 0.25f;
		public float						Land_CArea			= 0.8f;
		public float						Land_maxAngle		= 0.0f;
		public float						Land_minAngle		= 0.0f;
		public Material						basematerial;
		public Material						basematerial1;
		public Material						basematerial2;
		public float						edgeUVSize			= 1.0f;
		public float						edgeUVOff			= 0.0f;
		public Mesh							pagemesh;
		public Mesh							holemesh;
		public GameObject					pageobject;
		public GameObject					holeobject;
		public int							frontmat;
		public int							backmat;
		public Vector3						rotate				= Vector3.zero;
		public bool							rebuildmeshes		= false;
		public bool							rebuild				= false;
		public float						Flip				= 0.0f;
		public bool							enableturner		= true;
		public bool							enablelander		= true;
		public bool							enableflexer		= true;
		public bool							runcontrollers		= true;
		public List<Texture2D>				pagetextures		= new List<Texture2D>();
		public bool							PivotBase			= false;	//true;
		public bool							PivotEdge			= true;
		public bool							animate				= false;
		public float						time				= 0.0f;
		public float						speed				= 1.0f;
		public AnimationCurve				pagesectioncrv		= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public float						BottomMaxAngle		= 180.0f;
		public float						shuffle				= 14.0f;
		public float						spineradius			= 0.0f;
		public AnimationCurve				spineCurve			= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public GameObject					spine;
		public bool							usebookthickness	= false;
		public float						bookthickness		= 1.0f;	// Page gap will be calculated 
		public bool							showmaterials		= false;
		public int							shownummaterials	= 10;
		public bool							showmesh			= false;
		public Vector2						matpos				= Vector2.zero;
		public Texture2D					background;
		public Texture2D					background1;
		public Rect							copyarea			= new Rect(0.1f, 0.1f, 0.8f, 0.8f);
		public Rect							copyarea1			= new Rect(0.1f, 0.1f, 0.8f, 0.8f);
		public bool							spineedge			= true;
		public bool							UseThreading		= false;
		public bool							changespineangle	= true;
		public Vector3						pagesizevariation	= Vector3.zero;
		public Texture2D					mask;
		public bool							nobackgrounds		= false;
		public bool							useholepage			= true;
		public int							xhole				= 1;
		public int							yhole				= 1;
		public bool							showobjects			= false;
		public int							editpage			= 0;
		public bool							showmeshoptions		= false;
		public bool							showflipoptions		= false;
		public bool							showbackgrounds		= false;
		public bool							showcurves			= false;

		// Page Texture streaming, file or web
		public int							streamcount			= 0;
		public string						url					= "www.website.com/pages/";
		public string						filelist			= "pages.txt";
		public bool							usefilelist			= false;
		public bool							usefilearray		= false;
		public List<string>					files				= new List<string>();
		public string						prefix				= "";
		public string						extension			= ".jpg";

		// Control Options
		public float						page				= 0.0f;
		public float						turnspd				= 0.0f;
		public float						turntime			= 1.0f;
		public bool							Snap				= true;

		public bool							updatecollider		= false;
#if UNITY_6000_0_OR_NEWER
		public PhysicsMaterial				physicsMat;
#else
		public PhysicMaterial				physicsMat;
#endif
		public Transform					frontcover;
		public Vector3						frontpivot;
		public Vector3						frontoffset;
		public float						frontang;
		public Transform					backcover;
		public float						backang;
		public Vector3						backpivot;
		public bool							showcover			= false;
		public Transform					spineBone;
		public Vector3						spinepivot;
		public MegaBookAxis					frontAxis			= MegaBookAxis.Z;
		public MegaBookAxis					backAxis			= MegaBookAxis.Z;
		public MegaBookAxis					spineAxis			= MegaBookAxis.Z;
		public Vector3						coverOffset			= Vector3.zero;
		public MegaBookCover				bookCover;
		public Vector3						coverScale			= Vector3.one;
		public bool							autoFit				= false;
		public Vector3						autoFitSize			= Vector3.one;
		public float						spineScale			= 1.0f;

#if false
		public bool							showdynammesh		= false;
		public bool							dynammeshenabled	= false;
		public MegaBookDynamicMesh			dynamobj;
		public Vector3						dynamoffset;
		public Vector3						dynamscale			= Vector3.one;
		public Vector3						dynamrot;
		public Vector3						backdynamoffset;
		public Vector3						backdynamscale		= Vector3.one;
		public Vector3						backdynamrot;
#endif
		public bool							usecols				= false;
		public Color						color				= Color.white;
		public bool							useMatCol			= false;
		public Color						matColor			= Color.white;

		public bool							linkeditpage		= true;

		public bool							castshadows			= true;
		public bool							receiveshadows		= true;
		public bool							uselightprobes		= false;
		public RenderLayers					renderLayers		= RenderLayers.Default;

		public bool							useuv2				= true;
		public bool							dontBuildOnPlay		= true;
		public bool							recalcTangents		= false;

		public Transform					anchorOverride		= null;
		public ReflectionProbeUsage			reflectionProbes	= ReflectionProbeUsage.Off;
		public LightProbeUsage				lightProbeUsage		= LightProbeUsage.Off;
		public MotionVectorGenerationMode	motionVectors		= MotionVectorGenerationMode.Camera;
		public GameObject					proxyVolume			= null;

		// Events
		public MegaBookBuiltEvent			bookBuiltEvent;
		public MegaBookPageTurnEvent		pageTurnEvent;

		public string						shaderTextureName	= "";
		public string						shaderTexture		= "_MainTex";

		MegaBookPerlin						iperlin				= MegaBookPerlin.Instance;

		// Head bands and spine fabric
		public bool							showBindings		= false;
		public bool							addHeadband			= false;
		public float						headbandLength		= 1.0f;
		public float						headbandRadius		= 0.01f;
		[Range(1, 64)]
		public int							headbandSegs		= 8;
		[Range(3, 32)]
		public int							headbandSides		= 8;
		public float						headbandWidth		= 1.0f;
		public Vector3						headbandOffset		= new Vector3(0.0f, 0.0f, 0.0f);
		public Material						headbandMat;

		public bool							addFabric			= false;
		public float						fabricLength		= 1.0f;
		public float						fabricWidth			= 1.0f;
		public float						fabricThickness		= 0.01f;
		public int							fabricSegs			= 8;
		public Vector3						fabricOffset		= Vector3.zero;
		public Material						fabricMat;
		public bool							updateBindings		= false;

		public TMProSubdiv					TMProSubdiv			= TMProSubdiv.Auto;

		public MegaBookHeadband				headband1;
		public MegaBookHeadband				headband2;
		public MegaBookSpineFabric			spineFabric;
		public bool							useUndo				= false;
		public int							toolbarMode			= 0;
		public bool							showgeneral			= true;

		public Vector2						scrollpos;
		static MaterialPropertyBlock		pblock;
		bool								updateObjs			= false;

		[ContextMenu("Help")]
		public void Help()
		{
			Application.OpenURL("http://www.west-racing.com/mf/?page_id=5422");
		}

		public int GetPageCount()
		{
			return pages.Count;
		}

		public int GetCurrentPage()
		{
			return (int)page;
		}

		public float GetCurrentTurn()
		{
			return page % 1.0f;
		}

		public float GetBottomAngle()
		{
			return BottomAngle;
		}

		public void GetTransform(int page, Vector2 pos, out Vector3 p, out Quaternion r)
		{
			p = Vector3.zero;
			r = Quaternion.identity;
		}

		public IEnumerator DownloadTexture(string url, int p, bool front)
		{
			UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url);
			yield return www.SendWebRequest();

			Texture2D t = UnityEngine.Networking.DownloadHandlerTexture.GetContent(www);
			SetPageTexture(t, p, front);
		}

		public Texture2D GetPageTexture(int p, bool front)
		{
			Material[] mats = pages[p].obj.GetComponent<MeshRenderer>().materials;

			if ( front )
				return (Texture2D)mats[0].GetTexture(shaderTexture);
			else
				return (Texture2D)mats[1].GetTexture(shaderTexture);
		}

		public void SetPageTexture(Texture2D tex, int p, bool front)
		{
			Material[] mats = pages[p].obj.GetComponent<MeshRenderer>().materials;

			if ( front )
				mats[0].SetTexture(shaderTexture, tex);
			else
				mats[1].SetTexture(shaderTexture, tex);
		}

		public float MinPageVal()
		{
			if ( frontcover || bookCover )
				return -1.0f;

			return 0.0f;
		}

		public float MaxPageVal()
		{
			if ( backcover || bookCover )
				return NumPages + 1;

			return NumPages;
		}

		public void NextPage()
		{
			page += 1.0f;
			if ( page > MaxPageVal() )
				page = MaxPageVal();
		}

		public void PrevPage()
		{
			page -= 1.0f;

			if ( page < MinPageVal() )
				page = MinPageVal();
		}

		public void SetPage(float val, bool force)
		{
			page = val;
			page = Mathf.Clamp(page, MinPageVal(), MaxPageVal());
			if ( force )
			{
				turnspd = 0.0f;
				Flip = page;
			}
		}

		public float GetPage()
		{
			return Flip;
		}

		public void SetSnap(bool snapvalue)
		{
			Snap = snapvalue;
		}

		public bool GetSnap()
		{
			return Snap;
		}

		public void SetTurnTime(float time)
		{
			turntime = time;
		}

		public float GetTurnTime()
		{
			return turntime;
		}

		public void ApplyScaling()
		{
			Vector3 scl = transform.lossyScale;

			pageWidth *= scl.x;
			pageLength *= scl.z;
			pageHeight *= scl.y;
			rebuildmeshes = true;
			UpdateSettings();
		}

		int pageindex = 0;

		public void BuildPageTextures(MegaBookPageParams page)
		{
			page.madefront = null;
			page.madeback = null;
			page.texturesDirty = false;
			//if ( nobackgrounds == false )
			{
				if ( page.pageobj == null )
				{
					if ( (nobackgrounds == false) || (page.usebackground && (page.background || background)) )    //&& page.usebackground )
					{
						Texture2D bg = background;
						if ( page.background )
							bg = page.background;

						Rect rect = copyarea;
						if ( page.copyarea.width != 0.0f && page.copyarea.height != 0.0f )
							rect = page.copyarea;

						if ( bg && page.front )
						{
							if ( bg.isReadable && page.front.isReadable )
								page.madefront = MakePage(bg, page.front, rect, false, page.alphatexturefront, page.frontMask);
							else
							{
								if ( !bg.isReadable )
									Debug.LogWarning("Background Texture " + bg.name + " is not readable");

								if ( !page.front.isReadable )
									Debug.LogWarning("Front Texture " + page.front.name + " is not readable");
							}
						}
					}

					if ( (nobackgrounds == false) || (page.usebackground1 && (page.background1 || background1)) ) //&& page.usebackground1 )
					{
						Texture2D bg = background1;
						if ( page.background1 )
							bg = page.background1;

						Rect rect = copyarea1;
						if ( page.copyarea1.width != 0.0f && page.copyarea1.height != 0.0f )
							rect = page.copyarea1;

						if ( bg && page.back )
						{
							if ( bg.isReadable && page.back.isReadable )
								page.madeback = MakePage(bg, page.back, rect, true, page.alphatextureback, page.backMask);
							else
							{
								if ( !bg.isReadable )
									Debug.LogWarning("Background Texture " + bg.name + " is not readable");

								if ( !page.back.isReadable )
									Debug.LogWarning("Back Texture " + page.back.name + " is not readable");
							}
						}
					}
				}
			}
		}

		public void BuildPageTextures()
		{
			for ( int i = 0; i < pageparams.Count; i++ )
			{
				MegaBookPageParams page = pageparams[i];
				BuildPageTextures(page);
			}
		}

		Texture2D MakePage(Texture2D src, Texture2D image, Rect area, bool back, bool alpha, Texture2D maskover)
		{
			if ( src )
			{
				Texture2D page = new Texture2D(src.width, src.height);

				page.SetPixels32(src.GetPixels32());

				Rect rect = area;

				if ( back )
				{
					float w = rect.width;
					rect.xMax = 1.0f - rect.xMin;
					rect.xMin = rect.xMax - w;
				}

				rect.xMin = rect.xMin * src.width;
				rect.xMax = rect.xMax * src.width;
				rect.yMin = rect.yMin * src.height;
				rect.yMax = rect.yMax * src.height;

				float width = 1.0f / (float)src.width;
				float height = 1.0f / (float)src.height;

				float h1 = 1.0f / rect.height;
				float w1 = 1.0f / rect.width;

				Texture2D maskTex = mask;
				if ( maskover )
					maskTex = maskover;

				if ( maskTex )
				{
					if ( image )
					{
						if ( alpha )
						{
							for ( int y = (int)rect.y; y < rect.y + rect.height; y++ )
							{
								if ( y >= 0 && y < page.height )
								{
									float ya = (y - rect.y) * h1;	/// rect.height;
									float ya1 = (float)y * height;	/// (float)src.height;

									for ( int x = (int)rect.x; x < rect.x + rect.width; x++ )
									{
										if ( x >= 0 && x < page.width )
										{
											float xa = (x - rect.x) * w1;	/// rect.width;
											float xa1 = (float)x * width;	/// (float)src.width;
											Color col = image.GetPixelBilinear(xa, ya);
											if ( col.a != 0.0f )
											{
												Color col1 = page.GetPixelBilinear(xa1, ya1);
												Color mcol = maskTex.GetPixelBilinear(xa, ya);
												col = Color.Lerp(col1, col, mcol.r);
												page.SetPixel(x, y, col);	//image.GetPixelBilinear(xa, ya));
											}
										}
									}
								}
							}
						}
						else
						{
							for ( int y = (int)rect.y; y < rect.y + rect.height; y++ )
							{
								if ( y >= 0 && y < page.height )
								{
									float ya = (y - rect.y) * h1;	/// rect.height;
									float ya1 = (float)y * height;	/// (float)src.height;

									for ( int x = (int)rect.x; x < rect.x + rect.width; x++ )
									{
										if ( x >= 0 && x < page.width )
										{
											float xa = (x - rect.x) * w1;	/// rect.width;
											float xa1 = (float)x * width;	/// (float)src.width;
											Color col = image.GetPixelBilinear(xa, ya);
											Color col1 = page.GetPixelBilinear(xa1, ya1);
											Color mcol = maskTex.GetPixelBilinear(xa, ya);
											col = Color.Lerp(col1, col, mcol.r);
											page.SetPixel(x, y, col);	//image.GetPixelBilinear(xa, ya));
										}
									}
								}
							}
						}
					}
				}
				else
				{
					if ( image )
					{
						if ( alpha )
						{
							for ( int y = (int)rect.y; y < rect.y + rect.height; y++ )
							{
								if ( y >= 0 && y < page.height )
								{
									float ya = (y - rect.y) / rect.height;

									for ( int x = (int)rect.x; x < rect.x + rect.width; x++ )
									{
										if ( x >= 0 && x < page.width )
										{
											float xa = (x - rect.x) / rect.width;
											Color c = image.GetPixelBilinear(xa, ya);
											if ( c.a != 0.0f )
												page.SetPixel(x, y, c);	//image.GetPixelBilinear(xa, ya));
										}
									}
								}
							}
						}
						else
						{
							for ( int y = (int)rect.y; y < rect.y + rect.height; y++ )
							{
								if ( y >= 0 && y < page.height )
								{
									float ya = (y - rect.y) / rect.height;

									for ( int x = (int)rect.x; x < rect.x + rect.width; x++ )
									{
										if ( x >= 0 && x < page.width )
										{
											float xa = (x - rect.x) / rect.width;
											page.SetPixel(x, y, image.GetPixelBilinear(xa, ya));
										}
									}
								}
							}
						}
					}
				}

				page.Apply();	//true, true);

				return page;
			}
			else
				return image;
		}

		private void Awake()
		{
			DisposeArrays();
		}

		private void OnDestroy()
		{
			DisposeArrays();
		}

		void Start()
		{
			lastPage = -1000.0f;
#if UNITY_6000_0_OR_NEWER
			if ( GraphicsSettings.defaultRenderPipeline )
				shaderTexture = "_BaseMap";
			else
				shaderTexture = "_MainTex";
#else
			if ( GraphicsSettings.renderPipelineAsset )
				shaderTexture = "_BaseMap";
			else
				shaderTexture = "_MainTex";
#endif
			if ( shaderTextureName.Length > 0 )
				shaderTexture = shaderTextureName;

			if ( bookBuiltEvent == null )
				bookBuiltEvent = new MegaBookBuiltEvent();

			if ( pageTurnEvent == null )
				pageTurnEvent = new MegaBookPageTurnEvent();

			noiseApply = true;

#if UNITY_EDITOR
			if ( UnityEditor.PrefabUtility.IsPartOfAnyPrefab(gameObject) )
			{
				if ( !Application.isPlaying )
				{
					RebuildPrefab();
				}
			}
#endif
			if ( !dontBuildOnPlay )
			{
				if ( Application.isPlaying )
				{
					//if ( background && madepages.Count == 0 )
						BuildPageTextures();
				}
				Flip = page;
				rebuild = true;
			}
		}
	
		void RemovePages()
		{
			List<Transform> children = new List<Transform>();

			for ( int i = 0; i < gameObject.transform.childCount; i++ )
			{
				if ( gameObject.transform.GetChild(i).name == "Page" )
					children.Add(gameObject.transform.GetChild(i));
			}

			for ( int i = 0; i < children.Count; i++ )
			{
				MeshFilter mf = children[i].gameObject.GetComponent<MeshFilter>();
				MeshRenderer mr = children[i].gameObject.GetComponent<MeshRenderer>();

				//if ( mr )
					//Material[] mats = mr.sharedMaterials;

				if ( Application.isEditor )
					DestroyImmediate(children[i].gameObject);
				else
					Destroy(children[i].gameObject);
			}

			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}

		public void ClearMadeTextures(MegaBookPageParams pp)
		{
			pp.madefront = null;
			pp.madeback = null;
		}

		public void ClearMadeTextures()
		{
			for ( int i = 0; i < pageparams.Count; i++ )
			{
				pageparams[i].madefront = null;
				pageparams[i].madeback = null;
			}
		}

		GameObject MakePageObject(MegaBookPage page, int pnum)	//, Material mat)
		{
			GameObject cobj = new GameObject();
			cobj.name = "Page";
			cobj.layer = gameObject.layer;
			MeshFilter mf = cobj.AddComponent<MeshFilter>();
			MeshRenderer cmr = cobj.AddComponent<MeshRenderer>();

			cmr.shadowCastingMode = castshadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
			cmr.receiveShadows = receiveshadows;
			if ( uselightprobes )
				cmr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
			else
				cmr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;

			cmr.probeAnchor = anchorOverride;
			cmr.reflectionProbeUsage = reflectionProbes;
			cmr.lightProbeUsage = lightProbeUsage;
			cmr.motionVectorGenerationMode = motionVectors;

			cmr.renderingLayerMask = (uint)renderLayers;

			page.pnum = pnum;
			page.mf = mf;

			MegaBookPageParams pp = pageparams[pageindex];

			switch ( pp.colliderOverride )
			{
				case ColliderOverride.UseBook:
					if ( updatecollider )
					{
						page.collider = cobj.AddComponent<MeshCollider>();
						page.collider.sharedMaterial = physicsMat;
					}
					break;
				case ColliderOverride.NoCollider:
					break;
				case ColliderOverride.Collider:
					page.collider = cobj.AddComponent<MeshCollider>();
					page.collider.sharedMaterial = physicsMat;
					break;
			}

			float width = pageWidth;
			float length = pageLength;
			float height = pageHeight;

			Vector3 lscl = transform.lossyScale;
			width *= lscl.x;
			length *= lscl.z;
			height *= lscl.y;

			if ( pnum > 0 && pnum < NumPages - 1 )
			{
				width += Random.Range(-1.0f, 1.0f) * pagesizevariation.x;
				length += Random.Range(-1.0f, 1.0f) * pagesizevariation.y;
				height += Random.Range(-1.0f, 1.0f) * pagesizevariation.z;
			}

			page.stiff = pp.stiff;

			int frontMatId = 0;
			int backMatId = 1;

			if ( pageobject || pp.pageobj )
			{
			}
			else
			{
				Vector3 rot = rotate;
				Vector3 pivot = Vector3.zero;

				Mesh pm = pagemesh;
				if ( pp.pagemesh )
				{
					pm = pp.pagemesh;
					rot = pp.rotate;
					pivot = pp.pivot;
				}

				if ( pm && pm.subMeshCount >= 3 && pm.isReadable )
				{
					page.noHole = false;
					// make a copy of pagemesh
					Mesh mesh = CopyMesh(page, pm, width, length, height, rot, pivot, pp.scale);
					if ( pp.scale < 1.0f )
						page.noHole = true;
					mf.sharedMesh = mesh;

					int m1 = frontmat;
					int m2 = backmat;

					if ( pp.frontmatindex != -1 )
						m1 = pp.frontmatindex;

					if ( pp.backmatindex != -1 )
						m2 = pp.backmatindex;

					m1 = Mathf.Clamp(m1, 0, 2);
					m2 = Mathf.Clamp(m2, 0, 2);

					if ( pp.swapsides )
					{
						int m = m1;
						m1 = m2;
						m2 = m;
					}

					// New mesh content system
					int nmb = GetNumMaterialsBack(pp);
					int nmf = GetNumMaterialsFront(pp);
					int tnum = nmf + nmb;

					Material[] mats = new Material[3 + tnum];

					for ( int i = 0; i < nmf; i++ )
						mats[3 + i] = GetMaterial(page.pnum, true, i);

					nmb = GetNumMaterialsBack(pp);
					for ( int i = 0; i < nmb; i++ )
						mats[3 + nmf + i] = GetMaterial(page.pnum, false, i);

					if ( pp.frontmat )
						mats[m1] = new Material(pp.frontmat);
					else
					{
						if ( basematerial )
							mats[m1] = new Material(basematerial);
					}

					if ( pp.backmat )
						mats[m2] = new Material(pp.backmat);
					else
					{
						if ( basematerial1 )
							mats[m2] = new Material(basematerial1);
					}

					if ( pp.edgemat )
						mats[2] = pp.edgemat;
					else
						mats[2] = basematerial2;

					if ( pp.madefront )
						mats[m1].SetTexture(shaderTexture, pp.madefront);
					else
						mats[m1].SetTexture(shaderTexture, pp.front);

					if ( pp.madeback )
						mats[m2].SetTexture(shaderTexture, pp.madeback);
					else
						mats[m2].SetTexture(shaderTexture, pp.back);

					if ( pp.useFrontColor )
						mats[m1].color = pp.frontColor;
					else
					{
						if ( useMatCol )
							mats[m1].color = matColor;
					}

					if ( pp.useBackColor )
						mats[m2].color = pp.backColor;
					else
					{
						if ( useMatCol )
							mats[m2].color = matColor;
					}

					cmr.sharedMaterials = mats;

					if ( page.collider )
					{
						page.collider.sharedMesh = null;
						page.collider.sharedMesh = mesh;
					}

					Mesh hmesh = holemesh;
					if ( pp.holemesh != null )
						hmesh = pp.holemesh;

					if ( hmesh )
						page.holemesh = CopyMeshHole(page, hmesh, width, length, height, rot, pivot, pp.scale);

					frontMatId = m1;
					backMatId = m2;
				}
				else
				{
					page.noHole = false;
					// Make a procedural page mesh
					int ws = WidthSegs;
					if ( pp.widthSegs > 0 )
						ws = pp.widthSegs;

					int ls = LengthSegs;
					if ( pp.lengthSegs > 0 )
						ls = pp.lengthSegs;

					Mesh mesh = CreatePageMesh(page, width, length, height, ws, ls, HeightSegs);

					if ( useholepage )
						page.holemesh = CreatePageMeshHole(page, width, length, height, ws, ls, HeightSegs, xhole, yhole);

					mf.sharedMesh = mesh;	//new Mesh();

					Material[] mats;

					// New mesh content system
					int nmb = GetNumMaterialsBack(pp);
					int nmf = GetNumMaterialsFront(pp);
					int tnum = nmf + nmb;

					mats = new Material[3 + tnum];

					for ( int i = 0; i < nmf; i++ )
						mats[3 + i] = GetMaterial(page.pnum, true, i);

					nmb = GetNumMaterialsBack(pp);
					for ( int i = 0; i < nmb; i++ )
						mats[3 + nmf + i] = GetMaterial(page.pnum, false, i);

					if ( pp.frontmat )
						mats[0] = new Material(pp.frontmat);
					else
					{
						if ( basematerial )
							mats[0] = new Material(basematerial);
					}

					if ( pp.backmat )
						mats[1] = new Material(pp.backmat);
					else
					{
						if ( basematerial1 )
							mats[1] = new Material(basematerial1);
					}

					if ( pp.edgemat )
						mats[2] = pp.edgemat;
					else
						mats[2] = basematerial2;

					if ( pp.madefront )
						mats[0].SetTexture(shaderTexture, pp.madefront);
					else
						mats[0].SetTexture(shaderTexture, pp.front);

					if ( pp.madeback )
						mats[1].SetTexture(shaderTexture, pp.madeback);
					else
						mats[1].SetTexture(shaderTexture, pp.back);

					if ( pp.useFrontColor )
						mats[0].color = pp.frontColor;
					else
					{
						if ( useMatCol )
							mats[0].color = matColor;
					}

					if ( pp.useBackColor )
						mats[1].color = pp.backColor;
					else
					{
						if ( useMatCol )
							mats[1].color = matColor;
					}

					cmr.sharedMaterials = mats;

					if ( page.collider )
					{
						page.collider.sharedMesh = null;
						page.collider.sharedMesh = mesh;
					}

					frontMatId = 0;
					backMatId = 1;
				}
			}

			page.mesh.name = "PageMesh " + pnum;
			// Attach objects
			page.visobjlow = pp.visobjlow;
			page.visobjhigh = pp.visobjhigh;
			page.GetVerts(true);

			for ( int i = 0; i < pp.objects.Count; i++ )
				AttachObject(page, pp.objects[i]);

			pageindex++;
			pageindex = pageindex % pageparams.Count;
			cobj.transform.parent = transform;

			if ( pblock == null )
				pblock = new MaterialPropertyBlock();

			// New video code
			if ( pp.videoFrontClip )
			{
				page.videoFront = cobj.AddComponent<VideoPlayer>();
				page.videoFront.clip = pp.videoFrontClip;
				page.videoFront.isLooping = true;
				page.videoFront.playOnAwake = false;
				page.videoFront.renderMode = VideoRenderMode.RenderTexture;
				int rs = (int)pp.videoFrontReduceSize;
				RenderTexture rt = new RenderTexture((int)pp.videoFrontClip.width / rs, (int)pp.videoFrontClip.height / rs, 32);
				page.videoFront.targetTexture = rt;
				page.videoFront.SetDirectAudioVolume(0, pp.videoFrontVol);

				MeshRenderer mr = cobj.GetComponent<MeshRenderer>();
				pblock.SetTexture(shaderTexture, rt);
				if ( pp.videoFrontMatID != -1 )
					mr.SetPropertyBlock(pblock, pp.videoFrontMatID);
				else
					mr.SetPropertyBlock(pblock, frontMatId);

				if ( !pp.videoFrontPlayVis )
					page.videoFront.Play();
			}

			if ( pp.videoBackClip )
			{
				page.videoBack = cobj.AddComponent<VideoPlayer>();
				page.videoBack.clip = pp.videoBackClip;
				page.videoBack.isLooping = true;
				page.videoBack.playOnAwake = false;
				page.videoBack.renderMode = VideoRenderMode.RenderTexture;
				int rs = (int)pp.videoBackReduceSize;
				RenderTexture rt = new RenderTexture((int)pp.videoBackClip.width / rs, (int)pp.videoBackClip.height / rs, 32);
				page.videoBack.targetTexture = rt;
				page.videoBack.SetDirectAudioVolume(0, pp.videoBackVol);

				MeshRenderer mr = cobj.GetComponent<MeshRenderer>();
				pblock.SetTexture(shaderTexture, rt);

				if ( pp.videoBackMatID != -1 )
					mr.SetPropertyBlock(pblock, pp.videoBackMatID);
				else
					mr.SetPropertyBlock(pblock, backMatId);

				if ( !pp.videoBackPlayVis )
					page.videoBack.Play();
			}

			return cobj;
		}

		static void MakeQuad1(List<int> f, int a, int b, int c, int d)
		{
			f.Add(a);
			f.Add(b);
			f.Add(c);

			f.Add(c);
			f.Add(d);
			f.Add(a);
		}

		int MaxComponent(Vector3 v)
		{
			if ( Mathf.Abs(v.x) > Mathf.Abs(v.y) )
			{
				if ( Mathf.Abs(v.x) > Mathf.Abs(v.z) )
					return 0;
				else
					return 2;
			}
			else
			{
				if ( Mathf.Abs(v.y) > Mathf.Abs(v.z) )
					return 1;
				else
					return 2;
			}
		}

		Mesh CreatePageMesh(MegaBookPage page, float width, float length, float height, int widthsegs, int lengthsegs, int heightsegs)
		{
			Mesh mesh = new Mesh();

			Vector3 vb = new Vector3(width, height, length) / 2.0f;
			Vector3 va = -vb;

			if ( PivotBase )
			{
				va.y = 0.0f;
				vb.y = height;
			}

			if ( PivotEdge )
			{
				va.x = 0.0f;
				vb.x = width;
			}

			float dx = width / (float)widthsegs;
			float dy = height / (float)heightsegs;
			float dz = length / (float)lengthsegs;

			Vector3 p = va;

			// Lists should be static, clear out to reuse
			List<Vector3>	verts = new List<Vector3>();
			List<Vector2>	uvs = new List<Vector2>();
			List<Vector2>	uv2s = new List<Vector2>();
			List<int>		tris = new List<int>();
			List<int>		tris1 = new List<int>();
			List<int>		tris2 = new List<int>();

			Vector2 uv = Vector2.zero;

			float edgeuvoff = page.pnum * edgeUVOff;

			// Do we have top and bottom
			if ( width > 0.0f && length > 0.0f )
			{
				p.y = vb.y;
				for ( int iz = 0; iz <= lengthsegs; iz++ )
				{
					p.x = va.x;
					for ( int ix = 0; ix <= widthsegs; ix++ )
					{
						float alpha = (float)ix / (float)widthsegs;
						p.x = va.x + (pagesectioncrv.Evaluate(alpha) * width);

						verts.Add(p);

						uv.x = p.x / width;
						uv.y = (p.z + vb.z) / length;

						uvs.Add(uv);
						p.x += dx;
					}
					p.z += dz;
				}

				for ( int iz = 0; iz < lengthsegs; iz++ )
				{
					int kv = iz * (widthsegs + 1);
					for ( int ix = 0; ix < widthsegs; ix++ )
					{
						MakeQuad1(tris, kv, kv + widthsegs + 1, kv + widthsegs + 2, kv + 1);
						kv++;
					}
				}

				int index = verts.Count;

				p.y = va.y;
				p.z = va.z;

				for ( int iy = 0; iy <= lengthsegs; iy++ )
				{
					p.x = va.x;
					for ( int ix = 0; ix <= widthsegs; ix++ )
					{
						float alpha = (float)ix / (float)widthsegs;
						p.x = va.x + (pagesectioncrv.Evaluate(alpha) * width);

						verts.Add(p);
						uv.x = 1.0f - (p.x / width);
						uv.y = ((p.z + vb.z) / length);

						uvs.Add(uv);
						p.x += dx;
					}
					p.z += dz;
				}

				for ( int iy = 0; iy < lengthsegs; iy++ )
				{
					int kv = iy * (widthsegs + 1) + index;
					for ( int ix = 0; ix < widthsegs; ix++ )
					{
						MakeQuad1(tris1, kv, kv + 1, kv + widthsegs + 2, kv + widthsegs + 1);
						kv++;
					}
				}
			}

			// Front back
			if ( width > 0.0f && height > 0.0f )
			{
				int index = verts.Count;

				p.z = va.z;
				p.y = va.y;
				for ( int iz = 0; iz <= heightsegs; iz++ )
				{
					p.x = va.x;
					for ( int ix = 0; ix <= widthsegs; ix++ )
					{
						float alpha = (float)ix / (float)widthsegs;
						p.x = va.x + (pagesectioncrv.Evaluate(alpha) * width);

						verts.Add(p);
						uv.x = (p.x + vb.x) / width;
						uv.y = edgeuvoff + (((p.y + vb.y) / height) * edgeUVSize);
						uvs.Add(uv);
						p.x += dx;
					}
					p.y += dy;
				}

				for ( int iz = 0; iz < heightsegs; iz++ )
				{
					int kv = iz * (widthsegs + 1) + index;
					for ( int ix = 0; ix < widthsegs; ix++ )
					{
						MakeQuad1(tris2, kv, kv + widthsegs + 1, kv + widthsegs + 2, kv + 1);
						kv++;
					}
				}

				index = verts.Count;

				p.z = vb.z;
				p.y = va.y;
				for ( int iy = 0; iy <= heightsegs; iy++ )
				{
					p.x = va.x;
					for ( int ix = 0; ix <= widthsegs; ix++ )
					{
						float alpha = (float)ix / (float)widthsegs;
						p.x = va.x + (pagesectioncrv.Evaluate(alpha) * width);

						verts.Add(p);
						uv.x = (p.x + vb.x) / width;
						//uv.y = (p.y + vb.y) / height;
						uv.y = edgeuvoff + (((p.y + vb.y) / height) * edgeUVSize);

						uvs.Add(uv);
						p.x += dx;
					}
					p.y += dy;
				}

				for ( int iy = 0; iy < heightsegs; iy++ )
				{
					int kv = iy * (widthsegs + 1) + index;
					for ( int ix = 0; ix < widthsegs; ix++ )
					{
						MakeQuad1(tris2, kv, kv + 1, kv + widthsegs + 2, kv + widthsegs + 1);
						kv++;
					}
				}
			}

			// Left Right
			if ( length > 0.0f && height > 0.0f )
			{
				int index = verts.Count;

				p.x = vb.x;
				p.y = va.y;
				for ( int iz = 0; iz <= heightsegs; iz++ )
				{
					p.z = va.z;
					for ( int ix = 0; ix <= lengthsegs; ix++ )
					{
						verts.Add(p);
						uv.x = (p.z + vb.z) / length;
						//uv.y = (p.y + vb.y) / height;
						uv.y = edgeuvoff + (((p.y + vb.y) / height) * edgeUVSize);

						uvs.Add(uv);
						p.z += dz;
					}
					p.y += dy;
				}

				for ( int iz = 0; iz < heightsegs; iz++ )
				{
					int kv = iz * (lengthsegs + 1) + index;
					for ( int ix = 0; ix < lengthsegs; ix++ )
					{
						MakeQuad1(tris2, kv, kv + lengthsegs + 1, kv + lengthsegs + 2, kv + 1);
						kv++;
					}
				}

				if ( spineedge )
				{
					index = verts.Count;

					p.x = va.x;
					p.y = va.y;
					for ( int iy = 0; iy <= heightsegs; iy++ )
					{
						p.z = va.z;
						for ( int ix = 0; ix <= lengthsegs; ix++ )
						{
							verts.Add(p);
							uv.x = (p.z + vb.z) / length;
							//uv.y = (p.y + vb.y) / height;
							uv.y = edgeuvoff + (((p.y + vb.y) / height) * edgeUVSize);

							uvs.Add(uv);

							p.z += dz;
						}
						p.y += dy;
					}

					for ( int iy = 0; iy < heightsegs; iy++ )
					{
						int kv = iy * (lengthsegs + 1) + index;
						for ( int ix = 0; ix < lengthsegs; ix++ )
						{
							MakeQuad1(tris2, kv, kv + 1, kv + lengthsegs + 2, kv + lengthsegs + 1);
							kv++;
						}
					}
				}
			}

			if ( useuv2 )
			{
				for ( int i = 0; i < uvs.Count; i++ )
					uv2s.Add(Vector2.zero);
			}

			int ti = verts.Count;
			int tib = verts.Count;

			List<int[]> dtris = new List<int[]>();

			// Set colors
			List<Color>	cols = new List<Color>();

			if ( usecols )
			{
				for ( int i = 0; i < verts.Count; i++ )
					cols.Add(color);
			}

			MegaBookPageParams pp = pageparams[page.pnum];

			Vector2[] textuv2s = null;

			// new mesh content
			//if ( dynamobj && dynammeshenabled )
			{
				if ( pp.meshFront && pp.meshFrontRoot )
				{
					Vector3 losscl = transform.lossyScale;

					Vector3 lscl = Vector3.Scale(losscl, pp.meshFrontScale);
					Matrix4x4 tm = Matrix4x4.TRS(Vector3.Scale(pp.meshFrontOffset, losscl), Quaternion.Euler(pp.meshFrontRot), lscl);
					BuildMesh(pp, true);    //, ref textverts, ref texttris);

					Vector2[] textuvs = GetUVs();	//page.pnum, true);

					Vector3[] textverts = GetVertices();	//page.pnum, true);
					for ( int i = 0; i < textverts.Length; i++ )
						verts.Add(tm.MultiplyPoint3x4(textverts[i]));

					for ( int i = 0; i < textuvs.Length; i++ )
						uvs.Add(textuvs[i]);

					if ( useuv2 )
					{
						textuv2s = GetUV2s();	//page.pnum, true);
						for ( int i = 0; i < textuv2s.Length; i++ )
							uv2s.Add(textuv2s[i]);
					}

					if ( usecols )
					{
						Color[] colverts = GetColors();	//page.pnum, true);

						if ( colverts == null || colverts.Length == 0 )
						{
							for ( int i = 0; i < textverts.Length; i++ )
								cols.Add(color);
						}
						else
						{
							for ( int i = 0; i < colverts.Length; i++ )
								cols.Add(colverts[i]);
						}
					}

					for ( int m = 0; m < GetNumMaterialsFront(pp); m++ )
						dtris.Add(GetTris(m));
				}

				if ( pp.meshBack && pp.meshBackRoot )
				{
					Vector3 losscl = transform.lossyScale;

					tib = verts.Count;
					Vector3 lscl = Vector3.Scale(losscl, pp.meshBackScale);
					Matrix4x4 tm = Matrix4x4.TRS(Vector3.Scale(losscl, pp.meshBackOffset), Quaternion.Euler(pp.meshBackRot), lscl);
					BuildMesh(pp, false);

					Vector2[] textuvs = GetUVs();
					Vector3[] textverts = GetVertices();
					for ( int i = 0; i < textverts.Length; i++ )
						verts.Add(tm.MultiplyPoint3x4(textverts[i]));

					for ( int i = 0; i < textuvs.Length; i++ )
						uvs.Add(textuvs[i]);

					if ( useuv2 )
					{
						textuv2s = GetUV2s();
						for ( int i = 0; i < textuv2s.Length; i++ )
							uv2s.Add(textuv2s[i]);
					}

					if ( usecols )
					{
						Color[] colverts = GetColors();

						if ( colverts == null || colverts.Length == 0 )
						{
							for ( int i = 0; i < textverts.Length; i++ )
								cols.Add(color);
						}
						else
						{
							for ( int i = 0; i < colverts.Length; i++ )
								cols.Add(colverts[i]);
						}
					}

					for ( int m = 0; m < GetNumMaterialsBack(pp); m++ )
						dtris.Add(GetTris(m));
				}
			}

			AddNoise(verts, page, width, length);
			page.verts = verts.ToArray();
			page.sverts = verts.ToArray();  //new Vector3[page.verts.Length];

			mesh.Clear();
			mesh.MarkDynamic();

			mesh.subMeshCount = 3 + GetNumMaterialsFront(pp) + GetNumMaterialsBack(pp);

			//mesh.vertices = page.verts;	//verts.ToArray();
			mesh.SetVertices(page.verts);
			mesh.uv = uvs.ToArray();

			if ( useuv2 )
				mesh.uv2 = uv2s.ToArray();

			if ( usecols )
				mesh.colors = cols.ToArray();

			mesh.SetTriangles(tris.ToArray(), 0);
			mesh.SetTriangles(tris1.ToArray(), 1);
			mesh.SetTriangles(tris2.ToArray(), 2);

			int mo = GetNumMaterialsFront(pp);
			int ixd = 0;

			for ( int m = 0; m < mo; m++ )
			{
				int[] texttris = dtris[ixd++];   //dynamobj.GetTris(page.pnum, true, m);

				for ( int i = 0; i < texttris.Length; i++ )
					texttris[i] += ti;

				mesh.SetTriangles(texttris, 3 + m);
			}

			int mo1 = GetNumMaterialsBack(pp);

			for ( int m = 0; m < mo1; m++ )
			{
				//int[] texttris = dynamobj.GetTris(page.pnum, false, m);
				int[] texttris = dtris[ixd++];   //dynamobj.GetTris(page.pnum, true, m);

				for ( int i = 0; i < texttris.Length; i++ )
					texttris[i] += tib;
				mesh.SetTriangles(texttris, 3 + m + mo);
			}

			mesh.RecalculateNormals();

			BuildTangents(mesh, page.verts, mesh.normals, mesh.triangles, mesh.uv);

			mesh.RecalculateBounds();

			page.bbox = mesh.bounds;
			page.mesh = mesh;
			return mesh;
		}

		// TODO: curve for open vert def

		void AddNoise(List<Vector3> verts, MegaBookPage page, float width, float length)
		{
			if ( addNoise && noiseApply )
			{
				float phase = noisePhase;
				float phasey = noisePhaseVert;

				if ( !noiseSame )
					phase *= (float)page.pnum;

				if ( !noiseSameVert )
					phasey *= (float)page.pnum;

				float namt = 1.0f;

				if ( useNoiseBook )
					namt = noiseBook.Evaluate((float)page.pnum / (float)NumPages);

				for ( int i = 0; i < verts.Count; i++ )
				{
					Vector3 vp = verts[i];

					float alpha = vp.x / width;

					if ( vp.z < (-length * 0.48f) || vp.z > (length * 0.48f) )
					{
						if ( noiseStrength.z != 0.0f )
							vp.z += iperlin.Noise(vp.x * noiseScale + 0.5f, vp.y * noiseScale + 0.5f, phase) * noiseStrength.z * noiseCurveX.Evaluate(alpha) * namt;
					}

					if ( noiseStrength.y != 0.0f )
						vp.y += (iperlin.Noise(vp.x * noiseScaleVert + 0.5f, vp.z * noiseScaleVert + 0.5f, phasey) * noiseStrength.y) * noiseCurveY.Evaluate(alpha) * namt;    //vp.x / width);

					if ( vp.x > width * 0.98f )
					{
						if ( noiseStrength.x != 0.0f )
							vp.x += iperlin.Noise(vp.y * noiseScale + 0.5f, vp.z * noiseScale + 0.5f, phase) * noiseStrength.x * namt;    // * noiseCurveX.Evaluate(alpha);
					}

					verts[i] = vp;
				}
			}
		}

		Mesh CreatePageMeshHole(MegaBookPage page, float width, float length, float height, int widthsegs, int lengthsegs, int heightsegs, int xhole, int yhole)
		{
			MegaBookPageParams pp = pageparams[page.pnum];

			Mesh mesh = new Mesh();

			Vector3 vb = new Vector3(width, height, length) / 2.0f;
			Vector3 va = -vb;

			if ( PivotBase )
			{
				va.y = 0.0f;
				vb.y = height;
			}

			if ( PivotEdge )
			{
				va.x = 0.0f;
				vb.x = width;
			}

			float dx = width / (float)widthsegs;
			float dy = height / (float)heightsegs;
			float dz = length / (float)lengthsegs;

			Vector3 p = va;

			// Lists should be static, clear out to reuse
			List<Vector3>	verts = new List<Vector3>();
			List<Vector2>	uvs = new List<Vector2>();
			List<int>		tris = new List<int>();
			List<int>		tris1 = new List<int>();
			List<int>		tris2 = new List<int>();
			List<Color>		cols = new List<Color>();

			Vector2 uv = Vector2.zero;
			float edgeuvoff = page.pnum * edgeUVOff;

			// Do we have top and bottom
			if ( width > 0.0f && length > 0.0f )
			{
				p.y = vb.y;

				int[,] grid = new int[lengthsegs + 1, widthsegs + 1];

				for ( int iz = 0; iz <= lengthsegs; iz++ )
				{
					p.x = va.x;
					for ( int ix = 0; ix <= widthsegs; ix++ )
					{
						if ( ix <= xhole || ix >= widthsegs - xhole || iz <= yhole || iz >= lengthsegs - yhole )
						{
							grid[iz, ix] = verts.Count;

							float alpha = (float)ix / (float)widthsegs;
							p.x = va.x + (pagesectioncrv.Evaluate(alpha) * width);

							verts.Add(p);

							uv.x = p.x / width;
							uv.y = (p.z + vb.z) / length;

							uvs.Add(uv);
						}
						p.x += dx;
					}
					p.z += dz;
				}

				int kv = 0;
				for ( int iz = 0; iz < lengthsegs; iz++ )
				{
					for ( int ix = 0; ix < widthsegs; ix++ )
					{
						if ( ix < xhole || ix >= widthsegs - xhole || iz < yhole || iz >= lengthsegs - yhole )
						{
							kv = grid[iz, ix];
							MakeQuad1(tris, kv, grid[iz + 1, ix], grid[iz + 1, ix + 1], kv + 1);
						}
					}
				}

				p.y = va.y;
				p.z = va.z;

				for ( int iy = 0; iy <= lengthsegs; iy++ )
				{
					p.x = va.x;
					for ( int ix = 0; ix <= widthsegs; ix++ )
					{
						if ( ix <= xhole || ix >= widthsegs - xhole || iy <= yhole || iy >= lengthsegs - yhole )
						{
							grid[iy, ix] = verts.Count;

							float alpha = (float)ix / (float)widthsegs;
							p.x = va.x + (pagesectioncrv.Evaluate(alpha) * width);

							verts.Add(p);
							uv.x = 1.0f - (p.x / width);
							//uv.x = p.x / width;
							uv.y = ((p.z + vb.z) / length);

							uvs.Add(uv);
						}
						p.x += dx;
					}
					p.z += dz;
				}

				for ( int iy = 0; iy < lengthsegs; iy++ )
				{
					for ( int ix = 0; ix < widthsegs; ix++ )
					{
						if ( ix < xhole || ix >= widthsegs - xhole || iy < yhole || iy >= lengthsegs - yhole )
						{
							kv = grid[iy, ix];
							MakeQuad1(tris1, kv, kv + 1, grid[iy + 1, ix + 1], grid[iy + 1, ix]);
						}
					}
				}
			}

			// Front back
			if ( width > 0.0f && height > 0.0f )
			{
				int index = verts.Count;

				p.z = va.z;
				p.y = va.y;
				for ( int iz = 0; iz <= heightsegs; iz++ )
				{
					p.x = va.x;
					for ( int ix = 0; ix <= widthsegs; ix++ )
					{
						float alpha = (float)ix / (float)widthsegs;
						p.x = va.x + (pagesectioncrv.Evaluate(alpha) * width);

						verts.Add(p);
						uv.x = (p.x + vb.x) / width;
						//uv.y = (p.y + vb.y) / height;
						uv.y = edgeuvoff + (((p.y + vb.y) / height) * edgeUVSize);

						uvs.Add(uv);
						p.x += dx;
					}
					p.y += dy;
				}

				for ( int iz = 0; iz < heightsegs; iz++ )
				{
					int kv = iz * (widthsegs + 1) + index;
					for ( int ix = 0; ix < widthsegs; ix++ )
					{
						MakeQuad1(tris2, kv, kv + widthsegs + 1, kv + widthsegs + 2, kv + 1);
						kv++;
					}
				}

				index = verts.Count;

				p.z = vb.z;
				p.y = va.y;
				for ( int iy = 0; iy <= heightsegs; iy++ )
				{
					p.x = va.x;
					for ( int ix = 0; ix <= widthsegs; ix++ )
					{
						float alpha = (float)ix / (float)widthsegs;
						p.x = va.x + (pagesectioncrv.Evaluate(alpha) * width);

						verts.Add(p);
						uv.x = (p.x + vb.x) / width;
						//uv.y = (p.y + vb.y) / height;
						uv.y = edgeuvoff + (((p.y + vb.y) / height) * edgeUVSize);

						uvs.Add(uv);
						p.x += dx;
					}
					p.y += dy;
				}

				for ( int iy = 0; iy < heightsegs; iy++ )
				{
					int kv = iy * (widthsegs + 1) + index;
					for ( int ix = 0; ix < widthsegs; ix++ )
					{
						MakeQuad1(tris2, kv, kv + 1, kv + widthsegs + 2, kv + widthsegs + 1);
						kv++;
					}
				}
			}

			// Left Right
			if ( length > 0.0f && height > 0.0f )
			{
				int index = verts.Count;

				p.x = vb.x;
				p.y = va.y;
				for ( int iz = 0; iz <= heightsegs; iz++ )
				{
					p.z = va.z;
					for ( int ix = 0; ix <= lengthsegs; ix++ )
					{
						verts.Add(p);
						uv.x = (p.z + vb.z) / length;
						//uv.y = (p.y + vb.y) / height;
						uv.y = edgeuvoff + (((p.y + vb.y) / height) * edgeUVSize);

						uvs.Add(uv);
						p.z += dz;
					}
					p.y += dy;
				}

				for ( int iz = 0; iz < heightsegs; iz++ )
				{
					int kv = iz * (lengthsegs + 1) + index;
					for ( int ix = 0; ix < lengthsegs; ix++ )
					{
						MakeQuad1(tris2, kv, kv + lengthsegs + 1, kv + lengthsegs + 2, kv + 1);
						kv++;
					}
				}

				if ( spineedge )
				{
					index = verts.Count;

					p.x = va.x;
					p.y = va.y;
					for ( int iy = 0; iy <= heightsegs; iy++ )
					{
						p.z = va.z;
						for ( int ix = 0; ix <= lengthsegs; ix++ )
						{
							verts.Add(p);
							uv.x = (p.z + vb.z) / length;
							//uv.y = (p.y + vb.y) / height;
							uv.y = edgeuvoff + (((p.y + vb.y) / height) * edgeUVSize);

							uvs.Add(uv);

							p.z += dz;
						}
						p.y += dy;
					}

					for ( int iy = 0; iy < heightsegs; iy++ )
					{
						int kv = iy * (lengthsegs + 1) + index;
						for ( int ix = 0; ix < lengthsegs; ix++ )
						{
							MakeQuad1(tris2, kv, kv + 1, kv + lengthsegs + 2, kv + lengthsegs + 1);
							kv++;
						}
					}
				}
			}

			if ( usecols )
			{
				for ( int i = 0; i < verts.Count; i++ )
					cols.Add(color);
			}

			AddNoise(verts, page, width, length);

			page.verts1 = verts.ToArray();
			page.sverts1 = new Vector3[page.verts1.Length];

			int fnum = GetNumMaterialsFront(pp) + GetNumMaterialsBack(pp);

			mesh.Clear();
			mesh.subMeshCount = 3 + fnum;
			mesh.SetVertices(page.verts1);
			mesh.uv = uvs.ToArray();
			if ( usecols )
				mesh.colors = cols.ToArray();

			mesh.SetTriangles(tris.ToArray(), 0);
			mesh.SetTriangles(tris1.ToArray(), 1);
			mesh.SetTriangles(tris2.ToArray(), 2);

			mesh.RecalculateNormals();

			BuildTangents(mesh, page.verts1, mesh.normals, mesh.triangles, mesh.uv);

			mesh.RecalculateBounds();

			page.bbox = mesh.bounds;
			page.holemesh = mesh;
			return mesh;
		}

		void BuildTangents(Mesh mesh, NativeArray<Vector3> verts, Vector3[] norms, int[] tris, Vector2[] uvs)
		{
			int vertexCount = mesh.vertices.Length;

			Vector3[] tan1 = new Vector3[vertexCount];
			Vector3[] tan2 = new Vector3[vertexCount];
			Vector4[] tangents = new Vector4[vertexCount];

			for ( int a = 0; a < tris.Length; a += 3 )
			{
				int i1 = tris[a];
				int i2 = tris[a + 1];
				int i3 = tris[a + 2];

				Vector3 v1 = verts[i1];
				Vector3 v2 = verts[i2];
				Vector3 v3 = verts[i3];

				Vector2 w1 = uvs[i1];
				Vector2 w2 = uvs[i2];
				Vector2 w3 = uvs[i3];

				float x1 = v2.x - v1.x;
				float x2 = v3.x - v1.x;
				float y1 = v2.y - v1.y;
				float y2 = v3.y - v1.y;
				float z1 = v2.z - v1.z;
				float z2 = v3.z - v1.z;

				float s1 = w2.x - w1.x;
				float s2 = w3.x - w1.x;
				float t1 = w2.y - w1.y;
				float t2 = w3.y - w1.y;

				float r = 1.0f / (s1 * t2 - s2 * t1);

				Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
				Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

				tan1[i1] += sdir;
				tan1[i2] += sdir;
				tan1[i3] += sdir;

				tan2[i1] += tdir;
				tan2[i2] += tdir;
				tan2[i3] += tdir;
			}

			for ( int a = 0; a < vertexCount; a++ )
			{
				Vector3 n = norms[a];
				Vector3 t = tan1[a];

				Vector3.OrthoNormalize(ref n, ref t);
				tangents[a].x = t.x;
				tangents[a].y = t.y;
				tangents[a].z = t.z;
				tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
			}

			mesh.tangents = tangents;
		}

		void BuildTangents(Mesh mesh, Vector3[] verts, Vector3[] norms, int[] tris, Vector2[] uvs)
		{
			int vertexCount = mesh.vertices.Length;

			Vector3[] tan1 = new Vector3[vertexCount];
			Vector3[] tan2 = new Vector3[vertexCount];
			Vector4[] tangents = new Vector4[vertexCount];

			for ( int a = 0; a < tris.Length; a += 3 )
			{
				int i1 = tris[a];
				int i2 = tris[a + 1];
				int i3 = tris[a + 2];

				Vector3 v1 = verts[i1];
				Vector3 v2 = verts[i2];
				Vector3 v3 = verts[i3];

				Vector2 w1 = uvs[i1];
				Vector2 w2 = uvs[i2];
				Vector2 w3 = uvs[i3];

				float x1 = v2.x - v1.x;
				float x2 = v3.x - v1.x;
				float y1 = v2.y - v1.y;
				float y2 = v3.y - v1.y;
				float z1 = v2.z - v1.z;
				float z2 = v3.z - v1.z;

				float s1 = w2.x - w1.x;
				float s2 = w3.x - w1.x;
				float t1 = w2.y - w1.y;
				float t2 = w3.y - w1.y;

				float r = 1.0f / (s1 * t2 - s2 * t1);

				Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
				Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

				tan1[i1] += sdir;
				tan1[i2] += sdir;
				tan1[i3] += sdir;

				tan2[i1] += tdir;
				tan2[i2] += tdir;
				tan2[i3] += tdir;
			}

			for ( int a = 0; a < vertexCount; a++ )
			{
				Vector3 n = norms[a];
				Vector3 t = tan1[a];

				Vector3.OrthoNormalize(ref n, ref t);
				tangents[a].x = t.x;
				tangents[a].y = t.y;
				tangents[a].z = t.z;
				tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
			}

			mesh.tangents = tangents;
		}

		Mesh CopyMesh(MegaBookPage page, Mesh srcmesh, float width, float length, float height, Vector3 rot, Vector3 pivot, float scale = 1.0f)
		{
			List<Vector3> verts = new List<Vector3>();
			List<Vector3> norms = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			List<Vector2> uvs2 = new List<Vector2>();
			List<int> tris = new List<int>();
			List<int> tris1 = new List<int>();
			List<int> tris2 = new List<int>();
			List<Color> cols = new List<Color>();


			Mesh mesh = new Mesh();

			srcmesh.GetVertices(verts);
			srcmesh.GetUVs(0, uvs);
			srcmesh.GetUVs(1, uvs2);
			if ( uvs2.Count == 0 )
			{
				for ( int i = 0; i < uvs.Count; i++ )
					uvs2.Add(Vector2.zero);
			}
			srcmesh.GetTriangles(tris, 0);
			srcmesh.GetTriangles(tris1, 1);
			srcmesh.GetTriangles(tris2, 2);
			srcmesh.GetNormals(norms);
			srcmesh.GetColors(cols);
			if ( cols.Count == 0 )
			{
				for ( int i = 0; i < verts.Count; i++ )
					cols.Add(Color.white);
			}

			Vector3 scl = srcmesh.bounds.size;
			scl.x = 1.0f / scl.x;
			scl.y = 1.0f / scl.y;
			scl.z = 1.0f / scl.z;

			scl.x *= width * scale;
			scl.y *= length;
			scl.z *= height;

			Matrix4x4 tm = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(rot), scl);
			//Matrix4x4 tm = Matrix4x4.TRS(pivot, Quaternion.Euler(rot), scl);

			Vector3[] v = srcmesh.vertices;
			Vector3 min = Vector3.zero;
			Vector3 max = Vector3.zero;

			for ( int i = 0; i < verts.Count; i++ )
			{
				verts[i] = tm.MultiplyPoint3x4(verts[i]);

				if ( i == 0 )
				{
					min = verts[i];
					max = verts[i];
				}

				if ( verts[i].x < min.x ) min.x = verts[i].x;
				if ( verts[i].y < min.y ) min.y = verts[i].y;
				if ( verts[i].z < min.z ) min.z = verts[i].z;

				if ( verts[i].x > max.x ) max.x = verts[i].x;
				if ( verts[i].y > max.y ) max.y = verts[i].y;
				if ( verts[i].z > max.z ) max.z = verts[i].z;
			}

			float y = (max.y - min.y) * 0.5f;
			for ( int i = 0; i < verts.Count; i++ )
			{
				Vector3 p = verts[i];
				p.x -= min.x;
				p.y -= y;

				p.x -= pivot.x * (max.x - min.x);
				p.y -= pivot.y * (max.y - min.y);
				p.z -= pivot.z * (max.z - min.z);

				verts[i] = p;
			}

#if true
			// mesh content bit
			int ti = verts.Count;
			int tib = verts.Count;

			List<int[]> dtris = new List<int[]>();

			MegaBookPageParams pp = pageparams[page.pnum];

			Vector2[] textuv2s = null;

			// new mesh content
			//if ( dynamobj && dynammeshenabled )
			{
				if ( pp.meshFront && pp.meshFrontRoot )
				{
					Vector3 losscl = transform.lossyScale;

					Vector3 lscl = Vector3.Scale(losscl, pp.meshFrontScale);
					tm = Matrix4x4.TRS(Vector3.Scale(pp.meshFrontOffset, losscl), Quaternion.Euler(pp.meshFrontRot), lscl);
					BuildMesh(pp, true);    //, ref textverts, ref texttris);

					Vector2[] textuvs = GetUVs();

					Vector3[] textverts = GetVertices();	//page.pnum, true);
					for ( int i = 0; i < textverts.Length; i++ )
						verts.Add(tm.MultiplyPoint3x4(textverts[i]));

					for ( int i = 0; i < textuvs.Length; i++ )
						uvs.Add(textuvs[i]);

					if ( useuv2 )
					{
						textuv2s = GetUV2s();   //page.pnum, true);
						for ( int i = 0; i < textuv2s.Length; i++ )
							uvs2.Add(textuv2s[i]);
					}

					if ( usecols )
					{
						Color[] colverts = GetColors();	//page.pnum, true);

						if ( colverts == null || colverts.Length == 0 )
						{
							for ( int i = 0; i < textverts.Length; i++ )
								cols.Add(color);
						}
						else
						{
							for ( int i = 0; i < colverts.Length; i++ )
								cols.Add(colverts[i]);
						}
					}

					for ( int m = 0; m < GetNumMaterialsFront(pp); m++ )
						dtris.Add(GetTris(m));
				}

				if ( pp.meshBack && pp.meshBackRoot )
				{
					Vector3 losscl = transform.lossyScale;

					tib = verts.Count;
					Vector3 lscl = Vector3.Scale(losscl, pp.meshBackScale);
					tm = Matrix4x4.TRS(Vector3.Scale(losscl, pp.meshBackOffset), Quaternion.Euler(pp.meshBackRot), lscl);
					BuildMesh(pp, false);

					Vector2[] textuvs = GetUVs();
					Vector3[] textverts = GetVertices();
					for ( int i = 0; i < textverts.Length; i++ )
						verts.Add(tm.MultiplyPoint3x4(textverts[i]));

					for ( int i = 0; i < textuvs.Length; i++ )
						uvs.Add(textuvs[i]);

					if ( useuv2 )
					{
						textuv2s = GetUV2s();
						for ( int i = 0; i < textuv2s.Length; i++ )
							uvs2.Add(textuv2s[i]);
					}

					if ( usecols )
					{
						Color[] colverts = GetColors();

						if ( colverts == null || colverts.Length == 0 )
						{
							for ( int i = 0; i < textverts.Length; i++ )
								cols.Add(color);
						}
						else
						{
							for ( int i = 0; i < colverts.Length; i++ )
								cols.Add(colverts[i]);
						}
					}

					for ( int m = 0; m < GetNumMaterialsBack(pp); m++ )
						dtris.Add(GetTris(m));
				}
			}
#endif
			AddNoise(verts, page, width, length);

			mesh.subMeshCount = 3 + GetNumMaterialsFront(pp) + GetNumMaterialsBack(pp);

			page.verts = verts.ToArray(); //new NativeArray<Vector3>(v, Allocator.Persistent);
			page.sverts = new Vector3[page.verts.Length];   //, Allocator.Persistent);

			mesh.SetVertices(page.verts);
			mesh.uv = uvs.ToArray();

			if ( useuv2 )
				mesh.uv2 = uvs2.ToArray();

			if ( usecols )
				mesh.colors = cols.ToArray();

			mesh.SetTriangles(tris.ToArray(), 0);
			mesh.SetTriangles(tris1.ToArray(), 1);
			mesh.SetTriangles(tris2.ToArray(), 2);

			int mo = GetNumMaterialsFront(pp);
			int ixd = 0;

			for ( int m = 0; m < mo; m++ )
			{
				int[] texttris = dtris[ixd++];   //dynamobj.GetTris(page.pnum, true, m);

				for ( int i = 0; i < texttris.Length; i++ )
					texttris[i] += ti;

				mesh.SetTriangles(texttris, 3 + m);
			}

			int mo1 = GetNumMaterialsBack(pp);

			for ( int m = 0; m < mo1; m++ )
			{
				int[] texttris = dtris[ixd++];   //dynamobj.GetTris(page.pnum, true, m);

				for ( int i = 0; i < texttris.Length; i++ )
					texttris[i] += tib;
				mesh.SetTriangles(texttris, 3 + m + mo);
			}

			mesh.RecalculateNormals();

			BuildTangents(mesh, page.verts, mesh.normals, mesh.triangles, mesh.uv);

			mesh.RecalculateBounds();

			page.bbox = mesh.bounds;
			page.mesh = mesh;
			return mesh;
		}

		Mesh CopyMeshHole(MegaBookPage page, Mesh srcmesh, float width, float length, float height, Vector3 rot, Vector3 pivot, float scale = 1.0f)
		{
			MegaBookPageParams pp = pageparams[page.pnum];

			Mesh mesh = new Mesh();

			Vector3 scl = srcmesh.bounds.size;
			scl.x = 1.0f / scl.x;
			scl.y = 1.0f / scl.y;
			scl.z = 1.0f / scl.z;

			scl.x *= width;
			scl.y *= length;
			scl.z *= height;

			Matrix4x4 tm = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(rot), scl);

			Vector3[] v = srcmesh.vertices;
			Vector3 min = Vector3.zero;
			Vector3 max = Vector3.zero;

			for ( int i = 0; i < v.Length; i++ )
			{
				v[i] = tm.MultiplyPoint3x4(v[i]);

				if ( i == 0 )
				{
					min = v[i];
					max = v[i];
				}

				if ( v[i].x < min.x ) min.x = v[i].x;
				if ( v[i].y < min.y ) min.y = v[i].y;
				if ( v[i].z < min.z ) min.z = v[i].z;

				if ( v[i].x > max.x ) max.x = v[i].x;
				if ( v[i].y > max.y ) max.y = v[i].y;
				if ( v[i].z > max.z ) max.z = v[i].z;
			}

			page.verts1 = v;
			page.sverts1 = new Vector3[page.verts.Length];

			int fnum = GetNumMaterialsFront(pp) + GetNumMaterialsBack(pp);

			mesh.subMeshCount = srcmesh.subMeshCount + fnum;
			float y = (max.y - min.y) * 0.5f;
			for ( int i = 0; i < v.Length; i++ )
			{
				Vector3 p = v[i];
				p.x -= min.x;
				p.y -= y;

				p.x -= pivot.x * (max.x - min.x);
				p.y -= pivot.y * (max.y - min.y);
				p.z -= pivot.z * (max.z - min.z);

				v[i] = p;
			}

			mesh.vertices = v;
			mesh.uv = srcmesh.uv;
			mesh.uv2 = srcmesh.uv2;
			mesh.normals = srcmesh.normals;
			mesh.colors = srcmesh.colors;
			mesh.tangents = srcmesh.tangents;

			for ( int i = 0; i < srcmesh.subMeshCount; i++ )
				mesh.SetTriangles(srcmesh.GetTriangles(i), i);

			mesh.RecalculateBounds();
			page.bbox = mesh.bounds;
			page.holemesh = mesh;
			return mesh;
		}

		List<MegaBookPageAttach>	attached = new List<MegaBookPageAttach>();

		public void Detach()
		{
#if UNITY_2023_1_OR_NEWER
			MegaBookPageAttach[] attach = (MegaBookPageAttach[])FindObjectsByType(typeof(MegaBookPageAttach), FindObjectsSortMode.None);
#else
			MegaBookPageAttach[] attach = (MegaBookPageAttach[])FindObjectsOfType(typeof(MegaBookPageAttach));
#endif
			attached.Clear();
			for ( int i = 0; i < attach.Length; i++ )
			{
				if ( attach[i].book == this && attach[i].attached )
				{
					attached.Add(attach[i]);
					attach[i].DetachIt();
				}
			}
		}

		public void Attach()
		{
			for ( int i = 0; i < attached.Count; i++ )
				attached[i].AttachItUV();
		}

		void RebuildMeshes()
		{
			UpdateSettings();
			Detach();
			BuildPageMeshes();
			Attach();
			UpdateSettings();
		}

		void RebuildPrefab()
		{
			UpdateSettings();
			Detach();
			//BuildPageMeshes();
			BuildPages();
			Attach();
			UpdateSettings();
		}

		void DoBindings()
		{
			if ( updateBindings )
			{
				updateBindings = false;

				if ( addHeadband )
				{
					if ( !headband1 )
					{
						GameObject sobj = new GameObject("Headband");
						sobj.transform.SetParent(transform);
						sobj.transform.localEulerAngles = Vector3.zero;
						headband1 = sobj.AddComponent<MegaBookHeadband>();
					}

					if ( !headband2 )
					{
						GameObject sobj = new GameObject("Headband");
						sobj.transform.SetParent(transform);
						sobj.transform.localEulerAngles = Vector3.zero;
						headband2 = sobj.AddComponent<MegaBookHeadband>();
					}
				}
				else
				{
					if ( headband1 )
					{
						if ( Application.isPlaying )
							Destroy(headband1.gameObject);
						else
							DestroyImmediate(headband1.gameObject);

						headband1 = null;
					}

					if ( headband2 )
					{
						if ( Application.isPlaying )
							Destroy(headband2.gameObject);
						else
							DestroyImmediate(headband2.gameObject);

						headband2 = null;
					}
				}

				if ( addFabric )
				{
					if ( !spineFabric )
					{
						GameObject sobj = new GameObject("Fabric");
						sobj.transform.SetParent(transform);
						sobj.transform.localEulerAngles = Vector3.zero;
						spineFabric = sobj.AddComponent<MegaBookSpineFabric>();
					}
				}
				else
				{
					if ( spineFabric )
					{
						if ( Application.isPlaying )
							Destroy(spineFabric.gameObject);
						else
							DestroyImmediate(spineFabric.gameObject);

						spineFabric = null;
					}
				}

				if ( headband1 )
				{
					headband1.length = headbandLength;
					headband1.radius = headbandRadius;
					headband1.segs = headbandSegs;
					headband1.sides = headbandSides;

					MeshRenderer mr = headband1.GetComponent<MeshRenderer>();
					if ( mr )
						mr.sharedMaterial = headbandMat;

					headband1.BuildMesh();
				}

				if ( headband2 )
				{
					headband2.length = headbandLength;
					headband2.radius = headbandRadius;
					headband2.segs = headbandSegs;
					headband2.sides = headbandSides;

					MeshRenderer mr = headband2.GetComponent<MeshRenderer>();
					if ( mr )
						mr.sharedMaterial = headbandMat;

					headband2.BuildMesh();
				}

				if ( spineFabric )
				{
					spineFabric.length = fabricLength;
					spineFabric.width = pageLength * fabricWidth;
					spineFabric.thickness = fabricThickness;
					spineFabric.segs = fabricSegs;

					MeshRenderer mr = spineFabric.GetComponent<MeshRenderer>();
					if ( mr )
						mr.sharedMaterial = fabricMat;

					spineFabric.BuildMesh();
				}

				//float palpha = Flip / (float)NumPages;
				//SpineUpdate(palpha);
			}

			// Position objects
			if ( headband1 )
			{
				Vector3 off = headbandOffset;
				off.z += pageLength * 0.5f * headbandWidth;
				headband1.transform.localPosition = off;
			}

			if ( headband2 )
			{
				Vector3 off = headbandOffset;
				off.z += -pageLength * 0.5f * headbandWidth;
				headband2.transform.localPosition = off;
			}

			if ( spineFabric )
				spineFabric.transform.localPosition = fabricOffset;
		}

		bool isPrefab = false;

		public bool autoDisable = true;
		float lastPage;

		void Update()
		{
			if ( transform.hasChanged )
			{
				transform.hasChanged = false;
				updateObjs = true;
			}

			isPrefab = false;
#if UNITY_EDITOR
#if UNITY_6000_0_OR_NEWER
			if ( GraphicsSettings.defaultRenderPipeline )
				shaderTexture = "_BaseMap";
			else
				shaderTexture = "_MainTex";
#else
			if ( GraphicsSettings.renderPipelineAsset )
				shaderTexture = "_BaseMap";
			else
				shaderTexture = "_MainTex";
#endif
			if ( shaderTextureName.Length > 0 )
				shaderTexture = shaderTextureName;

			if ( UnityEditor.PrefabUtility.IsPartOfAnyPrefab(gameObject) )
			{
				isPrefab = true;
			}

			//for ( int i = 0; i < pages.Count; i++ )
			//{
				//if ( pages[i].videoFront )
				//{
					//pages[i].videoFront.StepForward();
				//}

				//if ( pages[i].videoBack )
				//{
					//pages[i].videoBack.Play();
				//}
			//}
#endif
			// Check if the arrays are present
			for ( int i = 0; i < pages.Count; i++ )
			{
				if ( !pages[i].jsverts.IsCreated )
				{
					rebuildmeshes = true;
					break;
				}
			}

			if ( Snap )
				page = (int)page;

			if ( Flip != page )
			{
				if ( Application.isPlaying )
					Flip = Mathf.SmoothDamp(Flip, page, ref turnspd, turntime);
				else
					Flip = page;

				float delta = Mathf.Abs(Flip - page);

				if ( delta < 0.0001f )
				{
					Flip = page;
					turnspd = 0.0f;
				}

				pageTurnEvent.Invoke(this, delta);
			}

			bool doattach = false;
			if ( !isPrefab )
			{
				if ( pages.Count == 0 || pages[0] == null || pages[0].obj == null )
				{
					rebuild = true;
				}

				if ( rebuild )	//meshes )
				{
					rebuild = false;
					// Detach objects
					Detach();
					RemovePages();
					BuildPages();
					doattach = true;

					//if ( headband1 )
					//headband1.BuildMesh();

					//if ( headband2 )
					//headband2.BuildMesh();

					//if ( spineFabric )
					//spineFabric.BuildMesh();

					Flip = page - 0.0001f;
				}

				if ( rebuildmeshes )
				{
					rebuildmeshes = false;
					RebuildMeshes();
				}
			}

			if ( changespineangle )
			{
				float alpha = Flip / (float)NumPages;

				BottomAngle = alpha * -BottomMaxAngle;	//.0f;
				BottomAngle = Mathf.Clamp(BottomAngle, -BottomMaxAngle, 0.0f);
			}
			else
				BottomAngle = 0.0f;

			Vector3 rot = transform.localRotation.eulerAngles;
			rot.x = 0.0f;
			rot.y = 0.0f;
			rot.z = -BottomAngle;
			transform.localEulerAngles = rot;   // = Quaternion.Euler(rot);

			DoBindings();

#if false
			float palpha = Flip / (float)NumPages;

			frontCoverAngle = pages[0].turnerangcon.Evaluate(Flip * 14.0f) * Turn_maxAngleCrv.Evaluate(palpha);
			frontCoverAngle += Turn_minAngleCrv.Evaluate(Flip * 14.0f) * palpha;

			backCoverAngle = pages[NumPages - 1].turnerangcon.Evaluate((Flip * 14.0f) - ((float)(NumPages - 1) * shuffle)) * Turn_maxAngleCrv.Evaluate(palpha);
			backCoverAngle += Turn_minAngleCrv.Evaluate((Flip * 14.0f) - ((float)(NumPages - 1) * shuffle)) * palpha;
#endif
			float palpha = Flip / (float)NumPages;
			SpineUpdate(palpha);

			if ( bookCover )
			{
				bookCover.book = this;
				bookCover.UpdateBones(this);
			}
			else
			{
				if ( frontcover )
				{
					float ca = 0.0f;
					if ( Flip < 0.0f )
						ca = -Flip;

					float ang = Mathf.Lerp(frontang, 0.0f, ca);
					Vector3 frot = Vector3.zero;
					frot[(int)frontAxis] = ang;

					//Quaternion r = Quaternion.Euler(frot);

					frontcover.position = transform.TransformPoint(frontpivot + coverOffset);

					frontcover.localEulerAngles = frot;	//new Vector3(0.0f, 0.0f, ang);	// - BottomAngle);
				}

				if ( backcover )
				{
					float ca = 0.0f;
					if ( Flip > NumPages )
						ca = Flip - NumPages;

					float ang = Mathf.Lerp(0.0f, backang, ca);

					if ( !changespineangle )
						ang = 0.0f;

					// Dont set position, allow user to to that, then dont need coveroffset
					backcover.position = transform.TransformPoint(backpivot + coverOffset);
					Vector3 brot = Vector3.zero;
					brot[(int)backAxis] = ang;
					backcover.localEulerAngles = brot;	//new Vector3(0.0f, 0.0f, ang);
				}

				if ( spineBone )
				{
					Vector3 srot = Vector3.zero;
					spineBone.position = transform.TransformPoint(spinepivot + coverOffset);
					srot[(int)spineAxis] = rot.z;
					spineBone.localEulerAngles = srot;	//new Vector3(0.0f, 0.0f, ang);
				}
			}

			if ( Application.isPlaying )
			{
				bool pageChanged = false;
				if ( autoDisable )
				{
					if ( page != lastPage || rebuild || rebuildmeshes || Flip != page )
					{
						pageChanged = true;
					}
				}

				if ( !pageChanged )
				{
					if ( !updateObjs )
						return;
				}

				lastPage = page;
			}

			// Do page turning
			//float palpha = Flip / (float)NumPages;
			//SpineUpdate(palpha);

			bool dohole = true;
			for ( int i = 0; i < pages.Count; i++ )
			{
				if ( i == 0 || i == pages.Count - 1 )
					dohole = false;
				else
					dohole = true;

				pages[i].Update(this, (Flip * 14.0f) - ((float)i * shuffle), dohole, palpha);
			}

			updateObjs = false;

			if ( doattach )
				Attach();

			float a = pages[0].turnerangcon.Evaluate((Flip * 14.0f) + 14.0f);
			if ( a > 0.0f )
				a = 0.0f;
			frontCoverAngle = a * Turn_maxAngleCrv.Evaluate(palpha);
			frontCoverAngle += Turn_minAngleCrv.Evaluate(Flip * 14.0f) * palpha;

			a = pages[pages.Count - 1].turnerangcon.Evaluate((Flip * 14.0f) - ((float)(NumPages) * shuffle));
			if ( a > 0.0f )
				a = 0.0f;
			backCoverAngle = a * Turn_maxAngleCrv.Evaluate(palpha);
			backCoverAngle += Turn_minAngleCrv.Evaluate((Flip * 14.0f) - ((float)(NumPages) * shuffle)) * palpha;
		}

		public float FrontCoverAngle()
		{
			return frontCoverAngle;
		}

		float frontCoverAngle;

		float backCoverAngle;

		public float BackCoverAngle()
		{
			return backCoverAngle;
		}

		MegaBookPage CreatePage(int i)
		{
			MegaBookPage page = new MegaBookPage();

			page.turner = new MegaBookBendMod();
			page.flexer = new MegaBookBendMod();
			page.lander = new MegaBookBendMod();

			page.flexer.axis = MegaBookAxis.X;
			page.flexer.fromto = true;
			page.flexer.from = -Flex_CArea * pageWidth;
			page.flexer.to = 0.0f;
			page.flexer.center = new Vector3(-Flex_CCenter * pageWidth, 0.0f, 0.0f);

			page.turner.axis = MegaBookAxis.X;
			page.turner.fromto = true;
			page.turner.from = -3.0f;
			page.turner.to = 0.0f;
			page.turner.center = new Vector3(-Turn_CCenter * pageWidth, 0.0f, 0.0f);	
			page.turner.gizmo_rotation = Vector3.zero;

			page.lander.axis = MegaBookAxis.X;
			page.lander.fromto = true;
			page.lander.from = -(Land_CArea * pageWidth);
			page.lander.to = 0.0f;
			page.lander.center = new Vector3(-(pageWidth / 2.0f * Land_CCenter), 0.0f, 0.0f);

			// Anim keys
			page.turnerfromcon.AddKey(0.0f, -3.0f);
			page.turnerfromcon.AddKey(7.0f, -25.0f);
			page.turnerfromcon.AddKey(14.0f, -3.0f);

			page.turnerangcon.AddKey(0.0f, -Turn_minAngle);
			page.turnerangcon.AddKey(2.0f, 0.0f);
			page.turnerangcon.AddKey(12.0f, 0.0f);
			Keyframe key = page.turnerangcon.keys[2];
			key.value = -Turn_maxAngle;	//180.0f;
			page.turnerangcon.MoveKey(2, key);

			page.flexangcon.AddKey(0.0f, 0.0f);
			page.flexangcon.AddKey(7.0f, -Flex_MaxAngle);
			//page.flexangcon.AddKey(7.0f, 0.0f);	//-Flex_MaxAngle);
			page.flexangcon.AddKey(12.0f, Flex_MaxAngle - ((Flex_MaxAngle / 100.0f) * 25.0f));
			page.flexangcon.AddKey(14.0f, 0.0f);

			page.landerangcon.AddKey(0.0f, Land_minAngle);
			page.landerangcon.AddKey(10.0f, 0.0f);
			page.landerangcon.AddKey(14.0f, Land_maxAngle);

			page.turnerfromcon1.keys = page.turnerfromcon.keys;

			page.obj = MakePageObject(page, i);

			pages.Add(page);

			return page;
		}

		public void BuildPageMeshes()
		{
			updateBindings = true;
			RemovePages();
			BuildPages();

			Flip = page - 0.0002f;
		}

		public void UpdateAttached()
		{
			int pnum = 0;
			for ( int i = 0; i < pages.Count; i++ )
			{
				MegaBookPage page = pages[i];

				pnum = i % pageparams.Count;
				MegaBookPageParams parms = pageparams[pnum];

				page.visobjlow = parms.visobjlow;
				page.visobjhigh = parms.visobjhigh;

				for ( int j = 0; j < parms.objects.Count; j++ )
				{
					//if ( parms.objects[j].obj )
					{
						page.objects[j].pos = parms.objects[j].pos;
						page.objects[j].rot = parms.objects[j].rot;
						page.objects[j].offset = parms.objects[j].offset;
						page.objects[j].attachforward = parms.objects[j].attachforward;
						page.objects[j].message = parms.objects[j].message;

						UpdateAttachObject(page, page.objects[j]);
					}
				}
			}
		}

		public void BuildPages()
		{
			Quaternion rot = transform.rotation;
			transform.rotation = Quaternion.identity;
			pageindex = 0;
			Random.InitState(seed);

			pages.Clear();

			float py = 0.0f;
			if ( NumPages > 1 )
				py = ((NumPages - 1) * pageGap) * 0.5f;

			for ( int i = 0; i < NumPages; i++ )
			{
				float alpha = (float)i / (float)(NumPages - 1);
				MegaBookPage page = CreatePage(i);

				page.obj.transform.localPosition = new Vector3(0.0f, py, 0.0f);

				py -= pageGap;

				// Need to have controls add to start value
				if ( Flex_Random )
					page.flexer.gizmo_rotation = new Vector3(0.0f, Random.Range(-Flex_RandomDegree, Flex_RandomDegree), 0.0f);
				else
					page.flexer.gizmo_rotation = new Vector3(0.0f, Flex_RandomDegree, 0.0f);

				float ts = Mathf.Lerp(Turn_Spread, Turn_Spread1, Turn_SpreadCrv.Evaluate(alpha));
				float ts1 = Mathf.Lerp(Turn_SpreadRead, Turn_SpreadRead1, Turn_SpreadCrv.Evaluate(alpha));
				Keyframe key = page.turnerfromcon.keys[0];
				key.value = -((Turn_CArea * pageWidth) + ((float)(NumPages - (float)i) * (pageGap * ts)));	//Turn_Spread)));
				page.turnerfromcon.MoveKey(0, key);

				key = page.turnerfromcon.keys[1];
				key.value = -pageWidth;
				page.turnerfromcon.MoveKey(1, key);

				key = page.turnerfromcon.keys[2];
				key.value = -((Turn_CArea * pageWidth) + ((float)i * (pageGap * ts)));	//Turn_Spread)));
				page.turnerfromcon.MoveKey(2, key);

				//page.turnerfromcon1.keys = page.turnerfromcon.keys;
				page.turnerfromcon1 = new AnimationCurve(page.turnerfromcon.keys);

				// read spread
				key = page.turnerfromcon1.keys[0];
				key.value = -((Turn_CArea * pageWidth) + ((float)(NumPages - (float)i) * (pageGap * ts1)));  //Turn_Spread)));
				page.turnerfromcon1.MoveKey(0, key);

				key = page.turnerfromcon1.keys[1];
				key.value = -pageWidth;
				page.turnerfromcon1.MoveKey(1, key);

				key = page.turnerfromcon1.keys[2];
				key.value = -((Turn_CArea * pageWidth) + ((float)i * (pageGap * ts1)));  //Turn_Spread)));
				page.turnerfromcon1.MoveKey(2, key);
			}

			List<bool> nop = new List<bool>();
			for ( int i = 0; i < pages.Count - 1; i++ )
				nop.Add(pages[i].noHole);

			for ( int i = 0; i < pages.Count - 1; i++ )
			{
				if ( nop[i] )
				{
					if ( i > 0 )
						pages[i - 1].noHole = true;
					pages[i + 1].noHole = true;
				}
			}

			UpdateSettings();
			transform.rotation = rot;
			bookBuiltEvent.Invoke(this);

			//noiseApply = false;
		}

		public void UpdateSettings()
		{
			ChangeWidthValue(pageWidth);
			ChangeBookThickness(bookthickness, usebookthickness);
			gapChange(pageGap);
			Flex_CCenterChange(Flex_CCenter);
			Flex_CAreaChange(Flex_CArea);
			Flex_MaxAngleChange(Flex_MaxAngle);
			Flex_RandomChange(Flex_RandomDegree, Flex_RandomSeed, Flex_Random);
			Turn_CCenterChange(Turn_CCenter);
			Turn_CAreaChange(Turn_CArea);
			Turn_maxAngleChange(Turn_maxAngle);
			Turn_minAngleChange(Turn_minAngle);
			Land_CCenterChange(Land_CCenter);
			Land_CAreaChange(Land_CArea);
			Land_MaxAngleChange(Land_maxAngle);
			Land_MinAngleChange(Land_minAngle);
		}

		public void ChangeBookThickness(float thick, bool use)
		{
			usebookthickness = use;
			if ( thick < 0.0f )
				thick = 0.0f;

			bookthickness = thick;
			if ( use )
			{
				if ( NumPages > 1 )
					pageGap = (bookthickness - pageHeight) / (NumPages - 1);
				else
					pageGap = 0.0f;

				gapChange(pageGap);
			}
		}

		void ChangeWidthValue(float widthVal)
		{
			pageWidth = widthVal;
			for ( int i = 0; i < pages.Count; i++ )
				pages[i].width = widthVal;
		}

		void pageLengthChange(float lengthVal)
		{
			for ( int i = 0; i < pages.Count;i++ )
				pages[i].length = lengthVal;
		}

		int spineRadiusMode = 0;

		void gapChange(float gapVal)
		{
			pageGap = gapVal;

			float py = 0.0f;
			if ( NumPages > 1 )
				py = ((NumPages - 1) * pageGap) * 0.5f;

			float radius = 0.0f;
		
			if ( spineradius != 0.0f )
				radius = py / spineradius;

			float px = 0.0f;
			float off = 0.0f;

			if ( spineRadiusMode == 0 )
			{
				float v = Mathf.Clamp(py / radius, -1.0f, 1.0f);
				float theta = Mathf.Asin(v);
				float maxx = radius - (Mathf.Cos(theta) * radius);

				for ( int i = 0; i < pages.Count; i++ )
				{
					if ( radius != 0.0f )
					{
						v = Mathf.Clamp(py / radius, -1.0f, 1.0f);
						theta = Mathf.Asin(v);
						px = radius - (Mathf.Cos(theta) * radius);

						//if ( i == 0 && radius < 0.0f )
							off = px;
						px -= off;	//maxx;	//off;
					}

					if ( pages[i].obj )
						pages[i].obj.transform.localPosition = new Vector3(px, py, 0.0f);
					py -= pageGap;
					pages[i].deform = true;
				}
			}
			else
			{
				for ( int i = 0; i < pages.Count; i++ )
				{
					if ( radius != 0.0f )
					{
						float v = Mathf.Clamp(py / radius, -1.0f, 1.0f);
						float theta = Mathf.Asin(v);
						px = radius - (Mathf.Cos(theta) * radius);

						if ( i == 0 && radius < 0.0f )
							off = px;
						px -= off;
					}

					if ( pages[i].obj )
						pages[i].obj.transform.localPosition = new Vector3(px, py, 0.0f);
					py -= pageGap;
					pages[i].deform = true;
				}
			}
		}

		void SpineUpdate(float alpha)
		{
			float py = 0.0f;
			if ( NumPages > 1 )
				py = ((NumPages - 1) * pageGap) * 0.5f;

			bookthickness = py * 2.0f;

			float radius = 0.0f;

			float sr = spineradius * spineCurve.Evaluate(alpha);
			if ( spineradius != 0.0f )
			{
				float d = sr;	//spineradius * spineCurve.Evaluate(alpha);
				if ( d != 0.0f )
					radius = py / d;
			}

			float px = 0.0f;
			float off = 0.0f;

			if ( spineRadiusMode == 0 )
			{
				float v = Mathf.Clamp(py / radius, -1.0f, 1.0f);
				float theta = Mathf.Asin(v);
				float maxx = radius - (Mathf.Cos(theta) * radius);

				for ( int i = 0; i < pages.Count; i++ )
				{
					if ( radius != 0.0f )
					{
						v = Mathf.Clamp(py / radius, -1.0f, 1.0f);
						theta = Mathf.Asin(v);
						px = radius - (Mathf.Cos(theta) * radius);

						if ( i == 0 && radius < 0.0f )
							off = px;
						px -= maxx;	//off;
					}

					if ( pages[i].obj )
						pages[i].obj.transform.localPosition = new Vector3(px, py, 0.0f);
					py -= pageGap;
					pages[i].deform = true;
				}
			}
			else
			{
				for ( int i = 0; i < pages.Count; i++ )
				{
					if ( radius != 0.0f )
					{
						float v = Mathf.Clamp(py / radius, -1.0f, 1.0f);
						float theta = Mathf.Asin(v);
						px = radius - (Mathf.Cos(theta) * radius);

						if ( i == 0 && radius < 0.0f )
							off = px;
						px -= off;
					}

					pages[i].obj.transform.localPosition = new Vector3(px, py, 0.0f);
					py -= pageGap;
					//pages[i].deform = true;
				}
			}

			if ( headband1 )
				headband1.DeformMesh(py * 2.0f, sr);

			if ( headband2 )
				headband2.DeformMesh(py * 2.0f, sr);

			if ( spineFabric )
				spineFabric.DeformMesh(py * 2.0f, sr);
		}

		public void Flex_CCenterChange(float newVal)
		{
			Flex_CCenter = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				pages[i].flexer.center = new Vector3(-Flex_CCenter * pageWidth, 0.0f, 0.0f);
				pages[i].deform = true;
			}
		}
	
		public void Flex_CAreaChange(float newVal)
		{
			Flex_CArea = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				pages[i].flexer.from = -Flex_CArea * pageWidth;
				pages[i].deform = true;
			}
		}
	
		public void Flex_MaxAngleChange(float newVal)
		{
			Flex_MaxAngle = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				Keyframe key = pages[i].flexangcon.keys[1];
				key.value = -Flex_MaxAngle;
				pages[i].flexangcon.MoveKey(1, key);

				key = pages[i].flexangcon.keys[2];
				key.value = Flex_MaxAngle - ((Flex_MaxAngle / 100.0f) * 25.0f);
				pages[i].flexangcon.MoveKey(2, key);

				pages[i].deform = true;
				pages[i].flexangcon.SmoothTangents(0, 0.0f);
				pages[i].flexangcon.SmoothTangents(1, 0.0f);
				pages[i].flexangcon.SmoothTangents(2, 0.0f);
				pages[i].flexangcon.SmoothTangents(3, 0.0f);
			}
		}

		public void Flex_RandomChange(float newVal, int seed, bool userand)
		{
			Flex_Random = userand;
			Flex_RandomSeed = seed;
			Flex_RandomDegree = newVal;

			Random.InitState(Flex_RandomSeed);

			for ( int i = 0; i < pages.Count; i++ )
			{
				if ( Flex_Random )
					pages[i].flexer.gizmo_rotation = new Vector3(0.0f, Random.Range(-Flex_RandomDegree, Flex_RandomDegree), 0.0f);
				else
					pages[i].flexer.gizmo_rotation = new Vector3(0.0f, Flex_RandomDegree, 0.0f);

				pages[i].deform = true;
			}
		}
	
		public void Turn_CCenterChange(float newVal)
		{
			Turn_CCenter = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				pages[i].turner.center = new Vector3(-newVal * pageWidth, 0.0f, 0.0f);
				pages[i].deform = true;
			}
		}

		public void Turn_SpreadChange(float newVal)
		{
			Turn_Spread = newVal;
			Turn_CAreaChange(Turn_CArea);
		}

		public void Turn_CAreaChange(float newVal)
		{
			Turn_CArea = newVal;
			if ( changespineangle )
			{
				for ( int i = 0; i < pages.Count; i++ )
				{
					float alpha = (float)i / (float)(pages.Count - 1);
					MegaBookPage page = pages[i];

					float ts = Mathf.Lerp(Turn_Spread, Turn_Spread1, Turn_SpreadCrv.Evaluate(alpha));
					float ts1 = Mathf.Lerp(Turn_SpreadRead, Turn_SpreadRead1, Turn_SpreadCrv.Evaluate(alpha));

					Keyframe key = page.turnerfromcon.keys[0];
					if ( i == 0 )
						key.value = -((Turn_CArea * pageWidth) + pageWidth);
					else
						key.value = -((Turn_CArea * pageWidth) + ((float)(NumPages - i) * (pageGap * ts)));	//Turn_Spread)));
					page.turnerfromcon.MoveKey(0, key);

					key = page.turnerfromcon.keys[1];
					key.value = -pageWidth;
					page.turnerfromcon.MoveKey(1, key);

					key = page.turnerfromcon.keys[2];
					key.value = -((Turn_CArea * pageWidth) + ((float)i * (pageGap * ts)));	//Turn_Spread)));
					page.turnerfromcon.MoveKey(2, key);

					page.turnerfromcon.SmoothTangents(0, 0.0f);
					page.turnerfromcon.SmoothTangents(1, 0.0f);
					page.turnerfromcon.SmoothTangents(2, 0.0f);

					//page.turnerfromcon1.keys = page.turnerfromcon.keys;
					page.turnerfromcon1 = new AnimationCurve(page.turnerfromcon.keys);

					key = page.turnerfromcon1.keys[0];
					if ( i == 0 )
						key.value = -((Turn_CArea * pageWidth) + pageWidth);
					else
						key.value = -((Turn_CArea * pageWidth) + ((float)(NumPages - i) * (pageGap * ts1))); //Turn_Spread)));
					page.turnerfromcon1.MoveKey(0, key);

					key = page.turnerfromcon1.keys[1];
					key.value = -pageWidth;
					page.turnerfromcon1.MoveKey(1, key);

					key = page.turnerfromcon1.keys[2];
					key.value = -((Turn_CArea * pageWidth) + ((float)i * (pageGap * ts1)));  //Turn_Spread)));
					page.turnerfromcon1.MoveKey(2, key);

					page.turnerfromcon1.SmoothTangents(0, 0.0f);
					page.turnerfromcon1.SmoothTangents(1, 0.0f);
					page.turnerfromcon1.SmoothTangents(2, 0.0f);

					page.deform = true;
				}
			}
			else
			{
				for ( int i = 0; i < pages.Count; i++ )
				{
					MegaBookPage page = pages[i];

					Keyframe key = page.turnerfromcon.keys[0];
					//if ( i == 0 )
						key.value = -((Turn_CArea * pageWidth) + (pageWidth * 4.0f));
					//else
						//key.value = -((Turn_CArea * pageWidth) + ((float)(NumPages - i) * (pageGap * Turn_Spread) * 18.0f));
					page.turnerfromcon.MoveKey(0, key);

					key = page.turnerfromcon.keys[1];
					key.value = -pageWidth * 4.0f;
					page.turnerfromcon.MoveKey(1, key);

					key = page.turnerfromcon.keys[2];
					key.value = -((Turn_CArea * pageWidth) + ((float)i * (pageGap * Turn_Spread)));
					page.turnerfromcon.MoveKey(2, key);

					page.turnerfromcon.SmoothTangents(0, 0.0f);
					page.turnerfromcon.SmoothTangents(1, 0.0f);
					page.turnerfromcon.SmoothTangents(2, 0.0f);

					//page.turnerfromcon1.keys = page.turnerfromcon.keys;
					page.turnerfromcon1 = new AnimationCurve(page.turnerfromcon.keys);

					key = page.turnerfromcon1.keys[0];
					//if ( i == 0 )
					key.value = -((Turn_CArea * pageWidth) + (pageWidth * 4.0f));
					//else
					//key.value = -((Turn_CArea * pageWidth) + ((float)(NumPages - i) * (pageGap * Turn_Spread) * 18.0f));
					page.turnerfromcon1.MoveKey(0, key);

					key = page.turnerfromcon1.keys[1];
					key.value = -pageWidth * 4.0f;
					page.turnerfromcon1.MoveKey(1, key);

					key = page.turnerfromcon1.keys[2];
					key.value = -((Turn_CArea * pageWidth) + ((float)i * (pageGap * Turn_SpreadRead)));
					page.turnerfromcon1.MoveKey(2, key);

					page.turnerfromcon1.SmoothTangents(0, 0.0f);
					page.turnerfromcon1.SmoothTangents(1, 0.0f);
					page.turnerfromcon1.SmoothTangents(2, 0.0f);

					page.deform = true;
				}
			}
		}
	
		public void Turn_maxAngleChange(float newValue)
		{
			Turn_maxAngle = newValue;
			for ( int i = 0; i < pages.Count; i++ )
			{
				Keyframe key = pages[i].turnerangcon.keys[2];
				key.value = -Turn_maxAngle;
				pages[i].turnerangcon.MoveKey(2, key);
				pages[i].deform = true;

				pages[i].turnerangcon.SmoothTangents(0, 0.0f);
				pages[i].turnerangcon.SmoothTangents(1, 0.0f);
				pages[i].turnerangcon.SmoothTangents(2, 0.0f);
			}
		}

		public void Turn_minAngleChange(float newValue)
		{
			Turn_minAngle = newValue;
			for ( int i = 0; i < pages.Count; i++ )
			{
				Keyframe key = pages[i].turnerangcon.keys[0];
				key.value = -Turn_minAngle;
				pages[i].turnerangcon.MoveKey(0, key);
				pages[i].deform = true;

				pages[i].turnerangcon.SmoothTangents(0, 0.0f);
				pages[i].turnerangcon.SmoothTangents(1, 0.0f);
				pages[i].turnerangcon.SmoothTangents(2, 0.0f);
			}
		}

		void Turn_CLevelChange(float newVal)
		{
			Turn_CLevel = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				pages[i].turner.gizmo_rotation = new Vector3(0.0f, Turn_CLevel, 0.0f);
				pages[i].turner.gizmo_pos = new Vector3((-(pageWidth / 2.0f - (Mathf.Cos(Mathf.Deg2Rad * Turn_CLevel) * (pageWidth / 2.0f)))),
					0.0f,
					(((Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * Turn_CLevel))) * (pageWidth / 2.0f)) + ((Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * 0.0f))) * (pageWidth / 2.0f))));
			}
		}
	
		public void Land_CCenterChange(float newVal)
		{
			Land_CCenter = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				pages[i].lander.center = new Vector3(-(pageWidth / 2.0f * Land_CCenter), 0.0f, 0.0f);
				pages[i].deform = true;
			}
		}

		public void Land_CAreaChange(float newVal)
		{
			Land_CArea = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				pages[i].lander.from = -(Land_CArea * pageWidth);
				pages[i].deform = true;
			}
		}

		public void Land_MaxAngleChange(float newVal)
		{
			Land_maxAngle = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				Keyframe key = pages[i].landerangcon.keys[2];
				key.value = Land_maxAngle;
				pages[i].landerangcon.MoveKey(2, key);
				pages[i].deform = true;

				pages[i].landerangcon.SmoothTangents(0, 0.0f);
				pages[i].landerangcon.SmoothTangents(1, 0.0f);
				pages[i].landerangcon.SmoothTangents(2, 0.0f);
			}
		}

		public void Land_MinAngleChange(float newVal)
		{
			Land_minAngle = newVal;
			for ( int i = 0; i < pages.Count; i++ )
			{
				Keyframe key = pages[i].landerangcon.keys[0];
				key.value = Land_minAngle;
				pages[i].landerangcon.MoveKey(0, key);
				pages[i].deform = true;

				pages[i].landerangcon.SmoothTangents(0, 0.0f);
				pages[i].landerangcon.SmoothTangents(1, 0.0f);
				pages[i].landerangcon.SmoothTangents(2, 0.0f);
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

		public void AttachObject(MegaBookPage pobj, MegaBookPageObject obj)
		{
			if ( obj != null )	//&& obj.obj != null )
			{
				Bounds bounds = pobj.bbox;

				// Calc local pos from pos in bounds
				Vector3 pp = obj.pos * 0.01f;

				pp.x = Mathf.Clamp01(pp.x);
				pp.y = Mathf.Clamp01(pp.y);
				pp.z = Mathf.Clamp01(pp.z);

				Vector3 lpos = bounds.min + (Vector3.Scale(pp, bounds.size));

				Vector3 objSpacePt = lpos;

				Vector3[] verts = pobj.verts;
				int[] tris = pobj.mesh.triangles;
				int index = -1;
				MegaBookNearest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref obj.BaryCoord);

				if ( index >= 0 )
				{
					obj.BaryVerts[0] = tris[index];
					obj.BaryVerts[1] = tris[index + 1];
					obj.BaryVerts[2] = tris[index + 2];
				}

				MegaBookNearest.NearestPointOnMesh1(objSpacePt + obj.attachforward, verts, tris, ref index, ref obj.BaryCoord1);

				if ( index >= 0 )
				{
					obj.BaryVerts1[0] = tris[index];
					obj.BaryVerts1[1] = tris[index + 1];
					obj.BaryVerts1[2] = tris[index + 2];
				}

				if ( obj.obj )
					obj.canvasGroup = obj.obj.GetComponent<CanvasGroup>();

				pobj.objects.Add(obj);
			}
		}

		public void FirstAttachObject(MegaBookPage pobj, MegaBookPageObject obj)
		{
			if ( obj != null )  //&& obj.obj != null )
			{
				Bounds bounds = pobj.bbox;

				// Calc local pos from pos in bounds
				Vector3 pp = pobj.obj.transform.InverseTransformPoint(obj.obj.transform.position);	//obj.pos * 0.01f;

				pp.x = Mathf.Clamp01(pp.x);
				pp.y = Mathf.Clamp01(pp.y);
				pp.z = Mathf.Clamp01(pp.z);

				Vector3 lpos = bounds.min + (Vector3.Scale(pp, bounds.size));

				Vector3 objSpacePt = lpos;

				Vector3[] verts = pobj.verts;
				int[] tris = pobj.mesh.triangles;
				int index = -1;
				MegaBookNearest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref obj.BaryCoord);

				if ( index >= 0 )
				{
					obj.BaryVerts[0] = tris[index];
					obj.BaryVerts[1] = tris[index + 1];
					obj.BaryVerts[2] = tris[index + 2];
				}

				MegaBookNearest.NearestPointOnMesh1(objSpacePt + obj.attachforward, verts, tris, ref index, ref obj.BaryCoord1);

				if ( index >= 0 )
				{
					obj.BaryVerts1[0] = tris[index];
					obj.BaryVerts1[1] = tris[index + 1];
					obj.BaryVerts1[2] = tris[index + 2];
				}

				if ( obj.obj )
					obj.canvasGroup = obj.obj.GetComponent<CanvasGroup>();

				pobj.objects.Add(obj);
			}
		}

		public void UpdateAttachObject(MegaBookPage pobj, MegaBookPageObject obj)
		{
			if ( obj != null && obj.obj )	//!= null && obj.active )
			{
				Bounds bounds = pobj.bbox;

				// Calc local pos from pos in bounds
				Vector3 pp = obj.pos * 0.01f;

				pp.x = Mathf.Clamp01(pp.x);
				pp.y = Mathf.Clamp01(pp.y);
				pp.z = Mathf.Clamp01(pp.z);

				Vector3 lpos = bounds.min + (Vector3.Scale(pp, bounds.size));

				Vector3 objSpacePt = lpos;

				Vector3[] verts = pobj.verts;
				int[] tris = pobj.mesh.triangles;
				int index = -1;
				MegaBookNearest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref obj.BaryCoord);

				if ( index >= 0 )
				{
					obj.BaryVerts[0] = tris[index];
					obj.BaryVerts[1] = tris[index + 1];
					obj.BaryVerts[2] = tris[index + 2];
				}

				MegaBookNearest.NearestPointOnMesh1(objSpacePt + obj.attachforward, verts, tris, ref index, ref obj.BaryCoord1);

				if ( index >= 0 )
				{
					obj.BaryVerts1[0] = tris[index];
					obj.BaryVerts1[1] = tris[index + 1];
					obj.BaryVerts1[2] = tris[index + 2];
				}

				//pobj.objects.Add(obj);
			}
		}

		public Vector3 CalcPos(GameObject obj, MegaBookPageParams page, Vector3 oldpos)
		{
			return oldpos;
		}

		public void SetPage(float p)
		{
			page = Mathf.Lerp(MinPageVal(), MaxPageVal(), p);
		}

		public void DisposeArrays()
		{
			for ( int i = 0; i < pages.Count; i++ )
				pages[i].DisposeArrays();
		}

		public void SetLayerID(int pagenum, bool front, LayerID lid)
		{
			if ( pagenum >= 0 && pagenum < pageparams.Count )
			{
				if ( front )
					pageparams[pagenum].frontLayerID = lid;
				else
					pageparams[pagenum].backLayerID = lid;

				updateObjs = true;
			}
		}

		public LayerID GetLayerID(int pagenum, bool front)
		{
			if ( pagenum >= 0 && pagenum < pageparams.Count )
			{
				if ( front )
					return pageparams[pagenum].frontLayerID;
				else
					return pageparams[pagenum].backLayerID;
			}

			return LayerID.None;
		}

		// Mesh content code
		List<Vector3>	verts		= new List<Vector3>();
		List<Vector2>	uvs			= new List<Vector2>();
		List<Vector2>	uv2s		= new List<Vector2>();
		List<Color>		cols		= new List<Color>();
		List<int[]>		subtris		= new List<int[]>();
		List<Material>	materials	= new List<Material>();

		public int GetNumMaterialsFront(MegaBookPageParams page)
		{
			materials.Clear();

			if ( page.meshFront )
			{
				GameObject obj = page.meshFrontRoot;

				if ( obj )
				{
					Transform[] objs = obj.GetComponentsInChildren<Transform>(true);

					for ( int i = 0; i < objs.Length; i++ )
					{
						TMPro.TextMeshPro tmp = objs[i].GetComponent<TMPro.TextMeshPro>();
						if ( tmp )
							materials.Add(objs[i].GetComponent<Renderer>().sharedMaterial);
						else
						{
							MeshFilter mf = objs[i].GetComponent<MeshFilter>();
							if ( mf && mf.sharedMesh && mf.sharedMesh.isReadable )
							{
								Renderer mr = objs[i].GetComponent<Renderer>();
								for ( int m = 0; m < mr.sharedMaterials.Length; m++ )
									materials.Add(mr.sharedMaterials[m]);
							}
						}
					}
				}
			}

			return materials.Count;
		}

		public int GetNumMaterialsBack(MegaBookPageParams page)
		{
			materials.Clear();

			if ( page.meshBack )
			{
				GameObject obj = page.meshBackRoot;

				if ( obj )
				{
					Transform[] objs = obj.GetComponentsInChildren<Transform>(true);

					for ( int i = 0; i < objs.Length; i++ )
					{
						TMPro.TextMeshPro tmp = objs[i].GetComponent<TMPro.TextMeshPro>();
						if ( tmp )
							materials.Add(objs[i].GetComponent<Renderer>().sharedMaterial);
						else
						{
							MeshFilter mf = objs[i].GetComponent<MeshFilter>();
							if ( mf && mf.sharedMesh && mf.sharedMesh.isReadable )
							{
								Renderer mr = objs[i].GetComponent<Renderer>();
								for ( int m = 0; m < mr.sharedMaterials.Length; m++ )
									materials.Add(mr.sharedMaterials[m]);
							}

						}

					}
				}
			}

			return materials.Count;
		}

		public Material GetMaterial(int page, bool front, int i)
		{
			return materials[i];
		}

		public void BuildMesh(MegaBookPageParams page, bool front)
		{
			verts.Clear();
			uvs.Clear();
			uv2s.Clear();
			cols.Clear();
			subtris.Clear();

			GameObject obj = page.meshFrontRoot;
			if ( !front )
				obj = page.meshBackRoot;

			if ( obj )
			{
				bool active = obj.activeInHierarchy;
				obj.SetActive(true);

				GetMeshData(obj, page, front);
				obj.SetActive(active);
			}
		}

		public Mesh GetTMProMesh(TMPro.TextMeshPro obj, bool force)
		{
			if ( obj )
			{
				if ( force )
				{
					obj.ForceMeshUpdate(true, true);

					if ( obj.textInfo != null && obj.textInfo.meshInfo != null && obj.textInfo.meshInfo.Length > 0 )
					{
						Mesh m = obj.textInfo.meshInfo[0].mesh;
						return obj.textInfo.meshInfo[0].mesh;
					}
				}
			}

			return null;
		}

		int GetSteps(TMProSubdiv val)
		{
			switch ( val )
			{
				case TMProSubdiv.UseBook:
				case TMProSubdiv.None:
				case TMProSubdiv.Auto:		return 0;
				case TMProSubdiv.Twice:		return 2;
				case TMProSubdiv.Triple:	return 3;
				case TMProSubdiv.Quad:		return 4;
			}

			return 0;
		}

		Mesh GetMesh(Transform obj, MegaBookPageParams pp, bool front, Vector3 lscl)
		{
			TMPro.TextMeshPro tmp = obj.GetComponent<TMPro.TextMeshPro>();
			if ( tmp )
			{
				int steps = 0;
				TMProSubdiv subdiv = front ? pp.frontTMProSubdiv : pp.backTMProSubdiv;

				switch ( subdiv )
				{
					case TMProSubdiv.UseBook:	steps = GetSteps(subdiv);	break;
					case TMProSubdiv.None:		steps = 0;	break;
					case TMProSubdiv.Auto:		steps = -1;	break;
					case TMProSubdiv.Twice:		steps = 2;	break;
					case TMProSubdiv.Triple:	steps = 3;	break;
					case TMProSubdiv.Quad:		steps = 4;	break;
				}

				Mesh tm = GetTMProMesh(tmp, true);

				if ( steps == 0 )
					return tm;

				List<Vector3>	newverts	= new List<Vector3>();
				List<Vector2>	newuvs		= new List<Vector2>();
				List<Vector2>	newuvs1		= new List<Vector2>();
				List<Color>		newcolors	= new List<Color>();
				List<Vector3>	newnorms	= new List<Vector3>();
				List<int>		newtris		= new List<int>();

				int num = tm.vertexCount / 4;

				Vector3[]	verts	= tm.vertices;
				Vector3[]	norms	= tm.normals;
				Vector2[]	uvs		= tm.uv;
				Vector2[]	uvs1	= tm.uv2;
				Color[]		cols	= tm.colors;
				int[]		tris	= tm.triangles;

				int stx = 0;
				int sty = 0;

				for ( int i = 0; i < num; i++ )
				{
					Vector3 p1 = verts[(i * 4) + 0];
					Vector3 p2 = verts[(i * 4) + 1];
					Vector3 p3 = verts[(i * 4) + 2];
					Vector3 p4 = verts[(i * 4) + 3];

					if ( p1 == Vector3.zero && p2 == Vector3.zero )
						continue;

					if ( steps == -1 )
					{
						Vector3 scl = lscl;	//obj.localScale;
						if ( front )
							scl = Vector3.Scale(scl, pp.meshFrontScale);
						else
							scl = Vector3.Scale(scl, pp.meshBackScale);

						scl.x = Mathf.Abs(scl.x);
						scl.y = Mathf.Abs(scl.y);
						scl.z = Mathf.Abs(scl.z);

						float tdx = Vector3.Distance(Vector3.Scale(p1, scl), Vector3.Scale(p4, scl));
						float tdy = Vector3.Distance(Vector3.Scale(p1, scl), Vector3.Scale(p2, scl));

						float px = pageWidth / (float)WidthSegs;
						float py = pageLength / (float)LengthSegs;

						stx = Mathf.CeilToInt((tdx / px));// + 0.5f);
						sty = Mathf.CeilToInt((tdy / py));	// + 0.5f);

						stx = Mathf.Clamp(stx, 1, 8);
						sty = Mathf.Clamp(stx, 1, 8);
					}
					else
					{
						stx = steps;
						sty = steps;
					}

					Vector2 uv1 = uvs[(i * 4) + 0];
					Vector2 uv2 = uvs[(i * 4) + 1];
					Vector2 uv3 = uvs[(i * 4) + 2];
					Vector2 uv4 = uvs[(i * 4) + 3];

					Vector2 uv12 = uvs1[(i * 4) + 0];
					Vector2 uv22 = uvs1[(i * 4) + 1];
					Vector2 uv32 = uvs1[(i * 4) + 2];
					Vector2 uv42 = uvs1[(i * 4) + 3];

					Color c1 = cols[(i * 4) + 0];
					Color c2 = cols[(i * 4) + 1];
					Color c3 = cols[(i * 4) + 2];
					Color c4 = cols[(i * 4) + 3];

					Vector3 norm = norms[(i * 4)];

					int index = newverts.Count;

					for ( int y = 0; y <= sty; y++ )
					{
						float ya = (float)y / (float)sty;
						Vector3 le		= Vector3.Lerp(p1, p2, ya);
						Vector3 re		= Vector3.Lerp(p4, p3, ya);
						Vector2 leuv	= Vector3.Lerp(uv1, uv2, ya);
						Vector2 reuv	= Vector3.Lerp(uv4, uv3, ya);
						Vector2 leuv2	= Vector3.Lerp(uv12, uv22, ya);
						Vector2 reuv2	= Vector3.Lerp(uv42, uv32, ya);
						Color lec = Color.Lerp(c1, c2, ya);
						Color rec = Color.Lerp(c4, c3, ya);

						for ( int x = 0; x <= stx; x++ )
						{
							float xa = (float)x / (float)stx;
							newverts.Add(Vector3.Lerp(le, re, xa));
							newuvs.Add(Vector2.Lerp(leuv, reuv, xa));
							newuvs1.Add(Vector2.Lerp(leuv2, reuv2, xa));
							newcolors.Add(Color.Lerp(lec, rec, xa));
							newnorms.Add(norm);
						}
					}

					// make new tris
					for ( int iz = 0; iz < sty; iz++ )
					{
						int kv = iz * (stx + 1) + index;
						for ( int ix = 0; ix < stx; ix++ )
						{
							MakeQuad2(newtris, kv, kv + stx + 1, kv + stx + 2, kv + 1);
							kv++;
						}
					}
				}
				Mesh newmesh = new Mesh();
				newmesh.subMeshCount = 1;
				newmesh.SetVertices(newverts);
				newmesh.SetUVs(0, newuvs);
				newmesh.SetUVs(1, newuvs1);
				newmesh.SetColors(newcolors);
				newmesh.SetNormals(newnorms);
				newmesh.SetTriangles(newtris, 0);
				return newmesh;
			}
			else
			{
				MeshFilter mf = obj.GetComponent<MeshFilter>();
				if ( mf )
					return mf.sharedMesh;
			}
			return null;
		}

		static void MakeQuad2(List<int> f, int a, int b, int c, int d)
		{
			f.Add(a);
			f.Add(b);
			f.Add(c);

			f.Add(c);
			f.Add(d);
			f.Add(a);
		}

		public void GetMeshData(GameObject obj, MegaBookPageParams pp, bool front)
		{
			if ( obj )
			{
				Matrix4x4 ptm = obj.transform.worldToLocalMatrix;
				Transform[] objs = obj.GetComponentsInChildren<Transform>(true);

				int vindex = 0;
				for ( int i = 0; i < objs.Length; i++ )
				{
					Matrix4x4 tm = ptm * objs[i].localToWorldMatrix;   // * ptm;

					Mesh mesh = GetMesh(objs[i], pp, front, tm.lossyScale);

					if ( mesh != null && mesh.isReadable )
					{
						Vector3[] vertices = mesh.vertices;
						Vector2[] uv1 = mesh.uv;
						Vector2[] uv2 = mesh.uv2;

						if ( uv1 == null || mesh.uv.Length == 0 )
							uv1 = new Vector2[vertices.Length];

						if ( uv2 == null || mesh.uv2.Length == 0 )
							uv2 = new Vector2[mesh.uv.Length];

						Color[] colors = mesh.colors;

						for ( int v = 0; v < mesh.vertexCount; v++ )
						{
							verts.Add(tm.MultiplyPoint3x4(vertices[v]));
							uvs.Add(uv1[v]);
						}

						for ( int v = 0; v < mesh.vertexCount; v++ )
							uv2s.Add(uv2[v]);

						if ( mesh.colors != null && mesh.colors.Length > 0 )
						{
							for ( int v = 0; v < mesh.vertexCount; v++ )
								cols.Add(colors[v]);
						}
						else
							for ( int v = 0; v < mesh.vertexCount; v++ )
								cols.Add(Color.white);

						for ( int m = 0; m < mesh.subMeshCount; m++ )
						{
							int[] tris = mesh.GetTriangles(m);

							for ( int t = 0; t < tris.Length; t++ )
								tris[t] += vindex;

							subtris.Add(tris);
						}

						vindex += mesh.vertexCount;
					}
				}
			}
		}

		public Vector3[] GetVertices()
		{
			return verts.ToArray();
		}

		public Color[] GetColors()
		{
			return cols.ToArray();
		}

		public Vector2[] GetUVs()
		{
			return uvs.ToArray();
		}

		public Vector2[] GetUV2s()
		{
			return uv2s.ToArray();
		}

		public int[] GetTris(int m)
		{
			return subtris[m];
		}

		public Vector3 MapPoint(int page, Vector3 p)
		{
			if ( page >= 0 && page < pages.Count )
				return pages[page].MapPoint(this, p);

			return Vector3.zero;
		}

		public Vector3 MapPoint(int page, float u, float v)
		{
			if ( page >= 0 && page < pages.Count )
				return pages[page].MapPoint(this, u, v);

			return Vector3.zero;
		}
	}
}