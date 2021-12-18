using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
    [SerializeField] ProjectileImpactEvent projectileImpactEvent;

    [field: SerializeField] public AudioClip[] Explosions { get; private set; }
    [field: SerializeField] public AudioClip[] Bounces { get; private set; }

    private void OnEnable() { Subscribe(); }
    private void OnDisable() { UnSubscribe(); }

    private void Subscribe() {
        projectileImpactEvent.OnProjectileImpact += PlayImpactSound;
    }

    private void UnSubscribe() {
        projectileImpactEvent.OnProjectileImpact -= PlayImpactSound;
    }

    private void PlayImpactSound(ProjectileImpactArgs args) {
        var speaker = GetFreeAudioSource();

        if(args.Type == ProjectileType.Explosive) {
            speaker.clip = Explosions[Random.Range(0, Explosions.Length)];
        }

        if(args.Type == ProjectileType.Bouncy) {
            speaker.clip = Bounces[Random.Range(0, Bounces.Length)];
        }

        speaker.Play();
    }

    private AudioSource GetFreeAudioSource() {
        foreach(AudioSource speaker in GetComponents<AudioSource>()) {
            if(!speaker.isPlaying) { return speaker; }
        }

        var result = this.gameObject.AddComponent<AudioSource>();
        return result;
    }
}
