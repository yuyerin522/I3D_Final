using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float moveSpeed = 5.0f;                   // 이동 속도
    public float mouseSensitivity = 200.0f;          // 마우스 감도
    public Transform cameraTransform;                // 카메라의 Transform
    private CharacterController characterController; // CharacterController

    private float xRotation = -7.0f;                  // 카메라 수직 회전값
    private bool goingUp = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        xRotation = cameraTransform.localRotation.eulerAngles.x;         // 카메라 초기 수직 회전값
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        // WASD로 이동
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        characterController.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) //점프
        {
            goingUp = true;
        }

        if (goingUp)
        {
            if (transform.position.y < 4f)
            {
                characterController.Move(Vector3.up * Time.deltaTime * 5.0f);
            }
            else
            {
                goingUp = false;
            }
        }
        else
        {
            characterController.Move(Vector3.down * Time.deltaTime * 2.5f);
        }
    }
}
