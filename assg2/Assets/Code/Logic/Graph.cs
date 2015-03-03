using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph {
	
	const float TILE_SIZE = 0.25f;
	public List<Edge> edges = new List<Edge>();
	public List<Node> nodes = new List<Node>();

	public static Vector3 originPosition = new Vector3 (2.75f, 0, 2.25f);
	// Use this for initialization
	public Graph () {
		CreateNode(Graph.originPosition);
	}

	public void addEdge(Edge e)
	{
		float cost = Vector3.Distance (e.end.position, Graph.originPosition);
		e.setCost(cost);
		edges.Add (e);
		GraphController.makeLine (e.start.position, e.end.position);
	}
	
	public void addNode(Node n)
	{
		nodes.Add (n);
	}
	
	public Node FindNode(Vector3 position)
	{
		foreach (Node n in nodes) {
			if(n.position == position)
				return n;
		}
		return null;
	}
	
	public void CreateEdgeFromOldNode(Node n, Vector3 position)
	{
		Node newNode = this.FindNode(position);
		if(newNode != null)
		{
			if(!newNode.hasEdge(n))
			{
				Edge e = new Edge(n,newNode);
				addEdge(e);
			}
		}
	}

	void Generate(Node origin, Vector3 position)
	{
		Collider[] wallColliders = Physics.OverlapSphere(position, TILE_SIZE / 2.0f, LayerMask.GetMask("Wall"));
		if (wallColliders.Length > 0)
			return;

		Collider[] nodeCollider = Physics.OverlapSphere(position, TILE_SIZE / 2.0f, LayerMask.GetMask("node"));
		if(nodeCollider.Length == 0)
		{
			CreateNode (position, origin);
		} 
		else 
		{
			//There is a node here so create edge
			CreateEdgeFromOldNode(origin, position);
		}
	}
	void CreateNode(Vector3 nodePosition, Node source = null)
	{
		//Create the node.
		Node n = new Node (nodePosition);

		//Create object for graph
		n.setGameObject(GraphController.makeBlock (nodePosition,n));

		this.addNode (n);

		if (source != null) {
			Edge e = new Edge(source,n);
			this.addEdge(e);
		}

		//Top
		Vector3 top = new Vector3(nodePosition.x, 0,nodePosition.z + TILE_SIZE);
		Generate (n, top);
		
		//Top-left
		Vector3 topL= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z + TILE_SIZE);
		Generate (n, topL);
		
		//Top-Right
		Vector3 topR= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z + TILE_SIZE);
		Generate (n, topR);
		
		//left
		Vector3 left= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z);
		Generate (n, left);
		
		//Right
		Vector3 right= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z);
		Generate (n, right);
		
		//bottom
		Vector3 bottom = new Vector3(nodePosition.x, 0,nodePosition.z - TILE_SIZE);
		Generate (n, bottom);
		
		//bottom-left
		Vector3 bottomL= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z - TILE_SIZE);
		Generate (n, bottomL);
		
		//bottom-Right
		Vector3 bottomR= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z - TILE_SIZE);
		Generate (n, bottomR);

	}
}