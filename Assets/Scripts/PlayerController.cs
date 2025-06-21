using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Rigidbody rb;
    public Vector3 InputKey;
    float Myfloat;

    public float pickupRange = 1.5f;
    public float moveSpeed;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        ////////////Move With WASD
        InputKey = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        InputKey.Normalize();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0); 
        }
    }

    void FixedUpdate()
    {

        rb.MovePosition((Vector3)transform.position + InputKey * 10 * Time.deltaTime);


        if (InputKey.magnitude >= 0.1f)
        {
            GetComponent<Animator>().SetFloat("Speed", 1);
            float Angle = Mathf.Atan2(InputKey.x, InputKey.z) * Mathf.Rad2Deg; //=========================================== LookAt
            float Smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, Angle, ref Myfloat, 0.1f); //=================== Smooth Rotation
            transform.rotation = Quaternion.Euler(0, Smooth, 0); //============================================================ Change Angle

        }
        else
        {

            GetComponent<Animator>().SetFloat("Speed", 0);
        }


    }

}
