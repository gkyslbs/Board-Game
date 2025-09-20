using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;   
    public float speed = 1f;  
    Transform[] route;     
    int idx = 0;            
    public void SetRoute(Transform[] points){
        route = points;
        idx = 0;
    }
    public void TakeDamage(int amount = 1){
        health = Mathf.Max(health - amount, 0);
        if (health == 0) Destroy(gameObject);
    }
    public void HasarAl(int _ = 1) { TakeDamage(); }
    public void SetHP(int v) { health = Mathf.Max(v, 0); }

    void Update(){
        if (route == null || route.Length == 0) return;
        if (idx >= route.Length) return;

        var target = route[idx];
        if (target == null) { idx++; return; }

        Vector3 p = transform.position;
        Vector3 d = target.position - p;
        float step = speed * Time.deltaTime;

        if (d.sqrMagnitude <= step * step)
        {
            transform.position = target.position;
            idx++;
        }
        else{
            transform.position = p + d.normalized * step;
        }
    }
}
