using UnityEngine;

public class FXManager : MonoBehaviour {
    [SerializeField] ProjectileImpactEvent projectileImpactEvent;

    [field: SerializeField] public GameObject AssPlosionPrefab { get; private set; }

    private void OnEnable() { Subscribe(); }
    private void OnDisable() { UnSubscribe(); }

    private void Subscribe() {
        projectileImpactEvent.OnProjectileImpact += PlayImpactFX;
    }

    private void UnSubscribe() {
        projectileImpactEvent.OnProjectileImpact -= PlayImpactFX;
    }

    private void PlayImpactFX(ProjectileImpactArgs args) {
        if(args.Type == ProjectileType.Explosive) {
            var fx = GetFreeFXSource();
            fx.gameObject.transform.position = args.CollisionDetials.GetContact(0).point;
            fx.Play();
        }
    }

    private ParticleSystem GetFreeFXSource() {
        foreach(ParticleSystem fx in GetComponentsInChildren<ParticleSystem>(true)) {
            if(!fx.isPlaying) { return fx; }
        }

        var result = Instantiate(AssPlosionPrefab, transform).GetComponent<ParticleSystem>();
        return result;
    }
}