using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonToggle : MonoBehaviour
{
    Vector3 thirdPersonPos = new Vector3(0.8f, 1.5f, -2.18f);
    Vector3 firstPersonPos = new Vector3(0f, 1.2f, .08f);
    bool isFirstPerson = false;

    void Start()
    {
        transform.localPosition = thirdPersonPos;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFirstPerson = !isFirstPerson;
            transform.localPosition = isFirstPerson ? firstPersonPos : thirdPersonPos;
        }
    }
}
