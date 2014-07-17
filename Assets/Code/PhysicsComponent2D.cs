// -------------------------------------------------------------------------------------------------
//  PhysicsComponent2D.cs
//  Created by Jesse Ozog (code@smashriot.com / @SmashRiot / SmashRiot.com) on 2014/07/15
//  Copyright 2014 SmashRiot, LLC. All rights reserved.
// -------------------------------------------------------------------------------------------------
using UnityEngine;

public class PhysicsComponent2D : MonoBehaviour {

	Rigidbody2D rigidbody2d; 

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public static PhysicsComponent2D Create(string name){

		GameObject physicsGameObject = new GameObject(name);
		PhysicsComponent2D physicsComponent2D = physicsGameObject.AddComponent<PhysicsComponent2D>();

		return physicsComponent2D;
	}
	
	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public void InitalizePhysicsComponent2D(Vector2 startPosition, float startRotation, FContainer parentContainer){
	
		gameObject.transform.position = startPosition * FPhysics.POINTS_TO_METERS;
		gameObject.transform.rotation = Quaternion.Euler(0, 0, startRotation);
		gameObject.transform.parent = FPWorld.instance.transform;

		FPNodeLink nodeLink = gameObject.AddComponent<FPNodeLink>();
		nodeLink.Init(parentContainer, true);
	}

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public void StartPhysics(){
		
		if (this.rigidbody2d != null){
			this.rigidbody2d.isKinematic = false;
		}
	}

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public void StopPhysics(){

		if (this.rigidbody2d != null){
			this.rigidbody2d.isKinematic = true;
		}
	}

	// ------------------------------------------------------------------------
	// http://docs.unity3d.com/ScriptReference/rigidbody2d.html
	// ------------------------------------------------------------------------
	public Rigidbody2D AddRigidBody2D(float angularDrag, float mass){
		
		this.rigidbody2d = gameObject.AddComponent<Rigidbody2D>();
		this.rigidbody2d.angularDrag = angularDrag;
		this.rigidbody2d.mass = mass;
		this.rigidbody2d.isKinematic = true;
		this.rigidbody2d.gravityScale = 0.0f;
		this.rigidbody2d.fixedAngle = true;
		
		return this.rigidbody2d;
	}

	// ------------------------------------------------------------------------
	// http://docs.unity3d.com/ScriptReference/Collider2D.html
	// http://docs.unity3d.com/ScriptReference/CircleCollider2D.html
	// ------------------------------------------------------------------------
	public CircleCollider2D AddCircleCollider2D(Vector2 center, float radius){

		CircleCollider2D newCollider = gameObject.AddComponent<CircleCollider2D>();
		newCollider.center = center * FPhysics.POINTS_TO_METERS;
		newCollider.radius = radius * FPhysics.POINTS_TO_METERS;
		newCollider.isTrigger = false;

		return newCollider;
	}

	// ------------------------------------------------------------------------
	// http://docs.unity3d.com/ScriptReference/BoxCollider2D.html
	// ------------------------------------------------------------------------
	public BoxCollider2D AddBoxCollider2D(Vector2 size){

		BoxCollider2D newCollider = gameObject.AddComponent<BoxCollider2D>();
		newCollider.size = new Vector2(size.x * FPhysics.POINTS_TO_METERS, size.y * FPhysics.POINTS_TO_METERS);
		newCollider.isTrigger = false;

		return newCollider;
	}

	// ------------------------------------------------------------------------
	// for static tiles (e.g. tilemap tiles), there is a performance improvement in disabling node link.
	// ------------------------------------------------------------------------
	public void DisableNodeLink(){

		this.gameObject.GetComponent<FPNodeLink>().enabled = false;
	}

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public void AddForce(Vector2 forceVector, ForceMode2D forceMode = ForceMode2D.Force){

		if (this.rigidbody2d != null){
			// the 2D force physics are slightly different, this 2* makes it look similar to 3d (perf is same with/without)
			this.rigidbody2d.AddForce(2 * forceVector, forceMode);
		}
	}
}