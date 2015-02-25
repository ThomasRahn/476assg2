using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GraphController : MonoBehaviour {

	public Graph graph;

	public List<Vector3> gridPositions = new List<Vector3>();
	// Use this for initialization
	void Start () {
		graph = new Graph ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void makeBlock(Vector3 position, Node n)
	{
		GameObject node = Instantiate(Resources.Load("Prefabs/Node"), position, Quaternion.identity)as GameObject;
		node.GetComponent<GraphElement> ().setNode (n);
	}

}
