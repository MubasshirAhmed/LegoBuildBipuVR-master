using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandMovementController : MonoBehaviour
{

    private float _horizontal;
    private float _vertical;
    private float _moveSpeed = 3.0f;
    private float _rotateSpeed = 30.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _vertical = Input.GetAxis("Vertical");
        _horizontal = Input.GetAxis("Horizontal");

        transform.Translate(_vertical * _moveSpeed * Vector3.right * Time.deltaTime);
        transform.Rotate(_horizontal * _rotateSpeed * transform.up * Time.deltaTime);
        if(Input.GetKey(KeyCode.O))
        {
            transform.Translate(_moveSpeed * transform.up * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.L))
        {
            transform.Translate((-_moveSpeed) * transform.up * Time.deltaTime);
        }
    }
}
