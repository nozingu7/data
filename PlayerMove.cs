using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Camera cam;
    public GameObject camTarget;
    public float moveSpeed = 10.0f;
    Rigidbody rb;
    Vector3 vec = new Vector3();
    float v, h;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraMoving();
        if (Input.GetKeyDown(KeyCode.I))
        {
            Manager.instance.inventory.SetActive(!Manager.instance.inventory.activeSelf);
        }
    }

    void Move()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        vec.Set(h, 0, v);
        vec = vec.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + vec);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
        }
    }

    void CameraMoving()
    {
        float dir = Vector3.Distance(transform.position, camTarget.transform.position);
        Vector3 dd = transform.position - camTarget.transform.position;
        dd.Normalize();
        if (dir > 5)
        {
            Vector3 pos = camTarget.transform.position;
            pos += dd * Time.deltaTime * 5.0f;
            camTarget.transform.position = pos;
        }
    }
}
