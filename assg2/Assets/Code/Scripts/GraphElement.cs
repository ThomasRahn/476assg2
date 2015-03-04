using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GraphElement : MonoBehaviour {

	public Node node;
	public Vector3 start;
	public Vector3[] nodes;
	// Use this for initialization
	void Start () {
		nodes = new Vector3[8];
	}
	
	// Update is called once per frame
	void Update () {
		/*for (int i = 0; i < node.edges.Count; i++){
			nodes [i] = node.edges[i].end.position;
		}*/
	}

	public void setNode(Node n)
	{
		this.node = n;
		start = node.position;
	}
}
