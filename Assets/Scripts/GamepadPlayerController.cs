using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadPlayerController : MonoBehaviour
{
	// Start is called before the first frame update
	public float speed = 6.0F;
	public float gravity = 20.0F;

	private Vector3 moveDirection = Vector3.zero;
	public CharacterController controller;

	private Vector3 rotation;
	void Start()
	{
		// Store reference to attached component
		controller = GetComponent<CharacterController>();
	}


    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

          

            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        UpdateAnimator();
    }



    private void UpdateAnimator()
	{

		Vector3 localVelocity = transform.InverseTransformDirection(moveDirection);
		

		float speed = localVelocity.z * 4;
		GetComponent<Animator>().SetFloat("forwardSpeed", Mathf.Abs(speed));
	}
}
