#if UNITY_EDITOR

using UnityEditor;
using Lean.Common;

namespace Lean.Touch
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanSelectable))]
	public class LeanSelectable_Inspector : LeanInspector<LeanSelectable>
	{
		private bool showUnusedEvents;

		// Draw the whole inspector
		protected override void DrawInspector()
		{
			// isSelected modified?
			if (Draw("isSelected") == true)
			{
				// Grab the new value
				var isSelected = serializedObject.FindProperty("isSelected").boolValue;

				// Apply it directly to each instance before the SerializedObject applies it when this method returns
				Each(t => t.IsSelected = isSelected);
			}
			Draw("DeselectOnUp");
			Draw("HideWithFinger");
			Draw("IsolateSelectingFingers");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnSelect.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnSelectSet.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnSelectUp.GetPersistentEventCount() > 0);
			var usedD = Any(t => t.OnDeselect.GetPersistentEventCount() > 0);

			showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onSelect");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onSelectSet");
			}

			if (usedC == true || showUnusedEvents == true)
			{
				Draw("onSelectUp");
			}

			if (usedD == true || showUnusedEvents == true)
			{
				Draw("onDeselect");
			}
		}
	}
}
#endif