using Godot;
using System;

public class ClothingScript : Sprite
{
    Rect2 clothing_box;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        clothing_box = new Rect2(this.Transform.origin, this.Texture.GetWidth(), this.Texture.GetHeight());
    }

    bool moving = false;
    float timer = 0;
    public override void _Process(float delta)
    {
        if (timer++ > 10)
        {
            GD.Print("Mouse position: " + GetGlobalMousePosition());
            GD.Print("Clothing position: " + clothing_box.Position);
            GD.Print("Clothing end: " + clothing_box.End);
            timer = 0;
        }

        if (PlayButtonScript.MouseInside(clothing_box, GetGlobalMousePosition()))
        {
            moving = true;
        }

        if (Input.IsMouseButtonPressed((int)ButtonList.Left) && moving)
        {
            Vector2 MousePosition = GetGlobalMousePosition();
            this.Transform = new Transform2D(0, new Vector2(MousePosition.x, MousePosition.y));
            clothing_box.Position = this.Transform.origin;
        }
        else
        {
            moving = false;
        }

    }
}
