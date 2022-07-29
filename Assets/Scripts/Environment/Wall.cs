using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{   
	public bool tileAboveIsWall;
    public Vector2 tileAbovePos;

    public Sprite wallDownSprite;

    public GameObject explodePrefab;
    public GameObject floorObj;
    public int health = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet") 
            || collision.gameObject.CompareTag("EnemyBullet"))
        {
            health--;
        }
    }


    private void FixedUpdate() {
        if (health <= 0) {
            if (tileAboveIsWall) {
                GameObject[] wallsAbove = GameObject.FindGameObjectsWithTag("Wall");
                
                foreach (GameObject wallAbove in wallsAbove)
                {   
                    if (wallAbove.transform.position.x == tileAbovePos.x && wallAbove.transform.position.y == tileAbovePos.y) {
                        wallAbove.GetComponent<SpriteRenderer>().sprite = wallDownSprite;
                        break;
                    }
                }
                
                
            }
            Destroy(gameObject);
            Instantiate(explodePrefab, transform.position, Quaternion.identity);
            Instantiate(floorObj, transform.position, Quaternion.identity);
        }
    }
}
