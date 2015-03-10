using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GraphElement : MonoBehaviour {

	public Node node;
	public Vector3 start;
	public Vector3[] nodes;
	public string cluster;
	// Use this for initialization
	void Start () {
		nodes = new Vector3[10];
	}
	
	// Update is called once per frame
	void Update () {
		this.cluster = node.cluster.ToString ();
	}

	public void setNode(Node n)
	{
		this.node = n;
		start = node.position;
	}
}
