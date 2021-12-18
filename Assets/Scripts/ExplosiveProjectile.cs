using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(AudioSource))]
public class ExplosiveProjectile : Projectile {
    [SerializeField] AudioClip[] impactSounds;
    [field: SerializeField] public bool PlayOnImpact { get; set; } = true;

    private ParticleSystem fx;
    private AudioSource speaker;

    private void Awake() {
        fx = GetComponent<ParticleSystem>();
        speaker = GetComponent<AudioSource>();
    }

    protected override void OnCollisionEnter(Collision collision) {
        if(PlayOnImpact) {
            ProjectileRigidBody.isKinematic = true;
            ProjectileRenderer.enabled = false;
            fx.Play();
            speaker.clip = impactSounds[Random.Range(0, impactSounds.Length)];
            speaker.Play();
            StartCoroutine(ReturnOnFXComplete(fx, collision));
        }
    }

    IEnumerator ReturnOnFXComplete(ParticleSystem fx, Collision collision) {
        while(fx.isPlaying) {
            yield return null;
        }
        base.OnCollisionEnter(collision);
    }
}
