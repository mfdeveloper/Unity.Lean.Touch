#if UNITY_EDITOR

using UnityEditor;
using Lean.Common;

namespace Lean.Touch
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanFingerUp))]
	public class LeanFingerUp_Inspector : LeanInspector<LeanFingerUp>
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("IgnoreStartedOverGui");
			Draw("IgnoreIsOverGui");
			Draw("RequiredSelectable");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnFinger.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnPosition.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onFinger");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("ScreenDepth");
				Draw("onPosition");
			}
		}
	}
}
#endif