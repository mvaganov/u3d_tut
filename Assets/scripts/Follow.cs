using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform target;

	public float turnSpeed = 90;
	public float moveSpeed = 5;
	public float minimumDistance = 3;

	GameObject forwardVector, followVector, turnArc;

	void Start(){
		forwardVector = new GameObject ("forward");
		forwardVector.transform.parent = transform;
		followVector = new GameObject ("follow");
		followVector.transform.parent = transform;
	}

	void Update () {
		if(target != null)
		{
			Vector3 delta = target.position - transform.position;
			float distance = delta.magnitude;
			Vector3 direction = delta.normalized; // same as (delta / distance);
			CharacterController cc = GetComponent<CharacterController>();
			Color aiColor = Color.cyan;
			if(cc != null)
			{
				Vector3 directionOnGround = direction;
				directionOnGround.y = 0;
				directionOnGround.Normalize();
				transform.rotation = Quaternion.RotateTowards (
					transform.rotation, 
					Quaternion.LookRotation (directionOnGround), 
					turnSpeed * Time.deltaTime);
				if(distance > minimumDistance)
				{
					cc.SimpleMove(direction);
				}
			}
			else 
			{
				transform.rotation = Quaternion.RotateTowards (
					transform.rotation, 
					Quaternion.LookRotation (direction), 
					turnSpeed * Time.deltaTime);
				if(rigidbody != null && rigidbody.useGravity == false) // use steeting behavior logic for floating rigid bodies
				{
					float accelerationForce = 2; // TODO make this a public variable
					if(Vector3.Dot(rigidbody.velocity, direction) < moveSpeed)
					{
						float stoppingDistance = rigidbody.velocity.magnitude / accelerationForce;
						if(distance > minimumDistance && stoppingDistance < distance-minimumDistance)
						{
							// move toward
							Vector3 idealVelocity = direction * moveSpeed;
							Vector3 difference = idealVelocity - rigidbody.velocity; 
							Vector3 accelerationVector = difference.normalized * accelerationForce * Time.deltaTime;
							rigidbody.velocity += accelerationVector;
						} else {
							// try to stop
							aiColor = Color.magenta;
							Vector3 accelerationVector = rigidbody.velocity.normalized * accelerationForce * Time.deltaTime;
							rigidbody.velocity -= accelerationVector;
						}
					}
					if(rigidbody.angularVelocity.magnitude > 1) // if spinning, try to stop spinning
						rigidbody.angularVelocity -= rigidbody.angularVelocity.normalized * accelerationForce * Time.deltaTime;
				} else
				if(distance > minimumDistance)
				{
					transform.position += direction * moveSpeed * Time.deltaTime;
				}
			}
			Lines.Make (ref followVector, aiColor, transform.position, transform.position + direction, 0.1f, 0);
			Lines.Make (ref forwardVector, Color.blue, transform.position, transform.position + transform.forward, 0.1f, 0);
			Lines.MakeArc(ref turnArc, Color.green, transform.position, Vector3.Cross(transform.forward, direction),
			              transform.forward, Vector3.Angle(transform.forward, direction), 10, 0.1f, 0);
		}
	}
}
