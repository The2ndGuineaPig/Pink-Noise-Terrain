using Godot;
using System;
using System.Net;
using System.Text;

public partial class Globals : Node
{
    public static Globals data;

    // add variables here 
    public Vector2I imgSize = new Vector2I(512, 512);
    public int seed;
    public Globals()
    {
        Globals.data = this;
    }

    public override void _Ready()
    {
        GD.Print("Globals loaded.");
    }
}