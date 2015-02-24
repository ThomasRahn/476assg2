using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph {
	
	const float TILE_SIZE = 0.5f;
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

	Node CreateNode(Vector3 nodePosition, Node source)
	{
		Node n = new Node (nodePosition);
		//Create object for graph
		GraphController.makeBlock (nodePosition);
		//Check if position has been taken


		if (source != null) {
			Edge e = new Edge(n,source);
			this.edges.Add(e);
		}

		//Generate 8 other positions

		//Top
		Vector3 top = new Vector3(nodePosition.x, 0,nodePosition.z + TILE_SIZE);
		if (!Physics.Raycast (nodePosition, top - nodePosition, Vector3.Distance (nodePosition, top))) {
			Node topNode = CreateNode (top, n);
			Edge topEdge = new Edge (topNode, n);
			this.edges.Add (topEdge);
		}

		//Top-left
		Vector3 topL= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z + TILE_SIZE);
		if (!Physics.Raycast (nodePosition, topL - nodePosition, Vector3.Distance (nodePosition, topL))) {
			Node topLNode = CreateNode (topL, n);
			Edge topLEdge = new Edge (topLNode, n);
			this.edges.Add (topLEdge);
		}

		//Top-Right
		Vector3 topR= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z + TILE_SIZE);
		if (!Physics.Raycast (nodePosition, topR - nodePosition, Vector3.Distance (nodePosition, topR))) {
			Node topRNode = CreateNode (topR, n);
			Edge topREdge = new Edge (topRNode, n);
			this.edges.Add (topREdge);
		}

		//left
		Vector3 left= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z);
		if (!Physics.Raycast (nodePosition, left - nodePosition, Vector3.Distance (nodePosition, left))) {
			Node leftNode = CreateNode(left,n);
			Edge leftEdge = new Edge (leftNode, n);
			this.edges.Add (leftEdge);
		}
		
		//Right
		Vector3 right= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z);
		if (!Physics.Raycast (nodePosition, right - nodePosition, Vector3.Distance (nodePosition, right))) {
			Node rightNode = CreateNode(right,n);
			Edge rightEdge = new Edge (rightNode, n);
			this.edges.Add (rightEdge);
		}

		//bottom
		Vector3 bottom = new Vector3(nodePosition.x, 0,nodePosition.z - TILE_SIZE);
		if (!Physics.Raycast (nodePosition, bottom - nodePosition, Vector3.Distance (nodePosition, bottom))) {
			Node bottomNode = CreateNode(bottom,n);
			Edge bottomEdge = new Edge (bottomNode, n);
			this.edges.Add (bottomEdge);
		}
			
		//bottom-left
		Vector3 bottomL= new Vector3(nodePosition.x - TILE_SIZE, 0,nodePosition.z - TILE_SIZE);
		if (!Physics.Raycast (nodePosition, bottomL - nodePosition, Vector3.Distance (nodePosition, bottomL))) {
			Node bottomLNode = CreateNode(bottomL,n);
			Edge bottomLEdge = new Edge (bottomLNode, n);
			this.edges.Add (bottomLEdge);
		}
		
		//bottom-Right
		Vector3 bottomR= new Vector3(nodePosition.x + TILE_SIZE, 0,nodePosition.z - TILE_SIZE);
		if (!Physics.Raycast (nodePosition, bottomR - nodePosition, Vector3.Distance (nodePosition, bottomR))) {
			Node bottomRNode = CreateNode (bottomR, n);
			Edge bottomREdge = new Edge (bottomRNode, n);
			this.edges.Add (bottomREdge);
		}

		return n;
	}
}
