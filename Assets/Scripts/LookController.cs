using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LookController : MonoBehaviour
{
    [SerializeField] Transform camTransform;
    [SerializeField] float sensitivity;
    bool gameEnded = false;
    float xRot = 0f;

    private void Start()
    {
        GameManager.Instance.OnGameEnd += GameEnded;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void GameEnded()
    {
        gameEnded = true;
    }
    void Update()
    {
        if (!gameEnded)
        {
            var mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);

            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);
            camTransform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }

    }
}
