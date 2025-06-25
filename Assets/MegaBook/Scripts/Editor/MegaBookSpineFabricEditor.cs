using UnityEngine;
using UnityEditor;

namespace MegaBook
{
	[CustomEditor(typeof(MegaBookSpineFabric))]
	public class MegaBookSpineFabricEditor : Editor
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
			MegaBookSpineFabric mod = (MegaBookSpineFabric)target;

			bool update = false;

			MegaBookGUI.Float(mod, "Length", ref mod.length, "The length of the fabric, the book will change the length multiplied by this so 1 is a good start value");
			SetUpdate(ref update);

			MegaBookGUI.Float(mod, "Width", ref mod.width, "The width of the fabric, usually the same as the page size.");
			SetUpdate(ref update);

			MegaBookGUI.Float(mod, "Thickness", ref mod.thickness, "The thickness of the fabric");
			SetUpdate(ref update);

			MegaBookGUI.IntSlider(mod, "Segments", ref mod.segs, 1, 32, "The number of vertices along the length of the headband object");
			SetUpdate(ref update);

			MegaBookGUI.Vector2(mod, "UV Offset", ref mod.uvoffset, "Offset into texture for UV's");
			SetUpdate(ref update);

			MegaBookGUI.Vector2(mod, "UV Size", ref mod.uvsize, "Size of UV area");
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