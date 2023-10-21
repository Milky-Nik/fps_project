using Godot;
using System;

public partial class Camera_Controller : Node3D
{
	[Export] float mouse_sensitivity = 0.25f;
	float defaultHeadYPositon;
	MovementState movementState;
	[Export] float crouching_Head_Position = -0.5f;
	[Export] float head_crouch_lerp_speed = 10f;

	#region HeadBobbing
    [Export] float head_bobbing_lerp_speed = 10f;
	[Export] float head_bobbing_sprinting_speed = 22f;
	[Export] float head_bobbing_walking_speed = 14f;
	[Export] float head_bobbing_crouching_speed = 10f;

	[Export] float head_bobbing_sprinting_intensity = .2f;
	[Export] float head_bobbing_walking_intensity = .1f;
	[Export] float head_bobbing_crouching_intensity = .05f;

	Vector2 head_bobbing_vector = Vector2.Zero;
	float head_bobbing_index = 0f;
	float head_bobbing_current_intensity = 0f;
	#endregion

    [Export] player player;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		defaultHeadYPositon = Position.Y;
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion eventMouseMotion)
		{
			player.RotateY(Mathf.DegToRad(-eventMouseMotion.Relative.X * mouse_sensitivity));
 
			var rotateX = Mathf.Clamp(Mathf.DegToRad(-eventMouseMotion.Relative.Y * mouse_sensitivity), Mathf.DegToRad(-89), Mathf.DegToRad(89));
            RotateX(rotateX);

            var rotation = Rotation;
			rotation.X = Mathf.Clamp(rotation.X, Mathf.DegToRad(-89), Mathf.DegToRad(89));
			Rotation = rotation;
        }
	}

    public override void _PhysicsProcess(double delta)
    {
		movementState = player.movementState;
        HeadBobbingHandler(delta);
		updateCameraPosition(delta);
    }

    void HeadBobbingHandler(double delta)
	{
		switch(movementState)
		{
			case MovementState.sprinting:
				head_bobbing_current_intensity = head_bobbing_sprinting_intensity;
				head_bobbing_index += head_bobbing_sprinting_speed;
			break;
			case MovementState.walking:
				head_bobbing_current_intensity = head_bobbing_walking_intensity;
				head_bobbing_index += head_bobbing_walking_speed;
			break;
			case MovementState.crouching:
				head_bobbing_current_intensity = head_bobbing_crouching_intensity;
				head_bobbing_index += head_bobbing_crouching_speed;
			break;
		}

		var pos = Position;

		if(player.IsOnFloor() && !player.isSliding() && player.isMovingInputsArePressed())
		{
			var hb_index = head_bobbing_index * (float)delta;
			head_bobbing_vector.Y = Mathf.Sin(hb_index);
			head_bobbing_vector.X = Mathf.Sin(hb_index/2)+0.5f;

			pos.Y = Mathf.Lerp(pos.Y, head_bobbing_vector.Y * (head_bobbing_current_intensity/2), (float)delta * head_bobbing_lerp_speed);
			pos.X = Mathf.Lerp(pos.X, head_bobbing_vector.X * (head_bobbing_current_intensity), (float)delta * head_bobbing_lerp_speed);
		}
		else
		{
			pos.Y = Mathf.Lerp(pos.Y,0, (float)delta * head_bobbing_lerp_speed);
			pos.X = Mathf.Lerp(pos.X,0, (float)delta * head_bobbing_lerp_speed);
		}


		Position = pos;
	}

	void updateCameraPosition(double delta)
	{
		var headpos = Position;

		float lerpTo = defaultHeadYPositon;

		if(movementState == MovementState.crouching) lerpTo += crouching_Head_Position;

		headpos.Y = Mathf.Lerp(headpos.Y, lerpTo, (float)delta * head_crouch_lerp_speed);;
		Position = headpos;
	}
}
