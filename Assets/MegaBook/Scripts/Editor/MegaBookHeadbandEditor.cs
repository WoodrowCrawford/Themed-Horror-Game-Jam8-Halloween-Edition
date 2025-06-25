using UnityEngine;
using UnityEditor;

namespace MegaBook
{
	[CustomEditor(typeof(MegaBookHeadband))]
	public class MegaBookHeadbandEditor : Editor
	{
		void SetUpdate(ref bool update)
		{
			if ( GUI.changed )
			{
				update = true;
				GUI.changed = false;
			}
		}

		public override void OnInspectorGUI()
		{
			MegaBookHeadband mod = (MegaBookHeadband)target;

			bool update = false;
			MegaBookGUI.Float(mod, "Radius", ref mod.radius, "The Radius of the Headband object");
			SetUpdate(ref update);

			MegaBookGUI.Float(mod, "Length", ref mod.length, "The length of the handband, the book will change the length multiplied by this so 1 is a good start value");
			SetUpdate(ref update);

			MegaBookGUI.IntSlider(mod, "Segments", ref mod.segs, 1, 32, "The number of vertices along the length of the headband object");
			SetUpdate(ref update);

			MegaBookGUI.IntSlider(mod, "Sides", ref mod.sides, 4, 32, "The number of sides to the headband mesh");
			SetUpdate(ref update);

			MegaBookGUI.Vector2(mod, "UV Offset", ref mod.uvoffset, "Offset into texture for UV's");
			SetUpdate(ref update);

			MegaBookGUI.Vector2(mod, "UV Size", ref mod.uvsize, "Size of UV area");
			SetUpdate(ref update);

			MegaBookGUI.Float(mod, "UV Twist", ref mod.uvtwist, "Twist the UV along the length of the headband mesh");
			SetUpdate(ref update);

			if ( (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed") )
				mod.BuildMesh();

			if ( update )
			{
				mod.BuildMesh();
				EditorUtil.SetDirty(target);
			}
		}
	}
}