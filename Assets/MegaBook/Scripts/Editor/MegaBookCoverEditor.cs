using UnityEngine;
using UnityEditor;

namespace MegaBook
{
	[CustomEditor(typeof(MegaBookCover))]
	public class MegaBookCoverEditor : Editor
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
			MegaBookCover mod = (MegaBookCover)target;

			bool chg = false;
			MegaBookGUI.Transform(mod, "Spine Bone", ref mod.spine, true, "The object that will be the spine bone");
			MegaBookGUI.Transform(mod, "Front Bone", ref mod.front, true, "The object that will be the front cover bone");
			MegaBookGUI.Transform(mod, "Back Bone", ref mod.back, true, "The object that will be the back cover bone");

			MegaBookGUI.Vector3(mod, "Front Pivot", ref mod.frontpivot, "The pivot point for the front cover bone");
			SetUpdate(ref chg);
			MegaBookGUI.Axis(mod, "Front Axis", ref mod.frontAxis, "The axis the front bone rotates around");
			SetUpdate(ref chg);
			MegaBookGUI.Vector3(mod, "Back Pivot", ref mod.backpivot, "The pivot point for the back cover bone");
			SetUpdate(ref chg);
			MegaBookGUI.Axis(mod, "Back Axis", ref mod.backAxis, "The Axis the back bone rotates around");
			SetUpdate(ref chg);
			MegaBookGUI.Vector3(mod, "Spine Pivot", ref mod.spinepivot, "The pivot point for the spine bone");
			SetUpdate(ref chg);
			MegaBookGUI.Axis(mod, "Spine Axis", ref mod.spineAxis, "The axis the spine bone will rotate around");
			SetUpdate(ref chg);
			MegaBookGUI.Float(mod, "Thickness", ref mod.thickness, "Quickly change the thickness of the book cover");
			SetUpdate(ref chg);
			MegaBookGUI.Axis(mod, "Cover Thickness Axis", ref mod.coverThickAxis, "The axis that the cover bones will move in if thickness changes");
			SetUpdate(ref chg);
			MegaBookGUI.Float(mod, "Spine Thickness", ref mod.spineThickness, "Adjust the Spine Bone thickness.");
			SetUpdate(ref chg);
			MegaBookGUI.Axis(mod, "Spine Thickness Axis", ref mod.spineThickAxis, "The axis that the spine bone will scale in if thickness changes");
			SetUpdate(ref chg);
			MegaBookGUI.Float(mod, "Cover Width", ref mod.coverWidth, "Width of the cover mesh from the pivot point, used in Autofitting cover");
			SetUpdate(ref chg);

			if ( mod.book )
			{
				MegaBookGUI.Slider(mod.book, "Page", ref mod.book.page, mod.book.MinPageVal(), mod.book.MaxPageVal(), "Helper slider to test the book page turning");

				if ( GUI.changed )
				{
					if ( mod.book.linkeditpage )
					{
						mod.book.editpage = (int)(mod.book.page + 0.5f);
						mod.book.editpage = Mathf.Clamp(mod.book.editpage, 0, mod.book.pageparams.Count - 1);
					}

					mod.book.Flip = mod.book.page;
					mod.book.turnspd = 0.0f;
					EditorUtility.SetDirty(mod.book);
				}
			}

			if ( (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed") )
			{
				chg = true;
			}

			if ( GUI.changed || chg )
			{
				EditorUtil.SetDirty(mod);
				if ( mod.book )
					EditorUtil.SetDirty(mod.book);
			}
		}

		private void OnSceneGUI()
		{
			MegaBookCover mod = (MegaBookCover)target;

			if ( mod.book )
			{
				if ( mod.front )
				{
					Handles.matrix = mod.book.transform.localToWorldMatrix;

					Vector3 pos = Handles.PositionHandle(mod.frontpivot, mod.transform.localRotation);	//book.transform.rotation);
					if ( !pos.Equals(mod.frontpivot) )
					{
						Undo.RecordObject(mod, "Changed Front Bone Pivot");
						mod.frontpivot = pos;
						EditorUtility.SetDirty(target);
					}

					Handles.Label(mod.frontpivot, "Front Pivot");
				}

				if ( mod.back )
				{
					Handles.matrix = mod.book.transform.localToWorldMatrix;

					Vector3 pos = Handles.PositionHandle(mod.backpivot, mod.transform.localRotation);	//.transform.rotation);
					if ( !pos.Equals(mod.backpivot) )
					{
						Undo.RecordObject(mod, "Changed Back Bone Pivot");
						mod.backpivot = pos;
						EditorUtility.SetDirty(target);
					}

					Handles.Label(mod.backpivot, "Back Pivot");
				}

				if ( mod.spine )
				{
					Handles.matrix = mod.book.transform.localToWorldMatrix;

					Vector3 pos = Handles.PositionHandle(mod.spinepivot, mod.transform.localRotation);	//mod.book.transform.rotation);
					if ( !pos.Equals(mod.spinepivot) )
					{
						Undo.RecordObject(mod, "Changed Spine Bone Pivot");
						mod.spinepivot = pos;
						EditorUtility.SetDirty(target);
					}

					Handles.Label(mod.spinepivot, "Spine Pivot");
				}
			}
		}
	}
}