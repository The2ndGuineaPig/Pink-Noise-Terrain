using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] Camera2D camera2D;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Called every time the node is added to the scene.
		// Initialization here
		// With anchor point in center set top left corner to 0,0
		
		camera2D.AnchorMode = Camera2D.AnchorModeEnum.FixedTopLeft;
		camera2D.LimitLeft = 0;
		camera2D.LimitTop = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Called every frame
		// Process here

		// Move camera with arrow keys

		if (Input.IsActionPressed("ui_right"))
		{
			camera2D.Position += new Vector2(1, 0);
		}

		if (Input.IsActionPressed("ui_left"))
		{
			camera2D.Position += new Vector2(-1, 0);
		}

		if (Input.IsActionPressed("ui_down"))
		{
			camera2D.Position += new Vector2(0, 1);
		}

		if (Input.IsActionPressed("ui_up"))
		{
			camera2D.Position += new Vector2(0, -1);
		}

	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.WheelUp)
			{
				GD.Print(mouseButton);
				camera2D.Zoom = camera2D.Zoom + new Vector2(0.1f, 0.1f);
			}
			else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
			{
				GD.Print(mouseButton);
				camera2D.Zoom = camera2D.Zoom - new Vector2(0.1f, 0.1f);

				// Enforce minimum zoom level of 1
				if (camera2D.Zoom.X < 1)
				{
					camera2D.Zoom = new Vector2(1, 1);
				}
			}
		}
	}
}


