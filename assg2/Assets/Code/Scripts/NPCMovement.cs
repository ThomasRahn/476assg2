using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour {

	public Node currentPosition;
	public IList<Node> path = new List<Node>();
	public Node nextNode;
	public Vector3 next;
	public int pathCount = 0;

	public float MaxAcceleration = 10.0f;
	public Vector3 Velocity = Vector3.zero;
	public float MaxVelocity = 0.5f;
	public float slowDown = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		pathCount = path.Count;
		if (path.Count != 0 ) {
			if (nextNode == null) {
				nextNode = path [path.Count - 1];
				next = nextNode.position;
				path.Remove (nextNode);
			}
			if (nextNode.position == (this.transform.position - new Vector3 (0, 0.5f, 0))) {
				currentPosition = nextNode;
				nextNode = path [path.Count - 1];
				next = nextNode.position;
				path.Remove (nextNode);
			}
		
			if (nextNode != null) {
				this.transform.rotation = Quaternion.LookRotation (nextNode.obj.transform.position - this.transform.position);
				this.transform.position = Vector3.MoveTowards (this.transform.position, nextNode.position + new Vector3 (0, 0.5f, 0), Time.deltaTime);
				//UpdatePosition();
			}

		} else {

			if(nextNode != null && this.transform.position != Graph.originPosition){
				this.transform.position = Vector3.MoveTowards (this.transform.position, nextNode.position + new Vector3 (0, 0.5f, 0), Time.deltaTime);
				//UpdatePosition();
			}

			if(this.transform.position == Graph.originPosition)
			{
				nextNode = null;
			}
		}
	}

	public void UpdatePosition(){ 
		
		Vector3 accel;
		if(Vector3.Distance(this.transform.position, nextNode.position) < slowDown){
			accel = Vector3.zero;
		}else{
			accel = (MaxAcceleration) * (nextNode.position - this.transform.position).normalized;
		}
		
		Velocity = Velocity + accel * Time.deltaTime;
		
		Velocity = Vector3.ClampMagnitude (Velocity, MaxVelocity);
		Velocity.y = 0.0f;

		this.transform.position = this.transform.position + Velocity * Time.deltaTime;
	}
}
