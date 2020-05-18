#if UNITY_EDITOR

using UnityEditor;

namespace Lean.Touch
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanFingerSwipe))]
	public class LeanFingerSwipe_Inspector : LeanSwipeBase_Inspector<LeanFingerSwipe>
	{
		protected override void DrawInspector()
		{
			Draw("IgnoreStartedOverGui");
			Draw("IgnoreIsOverGui");
			Draw("RequiredSelectable");

			base.DrawInspector();
		}
	}
}
#endif