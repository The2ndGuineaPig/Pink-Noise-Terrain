using Godot;
using System;

public partial class menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_Button_Button_up()
	{
		PackedScene scene = GD.Load<PackedScene>("res://main_node.tscn");
		GetTree().ChangeSceneToPacked(scene);
	}
}
