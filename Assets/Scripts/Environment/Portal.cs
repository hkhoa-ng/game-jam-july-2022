using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool isCleared = false;
    public SpriteRenderer spriteRenderer;
    private BoxCollider2D _collider;

    void Awake() {
        _collider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        InvokeRepeating(nameof(CheckStageCleared), 0f, 1.5f);
    }
    void CheckStageCleared() {
        int enemiesRemain = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemiesRemain == 0) {
            isCleared = true;
            spriteRenderer.enabled = true;
            _collider.enabled = true;
            CancelInvoke();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.NextStage();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
