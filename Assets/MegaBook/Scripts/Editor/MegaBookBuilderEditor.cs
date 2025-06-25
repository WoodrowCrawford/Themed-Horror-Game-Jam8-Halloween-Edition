using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MegaBook
{
	[CustomEditor(typeof(MegaBookBuilder))]
	public class MegaBookBuilderEditor : Editor
	{
		static List<MegaBookCover> coverStyles = new List<MegaBookCover>();
		static string[] coverStyleNames;

		static void CreateCoverStyles()
		{
			string[] csdirs = System.IO.Directory.GetDirectories(Application.dataPath, "MegaBook/Cover Styles");

			// Cover styles
			coverStyles.Clear();
			string[] folders = new string[1];
#if false
			string[] dirs = System.IO.Directory.GetDirectories(Application.dataPath, "MegaBook/Cover Styles");

			if ( dirs.Length == 0 )
			{
				Debug.Log("The 'MegaBook/Cover Styles' folder could not be found!");
				return;
			}

			string lpath = "Assets" + dirs[0].Substring(Application.dataPath.Length);

			string[] cfolders = new string[]
			{
				"Assets/MegaBook/Cover Styles"
			};

			cfolders[0] = lpath;
#endif
			for ( int i = 0; i < csdirs.Length; i++ )
			{
				folders[0] = "Assets" + csdirs[i].Substring(Application.dataPath.Length);

				string[] guids = AssetDatabase.FindAssets("t:Object", folders);
				//string[] guids = AssetDatabase.FindAssets("t:Object");	//, cfolders);
				for ( int j = 0; j < guids.Length; j++ )
				{
					string path = AssetDatabase.GUIDToAssetPath(guids[j]);

					MegaBookCover cover = (MegaBookCover)AssetDatabase.LoadAssetAtPath(path, typeof(MegaBookCover));

					if ( cover )
						coverStyles.Add(cover);
				}
			}

			coverStyleNames = new string[coverStyles.Count + 2];

			coverStyleNames[0] = "Select to Change Cover";
			coverStyleNames[1] = "Remove Cover";
			for ( int i = 0; i < coverStyles.Count; i++ )
				coverStyleNames[i + 2] = coverStyles[i].name;
		}

#if false
		[MenuItem("GameObject/Create Other/MegaBook")]
		static void CreateBook()
		{
			Vector3 pos = Vector3.zero;

			if ( UnityEditor.SceneView.lastActiveSceneView )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject go = new GameObject("Book");

			MegaBookBuilder book = go.AddComponent<MegaBookBuilder>();

			book.pagesectioncrv = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
			book.basematerial = (Material)Resources.Load("Materials/MegaBook Front", typeof(Material));
			book.basematerial1 = (Material)Resources.Load("Materials/MegaBook Back", typeof(Material));
			book.basematerial2 = (Material)Resources.Load("Materials/MegaBook Edge", typeof(Material));

			for ( int i = 0; i < book.NumPages; i++ )
			{
				MegaBookPageParams page = new MegaBookPageParams();

				if ( i == 0 )
				{
					page.front = (Texture2D)Resources.Load("Textures/MegaBook Cover", typeof(Texture2D));
					page.back = (Texture2D)Resources.Load("Textures/MegaBook InsideFrontCover", typeof(Texture2D));
				}
				else
				{
					if ( i == book.NumPages - 1 ) 
					{
						page.front = (Texture2D)Resources.Load("Textures/MegaBook InsideBackCover", typeof(Texture2D));
						page.back = (Texture2D)Resources.Load("Textures/MegaBook BackCover", typeof(Texture2D));
					}
					else
					{
						page.front = (Texture2D)Resources.Load("Textures/MegaBook Front", typeof(Texture2D));
						page.back = (Texture2D)Resources.Load("Textures/MegaBook Back", typeof(Texture2D));
					}
				}

				book.pageparams.Add(page);
			}

			go.transform.position = pos;
			Selection.activeObject = go;
			book.rebuild = true;
		}
#endif
		private MegaBookBuilder		src;

		private void OnEnable()
		{
			src = target as MegaBookBuilder;
		}

		static void SwapTextures(MegaBookBuilder mod, int t1, int t2)
		{
			if ( t1 >= 0 && t1 < mod.pagetextures.Count && t2 >= 0 && t2 < mod.pagetextures.Count && t1 != t2 )
			{
				Texture2D mt1 = mod.pagetextures[t1];
				mod.pagetextures.RemoveAt(t1);
				mod.pagetextures.Insert(t2, mt1);
				EditorUtility.SetDirty(mod);	//target);
			}
		}

		static void SwapPages(MegaBookBuilder mod, int t1, int t2)
		{
			if ( t1 >= 0 && t1 < mod.pages.Count && t2 >= 0 && t2 < mod.pages.Count && t1 != t2 )
			{
				MegaBookPageParams mt1 = mod.pageparams[t1];
				mod.pageparams.RemoveAt(t1);
				mod.pageparams.Insert(t2, mt1);
				EditorUtility.SetDirty(mod);	//target);
			}
		}

		static bool CheckTextureReadable(Texture2D texture)
		{
			if ( texture != null )
			{
				string texturePath = AssetDatabase.GetAssetPath(texture);
				TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(texturePath);
				if ( textureImporter.isReadable )
					return true;
				else
				{
					EditorUtility.DisplayDialog("Texture Select", "Texture is not set to Readable", "OK");
					return false;
				}
			}

			return true;
		}

		static void SetUpdate(MegaBookBuilder mod, ref bool update)
		{
			if ( GUI.changed )
			{
				update = true;
				GUI.changed = false;
				//EditorUtil.SetDirty(target);
				EditorUtility.SetDirty(mod);	//target);
			}
		}

#if false
		int ToolbarWrapped(int selected, string[] texts)
		{
			int longest = 0;

			for( int i = 1; i < texts.Length; i++)
			{
				if ( texts[i].Length > texts[longest].Length )
					longest = i;
			}

			float textWidth = GUI.skin.button.CalcSize(new GUIContent(texts[longest])).x;
			Debug.Log("width " + textWidth);
			int paddingWidth = GUI.skin.button.padding.left + GUI.skin.button.padding.right;
			RectOffset margin = GUI.skin.button.margin;
			//int xCount = (int)(GUILayoutUtility.GetLastRect().width / (textWidth + paddingWidth + margin.horizontal));
			int xCount = (int)(Screen.width / (textWidth));	// + paddingWidth));	// + margin.horizontal));
			Debug.Log("xcount " + xCount);
			return GUILayout.SelectionGrid(selected, texts, xCount);
		}
#endif
		static bool update = false;
		static bool updatemesh = false;

		public override void OnInspectorGUI()
		{
			MegaBookBuilder mod = (MegaBookBuilder)target;

			DisplayInspector(mod);
		}

		static Texture logoImage;

		static public void DisplayInspector(MegaBookBuilder mod)
		{
			if ( logoImage == null )
				logoImage = (Texture)Resources.Load<Texture>("Editor/logo" + Random.Range(0, 3));   // + ".png", typeof(Texture));

			if ( logoImage )
			{
				if ( Screen.width != 0 && logoImage.width != 0 )
				{
					float h = (float)logoImage.height / ((float)logoImage.width / (float)Screen.width);
					GUILayout.Box(logoImage, GUILayout.Width(Screen.width), GUILayout.Height(h));
				}
			}

			MegaBookGUI.undoEnable = true;

			if ( coverStyles.Count == 0 )
				CreateCoverStyles();

			update = false;
			updatemesh = false;

			if ( Event.current.type == EventType.ExecuteCommand && Event.current.commandName != null )
			{
				if ( Event.current.commandName == "UndoRedoPerformed")
				{
					mod.rebuild = true;
					mod.updateBindings = true;
					//EditorUtility.SetDirty(mod);
#if false
					if ( mod.headband1 )
						EditorUtility.SetDirty(mod.headband1);

					if ( mod.headband2 )
						EditorUtility.SetDirty(mod.headband2);

					if ( mod.spineFabric )
						EditorUtility.SetDirty(mod.spineFabric);
#endif
				}
			}

			bool isPrefab = PrefabUtility.IsPartOfAnyPrefab(mod.gameObject);

			MegaBookGUI.FoldOut(ref mod.showgeneral, "General", "General Options for the book such as reading and building options");

			if ( mod.showgeneral )
			{
				EditorGUILayout.BeginVertical("box");
				MegaBookGUI.Toggle(mod, "Use Undo", ref mod.useUndo, "The Unity Undo system can really slow the editor down, by default Undo is turned off for params that cause a mesh rebuild, check this to Undo on such params.");
				MegaBookGUI.Slider(mod, "Page", ref mod.page, mod.MinPageVal(), mod.MaxPageVal(), "The current page looking at in the book");

				if ( mod.page >= 0.0f )
				{
					float turn = mod.page % 1.0f;
					turn = Mathf.Clamp(turn, 0.0f, 0.999f);
					MegaBookGUI.Slider(mod, "Turn", ref turn, 0.0f, 0.999f, "Helper slider to turn just the current page for testing");
					mod.page = Mathf.Floor(mod.page) + turn;
				}
				else
				{
					float turn = 0.0f;
					MegaBookGUI.Slider(mod, "Turn", ref turn, 0.0f, 0.999f, "Helper slider to turn just the current page for testing");
				}

				if ( GUI.changed )
				{
					if ( mod.linkeditpage )
					{
						mod.editpage = (int)(mod.page + 0.5f);
						mod.editpage = Mathf.Clamp(mod.editpage, 0, mod.pageparams.Count - 1);
					}

					mod.Flip = mod.page;
					mod.turnspd = 0.0f;
					EditorUtility.SetDirty(mod);	//target);
					GUI.changed = false;
				}

				MegaBookGUI.Toggle(mod, "Auto Disable", ref mod.autoDisable, "Book will automatically detect if it needs to deform anything, if not will turn Off page turners.");
				MegaBookGUI.Toggle(mod, "Dont Build On Play", ref mod.dontBuildOnPlay, "Dont rebuild the book when running the scene");
				MegaBookGUI.Toggle(mod, "Link Edit Page", ref mod.linkeditpage, "Automatically changes the Edit Page value to match the current page");
				MegaBookGUI.Toggle(mod, "Snap", ref mod.Snap, "Should the page turn snap to a flat page or be free");
				MegaBookGUI.Float(mod, "Turn Time", ref mod.turntime, "How long it takes to turn a page");
				if ( GUI.changed )
				{
					mod.turnspd = 0.0f;
				}

				if ( isPrefab )
				{
					EditorGUILayout.HelpBox("Book is currently a Prefab. Please Unpack the Object to allow for full editing of the Book. If Book is a prefab it will not rebuild.", MessageType.Info);
				}

				//if ( !isPrefab )
				{
					MegaBookGUI.Int(mod, "Num Pages", ref mod.NumPages, "How many pages to be in the book");

					if ( mod.NumPages < 2 )
						mod.NumPages = 2;

					SetUpdate(mod, ref updatemesh);
				}

				MegaBookGUI.Text(mod, "Shader Texture Name Override", ref mod.shaderTextureName);
				EditorGUILayout.HelpBox("Book will use '" + mod.shaderTexture + "' for setting textures. Use the Override value above if your shaders use a different name for the Texture Param.", MessageType.Info);
				//MegaBookGUI.Text(mod, "Shader Texture Name", ref mod.shaderTexture);
				EditorGUILayout.EndVertical();
			}

			//if ( !isPrefab )
			{
				MegaBookGUI.FoldOut(ref mod.showmeshoptions, "Mesh Options", "Show the options for how the book looks, mesh options, textures etc.");
				if ( mod.showmeshoptions )
					DisplayMeshOptions(mod);
			}
			
			MegaBookGUI.FoldOut(ref mod.showflipoptions, "Page Turn Options", "Show the options that control the animation of the page turning.");
			if ( mod.showflipoptions )
				DisplayFlipOptions(mod);

			if ( mod.pageparams.Count > 1 )
			{
				MegaBookGUI.IntSlider(mod, "Edit Page", ref mod.editpage, 0, mod.pageparams.Count - 1, "The page to edit");
				if ( GUI.changed )
				{
					SceneView.RepaintAll();
					//EditorUtility.SetDirty(mod);
					GUI.changed = false;
				}
			}

			MegaBookGUI.Toggle(mod, "Highlight Page being Edited", ref mod.showPageMesh, "Will highlight the page being edited");
			if ( GUI.changed )
			{
				SceneView.RepaintAll();
				//EditorUtility.SetDirty(mod);
				GUI.changed = false;
			}
			MegaBookGUI.ColorField(mod, "Highlight Color", ref mod.highlightColor, "You can change the color used to highlight the page being edited");
			if ( GUI.changed )
			{
				SceneView.RepaintAll();
				//EditorUtility.SetDirty(mod);
				GUI.changed = false;
			}
			GUI.changed = false;

			//if ( !isPrefab )
			{
				MegaBookGUI.FoldOut(ref mod.showmaterials, "Page Params", "Show the Page Content params, such as page textures, mesh content, videos, as well as other overrides.");

				if ( mod.showmaterials && mod.pageparams.Count > 0 )
				{
					if ( GUILayout.Button(new GUIContent("Randomize Content", "Will find all different page textures in the current book and randomly set them to pages")) )
					{
						List<Texture2D> textures = new List<Texture2D>();
						for ( int i = 0; i < mod.pageparams.Count; i++ )
						{
							if ( mod.pageparams[i].front )
							{
								if ( !textures.Contains(mod.pageparams[i].front) )
									textures.Add(mod.pageparams[i].front);
							}

							if ( mod.pageparams[i].back )
							{
								if ( !textures.Contains(mod.pageparams[i].back) )
									textures.Add(mod.pageparams[i].back);
							}
						}

						if ( textures.Count > 1 )
						{
							Random.InitState(Random.Range(0, 32000));
							for ( int i = 0; i < mod.pageparams.Count; i++ )
							{
								mod.pageparams[i].front = textures[Random.Range(0, textures.Count)];

								while ( true )
								{
									Texture2D t = textures[Random.Range(0, textures.Count)];
									//if ( t != mod.pageparams[i].front )
									{
										mod.pageparams[i].back = t;
										break;
									}
								}
							}
						}
					}

					if ( GUILayout.Button(new GUIContent("Remove Unused Pages", "Removes any Page Params that are not used by the book, this might happen if you reduced the number of pages in a book")) )
					{
						int count = mod.pageparams.Count - mod.NumPages;
						for ( int i = 0; i < count; i++ )
						{
							mod.pageparams.RemoveAt(mod.pageparams.Count - 1);
						}
					}

					if ( GUILayout.Button(new GUIContent("Add All Pages", "Adds and missing Page Params to the book, this might happen if you increase the number of pages in the book.")) )
					{
						for ( int i = mod.pageparams.Count; i < mod.NumPages; i++ )
						{
							mod.pageparams.Add(NewPage(mod, mod.pageparams.Count - 1));
						}
					}

					if ( GUILayout.Button(new GUIContent("Add Page", "Add a new pageparam to the book")) )
						mod.pageparams.Add(NewPage(mod, mod.pageparams.Count - 1));

					EditorGUILayout.BeginVertical("box");
					if ( DisplayPage(mod, mod.pageparams[mod.editpage], mod.editpage) )
					{
						updatemesh = true;
					}

					EditorGUILayout.EndVertical();

					if ( GUILayout.Button(new GUIContent("Add Page", "Add a new pageparam to the book")) )
						mod.pageparams.Add(NewPage(mod, mod.pageparams.Count - 1));
				}

				if ( mod.pageparams.Count > 0 )
				{
					MegaBookGUI.FoldOut(ref mod.showobjects, "Page Objects", "Show the any objects that are attached to the page and the params that control their visibility, position etc.");

					if ( DisplayObjects(mod, mod.pageparams[mod.editpage], mod.editpage) )
						updatemesh = true;
				}

				MegaBookGUI.FoldOut(ref mod.showbackgrounds, "Texture Backgrounds", "Show the global page texture backgrounds and masks.");

				if ( mod.showbackgrounds )
					DisplayTextureBackgroundOptions(mod);

#if false
				MegaBookGUI.FoldOut(ref mod.showdynammesh, "Dynamic Mesh");

				if ( mod.showdynammesh )
					DisplayDynamMeshOptions(mod);
#endif
				DisplayBindings(mod);

				MegaBookGUI.FoldOut(ref mod.showcover, "Cover Params", "Options for the Book cover.");
				if ( mod.showcover )
					DisplayCoverOptions(mod);
			}

			if ( updatemesh )
			{
				if ( !isPrefab )
				{
					mod.rebuildmeshes = true;
					//mod.UpdateSettings();
					//mod.Detach();
					//mod.BuildPageMeshes();
					//mod.Attach();
					//mod.UpdateSettings();
				}
			}

			// debug
			MegaBookGUI.FoldOut(ref mod.showcurves, "Curves", "Shows the various curves created for the animation");
			if ( mod.showcurves )
				DisplayCurveOptions(mod);

			//if ( !isPrefab )
			{
				if ( MegaBookGUI.BigButton("Full Rebuild", "If the Book for anyreason gets messed up or something seems to be missing from the book Click this to do a full Rebuild.") )
				{
					if ( !isPrefab )
					{
						updatemesh = true;
						mod.rebuild = true;
					}
					EditorUtility.SetDirty(mod);	//target);
				}
			}

			if ( update )
			{
				if ( !isPrefab )
				{
					mod.UpdateSettings();
					mod.rebuild = true;
				}
			}

			if ( (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed") )
			{
				if ( !isPrefab )
				{
					mod.UpdateSettings();
					mod.rebuild = true;
					mod.updateBindings = true;

				}
				EditorUtility.SetDirty(mod);	//target);
			}

			if ( GUI.changed )
			{
				if ( mod.pages.Count > 0 )
					mod.pages[0].deform = true;

				EditorUtility.SetDirty(mod);	//target);
			}

			MegaBookGUI.undoEnable = false;
		}

		public static MegaBookPageParams NewPage(MegaBookBuilder book, int prev)
		{
			MegaBookPageParams page = new MegaBookPageParams();

			page.frontLayerID = LayerID.Layer1;
			page.backLayerID = LayerID.Layer1;

			if ( prev >= 0 && prev < book.pageparams.Count )
			{
				MegaBookPageParams pp = book.pageparams[prev];

				page.back					= pp.back;
				page.front					= pp.front;
				page.background				= pp.background;
				page.background1			= pp.background1;
				page.backmat				= pp.backmat;
				page.frontmat				= pp.frontmat;
				page.edgemat				= pp.edgemat;
				page.copyarea				= pp.copyarea;
				page.copyarea1				= pp.copyarea1;
				page.usebackground			= pp.usebackground;
				page.usebackground1			= pp.usebackground1;
				page.pagemesh				= pp.pagemesh;
				page.pageobj				= pp.pageobj;
				page.rotate					= pp.rotate;
				page.pivot					= pp.pivot;
				page.frontMask				= pp.frontMask;
				page.backMask				= pp.backMask;
				page.swapsides				= pp.swapsides;
				page.stiff					= pp.stiff;
				page.visobjlow				= pp.visobjlow;
				page.visobjhigh				= pp.visobjhigh;
				page.alphatexturefront		= pp.alphatexturefront;
				page.alphatextureback		= pp.alphatextureback;
				page.useFrontColor			= pp.useFrontColor;
				page.frontColor				= pp.frontColor;
				page.useBackColor			= pp.useBackColor;
				page.backColor				= pp.backColor;
				page.scale					= pp.scale;
				page.videoFrontClip			= pp.videoFrontClip;
				page.videoBackClip			= pp.videoBackClip;
				page.videoFrontVol			= pp.videoFrontVol;
				page.videoBackVol			= pp.videoBackVol;
				page.videoFrontPlayVis		= pp.videoFrontPlayVis;
				page.videoBackPlayVis		= pp.videoBackPlayVis;
				page.videoFrontReduceSize	= pp.videoFrontReduceSize;
				page.videoBackReduceSize	= pp.videoBackReduceSize;
				page.videoFrontMatID		= pp.videoFrontMatID;
				page.videoBackMatID			= pp.videoBackMatID;
				page.meshFront				= pp.meshFront;
				page.meshFrontRoot			= pp.meshFrontRoot;
				page.meshFrontOffset		= pp.meshFrontOffset;
				page.meshFrontScale			= pp.meshFrontScale;
				page.meshFrontRot			= pp.meshFrontRot;
				page.meshBack				= pp.meshBack;
				page.meshBackRoot			= pp.meshBackRoot;
				page.meshBackOffset			= pp.meshBackOffset;
				page.meshBackScale			= pp.meshBackScale;
				page.meshBackRot			= pp.meshBackRot;
				page.widthSegs				= pp.widthSegs;
				page.lengthSegs				= pp.lengthSegs;
				page.colliderOverride		= pp.colliderOverride;
				page.frontTMProSubdiv		= pp.frontTMProSubdiv;
				page.backTMProSubdiv		= pp.backTMProSubdiv;
				page.frontLayerID			= pp.frontLayerID;
				page.backLayerID			= pp.backLayerID;
			}

			return page;
		}

		static MegaBookPageObject copyobj = null;

		// Have a copy and paste so can easily setup objects with same values then can swap out object
		static bool DisplayObjects(MegaBookBuilder mod, MegaBookPageParams page, int i)
		{
			bool retval = false;

			page.objectIndex = Mathf.Clamp(page.objectIndex, 0, page.objects.Count - 1);

			if ( mod.showobjects )
			{
				EditorGUILayout.BeginVertical("box");

				if ( mod.page >= 0.0f )
				{
					float turn = mod.page % 1.0f;
					turn = Mathf.Clamp(turn, 0.0f, 0.999f);
					MegaBookGUI.Slider(mod, "Turn", ref turn, 0.0001f, 0.999f, "Helper slider to turn just the current page for testing");
					mod.page = Mathf.Floor(mod.page) + turn;
					GUI.changed = false;
					EditorUtility.SetDirty(mod);
				}
				else
				{
					float turn = 0.0f;
					MegaBookGUI.Slider(mod, "Turn", ref turn, 0.0f, 0.999f, "Helper slider to turn just the current page for testing");
				}

				MegaBookGUI.Float(mod, "Visible Low", ref page.visobjlow, "Default page turn value object will appear");
				MegaBookGUI.Float(mod, "Visible High", ref page.visobjhigh, "Default page turn value object will vanish");

				if ( GUILayout.Button(new GUIContent("Add Object", "Attach a GameObject to this page")) )
				{
					MegaBookPageObject pobj = new MegaBookPageObject();
					pobj.layerId = LayerID.Layer1;

					pobj.pos = new Vector3(50.0f, 0.0f, 50.0f);

					if ( page.objects.Count > 0 )
					{
						MegaBookPageObject prevobj = page.objects[page.objects.Count - 1];
						pobj.pos = prevobj.pos;
						pobj.rot = prevobj.rot;
						pobj.offset = prevobj.offset;
					}
					page.objects.Add(pobj);
					page.objectIndex = page.objects.Count - 1;
					if ( page.objectIndex > 0 )
					{
						CopyObject(page.objects[page.objectIndex - 1], pobj);
					}

					retval = true;
					mod.rebuild = true;
					GUI.changed = false;
					EditorUtility.SetDirty(mod);	//target);
				}

				if ( page.objects.Count > 0 )
				{
					if ( page.objects.Count > 1 )
					{
						MegaBookGUI.IntSlider(mod, "Object Index", ref page.objectIndex, 0, page.objects.Count - 1);
						page.objectIndex = Mathf.Clamp(page.objectIndex, 0, page.objects.Count - 1);
					}

					EditorGUILayout.BeginVertical("box");
					int j = page.objectIndex;
					{
						MegaBookPageObject pobj = page.objects[j];

						EditorGUILayout.HelpBox("Alpha: " + pobj.alpha.ToString("0.000") + " Appear: " + pobj.appearAlpha.ToString("0.000") + " Vanish: " + pobj.vanishAlpha.ToString("0.000"), MessageType.Info);
						
						MegaBookGUI.Toggle(mod, "Active", ref pobj.active, "Whether the object is being controlled by the book, uncheck if you want to control the object yourself");
						MegaBookGUI.LayerID(mod, "LayerId", ref pobj.layerId, "Attached objects can be on layers, select the layer here and if the page layer allows the objects will be controlled, else it will be hidden");
						GameObject newobj = MegaBookGUI.GameObject(mod, "Object", pobj.obj, true, "Gameobject that will appear on this page");

						if ( newobj != pobj.obj )
						{
							if ( pobj.obj == null )
							{
								pobj.pos = mod.CalcPos(newobj, page, pobj.pos);
							}
							else
							{
								mod.FirstAttachObject(mod.pages[mod.editpage], pobj);
							}

							pobj.obj = newobj;
							if ( newobj )
							{
								pobj.scale = newobj.transform.localScale;
								pobj.appearScale = newobj.transform.localScale;
								pobj.vanishScale = newobj.transform.localScale;
							}

							retval = true;
							GUI.changed = false;
						}

						MegaBookGUI.Toggle(mod, "Back of Page", ref pobj.backOfPage, "Check this if object is on the back of the page");

						MegaBookGUI.Vector3(mod, "Pos", ref pobj.pos, "Position of the object on the page");
						pobj.offset = MegaBookGUI.Float(mod, "Offset", pobj.offset * 100.0f, "How far the object is off the page surface") / 100.0f;
						MegaBookGUI.Vector3(mod, "Rot", ref pobj.rot, "Rotation of the object on the page");
						MegaBookGUI.Vector3(mod, "Scale", ref pobj.scale, "Scale of the object as it appears on the page");
						MegaBookGUI.Slider(mod, "Fade", ref pobj.fade, 0.0f, 1.0f, "Used if Canvas Group present to fade UI in and out");
						MegaBookGUI.Vector3(mod, "Fwd", ref pobj.attachforward, "Helper forward direction for calculating the rotation of the object as the page deforms");
						MegaBookGUI.BeginToggle(mod, "Use Obj Visi", ref pobj.overridevisi);

						EditorGUILayout.BeginHorizontal();
						MegaBookGUI.Float(mod, "Object Visibile At", ref pobj.visilow, "At what point in the page turn does the object get turned on");
						if ( GUILayout.Button("Set", GUILayout.Width(64.0f)) )
						{
							Undo.RecordObject(mod, "Set Visi Value");
							pobj.visilow = -1.0f + (mod.page % 1.0f);
							if ( pobj.backOfPage )
								pobj.visilow += 1.0f;
							if ( mod.editpage == 0 )
								pobj.visilow += 1.0f;
						}
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						MegaBookGUI.Float(mod, "Object Not Visible At", ref pobj.visihigh, "Point in the page turn that the object gets turned off. Use Set button to current page turn value.");
						if ( GUILayout.Button("Set", GUILayout.Width(64.0f)) )
						{
							Undo.RecordObject(mod, "Set Visi Value");
							pobj.visihigh = (mod.page % 1.0f);
							if ( pobj.backOfPage )
								pobj.visihigh += 1.0f;
							//if ( mod.editpage == 0 )
								//pobj.visihigh += 1.0f;
						}
						EditorGUILayout.EndHorizontal();

						MegaBookGUI.BeginToggle(mod, "Animate Appear/Vanish", ref pobj.scaleObj, "Does the object scale/rotate/translate as is appears and vanishes");
						EditorGUILayout.BeginHorizontal();
						MegaBookGUI.Float(mod, "Object Fully Visible At", ref pobj.visiScaleLow, "Point in Page turn that the object is at full appear scale and or rotation");
						if ( GUILayout.Button("Set", GUILayout.Width(64.0f)) )
						{
							Undo.RecordObject(mod, "Set Visi Value");
							pobj.visiScaleLow = -1.0f + (mod.page % 1.0f);
							if ( pobj.backOfPage )
								pobj.visiScaleLow += 1.0f;
							if ( mod.editpage == 0 )
								pobj.visiScaleLow += 1.0f;
						}
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						MegaBookGUI.Float(mod, "Object Starts Vanishing At", ref pobj.visiScaleHigh, "Point in Page turn that the object starts to use the Vanish scale and rotation");
						if ( GUILayout.Button("Set", GUILayout.Width(64.0f)) )
						{
							Undo.RecordObject(mod, "Set Visi Value");
							pobj.visiScaleHigh = (mod.page % 1.0f);
							if ( pobj.backOfPage )
								pobj.visiScaleHigh += 1.0f;
							//if ( mod.editpage == 0 )
								//pobj.visiScaleHigh += 1.0f;
						}
						EditorGUILayout.EndHorizontal();
						MegaBookGUI.Vector3(mod, "Appear Scale", ref pobj.appearScale, "The scale of the object when it first appears, will increase to 'Scale' as page turns");
						//MegaBookGUI.Vector3(mod, "Scale", ref pobj.scale, "Scale of the object as it appears on the page");
						MegaBookGUI.Vector3(mod, "Vanish Scale", ref pobj.vanishScale, "The scale the object when the object completes it Vanish");

						MegaBookGUI.Vector3(mod, "Appear Rot", ref pobj.appearRot, "The rotation of the object when it first appears");
						MegaBookGUI.Vector3(mod, "Vanish Rot", ref pobj.vanishRot, "The rotation of the object when it vanishes");

						MegaBookGUI.Vector3(mod, "Appear Pos", ref pobj.appearPos, "The position of the object when it first appears");
						MegaBookGUI.Vector3(mod, "Vanish Pos", ref pobj.vanishPos, "The position of the object when it vanishes");

						MegaBookGUI.Curve(mod, "Appear Curve", ref pobj.appearCrv, "Curve to control the appearing scaling and rotation");
						MegaBookGUI.Curve(mod, "Vanish Curve", ref pobj.vanishCrv, "Curve to control the vanishing scaling and rotation");

						MegaBookGUI.Curve(mod, "Offset Curve", ref pobj.offsetCrv, "Curve to control the offset of the object during the page turn");

						MegaBookGUI.Slider(mod, "Appear Fade", ref pobj.appearFade, 0.0f, 1.0f, "The Fade value for when a Canvas Group first appears");
						MegaBookGUI.Slider(mod, "Vanish Fade", ref pobj.vanishFade, 0.0f, 1.0f, "The Fade value for when a Canvas Group vanishes");
						MegaBookGUI.Curve(mod, "Appear Fade Curve", ref pobj.fadeAppearCrv, "Curve to control the fade of the object during the page turn");
						MegaBookGUI.Curve(mod, "Vanish Fade Curve", ref pobj.fadeVanishCrv, "Curve to control the fade of the object during the page turn");

						EditorGUILayout.EndToggleGroup();

						EditorGUILayout.EndToggleGroup();

						pobj.message = MegaBookGUI.Toggle(mod, "Message", pobj.message, "Send messages to the GameObject");

						EditorGUILayout.BeginHorizontal();

						if ( mod.editpage > 0 )
							GUI.enabled = true;
						else
							GUI.enabled = false;

						if ( GUILayout.Button(new GUIContent("Copy To Prev", "Copy this object to the Previous Page")) )
						{
							MegaBookPageObject newpobj = new MegaBookPageObject();
							CopyObject(pobj, newpobj);
							mod.pageparams[mod.editpage - 1].objects.Add(newpobj);
							retval = true;
							mod.rebuild = true;
							GUI.changed = false;
							EditorUtility.SetDirty(mod);    //target);
						}

						if ( GUILayout.Button(new GUIContent("Move To Prev", "Remove the object from this page and add it to the previous Page.")) )
						{
							MegaBookPageObject newpobj = new MegaBookPageObject();
							CopyObject(pobj, newpobj);
							mod.pageparams[mod.editpage - 1].objects.Add(newpobj);
							page.objects.RemoveAt(j);
							retval = true;
							mod.rebuild = true;
							GUI.changed = false;
							EditorUtility.SetDirty(mod);    //target);
						}

						if ( mod.editpage < mod.pageparams.Count - 2 )
							GUI.enabled = true;
						else
							GUI.enabled = false;

						if ( GUILayout.Button(new GUIContent("Move To Next", "Remove the object from this page and add it to the Next Page.")) )
						{
							MegaBookPageObject newpobj = new MegaBookPageObject();
							CopyObject(pobj, newpobj);
							mod.pageparams[mod.editpage + 1].objects.Add(newpobj);
							page.objects.RemoveAt(j);
							retval = true;
							mod.rebuild = true;
							GUI.changed = false;
							EditorUtility.SetDirty(mod);    //target);
						}

						if ( GUILayout.Button(new GUIContent("Copy To Next", "Copy this object to the Next Page")) )
						{
							MegaBookPageObject newpobj = new MegaBookPageObject();
							CopyObject(pobj, newpobj);
							mod.pageparams[mod.editpage + 1].objects.Add(newpobj);
							retval = true;
							mod.rebuild = true;
							GUI.changed = false;
							EditorUtility.SetDirty(mod);    //target);
						}

						EditorGUILayout.EndHorizontal();

						GUI.enabled = true;

						EditorGUILayout.BeginHorizontal();
						if ( GUILayout.Button(new GUIContent("Delete Page Object", "Remove this object from the page.")) )
						{
							page.objects.RemoveAt(j);
							retval = true;
							mod.rebuild = true;
							GUI.changed = false;
							EditorUtility.SetDirty(mod);	//target);
						}

						EditorGUILayout.BeginHorizontal();
						if ( GUILayout.Button(new GUIContent("Copy", "Copy this objects params to a buffer so can be pasted onto another page")) )
						{
							copyobj = pobj;
						}

						if ( copyobj == null )
							GUI.enabled = false;

						if ( GUILayout.Button(new GUIContent("Paste", "If the copy buffer has an object in it it will be added to this page")) )
						{
							//copyobj = pobj;
							CopyObject(copyobj, pobj);
						}
						EditorGUILayout.EndHorizontal();

						GUI.enabled = true;

						EditorGUILayout.EndHorizontal();
					}
					EditorGUILayout.EndVertical();
				}

				EditorGUILayout.EndVertical();
				if ( GUI.changed )
				{
					mod.UpdateAttached();

					EditorUtility.SetDirty(mod);	//target);
				}
			}

			return retval;
		}

		static void CopyObject(MegaBookPageObject from, MegaBookPageObject to)
		{
			to.pos				= from.pos;
			to.rot				= from.rot;
			to.offset			= from.offset;
			to.obj				= from.obj;
			to.attached			= from.attached;
			to.attachforward	= from.attachforward;
			to.overridevisi		= from.overridevisi;
			to.visilow			= from.visilow;
			to.visihigh			= from.visihigh;
			to.message			= from.message;
			to.appearScale		= from.appearScale;
			to.vanishScale		= from.vanishScale;
			to.scale			= from.scale;
			to.visiScaleLow		= from.visiScaleLow;
			to.visiScaleHigh	= from.visiScaleHigh;
			to.scaleObj			= from.scaleObj;
			to.appearRot		= from.appearRot;
			to.vanishRot		= from.vanishRot;
			to.appearPos		= from.appearPos;
			to.vanishPos		= from.vanishPos;
			to.backOfPage		= from.backOfPage;
			to.appearCrv.keys	= from.appearCrv.keys;
			to.vanishCrv.keys	= from.vanishCrv.keys;
			to.offsetCrv.keys	= from.offsetCrv.keys;
			to.appearAlpha		= from.appearAlpha;
			to.vanishAlpha		= from.vanishAlpha;
			to.alpha			= from.alpha;
			to.fade				= from.fade;
			to.appearFade		= from.appearFade;
			to.vanishFade		= from.vanishFade;
			to.fadeAppearCrv.keys	= from.fadeAppearCrv.keys;
			to.fadeVanishCrv.keys	= from.fadeVanishCrv.keys;
			to.active			= from.active;
			to.layerId			= from.layerId;
	}

	static bool DisplayPage(MegaBookBuilder mod, MegaBookPageParams page, int i)
		{
			bool retval = false;

			MegaBookGUI.Int(mod, "Width Segs", ref page.widthSegs, "How many segs to across the width of the page, the more the smoother the turn but at a little CPU cost");
			SetUpdate(mod, ref retval);

			MegaBookGUI.Int(mod, "Length Segs", ref page.lengthSegs, "How many segs to use for the length of the page, not so many needed here");
			SetUpdate(mod, ref retval);

			EditorGUILayout.BeginHorizontal();
			MegaBookGUI.Mesh(mod, "Page Mesh", ref page.pagemesh, false, "Select a custom mesh to use for the page.");
			if ( GUILayout.Button(new GUIContent("-", "Remove Mesh"), GUILayout.Width(16)) )
			{
				MegaBookGUI.RecordObject(mod, "Removed Mesh");
				page.pagemesh = null;
			}
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndHorizontal();
			if ( page.pagemesh )
			{
				if ( page.pagemesh.subMeshCount < 3 )
					EditorGUILayout.HelpBox("Page Meshes require at least 3 materials. This mesh will not be used!", MessageType.Error);
				if ( !page.pagemesh.isReadable )
					EditorGUILayout.HelpBox("Meshes must be Readable, please change the Import settings to use this mesh!", MessageType.Warning);
			}

			EditorGUILayout.BeginHorizontal();
			MegaBookGUI.Mesh(mod, "Hole Mesh", ref page.holemesh, false, "Select a custom mesh to use for the page with a hole in");
			if ( GUILayout.Button(new GUIContent("-", "Remove Mesh"), GUILayout.Width(16)) )
			{
				MegaBookGUI.RecordObject(mod, "Removed Mesh");
				page.holemesh = null;
			}
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndHorizontal();

			MegaBookGUI.Vector3(mod, "Rotate", ref page.rotate, "It might be that your custom mesh is not in the right orientation for the system, if so you can alter that with this value.");
			SetUpdate(mod, ref retval);

			MegaBookGUI.Vector3(mod, "Pivot", ref page.pivot, "It might be that your custom mesh pivot needs adjusting, if so you can alter that with this value.");
			SetUpdate(mod, ref retval);

			MegaBookGUI.Float(mod, "Scale Mesh Width", ref page.scale, "If a custom mesh selected this value can be used to scale the width of it, useful for torn out page");
			SetUpdate(mod, ref retval);

			MegaBookGUI.ColliderOverride(mod, "Collider Option", ref page.colliderOverride, "Use this value to set the collider option for this page");
			SetUpdate(mod, ref retval);

#if false
			MegaBookGUI.GameObject(mod, "Page Object", ref page.pageobj, false, "Object whose mesh will be used as the page");
			SetUpdate(mod, ref retval);

			MegaBookGUI.GameObject(mod, "Hole Object", ref page.holeobj, false, "Object whos mesh will be used as a page with a hole in");
			SetUpdate(mod, ref retval);
#endif
			MegaBookGUI.Header("Front of Page");

			EditorGUILayout.BeginVertical("window");
			MegaBookGUI.LayerID(mod, "Front Layer ID", ref page.frontLayerID, "Use this to control which attached objects will be shown");
			SetUpdate(mod, ref retval);

			MegaBookGUI.Int(mod, "Front Mat Index", ref page.frontmatindex, "If using a custom object you can say which material index to use for the front.");
			SetUpdate(mod, ref retval);

			MegaBookGUI.Material(mod, "Front Material", ref page.frontmat, false, "The material to use for the front of each page. The texture will be changed per page.");
			SetUpdate(mod, ref retval);

			EditorGUILayout.BeginHorizontal();
			MegaBookGUI.Toggle(mod, "Use Front Color", ref page.useFrontColor, "Check to use the color for the material color for the front of the page.");
			SetUpdate(mod, ref retval);

			MegaBookGUI.ColorField(mod, "", ref page.frontColor, "Change the color of the front material.");
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal("box");

			EditorGUILayout.BeginVertical("box");
			MegaBookGUI.Label("Front", 80.0f, "Texture to use for the front of the page");
			Texture2D ftex = MegaBookGUI.Texture2D(mod, "", page.front, false, 64.0f);
			if ( ftex != page.front )
			{
				page.front = ftex;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				page.front = null;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			//SetUpdate(mod, ref retval);
			EditorGUILayout.EndVertical();


			EditorGUILayout.BeginVertical("box");
			MegaBookGUI.Label("Background", 80.0f, "Texture to use of the background of the front of the page, must be Readable");
			Texture2D ptex = MegaBookGUI.Texture2D(mod, "", page.background, false, 64.0f); //GUILayout.Width(64));

			if ( ptex != page.background && CheckTextureReadable(ptex) )
			{
				//page.texturesDirty = true;
				page.background = ptex;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				page.background = null;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}

			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical("box");
			MegaBookGUI.Label("Mask", 80.0f, "Texture to use as the mask for blending content to the background for the front of the page");
			Texture2D mtex = MegaBookGUI.Texture2D(mod, "", page.frontMask, false, 64.0f); //GUILayout.Width(64));

			if ( mtex != page.frontMask && CheckTextureReadable(mtex) )
			{
				//page.texturesDirty = true;
				page.frontMask = mtex;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}

			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				page.frontMask = null;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}

			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();
			if ( MegaBookGUI.Toggle(mod, "Use Alpha as Mask", ref page.alphatexturefront, "If set the alpha of the front texture will be used as mask in the texture creation so you can make holes etc.") )
			{
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			//SetUpdate(mod, ref page.texturesDirty);

			if ( MegaBookGUI.Toggle(mod, "Front Background", ref page.usebackground, "Use background texture for the fronts of pages") )
			{
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}

			page.copyarea = MegaBookGUI.Rect(mod, "Copy Area", page.copyarea, "Area to copy create the texture in for the front");
			SetUpdate(mod, ref page.texturesDirty);
			if ( page.texturesDirty )
			{
				GUI.color = Color.red;
				if ( MegaBookGUI.BigButton("Click to Update Textures", "If the Button is Red then the Textures need rebuilding. Click to have the textures rebuild for this page") )
				{
					mod.ClearMadeTextures(page);
					mod.BuildPageTextures(page);
					mod.BuildPageMeshes();
					GUI.changed = false;
				}
			}
			else
			{
				GUI.color = Color.white;
				if ( MegaBookGUI.BigButton("Update Textures", "Click to have the textures rebuild for this page") )
				{
					mod.ClearMadeTextures(page);
					mod.BuildPageTextures(page);
					mod.BuildPageMeshes();
					GUI.changed = false;
				}
			}
			GUI.color = Color.white;

			MegaBookGUI.Header("Front Video Content");
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.BeginHorizontal();
			page.videoFrontClip = (UnityEngine.Video.VideoClip)EditorGUILayout.ObjectField("Front Video", page.videoFrontClip, typeof(UnityEngine.Video.VideoClip), false);
			if ( GUILayout.Button(new GUIContent("-", "Remove Video"), GUILayout.Width(16)) )
			{
				MegaBookGUI.RecordObject(mod, "Removed Video");
				page.videoFrontClip = null;
			}
			EditorGUILayout.EndHorizontal();

			MegaBookGUI.Slider(mod, "Volume", ref page.videoFrontVol, 0.0f, 1.0f, "Volume for the Front Video");
			SetUpdate(mod, ref retval);
			MegaBookGUI.Toggle(mod, "Play When Visible", ref page.videoFrontPlayVis, "Check this to have video play only when chance it can be seen");
			SetUpdate(mod, ref retval);
			MegaBookGUI.VideoSize(mod, "Texture Size Reduce", ref page.videoFrontReduceSize, "Set how much the smaller the Render Texture is than the video");
			SetUpdate(mod, ref retval);
			MegaBookGUI.Int(mod, "Mat ID", ref page.videoFrontMatID, "Set the material id the video player uses, use -1 for book to decide.");
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndVertical();

			MegaBookGUI.Header("Front Mesh Content");
			EditorGUILayout.BeginVertical("box");

			MegaBookGUI.Toggle(mod, "Use Front Mesh Content", ref page.meshFront, "Check this to add the mesh content to the front page");
			SetUpdate(mod, ref retval);
			
			EditorGUILayout.BeginHorizontal();
			MegaBookGUI.GameObject(mod, "Front Mesh Root", ref page.meshFrontRoot, true, "Select the root object of the mesh content to add to the front of the page.");
			if ( GUILayout.Button(new GUIContent("-", "Remove Object"), GUILayout.Width(16)) )
			{
				MegaBookGUI.RecordObject(mod, "Removed Object");
				page.meshFrontRoot = null;
			}
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndHorizontal();

			MegaBookGUI.Vector3(mod, "Front Mesh Offset", ref page.meshFrontOffset, "Use this value to change the position of the mesh content on the page.");
			SetUpdate(mod, ref retval);
			MegaBookGUI.Vector3(mod, "Front Mesh Rot", ref page.meshFrontRot, "Use this value to change the rotation of the mesh content on the page.");
			SetUpdate(mod, ref retval);
			MegaBookGUI.Vector3(mod, "Front Mesh Scale", ref page.meshFrontScale, "Use this value to change the scale of the mesh content on the page.");
			SetUpdate(mod, ref retval);
			MegaBookGUI.TMProSubdiv(mod, "Text Mesh Pro Subdiv", ref page.frontTMProSubdiv, "Use this value to subdivide value for this page for the TMPro mesh.");
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndVertical();

			// Back of page
			MegaBookGUI.Header("Back of Page");
			EditorGUILayout.BeginVertical("window");

			MegaBookGUI.LayerID(mod, "Back Layer ID", ref page.backLayerID, "Use this to control which attached objects will be shown");
			SetUpdate(mod, ref retval);

			MegaBookGUI.Int(mod, "Back Mat Index", ref page.backmatindex, "If using a custom object you can say which material index to use for the back.");
			SetUpdate(mod, ref retval);

			MegaBookGUI.Material(mod, "Back Material", ref page.backmat, false, "The material to use for the back of each page. The texture will be changed per page.");
			SetUpdate(mod, ref retval);

			EditorGUILayout.BeginHorizontal();
			MegaBookGUI.Toggle(mod, "Use Back Color", ref page.useBackColor, "Check to use the color for the material color for the back of the page.");
			SetUpdate(mod, ref retval);

			MegaBookGUI.ColorField(mod, "", ref page.backColor, "Change the color of the back material.");
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal("box");

			EditorGUILayout.BeginVertical("box");
			//EditorGUILayout.LabelField("Back", GUILayout.Width(80));
			MegaBookGUI.Label("Back", 80.0f, "Texture to use for the back of the page");

			Texture2D btex = MegaBookGUI.Texture2D(mod, "", page.back, false, 64.0f);
			if ( btex != page.back )
			{
				page.back = btex;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				page.back = null;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}

			//SetUpdate(mod, ref retval);
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical("box");
			//EditorGUILayout.LabelField("Background", GUILayout.Width(80));
			MegaBookGUI.Label("Background", 80.0f, "Texture to use of the background of the back of the page, must be Readable");

			ptex = MegaBookGUI.Texture2D(mod, "", page.background1, false, 64.0f);

			if ( ptex != page.background1 && CheckTextureReadable(ptex) )
			{
				//page.texturesDirty = true;
				page.background1 = ptex;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				page.background1 = null;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical("box");
			//EditorGUILayout.LabelField("Mask", GUILayout.Width(80));
			MegaBookGUI.Label("Mask", 80.0f, "Texture to use as the mask for blending content to the background for the back of the page");
			mtex = MegaBookGUI.Texture2D(mod, "", page.backMask, false, 64.0f); //GUILayout.Width(64));

			if ( mtex != page.backMask && CheckTextureReadable(mtex) )
			{
				//page.texturesDirty = true;
				page.backMask = mtex;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				page.backMask = null;
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();
			if ( MegaBookGUI.Toggle(mod, "Use Alpha as Mask", ref page.alphatextureback, "If set the alpha of the back texture will be used as mask in the texture creation so you can make holes etc.") )
			{
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			//SetUpdate(mod, ref page.texturesDirty);

			MegaBookGUI.Toggle(mod, "Swap Sides", ref page.swapsides, "Will swap the materials for the front and back of the page");

			// Should just update the page not the whole book
			MegaBookGUI.Toggle(mod, "Back Background", ref page.usebackground1, "Use background texture for the backs of pages");
			if ( GUI.changed )
			{
				mod.ClearMadeTextures(page);
				mod.BuildPageTextures(page);
				mod.BuildPageMeshes();
				GUI.changed = false;
			}
			page.copyarea1 = MegaBookGUI.Rect(mod, "Copy Area Back", page.copyarea1, "Area to copy create the texture in for the back");
			SetUpdate(mod, ref page.texturesDirty);

			if ( page.texturesDirty )
			{
				GUI.color = Color.red;
				if ( MegaBookGUI.BigButton("Click to Update Textures", "If the Button is Red then the Textures need rebuilding. Click to have the textures rebuild for this page") )
				{
					mod.ClearMadeTextures(page);
					mod.BuildPageTextures(page);
					mod.BuildPageMeshes();
					GUI.changed = false;
				}
			}
			else
			{
				GUI.color = Color.white;
				if ( MegaBookGUI.BigButton("Update Textures", "Click to have the textures rebuild for this page") )
				{
					mod.ClearMadeTextures(page);
					mod.BuildPageTextures(page);
					mod.BuildPageMeshes();
					GUI.changed = false;
				}
			}

			MegaBookGUI.Header("Back Video Content");
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.BeginHorizontal();
			page.videoBackClip = (UnityEngine.Video.VideoClip)EditorGUILayout.ObjectField("Back Video", page.videoBackClip, typeof(UnityEngine.Video.VideoClip), false);
			if ( GUILayout.Button(new GUIContent("-", "Remove Video"), GUILayout.Width(16)) )
			{
				MegaBookGUI.RecordObject(mod, "Removed Video");
				page.videoBackClip = null;
			}
			EditorGUILayout.EndHorizontal();

			MegaBookGUI.Slider(mod, "Volume", ref page.videoBackVol, 0.0f, 1.0f, "Volume for the Back Video");
			SetUpdate(mod, ref retval);
			MegaBookGUI.Toggle(mod, "Play When Visible", ref page.videoBackPlayVis, "Check this to have video play only when chance it can be seen");
			SetUpdate(mod, ref retval);
			MegaBookGUI.VideoSize(mod, "Texture Size Reduce", ref page.videoBackReduceSize, "Set how much the smaller the Render Texture is than the video");
			SetUpdate(mod, ref retval);
			MegaBookGUI.Int(mod, "Mat ID", ref page.videoBackMatID, "Set the material id the video player uses, use -1 for book to decide.");
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndVertical();

			MegaBookGUI.Header("Back Mesh Content");
			EditorGUILayout.BeginVertical("box");

			MegaBookGUI.Toggle(mod, "Use Back Mesh Content", ref page.meshBack, "Check this to add the mesh content to the front page");
			SetUpdate(mod, ref retval);

			EditorGUILayout.BeginHorizontal();
			MegaBookGUI.GameObject(mod, "Back Mesh Root", ref page.meshBackRoot, true, "Select the root object of the mesh content to add to the front of the page.");
			if ( GUILayout.Button(new GUIContent("-", "Remove Object"), GUILayout.Width(16)) )
			{
				MegaBookGUI.RecordObject(mod, "Removed Object");
				page.meshBackRoot = null;
			}
			SetUpdate(mod, ref retval);
			EditorGUILayout.EndHorizontal();

			MegaBookGUI.Vector3(mod, "Back Mesh Offset", ref page.meshBackOffset, "Use this value to change the position of the mesh content on the page.");
			SetUpdate(mod, ref retval);
			MegaBookGUI.Vector3(mod, "Back Mesh Rot", ref page.meshBackRot, "Use this value to change the rotation of the mesh content on the page.");
			SetUpdate(mod, ref retval);
			MegaBookGUI.Vector3(mod, "Back Mesh Scale", ref page.meshBackScale, "Use this value to change the scale of the mesh content on the page.");
			SetUpdate(mod, ref retval);
			MegaBookGUI.TMProSubdiv(mod, "Text Mesh Pro Subdiv", ref page.backTMProSubdiv, "Use this value to subdivide value for this page for the TMPro mesh.");
			SetUpdate(mod, ref retval);

			EditorGUILayout.EndVertical();

			EditorGUILayout.EndVertical();

			GUI.color = Color.white;
			// New mesh content system

			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button(new GUIContent("Insert", "Insert a new pageparams into the book")) )
			{
				mod.pageparams.Insert(i, NewPage(mod, i));
				retval = true;
			}

			if ( GUILayout.Button(new GUIContent("Delete", "Delete this pageparams from the book")) )
			{
				mod.pageparams.RemoveAt(i);
				retval = true;
			}

			if ( GUILayout.Button(new GUIContent("Up", "Move this pageparams one page towards the front of the book")) )
			{
				if ( i < mod.pages.Count - 1 )
					SwapPages(mod, i, i + 1);
				retval = true;
			}

			if ( GUILayout.Button(new GUIContent("Down", "Move this pageparams one page towards the back of the book")) )
			{
				if ( i > 0 )
					SwapPages(mod, i, i - 1);
				retval = true;
			}

			EditorGUILayout.EndHorizontal();

			return retval;
		}

		static void DisplayBindings(MegaBookBuilder book)
		{
			bool update = false;

			MegaBookGUI.FoldOut(ref book.showBindings, "Binding Objects", "The values that control the creation, position and look of the headband and spine fabric objects.");

			if ( book.showBindings )
			{
				EditorGUILayout.BeginVertical("box");
				MegaBookGUI.Toggle(book, "Add Headbands", ref book.addHeadband, "Add Headband objects to the page edges at the spine");
				SetUpdate(book, ref update);

				book.headbandLength = MegaBookGUI.Float(book, "Height", book.headbandLength * 100.0f, "Height of the headband relative to the height of the book.") / 100.0f;
				SetUpdate(book, ref update);

				book.headbandWidth = MegaBookGUI.Float(book, "Spacing", book.headbandWidth * 100.0f, "Spacing of the headbands relative to size of the page.") / 100.0f;
				SetUpdate(book, ref update);

				book.headbandRadius = MegaBookGUI.Float(book, "Radius", book.headbandRadius * 100.0f, "Radius of the headband.") / 100.0f;
				SetUpdate(book, ref update);

				MegaBookGUI.Int(book, "Segs", ref book.headbandSegs, "Number of segments to make the headband out off.");
				book.headbandSegs = Mathf.Clamp(book.headbandSegs, 1, 64);
				SetUpdate(book, ref update);

				MegaBookGUI.Int(book, "Sides", ref book.headbandSides, "Number of sides to the headband.");
				book.headbandSides = Mathf.Clamp(book.headbandSides, 3, 32);
				SetUpdate(book, ref update);

				book.headbandOffset = MegaBookGUI.Vector3(book, "Offset", book.headbandOffset * 100.0f, "Offset of the headbands from the book object.") / 100.0f;
				SetUpdate(book, ref update);

				MegaBookGUI.Material(book, "Headband Material", ref book.headbandMat, false, "Material to use for the headbands.");
				SetUpdate(book, ref update);

				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical("box");
				MegaBookGUI.Toggle(book, "Add Spine Fabric", ref book.addFabric, "Add Fabric to the spine to cover the page ends.");
				SetUpdate(book, ref update);

				book.fabricLength = MegaBookGUI.Float(book, "Height", book.fabricLength * 100.0f, "Height of the fabric relative to the height of the book.") / 100.0f;
				SetUpdate(book, ref update);

				book.fabricWidth = MegaBookGUI.Float(book, "Width", book.fabricWidth * 100.0f, "Width of the headband realtive to the page size.") / 100.0f;
				SetUpdate(book, ref update);

				book.fabricThickness = MegaBookGUI.Float(book, "Thickness", book.fabricThickness * 100.0f, "Thickness of fabric object.") / 100.0f;
				SetUpdate(book, ref update);

				MegaBookGUI.Int(book, "Segs", ref book.fabricSegs, "Number of segs to make the fabric out off.");
				book.fabricSegs = Mathf.Clamp(book.fabricSegs, 1, 64);
				SetUpdate(book, ref update);

				book.fabricOffset = MegaBookGUI.Vector3(book, "Offset", book.fabricOffset * 100.0f, "Offset of the frabric from the book object.") / 100.0f;
				SetUpdate(book, ref update);

				MegaBookGUI.Material(book, "Fabric Material", ref book.fabricMat, false, "Material to use for the fabric.");
				SetUpdate(book, ref update);

				book.updateBindings = update;

				EditorGUILayout.EndVertical();
			}
		}

		static void DisplayMeshOptions(MegaBookBuilder mod)
		{
			EditorGUILayout.BeginVertical("box");

			MegaBookGUI.undoEnable = mod.useUndo;

			MegaBookGUI.Float(mod, "Page Length", ref mod.pageLength, "Length of the pages to build");

			if ( mod.pageLength < 0.001f )
				mod.pageLength = 0.001f;
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Float(mod, "Page Width", ref mod.pageWidth, "The width of the pages to build");

			if ( mod.pageWidth < 0.001f )
				mod.pageWidth = 0.001f;
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Float(mod, "Page Height", ref mod.pageHeight, "The thickness of the pages to build");
			if ( mod.pageHeight < 0.0f )
				mod.pageHeight = 0.0f;
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Int(mod, "Width Segs", ref mod.WidthSegs, "How many segs to across the width of the page, the more the smoother the turn but at a little CPU cost");
			if ( mod.WidthSegs < 1 )
				mod.WidthSegs = 1;
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Int(mod, "Length Segs", ref mod.LengthSegs, "How many segs to use for the length of the page, not so many needed here");
			if ( mod.LengthSegs < 1 )
				mod.LengthSegs = 1;
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Int(mod, "Height Segs", ref mod.HeightSegs, "How many segs to use for the height of the page, 1 will normally suffice");
			if ( mod.HeightSegs < 1 )
				mod.HeightSegs = 1;
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Spine Edge", ref mod.spineedge, "Check this to turn of the page edges polys by the spine");
			SetUpdate(mod, ref updatemesh);

			// Noise
			MegaBookGUI.Header("Noise");
			EditorGUILayout.BeginVertical("box");
			MegaBookGUI.BeginToggle(mod, "Use Noise", ref mod.addNoise, "Use noise when making the page mesh, good for old books");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Vector3(mod, "Noise Strength", ref mod.noiseStrength, "Strength of the Noise to add in each direction");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Float(mod, "Noise Scale", ref mod.noiseScale, "Scale the noise amount added");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Float(mod, "Noise Phase", ref mod.noisePhase, "Moves the noise lookup for the flat plane of the page");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Noise Same", ref mod.noiseSame, "Check this to use the same noise on all the pages");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Float(mod, "Noise Scale Vert", ref mod.noiseScaleVert, "Scale the vert noise amount added");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Float(mod, "Noise Phase Vert", ref mod.noisePhaseVert, "Moves the noise lookup for the vertical plane of the page");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Noise Same Vert", ref mod.noiseSameVert, "Check this to use the same vertical noise on all the pages");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Curve(mod, "Noise X amount", ref mod.noiseCurveX, "Controls the amount of noise added in the X direction on distance from spine edge");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Curve(mod, "Noise Y amount", ref mod.noiseCurveY, "Controls the amount of noise added in the Y direction on distance from spine edge");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Use Noise Through Book curve", ref mod.useNoiseBook, "Use the noise through book curve.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Curve(mod, "Noise Through Book amount", ref mod.noiseBook, "Controls the amount of noise added through the book");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Apply Noise", ref mod.noiseApply, "Check this to apply noise while editing, can slow editing, enable as you need");
			SetUpdate(mod, ref updatemesh);

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndToggleGroup();

			MegaBookGUI.Toggle(mod, "Page Hole Mesh", ref mod.useholepage, "Make page meshes with holes in to reduce overdraw");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Int(mod, "X Hole", ref mod.xhole, "How many vertices in from each edge the hole will start");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Int(mod, "Y Hole", ref mod.yhole, "How many vertices in from the edge the hole will start");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Vector3(mod, "Variation", ref mod.pagesizevariation, "Adds variation to the page sizes for an older book look with uncut pages");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Curve(mod, "Vertex Dist", ref mod.pagesectioncrv, "This curve controls how the vertices are distributed across the width of the page.");
			SetUpdate(mod, ref updatemesh);

			EditorGUILayout.BeginHorizontal();
			MegaBookGUI.Mesh(mod, "Page Mesh", ref mod.pagemesh, true, "Tells the book to use a custom page mesh");
			if ( GUILayout.Button(new GUIContent("-", "Remove Mesh"), GUILayout.Width(16)) )
			{
				MegaBookGUI.RecordObject(mod, "Removed Mesh");
				mod.pagemesh = null;
			}
			SetUpdate(mod, ref updatemesh);
			EditorGUILayout.EndHorizontal();
			if ( mod.pagemesh )
			{
				if ( mod.pagemesh.subMeshCount < 3 )
					EditorGUILayout.HelpBox("Page Meshes require at least 3 materials. This mesh will not be used!", MessageType.Error);
				if ( !mod.pagemesh.isReadable )
					EditorGUILayout.HelpBox("Meshes must be Readable, please change the Import settings to use this mesh!", MessageType.Warning);
			}

			EditorGUILayout.BeginHorizontal();
			MegaBookGUI.Mesh(mod, "Hole Mesh", ref mod.holemesh, true, "Tells the book to use a custom mesh for the page with a hole in");
			if ( GUILayout.Button(new GUIContent("-", "Remove Mesh"), GUILayout.Width(16)) )
			{
				MegaBookGUI.RecordObject(mod, "Removed Mesh");
				mod.holemesh = null;
			}
			SetUpdate(mod, ref updatemesh);
			EditorGUILayout.EndHorizontal();

#if false
			MegaBookGUI.GameObject(mod, "Page Object", ref mod.pageobject, false, "Tells the book to use a custom page object");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.GameObject(mod, "Hole Object", ref mod.holeobject, false, "Tells the book to use a custom object for the page with a hole in");
			SetUpdate(mod, ref updatemesh);
#endif
			MegaBookGUI.Int(mod, "Front Mat ID", ref mod.frontmat, "If using a custom mesh you can say which material index to use for the front.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Int(mod, "Back Mat ID", ref mod.backmat, "If using a custom mesh you can say which material index to use for the back.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Vector3(mod, "Rotate", ref mod.rotate, "It might be that your custom mesh or object is not in the right orientation for the system, if so you can alter that with this value.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Material(mod, "Front Material", ref mod.basematerial, true, "The material to use for the front of each page. The texture will be changed per page.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Material(mod, "Back Material", ref mod.basematerial1, true, "The material to use for the back of each page. The texture will be changed per page.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Material(mod, "Edge Material", ref mod.basematerial2, true, "The material to use for the edge of the page.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Float(mod, "Edge UV Size", ref mod.edgeUVSize, "Scale the vertical UV for the page edges, useful if you have a page edge texture.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Float(mod, "Edge UV Offset", ref mod.edgeUVOff, "Offset added to the UV per page to move the edge UV through the edge texture.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.undoEnable = true;

			MegaBookGUI.Float(mod, "Page Gap", ref mod.pageGap, "The values controls the gap between each page in the book if the Use Book Thickness option is not selected.");
			SetUpdate(mod, ref update);

			MegaBookGUI.Slider(mod, "Spine Radius", ref mod.spineradius, -1.0f, 1.0f, "You can specify a radius for the spine if you dont want a flat edge.");
			SetUpdate(mod, ref update);

			MegaBookGUI.Curve(mod, "Spine Radius Curve", ref mod.spineCurve, "Curve to control the spine radius as the book is read.");
			SetUpdate(mod, ref update);

			MegaBookGUI.Toggle(mod, "Use Book Thickness", ref mod.usebookthickness, "If you select this option then the spacing of the pages will be calculated to fit in the thickness defined below.");
			SetUpdate(mod, ref update);

			MegaBookGUI.Float(mod, "Book Thickness", ref mod.bookthickness, "The thickness of the whole book to use when the Use Book Thickness option is selected.");
			SetUpdate(mod, ref update);

			MegaBookGUI.Toggle(mod, "Page Colliders", ref mod.updatecollider, "This option will generate a mesh collider for each page of the book.");
			SetUpdate(mod, ref update);

			MegaBookGUI.TMProSubdiv(mod, "Text Mesh Pro Subdiv", ref mod.TMProSubdiv, "Use this value to subdivide value for the book for the TMPro mesh.");
			SetUpdate(mod, ref update);

			MegaBookGUI.PhysicsMat(mod, "Physics Material", ref mod.physicsMat, false, "The physics material to apply to the Page Mesh Colliders.");
			SetUpdate(mod, ref update);

			MegaBookGUI.undoEnable = mod.useUndo;

			MegaBookGUI.Toggle(mod, "Use UV2", ref mod.useuv2, "Use UV2 instead of main UV channel");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.BeginToggle(mod, "Allow Color", ref mod.usecols, "When using dynamic mesh content your content may be using the vertex colors, if so you will need to check this book so that the book system knows to include vertex color in the page mesh data.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Use Mat Color", ref mod.useMatCol, "Check this to use the Color value below to override the Material Color for the pages.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.ColorField(mod, "Material Color", ref mod.matColor, "You can use this value to change the material color for the pages.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.ColorField(mod, "Color", ref mod.color, "You can use this value to set a tint to your pages if the vertex color option is enabled above.");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Header("Unity Mesh Settings");
			EditorGUILayout.BeginVertical("box");
			MegaBookGUI.Toggle(mod, "Cast Shadows", ref mod.castshadows, "Will the pages cast shadows");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Receive Shadows", ref mod.receiveshadows, "Will the pages receive shadows");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.RenderLayers(mod, "Render Layers Mask", ref mod.renderLayers, "Render Layers Mask for HDRP");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Use Light Probes", ref mod.uselightprobes, "Will the page meshes work with light probes");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Transform(mod, "Anchor Override", ref mod.anchorOverride, true, "Anchor override for the page mesh");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.ReflectionProbeUsage(mod, "Reflection Usage", ref mod.reflectionProbes, "Will the pages work with reflection probes");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.LightProbeUsage(mod, "Light Probe Usage", ref mod.lightProbeUsage, "Light Probe value");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.GameObject(mod, "Proxy Volume", ref mod.proxyVolume, true);
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.MotionVectorGenerationMode(mod, "Motion Vectors", ref mod.motionVectors, "Will the page mesh have motion vectors");
			SetUpdate(mod, ref updatemesh);

			MegaBookGUI.Toggle(mod, "Recalc Tangents", ref mod.recalcTangents, "Should the system calculate tangents for the pages");
			SetUpdate(mod, ref updatemesh);
			EditorGUILayout.EndVertical();

			MegaBookGUI.undoEnable = true;

			EditorGUILayout.EndToggleGroup();
			EditorGUILayout.EndVertical();
		}

		static void DisplayFlipOptions(MegaBookBuilder mod)
		{
			float val;
			bool bval;

			EditorGUILayout.BeginVertical("box");

			//MegaBookGUI.Float(mod, "Spine Angle", ref mod.BottomAngle, "Spine Angle of the book");
			MegaBookGUI.Float(mod, "Max Spine Angle", ref mod.BottomMaxAngle, "Max angle the spine will turn to");
			MegaBookGUI.Toggle(mod, "Change Spine Angle", ref mod.changespineangle, "Will the book change the spine angle as the book turns");

			MegaBookGUI.Slider(mod, "Turn Center", ref mod.Turn_CCenter, 0.0f, 1.0f, "Where across the page the centre of the turn will happen");
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Slider(mod, "Turn Size", mod.Turn_CArea, 0.01f, 1.0f, "Low value will give a flat book, high value will look leafed through");
			if ( val != mod.Turn_CArea )
				mod.Turn_CAreaChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Turn Max Angle", mod.Turn_maxAngle, "Angle of the page when fully turned");
			if ( val != mod.Turn_maxAngle )
				mod.Turn_maxAngleChange(val);
			SetUpdate(mod, ref update);

			MegaBookGUI.Curve(mod, "Turn Max Angle Curve", ref mod.Turn_maxAngleCrv, "Curve for Angle of the page when fully turned");
			if ( GUI.changed )
				mod.Turn_maxAngleChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Turn Min Angle", mod.Turn_minAngle, "Angle of the page when it is not turned at all");
			if ( val != mod.Turn_minAngle )
				mod.Turn_minAngleChange(val);
			SetUpdate(mod, ref update);

			MegaBookGUI.Curve(mod, "Turn Min Angle Curve", ref mod.Turn_minAngleCrv, "Curve for Angle of the page when not turned at all");
			if ( GUI.changed )
				mod.Turn_minAngleChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Spread First Page Right", mod.Turn_Spread, "Controls how much the pages spread apart as the book is read, it will use First and Last values to change the look as the book is read.");
			if ( val != mod.Turn_Spread )
				mod.Turn_SpreadChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Spread Last Page Right", mod.Turn_Spread1, "Controls how much the pages spread apart as the book is read, it will use First and Last values to change the look as the book is read.");
			if ( val != mod.Turn_Spread1 )
			{
				mod.Turn_Spread1 = val;
				mod.Turn_SpreadChange(mod.Turn_Spread);
			}
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Spread First Page Left", mod.Turn_SpreadRead, "Controls how much the pages spread apart as the book is read, this controls the spread for when the page has been turned");
			if ( val != mod.Turn_SpreadRead )
			{
				mod.Turn_SpreadRead = val;
				mod.Turn_SpreadChange(mod.Turn_Spread);
			}
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Spread Last Page Left", mod.Turn_SpreadRead1, "Controls how much the pages spread apart as the book is read, this controls the spread for end of the book.");
			if ( val != mod.Turn_SpreadRead1 )
			{
				mod.Turn_SpreadRead1 = val;
				mod.Turn_SpreadChange(mod.Turn_Spread);
			}
			SetUpdate(mod, ref update);

			MegaBookGUI.Curve(mod, "Spread Curve", ref mod.Turn_SpreadCrv, "This curve controls how the spread values above are interpolated between.");
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Slider(mod, "Open Origin", mod.Land_CCenter, 0.0f, 1.0f, "Point on the page where the animation will start");
			if ( val != mod.Land_CCenter )
				mod.Land_CCenterChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Slider(mod, "Open Size", mod.Land_CArea, 0.01f, 1.0f, "The size of the area effected by the open animation");
			if ( val != mod.Land_CArea )
				mod.Land_CAreaChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Open Max Angle", mod.Land_maxAngle, "The max angle the page will use when ending its turn");
			if ( val != mod.Land_maxAngle )
				mod.Land_MaxAngleChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Open Min Angle", mod.Land_minAngle, "The angle the page will use when starting the end of the turn");
			if ( val != mod.Land_minAngle )
				mod.Land_MinAngleChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Slider(mod, "Flip Origin", mod.Flex_CCenter, 0.0f, 1.0f, "Point on the page where the extra page flip animation will start");
			if ( val != mod.Flex_CCenter )
				mod.Flex_CCenterChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Slider(mod, "Flip Size", mod.Flex_CArea, 0.01f, 1.0f, "The area effected by the extra page flip animation");
			if ( val != mod.Flex_CArea )
				mod.Flex_CAreaChange(val);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Flip Max Angle", mod.Flex_MaxAngle, "The max angle the flip animation will apply. If this value is too large it could result in pages passing through one another as they turn.");
			if ( val != mod.Flex_MaxAngle )
				mod.Flex_MaxAngleChange(val);
			SetUpdate(mod, ref update);

			bval = MegaBookGUI.Toggle(mod, "Flip Random", mod.Flex_Random, "You can tell the system to use a different flip angle on each page in the book, check this option to turn that on.");
			if ( bval != mod.Flex_Random )
				mod.Flex_RandomChange(mod.Flex_RandomDegree, mod.Flex_RandomSeed, bval);
			SetUpdate(mod, ref update);

			int ival = MegaBookGUI.Int(mod, "Flip Random Seed", mod.Flex_RandomSeed, "The random number seed to use.");
			if ( ival != mod.Flex_RandomSeed )
				mod.Flex_RandomChange(mod.Flex_RandomDegree, ival, bval);
			SetUpdate(mod, ref update);

			val = MegaBookGUI.Float(mod, "Flip Random Ang", mod.Flex_RandomDegree, "The maximum Flip angle the random generator will use on a page.");
			if ( val != mod.Flex_RandomDegree )
				mod.Flex_RandomChange(val, mod.Flex_RandomSeed, mod.Flex_Random);
			SetUpdate(mod, ref update);

			EditorGUILayout.EndVertical();
		}

		static void DisplayCoverOptions(MegaBookBuilder mod)
		{
			bool chg = false;
			EditorGUILayout.BeginVertical("box");

			int cs = EditorGUILayout.Popup("Cover Style", 0, coverStyleNames);

			if ( cs > 0 )
			{
				if ( mod.bookCover )
					DestroyImmediate(mod.bookCover.gameObject);

				if ( cs > 1 )
				{
					GameObject newcov = Instantiate(coverStyles[cs - 2].gameObject);
					newcov.transform.SetParent(mod.transform.parent);
					newcov.transform.localPosition = Vector3.zero;
					newcov.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
					mod.bookCover = newcov.GetComponent<MegaBookCover>();
					mod.page = -1.0f;
				}
			}

			MegaBookGUI.BookCover(mod, "Cover Object", ref mod.bookCover, true, "Use a book cover object for the cover of the book");
			SetUpdate(mod, ref chg);
			mod.coverScale = MegaBookGUI.Vector3(mod, "Cover Scale", mod.coverScale * 100.0f, "Scale the Book cover object if attached.") / 100.0f;
			SetUpdate(mod, ref chg);

			MegaBookGUI.Toggle(mod, "Auto Size Cover", ref mod.autoFit, "Auto size the cover to fit the book");
			SetUpdate(mod, ref chg);
			mod.autoFitSize = MegaBookGUI.Vector3(mod, "Autofit Size", mod.autoFitSize * 100.0f, "If autofit used this can over or under size the cover.") / 100.0f;
			SetUpdate(mod, ref chg);

			mod.spineScale = MegaBookGUI.Float(mod, "Spine Scale", mod.spineScale * 100.0f, "Scaling for Spine Thickness only.") / 100.0f;
			SetUpdate(mod, ref chg);

			mod.coverOffset = MegaBookGUI.Vector3(mod, "Cover Offset", mod.coverOffset * 100.0f, "Repositions the cover on the book.") / 100.0f;
			SetUpdate(mod, ref chg);

			if ( !mod.bookCover )
			{
				MegaBookGUI.Transform(mod, "Front Cover", ref mod.frontcover, true, "The transform to use as the front cover of the book");
				SetUpdate(mod, ref chg);
				MegaBookGUI.Float(mod, "Front Ang", ref mod.frontang);
				SetUpdate(mod, ref chg);
				mod.frontpivot = MegaBookGUI.Vector3(mod, "Front Pivot", mod.frontpivot * 100.0f, "The pivot point for the front cover object") / 100.0f;
				SetUpdate(mod, ref chg);
				mod.frontoffset = MegaBookGUI.Vector3(mod, "Front Offset", mod.frontoffset * 100.0f) / 100.0f;
				SetUpdate(mod, ref chg);
				MegaBookGUI.Axis(mod, "Front Axis", ref mod.frontAxis, "The axis to rotate the front cover around");
				SetUpdate(mod, ref chg);
				MegaBookGUI.Transform(mod, "Back Cover", ref mod.backcover, true, "The transform to use as the back cover of the book");
				SetUpdate(mod, ref chg);
				MegaBookGUI.Float(mod, "Back Ang", ref mod.backang);
				SetUpdate(mod, ref chg);
				mod.backpivot = MegaBookGUI.Vector3(mod, "Back Pivot", mod.backpivot * 100.0f, "The pivot point for the back cover object") / 100.0f;
				SetUpdate(mod, ref chg);
				MegaBookGUI.Axis(mod, "Back Axis", ref mod.backAxis, "The axis to rotate the back cover around");
				SetUpdate(mod, ref chg);
				MegaBookGUI.Transform(mod, "Spine", ref mod.spineBone, true, "The transform to use as the spine of the book");
				SetUpdate(mod, ref chg);
				mod.spinepivot = MegaBookGUI.Vector3(mod, "Spine Pivot", mod.spinepivot * 100.0f, "The pivot point for the spine object") / 100.0f;
				SetUpdate(mod, ref chg);
				MegaBookGUI.Axis(mod, "Spine Axis", ref mod.spineAxis, "The axis to rotate the spine around");
				SetUpdate(mod, ref chg);
			}

			GUI.enabled = true;

			EditorGUILayout.EndVertical();
		}

		static void DisplayCurveOptions(MegaBookBuilder mod)
		{
			for ( int i = 0; i < mod.pages.Count; i++ )
				MegaBookGUI.Curve(mod, "Turn " + i, ref mod.pages[i].turnerfromcon);
		}

		static void DisplayTextureBackgroundOptions(MegaBookBuilder mod)
		{
			EditorGUILayout.BeginVertical("box");
			MegaBookGUI.Toggle(mod, "No Backgrounds", ref mod.nobackgrounds);
			SetUpdate(mod, ref update);

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginVertical("box");

			//EditorGUILayout.LabelField("Front", GUILayout.Width(80));
			MegaBookGUI.Label("Front", 80.0f, "Front of page Background texture");
			Texture2D tex = MegaBookGUI.Texture2D(mod, "", mod.background, false, 64.0f, "Front of the page Background texture");

			if ( tex != mod.background && CheckTextureReadable(tex) )
			{
				if ( tex == null )
				{
					mod.ClearMadeTextures();
					update = true;
				}
				mod.background = tex;
			}
			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				mod.background = null;
				GUI.changed = false;
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical("box");
			MegaBookGUI.Label("Back", 80.0f, "Back of page Background texture");
			tex = MegaBookGUI.Texture2D(mod, "", mod.background1, false, 64.0f, "Back of page Background texture");

			if ( tex != mod.background1 && CheckTextureReadable(tex) )
			{
				if ( tex == null )
				{
					mod.ClearMadeTextures();
					update = true;
				}
				mod.background1 = tex;
			}
			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				mod.background1 = null;
				GUI.changed = false;
			}

			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical("box");
			MegaBookGUI.Label("Mask", 80.0f, "Mask texture to use");
			tex = MegaBookGUI.Texture2D(mod, "", mod.mask, false, 64.0f, "Mask texture to use");

			if ( tex != mod.mask && CheckTextureReadable(tex) )
			{
				if ( tex == null )
				{
					mod.ClearMadeTextures();
					update = true;
				}
				mod.mask = tex;
			}
			if ( GUILayout.Button("Clear", GUILayout.Width(64)) )
			{
				mod.mask = null;
				GUI.changed = false;
			}

			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();
			Rect copyarea = MegaBookGUI.Rect(mod, "Copy Area", mod.copyarea, "Copy area for the front of the page");
			if ( copyarea != mod.copyarea )
			{
				mod.copyarea = copyarea;
				mod.ClearMadeTextures();
				update = true;
				if ( mod.editpage >= 0 && mod.editpage < mod.pageparams.Count )
				{
					MegaBookPageParams pp = mod.pageparams[mod.editpage];
					mod.BuildPageTextures(pp);
				}
			}

			copyarea = MegaBookGUI.Rect(mod, "Copy Area Back", mod.copyarea1, "Copy area for the back of the page");
			if ( copyarea != mod.copyarea1 )
			{
				mod.copyarea1 = copyarea;
				mod.ClearMadeTextures();
				update = true;
			}

			EditorGUILayout.BeginHorizontal();

			if ( MegaBookGUI.BigButton("Clear", "Clear all made textures from the book") )
			{
				mod.ClearMadeTextures();
				updatemesh = true;
			}

			bool pbar = false;
			if ( MegaBookGUI.BigButton("Make Pages", "Click to rebuild the textures for the book") )
			{
				mod.ClearMadeTextures();
				//if ( mod.nobackgrounds == false )
				{
					if ( !EditorUtility.DisplayCancelableProgressBar("Creating Page Textures", "Page 0 of " + mod.pageparams.Count, 0.0f) )
					{
						for ( int i = 0; i < mod.pageparams.Count; i++ )
						{
							float alpha = (float)i / (float)(mod.pageparams.Count - 1);
							if ( EditorUtility.DisplayCancelableProgressBar("Creating Page Textures", "Makeing Page " + i + " of " + mod.pageparams.Count, alpha) )
								break;

							MegaBookPageParams page = mod.pageparams[i];
							mod.BuildPageTextures(page);
						}
					}

					EditorUtility.ClearProgressBar();
					pbar = true;
				}
				//mod.BuildPageTextures();
				updatemesh = true;
			}

			if ( !pbar )
			{
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
		}

		void OnSceneGUI()
		{
			MegaBookBuilder mod = (MegaBookBuilder)target;

			SceneGUI(mod);
			for ( int i = 0; i < mod.pageparams.Count; i++ )
			{
				MegaBookPageParams pp = mod.pageparams[i];
				for ( int j = 0; j < pp.objects.Count; j++ )
				{
					MegaBookPageObject pobj = pp.objects[j];

					if ( pobj.obj && pobj.obj.activeInHierarchy )
					{
						float psize = 0.01f;
						if ( mod.editpage == i && pp.objectIndex == j )
						{
							Handles.color = Color.red;
							psize = 0.0f;
						}
						else
							Handles.color = Color.green;

						if ( Handles.Button(pobj.obj.transform.position, pobj.obj.transform.rotation, 0.01f, psize, Handles.SphereHandleCap) )
						{
							mod.editpage = i;
							pp.objectIndex = j;
							EditorUtility.SetDirty(target);
						}
					}
				}
			}
		}

		static bool		isDragging = false;
		static Vector3	numPagesPosition;
		static Vector3	newNumPagesPosition;
		static Vector3	lengthPosition;
		static Vector3	newLengthPosition;
		static Vector3	widthPosition;
		static Vector3	newWidthPosition;
		static int		id;
		static float	pagescale = 1.0f;

		static public void SceneGUI(MegaBookBuilder mod)
		{
			if ( mod.editpage >= mod.pages.Count )
				return;
			MegaBookPage pg = mod.pages[mod.editpage];

			int id1 = GUIUtility.GetControlID(FocusType.Passive);
			int id2 = GUIUtility.GetControlID(FocusType.Passive);

			Quaternion rot = Quaternion.Euler(0.0f, mod.transform.eulerAngles.y, 0.0f);

			Vector3 xoff = Vector3.forward * mod.pageLength * 0.5f;
			Vector3 yoff = Vector3.right * mod.pageWidth * 0.5f;
			Vector3 yoff2 = Vector3.right * mod.pageWidth;

			if ( Event.current.type == EventType.MouseDown )
			{
				isDragging = true;

				numPagesPosition = mod.transform.position + (rot * yoff);
				lengthPosition = mod.transform.position + (rot * (xoff + yoff));
				widthPosition = mod.transform.position + (rot * yoff2);

				newNumPagesPosition = numPagesPosition;
				newLengthPosition = lengthPosition;
				newWidthPosition = widthPosition;
				pagescale = mod.page;
			}
			else
			{
				if ( Event.current.type == EventType.MouseUp )
				{
					pagescale = mod.page;

					isDragging = false;
					if ( GUIUtility.hotControl == id1 )
					{
						Undo.RecordObject(mod, "Changed Book Length");

						Vector3 p1 = mod.transform.InverseTransformPoint(lengthPosition);
						Vector3 p2 = mod.transform.InverseTransformPoint(newLengthPosition);

						float d = p2.z - p1.z;

						mod.pageLength += d * 1.0f;
						mod.pageLength = Mathf.Clamp(mod.pageLength, 0.001f, 2.0f);

						lengthPosition = newLengthPosition;
						mod.rebuildmeshes = true;
						EditorUtility.SetDirty(mod);
					}

					if ( GUIUtility.hotControl == id2 )
					{
						Undo.RecordObject(mod, "Changed Book Width");

						Vector3 p1 = mod.transform.InverseTransformPoint(widthPosition);
						Vector3 p2 = mod.transform.InverseTransformPoint(newWidthPosition);

						float d = p2.x - p1.x;

						mod.pageWidth += d * 1.0f;
						mod.pageWidth = Mathf.Clamp(mod.pageWidth, 0.001f, 2.0f);

						widthPosition = newWidthPosition;

						mod.rebuildmeshes = true;
						EditorUtility.SetDirty(mod);
					}
				}
				else
				{
					if ( !isDragging )
					{
						newLengthPosition = mod.transform.position + (rot * (xoff + yoff));
						newWidthPosition = mod.transform.position + (rot * yoff2);

						widthPosition = newWidthPosition;
						lengthPosition = newLengthPosition;
					}
				}
			}

			EditorGUI.BeginChangeCheck();
			Vector3 ppos = mod.transform.position + ((rot * (-Vector3.forward * mod.pageLength * 1.1f * 0.5f)));
			float page;

			if ( pagescale >= 0.0f )
				page = Handles.ScaleSlider(mod.page, ppos, rot * Vector3.right, rot, HandleUtility.GetHandleSize(ppos), 0.0f);
			else
				page = Handles.ScaleSlider(mod.page, ppos, rot * -Vector3.right, rot, HandleUtility.GetHandleSize(ppos), 0.0f);

			page = Mathf.Clamp(page, mod.MinPageVal(), mod.MaxPageVal());
			float pdelta = page - mod.page;

			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(mod, "Changed Page Turn");
				float ps = pagescale;
				if ( ps < 1.0f )
					ps = 1.0f;

				mod.page += pdelta / (ps * 15.0f);	//* 0.02f;
				EditorUtility.SetDirty(mod);    //target);
			}
			Handles.Label(ppos, "Page " + mod.page.ToString("0.00"));

			// Num Pages slider
#if false
			EditorGUI.BeginChangeCheck();
			ppos = mod.transform.position + ((rot * (Vector3.forward * mod.pageLength * 1.1f * 0.5f)));
			int numpages = (int)Handles.ScaleSlider((float)mod.NumPages, ppos, rot * Vector3.up, rot, HandleUtility.GetHandleSize(ppos), 0.0f);

			numpages = Mathf.Clamp(numpages, 2, 100);

			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(mod, "Changed Num Pages");
				//mod.page += pdelta * 0.02f;
				mod.NumPages = numpages;
				mod.rebuildmeshes = true;
				EditorUtility.SetDirty(mod);    //target);
			}
			Handles.Label(ppos, "Num Pages " + mod.NumPages);
#endif
			//newLengthPosition.y = 0.0f;
			//newWidthPosition.y = 0.0f;
#if UNITY_2022_1_OR_NEWER
			newLengthPosition = Handles.FreeMoveHandle(id1, newLengthPosition, HandleUtility.GetHandleSize(ppos) * 0.03f, Vector3.zero, Handles.DotHandleCap);
			newWidthPosition = Handles.FreeMoveHandle(id2, newWidthPosition, HandleUtility.GetHandleSize(ppos) * 0.03f, Vector3.zero, Handles.DotHandleCap);
#else
			newLengthPosition = Handles.FreeMoveHandle(id1, newLengthPosition, rot, HandleUtility.GetHandleSize(ppos) * 0.03f, Vector3.zero, Handles.DotHandleCap);
			newWidthPosition = Handles.FreeMoveHandle(id2, newWidthPosition, rot, HandleUtility.GetHandleSize(ppos) * 0.03f, Vector3.zero, Handles.DotHandleCap);
#endif
			Vector3 p1l = mod.transform.InverseTransformPoint(lengthPosition);
			Vector3 p2l = mod.transform.InverseTransformPoint(newLengthPosition);

			float dl = p2l.z - p1l.z;

			Vector3 p1w = mod.transform.InverseTransformPoint(widthPosition);
			Vector3 p2w = mod.transform.InverseTransformPoint(newWidthPosition);

			float dw = p2w.x - p1w.x;

			Handles.matrix = mod.transform.parent.localToWorldMatrix;
			Handles.color = mod.highlightColor;	//new Color(0.4f, 0.4f, 0.4f, 0.5f);//Color.white;
			Handles.DrawWireCube(new Vector3((mod.pageWidth + (dw * 1.0f)) * 0.5f, 0.0f, 0.0f), new Vector3(mod.pageWidth + (dw * 1.0f), 0.0f, mod.pageLength + (dl * 2.0f)));
			Handles.matrix = Matrix4x4.identity;

			switch ( Tools.current )
			{
				case Tool.Move:
					for ( int i = 0; i < pg.objects.Count; i++ )
					{
						if ( pg.objects[i].obj && pg.objects[i].obj.activeInHierarchy )
						{
							MegaBookPageParams parms = mod.pageparams[mod.editpage];	//.objects[i]];
							if ( i == parms.objectIndex )
							{
								Transform trans = pg.objects[i].obj.transform;
								EditorGUI.BeginChangeCheck();
								Vector3 pos = Handles.PositionHandle(trans.position, mod.transform.parent.rotation);	//.rotation);	//Quaternion.identity);
								if ( EditorGUI.EndChangeCheck() )
								{
									Undo.RecordObject(mod, "Changed Page Object Pos");
									Vector3 delta = trans.position - pos;

									delta.y = 0.0f;

									MegaBookPageObject pobj = parms.objects[i];

									pobj.pos += delta;

									pobj.pos.x = Mathf.Clamp(pobj.pos.x, 0.0f, 99.99f);
									pobj.pos.z = Mathf.Clamp(pobj.pos.z, 0.0f, 99.99f);

									mod.UpdateAttached();
									EditorUtility.SetDirty(mod);	//target);
								}
							}
						}
					}

					if ( mod.frontcover )
					{
						Handles.matrix = Matrix4x4.identity;	//mod.frontcover.localToWorldMatrix;

						EditorGUI.BeginChangeCheck();

						Vector3 piv = Handles.PositionHandle(mod.transform.TransformPoint(mod.frontpivot), Quaternion.identity);
						if ( EditorGUI.EndChangeCheck() )
						{
							Undo.RecordObject(mod, "Changed Front Cover Pivot");
							mod.frontpivot = mod.transform.InverseTransformPoint(piv);
							EditorUtility.SetDirty(mod);
						}

						Handles.matrix = mod.frontcover.localToWorldMatrix;
						Handles.Label(mod.frontpivot, "Front Pivot");
					}

					if ( mod.backcover )
					{
						Handles.matrix = Matrix4x4.identity;    //mod.frontcover.localToWorldMatrix;

						EditorGUI.BeginChangeCheck();

						Vector3 piv = Handles.PositionHandle(mod.transform.TransformPoint(mod.backpivot), Quaternion.identity);
						if ( EditorGUI.EndChangeCheck() )
						{
							Undo.RecordObject(mod, "Changed Back Cover Pivot");
							mod.backpivot = mod.transform.InverseTransformPoint(piv);
							EditorUtility.SetDirty(mod);
						}

						Handles.matrix = mod.backcover.localToWorldMatrix;
						Handles.Label(mod.backpivot, "Back Pivot");
					}

					if ( mod.spineBone )
					{
						Handles.matrix = Matrix4x4.identity;    //mod.frontcover.localToWorldMatrix;

						EditorGUI.BeginChangeCheck();

						Vector3 piv = Handles.PositionHandle(mod.transform.TransformPoint(mod.spinepivot), Quaternion.identity);
						if ( EditorGUI.EndChangeCheck() )
						{
							Undo.RecordObject(mod, "Changed Spine Pivot");
							mod.spinepivot = mod.transform.InverseTransformPoint(piv);
							EditorUtility.SetDirty(mod);
						}

						Handles.matrix = mod.spineBone.localToWorldMatrix;
						Handles.Label(mod.spinepivot, "Spine Pivot");
					}

					break;
			}

			if ( mod.showPageMesh )
			{
				Mesh m;
				if ( mod.pages[mod.editpage].showinghole )
					m = mod.pages[mod.editpage].holemesh;
				else
					m = mod.pages[mod.editpage].mesh;

				if ( m != null )
				{
					Vector3[] verts = m.vertices;
					int[] tris = m.triangles;

					Handles.matrix = mod.pages[mod.editpage].obj.transform.localToWorldMatrix;
					Handles.color = mod.highlightColor;
					for ( int i = 0; i < tris.Length; i += 3 )
					{
						Vector3 p1 = verts[tris[i]];
						Vector3 p2 = verts[tris[i + 1]];
						Vector3 p3 = verts[tris[i + 2]];
						Handles.DrawLine(p1, p2);
						Handles.DrawLine(p2, p3);
						Handles.DrawLine(p3, p1);
					}
				}
			}
		}
	}
}