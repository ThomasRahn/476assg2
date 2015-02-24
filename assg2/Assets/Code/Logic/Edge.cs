using UnityEngine;
using System.Collections;

public class Edge {

	public Node start;
	public Node end;

	public Edge(Node s, Node e)
	{
		this.start = s;
		this.end = e;

		start.AddEdge (this);
		end.AddEdge (this);
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
}
