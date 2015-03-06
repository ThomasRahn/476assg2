using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {

	public Vector3 position;
	public IList<Edge> edges = new List<Edge>();
	public GameObject obj;
	public float cost;
	public Node prevNode;

	public Node(Vector3 position)
	{
		this.position = position;
		this.cost = Vector3.Distance (position, Graph.originPosition);
	}

	public void AddEdge(Edge e)
	{
		if(e.start.position == this.position)
			this.edges.Add(e);
	}

	public void CreateEdge(Node n)
	{
		Edge e = new Edge (this, n);
		this.AddEdge (e);
	}

	public bool hasEdge(Node n)
	{
		foreach (Edge e in edges) 
		{
			if(e.end.position == n.position)
			{
				return true;
			}
		}
		return false;
	}

	public Node[] getAllConnectingNodes()
	{
		Node[] nodes = new Node[edges.Count];
		for(int i = 0; i < nodes.Length; i++)
		{
			nodes[i] = edges[i].end;
		}
		return nodes;
	}

	public void setGameObject(GameObject obj)
	{
		this.obj = obj;
	}

}
