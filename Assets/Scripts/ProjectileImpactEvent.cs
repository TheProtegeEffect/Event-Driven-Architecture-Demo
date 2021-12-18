using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileImpactEvent", menuName = "Events/ProjectileImpactEvent")]
public class ProjectileImpactEvent : ScriptableObject {
    public Action<ProjectileImpactArgs> OnProjectileImpact;

    public void Raise(ProjectileImpactArgs args) {
        OnProjectileImpact?.Invoke(args);
    }
}