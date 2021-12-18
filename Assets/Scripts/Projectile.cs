using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Projectile : MonoBehaviour {
    [field: SerializeField] public float ShootForce { get; private set; }
    [field: SerializeField] public Rigidbody ProjectileRigidBody { get; private set; }
    [field: SerializeField] public Renderer ProjectileRenderer { get; private set; }
    [field: SerializeField] public bool ReturnOnImpact { get; private set; }

    public GameObjectPool Pool { get; set; }

    private void Awake() {
        ProjectileRigidBody = GetComponent<Rigidbody>();
        ProjectileRenderer = GetComponent<Renderer>();
    }

    private void Start() {
        Physics.IgnoreLayerCollision(6, 7, true);
    }

    protected void OnEnable() {
        ProjectileRigidBody.isKinematic = false;
        ProjectileRenderer.enabled = true;
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if(ReturnOnImpact) {
            Pool.Return(this.gameObject);
        }
    }
}
