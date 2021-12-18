using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectPool {
    const int Default_Init_Size = 100;

    GameObject prefab;
    Transform container;

    Dictionary<GameObject, bool> pool;

    public Action<GameObject> OnGet;
    public Action<GameObject> OnReturn;
    public Action<GameObject> OnInit;

    public GameObjectPool(GameObject prefab,
        int initPoolSize = Default_Init_Size,
        Transform container = null,
        Action<GameObject> onGet = null,
        Action<GameObject> onReturn = null,
        Action<GameObject> onInit = null) {

        this.prefab = prefab;

        if(container == null) {
            container = new GameObject($"{prefab.name} Pool").transform;
        }

        this.container = container;
        pool = new Dictionary<GameObject, bool>();

        OnGet += onGet;
        OnReturn += onReturn;
        OnInit += onInit;

        InitPool(initPoolSize);
    }

    private void InitPool(int initPoolSize) {
        GameObject goToAdd = null;

        for(int i = 0; i < initPoolSize; i++) {
            goToAdd = AddToPool();
        }
    }

    private GameObject AddToPool() {
        GameObject goToAdd = GameObject.Instantiate(prefab);
        goToAdd.transform.SetParent(container);
        goToAdd.GetComponent<Projectile>().Pool = this;
        goToAdd.SetActive(false);
        pool.Add(goToAdd, true);
        OnInit?.Invoke(goToAdd);
        return goToAdd;
    }

    public GameObject Get(bool enableOnGet = false) {
        GameObject result = null;

        foreach(KeyValuePair<GameObject, bool> go in pool) {
            if(go.Value) { result = go.Key; }
        }

        if(result == null) {
            result = AddToPool();
        }

        pool[result] = false;
        if(enableOnGet) { result.gameObject.SetActive(true); }

        OnGet?.Invoke(result);
        return result;
    }

    public void Return(GameObject go, bool disableOnReturn = true) {
        if(pool.ContainsKey(go)) {
            pool[go] = false;
            if(disableOnReturn) { go.SetActive(false); }
            OnReturn?.Invoke(go);
        }
    }
}
