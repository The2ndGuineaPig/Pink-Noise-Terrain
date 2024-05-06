using Godot;
using System;

public partial class dpad : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Draw()
    {
        DrawRect(new Rect2(0, 0, 300, 300), new Color(0.40f, 0.40f, 0.40f, 1));
        DrawCircle(new Vector2(150, 100), 50, new Color(0.56f, 0.56f, 0.56f, 1));
        DrawCircle(new Vector2(100, 150), 50, new Color(0.56f, 0.56f, 0.56f, 1));
        DrawCircle(new Vector2(200, 150), 50, new Color(0.56f, 0.56f, 0.56f, 1));
		DrawCircle(new Vector2(150, 200), 50, new Color(0.56f, 0.56f, 0.56f, 1));
        base._Draw();

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
