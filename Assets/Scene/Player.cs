using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // movement vars
    private Vector3 rotation;
    private Vector3 direction;
    private float horizontal;
    private float vertical;
    private float rayRange = 5;
    public float speed = 10.0f;
    public float rotSpeed = 120.0f;
    // stats
    public int wood = 0;
    public int stone = 0;
    public int depositWood = 0;
    public int depositStone = 0;

    //interal script objects
    private Collider lastSeenObject; //used to clear the outline of the last seen object by the player

    // Start is called before the first frame update
    void Start()
    {
        rotation = new Vector3(0, 0, 0);
        horizontal = Input.GetAxis("Horizontal");
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (vertical > 0)
        {
            this.GetComponent<CharacterController>().SimpleMove(transform.forward * this.speed);
        }
        else if (vertical < 0)
        {
            this.GetComponent<CharacterController>().SimpleMove(-transform.forward * this.speed);
        }

        if (horizontal > 0)
        {
            rotation += new Vector3(0, 1, 0) * Time.deltaTime * rotSpeed;
        }
        else if (horizontal < 0)
        {
            rotation -= new Vector3(0, 1, 0) * Time.deltaTime * rotSpeed;
        }
        this.GetComponent<Transform>().eulerAngles = rotation;

        //print("wood:" + this.wood);
        //print("stone:" + this.stone);
        //print("depositWood:" + this.depositWood);
        //print("depositStone:" + this.depositStone);

        castRay();
        // Let anyone end the game with the 'esc' Key in editor or in the build
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    } // end Update()

    //Player interaction method. Uses raycast to detect collision with objects and allows player to interact with it.
    void castRay()
    {
        RaycastHit hitInfo = new RaycastHit();
        // if the player is currently looking at and within range to interact with an object 
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, rayRange))
        {
            //print("hit:" + hitInfo.collider);

            lastSeenObject = hitInfo.collider; //save collider to remove highlight when no longer being looked at

            //grab the outline component to highlight the object they are looking at
            var outline = hitInfo.collider.GetComponent<Outline>();
            if (outline != null)
            {        //if there's no outline component on it, nothing happens.
                outline.enabled = true; //indicates visually to the player they can interact with this object!
            }
            // if the player wishes to interact with an object in front of them
            if (Input.GetKeyDown(KeyCode.E))
            {
                // check which object and increase the proper resource value
                if (hitInfo.collider.tag == "ROCK")
                {
                    this.stone++;
                    Destroy(hitInfo.collider.gameObject);
                    StonePlayer.stoneValue += 10;
                }
                if (hitInfo.collider.tag == "TREE")
                {
                    this.wood++;
                    Destroy(hitInfo.collider.gameObject);
                    WoodPlayer.woodValue += 10;
                }
                if (hitInfo.collider.tag == "DEPOSIT") //deposit resources in the cart
                {
                    this.depositWood += this.wood;
                    this.wood = 0;
                    this.depositStone += this.stone;
                    this.stone = 0;
                    WoodDeposit.woodValue += WoodPlayer.woodValue;
                    StoneDeposit.stoneValue += StonePlayer.stoneValue;
                    WoodPlayer.woodValue = 0;
                    StonePlayer.stoneValue = 0;
                }
            }
        }
        //check if the last seen object should no longer be highlighted
        else if (lastSeenObject != null)
        {
            print("lastSeenObject: " + lastSeenObject);
            var outline = lastSeenObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false; //remove the highlight on the previous object
            }
            lastSeenObject = null; //small optimization
        }
    }
}
