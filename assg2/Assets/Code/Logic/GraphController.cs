using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphController : MonoBehaviour {

	public Graph graph = null;
	public Node start = null;
	// Use this for initialization
	public Node npcPosition;

	public List<Edge> open_list = new List<Edge> ();
	public IList<Edge> closed_list = new List<Edge> ();
	void Start () {
		graph = new Graph ();
		start = graph.FindNode (new Vector3 (-3.5f,0,-4.0f));

		open_list.Sort(
			delegate(Edge e1, Edge e2)
			{
			return e1.getCost().CompareTo(e2.getCost());
			}
		);

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
			foreach(Edge edge in current_node.edges)
			{
				Debug.Log(edge.end.position);
				if(!open_list.Contains(edge) && !closed_list.Contains(edge))
				{
					open_list.Add(edge);
					changeObjColor(edge.end.obj, Color.magenta);
				}
			}
			open_list.Sort();

			current_node = open_list[0].end;
			closed_list.Add(open_list[0]);
			open_list.RemoveAt(0);

			changeObjColor(current_node.obj,Color.cyan);

			if(current_node.position == Graph.originPosition)
			{
				reached_goal = true;
			}

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
