using Godot;
using System;

public class PlayButtonScript : Sprite
{
    private Texture hover_texture = GD.Load<Texture>("res://Assets/PlayHover.png");
    private Texture normal_texture = GD.Load<Texture>("res://Assets/PlayNeutral.png");
    private Rect2 box;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("PlayButtonScript is ready");
        box = new Rect2(this.Transform.origin, this.Texture.GetWidth(), this.Texture.GetHeight());
    }

    public static bool MouseInside(Rect2 box, Vector2 mousePosition)
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
            GD.Print("clicked");
            GD.Print(GetGlobalMousePosition());
            if (MouseInside(box, GetGlobalMousePosition()))
            {
                GD.Print("clicked");
                if (mouseButton.ButtonIndex == (int)ButtonList.Left && mouseButton.Pressed)
                {
                    GD.Print("Play button clicked");
                    this.GetNode<AudioStreamPlayer>("SelectSound").Play();
                    PackedScene simultaneousScene = (PackedScene)ResourceLoader.Load("res://Scenes/Game.tscn"); // TODO magic
                    GetTree().ChangeSceneTo(simultaneousScene);
                }
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
