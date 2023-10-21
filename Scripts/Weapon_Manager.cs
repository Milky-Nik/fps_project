using Godot;
using System;
using System.Collections.Generic;

public partial class Weapon_Manager : Node3D
{
    [Export] AttackManager attackManager;
    [Export] Resource weapon_stats1;
    [Export] Resource weapon_stats2;
    [Export] Resource weapon_stats3;
    [Export] Resource weapon_stats4;
    List<Resource> weapon_resource_stats = new List<Resource>();

    int dmg;

    public Node3D currentSelectedWeapon {get; private set;}
    public Resource currentSelectedWeaponStats {get; private set;}
    public override void _Ready()
    {
        weapon_resource_stats.Add(weapon_stats1);
        weapon_resource_stats.Add(weapon_stats2);
        weapon_resource_stats.Add(weapon_stats3);
        weapon_resource_stats.Add(weapon_stats4);
        
        WeaponSwitch(0);
    }

    public void WeaponSwitch(int weapon_index)
    {
        if(currentSelectedWeapon != null) currentSelectedWeapon.Hide();

        currentSelectedWeapon = (Node3D)GetChild(weapon_index);

        if(weapon_resource_stats[weapon_index] is WeaponStats weaponStats) 
        attackManager.ChangeStats(weaponStats.Damage, weaponStats.AttackSpeed, weaponStats.isMeleeWeapon);
        
        currentSelectedWeapon.Show();
    }
}
