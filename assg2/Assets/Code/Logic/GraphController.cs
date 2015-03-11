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

	public ClusterManager cm;

	//3 possible A* algorithms
	public enum algorithm {
		dijkstra,
		euclidean,
		cluster
	};

	void Start () {
		//Create the graph
		graph = new Graph ();

		//Set initial spawn point for user
		start = graph.FindNode (new Vector3 (-3.25f,0,-2.95f));
		ObjectCreator.changeObjColor (start.obj, Color.cyan);

		//Create NPC
		NPC = ObjectCreator.CreateNPC (start);

		//Makew the cluster manager
		cm = new ClusterManager (graph);

		//Initial algorithm
		current = algorithm.euclidean;
		euclidean ();
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
		}else if (Input.GetKeyDown(KeyCode.C) && current != algorithm.cluster) {
			current = algorithm.cluster;
			start = NPC.GetComponent<NPCMovement>().nextNode;
			Reset();
			cluster();
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
					case algorithm.cluster:
						Reset();
						cluster();
						break;
					}
					ObjectCreator.changeObjColor(node.obj,Color.red);
				}
			}
		}

	}

	//Resets all nodes to un-inspected and clears lists.
	private void Reset()
	{
		foreach (Node n in graph.nodes) {
			ObjectCreator.changeObjColor (n.obj, GraphController.default_color);
			n.ResetCost();
			n.heuristic = n.cost;
		}

		open_list.Clear ();
		closed_list.Clear ();

		ObjectCreator.changeObjColor (graph.FindNode(Graph.originPosition).obj, Color.red);
	}

	//Performs A* algorithm using euclidean distance to find the shortest path.
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
					ObjectCreator.Inspect(node);

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
			ObjectCreator.changeObjColor(current_node.obj, Color.cyan);
			actual_path.Add(current_node);
			current_node = current_node.prevNode;
		}
		ObjectCreator.changeObjColor (graph.FindNode(Graph.originPosition).obj, Color.red);
		NPC.GetComponent<NPCMovement> ().path = actual_path;
	}

	//Performs Dijkstra to find the shortest path. Null heuristic (no priority)
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
					ObjectCreator.Inspect(neighborList[i]);
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
			ObjectCreator.GoToNode(n);
		}
		NPC.GetComponent<NPCMovement> ().path = pathList;
		ObjectCreator.changeObjColor (graph.FindNode(Graph.originPosition).obj, Color.red);
	}

	void cluster()
	{
		open_list.Add (start);
		start.prevNode = null;
		start.heuristic = 0.0f;
		Node current_node = null;
		while (open_list.Count != 0) 
		{
			current_node = GetLowerCost();
			if(current_node == null)
				break;
			
			open_list.Remove(current_node);
			
			if(current_node.position == Graph.originPosition)
			{
				Debug.Log("breaking early");
				current_node.prevNode = closed_list[closed_list.Count - 1];
				break;
			}else{
				foreach(Node node in current_node.getAllConnectingNodes())
				{
					ObjectCreator.Inspect(node);
					
					if(!open_list.Contains(node) && !closed_list.Contains(node))
					{
						node.prevNode = current_node;
						float cost = Vector3.Distance(node.position, current_node.position);
						if(node.cluster != current_node.cluster)
						{
							cost = cm.getCost(current_node.cluster,node.cluster);
						}

						node.heuristic = cost + current_node.heuristic;
						open_list.Add(node);
					}
					else
					{
						float cost = Vector3.Distance(node.position, current_node.position);
						if(node.cluster != current_node.cluster)
						{
							cost = cm.getCost(current_node.cluster,node.cluster);
						}

						if(node.heuristic > (cost + current_node.heuristic))
						{
							node.heuristic = cost + current_node.heuristic;
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
			ObjectCreator.changeObjColor(current_node.obj, Color.cyan);
			actual_path.Add(current_node);
			current_node = current_node.prevNode;
		}
		ObjectCreator.changeObjColor (graph.FindNode(Graph.originPosition).obj, Color.red);
		NPC.GetComponent<NPCMovement> ().path = actual_path;
	}


	//Gets the best possible node with the lowest heuristic from the open list
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



}
