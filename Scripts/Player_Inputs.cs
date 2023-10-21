using Godot;
using System;

public partial class Player_Inputs : Node
{
    [Export] Weapon_Manager weapon_Manager;
    [Export] AttackManager attackManager;

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventKey inputEventKey)
        {
            if(inputEventKey.Keycode == Key.Key1) weapon_Manager.WeaponSwitch(0);
            if(inputEventKey.Keycode == Key.Key2) weapon_Manager.WeaponSwitch(1);
            if(inputEventKey.Keycode == Key.Key3) weapon_Manager.WeaponSwitch(2);
            if(inputEventKey.Keycode == Key.Key4) weapon_Manager.WeaponSwitch(3);
        }
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionPressed("fire"))
        {
            attackManager.Shoot();
        }
    }
}
