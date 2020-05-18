#if UNITY_EDITOR
using UnityEditor;

namespace Lean.Common.Examples
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanLinkTo))]
	public class LeanLinkTo_Editor : LeanInspector<LeanLinkTo>
	{
		protected override void DrawInspector()
		{
			base.DrawInspector();
		}
	}
}
#endif