using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace MegaBook
{
	public class MegaBookCreateWindow : EditorWindow
	{
		bool					getlist			= true;
		Texture2D				frontTexture;
		Texture2D				backTexture;
		Texture					logoImage;
		MegaBookBuilder			bookStyle;
		MegaBookCover			coverStyle;
		int						numPages		= 0;
		float					width			= 0.0f;
		float					length			= 0.0f;
		float					height			= 0.0f;
		float					thickness		= 0.0f;
		int						styleIndex		= 0;
		int						coverStyleIndex	= 0;
		string[]				styleNames;
		string[]				coverStyleNames;
		List<MegaBookBuilder>	bookStyles		= new List<MegaBookBuilder>();
		List<MegaBookCover>		coverStyles		= new List<MegaBookCover>();
		Vector3					rotation		= Vector3.zero;

		Vector2					scrollPos;

		[MenuItem("Window/Create a Book")]
		static void Init()
		{
			EditorWindow.GetWindow(typeof(MegaBookCreateWindow), false, "Create a Book");
		}

		void OnGUI()
		{
			if ( getlist || bookStyles.Count == 0 )
			{
#if false
				string[] dirs = System.IO.Directory.GetDirectories(Application.dataPath, "MegaBook/Styles");

				if ( dirs.Length == 0 )
				{
					EditorGUILayout.HelpBox("The 'MegaBook/Styles' folder could not be found!", MessageType.Warning);
					return;
				}

				string lpath = "Assets" + dirs[0].Substring(Application.dataPath.Length);

				string[] folders = new string[]
				{
					"Assets/MegaBook/Styles"
				};
				folders[0] = lpath;

				dirs = System.IO.Directory.GetDirectories(Application.dataPath, "MegaBook/Cover Styles");

				if ( dirs.Length == 0 )
				{
					EditorGUILayout.HelpBox("The 'MegaBook/Cover Styles' folder could not be found!", MessageType.Warning);
					return;
				}

				lpath = "Assets" + dirs[0].Substring(Application.dataPath.Length);

				string[] cfolders = new string[]
				{
					"Assets/MegaBook/Cover Styles"
				};

				cfolders[0] = lpath;
#endif
				// Book styles
				bookStyles.Clear();

				string[] csdirs = System.IO.Directory.GetDirectories(Application.dataPath, "MegaBook/Cover Styles");
				string[] sdirs = System.IO.Directory.GetDirectories(Application.dataPath, "MegaBook/Styles");

				string[] folders = new string[1];

				for ( int i = 0; i < sdirs.Length; i++ )
				{
					folders[0] = "Assets" + sdirs[i].Substring(Application.dataPath.Length);

					string[] guids = AssetDatabase.FindAssets("t:Object", folders);
					for ( int j = 0; j < guids.Length; j++ )
					{
						string path = AssetDatabase.GUIDToAssetPath(guids[j]);

						MegaBookBuilder book = (MegaBookBuilder)AssetDatabase.LoadAssetAtPath(path, typeof(MegaBookBuilder));

						if ( book )
							bookStyles.Add(book);
					}
				}

				styleNames = new string[bookStyles.Count + 1];

				styleNames[0] = "None";
				for ( int i = 0; i < bookStyles.Count; i++ )
					styleNames[i + 1] = bookStyles[i].name;

				// Cover styles
				coverStyles.Clear();

				for ( int i = 0; i < csdirs.Length; i++ )
				{
					folders[0] = "Assets" + csdirs[i].Substring(Application.dataPath.Length);

					string[] guids = AssetDatabase.FindAssets("t:Object", folders);
					for ( int j = 0; j < guids.Length; j++ )
					{
						string path = AssetDatabase.GUIDToAssetPath(guids[j]);

						MegaBookCover cover = (MegaBookCover)AssetDatabase.LoadAssetAtPath(path, typeof(MegaBookCover));

						if ( cover )
							coverStyles.Add(cover);
					}
				}

				coverStyleNames = new string[coverStyles.Count + 1];

				coverStyleNames[0] = "None";
				for ( int i = 0; i < coverStyles.Count; i++ )
					coverStyleNames[i + 1] = coverStyles[i].name;

				getlist = false;
			}

			if ( logoImage == null )
				logoImage = (Texture)Resources.Load<Texture>("Editor/logo" + Random.Range(0, 3));	// + ".png", typeof(Texture));

			if ( logoImage )
			{
				float h = (float)logoImage.height / ((float)logoImage.width / (float)position.width);
				GUILayout.Box(logoImage, GUILayout.Width(position.width), GUILayout.Height(h));
			}

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

			MegaBookGUI.Header("Create Book Params");

			EditorGUILayout.BeginVertical("box");

			styleIndex = Mathf.Clamp(styleIndex, 0, styleNames.Length - 1);
			coverStyleIndex = Mathf.Clamp(coverStyleIndex, 0, coverStyleNames.Length - 1);

			styleIndex = EditorGUILayout.Popup("Library", styleIndex, styleNames);
			coverStyleIndex = EditorGUILayout.Popup("Cover", coverStyleIndex, coverStyleNames);

			MegaBookGUI.Header("Override Values");
			EditorGUILayout.HelpBox("Leave values at 0 below to use values in the selected book above. Set to non zero to have the book built with those values instead.", MessageType.Info);
			EditorGUILayout.BeginVertical("box");
			numPages = EditorGUILayout.IntField("Num Pages", numPages);
			width = EditorGUILayout.FloatField("Page Width", width);
			length = EditorGUILayout.FloatField("Page Length", length);
			height = EditorGUILayout.FloatField("Page Height", height);
			thickness = EditorGUILayout.FloatField("Book Thickness", thickness);
			rotation = EditorGUILayout.Vector3Field("Rotation", rotation);
			EditorGUILayout.EndVertical();

			// These override book style values
			MegaBookGUI.Header("Page Textures");
			EditorGUILayout.BeginVertical("box");
			frontTexture = (Texture2D)EditorGUILayout.ObjectField("Front", frontTexture, typeof(Texture2D), false);	//, GUILayout.Width(64));
			backTexture = (Texture2D)EditorGUILayout.ObjectField("Back", backTexture, typeof(Texture2D), false);	//, GUILayout.Width(64));
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndScrollView();

			GUI.enabled = false;
			if ( styleIndex > 0 )
				GUI.enabled = true;

			if ( MegaBookGUI.BigButton("Create Book") )
				CreateBook(bookStyles[styleIndex - 1]);
		}

		public BoxCollider MakeCollider(GameObject root)
		{
			Transform rtm = root.transform;

			BoxCollider boxCol = root.AddComponent<BoxCollider>();
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

			MeshFilter[] allDescendants = root.GetComponentsInChildren<MeshFilter>();
			foreach ( MeshFilter mf in allDescendants )
			{
				Mesh mesh = mf.sharedMesh;
				SkinnedMeshRenderer sr = mf.gameObject.GetComponent<SkinnedMeshRenderer>();
				if ( sr )
				{
					Mesh smesh = new Mesh();
					sr.BakeMesh(smesh);
					mesh = smesh;
				}

				Vector3[] verts = mesh.vertices;

				Transform tm = mf.transform;

				for ( int i = 0; i < verts.Length; i++ )
					bounds.Encapsulate(rtm.InverseTransformPoint(tm.TransformPoint(verts[i])));
			}

			Vector3 c = Vector3.zero;
			c.x += (bounds.size.x * 0.45f);
			boxCol.center = c;	//bounds.center;
			Vector3 s = bounds.size;
			s.x *= 0.95f;
			boxCol.size = s;

			return boxCol;
		}

		void CreateBook(MegaBookBuilder srcbook)
		{
			if ( srcbook == null )
				return;

			Vector3 pos = Vector3.zero;

			if ( UnityEditor.SceneView.lastActiveSceneView )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject root = new GameObject(srcbook.name + " - Root");
			root.transform.position = pos;
			root.transform.eulerAngles = rotation;
			MegaBook mbook = root.AddComponent<MegaBook>();

			GameObject go = new GameObject(srcbook.name);
			go.transform.SetParent(root.transform);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;

			MegaBookBuilder book = go.AddComponent<MegaBookBuilder>();

			EditorUtility.CopySerialized(srcbook, book);

			if ( coverStyleIndex == 0 )
			{
				if ( srcbook.bookCover )
				{
					GameObject cobj = Instantiate(srcbook.bookCover.gameObject);
					cobj.transform.SetParent(root.transform);
					cobj.transform.localPosition = Vector3.zero;
					cobj.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
					book.bookCover = cobj.GetComponent<MegaBookCover>();
					book.bookCover.UpdateBones(book);
				}
			}
			else
			{
				if ( coverStyles[coverStyleIndex - 1] )
				{
					GameObject cobj = Instantiate(coverStyles[coverStyleIndex - 1].gameObject);
					cobj.transform.SetParent(root.transform);
					cobj.transform.localPosition = Vector3.zero;
					cobj.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
					book.bookCover = cobj.GetComponent<MegaBookCover>();
					book.bookCover.UpdateBones(book);
					book.page = -1.0f;
				}
			}
			Physics.SyncTransforms();

			book.headband1 = null;
			book.headband2 = null;
			book.spineFabric = null;

			book.pages.Clear();
			book.updateBindings = true;

			if ( numPages > 0 )
				book.NumPages = numPages;

			if ( width != 0.0f )
				book.pageWidth = width;

			if ( length != 0.0f )
				book.pageLength = length;

			if ( height != 0.0f )
				book.pageHeight = height;

			if ( thickness != 0.0f )
				book.bookthickness = thickness;

			for ( int i = 0; i < book.NumPages; i++ )
			{
				MegaBookPageParams page;

				if ( i < book.pageparams.Count )
					page = book.pageparams[i];
				else
				{
					page = MegaBookBuilderEditor.NewPage(book, book.pageparams.Count - 1);
					book.pageparams.Add(page);
				}

				if ( frontTexture )
					page.front = frontTexture;

				if ( backTexture )
					page.back = backTexture;
			}

			for ( int i = book.NumPages; i < book.pageparams.Count; i++ )
			{
				book.pageparams.RemoveAt(book.pageparams.Count - 1);
			}

			Physics.SyncTransforms();
			BoxCollider boxcol = MakeCollider(root);
			if ( boxcol )
			{
				Bounds b = boxcol.bounds;
				RaycastHit hit;

				Vector3 size = b.size;
				if ( Physics.Raycast(pos + (Vector3.up * 100.0f), Vector3.down, out hit) )
				{
					pos = hit.point;
					pos.y += size.y * 0.5f;
				}
			}

			DestroyImmediate(boxcol);
			root.transform.position = pos;
			Selection.activeObject = root;
			book.rebuild = true;
			Physics.SyncTransforms();
		}

		private void Awake()
		{
			LoadPrefs();
		}

		private void OnFocus()
		{
			//LoadPrefs();
		}

		void LoadPrefs()
		{
			if ( EditorPrefs.HasKey("mb_width") )
				width = EditorPrefs.GetFloat("mb_width");

			if ( EditorPrefs.HasKey("mb_length") )
				length = EditorPrefs.GetFloat("mb_length");

			if ( EditorPrefs.HasKey("mb_height") )
				height = EditorPrefs.GetFloat("mb_height");

			if ( EditorPrefs.HasKey("mb_thickness") )
				thickness = EditorPrefs.GetFloat("mb_thickness");

			if ( EditorPrefs.HasKey("mb_numpages") )
				numPages = EditorPrefs.GetInt("mb_numpages");

			if ( EditorPrefs.HasKey("mb_style") )
				styleIndex = EditorPrefs.GetInt("mb_style");

			if ( EditorPrefs.HasKey("mb_coverstyle") )
				coverStyleIndex = EditorPrefs.GetInt("mb_coverstyle");

			if ( EditorPrefs.HasKey("mb_rotationx") )
				rotation.x = EditorPrefs.GetFloat("mb_rotationx");

			if ( EditorPrefs.HasKey("mb_rotationy") )
				rotation.y = EditorPrefs.GetFloat("mb_rotationy");

			if ( EditorPrefs.HasKey("mb_rotationz") )
				rotation.z = EditorPrefs.GetFloat("mb_rotationz");

			if ( EditorPrefs.HasKey("mb_fronttexture") )
			{
				string path = EditorPrefs.GetString("mb_fronttexture");
				frontTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
			}

			if ( EditorPrefs.HasKey("mb_backtexture") )
			{
				string path = EditorPrefs.GetString("mb_backtexture");
				backTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
			}
		}

		void SavePrefs()
		{
			EditorPrefs.SetFloat("mb_width", width);
			EditorPrefs.SetFloat("mb_length", length);
			EditorPrefs.SetFloat("mb_height", height);
			EditorPrefs.SetFloat("mb_thickness", thickness);
			EditorPrefs.SetInt("mb_numpages", numPages);
			EditorPrefs.SetInt("mb_style", styleIndex);
			EditorPrefs.SetInt("mb_coverstyle", coverStyleIndex);
			EditorPrefs.SetFloat("mb_rotationx", rotation.x);
			EditorPrefs.SetFloat("mb_rotationy", rotation.y);
			EditorPrefs.SetFloat("mb_rotationz", rotation.z);

			if ( frontTexture )
			{
				string path = AssetDatabase.GetAssetPath(frontTexture);
				EditorPrefs.SetString("mb_fronttexture", path);
			}
			else
				EditorPrefs.DeleteKey("mb_fronttexture");

			if ( backTexture )
			{
				string path = AssetDatabase.GetAssetPath(backTexture);
				EditorPrefs.SetString("mb_backtexture", path);
			}
			else
				EditorPrefs.DeleteKey("mb_backtexture");
		}

		private void OnLostFocus()
		{
			SavePrefs();
		}

		private void OnDestroy()
		{
			SavePrefs();
		}
	}
}