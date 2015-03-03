using UnityEngine;
using System.Collections;

public class GraphElement : MonoBehaviour {

	public Node node;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setNode(Node n)
	{
		this.node = n;
	}
}
