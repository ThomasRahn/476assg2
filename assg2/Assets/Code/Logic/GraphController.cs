using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphController : MonoBehaviour {

	public Graph graph = null;
	public Node start = null;
	// Use this for initialization
	public Node npcPosition;

	public IList<Node> open_list = new List<Node> ();
	public IList<Node> closed_list = new List<Node> ();
	void Start () {
		graph = new Graph ();
		start = graph.FindNode (new Vector3 (-3.5f,0,-4.0f));
	
		changeObjColor (start.obj, Color.cyan);
		StartCoroutine (AStar ());
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator AStar()
	{
		bool reached_goal = false;
		Node current_node = start;
		while (!reached_goal) 
		{


			yield return new WaitForSeconds (10.0f);
		}

	}
	private void changeObjColor(GameObject obj, Color c)
	{
		Transform cube = obj.transform.FindChild ("Cube");
		if (cube != null) {
			cube.renderer.material.color = c;
		}
	}

	public static GameObject makeBlock(Vector3 position, Node n)
	{

		GameObject node = Instantiate(Resources.Load("Prefabs/Node"), position, Quaternion.identity)as GameObject;
		node.GetComponent<GraphElement> ().setNode (n);
		if (position == Graph.originPosition) {
			node.transform.FindChild("Cube").renderer.material.color = Color.red;
		}
		return node;
	}

	public static void makeLine(Vector3 start, Vector3 end)
	{
		GameObject line = Instantiate(Resources.Load("Prefabs/Line"), new Vector3(0,-10,0), Quaternion.identity)as GameObject;
		LineRenderer lineR = line.GetComponent<LineRenderer> ();
		lineR.SetPosition (0, start + new Vector3(0,0.3f,0));
		lineR.SetPosition (1, end + new Vector3(0,0.3f,0));
		lineR.renderer.material.color = Color.red;

	}

}
