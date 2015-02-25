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
				this.edges.Add(e);
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
			this.edges.Add(e);
		}

		//Generate 8 other positions

		//Top
		RaycastHit hit;
		Vector3 top = new Vector3(nodePosition.x, 0,nodePosition.z + TILE_SIZE);
		if (!Physics.SphereCast (nodePosition, TILE_SIZE / 2, top - nodePosition, out hit, Vector3.Distance (nodePosition, top))) {
			Node topNode = CreateNode (top, n);
			Edge topEdge = new Edge (topNode, n);
			this.edges.Add (topEdge);
		} else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(n, top);
			}
		}

		//Top-left
		Vector3 topL= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z + TILE_SIZE);
		if (!Physics.SphereCast (nodePosition, TILE_SIZE/2, topL - nodePosition,out hit, Vector3.Distance (nodePosition, topL))) {
			Node topLNode = CreateNode (topL, n);
			Edge topLEdge = new Edge (topLNode, n);
			this.edges.Add (topLEdge);
		}else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(n, top);
			}
		}

		//Top-Right
		Vector3 topR= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z + TILE_SIZE);
		if (!Physics.SphereCast (nodePosition, TILE_SIZE/2, topR - nodePosition,out hit, Vector3.Distance (nodePosition, topR))) {
			Node topRNode = CreateNode (topR, n);
			Edge topREdge = new Edge (topRNode, n);
			this.edges.Add (topREdge);
		} else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(n, top);
			}
		}

		//left
		Vector3 left= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z);
		if (!Physics.SphereCast (nodePosition, TILE_SIZE/2, left - nodePosition,out hit, Vector3.Distance (nodePosition, left))) {
			Node leftNode = CreateNode(left,n);
			Edge leftEdge = new Edge (leftNode, n);
			this.edges.Add (leftEdge);
		} else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(n, top);
			}
		}
		
		//Right
		Vector3 right= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z);
		if (!Physics.SphereCast (nodePosition, TILE_SIZE/2, right - nodePosition,out hit, Vector3.Distance (nodePosition, right))) {
			Node rightNode = CreateNode(right,n);
			Edge rightEdge = new Edge (rightNode, n);
			this.edges.Add (rightEdge);
		} else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(n, top);
			}
		}

		//bottom
		Vector3 bottom = new Vector3(nodePosition.x, 0,nodePosition.z - TILE_SIZE);
		if (!Physics.SphereCast (nodePosition, TILE_SIZE/2, bottom - nodePosition,out hit, Vector3.Distance (nodePosition, bottom))) {
			Node bottomNode = CreateNode(bottom,n);
			Edge bottomEdge = new Edge (bottomNode, n);
			this.edges.Add (bottomEdge);
		} else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(n, top);
			}
		}
			
		//bottom-left
		Vector3 bottomL= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z - TILE_SIZE);
		if (!Physics.SphereCast (nodePosition, TILE_SIZE/2, bottomL - nodePosition,out hit, Vector3.Distance (nodePosition, bottomL))) {
			Node bottomLNode = CreateNode(bottomL,n);
			Edge bottomLEdge = new Edge (bottomLNode, n);
			this.edges.Add (bottomLEdge);
		} else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(n, top);
			}
		}
		
		//bottom-Right
		Vector3 bottomR= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z - TILE_SIZE);
		if (!Physics.SphereCast (nodePosition, TILE_SIZE/2, bottomR - nodePosition,out hit, Vector3.Distance (nodePosition, bottomR))) {
			Node bottomRNode = CreateNode (bottomR, n);
			Edge bottomREdge = new Edge (bottomRNode, n);
			this.edges.Add (bottomREdge);
		} else {
			if(hit.collider.gameObject.tag == "graphBlock")
			{
				CreateEdgeFromOldNode(n, top);
			}
		}

		return n;
	}
}
