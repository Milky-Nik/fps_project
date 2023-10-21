using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;
using System.Data.Common;

public partial class bullet : Node3D
{
    float speed;
    float gravity;
    Vector3 startPosition;
    Vector3 startForward;

    bool isInitialized = false;
    float startTime = -1f;

    public static double Timetime = 0;

    public void Initialize(Vector3 startPoint, float speed, float gravity)
    {
        startPosition = startPoint;
        startForward = startPoint + Vector3.Forward;
        this.speed = speed;
        this.gravity = gravity;
        isInitialized = true;
    }

    private Vector3 FindPointOnParabola(float delta)
    {
        Vector3 point = startPosition + (startForward * speed * delta);
        Vector3 gravityVec = Vector3.Down * gravity * delta * delta;
        return point + gravityVec;
    }

    public bool CastRayBetweenPoints(Vector3 startPoint, Vector3 endPoint, out Dictionary hit)
    {
        var rayQuery = new PhysicsRayQueryParameters3D
        {
            From = startPoint,
            To = endPoint,
        };

        var result = GetWorld3D().DirectSpaceState.IntersectRay(rayQuery);

        if(result.Count > 0)
        {
            hit = result;
            return true;
        }
        else
        {
            hit = null;
            return false;
        }
    }

    public override void _Ready()
    {
        Timetime = 0;
    }

    public override void _Process(double delta) {
        Timetime += delta;

        if(isInitialized && startTime > 0 )
        {
            float currentTime = (float)Timetime - startTime;
            Vector3 currentPoint = FindPointOnParabola(currentTime);
            Position = currentPoint;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print(Position);

        if(isInitialized)
        {
            if(startTime < 0 ) startTime = (float)delta;

            Dictionary hit;
            float currentTime = (float)Timetime - startTime;
            float prevTime = currentTime - (float)delta;
            float nextTime = currentTime + (float)delta;

            Vector3 currentPoint = FindPointOnParabola(currentTime);

            Vector3 nextPoint = FindPointOnParabola(nextTime);
            // LookAt(nextPoint);

            if(prevTime > 0)
            {
                Vector3 prevPoint = FindPointOnParabola(prevTime);
                if(CastRayBetweenPoints(prevPoint, currentPoint, out hit))
                {
                    OnHit(hit);
                }
            }

            if(CastRayBetweenPoints(currentPoint, nextPoint, out hit))
            {
                OnHit(hit);
            }
        }
    }

    private void OnHit(Dictionary hit)
    {
        var collidedNode = hit["collider"] as Object;

        DamageableNode collidedDamageableNode = collidedNode as DamageableNode;

        if(collidedDamageableNode != null)
        {
            collidedDamageableNode.OnHit(hit);
        }

         GD.Print("hit! " + hit["collider"]);

        QueueFree();
    }
}