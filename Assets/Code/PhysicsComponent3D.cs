// -------------------------------------------------------------------------------------------------
//  PhysicsComponent3D.cs
//  Created by Jesse Ozog (code@smashriot.com / @SmashRiot / SmashRiot.com) on 2014/07/15
//  Copyright 2014 SmashRiot, LLC. All rights reserved.
// -------------------------------------------------------------------------------------------------
using UnityEngine;

public class PhysicsComponent3D : MonoBehaviour {

	Rigidbody rigidbody3d;

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public static PhysicsComponent3D Create(string name){

		GameObject physicsGameObject = new GameObject(name);
		PhysicsComponent3D physicsComponent3D = physicsGameObject.AddComponent<PhysicsComponent3D>();
		
		return physicsComponent3D;
	}

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public void InitalizePhysicsComponent3D(Vector2 startPosition, float startRotation, FContainer parentContainer){

		gameObject.transform.position = startPosition * FPhysics.POINTS_TO_METERS; // assigning a vec2 to vec3
		gameObject.transform.rotation = Quaternion.Euler(0, 0, startRotation);
		gameObject.transform.parent = FPWorld.instance.transform;

		FPNodeLink nodeLink = gameObject.AddComponent<FPNodeLink>();
		nodeLink.Init(parentContainer, true);
	}

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public void StartPhysics(){
		
		if (this.rigidbody3d != null){
			this.rigidbody3d.isKinematic = false;
		}
	}

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public void StopPhysics(){

		if (this.rigidbody3d != null){
			this.rigidbody3d.isKinematic = true;
		}
	}

	// ------------------------------------------------------------------------
	// http://docs.unity3d.com/ScriptReference/Rigidbody.html
	// ------------------------------------------------------------------------
	public Rigidbody AddRigidBody3D(float angularDrag, float mass){

		this.rigidbody3d = gameObject.AddComponent<Rigidbody>();
		this.rigidbody3d.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		this.rigidbody3d.angularDrag = angularDrag;
		this.rigidbody3d.mass = mass;
		this.rigidbody3d.isKinematic = true;
		this.rigidbody3d.useGravity = false;

		return this.rigidbody;
	}

	// ------------------------------------------------------------------------
	// http://docs.unity3d.com/ScriptReference/Collider.html
	// http://docs.unity3d.com/ScriptReference/SphereCollider.html
	// ------------------------------------------------------------------------
	public SphereCollider AddSphereCollider3D(Vector2 center, float radius){

		SphereCollider newCollider = gameObject.AddComponent<SphereCollider>();
		newCollider.radius = radius * FPhysics.POINTS_TO_METERS;
		newCollider.isTrigger = false;

		return newCollider;
	}

	// ------------------------------------------------------------------------
	// http://docs.unity3d.com/ScriptReference/BoxCollider.html
	// ------------------------------------------------------------------------
	public BoxCollider AddBoxCollider3D(Vector2 size){

		BoxCollider newCollider = gameObject.AddComponent<BoxCollider>();
		newCollider.size = new Vector3(size.x * FPhysics.POINTS_TO_METERS, size.y * FPhysics.POINTS_TO_METERS, FPhysics.DEFAULT_Z_THICKNESS);
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
	public void AddForce(Vector3 forceVector, ForceMode forceMode = ForceMode.Force){

		if (this.rigidbody3d != null){
			this.rigidbody3d.AddForce(forceVector, forceMode);
		}
	}
}