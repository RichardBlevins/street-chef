using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float Mouse_Sensitivity = 0.004f;
	private Node3D Head;

    public override void _Ready()
    {
        base._Ready();
		Head = GetNode<Node3D>("Head");

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion motion) {
			// rotate left and right
			RotateY(-motion.Relative.X * Mouse_Sensitivity);
			// rotate up and down
			Head.RotateX(-motion.Relative.Y * Mouse_Sensitivity);


			// clamp head rotation so doesnt flip
			Vector3 rot = Head.Rotation;
        	rot.X = Mathf.Clamp(rot.X, -Mathf.Pi / 2, Mathf.Pi / 2);
        	Head.Rotation = rot;
		}

	}
}
