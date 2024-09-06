using Godot;
using System;

public class AddClotheButton : Sprite
{
    [Export]
    public PackedScene clothe_to_add;
    private Rect2 this_box;
    public override void _Ready()
    {
        Vector2 topleft = this.Transform.origin - new Vector2(this.Texture.GetWidth() / 2, this.Texture.GetHeight() / 2);
        this_box = new Rect2(topleft, this.Texture.GetWidth(), this.Texture.GetHeight());
    }

    bool toggle = false;
    public override void _Process(float delta)
    {
        set_box();

        if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            if (!toggle)
            {
                if (PlayButtonScript.MouseInside(this_box, GetGlobalMousePosition()))
                {
                    GD.Print("AddClotheButton clicked");
                    GetTree().Root.GetNode<Node2D>("Node2D").AddChild(clothe_to_add.Instance());
                }
                toggle = true;
            }
        }
        else
        {
            toggle = false;
        }
    }
    private void set_box()
    {
        Vector2 topleft = this.Transform.origin - new Vector2(this.Transform.Scale.y * this.Texture.GetWidth() / 2, this.Transform.Scale.y * this.Texture.GetHeight() / 2);
        topleft -= new Vector2(0, GetTree().Root.GetNode<Node2D>("Node2D").GetNode<ScrollContainer>("ScrollContainer").ScrollVertical);
        this_box = new Rect2(topleft, this.Transform.Scale.x * this.Texture.GetWidth(), this.Transform.Scale.y * this.Texture.GetHeight());
    }
}

