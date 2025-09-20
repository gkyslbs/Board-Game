using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPath : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] defaultCells;     
    public Transform[] clickableCells;
    //I gave names to the cells and arranged their paths like this
    public int count = 3;                
    public float interval = 1f;         
    public LayerMask enemyMask;
    public float spawnClearRadius = 0.6f;
    [HideInInspector] public bool done = false; 
    void Start(){
        done = false;
        StartCoroutine(SpawnCo());
    }
    IEnumerator SpawnCo(){
        if (!enemyPrefab) { done = true; yield break; }

        var route = BuildRoute();
        if (route.Count == 0) { done = true; yield break; }

        var points = route.ToArray();

        for (int i = 0; i < count; i++){
            while (Physics2D.OverlapCircle(transform.position, spawnClearRadius, enemyMask))

                yield return null;

            var go = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            var e = go.GetComponent<Enemy>();
            if (e) e.SetRoute(points);

            yield return new WaitForSeconds(interval);
        }
        done = true;
    }
    List<Transform> BuildRoute(){
        var r = new List<Transform>();
        if (defaultCells != null) 
        foreach (var t in defaultCells) if (t) r.Add(t);
        //FOR LISTS!!
        if (clickableCells != null) 
        foreach (var t in clickableCells) if (t) r.Add(t);
        return r;
    }
}
