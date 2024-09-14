using Godot;
using System;

public class BinClearScript : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Rect2 this_box;
    Node2D item_node;
    public override void _Ready()
    {
        Vector2 topleft = this.Transform.origin - new Vector2(this.Transform.Scale.x * (this.Texture.GetWidth() / 2), this.Transform.Scale.y * (this.Texture.GetHeight() / 2));
        this_box = new Rect2(topleft, this.Transform.Scale.x * this.Texture.GetWidth(), this.Transform.Scale.y * this.Texture.GetHeight());
        item_node = GetTree().Root.GetNode<Node2D>("Node2D").GetNode<Node2D>("Items");
    }

    bool toggle = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
        if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            if (!toggle)
            {
                if (PlayButtonScript.MouseInside(this_box, GetGlobalMousePosition()))
                {
                    GD.Print("bin clicked");
                    GetTree().Root.GetNode<Node2D>("Node2D").GetNode<AudioStreamPlayer>("SelectSound").Play();
                    delete_children();

                    ClothingScript.hover_list.Clear();
                    ClothingScript.g_move_block_number = 0;
                }
                toggle = true;
            }
        }
        else
        {
            toggle = false;
        }
    }

    void delete_children()
    {
        foreach (Sprite clothe in item_node.GetChildren())
        {
            clothe.Free();
        }
    }

}

