using Godot;
using System;

public partial class StartMenu : Control 
{

	[Export] LineEdit imgSizeEdit;
	[Export] LineEdit seedEdit;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("StartMenu loaded.");
		imgSizeEdit.Text = "512";
		seedEdit.Text = "0";
		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _BtnGenerateUp()
	{
		GD.Print("Button Generate Up");
		Globals.data.imgSize = new Vector2I(int.Parse(imgSizeEdit.Text), int.Parse(imgSizeEdit.Text));
		GD.Print(Globals.data.imgSize);
		Globals.data.seed = int.Parse(seedEdit.Text);
		GD.Print(Globals.data.seed);
		GetTree().ChangeSceneToFile("res://Visualization/2DVisualization.tscn"); 
	}


}
