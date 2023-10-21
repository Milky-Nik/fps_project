using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public abstract partial class DamageableNode : Node
{
    public abstract void OnHit(Dictionary hit);
}
