using UnityEngine;
using System.Collections;

public class PoVController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] nodes = GameObject.FindGameObjectsWithTag ("graphBlock");
		for(int i = 0; i < nodes.Length; i++)
		{
			for(int j = i + 1; j < nodes.Length; j++)
			{
				if(i == j)
					continue;
				
				RaycastHit hit;
				Debug.DrawLine(nodes[i].transform.position, nodes[j].transform.position , Color.red);
				if(!Physics.Raycast(nodes[i].transform.position, (nodes[j].transform.position - nodes[i].transform.position),
				                   out hit, Vector3.Distance(nodes[i].transform.position, nodes[j].transform.position),LayerMask.GetMask("Wall")))
				{
					GraphController.makeLine(nodes[i].transform.position, nodes[j].transform.position);
				}

			}
		}
	}
	// Update is called once per frame
	void Update () {

	}
}
