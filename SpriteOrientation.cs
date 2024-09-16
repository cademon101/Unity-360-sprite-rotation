using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class SpriteOrientation : MonoBehaviour
{
    //[N][][Its also asking for an enemy manager]
    //AngleToPlayer
    public Transform camera_position;
    private Vector3 targetPos;
    private Vector3 targetDir;
    private Animator animator;
    bool isRotated = false;
    [SerializeField] bool has8DirectionalSprite = false;
    
    //private Sprite sprite;
    Vector3 spritePosition;
    Quaternion spriteRotation;
    Vector3 spriteScale;
    Transform spriteTransform;
    GameObject sprite;
    
    Vector3 orientationPosition;
    Quaternion orientationRotation;
    Vector3 orientationScale;
    Transform orientationTransform;
    GameObject orientation;

    Vector3 NormalScale; 
    //[Testing][]
    private SpriteRenderer spriteRenderer;

    private Transform playerPos;
    private Vector3 playerTarget;
    //[][]To-do
    public float angle;
    public int lastIndex;
    bool flipped = false;

    [Header("Camera Marker:")]
    /// If this is true, we'll rotate our model towards the direction
    [Tooltip("Uses the position of the camera as a point to always look at")]
    
    public GameObject camera_marker;
    GameObject player;
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Sprite")
            {
                Debug.Log(this.name + " connected " + child.gameObject.name);

                sprite = child.gameObject;
                spriteTransform = sprite.transform;
                spritePosition = spriteTransform.position;
                spriteRotation = spriteTransform.rotation;
                spriteScale = spriteTransform.localScale;
            }
            if (child.tag == "Orientation")
            {
                Debug.Log(this.name + " connected " + child.gameObject.name);

                orientation = child.gameObject;
                orientationTransform = orientation.transform;
                orientationPosition = orientationTransform.position;
                orientationRotation = orientationTransform.rotation;
                orientationScale = orientationTransform.localScale;
            }
        }
        camera_marker = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera_marker == null)
        {
            Debug.LogError("MainCamera not found");
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found");
            return;
        }
        camera_marker = GameObject.FindGameObjectWithTag("MainCamera");
        // Playermove?? 
        player = GameObject.FindGameObjectWithTag("Player");
        camera_position = camera_marker.transform;
        playerPos = player.transform;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        animator.SetFloat(name: "spriteOrientation", lastIndex);
        //Get positon and direction
        targetPos = new Vector3(camera_position.position.x, camera_position.position.y, camera_position.position.z);
        targetDir = targetPos - orientationTransform.position;

        //Our angle
        angle = Vector3.SignedAngle(from: targetDir, to: orientationTransform.forward, axis: Vector3.up);
        //just for testing sight of player [][X]

        if (!has8DirectionalSprite)
        {
            if (lastIndex >= 0 && lastIndex <= 4 && !isRotated)
            {
                sprite.transform.rotation = Quaternion.Euler(sprite.transform.rotation.eulerAngles.x, sprite.transform.rotation.eulerAngles.y + 180, sprite.transform.rotation.eulerAngles.z);
                isRotated = true;
            }
            else if (lastIndex >= 5 && lastIndex <= 7 && isRotated)
            {
                Debug.Log("I am flipping!");
                sprite.transform.rotation = Quaternion.Euler(sprite.transform.rotation.eulerAngles.x, sprite.transform.rotation.eulerAngles.y - 180, sprite.transform.rotation.eulerAngles.z);
                isRotated = false;
            }

        }

        lastIndex = GetIndex(angle);
    }

    private int GetIndex(float angle)
    {
        //front
        if (angle > -53f && angle < 53f)
            return 0;
        if (angle >= 53f && angle < 72f)
            return 7;
        if (angle >= 72f && angle < 109f)
            return 6;
        if (angle >= 109f && angle < 127f)
            return 5;


        //back
        if (angle <= -127 || angle >= 127f)
            return 4;
        if (angle >= -127f && angle < -109f)
            return 3;
        if (angle >= -109f && angle < -72f)
            return 2;
        if (angle >= -72f && angle <= -53f)
            return 1;

        return lastIndex;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(from:transform.position, direction:transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, targetPos);
        Gizmos.DrawLine(transform.position, playerTarget);
    }
}
