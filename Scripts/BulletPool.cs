using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool I;
    public GameObject bulletPrefab;
    public int initial = 16;
    readonly Queue<Bullet> q = new Queue<Bullet>();

    void Awake(){
        I = this;
        for (int i = 0;
        i < initial; i++) 
        AddOne();
    }
    Bullet AddOne(){
        var go = Instantiate(bulletPrefab);
        var b = go.GetComponent<Bullet>();
        b.pool = this;
        go.SetActive(false);
        q.Enqueue(b);
        return b;
    }
    public Bullet Get(Vector3 pos){
        if (q.Count == 0) AddOne();
        var b = q.Dequeue();
        b.transform.position = pos;
        b.gameObject.SetActive(true);
        return b;
    }

    public void Release(Bullet b){
        b.gameObject.SetActive(false);
        q.Enqueue(b);
    }
}
