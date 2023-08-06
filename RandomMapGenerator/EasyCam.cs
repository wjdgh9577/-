using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyCam : MonoBehaviour
{
    public float speed;
    public float rotateSpeed = 5f;

    public float cameraRotationLimit = 80f;

    float rotx, roty;
    float prex = 0;
    float prey = 0;
    /*
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(transform.forward.x, 0, transform.forward.z).normalized * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(transform.forward.x, 0, transform.forward.z) * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed;
        }
        if (Input.GetKey(KeyCode.R))
        {
            transform.position += Vector3.up * speed;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            transform.position -= Vector3.up * speed;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        this.rotx = Input.GetAxis("Mouse X") * this.rotateSpeed;
        this.roty = Input.GetAxis("Mouse Y") * this.rotateSpeed;
        transform.localRotation *= Quaternion.Euler(this.prey, 0, 0);
        transform.localRotation *= Quaternion.Euler(0, -this.prex, 0);

        this.prex += this.rotx;
        this.prey = Mathf.Clamp(this.prey + this.roty, -this.cameraRotationLimit, this.cameraRotationLimit);

        transform.localRotation *= Quaternion.Euler(0, this.prex, 0);
        transform.localRotation *= Quaternion.Euler(-this.prey, 0, 0);
    }*/
}
