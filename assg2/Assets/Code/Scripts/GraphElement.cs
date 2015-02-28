using UnityEngine;
using System.Collections;

public class GraphElement : MonoBehaviour {

	public Node node;
	public Vector3 pos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		pos = node.position;
	}

	public void setNode(Node n)
	{
		this.node = n;
	}
}
