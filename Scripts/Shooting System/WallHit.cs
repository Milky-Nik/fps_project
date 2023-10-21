using Godot;
using Godot.Collections;
using System;

public partial class WallHit : DamageableNode
{
    [Export] public PackedScene particlesPrefab;

    public override void OnHit(Dictionary hit)
    {
        // var particles = particlesPrefab.Instantiate(particlesPrefab, hit.point + (hit.normal * 0.05f), Quaternion.LookRotation(hit.normal), transform.root.parent);

        var particles = particlesPrefab.Instantiate() as Node3D;
        GetParent().AddChild(particles);

        particles.Position = (Vector3)hit["position"] + (Vector3)hit["normal"] * 0.05f;
        particles.Rotation = (Vector3)hit["normal"];
    }
}
