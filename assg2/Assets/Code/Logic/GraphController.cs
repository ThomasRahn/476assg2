using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GraphController : MonoBehaviour {

	const float TILE_SIZE = 0.5f;
	const int WORLD_WIDTH = 20;
	const int WORLD_HEIGHT = 20;
	public Vector3 start_position = new Vector3(0,0,0);
	private float offset = 0.5f;
	float spawnSpeed = 0.01f;
	private float bounds = 5.0f;

	public Graph graph;

	public List<Vector3> gridPositions = new List<Vector3>();
	// Use this for initialization
	void Start () {
		graph = new Graph ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void makeBlock(Vector3 position)
	{
		GameObject node = Instantiate(Resources.Load("Prefabs/Node"), position, Quaternion.identity)as GameObject;
	}
}
