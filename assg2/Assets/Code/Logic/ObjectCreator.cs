using UnityEngine;
using System.Collections;

public class ObjectCreator : MonoBehaviour {

	public static GameObject makeBlock(Vector3 position, Node n)
	{
		
		GameObject node = Instantiate(Resources.Load("Prefabs/Node"), position, Quaternion.identity)as GameObject;
		node.GetComponent<GraphElement> ().setNode (n);
		if (position == Graph.originPosition) {
			node.transform.FindChild ("Cube").renderer.material.color = Color.red;
		} else {
			node.transform.FindChild ("Cube").renderer.material.color = Color.yellow;
		}
		return node;
	}
	
	public static void makeLine(Vector3 start, Vector3 end)
	{
		GameObject line = Instantiate (Resources.Load ("Prefabs/Line"), new Vector3 (0, -10, 0), Quaternion.identity)as GameObject;
		LineRenderer lineR = line.GetComponent<LineRenderer> ();
		lineR.SetPosition (0, start + new Vector3 (0, 0.3f, 0));
		lineR.SetPosition (1, end + new Vector3 (0, 0.3f, 0));
		lineR.renderer.material.color = Color.red;
	}
	
	public static GameObject CreateNPC(Node start)
	{
		GameObject NPC = Instantiate (Resources.Load ("Prefabs/Npc"), start.position + new Vector3(0,1,0), Quaternion.identity) as GameObject;
		NPC.GetComponent<NPCMovement> ().currentPosition = start;
		return NPC;
	}

	public static void Inspect(Node node)
	{
		changeObjColor (node.obj, Color.magenta);
	}
	
	public static void GoToNode(Node node)
	{
		changeObjColor (node.obj, Color.cyan);
	}
	
	public static void changeObjColor(GameObject obj, Color c)
	{
		Transform cube = obj.transform.FindChild ("Cube");
		if (cube != null) {
			cube.renderer.material.color = c;
		}
	}
}
