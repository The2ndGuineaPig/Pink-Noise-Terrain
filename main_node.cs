using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class main_node : Node
{
	PackedScene scene = GD.Load<PackedScene>("res://Visualization/3DScene.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		AddChild(scene.Instantiate() as Node3D);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_button_button_down()
	{
		Simplex.GenerateRandomSeed();
		Node3D node = GetChildren()[1] as Node3D;
		Visualization3D meshNode = node.GetChild<MeshInstance3D>(0) as Visualization3D;
		meshNode.InstantiateMesh(0.01, 0.5f, new Vector2(1.0f, 1.0f));
		GD.Print("Button pressed");
	}	

	public void _on_button_button_up()
	{

		AddChild(scene.Instantiate() as Node3D);
		
	}

}
