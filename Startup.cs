using Godot;
using System;

public partial class Startup : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(Simplex.noise(2,5));
		Random rnd = new Random("Your string".GetHashCode());
		int randomNumber = rnd.Next();
		GD.Print(randomNumber);
		GD.Print(rnd.Next());
		GD.Print(rnd.Next());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
