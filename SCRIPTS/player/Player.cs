using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// Game Objects : 
	Node3D cam;
	
	
	
	// Variables : 
	float TargetZSpeed;
	float TargetXSpeed;

	float ZSpeed;
	float XSpeed;

	float MaxSpeed = 10f;
	
	float MaxCamLookX = 70;
	
	float GravityAcceleration = 0.2f;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cam = (Node3D)GetNode("cam");
	}

	public override void _PhysicsProcess(double delta)
	{
		InterpolateSpeed();
		ApplyMovementForces();
		ApplyGravity();
		
		this.MoveAndSlide();
	}
	
	// 
	public void Move(Vector2 direction)
	{
		// direction is the value of the input in the x and z axis , but inform of a vec2
		// called from the playerInput script every process frame.
		
		Vector2 directionNormalized = direction.Normalized();
		
		TargetZSpeed = directionNormalized.Y * MaxSpeed;
		TargetXSpeed = directionNormalized.X * MaxSpeed;
	}
	
	void InterpolateSpeed()
	{
		XSpeed = Mathf.Lerp(from: XSpeed , to: TargetXSpeed , weight: 0.05f);
		ZSpeed = Mathf.Lerp(from: ZSpeed , to: TargetZSpeed , weight: 0.05f);
	}
	
	void ApplyMovementForces()
	{
		Vector3 sideDirection = this.GlobalTransform.Basis.X;
		Vector3 sideVel = XSpeed * sideDirection;
		
		Vector3 forwardDirection = this.GlobalTransform.Basis.Z;
		Vector3 forwardVel = forwardDirection * ZSpeed;
		
		Vector3 newVel = sideVel + forwardVel;
		
		float initialYVel = Velocity.Y;
		newVel.Y = initialYVel;					// avoid cancelling the velocity in the y axis : 
		
		Velocity = newVel;
	}
	
	public void Look(Vector2 mouseRel)
	{
		// rotate in the y axis : 
		Vector3 newRot = this.RotationDegrees;
		newRot.Y = newRot.Y - (mouseRel.X / 10);
		this.RotationDegrees = newRot;
		
		// rotate the camera in the x axis : 
		Vector3 newCamRot = cam.RotationDegrees;
		newCamRot.X = newCamRot.X - (mouseRel.Y / 15);
		newCamRot.X = Mathf.Clamp(value: newCamRot.X , min: -MaxCamLookX , max: MaxCamLookX);
		
		cam.RotationDegrees = newCamRot;
	}
	
	void ApplyGravity()
	{
		Vector3 newVel = Velocity;
		
		if(this.IsOnFloor() == false)
		{
			newVel.Y = newVel.Y - GravityAcceleration;
		}
		else
		{
			newVel.Y = 0;
		}
		
		Velocity = newVel;
	}
}
