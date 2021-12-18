using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Projectile : MonoBehaviour {
    [field: SerializeField] public float ShootForce { get; private set; }
    [field: SerializeField] public Rigidbody ProjectileRigidBody { get; protected set; }
    [field: SerializeField] public Renderer ProjectileRenderer { get; protected set; }
    [field: SerializeField] public ProjectileType TypeOfProjectile { get; set; } = ProjectileType.Solid;
    [field: SerializeField] public float LifeSpan { get; private set; } = 5f;

    [SerializeField] ProjectileImpactEvent onImpactEvent;

    private void Awake() {
        ProjectileRigidBody = GetComponent<Rigidbody>();
        ProjectileRenderer = GetComponent<Renderer>();
        Physics.IgnoreLayerCollision(6, 7, true);
    }

    public void OnEnable() {
        StartCoroutine(TimeOfDeath(LifeSpan));
    }

    protected virtual void OnCollisionEnter(Collision details) {
        var collisionDetails = new ProjectileImpactArgs(this, details, TypeOfProjectile);
        onImpactEvent?.Raise(collisionDetails);
        if(TypeOfProjectile == ProjectileType.Explosive) {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TimeOfDeath(float time) {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
