using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BzKovSoft.ObjectSlicer
{
	public class BzMeshData
	{
		public List<Vector3> Vertices;
		public List<Vector3> Normals;

		public List<Color> Colors;
		public List<Color32> Colors32;

		public List<Vector2> UV;
		public List<Vector2> UV2;
		public List<Vector2> UV3;
		public List<Vector2> UV4;
		public List<Vector4> Tangents;

		public List<BoneWeight> BoneWeights;
		public Matrix4x4[] Bindposes;

		public int[][] SubMeshes;

		public Material[] Materials;

		public bool NormalsExists { get { return Normals != null; } }
		public bool ColorsExists { get { return Colors != null; } }
		public bool Colors32Exists { get { return Colors32 != null; } }
		public bool UVExists { get { return UV != null; } }
		public bool UV2Exists { get { return UV2 != null; } }
		public bool UV3Exists { get { return UV3 != null; } }
		public bool UV4Exists { get { return UV4 != null; } }
		public bool TangentsExists { get { return Tangents != null; } }
		public bool BoneWeightsExists { get { return BoneWeights != null; } }
		public bool MaterialsExists { get { return Materials != null; } }


		public BzMeshData(Mesh initFrom, Material[] materials)
		{
			//if (initFrom.blendShapeCount != 0)
			//	throw new NotSupportedException("blendShapes not supported. Please contact publisher to fix it");

			Materials = materials;

			var normals = initFrom.normals;
			var colors = initFrom.colors;
			var colors32 = initFrom.colors32;
			var uv = initFrom.uv;
			var uv2 = initFrom.uv2;
			var uv3 = initFrom.uv3;
			var uv4 = initFrom.uv4;
			var tangents = initFrom.tangents;
			var boneWeights = initFrom.boneWeights;
			var bindposes = initFrom.bindposes;

			SubMeshes = new int[initFrom.subMeshCount][];
			//for (int subMeshIndex = 0; subMeshIndex < initFrom.subMeshCount; ++subMeshIndex)
			//	SubMeshes[subMeshIndex] = initFrom.GetTriangles(subMeshIndex);

			Vertices = new List<Vector3>(initFrom.vertices);
			if (normals.Length != 0) Normals = new List<Vector3>(normals);
			if (colors.Length != 0) Colors = new List<Color>(colors);
			if (colors32.Length != 0) Colors32 = new List<Color32>(colors32);
			if (uv.Length != 0) UV = new List<Vector2>(uv);
			if (uv2.Length != 0) UV2 = new List<Vector2>(uv2);
			if (uv3.Length != 0) UV3 = new List<Vector2>(uv3);
			if (uv4.Length != 0) UV4 = new List<Vector2>(uv4);
			if (tangents.Length != 0) Tangents = new List<Vector4>(tangents);
			if (boneWeights.Length != 0) BoneWeights = new List<BoneWeight>(boneWeights);
			if (boneWeights.Length != 0) Bindposes = bindposes;
		}

		public Mesh GenerateMesh()
		{
			Mesh mesh = new Mesh();
			
			mesh.vertices = Vertices.ToArray();
			if (NormalsExists)
				mesh.normals = Normals.ToArray();

			if (ColorsExists)
				mesh.colors = Colors.ToArray();
			if (Colors32Exists)
				mesh.colors32 = Colors32.ToArray();

			if (UVExists)
				mesh.uv = UV.ToArray();
			if (UV2Exists)
				mesh.uv2 = UV2.ToArray();
			if (UV3Exists)
				mesh.uv3 = UV3.ToArray();
			if (UV4Exists)
				mesh.uv4 = UV4.ToArray();

			if (TangentsExists)
				mesh.tangents = Tangents.ToArray();

			if (BoneWeightsExists)
			{
				mesh.boneWeights = BoneWeights.ToArray();
				mesh.bindposes = Bindposes;
			}
			
			mesh.subMeshCount = SubMeshes.Length;
			for (int subMeshIndex = 0; subMeshIndex < SubMeshes.Length; ++subMeshIndex)
				mesh.SetTriangles(SubMeshes[subMeshIndex], subMeshIndex);

			return mesh;
		}
	}
}
