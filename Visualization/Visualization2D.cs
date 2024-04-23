using Godot;
using System;

public partial class Visualization2D : TextureRect
{
	private Image image;
	[Export] int imgWidth;
	[Export] int imgHeight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Test");
		image = Image.Create(imgWidth, imgHeight, false, Image.Format.Rgba8);

		for (int i = 0; i < imgWidth; i++)
		{
			for (int j = 0; j < imgHeight; j++)
			{
				image.SetPixel(i, j, Colors.Black);
			}	
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
