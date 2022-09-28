using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrainface
{
    ShapeGenerator shapeGenerator;
    public Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA; // perpendicular to localUp
    Vector3 axisB; // perpendicular to localUp & perpendicular to axisA

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;

    public Terrainface(ShapeGenerator _shapeGenerator, Mesh _mesh, int _resolution, Vector3 _localUp)
    {
        this.shapeGenerator = _shapeGenerator;
        this.mesh = _mesh;
        this.resolution = _resolution;
        this.localUp = _localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        triangles = new int[(resolution) * (resolution) * 6];
        uv = new Vector2[(resolution + 1) * (resolution + 1)];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)

        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = PointOnCubeToPointOnSphere(pointOnUnitCube);

                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;

                    uv[i] = new Vector2(x, y);
                }
                else
                {
                    uv[i] = new Vector2(x, y);
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();
    }

    public static Vector3 PointOnCubeToPointOnSphere(Vector3 p)
    {
        float x2 = p.x * p.x;
        float y2 = p.y * p.y;
        float z2 = p.z * p.z;

        float x = p.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 * z2) / 3);
        float y = p.y * Mathf.Sqrt(1 - (z2 + x2) / 2 + (z2 * x2) / 3);
        float z = p.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 * y2) / 3);

        return new Vector3(x, y, z);
    }
}
