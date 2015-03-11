using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour {
	
	public Node currentPosition;
	public IList<Node> path = new List<Node>();
	public Node nextNode;
	public Vector3 next;
	public int pathCount = 0;
	
	public float MaxAcceleration = 3.0f;
	public Vector3 Velocity = Vector3.zero;
	public float MaxVelocity = 4.0f;
	public float rotateSpeed = 100.0f;

	public int current_node = 0;
	public Vector3 target_position;
	// Use this for initialization
	void Start () {

	}

	void Update()
	{
		Node target = getTarget();
		if (target != null) {
			target_position = target.position + new Vector3 (0, 1, 0);
			nextNode = target;

			Vector3 accel = (MaxAcceleration) * (target_position - this.transform.position).normalized;
		
			Velocity = Velocity + accel * Time.deltaTime;
		
			Velocity = Vector3.ClampMagnitude (Velocity, MaxVelocity);

			if(Vector3.Distance(Graph.originPosition + new Vector3(0,1,0),this.transform.position) < 0.75f)
			{
				Velocity = Vector3.Scale(Velocity, new Vector3(0.9f,0,0.9f));
			}
		
			this.transform.position = this.transform.position + Velocity * Time.deltaTime;
		}
		if (Velocity.sqrMagnitude > 0f)
			transform.rotation = Quaternion.LookRotation (Velocity.normalized, Vector3.up);
	}

	private Node getTarget(){

		Node target = null;
		if (path.Count != 0 ) {
			current_node = path.Count - 1;
			if(current_node < 0)
				return null;

			target = path[current_node];
			Vector3 temp = target.position + new Vector3(0,1,0);
			if(Vector3.Distance(temp, this.transform.position) <= 0.2f)
			{
				current_node --;
				path.RemoveAt(current_node+1);
			}
		}
		return target;
	}
}