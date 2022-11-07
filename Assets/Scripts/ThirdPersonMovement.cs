using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private MovementCharacteristics _characteristics;

    private float vertical;

    private readonly string STR_VERTICAL = "Vertical";

    private const float DISTANC_CAMERA_OFFSET = 5f;

    private CharacterController controller;

    private Animator animator;

    private Vector3 direction;
    private Quaternion look;

    private Vector3 TargetRotate => _camera.forward * DISTANC_CAMERA_OFFSET;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        Cursor.visible = _characteristics.VisibleCursor;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotate();
        Animation();
    }

    private void Movement()
    {
        if (controller.isGrounded)
        {
            vertical = Input.GetAxis(STR_VERTICAL);

            direction = transform.TransformDirection(0f, 0f, vertical);
        }

        direction.y -= _characteristics.Gravity * Time.deltaTime;
        Vector3 dir = direction * _characteristics.MovementSpeed * Time.deltaTime;

        controller.Move(dir);

    }

    private void Rotate()
    {
        Vector3 target = TargetRotate;
        target.y = 0f;

        look = Quaternion.LookRotation(target);

        float speed = _characteristics.AngleSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, look, speed);

    }

    private void Animation()
    {
        if (Input.GetAxis(STR_VERTICAL) != 0)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }
}
