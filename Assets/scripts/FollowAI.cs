using UnityEngine;
using System.Collections;

public class FollowAI : MonoBehaviour {

	public GameObject target;
	GameObject followLine, deltaLine, unitV, forwardVector, angleArc, angleAxis;
	public float speed = 2;
	void Start () {
		Lines.Make (ref followLine, Color.red, Vector3.zero, 
		           target.transform.position, 1, 0);
	}
	void Update () {
		Lines.Make (ref deltaLine, Color.cyan, transform.position,
		            target.transform.position, .1f, .1f);
		Vector3 delta = target.transform.position - transform.position;
		float distance = delta.magnitude;
		print (delta + " which is " + distance + " long");
		Vector3 unitVector = delta.normalized;
		Lines.Make (ref unitV, Color.green, transform.position, 
		           transform.position + unitVector, 2, 0);
		if(distance > 5)
		{
			CharacterController cm = GetComponent<CharacterController> ();
			if(cm == null) {
				transform.position += unitVector * speed * Time.deltaTime; // move through air
			} else {
				cm.SimpleMove(unitVector * speed);	// move on ground with CharacterController
			}
		}
		Lines.Make (ref forwardVector, Color.blue, transform.position,
		           transform.position + transform.forward, .2f, .2f);
		float angle = Vector3.Angle (transform.forward, unitVector);
		Vector3 cross = Vector3.Cross (transform.forward, unitVector);
		Lines.MakeArc (ref angleArc, Color.magenta, transform.position,
		              cross, transform.forward, angle, 10, .1f, 0);
		Lines.Make (ref angleAxis, Color.magenta, transform.position + cross,
		           transform.position - cross, .1f, .1f);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, 
		           Quaternion.LookRotation (unitVector), 90 * Time.deltaTime);
	}
}
