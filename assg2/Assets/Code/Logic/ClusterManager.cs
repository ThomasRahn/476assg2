using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ClusterManager {

	List<Cluster> clusters;
	private Graph graph;
	public ClusterManager(Graph graph)
	{
		this.graph = graph;
		GenerateLookUpTable ();

		for (int i = 0; i < clusters.Count; i++) {
			Debug.Log (clusters [i].start.ToString ());
			Debug.Log (clusters [i].end.ToString ());
			Debug.Log (clusters [i].cost);
		}
	}

	private void GenerateLookUpTable()
	{
		clusters = new List<Cluster> ();

		foreach (Node.Cluster cluster_value in Enum.GetValues(typeof(Node.Cluster))) {
			List<Node> nodes_from_cluster = graph.GetRoomNodes (cluster_value);

			foreach (Node n in nodes_from_cluster) {
				foreach (Node connecting in n.getAllConnectingNodes()) {
					//Connected to a different cluser
					if (n.cluster != connecting.cluster) {
						Cluster cluster = new Cluster ();
						cluster.start = n.cluster;
						cluster.end = connecting.cluster;
						cluster.cost = Vector3.Distance (n.position, connecting.position);
						if (!alreadyContains (cluster)) {
							clusters.Add (cluster);
						}
					}
				}
			}
		}

		foreach (Node.Cluster cluster_value in Enum.GetValues(typeof(Node.Cluster))) 
		{
			foreach (Node.Cluster adjacent_cluster in Enum.GetValues(typeof(Node.Cluster))) 
			{
				if(getCost(cluster_value,adjacent_cluster) < 0.0f && cluster_value != adjacent_cluster)
				{
					//calculate cost
					//CalculateSmallestCost(cluster_value, adjacent_cluster);

				}else{
					continue;
				}
			}
		}
	}

	public void CalculateSmallestCost(Node.Cluster v1,Node.Cluster v2)
	{
		float cost = 0.0f;
		while (cost > 0.0f) {
			List<Cluster> connecting = GetConnectingClusters(v1);
			foreach(Cluster c in connecting)
			{

				cost = (c.cost + GetCostOfPath(c, v1, v2)) < cost ? c.cost + GetCostOfPath(c, v1, v2) : 0.0f;
			}
		}
		Cluster new_cluster = new Cluster ();
		new_cluster.start = v1;
		new_cluster.end = v2;
		new_cluster.cost = cost;
		if(!alreadyContains(new_cluster))
			clusters.Add (new_cluster);
	}

	private float GetCostOfPath(Cluster c, Node.Cluster start, Node.Cluster end)
	{
		foreach (Cluster connecting in GetConnectingClusters(c.start)) {
			if(connecting.end == start)
				continue;
			else if(connecting.end == end)
				return connecting.cost;
			else 
				return GetCostOfPath(connecting,start,end);
		}
		return 0.0f;
	}

	public List<Cluster> GetConnectingClusters(Node.Cluster clus)
	{
		List<Cluster> group = new List<Cluster>();
		foreach (Cluster cluster in clusters) {
			if(clus == cluster.start)
				group.Add (cluster);
		}
		return group;
	}

	//Returns the cost with the Cluster start and end point
	public float getCost(Node.Cluster c1, Node.Cluster c2)
	{
		foreach (Cluster cluster in clusters) 
		{
			if(cluster.start == c1 && cluster.end == c2)
			{
				return cluster.cost;
			}
		}
		return -1.0f;
	}

	private bool alreadyContains(Cluster clus)
	{
		foreach (Cluster cluster in clusters) {
			if(clus.start == cluster.start && clus.end == cluster.end)
			{
				return true;
			}
		}
		return false;
	}
}
