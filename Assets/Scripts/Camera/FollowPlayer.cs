using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Vector3 heightOffset = new Vector3(0, 0, -10);
    [Range(0, 10)]
    [SerializeField] private float smoothFactor = 0.1f;


    [SerializeField] private float maxX = 10;
    [SerializeField] private float maxY = 10;
    [SerializeField] private float minX = -10;
    [SerializeField] private float minY = -10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = player.transform.position + heightOffset;
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothFactor);
    }

    public void setNewBoundary(Vector2 camMinChange, Vector2 camMaxChange)
    {
        maxX += camMaxChange.x;
        maxY += camMaxChange.y;
        minX += camMinChange.x;
        minY += camMinChange.y;
    }

    

}
