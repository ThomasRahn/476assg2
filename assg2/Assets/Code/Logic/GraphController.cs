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
	public GameObject NPC;

	public enum algorithm {
		dijkstra,
		euclidean,
		cluster
	};

	void Start () {
		graph = new Graph ();
		CreateNPC ();
		current = algorithm.euclidean;
		euclidean ();
		//dijkstra ();
	}

	void CreateNPC()
	{
		start = graph.FindNode (new Vector3 (-3.25f,0,-2.95f));
		changeObjColor (start.obj, Color.cyan);
		NPC = Instantiate (Resources.Load ("Prefabs/Npc"), start.position + new Vector3(0,1,0), Quaternion.identity) as GameObject;
		NPC.GetComponent<NPCMovement> ().currentPosition = start;

	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A) && current != algorithm.euclidean) {
			current = algorithm.euclidean;
			start = NPC.GetComponent<NPCMovement>().nextNode;
			Reset();
			euclidean();
		} else if (Input.GetKeyDown(KeyCode.D) && current != algorithm.dijkstra) {
			current = algorithm.dijkstra;
			start = NPC.GetComponent<NPCMovement>().nextNode;
			Reset();
			dijkstra();
		}
		behaviourType.text = "Algorith: " + current.ToString ();

		if (Input.GetMouseButtonDown (0)) 
		{
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] rayHits = Physics.RaycastAll(mouseRay);
			foreach(RaycastHit hit in rayHits)
			{
				if(hit.collider.gameObject.CompareTag("cube"))
				{
					Transform parent = hit.collider.gameObject.transform.parent;
					Node node = graph.FindNode(parent.position);
					Graph.originPosition = node.position;
					start = NPC.GetComponent<NPCMovement>().nextNode;
					switch(current)
					{
					case algorithm.dijkstra:
						Reset();
						dijkstra();
						break;
					case algorithm.euclidean:
						Reset();
						euclidean();
						break;
					}
					changeObjColor(node.obj,Color.red);
				}
			}
		}

	}
	private void Reset()
	{
		foreach (Node n in graph.nodes) {
			changeObjColor (n.obj, GraphController.default_color);
			n.ResetCost();
			n.heuristic = n.cost;
		}

		open_list.Clear ();
		closed_list.Clear ();

		changeObjColor (graph.FindNode(Graph.originPosition).obj, Color.red);
	}

	void euclidean()
	{
		open_list.Add (start);
		start.prevNode = null;
		start.heuristic = start.cost;
		Node current_node = null;
		while (open_list.Count != 0) 
		{
			current_node = GetLowerCost();
			if(current_node == null)
				break;

			open_list.Remove(current_node);

			if(current_node.position == Graph.originPosition)
			{
				current_node.prevNode = closed_list[closed_list.Count - 1];
				break;
			}else{
				foreach(Node node in current_node.getAllConnectingNodes())
				{
					Inspect(node);

					if(!open_list.Contains(node) && !closed_list.Contains(node))
					{
						node.prevNode = current_node;
						node.heuristic = node.cost + current_node.heuristic;
						open_list.Add(node);
					}
					else
					{
						if(node.heuristic > (node.cost + current_node.heuristic))
						{
							node.heuristic = node.cost + current_node.heuristic;
						}
					}
				}
				closed_list.Add(current_node);
			}

		}

		IList<Node> actual_path = new List<Node> ();
		int counter = 0;
		while (current_node.prevNode != null && counter < 200) 
		{
			counter++;
			changeObjColor(current_node.obj, Color.cyan);
			actual_path.Add(current_node);
			current_node = current_node.prevNode;
		}
		changeObjColor (graph.FindNode(Graph.originPosition).obj, Color.red);
		NPC.GetComponent<NPCMovement> ().path = actual_path;
	}

	void dijkstra()
	{
		IList<Node> pathList = new List<Node>();
		open_list.Add (start);
		
		while (open_list[0].position != Graph.originPosition) {
			Node node = open_list[0];
			Node[] neighborList = open_list[0].getAllConnectingNodes();
			for (int i = 0; i < neighborList.Length; i++) {
				if (!closed_list.Contains (neighborList[i]) && !open_list.Contains (neighborList[i])) {
					neighborList[i].prevNode = node;
					open_list.Add(neighborList[i]);
					Inspect(neighborList[i]);
				}
			}
			closed_list.Add (node);
			open_list.Remove(node);
		}

		pathList.Add (open_list [0]);
		while(true) {
			
			if (pathList[pathList.Count - 1].prevNode.position == start.position) {
				pathList.Add (pathList[pathList.Count - 1].prevNode);
				break;
			}
			else {
				pathList.Add (pathList[pathList.Count - 1].prevNode);
			}
		}

		foreach (Node n in pathList) {
			GoToNode(n);
		}
		NPC.GetComponent<NPCMovement> ().path = pathList;
		changeObjColor (graph.FindNode(Graph.originPosition).obj, Color.red);
	}

	private Node GetLowerCost()
	{
		if (open_list.Count == 0) {
			return null;
		}
		Node node = open_list [0];

		foreach (Node n in open_list) {
			if(n.heuristic < node.heuristic)
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
