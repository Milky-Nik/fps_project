using Godot;
using System;

public partial class CharacterAnimController : Node
{
	[Export] float weaponVisualRecoil = 0.1f;
	[Export] Node3D WeaponPosition;
	float defaultWeaponPositionZ;

	[Export] AnimationPlayer animationPlayer;
	[Export] player player;
	[Export] float hard_landing_velocity = -15f;
	static string JUMP_ANIMATION = "jump";
	static string LAND_ANIMATION = "landing";
	static string HARD_LAND_ANIMATION = "hard_landing";

	public override void _Ready()
	{
		defaultWeaponPositionZ = WeaponPosition.Position.Z;


		player.OnPlayerLand += landingVelocity =>
		{
			if(landingVelocity >= hard_landing_velocity) PlayLandAnimation();
			else
			{
				// GD.Print("hard landing");
				PlayHardLandAnimation();
			} 
		};
		player.OnPlayerJump += e =>
		{
			PlayJumpAnimation();
		};
	}

	void PlayJumpAnimation() => animationPlayer.Play(JUMP_ANIMATION);
	void PlayLandAnimation() => animationPlayer.Play(LAND_ANIMATION);
	void PlayHardLandAnimation() => animationPlayer.Play(HARD_LAND_ANIMATION);


    public override void _PhysicsProcess(double delta)
    {
		//makes weapon always return to default position
		
		float lerpingZ = Mathf.Lerp(WeaponPosition.Position.Z, defaultWeaponPositionZ, (float)delta * 10f);

		WeaponPosition.Position = new Vector3(WeaponPosition.Position.X, WeaponPosition.Position.Y, lerpingZ);
    }

    public void PlayWeaponShootAnimation()
	{
		WeaponPosition.Position = new Vector3(WeaponPosition.Position.X, WeaponPosition.Position.Y, WeaponPosition.Position.Z + weaponVisualRecoil);
	}
}

