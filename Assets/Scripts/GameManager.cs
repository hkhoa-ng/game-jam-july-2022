using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] grid;
    public Room[] rooms = new Room[16];

    [SerializeField] private int startIndex;
    [SerializeField]private int currentIndex;
    private int endIndex;
    [SerializeField]private int mainPathLength;

    private int minPathLength = 5;
    private int maxPathLength = 10;

    // 0: Left, 1: Right, 2: Up, 3: Down 
    private int direction;

    public GameObject player;
    private FollowPlayer cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = Camera.main.GetComponent<FollowPlayer>();

        startIndex = Random.Range(0, 16);
        currentIndex = startIndex;
        mainPathLength = Random.Range(minPathLength, maxPathLength + 1);


        for (int i = 0; i < 16; i++)
        {
            rooms[i] = grid[i].GetComponent<Room>();
        }

        // Initialise main path
        for (int i = 0; i < mainPathLength; i++)
        {
            direction = Random.Range(0, 4);
            if (direction == 0 && rooms[currentIndex].portalLeft != null && !rooms[currentIndex - 1].isInitialised)
            {
                rooms[currentIndex].portalLeft.GetComponent<PortalTransition>().isActive = true;
                rooms[currentIndex].isInitialised = true;
                currentIndex -= 1;
            }
            else if (direction == 1 && rooms[currentIndex].portalRight != null && !rooms[currentIndex + 1].isInitialised)
            {
                rooms[currentIndex].portalRight.GetComponent<PortalTransition>().isActive = true;
                rooms[currentIndex].isInitialised = true;
                currentIndex += 1;
            }
            else if (direction == 2 && rooms[currentIndex].portalTop != null && !rooms[currentIndex - 4].isInitialised)
            {
                rooms[currentIndex].portalTop.GetComponent<PortalTransition>().isActive = true;
                rooms[currentIndex].isInitialised = true;
                currentIndex -= 4;
            }
            else if (direction == 3 && rooms[currentIndex].portalBottom != null && !rooms[currentIndex + 4].isInitialised)
            {
                rooms[currentIndex].portalBottom.GetComponent<PortalTransition>().isActive = true;
                rooms[currentIndex].isInitialised = true;
                currentIndex += 4;
            }
            else
            {
                i--;
            }
        }
        player.transform.position = grid[startIndex].transform.position;
        Vector2 newBoundary = (new Vector2(19 * (startIndex % 4), 0)) +  new Vector2(0, -19) * (startIndex / 4);
        cameraFollow.setNewBoundary(newBoundary , newBoundary);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
