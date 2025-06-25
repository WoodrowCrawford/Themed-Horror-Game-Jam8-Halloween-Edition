using UnityEngine;
using UnityEditor;

namespace MegaBook
{
	[CustomEditor(typeof(MegaBook))]
	public class MegaBookEditor : Editor
	{
		private MegaBookBuilder src;

		EditorWindow inspector;

		private void OnEnable()
		{
			src = target as MegaBookBuilder;
		}

		static void drawString(string text, Vector3 worldPos, Color? colour = null)
		{
			UnityEditor.Handles.BeginGUI();
			if ( colour.HasValue )
				GUI.color = colour.Value;
			var view = UnityEditor.SceneView.currentDrawingSceneView;
			Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

			if ( screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0 )
			{
				UnityEditor.Handles.EndGUI();
				return;
			}

			Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
			GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
			UnityEditor.Handles.EndGUI();
		}

		public override void OnInspectorGUI()
		{
			MegaBook mod = (MegaBook)target;

			if ( mod.book )
				MegaBookBuilderEditor.DisplayInspector(mod.book);
			else
				mod.book = mod.GetComponentInChildren<MegaBookBuilder>();
		}

		void OnSceneGUI()
		{
			MegaBook mod = (MegaBook)target;

			if ( mod.book )
			{
				MegaBookBuilderEditor.SceneGUI(mod.book);

				for ( int i = 0; i < mod.book.pageparams.Count; i++ )
				{
					MegaBookPageParams pp = mod.book.pageparams[i];
					for ( int j = 0; j < pp.objects.Count; j++ )
					{
						MegaBookPageObject pobj = pp.objects[j];

						if ( pobj.obj && pobj.obj.activeInHierarchy )
						{
							float psize = 0.01f;
							float ps = 0.01f;
							if ( mod.book.editpage == i && pp.objectIndex == j )
							{
								Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);//Color.red;
								psize = 0.0f;
								ps = 0.005f;
							}
							else
								Handles.color = Color.green;

							if ( Handles.Button(pobj.obj.transform.position, pobj.obj.transform.rotation, ps, psize, Handles.SphereHandleCap) )
							{
								mod.book.editpage = i;
								pp.objectIndex = j;
								EditorUtility.SetDirty(target);
							}
						}
					}
				}
			}
		}
	}
}
