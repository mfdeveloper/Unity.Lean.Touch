#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Lean.Touch
{
	[CustomEditor(typeof(LeanTouch))]
	public class LeanTouch_Editor : Editor
	{
		private static List<LeanFinger> allFingers = new List<LeanFinger>();

		private static GUIStyle fadingLabel;

		[MenuItem("GameObject/Lean/Touch", false, 1)]
		public static void CreateTouch()
		{
			var gameObject = new GameObject(typeof(LeanTouch).Name);

			Undo.RegisterCreatedObjectUndo(gameObject, "Create Touch");

			gameObject.AddComponent<LeanTouch>();

			Selection.activeGameObject = gameObject;
		}

		// Draw the whole inspector
		public override void OnInspectorGUI()
		{
			if (LeanTouch.Instances.Count > 1)
			{
				EditorGUILayout.HelpBox("There is more than one active and enabled LeanTouch...", MessageType.Warning);

				EditorGUILayout.Separator();
			}

			var touch = (LeanTouch)target;

			EditorGUILayout.Separator();

			DrawSettings(touch);

			EditorGUILayout.Separator();

			DrawFingers(touch);

			EditorGUILayout.Separator();

			Repaint();
		}

		private void DrawSettings(LeanTouch touch)
		{
			DrawDefault("TapThreshold");
			DrawDefault("SwipeThreshold");
#if LEAN_ALLOW_RECLAIM
			DrawDefault("ReclaimThreshold");
#endif
			DrawDefault("ReferenceDpi");
			DrawDefault("GuiLayers");

			EditorGUILayout.Separator();

			DrawDefault("RecordFingers");
			
			if (touch.RecordFingers == true)
			{
				EditorGUI.indentLevel++;
					DrawDefault("RecordThreshold");
					DrawDefault("RecordLimit");
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Separator();

			DrawDefault("SimulateMultiFingers");

			if (touch.SimulateMultiFingers == true)
			{
				EditorGUI.indentLevel++;
					DrawDefault("PinchTwistKey");
					DrawDefault("MovePivotKey");
					DrawDefault("MultiDragKey");
					DrawDefault("FingerTexture");
				EditorGUI.indentLevel--;
			}
		}

		private void DrawFingers(LeanTouch touch)
		{
			EditorGUILayout.LabelField("Fingers", EditorStyles.boldLabel);

			allFingers.Clear();
			allFingers.AddRange(LeanTouch.Fingers);
			allFingers.AddRange(LeanTouch.InactiveFingers);
			allFingers.Sort((a, b) => a.Index.CompareTo(b.Index));

			for (var i = 0; i < allFingers.Count; i++)
			{
				var finger   = allFingers[i];
				var progress = touch.TapThreshold > 0.0f ? finger.Age / touch.TapThreshold : 0.0f;
				var style    = GetFadingLabel(finger.Set, progress);

				if (style.normal.textColor.a > 0.0f)
				{
					var screenPosition = finger.ScreenPosition;

					EditorGUILayout.LabelField("#" + finger.Index + " x " + finger.TapCount + " (" + Mathf.FloorToInt(screenPosition.x) + ", " + Mathf.FloorToInt(screenPosition.y) + ") - " + finger.Age.ToString("0.0"), style);
				}
			}
		}

		private void DrawDefault(string name)
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(serializedObject.FindProperty(name));

			if (EditorGUI.EndChangeCheck() == true)
			{
				serializedObject.ApplyModifiedProperties();
			}
		}

		private static GUIStyle GetFadingLabel(bool active, float progress)
		{
			if (fadingLabel == null)
			{
				fadingLabel = new GUIStyle(EditorStyles.label);
			}

			var a = EditorStyles.label.normal.textColor;
			var b = a; b.a = active == true ? 0.5f : 0.0f;

			fadingLabel.normal.textColor = Color.Lerp(a, b, progress);

			return fadingLabel;
		}
	}
}
#endif