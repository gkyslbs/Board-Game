using UnityEngine;

public class DefenderShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int damage = 3;  
    public float range = 4f; 
    public float interval = 3f; 
    public float bulletSpeed = 10f;
    public LayerMask enemyMask;
    public enum ShootDir { Forward, All }
    public ShootDir direction = ShootDir.Forward; 

    public AudioClip shootSfx; 
    float timer;
    void Update(){
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        Transform target = FindTarget();
        if (!target) return;
        Bullet b = null;
        if (BulletPool.I != null) b = BulletPool.I.Get(transform.position);
        else{
            var go = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
            b = go.GetComponent<Bullet>();
        }

        if (b){
            b.speed = bulletSpeed;
            b.damage = damage;     
            b.Init(target);
        }

        if (shootSfx)
            AudioSource.PlayClipAtPoint(shootSfx, Camera.main ? Camera.main.transform.position : transform.position, 1f);

        timer = interval;
    }

    Transform FindTarget(){
        var hits = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        Transform best = null; float bestScore = float.MaxValue;


        float myCol = Mathf.Round(transform.position.x);

        foreach (var h in hits){
            Vector2 diff = h.transform.position - transform.position;

            if (direction == ShootDir.Forward)
            {
                if (diff.y <= 0f) 
                    continue;

                if (Mathf.Round(h.transform.position.x) != myCol) 
                    continue; 
                if (diff.y < bestScore) { bestScore = diff.y; best = h.transform; }
            }
            else{
                float d = diff.sqrMagnitude;
                if (d < bestScore) { bestScore = d; best = h.transform; }
            }
        }
        return best;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
