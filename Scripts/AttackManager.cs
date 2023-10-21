using Godot;
using System;

public partial class AttackManager : Node
{
    [Export] CharacterAnimController characterAnimController;
    player player;
    float damage = 0;
    float attack_speed = 1;
    float attack_speed_timer = 0;
    bool isMeleeWeapon = false;

    public override void _Ready()
    {
        attack_speed_timer = attack_speed;
        player = GetParent() as player;
    }

    public override void _Process(double delta)
    {
        if(attack_speed_timer < attack_speed) attack_speed_timer += (float)delta;
    }

    public void Shoot()
    {
        if(attack_speed_timer < attack_speed) return;
        attack_speed_timer = 0;

        if(!isMeleeWeapon) characterAnimController.PlayWeaponShootAnimation();
    }

    public void ChangeStats(float damage, float attack_speed, bool isMeleeWeapon)
    {
        this.damage = damage;
        this.attack_speed = attack_speed;
        this.isMeleeWeapon = isMeleeWeapon;

        attack_speed_timer = attack_speed;
    }
}
