using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    public EnemyFollowsPlayer followPlayer;
    public EnemyMovesRandomly moveRandomly;
    public int timer = 3;
    // Start is called before the first frame update
    void Start()
    {
        followPlayer = GetComponent<EnemyFollowsPlayer>();
        moveRandomly = GetComponent<EnemyMovesRandomly>();
        followPlayer.enabled = true;
        moveRandomly.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            followPlayer.enabled = false;
            moveRandomly.enabled = true;
            moveRandomly.OnEnable();
            StartCoroutine(Wandering());
        }
    }

    IEnumerator Wandering()
    {
        yield return new WaitForSeconds(timer);
        followPlayer.enabled = true;
        moveRandomly.enabled = false;
    }
}
