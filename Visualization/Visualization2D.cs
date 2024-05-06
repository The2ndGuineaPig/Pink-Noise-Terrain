using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Visualization2D : Sprite2D
{
	private Image image;
	private Vector2I imageSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetImageSize(new Vector2I(512, 512));
		RenderNoise(0.1, new Vector2(0.0f, 0.0f));
	}

	public void SetImageSize(Vector2I imgSize)
	{
		imageSize = imgSize;
		image = Image.Create(imageSize.X, imageSize.Y, false, Image.Format.Rgba8);
	}

	public void SetCanvasPos(Vector2I uv)
	{
		Position = uv;
	}

	public void RenderNoise(double sizeStep, Vector2 noiseOrigin)
	{
		for (int i = 0; i < imageSize.X; i++)
		{
			for (int j = 0; j < imageSize.Y; j++)
			{
				float noiseValue = ((float)Simplex.noise(
					(double)(noiseOrigin.X + i * sizeStep),
					(double)(noiseOrigin.Y + j * sizeStep)
				) + 1.0f) / 2.0f; // Simplex.noise returnere værdi [-1; 1], så addere 1, så [0; 2] og dividere med 2, dermed fåes værdimængden [0; 1]

				image.SetPixel(i, j, new Color(noiseValue, noiseValue, noiseValue));
			}
		}

		Texture = ImageTexture.CreateFromImage(image);
	}
}
