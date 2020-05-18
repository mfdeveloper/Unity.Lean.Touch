using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Lean.Common.Examples
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanCircuit))]
	public class LeanCircuit_Inspector : LeanInspector<LeanCircuit>
	{
		private int currentPath;

		protected override void DrawInspector()
		{
			if (Target.Paths != null)
			{
				currentPath = EditorGUILayout.IntSlider(currentPath, 0, Target.Paths.Count - 1);
			}

			EditorGUILayout.Separator();

			Draw("LineRadius");
			Draw("PointRadius");
			Draw("ShadowColor");
			Draw("ShadowOffset");

			EditorGUILayout.Separator();

			Draw("Paths");

			Target.UpdateMesh();
		}

		protected override void DrawScene()
		{
			var dirty  = false;
			var matrix = Target.transform.localToWorldMatrix;

			Undo.RecordObject(Target, "Points Changed");

			if (Target.Paths != null && currentPath >= 0 && currentPath < Target.Paths.Count)
			{
				var path = Target.Paths[currentPath];

				if (path.Points != null)
				{
					Handles.matrix = matrix;

					Handles.BeginGUI();
					{
						for (var i = 0; i < path.Points.Count; i++)
						{
							var point     = path.Points[i];
							var pointName = "Point " + i;
							var scrPoint  = Camera.current.WorldToScreenPoint(matrix.MultiplyPoint(point));
							var rect      = new Rect(0.0f, 0.0f, 50.0f, 20.0f); rect.center = new Vector2(scrPoint.x, Screen.height - scrPoint.y - 35.0f);
							var rect1     = rect; rect.x += 1.0f;
							var rect2     = rect; rect.x -= 1.0f;
							var rect3     = rect; rect.y += 1.0f;
							var rect4     = rect; rect.y -= 1.0f;

							GUI.Label(rect1, pointName, EditorStyles.miniBoldLabel);
							GUI.Label(rect2, pointName, EditorStyles.miniBoldLabel);
							GUI.Label(rect3, pointName, EditorStyles.miniBoldLabel);
							GUI.Label(rect4, pointName, EditorStyles.miniBoldLabel);
							GUI.Label(rect, pointName, EditorStyles.whiteMiniLabel);
						}

						for (var i = 1; i < path.Points.Count; i++)
						{
							var pointA   = path.Points[i - 1];
							var pointB   = path.Points[i];
							var midPoint = (pointA + pointB) * 0.5f;
							var scrPoint = Camera.current.WorldToScreenPoint(matrix.MultiplyPoint(midPoint));
				
							if (GUI.Button(new Rect(scrPoint.x - 5.0f, Screen.height - scrPoint.y - 45.0f, 20.0f, 20.0f), "+") == true)
							{
								path.Points.Insert(i, midPoint); dirty = true;
							}
						}
					}
					Handles.EndGUI();

					for (var i = 0; i < path.Points.Count; i++)
					{
						var oldPoint = path.Points[i];
						var newPoint = Handles.PositionHandle(oldPoint, Quaternion.identity);

						if (oldPoint != newPoint)
						{
							newPoint.x = Mathf.Round(newPoint.x);
							newPoint.y = Mathf.Round(newPoint.y);
							newPoint.z = Mathf.Round(newPoint.z);

							path.Points[i] = newPoint; dirty = true;
						}
					}
				}
			}

			if (dirty == true)
			{
				EditorUtility.SetDirty(Target);
			}
		}
	}
}
#endif
