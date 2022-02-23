using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
#if UNITY_EDITOR
#else
public class AutoAddressing
{
	[Conditional("UNDEF")] private static void OnPostprocessAllAssetsImport(string directory, string[] importedAssets, string lebelname) { }
	[Conditional("UNDEF")] private static void OnPostprocessAllAssetsRemove(string directory, string[] RemovedAssets) { }
}
public static class Debug
{
	[Conditional("UNDEF")] public static void Assert(bool condition, string message, Object context) { }
	[Conditional("UNDEF")] public static void Assert(bool condition, object message, Object context) { }
	[Conditional("UNDEF")] public static void Assert(bool condition, string message) { }
	[Conditional("UNDEF")] public static void Assert(bool condition, object message) { }
	[Conditional("UNDEF")] public static void Assert(bool condition, Object context) { }
	[Conditional("UNDEF")] public static void Assert(bool condition) { }
	[Conditional("UNDEF")] public static void Assert(bool condition, string format, params object[] args) { }
	[Conditional("UNDEF")] public static void AssertFormat(bool condition, string format, params object[] args) { }
	[Conditional("UNDEF")] public static void AssertFormat(bool condition, Object context, string format, params object[] args) { }
	[Conditional("UNDEF")] public static void Break() { }
	[Conditional("UNDEF")] public static void ClearDeveloperConsole() { }
	[Conditional("UNDEF")] public static void DebugBreak() { }
	[Conditional("UNDEF")] public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest) { }
	[Conditional("UNDEF")] public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) { }
	[Conditional("UNDEF")] public static void DrawLine(Vector3 start, Vector3 end) { }
	[Conditional("UNDEF")] public static void DrawLine(Vector3 start, Vector3 end, Color color) { }
	[Conditional("UNDEF")] public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) { }
	[Conditional("UNDEF")] public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest) { }
	[Conditional("UNDEF")] public static void DrawRay(Vector3 start, Vector3 dir) { }
	[Conditional("UNDEF")] public static void DrawRay(Vector3 start, Vector3 dir, Color color) { }
	[Conditional("UNDEF")] public static void Log(object message) { }
	[Conditional("UNDEF")] public static void Log(object message, Object context) { }
	[Conditional("UNDEF")] public static void LogAssertion(object message, Object context) { }
	[Conditional("UNDEF")] public static void LogAssertion(object message) { }
	[Conditional("UNDEF")] public static void LogAssertionFormat(Object context, string format, params object[] args) { }
	[Conditional("UNDEF")] public static void LogAssertionFormat(string format, params object[] args) { }
	[Conditional("UNDEF")] public static void LogError(object message, Object context) { }
	[Conditional("UNDEF")] public static void LogError(object message) { }
	[Conditional("UNDEF")] public static void LogErrorFormat(string format, params object[] args) { }
	[Conditional("UNDEF")] public static void LogErrorFormat(Object context, string format, params object[] args) { }
	[Conditional("UNDEF")] public static void LogException(System.Exception exception, Object context) { }
	[Conditional("UNDEF")] public static void LogException(System.Exception exception) { }
	[Conditional("UNDEF")] public static void LogFormat(Object context, string format, params object[] args) { }
	[Conditional("UNDEF")] public static void LogFormat(string format, params object[] args) { }
	[Conditional("UNDEF")] public static void LogWarning(object message) { }
	[Conditional("UNDEF")] public static void LogWarning(object message, Object context) { }
	[Conditional("UNDEF")] public static void LogWarningFormat(string format, params object[] args) { }
	[Conditional("UNDEF")] public static void LogWarningFormat(Object context, string format, params object[] args) { }
}
#endif