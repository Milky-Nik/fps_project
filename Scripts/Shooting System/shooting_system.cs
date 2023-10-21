using Godot;
using System;

public partial class shooting_system : Node3D
{
    Vector3 shootPoint;
    [Export] private float gravityForce = 9.8f;
    [Export] private float BulletSpeed = 70f;
    [Export] private float BulletLifeTime = 10f;
    [Export] PackedScene bulletPref;
    [Export] PackedScene testCube;

    public override void _Ready()
    {
        shootPoint = Position;
    }


    public void Shoot()
    {
        var bullet = bulletPref.Instantiate() as bullet;

        AddChild(bullet);

        bullet.Initialize(shootPoint, BulletSpeed, gravityForce);
    }
}
