// -------------------------------------------------------------------------------------------------
//  PhysicsSprite3D.cs
//  Created by Jesse Ozog (code@smashriot.com / @SmashRiot / SmashRiot.com) on 2014/07/15
//  Copyright 2014 SmashRiot, LLC. All rights reserved.
// -------------------------------------------------------------------------------------------------
using UnityEngine;

public class PhysicsSprite3D : FContainer { 

	private const int PHYSICS_ANGUALR_DRAG = 10;
	private const int PHYSICS_MASS = 10;
	public PhysicsComponent3D physicsComponent3D;
	private FSprite physicsSprite;

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public PhysicsSprite3D(Vector2 position, float rotation = 0.0f, bool squareCollider = true) : base(){

		// add physics component
		this.physicsComponent3D = PhysicsComponent3D.Create("PhysicsSprite3D");
		this.physicsComponent3D.InitalizePhysicsComponent3D(position, rotation, this);

		// create the sprite
		this.physicsSprite = new FSprite("tile3d"); 
		this.AddChild(this.physicsSprite);

		// 3D physics - Rigidbody rigidbody = 
		physicsComponent3D.AddRigidBody3D(PHYSICS_ANGUALR_DRAG, PHYSICS_MASS);  // angular drag, masss

		// add collider
		if (squareCollider){ this.physicsComponent3D.AddBoxCollider3D(new Vector2(this.physicsSprite.width, this.physicsSprite.height)); }
		else { this.physicsComponent3D.AddSphereCollider3D(new Vector2(this.physicsSprite.x, this.physicsSprite.y), this.physicsSprite.width * 0.5f); }
	
		// stop, and make sure GO is active
		this.physicsComponent3D.StopPhysics();
		this.physicsComponent3D.gameObject.SetActive(true);
	}

}