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
    private float   horizontal;
    private float   vertical;
    private float rayRange = 5;
    public float speed = 10.0f;
    public float rotSpeed = 120.0f;
    // stats
    public int   wood = 0;
    public int   stone = 0;
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
        vertical   = Input.GetAxis("Vertical");

        if(vertical > 0){
            this.GetComponent<CharacterController>().SimpleMove( transform.forward * this.speed );
        } else if (vertical < 0 ) {
            this.GetComponent<CharacterController>().SimpleMove( -transform.forward * this.speed );
        }
        
        if(horizontal > 0 ){
            rotation += new Vector3(0, 1, 0) * Time.deltaTime * rotSpeed;
        } else if(horizontal < 0) {
            rotation -= new Vector3(0, 1, 0) * Time.deltaTime * rotSpeed;
        }
        this.GetComponent<Transform>().eulerAngles = rotation;

        castRay();
        // Let anyone end the game with the 'esc' Key in editor or in the build
        if(Input.GetKeyDown(KeyCode.Escape)){
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    } // end Update()

    void castRay(){
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, rayRange)){
            //print("hit:" + hitInfo.collider);
            // if the player wishes to interact with an object in front of them
            if(Input.GetKeyDown(KeyCode.E)){
                // check which object and increase the proper resource value
                if(hitInfo.collider.tag == "ROCK"){
                    this.stone++;
                    Destroy(hitInfo.collider.gameObject);
                }   
                if(hitInfo.collider.tag == "TREE"){
                    this.wood++;
                    Destroy(hitInfo.collider.gameObject);
                }  
            }
        }
    }
}
