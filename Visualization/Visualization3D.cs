using Godot;
using System;
using System.Collections.Generic; 

public partial class Visualization3D : MeshInstance3D
{
	private Vector2I terrainVertexAmount;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetTerrainVertexAmount(new Vector2I(512, 512));
		InstantiateMesh(0.01, 1.0f, new Vector2(0.0f, 0.0f));
	}

	public void SetTerrainVertexAmount(Vector2I v)
	{
		terrainVertexAmount = v;
	}

	public void InstantiateMesh(double sizeStep, float heightmapHeight, Vector2 noiseOrigin) {
		// reference: https://docs.godotengine.org/en/stable/tutorials/3d/procedural_geometry/arraymesh.html
		Godot.Collections.Array surfaceArray = new Godot.Collections.Array();
		surfaceArray.Resize((int)Mesh.ArrayType.Max);

		List<Vector3> verts = new List<Vector3>();
		List<Color> color = new List<Color>();
		List<int> indices = new List<int>();

		// Bestem heightmap data
		int thisRow = 0;
        int prevRow = 0;
		int pointIndex = 0;

		for (int i = 0; i < terrainVertexAmount.X; i++)
		{
			for (int j = 0; j < terrainVertexAmount.Y; j++)
			{
				double x = (double)(noiseOrigin.X + i * sizeStep);
				double z = (double)(noiseOrigin.Y + j * sizeStep);

				double noiseValue = (Simplex.noise(x, z) + 1.0f) / 2.0f;
				double y = noiseValue * heightmapHeight;

				// Addere vertex data
				Vector3 vert = new Vector3((float)x, (float)y, (float)z);
				verts.Add(vert);
				color.Add(new Color((float)noiseValue, (float)noiseValue, (float)noiseValue));
				pointIndex++;

				// Addere quad-mesh data, i.e. 2 trekanter
				if (i > 0 && j > 0) {
					indices.Add(prevRow + j - 1);
                    indices.Add(thisRow + j - 1);
                    indices.Add(prevRow + j);

                    indices.Add(prevRow + j);
                    indices.Add(thisRow + j - 1);
                    indices.Add(thisRow + j);
				}
			}
			prevRow = thisRow;
            thisRow = pointIndex;
		}

		// Set mesh
		surfaceArray[(int)Mesh.ArrayType.Vertex] = verts.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Index] = indices.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Color] = color.ToArray();
		
		//SurfaceTool surfaceTool = new SurfaceTool();
		ArrayMesh arrMesh = Mesh as ArrayMesh;
        if (arrMesh != null)
        {
            arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
			//arrMesh.RegenNormalMaps();
			//surfaceTool.CreateFrom(arrMesh, 0);
			//surfaceTool.GenerateNormals();
			//arrMesh = surfaceTool.Commit();
        }


		// Center mesh til origo
		Position = new Vector3(
			-(float)(terrainVertexAmount.X * sizeStep * 0.5),
			0.0f,
			-(float)(terrainVertexAmount.Y * sizeStep * 0.5)
		);
	}
}
