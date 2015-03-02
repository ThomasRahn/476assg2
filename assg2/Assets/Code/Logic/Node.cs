using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {

	public Vector3 position;
	public IList<Edge> edges = new List<Edge>();
	public GameObject obj;

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

	public bool hasEdge(Node n)
	{
		foreach (Edge e in edges) 
		{
			Node connecting = e.GetConnectingNode(n);

			if(connecting != null && connecting.position == this.position)
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
