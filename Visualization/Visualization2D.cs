using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Visualization2D : Sprite2D
{
	private Image image;
	private Vector2I imageSize;
	private Vector2 noiseOrigin = new(0.0f, 0.0f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetImageSize(new(512, 512));
		RenderNoise();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetImageSize(Vector2I imgSize)
	{
		imageSize = imgSize;
		image = Image.Create(imageSize.X, imageSize.Y, false, Image.Format.Rgba8);
	}

	public void SetNoiseOrigo(Vector2 xy)
	{
		noiseOrigin = xy;
	}

	public void SetCanvasPos(Vector2I uv)
	{
		Position = uv;
	}

	public void RenderNoise()
	{
		for (int i = 0; i < imageSize.X; i++)
		{
			for (int j = 0; j < imageSize.Y; j++)
			{
				float noiseValue = (float)Simplex.noise((double)(noiseOrigin.X + i), (double)(noiseOrigin.Y + j)) / 2.0f + 1.0f;
				image.SetPixel(i, j, new Color(noiseValue, noiseValue, noiseValue));
			}
		}

		Texture = ImageTexture.CreateFromImage(image);
	}
}
