using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph {
	
	const float TILE_SIZE = 0.25f;
	public List<Edge> edges = new List<Edge>();
	public List<Node> nodes = new List<Node>();
	
	// Use this for initialization
	public Graph () {
		Node initial = CreateNode(new Vector3(0, 0, 0), null);
		nodes.Add (initial);
	}
	
	
	public void addEdge(Edge e)
	{
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
				Edge e = new Edge(newNode,n);
				addEdge(e);
			}
		}
	}


	void Generate(Node origin, Vector3 position)
	{
		RaycastHit hit;
		if (!Physics.SphereCast (origin.position, TILE_SIZE / 2, position - origin.position, out hit, Vector3.Distance (origin.position, position))) {
			Node topNode = CreateNode (position, origin);
			Edge topEdge = new Edge (topNode, origin);
			addEdge (topEdge);
		} else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(origin, position);
			}
		}
	}
	Node CreateNode(Vector3 nodePosition, Node source)
	{
		Node n = new Node (nodePosition);
		//Create object for graph
		GraphController.makeBlock (nodePosition,n);
		//Check if position has been taken
		
		
		if (source != null) {
			Edge e = new Edge(n,source);
			addEdge(e);
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
		
		return n;
	}
}