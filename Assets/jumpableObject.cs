using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class jumpableObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3[] getNormals(Vector3 player) {
		Mesh mesh = this.GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		List<Vector3> applicableNormals = new List<Vector3>();

		for (int i = 0; i < mesh.normals.Length; i++) {
			Vector3 vertex = vertices [i];
			Vector3 normal = normals [i];

			if (isOnSide(vertex.x, player.x, normal.x) 
				&& isOnSide(vertex.y, player.y, normal.y) 
				&& isOnSide(vertex.z, player.z, normal.z)) {
				applicableNormals.Add (normal);
			}
		}

		return applicableNormals.ToArray();
	}

	bool isOnSide(float v, float p, float n) {
		if (n > 0) {
			return v > p;
		} else if (n < 0) {
			return v < p;
		}

		return true;
	}

	public Vector3[] getVertices() {
		return this.GetComponent<MeshFilter> ().mesh.vertices;
	}
}
