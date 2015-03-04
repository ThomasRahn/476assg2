using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GraphController : MonoBehaviour {

	public Graph graph = null;
	public Node start = null;
	// Use this for initialization
	public Node npcPosition;

	public IList<Node> open_list = new List<Node> ();
	public IList<Node> closed_list = new List<Node> ();
	public algorithm current;
	private static Color default_color = Color.yellow;

	public Text behaviourType;

	public enum algorithm {
		dijkstra,
		euclidean,
		cluster
	};

	void Start () {
		graph = new Graph ();
		start = graph.FindNode (new Vector3 (-3.5f,0,-4.0f));
		current = algorithm.euclidean;
		changeObjColor (start.obj, Color.cyan);
		euclidean ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A) && current != algorithm.euclidean) {
			Debug.Log("euc");
			current = algorithm.euclidean;
			Reset();
			euclidean();
		} else if (Input.GetKeyDown(KeyCode.D) && current != algorithm.dijkstra) {
			Debug.Log("dij");
			current = algorithm.dijkstra;
			Reset();
			dijkstra();
		}
		behaviourType.text = "Algorith: " + current.ToString ();
	}
	private void Reset()
	{
		foreach (Node n in open_list) {
			changeObjColor (n.obj, GraphController.default_color);
		}
		open_list.Clear ();
		foreach (Node n in closed_list) {
			changeObjColor (n.obj, GraphController.default_color);
		}
		closed_list.Clear ();


		changeObjColor (graph.FindNode(Graph.originPosition).obj, Color.red);
	}
	void euclidean()
	{
		bool reached_goal = false;
		Node current_node = null;
		open_list.Add (start);
		while (!reached_goal) 
		{
			if(open_list.Count == 0)
			{
				break;
			}
			current_node = GetLowerCost();
			if(current_node.position == Graph.originPosition)
			{
				break;
			}
			foreach(Node n in current_node.getAllConnectingNodes())
			{
				if(!closed_list.Contains(n) && !open_list.Contains(n))
				{
					open_list.Add(n);
					if(n.position != Graph.originPosition)
						Inspect(n);
				}
			}

			GoToNode(current_node);
			open_list.Remove(current_node);
			closed_list.Add(current_node);
		}
	}

	void dijkstra()
	{
		bool reached_goal = false;
		Node current_node = null;
		open_list.Add (start);
		while (!reached_goal) {
			if (open_list.Count == 0) {
				break;
			}
			current_node = open_list [0];
			if (current_node.position == Graph.originPosition) {
				break;
			}
			foreach (Node n in current_node.getAllConnectingNodes()) {
				if (!closed_list.Contains (n) && !open_list.Contains (n)) {
					open_list.Add (n);
					if (n.position != Graph.originPosition)
						Inspect (n);
				}
			}

			GoToNode(current_node);
			open_list.Remove(current_node);
			closed_list.Add(current_node);

		}
	}
	private Node GetLowerCost()
	{
		if (open_list.Count == 0) {
			return null;
		}
		Node node = open_list [0];

		foreach (Node n in open_list) {
			if(n.cost < node.cost)
				node = n;
		}
		return node;
	}
	private void Inspect(Node node)
	{
		changeObjColor (node.obj, Color.magenta);
	}
	private void GoToNode(Node node)
	{
		changeObjColor (node.obj, Color.cyan);
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
			node.transform.FindChild ("Cube").renderer.material.color = Color.red;
		} else {
			node.transform.FindChild ("Cube").renderer.material.color = GraphController.default_color;
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
