using UnityEngine;
using System.Collections;
using System;

public class Edge : IComparable {

	public Node start;
	public Node end;
	float cost;

	public Edge(Node s, Node e)
	{
		this.start = s;
		this.end = e;
		cost = 1.0f;
		start.AddEdge (this);
	}

	public Node GetConnectingNode(Node n)
	{
		if (n == start) {
			return end;
		} else if (n == end) {
			return start;
		} else {
			return null;
		}
	}

	public void setCost(float c)
	{
		if(c >= 0)
			cost = c;
	}
	public float getCost()
	{
		return cost;
	}

	public int CompareTo(object obj)
	{
		Edge e = obj as Edge;
		if (this.cost == e.getCost())
			return 0;
		else if (this.getCost() < e.getCost())
			return -1;
		else
			return 1; // this.Length > p.Length
	}
}
