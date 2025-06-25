using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using Unity.Collections;

namespace MegaBook
{
	[InitializeOnLoad]
	public class CompilerCheck
	{
		static CompilerCheck()
		{
			CompilationPipeline.assemblyCompilationFinished += Dispose2;
			CompilationPipeline.compilationStarted += Dispose1;
			EditorApplication.playModeStateChanged += LogPlayModeState;
		}

		static void MemoryCleanup()
		{
#if UNITY_2023_1_OR_NEWER
			MegaBookBuilder[] books = GameObject.FindObjectsByType<MegaBookBuilder>(FindObjectsSortMode.None);
#else
			MegaBookBuilder[] books = GameObject.FindObjectsOfType<MegaBookBuilder>();
#endif
			for ( int i = 0; i < books.Length; i++ )
				books[i].DisposeArrays();
		}

		static void Dispose(string assembly)
		{
			MemoryCleanup();
		}

		static void Dispose2(string assembly, CompilerMessage[] msgs)
		{
			MemoryCleanup();
		}

		static void Dispose1(object obj)
		{
		}

		static void LogPlayModeState(PlayModeStateChange state)
		{
			NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;

			if ( state == PlayModeStateChange.ExitingEditMode )
			{
#if UNITY_2023_1_OR_NEWER
				MegaBookBuilder[] books = GameObject.FindObjectsByType<MegaBookBuilder>(FindObjectsSortMode.None);
#else
				MegaBookBuilder[] books = GameObject.FindObjectsOfType<MegaBookBuilder>();
#endif
				for ( int i = 0; i < books.Length; i++ )
					books[i].DisposeArrays();

				return;
			}

			if ( state == PlayModeStateChange.ExitingPlayMode )
			{
#if UNITY_2023_1_OR_NEWER
				MegaBookBuilder[] books = GameObject.FindObjectsByType<MegaBookBuilder>(FindObjectsSortMode.None);
#else
				MegaBookBuilder[] books = GameObject.FindObjectsOfType<MegaBookBuilder>();
#endif
				for ( int i = 0; i < books.Length; i++ )
				{
				}
			}

			if ( state == PlayModeStateChange.EnteredEditMode )
			{
#if UNITY_2023_1_OR_NEWER
				MegaBookBuilder[] books = GameObject.FindObjectsByType<MegaBookBuilder>(FindObjectsSortMode.None);
#else
				MegaBookBuilder[] books = GameObject.FindObjectsOfType<MegaBookBuilder>();
#endif
				for ( int i = 0; i < books.Length; i++ )
				{
				}
			}

			if ( state == PlayModeStateChange.EnteredPlayMode )
			{
			}
			else
			{
			}
		}
	}
}
