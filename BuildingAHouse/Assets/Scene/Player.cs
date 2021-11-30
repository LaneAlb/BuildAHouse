using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotSpeed = 120.0f;
    public int   resources = 0;

    private Vector3 rotation;
    private Vector3 direction;
    private float   horizontal;
    private float   vertical;
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

        // Let anyone end the game with the 'esc' Key in editor or in the build
        if(Input.GetKeyDown(KeyCode.Escape)){
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
