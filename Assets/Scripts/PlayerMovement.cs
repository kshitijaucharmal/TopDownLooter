using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float speed = 500f;

    private Vector3 movement;
    private Vector3 mousePosition;
    private Camera cam;
    private Rigidbody rb;
    private float angle;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void GetInputs(){
        // Movement Input
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        movement.Normalize();

        // Mouse Input
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLenght;
        if (groundPlane.Raycast(cameraRay, out rayLenght))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
            transform.LookAt(pointToLook);
        }
    }

    // Update is called once per frame
    void Update() {
        GetInputs();
    }

    void FixedUpdate(){
        // Tight controls change velocity
        var vel = rb.velocity;
        vel = movement * speed * Time.fixedDeltaTime;
        vel.y = rb.velocity.y;
        rb.velocity = vel;

        rb.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
