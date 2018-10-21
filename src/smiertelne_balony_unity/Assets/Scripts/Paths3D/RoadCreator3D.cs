﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathCreator3D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class RoadCreator3D : MonoBehaviour {

    [Range(.05f, 1.5f)]
    public float plynnosc = 1;
    public float rozmiarDrogi = 1;
    public bool automatycznaAktualizacja;
    public float kafelkowanie = 1;

    public void UpdateRoad() {
        Path3D path = GetComponent<PathCreator3D>().path;
        Vector2[] points = VectorConverter.ConvertToVectors2( path.CalculateEvenlySpacedPoints( plynnosc ), VectorConvOptions.XtoX, VectorConvOptions.ZtoY );
        GetComponent<MeshFilter>().mesh = CreateRoadMesh( points, path.IsClosed );

        int textureRepeat = Mathf.RoundToInt( kafelkowanie * points.Length * plynnosc * 0.05f );
        if ( GetComponent<MeshRenderer>().sharedMaterial != null ) {
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2( 1, textureRepeat );
        }
    }

    Mesh CreateRoadMesh( Vector2[] points, bool isClosed ) {
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for ( int i = 0; i < points.Length; i++ ) {
            Vector2 forward = Vector2.zero;
			
            if ( i < points.Length - 1 || isClosed ) {
                forward += points[(i+1) % points.Length] - points[i];
            }
			
            if ( i > 0 || isClosed ) {
                forward += points[i] - points[(i-1+points.Length) % points.Length];
            }

            forward.Normalize();
            Vector2 left = new Vector2( -forward.y, forward.x );

            verts[vertIndex] = points[i] + left * rozmiarDrogi * 0.5f;
            verts[vertIndex + 1] = points[i] - left * rozmiarDrogi * 0.5f;

            float completionPercent = i / (float)( points.Length-1 );
            float v = 1 - Mathf.Abs( 2 * completionPercent - 1 );
            uvs[vertIndex] = new Vector2( 0, v );
            uvs[vertIndex + 1] = new Vector2( 1, v );

            if (i < points.Length - 1 || isClosed) {
				tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
				tris[triIndex + 2] = vertIndex + 1;

				tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 5] = (vertIndex + 3)  % verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        return mesh;
    }
}
