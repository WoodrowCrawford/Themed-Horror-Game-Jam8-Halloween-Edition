using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MegaBook
{
	[CustomEditor(typeof(MegaBookSkinCover))]
	public class MegaBookSkinCoverEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaBookSkinCover mod = (MegaBookSkinCover)target;

			MegaBookGUI.Transform(mod, "Spine Bone", ref mod.spineBone, true, "The object that will be the spine bone");
			MegaBookGUI.Transform(mod, "Front Bone", ref mod.frontBone, true, "The object that will be the front cover bone");
			MegaBookGUI.Transform(mod, "Back Bone", ref mod.backBone, true, "The object that will be the back cover bone");

			bool show = mod.spineBone && mod.frontBone && mod.backBone;

			MegaBookGUI.Axis(mod, "Up Axis", ref mod.upAxis, "The axis that will be used for up by the skin weight system");
			//MegaBookGUI.Toggle(mod, "Flip Dir", ref mod.flip, "Flip the Axis direction for weights");
			MegaBookGUI.Float(mod, "Front Falloff", ref mod.frontFalloff, "Distance over which the bone weights will go from front cover to spine");
			MegaBookGUI.Curve(mod, "Front Falloff Curve", ref mod.frontFallCurve, "Controls how the weight values transition over the fall off distance");
			MegaBookGUI.Float(mod, "Back Falloff", ref mod.backFalloff, "Distance over which the bone weights will go from back cover to spine");
			MegaBookGUI.Curve(mod, "Back Falloff Curve", ref mod.backFallCurve, "Controls how the weight values transition over the fall off distance");

			if ( mod.frontFalloff < 0.0f )
				mod.frontFalloff = 0.0f;

			if ( mod.backFalloff < 0.0f )
				mod.backFalloff = 0.0f;

			MegaBookGUI.Slider(mod, "Vert Size", ref mod.size, 0.00005f, 0.005f, "Size of the vertex weight gizmos");

			MegaBookGUI.Text(mod, "Prefab Path", ref mod.savePath, "Where the cover prefab will be created. Best left as 'MegaBook/Cover Styles' as this is currently where MegaBook expects Covers to be.");

			GUI.enabled = show;
			if ( GUILayout.Button("Create Skinned Cover") )
				MakeSkinMesh(mod);

			GUI.enabled = true;

			if ( !show )
				EditorGUILayout.HelpBox("Select the Cover Object and Bones", MessageType.Info);

			if ( (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed") )
			{
				EditorUtil.SetDirty(mod);
			}

			if ( GUI.changed )
				EditorUtil.SetDirty(mod);
		}

		private void OnSceneGUI()
		{
			MegaBookSkinCover mod = (MegaBookSkinCover)target;

			if ( mod.frontBone )
			{
				Handles.matrix = Matrix4x4.identity;

				Vector3 pos = Handles.PositionHandle(mod.frontBone.position, Quaternion.identity);
				if ( !pos.Equals(mod.frontBone.position) )
				{
					Undo.RecordObject(mod.frontBone, "Changed Front Bone Position");
					mod.frontBone.position = pos;
					EditorUtility.SetDirty(target);
				}

				Handles.Label(mod.frontBone.position, "Front Bone");
			}

			if ( mod.spineBone )
			{
				Handles.matrix = Matrix4x4.identity;

				Vector3 pos = Handles.PositionHandle(mod.spineBone.position, Quaternion.identity);
				if ( !pos.Equals(mod.spineBone.position) )
				{
					Undo.RecordObject(mod.spineBone, "Changed Spine Bone Position");
					mod.spineBone.position = pos;
					EditorUtility.SetDirty(target);
				}

				Handles.Label(mod.spineBone.position, "Spine Bone");
			}

			if ( mod.backBone )
			{
				Handles.matrix = Matrix4x4.identity;

				Vector3 pos = Handles.PositionHandle(mod.backBone.position, Quaternion.identity);
				if ( !pos.Equals(mod.backBone.position) )
				{
					Undo.RecordObject(mod.backBone, "Changed Back Bone Position");
					mod.backBone.position = pos;
					EditorUtility.SetDirty(target);
				}

				Handles.Label(mod.backBone.position, "Back Bone");
			}
		}

		[DrawGizmo(GizmoType.Active | GizmoType.InSelectionHierarchy)]	// | GizmoType.NotInSelectionHierarchy)]
		static void RenderGizmo(MegaBookSkinCover cover, GizmoType gizmoType)
		{
			if ( !cover.frontBone || !cover.backBone || !cover.spineBone )
				return;

			MeshFilter mf = cover.GetComponent<MeshFilter>();
			if ( mf )
			{
				Mesh mesh = mf.sharedMesh;

				if ( mesh != null )
				{
					Gizmos.matrix = cover.transform.localToWorldMatrix;

					Vector3[] verts = mesh.vertices;

					Vector3 frontLPos = cover.transform.InverseTransformPoint(cover.frontBone.position);
					Vector3 spineLPos = cover.transform.InverseTransformPoint(cover.spineBone.position);
					Vector3 backLPos = cover.transform.InverseTransformPoint(cover.backBone.position);

					int ax = (int)cover.upAxis;

					// different ways for x and z axis, do something else for y

					switch ( cover.upAxis )
					{
						case MegaBookAxis.X:
							for ( int i = 0; i < verts.Length; i++ )
							{
								Vector3 p = verts[i];
								float z = p.x;

								if ( p.y > spineLPos.y )
								{
									// front
									if ( z < frontLPos[ax] )
									{
										Gizmos.color = Color.green;
									}
									else
									{
										if ( cover.frontFalloff > 0.0f )
										{
											if ( z > frontLPos[ax] - cover.frontFalloff )
											{
												float a = (z - frontLPos[ax]) / cover.frontFalloff;
												Gizmos.color = Color.Lerp(Color.green, Color.red, cover.frontFallCurve.Evaluate(a));
											}
										}
										else
										{
											if ( z > frontLPos[ax] - cover.frontFalloff )
												Gizmos.color = Color.red;
											else
												Gizmos.color = Color.green;
										}
									}
								}
								else
								{
									// back
									if ( z < backLPos[ax] )
									{
										Gizmos.color = Color.blue;
									}
									else
									{
										if ( cover.backFalloff > 0.0f )
										{
											if ( z > backLPos[ax] - cover.backFalloff )
											{
												float a = (z - backLPos[ax]) / cover.backFalloff;
												Gizmos.color = Color.Lerp(Color.blue, Color.red, cover.backFallCurve.Evaluate(a));
											}
										}
										else
										{
											if ( z > backLPos[ax] - cover.backFalloff )
												Gizmos.color = Color.red;
											else
												Gizmos.color = Color.blue;
										}
									}
								}
								Gizmos.DrawSphere(verts[i], cover.size);
							}
							break;

						case MegaBookAxis.Y:
							for ( int i = 0; i < verts.Length; i++ )
							{
								float z = verts[i][ax];
								if ( z >= frontLPos[ax] )
									Gizmos.color = Color.green;
								else
								{
									if ( z <= backLPos[ax] )
										Gizmos.color = Color.blue;
									else
									{
										if ( z > frontLPos[ax] - cover.frontFalloff )
										{
											float a = (frontLPos[ax] - z) / cover.frontFalloff;
											Gizmos.color = Color.Lerp(Color.green, Color.red, cover.frontFallCurve.Evaluate(a));
										}
										else
										{
											if ( z < backLPos[ax] + cover.backFalloff )
											{
												float a = (z - backLPos[ax]) / cover.backFalloff;
												Gizmos.color = Color.Lerp(Color.blue, Color.red, cover.backFallCurve.Evaluate(a));
											}
											else
												Gizmos.color = Color.red;
										}
									}
								}
								Gizmos.DrawSphere(verts[i], cover.size);
							}
							break;

						case MegaBookAxis.Z:
							break;
					}
				}
			}
		}

		static Mesh CloneMesh(Mesh mesh)
		{
			Mesh clonemesh = new Mesh();
			clonemesh.vertices		= mesh.vertices;
			clonemesh.uv2			= mesh.uv2;
			clonemesh.uv2			= mesh.uv2;
			clonemesh.uv			= mesh.uv;
			clonemesh.normals		= mesh.normals;
			clonemesh.tangents		= mesh.tangents;
			clonemesh.colors		= mesh.colors;
			clonemesh.subMeshCount	= mesh.subMeshCount;

			for ( int s = 0; s < mesh.subMeshCount; s++ )
				clonemesh.SetTriangles(mesh.GetTriangles(s), s);

			clonemesh.boneWeights	= mesh.boneWeights;
			clonemesh.bindposes		= mesh.bindposes;
			clonemesh.name			= mesh.name + "_copy";
			clonemesh.RecalculateBounds();

			return clonemesh;
		}

		public void MakeSkinMesh(MegaBookSkinCover cover1)
		{
			if ( cover1.frontBone && cover1.spineBone && cover1.backBone )
			{
				MegaBookCover tempcover = cover1.gameObject.AddComponent<MegaBookCover>();
				tempcover.spine = cover1.spineBone;
				tempcover.front = cover1.frontBone;
				tempcover.back = cover1.backBone;

				GameObject newobj = Instantiate(cover1.gameObject);
				newobj.name = cover1.name;	// + "- Cover";

				DestroyImmediate(tempcover);

				// Destroy any child objects in newobj that are not bones
				List<GameObject> destroyObjs = new List<GameObject>();
				for ( int i = 0; i < newobj.transform.childCount; i++ )
				{
					Transform child = newobj.transform.GetChild(i);
					if ( child.name != "Front" && child.name != "Back" && child.name != "Spine" )
						destroyObjs.Add(child.gameObject);
				}

				for ( int i = 0; i < destroyObjs.Count; i++ )
				{
					DestroyImmediate(destroyObjs[i]);
				}

				MegaBookCover cover = newobj.GetComponent<MegaBookCover>();

				Material[] mats = null;

				MeshFilter mf = cover1.GetComponent<MeshFilter>();

				Mesh newmesh = CloneMesh(mf.sharedMesh);
				MeshFilter newmf = newobj.GetComponent<MeshFilter>();
				newmf.sharedMesh = newmesh;

				MeshRenderer oldmr = cover1.GetComponent<MeshRenderer>();
				mats = oldmr.sharedMaterials;

				MegaBookSkinCover skincov = newobj.GetComponent<MegaBookSkinCover>();
				if ( skincov )
					DestroyImmediate(skincov);

				SkinnedMeshRenderer mr = cover.gameObject.AddComponent<SkinnedMeshRenderer>();

				Mesh smesh = newmesh;
				Vector3[] verts = smesh.vertices;
				BoneWeight[] weights = new BoneWeight[smesh.vertexCount];
				Bounds b = smesh.bounds;

				Vector3 frontLPos = cover.transform.InverseTransformPoint(cover.front.position);
				Vector3 spineLPos = cover.transform.InverseTransformPoint(cover.spine.position);
				Vector3 backLPos = cover.transform.InverseTransformPoint(cover.back.position);

				float cw = 0.0f;

				switch ( cover1.upAxis )
				{
					case MegaBookAxis.X:
						for ( int i = 0; i < verts.Length; i++ )
						{
							Vector3 p = verts[i];
							float z = p.x;

							if ( p.y > spineLPos.y )
							{
								// front
								if ( z < frontLPos.x )
								{
									float w = frontLPos.x - z;
									if ( w > cw )
										cw = w;

									weights[i].boneIndex0 = 0;
									weights[i].boneIndex1 = 1;
									weights[i].weight0 = 1.0f;
									weights[i].weight1 = 0.0f;
								}
								else
								{
									if ( cover1.frontFalloff > 0.0f )
									{
										if ( z > frontLPos.x - cover1.frontFalloff )
										{
											float a = cover1.frontFallCurve.Evaluate((z - frontLPos.x) / cover1.frontFalloff);

											weights[i].boneIndex0 = 0;
											weights[i].boneIndex1 = 1;
											weights[i].weight0 = 1.0f - a;
											weights[i].weight1 = a;
										}
									}
									else
									{
										if ( z > frontLPos.x - cover1.frontFalloff )
										{
											weights[i].boneIndex0 = 0;
											weights[i].boneIndex1 = 1;
											weights[i].weight0 = 0.0f;
											weights[i].weight1 = 1.0f;
										}
										else
										{
											weights[i].boneIndex0 = 0;
											weights[i].boneIndex1 = 1;
											weights[i].weight0 = 1.0f;
											weights[i].weight1 = 0.0f;
										}
									}
								}
							}
							else
							{
								// back
								if ( z < backLPos.x )
								{
									weights[i].boneIndex0 = 1;
									weights[i].boneIndex1 = 2;
									weights[i].weight0 = 0.0f;
									weights[i].weight1 = 1.0f;
								}
								else
								{
									if ( cover1.backFalloff > 0.0f )
									{
										if ( z > backLPos.x - cover1.backFalloff )
										{
											float a = cover1.backFallCurve.Evaluate((z - backLPos.x) / cover1.backFalloff);
											weights[i].boneIndex0 = 2;
											weights[i].boneIndex1 = 1;
											weights[i].weight0 = 1.0f - a;
											weights[i].weight1 = a;
										}
									}
									else
									{
										if ( z > backLPos.x - cover1.backFalloff )
										{
											weights[i].boneIndex0 = 1;
											weights[i].boneIndex1 = 2;
											weights[i].weight0 = 1.0f;
											weights[i].weight1 = 0.0f;
										}
										else
										{
											weights[i].boneIndex0 = 1;
											weights[i].boneIndex1 = 2;
											weights[i].weight0 = 0.0f;
											weights[i].weight1 = 1.0f;
										}
									}
								}
							}
						}
						break;

					case MegaBookAxis.Y:
						break;
					case MegaBookAxis.Z:
						break;
				}

				smesh.boneWeights = weights;

				Transform[] bones = new Transform[3];
				Matrix4x4[] poses = new Matrix4x4[3];

				bones[0] = cover.front;
				poses[0] = cover.front.worldToLocalMatrix * cover.transform.localToWorldMatrix;
				bones[1] = cover.spine;
				poses[1] = cover.spine.worldToLocalMatrix * cover.transform.localToWorldMatrix;
				bones[2] = cover.back;
				poses[2] = cover.back.worldToLocalMatrix * cover.transform.localToWorldMatrix;

				smesh.bindposes = poses;
				mr.bones = bones;
				mr.sharedMesh = smesh;
				mr.materials = mats;
				mr.updateWhenOffscreen = true;

				Vector3 piv = cover.front.localPosition - cover.spine.localPosition;
				piv.x = -piv.x;
				cover.frontpivot = piv;	//cover.transform.InverseTransformPoint(cover.front.position);
				piv = cover.back.localPosition - cover.spine.localPosition;
				piv.x = -piv.x;
				cover.backpivot = piv;	//cover.transform.InverseTransformPoint(cover.back.position);
				cover.spinepivot = Vector3.zero;	//cover.transform.InverseTransformPoint(cover.spine.position);

				cover.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
				cover.coverWidth = cw;

				CreatePrefab(newobj, cover1.savePath);

				DestroyImmediate(newobj);
			}
		}

		static void CreatePrefab(GameObject obj, string path)
		{
			if ( obj != null )
			{
				GameObject prefab = PrefabUtility.SaveAsPrefabAsset(obj, "Assets/" + path + "/" + obj.name + ".prefab");
				MeshFilter mf = obj.GetComponent<MeshFilter>();
				MeshFilter mf1 = prefab.GetComponent<MeshFilter>();

				if ( mf )
				{
					MeshFilter newmf = prefab.GetComponent<MeshFilter>();

					Mesh mesh = mf.sharedMesh;
					mf1.sharedMesh = mesh;
					SkinnedMeshRenderer smr = prefab.GetComponent<SkinnedMeshRenderer>();
					smr.sharedMesh = mesh;

					AssetDatabase.AddObjectToAsset(mesh, prefab);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
		}
	}
}