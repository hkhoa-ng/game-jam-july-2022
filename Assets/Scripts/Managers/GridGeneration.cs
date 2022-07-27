using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    public GameObject[] grid;
    private Room[] rooms = new Room[16];

    private int startIndex;
    private int currentIndex;
    private int endIndex;
    private int mainPathLength;

    private int minPathLength = 5;
    private int maxPathLength = 10;
    
    // 0: Left, 1: Right, 2: Up, 3: Down 
    private int direction;
    
    // Start is called before the first frame update
    void Start()
    {
        startIndex = Random.Range(0, 16);
        currentIndex = startIndex;
        mainPathLength = Random.Range(minPathLength, maxPathLength + 1);
        for (int i = 0; i < 16; i++) 
        {
            rooms[i] = grid[i].GetComponent<Room>();
        }

        for (int i = 0; i < mainPathLength; i++)
        {
            direction = Random.Range(0, 4);
            if (direction == 0 && rooms[currentIndex].portalLeft != null)
            {

            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
