using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {

	public enum Cluster
	{
		room1,
		room2,
		room3,
		corridor1,
		corridor2

	}


	public Vector3 position;
	public IList<Edge> edges = new List<Edge>();
	public GameObject obj;
	public float cost;
	public int g_heuristic;
	public float heuristic;
	public Node prevNode;

	public Cluster cluster;

	public Node(Vector3 position)
	{
		this.position = position;
		ResetCost ();

		if(position.z < -1.85f && position.x < 2.0f)
		{
			cluster = Cluster.room1;
		}
		else if (position.z > -0.9f && position.x > 1.85f)
		{
			cluster = Cluster.room2;
		}
		else if (position.z > 2.1f && position.x < 0.0f)
		{
			cluster = Cluster.room3;
		}else{
			if(position.z < 0.0f)
				cluster = Cluster.corridor1;
			else 
				cluster = Cluster.corridor2;
		}
		
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

	public void ResetCost()
	{
		this.cost = Vector3.Distance (position, Graph.originPosition);
	}
}
