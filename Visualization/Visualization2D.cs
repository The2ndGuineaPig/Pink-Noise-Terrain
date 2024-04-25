using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Visualization2D : Sprite2D
{
	private Image image;
	[Export] private int imgWidth;
	[Export] private int imgHeight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		image = Image.Create(imgWidth, imgHeight, false, Image.Format.Rgba8);

		for (int i = 0; i < imgWidth; i++)
		{
			for (int j = 0; j < imgHeight; j++)
			{
				float R = 255.0f / (float)i;
				float G = 255.0f / (float)i;
				float B = 255.0f / (float)(i*j);
				image.SetPixel(i, j, new Color(R, G, B));
			}
		}

		Texture = ImageTexture.CreateFromImage(image);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
