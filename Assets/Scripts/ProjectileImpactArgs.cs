using System;
using UnityEngine;

public class ProjectileImpactArgs : EventArgs {
    public Projectile Sender { get; private set; }
    public Collision CollisionDetials { get; private set; }
    public ProjectileType Type { get; private set; }

    public ProjectileImpactArgs(Projectile sender, Collision collisionDetails, ProjectileType type) {
        Sender = sender;
        CollisionDetials = collisionDetails;
        Type = type;
    }
}