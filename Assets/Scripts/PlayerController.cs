using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private TextMeshProUGUI startText;
    [SerializeField]
    private TextMeshProUGUI heldText;
    private float movementNerf;
    float startTime = 0;
    float held = 0;
    bool holding = false;

    void Start()
    {
    }

    void Update()
    {
        HandleMovementInput();
        HandleRotationInput();
        HandleDashInput();
    }

    void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.name == "Wall")  // or if(gameObject.CompareTag("YourWallTag"))
        //{
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0 , 0);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
       // }
    }

    void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);

        transform.Translate(movement * (movementSpeed - movementNerf) * Time.deltaTime, Space.World);
    }

    void HandleRotationInput()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startTime = Time.time;
            movementNerf = 5;
            startText.text = startTime.ToString();
            held = 0;
            holding = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (held >= 0.75)
            {
                //Dash
                Vector3 mousePosUV = Input.mousePosition.normalized;
                transform.position = Vector3.Lerp(transform.position, target.position, dashSpeed * Time.deltaTime);
            }
            startTime = 0;
            movementNerf = 0;
            holding = false;
        }

        if (holding)
        {
            held = Time.time - startTime;
            heldText.text = held.ToString();
        }
    }
}
