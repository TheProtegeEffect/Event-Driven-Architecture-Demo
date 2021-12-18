using System;
using System.Collections;
using UnityEngine;

namespace CoupledBad {
    public class ShootingController : MonoBehaviour {
        [SerializeField] Transform firePosition;
        [SerializeField] GameObject rollingProjectilePrefab;
        [SerializeField] GameObject explodingProjectilePrefab;
        [SerializeField] GameObject bouncyProjectilePrefab;
        [SerializeField] float randomDirDegree = 0f;
        [SerializeField] bool randomizeColor = true;

        private GameObjectPool rollingPool;
        private GameObjectPool explosionsPool;
        private GameObjectPool bouncyPool;

        private void Start() {
            rollingPool = new GameObjectPool(rollingProjectilePrefab, 20);
            explosionsPool = new GameObjectPool(explodingProjectilePrefab, 20);
            bouncyPool = new GameObjectPool(bouncyProjectilePrefab, 20);
        }

        public void FireRollingProjectile() {
            FireProjectile(rollingPool.Get());
        }

        public void FireExplodingProjectile() {
            FireProjectile(explosionsPool.Get());
        }

        public void FireBouncyProjectile() {
            FireProjectile(bouncyPool.Get());
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

        private Color RandColor => new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }
}