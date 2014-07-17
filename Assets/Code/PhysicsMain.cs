// -------------------------------------------------------------------------------------------------
//  PhysicsMain.cs
//  Created by Jesse Ozog (code@smashriot.com / @SmashRiot / SmashRiot.com) on 2014/07/15
//  Copyright 2014 SmashRiot, LLC. All rights reserved.
//
//  The purpose of this project is to illustrate the performance difference between using 3D rigidbody/colliders
//  and 2D rigidbody/colliders with Futile. 
//
//  The game will start with a solid border of square tiles and then fill in circle/spere colliders. 
//  Starts in 3D mode, and then press "R" to restart it in 2D mode.
// 
//  The 2d/3d code is as close to analogous as possible.  
//
//  Posted to: http://www.reddit.com/r/futile/comments/29l7qd/slow_physics_performance_after_converting_from/  
// -------------------------------------------------------------------------------------------------
using UnityEngine;

public class PhysicsMain : MonoBehaviour {

	private const int TILES_WIDE = 80; 
	private const int TILES_HIGH = 44; 
	private const int TILE_WIDTH = 64;
	private const int TILE_HEIGHT = 64;
	private const int MAX_OBJECTS = 1200;
	private const int CONTAINER_OFFSET_X = -640;
	private const int CONTAINER_OFFSET_Y = 360;
	private const int ACCELERATION_MAX = 1000;
	private int nunmberOfObjectsAdded = 0;
	private bool usePhysics2D = false;
	private FContainer objectContainer;

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	private void Start(){

		// init futile
		FutileParams fparms = new FutileParams(true, true, true, true);
		fparms.AddResolutionLevel(1280.0f, 1.0f, 1.0f, "");
		fparms.origin = new Vector2(0.5f,0.5f);
		Futile.instance.Init(fparms);
		Futile.instance.camera.depth = 360;
		Futile.instance.camera.clearFlags = CameraClearFlags.Nothing;

		// load images
		Futile.atlasManager.LoadAtlas("Images/spritesheet");
		
		// zoom it out so we can fit a bunch more sprites in the box
		Futile.stage.scale = 0.25f;

		// set the points to world size
		FPWorld.Create(16.0f); 

		// setup container to hold all the objects
		this.objectContainer = new FContainer(); 
		this.objectContainer.SetPosition(CONTAINER_OFFSET_X/Futile.stage.scale, CONTAINER_OFFSET_Y/Futile.stage.scale); // center this in 1280x720 window
		Futile.stage.AddChild(objectContainer);

		// setup tile border objects
		if (this.usePhysics2D){	this.setupTileBorder2D(); }
		else { this.setupTileBorder3D(); }
	}

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	private void recreateWorld(){

		// destroy root GO and remove container from stage
		this.objectContainer.RemoveFromContainer();
		Futile.Destroy(GameObject.Find("FPWorld Root"));
		this.nunmberOfObjectsAdded = 0;

		// recreate physics game object: FPWorld Root
		FPWorld.Create(16.0f); 

		// new container
		this.objectContainer = new FContainer(); 
		this.objectContainer.SetPosition(CONTAINER_OFFSET_X/Futile.stage.scale, CONTAINER_OFFSET_Y/Futile.stage.scale); // center this in 1280x720 window
		this.objectContainer.shouldSortByZ = true; // enable z sorting
		Futile.stage.AddChild(objectContainer);

		// re-add the border tiles in a rectanagle
		if (this.usePhysics2D){	this.setupTileBorder2D(); }
		else { this.setupTileBorder3D(); }
	}

	// ------------------------------------------------------------------------
	// every tick until max add a new object in the middle of the square
	// ------------------------------------------------------------------------
	private void Update(){

		// reset and flop physics 2d/3d
		if (Input.GetKeyDown("r")){
			this.usePhysics2D = !this.usePhysics2D;
			this.recreateWorld();
		}
	}

	// ------------------------------------------------------------------------
	// ------------------------------------------------------------------------
	private void FixedUpdate(){

		// spawn an object somewhere in the rectanagle
		if (this.usePhysics2D){ this.addRandomObject2D(); }
		else { this.addRandomObject3D(); }
	}

	// ------------------------------------------------------------------------
	// uses PhysicsSprite2D and PhysicsComponent2D
	// ------------------------------------------------------------------------
	private void addRandomObject2D(){

		if (nunmberOfObjectsAdded < MAX_OBJECTS){

			// use a circle collider
			Vector2 position = new Vector2(UnityEngine.Random.Range(64,64*(TILES_WIDE-1)), -UnityEngine.Random.Range(64,64*(TILES_HIGH-1)));
			PhysicsSprite2D newObject = new PhysicsSprite2D(position, 0, false); // pos, rot, sq
			newObject.physicsComponent2D.StartPhysics();
			Vector2 forceVector = new Vector2(UnityEngine.Random.Range(-ACCELERATION_MAX,ACCELERATION_MAX), UnityEngine.Random.Range(-ACCELERATION_MAX,ACCELERATION_MAX));
			newObject.physicsComponent2D.AddForce(forceVector);
			this.objectContainer.AddChild(newObject);
			this.nunmberOfObjectsAdded++;
		}
	}

