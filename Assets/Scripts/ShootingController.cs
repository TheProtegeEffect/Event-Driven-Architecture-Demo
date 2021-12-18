using System;
using System.Collections;
using UnityEngine;

public class ShootingController : MonoBehaviour {
    [SerializeField] Transform firePosition;
    [SerializeField] GameObject prefab;
    [SerializeField] PhysicMaterial slippery, bouncy;
    [SerializeField] float randomDirDegree = 0f;
    [SerializeField] bool randomizeColor = true;

    public void FireSolidProjectile() {
        Projectile projectile = GetFreeProjectile();
        projectile.gameObject.GetComponent<Collider>().material = slippery;
        projectile.TypeOfProjectile = ProjectileType.Solid;
        FireProjectile(projectile.gameObject);
    }

    public void FireExplodingProjectile() {
        Projectile projectile = GetFreeProjectile();
        projectile.gameObject.GetComponent<Collider>().material = slippery;
        projectile.TypeOfProjectile = ProjectileType.Explosive;
        FireProjectile(projectile.gameObject);
    }

    public void FireBouncyProjectile() {
        Projectile projectile = GetFreeProjectile();
        projectile.gameObject.GetComponent<Collider>().material = bouncy;
        projectile.TypeOfProjectile = ProjectileType.Bouncy;
        FireProjectile(projectile.gameObject);
    }

    private void FireProjectile(GameObject projectilePrefab) {
        projectilePrefab.SetActive(true);

        Projectile projectile = projectilePrefab.GetComponent<Projectile>();
        Vector3 dir = firePosition.forward;
        dir.x = UnityEngine.Random.Range(-randomDirDegree, randomDirDegree);
        dir.z = UnityEngine.Random.Range(-randomDirDegree, randomDirDegree);

        projectilePrefab.transform.position = firePosition.position;

        if(randomizeColor) {
            projectile.ProjectileRenderer.material.color = RandColor;
        }

        projectile.ProjectileRigidBody.AddForce(dir.normalized * projectile.ShootForce);
    }

    private IEnumerator AutoFire(float delay, Action fireType) {
        var pause = new WaitForSeconds(delay);
        while(true) {
            fireType?.Invoke();
            yield return pause;
        }
    }

    private Projectile GetFreeProjectile() {
        foreach(Projectile projectile in GetComponentsInChildren<Projectile>(true)) {
            if(!projectile.gameObject.activeSelf) { return projectile; }
        }

        var result = Instantiate(prefab, transform).GetComponent<Projectile>();
        return result;
    }

    private Color RandColor => new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
}