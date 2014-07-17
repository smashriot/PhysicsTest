// -------------------------------------------------------------------------------------------------
//  PhysicsSprite2D.cs
//  Created by Jesse Ozog (code@smashriot.com / @SmashRiot / SmashRiot.com) on 2014/07/15
//  Copyright 2014 SmashRiot, LLC. All rights reserved.
// -------------------------------------------------------------------------------------------------
using UnityEngine;

public class PhysicsSprite2D : FContainer { 

	private const int PHYSICS_ANGUALR_DRAG = 10;
	private const int PHYSICS_MASS = 10;
	public PhysicsComponent2D physicsComponent2D;
	private FSprite physicsSprite;

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	public PhysicsSprite2D(Vector2 position, float rotation = 0.0f, bool squareCollider = true) : base(){

		// add physics2d component
		this.physicsComponent2D = PhysicsComponent2D.Create("PhysicsSprite2D");
		this.physicsComponent2D.InitalizePhysicsComponent2D(position, rotation, this);

		// create the square sprite
	    this.physicsSprite = new FSprite("tile2d"); 
		this.AddChild(this.physicsSprite);

		// 2D physics - Rigidbody2D rigidbody2D = 
		physicsComponent2D.AddRigidBody2D(PHYSICS_ANGUALR_DRAG, PHYSICS_MASS);  // angular drag, masss

		// add collider
		if (squareCollider){ this.physicsComponent2D.AddBoxCollider2D(new Vector2(this.physicsSprite.width, this.physicsSprite.height)); }
		else { this.physicsComponent2D.AddCircleCollider2D(new Vector2(this.physicsSprite.x, this.physicsSprite.y), this.physicsSprite.width * 0.5f); }

		// stop, and make sure GO is active
		this.physicsComponent2D.StopPhysics();
		this.physicsComponent2D.gameObject.SetActive(true);
	}

}