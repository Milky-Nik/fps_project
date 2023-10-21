using Godot;
using System;

public enum MovementState
{
	walking,
	sprinting,
	crouching,
	sliding
}

public partial class player : CharacterBody3D
{
	[Export] public shooting_system shooting_System;
	public event Action<float> OnPlayerLand = delegate { };
	public event Action<float> OnPlayerJump = delegate { };
	float currentSpeed;
	[Export] float walking_speed = 6.0f;
	[Export] float running_speed = 9.5f;
	[Export] float crouching_speed = 4.5f;

	public MovementState movementState;

	[Export] float jump_velocity = 4.5f;
	[Export] public CollisionShape3D standingCollisionShape;
	[Export] public CollisionShape3D crouchCollisionShape;
	[Export] public RayCast3D RayCrouchHeadCheck;

	[Export] float crouch_speed_lerp_speed = 0.5f;
	[Export] float walking_lerp_speed = 10f;
	[Export] float gravityMultiplier;

	Vector3 currentDirection = Vector3.Zero;
	Vector2 inputDirection = Vector2.Zero;
	Vector3 direction = Vector3.Zero;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	Vector3 last_velocity = Vector3.Zero;

	public override void _Ready()
	{
		gravity *= gravityMultiplier;

		currentSpeed = walking_speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		MovementHandler(delta);

		if(last_velocity.Y < 0.0f && IsOnFloor()) OnPlayerLand(last_velocity.Y); 

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = jump_velocity;
			OnPlayerJump(1);
		}


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		inputDirection = Input.GetVector("left", "right", "forward", "backward");

		direction = (Transform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();

		var currentMovementLerping =  delta * walking_lerp_speed;

		if(!isMovingInputsArePressed() && isSliding()) currentMovementLerping = delta * crouch_speed_lerp_speed;

		float currentDirectionX = Mathf.Lerp(currentDirection.X, direction.X, (float)currentMovementLerping);
		float currentDirectionZ = Mathf.Lerp(currentDirection.Z, direction.Z, (float)currentMovementLerping);

		currentDirection = new Vector3(currentDirectionX,0,currentDirectionZ);

		if (currentDirection != Vector3.Zero)
		{
			velocity.X = currentDirection.X * currentSpeed;
			velocity.Z = currentDirection.Z * currentSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, currentSpeed);
		}

		Velocity = velocity;

		last_velocity = velocity;
		MoveAndSlide();
	}

	void MovementHandler(double delta)
	{
		if (Input.IsActionPressed("crouch") || RayCrouchHeadCheck.IsColliding())
		{
			movementState = MovementState.crouching;
			ChangeSpeed(delta, crouching_speed);

			standingCollisionShape.Disabled = true;
			crouchCollisionShape.Disabled = false;

			return;
		}

		standingCollisionShape.Disabled = false;
		crouchCollisionShape.Disabled = true;

		if(Input.IsActionPressed("sprint") && isMovingInputsArePressed())
		{
			movementState = MovementState.sprinting;
			ChangeSpeed(delta, running_speed);
			return;
		}

		movementState = MovementState.walking;
		ChangeSpeed(delta, walking_speed);
	}

	void ChangeSpeed(double delta, float newSpeed)
	{
		float currentLerpSpeed;

		if(movementState == MovementState.crouching)
		{
			currentLerpSpeed = crouch_speed_lerp_speed;
		}
		else
		{
			currentLerpSpeed = walking_lerp_speed;
		}

		currentSpeed = Mathf.Lerp(currentSpeed, newSpeed, (float)delta * currentLerpSpeed);
	}

	public bool isSliding()
	{
		if(movementState == MovementState.crouching && currentSpeed >= walking_speed+1) return true;
		else return false;
	}

	public bool isMovingInputsArePressed()
	{
		if(inputDirection != Vector2.Zero) return true;
		else return false;
	}
}
