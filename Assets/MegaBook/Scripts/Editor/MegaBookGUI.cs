using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;

namespace MegaBook
{
	using UnityEngine;
	using UnityEditor;

	public class EditorUtil
	{
		static public void SetDirty(Object target)
		{
			if ( target )
			{
				EditorUtility.SetDirty(target);
				if ( !Application.isPlaying )
					EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
	}

	public class MegaHandles
	{
		public static void DotCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			Handles.DotHandleCap(id, pos, rot, size, EventType.Repaint);
		}

		public static void SphereCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			Handles.SphereHandleCap(id, pos, rot, size, EventType.Repaint);
		}

		public static void ArrowCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			Handles.ArrowHandleCap(id, pos, rot, size, EventType.Repaint);
		}
	}

	public class MegaBookGUI
	{
		static GUIStyle headerStyle;
		static GUIStyle foldoutStyle;

		public static bool dragging = false;

		static public void Header(string name)
		{
			if ( headerStyle == null )
			{
				headerStyle = new GUIStyle();
				headerStyle.normal.textColor = Color.grey;
				headerStyle.fontSize = 16;
				headerStyle.fontStyle = FontStyle.Bold;
			}
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField(name, headerStyle);
			EditorGUILayout.EndVertical();
		}

		static GUIStyle buttonStyle;

		static public bool BigButton(string name, string tip = "")
		{
			if ( buttonStyle == null )
			{
				buttonStyle = new GUIStyle(EditorStyles.miniButton);
				buttonStyle.fontSize = 16;
				buttonStyle.fontStyle = FontStyle.Bold;
				buttonStyle.fixedHeight = 40;
			}

			if ( GUILayout.Button(new GUIContent(name, tip), buttonStyle) )
				return true;

			return false;
		}

		static public void FoldOut(ref bool open, string name, string tip = "")
		{
			if ( headerStyle == null )
			{
				headerStyle = new GUIStyle();
				headerStyle.normal.textColor = Color.white;
				headerStyle.fontSize = 16;
				headerStyle.fontStyle = FontStyle.Bold;
			}

			if ( foldoutStyle == null )
			{
				foldoutStyle = new GUIStyle(EditorStyles.foldout);
				Color col = new Color(0.8f, 0.8f, 0.8f, 1.0f);	//.white;
				foldoutStyle.normal.textColor = col;
				foldoutStyle.fontSize = 16;
				foldoutStyle.fontStyle = FontStyle.Bold;
				//foldoutStyle.fixedWidth = 16;
				foldoutStyle.fontStyle = FontStyle.Bold;
				foldoutStyle.normal.textColor = col;
				foldoutStyle.onNormal.textColor = col;
				foldoutStyle.hover.textColor = col;
				foldoutStyle.onHover.textColor = col;
				foldoutStyle.focused.textColor = col;
				foldoutStyle.onFocused.textColor = col;
				foldoutStyle.active.textColor = col;
				foldoutStyle.onActive.textColor = col;
			}

			EditorGUILayout.BeginVertical("box");
			open = EditorGUILayout.Foldout(open, new GUIContent(name, tip), true, foldoutStyle);
			EditorGUILayout.EndVertical();
		}

		static public bool undoEnable = true;

		static void BeginChangeCheck()
		{
			//if ( undoEnable )
			{
				EditorGUI.BeginChangeCheck();
			}
		}

		static bool EndChangeCheck()
		{
			//if ( undoEnable )
			{
				return EditorGUI.EndChangeCheck();
			}
		}

		static public void RecordObject(Object target, string name)
		{
			if ( undoEnable )
			{
				Undo.RecordObject(target, "Changed " + name);
			}
		}

		static public void Int(Object target, string name, ref int val)
		{
			BeginChangeCheck();
			int newval = EditorGUILayout.IntField(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public int Int(Object target, string name, int val)
		{
			BeginChangeCheck();
			int newval = EditorGUILayout.IntField(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public void Int(Object target, string name, ref int val, string tip)
		{
			BeginChangeCheck();
			int newval = EditorGUILayout.IntField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public int Int(Object target, string name, int val, string tip)
		{
			BeginChangeCheck();
			int newval = EditorGUILayout.IntField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public void Float(Object target, string name, ref float val)
		{
			BeginChangeCheck();
			float newval = EditorGUILayout.FloatField(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Float(Object target, string name, ref float val, string tip)
		{
			BeginChangeCheck();
			//float newval = EditorGUILayout.FloatField(name, val);
			float newval = EditorGUILayout.FloatField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Float(Object target, string name, ref float val, float prec, string tip)
		{
			BeginChangeCheck();
			//float newval = EditorGUILayout.FloatField(name, val);
			float newval = EditorGUILayout.FloatField(new GUIContent(name, tip), val * prec) / prec;
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public float Float(Object target, string name, float val)
		{
			BeginChangeCheck();
			float newval = EditorGUILayout.FloatField(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public float Float(Object target, string name, float val, string tip)
		{
			BeginChangeCheck();
			//float newval = EditorGUILayout.FloatField(name, val);
			float newval = EditorGUILayout.FloatField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public float Float(Object target, float val, params GUILayoutOption[] options)
		{
			BeginChangeCheck();
			float newval = EditorGUILayout.FloatField(val, options);
			if ( EndChangeCheck() )
				RecordObject(target, "Float");

			return newval;
		}

		static public void Vector3(Object target, string name, ref Vector3 val)
		{
			BeginChangeCheck();
			Vector3 newval = EditorGUILayout.Vector3Field(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Vector3(Object target, string name, ref Vector3 val, string tip)
		{
			BeginChangeCheck();
			//Vector3 newval = EditorGUILayout.Vector3Field(name, val);
			Vector3 newval = EditorGUILayout.Vector3Field(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public Vector3 Vector3(Object target, string name, Vector3 val, string tip = "")
		{
			BeginChangeCheck();
			Vector3 newval = EditorGUILayout.Vector3Field(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public void Vector2(Object target, string name, ref Vector2 val, string tip = "")
		{
			BeginChangeCheck();
			Vector2 newval = EditorGUILayout.Vector2Field(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Vector3Field2(Object target, string name, ref Vector3 val)
		{
			BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			Vector2 v2 = UnityEngine.Vector2.zero;
			v2.x = val.x;
			v2.y = val.y;
			v2 = EditorGUILayout.Vector2Field(name, v2);
			EditorGUILayout.EndHorizontal();
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val.x = v2.x;
				val.y = v2.y;
			}
		}

		static public void Transform(Object target, string name, ref Transform val, bool flag)
		{
			BeginChangeCheck();
			Transform newobj = (Transform)EditorGUILayout.ObjectField(name, val, typeof(Transform), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void Transform(Object target, string name, ref Transform val, bool flag, string tip)
		{
			BeginChangeCheck();
			Transform newobj = (Transform)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(Transform), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void BookCover(Object target, string name, ref MegaBookCover val, bool flag, string tip = "")
		{
			BeginChangeCheck();
			MegaBookCover newobj = (MegaBookCover)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(MegaBookCover), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void Toggle(Object target, string name, ref bool val)
		{
			BeginChangeCheck();
			bool newval = EditorGUILayout.Toggle(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public bool Toggle(Object target, string name, bool val)
		{
			BeginChangeCheck();
			bool newval = EditorGUILayout.Toggle(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public bool Toggle(Object target, string name, ref bool val, string tip)
		{
			bool changed = false;
			BeginChangeCheck();
			bool newval = EditorGUILayout.Toggle(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
				changed = true;
			}

			return changed;
		}

		static public bool Toggle(Object target, string name, bool val, string tip)
		{
			BeginChangeCheck();
			bool newval = EditorGUILayout.Toggle(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public void Text(Object target, string name, ref string val, string tip = "")
		{
			BeginChangeCheck();
			string newval = EditorGUILayout.TextField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Curve(Object target, string name, ref AnimationCurve crv)
		{
			BeginChangeCheck();
			AnimationCurve newcrv = EditorGUILayout.CurveField(name, crv);
			if ( EndChangeCheck() )
			{
				Undo.RegisterCompleteObjectUndo(target, "Changed " + name);
				crv = newcrv;
			}
		}

		static public void Curve(Object target, string name, ref AnimationCurve crv, string tip)
		{
			BeginChangeCheck();
			AnimationCurve newcrv = EditorGUILayout.CurveField(new GUIContent(name, tip), crv);
			if ( EndChangeCheck() )
			{
				Undo.RegisterCompleteObjectUndo(target, "Changed " + name);
				crv = newcrv;
			}
		}


		static public void ColorField(Object target, string name, ref Color val, string tip = "")
		{
			BeginChangeCheck();
			Color newval = EditorGUILayout.ColorField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Color32(Object target, string name, ref Color32 val)
		{
			BeginChangeCheck();
			Color32 newval = EditorGUILayout.ColorField(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void ColorField(Object target, ref Color val)
		{
			BeginChangeCheck();
			Color newval = EditorGUILayout.ColorField(val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Color Changed ");
				val = newval;
			}
		}

		static public void Slider(Object target, string name, ref float val, float min, float max)
		{
			BeginChangeCheck();
			float newval = EditorGUILayout.Slider(name, val, min, max);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public float Slider(Object target, string name, float val, float min, float max)
		{
			BeginChangeCheck();
			val = Mathf.Clamp(val, min, max);
			float newval = EditorGUILayout.Slider(name, val, min, max);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public void Slider(Object target, string name, ref float val, float min, float max, string tip)
		{
			BeginChangeCheck();
			float newval = EditorGUILayout.Slider(new GUIContent(name, tip), val, min, max);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public float Slider(Object target, string name, float val, float min, float max, string tip)
		{
			BeginChangeCheck();
			float newval = EditorGUILayout.Slider(new GUIContent(name, tip), val, min, max);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public void IntSlider(Object target, string name, ref int val, int min, int max, string tip = "")
		{
			BeginChangeCheck();
			int newval = EditorGUILayout.IntSlider(new GUIContent(name, tip), val, min, max);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Int(Object target, string name, ref int val, int min, int max)
		{
			BeginChangeCheck();
			int newval = EditorGUILayout.IntSlider(name, val, min, max);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void MinMax(Object target, string name, ref float min, ref float max, float minlim, float maxlim)
		{
			BeginChangeCheck();
			EditorGUILayout.MinMaxSlider(name, ref min, ref max, minlim, maxlim);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				if ( min > max )
				{
					float t = min;
					min = max;
					max = t;
				}
			}
		}

		static public void Object(Object target, string name, ref Object val, System.Type type, bool flag)
		{
			BeginChangeCheck();
			Object newobj = EditorGUILayout.ObjectField(name, val, type, flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void GameObject(Object target, string name, ref GameObject val, bool flag)
		{
			BeginChangeCheck();
			GameObject newobj = (GameObject)EditorGUILayout.ObjectField(name, val, typeof(GameObject), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public GameObject GameObject(Object target, string name, GameObject val, bool flag)
		{
			BeginChangeCheck();
			GameObject newobj = (GameObject)EditorGUILayout.ObjectField(name, val, typeof(GameObject), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}

			return newobj;
		}

		static public void GameObject(Object target, string name, ref GameObject val, bool flag, string tip)
		{
			BeginChangeCheck();
			//GameObject newobj = (GameObject)EditorGUILayout.ObjectField(name, val, typeof(GameObject), flag);
			GameObject newobj = (GameObject)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(GameObject), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public GameObject GameObject(Object target, string name, GameObject val, bool flag, string tip)
		{
			BeginChangeCheck();
			//GameObject newobj = (GameObject)EditorGUILayout.ObjectField(name, val, typeof(GameObject), flag);
			GameObject newobj = (GameObject)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(GameObject), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}

			return newobj;
		}

#if UNITY_6000_0_OR_NEWER
		static public void PhysicsMat(Object target, string name, ref PhysicsMaterial val, bool flag, string tip)
		{
			BeginChangeCheck();
			//GameObject newobj = (GameObject)EditorGUILayout.ObjectField(name, val, typeof(GameObject), flag);
			PhysicsMaterial newobj = (PhysicsMaterial)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(PhysicsMaterial), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}
#else
		static public void PhysicsMat(Object target, string name, ref PhysicMaterial val, bool flag, string tip)
		{
			BeginChangeCheck();
			//GameObject newobj = (GameObject)EditorGUILayout.ObjectField(name, val, typeof(GameObject), flag);
			PhysicMaterial newobj = (PhysicMaterial)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(PhysicMaterial), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}
#endif
		static public void Mesh(Object target, string name, ref Mesh val, bool flag, string tip = "")
		{
			BeginChangeCheck();
			Mesh newobj = (Mesh)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(Mesh), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public Mesh Mesh(Object target, string name, Mesh val, bool flag)
		{
			BeginChangeCheck();
			Mesh newobj = (Mesh)EditorGUILayout.ObjectField(name, val, typeof(Mesh), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}

			return newobj;
		}

		static public void Axis(Object target, string name, ref MegaBookAxis val)
		{
			BeginChangeCheck();
			MegaBookAxis newaxis = (MegaBookAxis)EditorGUILayout.EnumPopup(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newaxis;
			}
		}

		static public void Axis(Object target, string name, ref MegaBookAxis val, string tip)
		{
			BeginChangeCheck();
			MegaBookAxis newaxis = (MegaBookAxis)EditorGUILayout.EnumPopup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newaxis;
			}
		}

		static public void LayerID(Object target, string name, ref LayerID val, string tip)
		{
			BeginChangeCheck();
			LayerID lid = (LayerID)EditorGUILayout.EnumFlagsField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = lid;
			}
		}

		static public void ColliderOverride(Object target, string name, ref ColliderOverride val, string tip)
		{
			BeginChangeCheck();
			ColliderOverride newcol = (ColliderOverride)EditorGUILayout.EnumPopup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newcol;
			}
		}

		static public void TMProSubdiv(Object target, string name, ref TMProSubdiv val, string tip)
		{
			BeginChangeCheck();
			TMProSubdiv newcol = (TMProSubdiv)EditorGUILayout.EnumPopup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newcol;
			}
		}

		static public void VideoSize(Object target, string name, ref VideoReduceSize val, string tip)
		{
			BeginChangeCheck();
			VideoReduceSize newaxis = (VideoReduceSize)EditorGUILayout.EnumPopup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newaxis;
			}
		}

		static public void RenderLayers(Object target, string name, ref RenderLayers val, string tip = "")
		{
			BeginChangeCheck();
			RenderLayers pu = (RenderLayers)EditorGUILayout.EnumFlagsField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = pu;
			}
		}

		static public void ReflectionProbeUsage(Object target, string name, ref ReflectionProbeUsage val, string tip = "")
		{
			BeginChangeCheck();
			ReflectionProbeUsage pu = (ReflectionProbeUsage)EditorGUILayout.EnumPopup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = pu;
			}
		}

		static public void LightProbeUsage(Object target, string name, ref LightProbeUsage val, string tip = "")
		{
			BeginChangeCheck();
			LightProbeUsage pu = (LightProbeUsage)EditorGUILayout.EnumPopup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = pu;
			}
		}

		static public void MotionVectorGenerationMode(Object target, string name, ref MotionVectorGenerationMode val, string tip = "")
		{
			BeginChangeCheck();
			MotionVectorGenerationMode pu = (MotionVectorGenerationMode)EditorGUILayout.EnumPopup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = pu;
			}
		}

		static public void EnumPopup(Object target, string name, ref System.Enum val, string tip = "")
		{
			BeginChangeCheck();
			System.Enum newval = EditorGUILayout.EnumPopup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void BeginToggle(Object target, string name, ref bool val)
		{
			BeginChangeCheck();
			bool newval = EditorGUILayout.BeginToggleGroup(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void BeginToggle(Object target, string name, ref bool val, string tip)
		{
			BeginChangeCheck();
			bool newval = EditorGUILayout.BeginToggleGroup(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public Vector3 PositionHandle(Object target, Vector3 pos, Quaternion rot)
		{
			Vector3 npos = Handles.PositionHandle(pos, rot);
			if ( npos != pos )
				RecordObject(target, "Position Handle Changed");
			return npos;
		}

		static public Vector3 FreeHandle(Object target, Vector3 pos, Quaternion rot, float size, Vector3 snap, Handles.CapFunction cap, int id = 0)
		{
#if UNITY_2022_1_OR_NEWER
			Vector3 npos = Handles.FreeMoveHandle(id, pos, size, snap, cap);
#else
			Vector3 npos = Handles.FreeMoveHandle(id, pos, rot, size, snap, cap);
#endif
			if ( npos != pos )
				RecordObject(target, "Free Handle Changed");
			return npos;
		}

		static public Vector3 SliderHandle(Object target, Vector3 pos, Vector3 dir, float size, Handles.CapFunction cap, float snap, int id = 21)
		{
			Vector3 npos = Handles.Slider(id, pos, dir, size, cap, snap);   //FreeMoveHandle(pos, rot, size, snap, cap);
			if ( npos != pos )
				RecordObject(target, "Free Handle Changed");
			return npos;
		}

		static public void Texture2D(Object target, string name, ref Texture2D val, bool flag = true, string tip = "")
		{
			BeginChangeCheck();
			Texture2D newobj = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(Texture2D), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void Texture2D(Object target, string name, ref Texture2D val, bool flag, float width, string tip = "")
		{
			BeginChangeCheck();
			Texture2D newobj = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(Texture2D), flag, GUILayout.Width(width));
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void Texture2D(Object target, string name, ref Texture2D val, bool flag, float width)
		{
			BeginChangeCheck();
			Texture2D newobj = (Texture2D)EditorGUILayout.ObjectField(name, val, typeof(Texture2D), flag, GUILayout.Width(width));
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public Texture2D Texture2D(Object target, string name, Texture2D val, bool flag, float width, string tip = "")
		{
			BeginChangeCheck();
			Texture2D newobj = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(Texture2D), flag, GUILayout.Width(width));
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}

			return val;
		}

		static public Texture2D Texture2D(Object target, string name, Texture2D val, bool flag = true, string tip = "")
		{
			BeginChangeCheck();
			Texture2D newobj = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(Texture2D), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}

			return newobj;
		}

		static public void Texture(Object target, string name, ref Texture val, bool flag = true)
		{
			BeginChangeCheck();
			Texture newobj = (Texture)EditorGUILayout.ObjectField(name, val, typeof(Texture), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void Label(string name, float width, string tip)
		{
			EditorGUILayout.LabelField(new GUIContent(name, tip), GUILayout.Width(width));
		}

		static public void EndToggle()
		{
			EditorGUILayout.EndToggleGroup();
		}

		static public void Popup(Object target, string name, ref int val, string[] values)
		{
			BeginChangeCheck();
			int newval = EditorGUILayout.Popup(name, val, values);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Material(Object target, string name, ref Material val, bool flag = true, string tip = "")
		{
			BeginChangeCheck();
			Material newmat = (Material)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(Material), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newmat;
			}
		}

		static public void Headband(Object target, string name, ref MegaBookHeadband val, bool flag = true, string tip = "")
		{
			BeginChangeCheck();
			MegaBookHeadband newband = (MegaBookHeadband)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(MegaBookHeadband), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newband;
			}
		}

		static public void SpineFabric(Object target, string name, ref MegaBookSpineFabric val, bool flag = true, string tip = "")
		{
			BeginChangeCheck();
			MegaBookSpineFabric newband = (MegaBookSpineFabric)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(MegaBookSpineFabric), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newband;
			}
		}

		static public void DynamicMesh(Object target, string name, ref MegaBookDynamicMesh val, bool flag = true)
		{
			BeginChangeCheck();
			MegaBookDynamicMesh newmat = (MegaBookDynamicMesh)EditorGUILayout.ObjectField(name, val, typeof(MegaBookDynamicMesh), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newmat;
			}
		}

		static public Rect Rect(Object target, string name, Rect val)
		{
			BeginChangeCheck();
			Rect newval = EditorGUILayout.RectField(name, val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}

		static public Rect Rect(Object target, string name, Rect val, string tip)
		{
			BeginChangeCheck();
			Rect newval = EditorGUILayout.RectField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
			}

			return newval;
		}
	}
}