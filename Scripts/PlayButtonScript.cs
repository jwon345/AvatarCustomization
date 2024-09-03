using Godot;
using System;

public class PlayButtonScript : Sprite
{
    private Texture hover_texture = GD.Load<Texture>("res://Assets/PlayHover.png");
    private Texture normal_texture = GD.Load<Texture>("res://Assets/PlayNeutral.png");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("PlayButtonScript is ready");
    }

    private bool MouseInside(Rect2 box, Vector2 mousePosition)
    {
        if (box.Position.x < mousePosition.x &&
            box.Position.y < mousePosition.y &&
            box.End.x > mousePosition.x &&
            box.End.y > mousePosition.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            Rect2 box = new Rect2(this.Transform.origin, this.Texture.GetWidth(), this.Texture.GetHeight());

            if (MouseInside(box, mouseMotion.Position))
            {
                this.Texture = hover_texture;
            }
            else
            {
                this.Texture = normal_texture;
                // GD.Print("Mouse is not over the play button");
            }
        }
        if (@event is InputEventMouseButton mouseButton)
        {
            if (this.Texture.GetWidth() > mouseButton.Position.x &&
                this.Texture.GetHeight() > mouseButton.Position.y)
            {
                GD.Print("Play button clicked");
            }
        }
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.

    float counter = 0;
    public override void _Process(float delta)
    {
        if (counter++ > 2)
        {
            // GD.Print("x:" + this.Transform.origin.x + " y:" + this.Transform.origin.y);
        }
    }
}
