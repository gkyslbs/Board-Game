using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;          
    public BulletPool pool;
    Transform target;
    public void Init(Transform t) { target = t; }

    void Update(){
        if (!target) { Despawn(); return; }

        Vector3 p = transform.position;
        Vector3 to = target.position - p;
        float step = speed * Time.deltaTime;

        Vector3 dir = to.normalized;
        transform.right = dir;

        if (to.sqrMagnitude <= step * step){
            var e = target.GetComponent<Enemy>();
            if (e) e.TakeDamage(damage);
            Despawn();
            return;}

        transform.position = p + dir * step;
    }

    void Despawn(){
        if (pool != null) pool.Release(this);
        else if (BulletPool.I != null) BulletPool.I.Release(this);
        else Destroy(gameObject);
    }
}

