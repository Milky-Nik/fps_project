using Godot;
using System;

public partial class WeaponStats : Resource
{
    [Export] public float Damage {get; private set;}
    [Export] public float AttackSpeed {get; private set;}
    [Export] public bool isMeleeWeapon {get; private set;}

    // public WeaponStats() : this(0, 0) {}
    // public WeaponStats(int damage, int attackSpeed)
    // {
    //     Damage = damage;
    //     AttackSpeed = attackSpeed;
    // }
}
