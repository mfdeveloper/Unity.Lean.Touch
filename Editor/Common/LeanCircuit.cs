﻿using UnityEngine;
using System.Collections.Generic;

namespace Lean.Common.Examples
{
	/// <summary>This component generates a basic circuit mesh based on the specified paths, with circles at the end of each path, unless they intersect another.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter))]
	[AddComponentMenu("")]
	public class LeanCircuit : MonoBehaviour
	{
		[System.Serializable]
		public class Path
		{
			public List<Vector3> Points;
		}

		class Node
		{
			public Vector3 Point;
			public int     Count;

			public bool Increment(Vector3 p)
			{
				if (Point == p)
				{
					Count += 1;

					return true;
				}

				return false;
			}
		}

		public List<Path> Paths;

		public float LineRadius = 0.2f;

		public float PointRadius = 0.5f;

		public Color ShadowColor = Color.black;

		public Vector3 ShadowOffset = Vector3.right;

		[System.NonSerialized]
		private MeshFilter cachedMeshFilter;

		[System.NonSerialized]
		private bool cachedMeshFilterSet;

		[System.NonSerialized]
		private Mesh mesh;

		private static List<Vector3> positions = new List<Vector3>();

		private static List<Vector3> normals = new List<Vector3>();

		private static List<Color> colors = new List<Color>();

		private static List<Vector2> coords = new List<Vector2>();

		private static List<int> indices = new List<int>();

		private static List<Node> nodes = new List<Node>();

		[ContextMenu("Update Mesh")]
		public void UpdateMesh()
		{
			if (cachedMeshFilterSet == false)
			{
				cachedMeshFilter    = GetComponent<MeshFilter>();
				cachedMeshFilterSet = true;
			}

			if (mesh == null)
			{
				mesh = new Mesh();
#if UNITY_EDITOR
				mesh.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
#endif
				mesh.name = "Circuit";

				cachedMeshFilter.sharedMesh = mesh;
			}

			positions.Clear();
			normals.Clear();
			colors.Clear();
			coords.Clear();
			indices.Clear();
			nodes.Clear();

			if (Paths != null)
			{
				Populate();
			}

			mesh.Clear();
			mesh.SetVertices(positions);
			mesh.SetColors(colors);
			mesh.SetNormals(normals);
			mesh.SetUVs(0, coords);
			mesh.SetTriangles(indices, 0);
		}

		private void Populate()
		{
			// Write shadows
			foreach (var path in Paths)
			{
				if (path.Points != null)
				{
					for (var j = 1; j < path.Points.Count; j++)
					{
						var pointA = path.Points[j - 1];
						var pointB = path.Points[j];

						AddNode(pointA);
						AddNode(pointB);

						AddLine(ShadowOffset + pointA, ShadowOffset + pointB, ShadowColor);
					}
				}
			}

			foreach (var node in nodes)
			{
				if (node.Count == 1)
				{
					AddPoint(node.Point + ShadowOffset, PointRadius, ShadowColor);
				}
				else
				{
					AddPoint(node.Point + ShadowOffset, LineRadius, ShadowColor);
				}
			}

			// Write main
			foreach (var path in Paths)
			{
				if (path.Points != null)
				{
					for (var j = 1; j < path.Points.Count; j++)
					{
						var pointA = path.Points[j - 1];
						var pointB = path.Points[j];

						AddLine(pointA, pointB, Color.white);
					}
				}
			}

			foreach (var node in nodes)
			{
				if (node.Count == 1)
				{
					AddPoint(node.Point, PointRadius, Color.white);
				}
				else
				{
					AddPoint(node.Point, LineRadius, Color.white);
				}
			}
		}

		protected virtual void Start()
		{
			UpdateMesh();
		}
#if UNITY_EDITOR
		protected virtual void OnValidate()
		{
			if (mesh != null)
			{
				UpdateMesh();
			}
		}
#endif
		private void AddLine(Vector3 a, Vector3 b, Color color)
		{
			if (a != b)
			{
				var right = Vector3.Cross(a - b, Vector3.up).normalized * LineRadius;
				var index = positions.Count;

				positions.Add(a - right);
				positions.Add(a + right);
				positions.Add(b + right);
				positions.Add(b - right);

				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);

				normals.Add(Vector3.up);
				normals.Add(Vector3.up);
				normals.Add(Vector3.up);
				normals.Add(Vector3.up);

				coords.Add(Vector2.zero);
				coords.Add(Vector2.one);
				coords.Add(Vector2.one);
				coords.Add(Vector2.zero);

				indices.Add(index + 2);
				indices.Add(index + 1);
				indices.Add(index    );
				
				indices.Add(index + 3);
				indices.Add(index + 2);
				indices.Add(index    );
			}
		}

		private void AddPoint(Vector3 a, float radius, Color color)
		{
			var index = positions.Count;
			var count = 36;
			var step  = Mathf.PI * 2.0f / count;

			for (var i = 0; i < count; i++)
			{
				var angle = i * step;

				positions.Add(a + new Vector3(Mathf.Sin(angle) * radius, 0.0f, Mathf.Cos(angle) * radius));

				colors.Add(color);

				normals.Add(Vector3.up);

				coords.Add(new Vector2(0.5f, 0.5f));
			}

			for (var i = 2; i < count; i++)
			{
				indices.Add(index    );
				indices.Add(index + i - 1);
				indices.Add(index + i);
			}
		}

		private void AddNode(Vector3 point)
		{
			for (var i = nodes.Count - 1; i >= 0; i--)
			{
				var node = nodes[i];

				if (node.Increment(point) == true)
				{
					return;
				}
			}

			var addNode = new Node();

			addNode.Point = point;
			addNode.Count = 1;

			nodes.Add(addNode);
		}
	}
}