using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {

	public Vector3 position;
	public IList<Edge> edges = new List<Edge>();

	public Node(Vector3 position)
	{
		this.position = position;
	}

	public void AddEdge(Edge e)
	{
		this.edges.Add(e);
	}

	public void CreateEdge(Node n)
	{
		Edge e = new Edge (this, n);
		this.AddEdge (e);
	}

}
