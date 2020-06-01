using Godot;

public class Main : Spatial
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        return;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept")) {
            GetTree().ReloadCurrentScene();
        }
    }
}
