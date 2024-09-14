using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClothingScript : Sprite
{

    public enum ClothingType //sets the type of clothing property for the snap logic to choose it's snap point
    {
        Hat,
        Mouth,
        Neck,
        Shirt,
        Pants,
        Shoes,
        Eye,
        Unknown,

    }

    private List<ClothingType> top_layer_list = new List<ClothingType> { ClothingType.Hat, ClothingType.Mouth, ClothingType.Neck, ClothingType.Eye };
    private List<ClothingType> bottom_layer_list = new List<ClothingType> { ClothingType.Pants, ClothingType.Shirt };

    [Export] //this makes the editor show this thing in the drop down for it to be selectable
    public ClothingType clothing_type = ClothingType.Unknown; // setting the default value to unknwo
    [Export]
    public Vector2 clothing_offset = new Vector2(0, 0); // this is the offset for the snap point
    private Rect2 clothing_box; //this is the hitbox for the mouse 
    public static int g_move_block_number = 0;
    public static List<hover_item> hover_list = new List<hover_item> { };
    public struct hover_item
    {
        public int id;
        public int layer;
    }

    public hover_item this_hover_item;

    public static int counter = 1;
    private int this_id;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Vector2 topleft = this.Transform.origin - new Vector2(this.Texture.GetWidth() / 2, this.Texture.GetHeight() / 2); //Since we want to keep the origin centered. This  to get the top left corner of the sprite
        clothing_box = new Rect2(topleft, this.Texture.GetWidth(), this.Texture.GetHeight()); // define the hitbox
        counter++;
        this_id = counter;
        GD.Print("Clothing ID: " + this_id);

        if (top_layer_list.Contains(this.clothing_type))
        {
            this.ZIndex = 1;
        }
        else
        {
            this.ZIndex = 0;
        }

        this.this_hover_item.id = this.this_id;
        this.this_hover_item.layer = this.ZIndex;

        GD.Print(this.this_id);
        GD.Print(this.ZIndex);
    }

    bool moving = false; // for toggle loging when dragging
    bool is_hovering = false; // for toggle loging when dragging
    float timer = 0;
    public override void _Process(float delta)
    {

        // timer += delta; //debugging stuff
        // if (timer > 0.5)
        // {
        //     // GD.Print(hover_list);
        //     // GD.Print("Mouse position: " + GetGlobalMousePosition());
        //     // GD.Print("Clothing position: " + clothing_box.Position);
        //     // GD.Print("Clothing end: " + clothing_box.End);
        //     // timer = 0;
        // }

        hover_handler();

        // if (g_move_block_number == 0 || g_move_block_number == this_id)
        if (select_checker())
        {
            if (Input.IsMouseButtonPressed((int)ButtonList.Left) && hover_list.Contains(this_hover_item))
            {
                g_move_block_number = this_id;
                moving = true;
                Vector2 MousePosition = GetGlobalMousePosition();
                this.Transform = new Transform2D(0, new Vector2(MousePosition.x, MousePosition.y));
                UpdateClothingBox();
            }
            else if (moving)
            {
                g_move_block_number = 0;
                moving = false;
                SnapToPoint(clothing_type);
            }
        }

    }

    public void hover_handler()
    {
        if (PlayButtonScript.MouseInside(clothing_box, GetGlobalMousePosition()))
        {
            is_hovering = true;
            if (!hover_list.Contains(this_hover_item))
            {
                hover_list.Add(this_hover_item);
                // GD.Print(hover_list);
            }

        }
        else
        {
            if (hover_list.Contains(this.this_hover_item))
            {
                hover_list.Remove(this.this_hover_item);
            }
            is_hovering = false;
        }

    }

    private bool select_checker()
    {
        if (g_move_block_number == this_id)
        {
            return true;
        }

        int highest_index = 0;
        foreach (hover_item item in hover_list)
        {
            if (item.layer > highest_index)
            {
                highest_index = item.layer;
            }
        }
        if ((this.ZIndex >= highest_index) && (g_move_block_number == 0))
        {
            return true;
        }
        return false;
    }

    public void SnapToPoint(ClothingType type)
    {
        Vector2 snap_point = new Vector2(0, 0);
        switch (type)
        {
            case ClothingType.Hat:
                snap_point = GetTree().Root.GetNode<Node2D>("Node2D").GetNode<Sprite>("Player").GetNode<Node>("ClothingPositions").GetNode<Position2D>("HatPos").Transform.origin;
                break;
            case ClothingType.Mouth:
                snap_point = GetTree().Root.GetNode<Node2D>("Node2D").GetNode<Sprite>("Player").GetNode<Node>("ClothingPositions").GetNode<Position2D>("MouthPos").Transform.origin;
                break;
            case ClothingType.Neck:
                snap_point = GetTree().Root.GetNode<Node2D>("Node2D").GetNode<Sprite>("Player").GetNode<Node>("ClothingPositions").GetNode<Position2D>("NeckPos").Transform.origin;
                break;
            case ClothingType.Shirt:
                snap_point = GetTree().Root.GetNode<Node2D>("Node2D").GetNode<Sprite>("Player").GetNode<Node>("ClothingPositions").GetNode<Position2D>("ShirtPos").Transform.origin;
                break;
            case ClothingType.Pants:
                snap_point = GetTree().Root.GetNode<Node2D>("Node2D").GetNode<Sprite>("Player").GetNode<Node>("ClothingPositions").GetNode<Position2D>("PantPos").Transform.origin;
                break;
            case ClothingType.Shoes:
                snap_point = GetTree().Root.GetNode<Node2D>("Node2D").GetNode<Sprite>("Player").GetNode<Node>("ClothingPositions").GetNode<Position2D>("ShoePos").Transform.origin;
                break;
            case ClothingType.Eye:
                snap_point = GetTree().Root.GetNode<Node2D>("Node2D").GetNode<Sprite>("Player").GetNode<Node>("ClothingPositions").GetNode<Position2D>("EyePos").Transform.origin;
                break;
            case ClothingType.Unknown:
                snap_point = new Vector2(0, 0);
                break;
        }

        // GD.Print("Snap point: " + snap_point);
        // GD.Print("Clothing Type " + type);
        //TODO make this a function
        if (CloseToPoint(this.Transform.origin, snap_point))
        {
            this.Transform = new Transform2D(0, snap_point + clothing_offset);
            // clothing_box.Position = this.Transform.origin - new Vector2(this.Texture.GetWidth() / 2, this.Texture.GetHeight() / 2);
            UpdateClothingBox();
            GetTree().Root.GetNode<Node2D>("Node2D").GetNode<AudioStreamPlayer>("SelectSound").Play();
        }
    }

    private bool CloseToPoint(Vector2 object_pos, Vector2 snap_point, float tolerance = 100)
    {
        if (object_pos.x > snap_point.x - tolerance && object_pos.x < snap_point.x + tolerance &&
            object_pos.y > snap_point.y - tolerance && object_pos.y < snap_point.y + tolerance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateClothingBox()
    {
        Vector2 topleft = this.Transform.origin - new Vector2(this.Texture.GetWidth() / 2, this.Texture.GetHeight() / 2); //Since we want to keep the origin centered. This  to get the top left corner of the sprite
        clothing_box = new Rect2(topleft, this.Texture.GetWidth(), this.Texture.GetHeight()); // define the hitbox
    }
}

