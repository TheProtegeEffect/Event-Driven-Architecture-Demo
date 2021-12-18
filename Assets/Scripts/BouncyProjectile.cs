using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BouncyProjectile : Projectile {
    [SerializeField] AudioClip[] impactSounds;
    [field: SerializeField] public bool PlayOnImpact { get; set; } = true;

    private AudioSource speaker;
    float highestMagnitude;

    private void Awake() {
        speaker = GetComponent<AudioSource>();
        speaker.clip = impactSounds[Random.Range(0, impactSounds.Length)];
    }

    private void Update() {
        if(ProjectileRigidBody.velocity.magnitude > highestMagnitude) {
            highestMagnitude = ProjectileRigidBody.velocity.magnitude;
        }
    }

    protected override void OnCollisionEnter(Collision collision) {
        if(PlayOnImpact) {
            speaker.volume = (1 / (highestMagnitude / ProjectileRigidBody.velocity.magnitude));
            speaker.Play();
        }
    }
}
