#if UNITY_EDITOR

using UnityEditor;
using Lean.Common;

namespace Lean.Touch
{
	public abstract class LeanSwipeBase_Inspector<T> : LeanInspector<LeanSwipeBase>
		where T : LeanSwipeBase
	{
		private bool showUnusedEvents;

		protected override void DrawInspector()
		{
			Draw("RequiredAngle");
			Draw("RequiredArc");

			EditorGUILayout.Separator();

			var usedA = Any(t => t.OnFinger.GetPersistentEventCount() > 0);
			var usedB = Any(t => t.OnDelta.GetPersistentEventCount() > 0);
			var usedC = Any(t => t.OnDistance.GetPersistentEventCount() > 0);
			var usedD = Any(t => t.OnWorldFrom.GetPersistentEventCount() > 0);
			var usedE = Any(t => t.OnWorldTo.GetPersistentEventCount() > 0);
			var usedF = Any(t => t.OnWorldDelta.GetPersistentEventCount() > 0);
			var usedG = Any(t => t.OnWorldFromTo.GetPersistentEventCount() > 0);

			EditorGUI.BeginDisabledGroup(usedA && usedB && usedC && usedD && usedE && usedF && usedG);
				showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			if (usedA == true || showUnusedEvents == true)
			{
				Draw("onFinger");
			}

			if (usedB == true || usedC == true || showUnusedEvents == true)
			{
				Draw("Modify");
				Draw("Coordinate");
				Draw("Multiplier");
			}

			if (usedB == true || showUnusedEvents == true)
			{
				Draw("onDelta");
			}

			if (usedC == true || showUnusedEvents == true)
			{
				Draw("onDistance");
			}

			if (usedD == true || usedE == true || usedF == true || usedG == true || showUnusedEvents == true)
			{
				Draw("ScreenDepth");
			}

			if (usedD == true || showUnusedEvents == true)
			{
				Draw("onWorldFrom");
			}

			if (usedE == true || showUnusedEvents == true)
			{
				Draw("onWorldTo");
			}

			if (usedF == true || showUnusedEvents == true)
			{
				Draw("onWorldDelta");
			}

			if (usedG == true || showUnusedEvents == true)
			{
				Draw("onWorldFromTo");
			}
		}
	}
}
#endif