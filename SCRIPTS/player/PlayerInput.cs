using Godot;
using System;

public partial class PlayerInput : Node
{
	// Game Objects : 
	Node player;
	
	
	// Variables : 
	int InputX;
	int InputY;
	
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode("../../");
		
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent ev)
	{
		if(ev is InputEventMouseMotion)
		{
			InputEventMouseMotion mm = (InputEventMouseMotion)ev;
			
			player.Call(method: "Look" , mm.Relative);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CheckInputActions();
		
		// move the player : 
		player.Call(method: "Move" , GetInputDirectionWasd());
	}
	
	void CheckInputActions()
	{
		if(Input.IsActionPressed("forward"))
		{
			InputY = -1;
		}
		
		if(Input.IsActionPressed("back"))
		{
			InputY = 1;
		}
		
		if(Input.IsActionPressed("left"))
		{
			InputX = -1;
		}
		
		if(Input.IsActionPressed("right"))
		{
			InputX = 1;
		}
		
		// 
		if(Input.IsActionJustReleased("forward") || Input.IsActionJustReleased("back"))
		{
			InputY = 0;
		}
		
		if(Input.IsActionJustReleased("left") || Input.IsActionJustReleased("right"))
		{
			InputX = 0;
		}
	}
	
	public Vector2 GetInputDirectionWasd()
	{
		// returns a vector corresponding to the input direction of the wasd keys pressed.
		Vector2 inputDirection = new Vector2(InputX , InputY);
		return inputDirection;
	}
}
