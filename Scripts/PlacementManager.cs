using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PlacementManager : MonoBehaviour
{
    public Camera cam;

    public GameObject defence1Prefab; 
    public GameObject defence2Prefab; 
    public GameObject defence3Prefab; 
    public int defence1Count = 3;
    public int defence2Count = 2;
    public int defence3Count = 1;

    public TextMeshProUGUI defence1Text, defence2Text, defence3Text;

    public Button defence1Btn, defence2Btn, defence3Btn;
    public AudioClip placeSfx;
    enum Pick { None, Defence1, Defence2, Defence3 }
    Pick selected = Pick.None;

    HashSet<Collider2D> occupied = new HashSet<Collider2D>();

    void Start(){
        if (!cam) cam = Camera.main;
        RefreshTexts();
        RefreshUI();
    }

    void Update()
    {
        if (!TryGetPointerDown(out Vector2 wp) || selected == Pick.None) return;

        var hit = Physics2D.OverlapPoint(wp);
        if (hit && hit.CompareTag("Placeable") && !occupied.Contains(hit) && HasBudget(selected))
        {
            var pf = GetPrefab(selected);
            if (!pf) return;
            Instantiate(pf, hit.bounds.center, Quaternion.identity);
            occupied.Add(hit);

            Decrement(selected);
            RefreshTexts();


            if (placeSfx) AudioSource.PlayClipAtPoint(placeSfx, Camera.main ? Camera.main.transform.position : hit.bounds.center, 1f);

            if (!HasBudget(selected)) selected = Pick.None;
            RefreshUI();
        }
    }
    public void SelectDefence1() { if (defence1Count > 0) selected = Pick.Defence1; 
        RefreshUI(); }
    public void SelectDefence2() { if (defence2Count > 0) selected = Pick.Defence2; 
        RefreshUI(); }
    public void SelectDefence3() { if (defence3Count > 0) selected = Pick.Defence3; 
        RefreshUI(); }
    bool TryGetPointerDown(out Vector2 worldPos){
        if (!cam) cam = Camera.main;

        if (Input.GetMouseButtonDown(0))
        { worldPos = cam.ScreenToWorldPoint(Input.mousePosition); return true; }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        { worldPos = cam.ScreenToWorldPoint(Input.GetTouch(0).position); return true; }

        worldPos = default; return false;
    }

    bool HasBudget(Pick p) =>
        p == Pick.Defence1 ? defence1Count > 0 :
        p == Pick.Defence2 ? defence2Count > 0 :
        p == Pick.Defence3 ? defence3Count > 0 : 
        false;

    void Decrement(Pick p){
        if (p == Pick.Defence1) defence1Count = Mathf.Max(0, defence1Count - 1);
        else if (p == Pick.Defence2) defence2Count = Mathf.Max(0, defence2Count - 1);
        else if (p == Pick.Defence3) defence3Count = Mathf.Max(0, defence3Count - 1);
    }

    GameObject GetPrefab(Pick p) =>
        p == Pick.Defence1 ? defence1Prefab :
        p == Pick.Defence2 ? defence2Prefab :
        p == Pick.Defence3 ? defence3Prefab : null;

    void RefreshTexts(){
        if (defence1Text) defence1Text.text = $"x{defence1Count}";
        if (defence2Text) defence2Text.text = $"x{defence2Count}";
        if (defence3Text) defence3Text.text = $"x{defence3Count}";
    }

    void RefreshUI()
    {
        if (defence1Btn)
        {
            defence1Btn.interactable = defence1Count > 0;
            defence1Btn.transform.localScale = (selected == Pick.Defence1) ? Vector3.one * 1.15f : Vector3.one;
        }
        if (defence2Btn)
        {
            defence2Btn.interactable = defence2Count > 0;
            defence2Btn.transform.localScale = (selected == Pick.Defence2) ? Vector3.one * 1.15f : Vector3.one;
        }
        if (defence3Btn)
        {
            defence3Btn.interactable = defence3Count > 0;
            defence3Btn.transform.localScale = (selected == Pick.Defence3) ? Vector3.one * 1.15f : Vector3.one;
        }
    }
}