	// ------------------------------------------------------------------------
	// make the rectangle to contain the spawn objects
	// uses PhysicsSprite2D and PhysicsComponent2D
	// newTile.physicsComponent.DisableNodeLink(); <-- the border tiles never move so disable nodeLink to save some cycles
	// ------------------------------------------------------------------------
	private void setupTileBorder2D(){

		for (int x = 0; x<=TILES_WIDE; x++){
			PhysicsSprite2D newTile = new PhysicsSprite2D(new Vector2(x*TILE_WIDTH, -TILE_HEIGHT*0.5f));
			newTile.physicsComponent2D.DisableNodeLink(); 
			this.objectContainer.AddChild(newTile);

			PhysicsSprite2D newTile2 = new PhysicsSprite2D(new Vector2(x*TILE_WIDTH, -TILE_HEIGHT*TILES_HIGH - TILE_HEIGHT*0.5f));
			newTile.physicsComponent2D.DisableNodeLink();
			this.objectContainer.AddChild(newTile2);
		}

		for (int y = 1; y<TILES_HIGH; y++){
			PhysicsSprite2D newTile = new PhysicsSprite2D(new Vector2(0, -y*TILE_HEIGHT - TILE_HEIGHT*0.5f));
			newTile.physicsComponent2D.DisableNodeLink();
			this.objectContainer.AddChild(newTile);

			PhysicsSprite2D newTile2 = new PhysicsSprite2D(new Vector2(TILE_WIDTH*TILES_WIDE, -y*TILE_HEIGHT - TILE_HEIGHT*0.5f));
			newTile.physicsComponent2D.DisableNodeLink();
			this.objectContainer.AddChild(newTile2);
		}
	}

	// ------------------------------------------------------------------------
	// uses PhysicsSprite3D and PhysicsComponent3D
	// ------------------------------------------------------------------------
	private void addRandomObject3D(){

		if (nunmberOfObjectsAdded < MAX_OBJECTS){

			// use a sphere collider
			Vector2 position = new Vector2(UnityEngine.Random.Range(64,64*(TILES_WIDE-1)), -UnityEngine.Random.Range(64,64*(TILES_HIGH-1)));
			PhysicsSprite3D newObject = new PhysicsSprite3D(position, 0, false); // pos, rot, sq
			newObject.physicsComponent3D.StartPhysics();
			Vector3 forceVector = new Vector3(UnityEngine.Random.Range(-ACCELERATION_MAX,ACCELERATION_MAX), UnityEngine.Random.Range(-ACCELERATION_MAX,ACCELERATION_MAX), 0);
			newObject.physicsComponent3D.AddForce(forceVector);
			this.objectContainer.AddChild(newObject);
			this.nunmberOfObjectsAdded++;
		}
	}

	// ------------------------------------------------------------------------
	// make the rectangle to contain the spawn objects
	// uses PhysicsSprite3D and PhysicsComponent3D
	// newTile.physicsComponent.DisableNodeLink(); <-- the border tiles never move so disable nodeLink to save some cycles
	// ------------------------------------------------------------------------
	private void setupTileBorder3D(){

		for (int x = 0; x<=TILES_WIDE; x++){

			PhysicsSprite3D newTile = new PhysicsSprite3D(new Vector2(x*TILE_WIDTH, -TILE_HEIGHT*0.5f));
			newTile.physicsComponent3D.DisableNodeLink(); 
			this.objectContainer.AddChild(newTile);

			PhysicsSprite3D newTile2 = new PhysicsSprite3D(new Vector2(x*TILE_WIDTH, -TILE_HEIGHT*TILES_HIGH - TILE_HEIGHT*0.5f));
			newTile.physicsComponent3D.DisableNodeLink();
			this.objectContainer.AddChild(newTile2);
		}

		for (int y = 1; y<TILES_HIGH; y++){
			PhysicsSprite3D newTile = new PhysicsSprite3D(new Vector2(0, -y*TILE_HEIGHT - TILE_HEIGHT*0.5f));
			newTile.physicsComponent3D.DisableNodeLink();
			this.objectContainer.AddChild(newTile);

			PhysicsSprite3D newTile2 = new PhysicsSprite3D(new Vector2(TILE_WIDTH*TILES_WIDE, -y*TILE_HEIGHT - TILE_HEIGHT*0.5f));
			newTile.physicsComponent3D.DisableNodeLink();
			this.objectContainer.AddChild(newTile2);
		}
	}
}