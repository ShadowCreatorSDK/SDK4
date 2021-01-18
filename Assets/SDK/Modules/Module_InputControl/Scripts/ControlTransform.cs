using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTransform : MonoBehaviour {
    [Range(0.1f,10f)]
    public float speed = 0.4f;
    Vector3 positionReset, eulerAnglesReset;

    void Awake() {
        positionReset = transform.position;
        eulerAnglesReset = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update() {

        if(Application.platform == RuntimePlatform.Android)
            return;
		
        if(Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        } else if(Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        
        if(Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        } else if(Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        if(Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * speed * 10);
            transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * speed * 10);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }

        if(Input.GetKey(KeyCode.Escape)) {
            transform.position = positionReset;
            transform.eulerAngles = eulerAnglesReset;
        }
    }
}
