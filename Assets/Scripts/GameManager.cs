using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] roomGameObjects;
    public Room[] rooms = new Room[16];

    [SerializeField] private int startIndex;
    public int currentIndex;
    [SerializeField] private int endIndex;
    [SerializeField] private int mainPathLength;

    private int minPathLength = 8;
    private int maxPathLength = 16;

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
            rooms[i] = roomGameObjects[i].GetComponent<Room>();
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
            }
            if (i == mainPathLength - 1) endIndex = currentIndex;
        }
        player.transform.position = roomGameObjects[startIndex].transform.position;
        Camera.main.transform.position = roomGameObjects[startIndex].transform.position + new Vector3(0, 0, -10);
        Vector2 newBoundary = (new Vector2(19 * (startIndex % 4), 0)) +  new Vector2(0, -19) * (startIndex / 4);
        cameraFollow.setNewBoundary(newBoundary , newBoundary);
        currentIndex = startIndex;

        // Initialise the rest of the rooms
        for (int i = 0; i < 16; i++)
        {
            if (!rooms[i].isInitialised)
            {
                int rand;
                rand = Random.Range(0,5);
                if (rand == 4 && rooms[i].portalLeft != null)
                {
                    rooms[i].portalLeft.GetComponent<PortalTransition>().isActive = true;
                }
                rand = Random.Range(0, 5);
                if (rand == 4 && rooms[i].portalRight != null)
                {
                    rooms[i].portalRight.GetComponent<PortalTransition>().isActive = true;
                }
                rand = Random.Range(0, 5);
                if (rand == 4 && rooms[i].portalTop != null)
                {
                    rooms[i].portalTop.GetComponent<PortalTransition>().isActive = true;
                }
                rand = Random.Range(0, 5);
                if (rand == 4 && rooms[i].portalBottom != null)
                {
                    rooms[i].portalBottom.GetComponent<PortalTransition>().isActive = true;
                }
                rooms[i].isInitialised = true;
            }
        }

        // Trigger start Room event
        rooms[startIndex].isEntered = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRoomEnter(int roomIndex)
    {
        if (rooms[roomIndex].isEntered)
        {

            if (rooms[roomIndex].portalLeft != null)
            {
                rooms[roomIndex].portalLeft.GetComponent<PortalTransition>().isActive = true;
            }
            if (rooms[roomIndex].portalRight != null)
            {
                rooms[roomIndex].portalRight.GetComponent<PortalTransition>().isActive = true;
            }
            if (rooms[roomIndex].portalBottom != null)
            {
                rooms[roomIndex].portalBottom.GetComponent<PortalTransition>().isActive = true;
            }
            if (rooms[roomIndex].portalTop != null)
            {
                rooms[roomIndex].portalTop.GetComponent<PortalTransition>().isActive = true;
            }
        }
        else
        {
            if (rooms[roomIndex].portalLeft != null)
            {
                rooms[roomIndex].portalLeft.GetComponent<PortalTransition>().isActive = false;
            }
            if (rooms[roomIndex].portalRight != null)
            {
                rooms[roomIndex].portalRight.GetComponent<PortalTransition>().isActive = false;
            }
            if (rooms[roomIndex].portalBottom != null)
            {
                rooms[roomIndex].portalBottom.GetComponent<PortalTransition>().isActive = false;
            }
            if (rooms[roomIndex].portalTop != null)
            {
                rooms[roomIndex].portalTop.GetComponent<PortalTransition>().isActive = false;
            }
        }
    }
}
