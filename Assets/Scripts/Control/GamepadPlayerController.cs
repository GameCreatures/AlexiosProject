using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control
{
    public class GamepadPlayerController : MonoBehaviour
    {
        // Start is called before the first frame update

        [SerializeField] float _jumpHeight = 15f;
        [SerializeField] float _jumpSpeed = -20f;

        public float _MovementSpeed = 6.0F;
        public float _gravity = -98.1f;
        public float _turnSmoothTime = 0.1f;
        float _turnSmoothVelocity;


        private Vector3 _moveDirection = Vector3.zero;
        public CharacterController _gamePadController;

        private Vector3 _playerRotation;

        Fighter _Fighter;
        private CombatTarget _Target;

        private void Start()
        {
            _gamePadController = this.GetComponent<CharacterController>();
            _Fighter = this.GetComponent<Fighter>();
        }

        private void FixedUpdate()
        {
            InteractWithCombat();
            MoveWithGamepad();

            UpdateAnimator();
        }

        void OnTriggerEnter(Collider other)
        {
            print("Colliding");
            //Check for a match with the specified name on any GameObject that collides with your GameObject
            //if (collision.gameObject.name == "MyGameObjectName")
            //{
            //    //If the GameObject's name matches the one you suggest, output this message in the console
            //    Debug.Log("Do something here");
            //}

            //Check for a match with the specific tag on any GameObject that collides with your GameObject
            if (other.gameObject.tag == "Enemy")
            {
                //If the GameObject has the same tag as specified, output this message in the console
                _Target = other.gameObject.GetComponent<CombatTarget>();
            }
        }

        void OnTriggerExit(Collider other)
        {
            print("No longer in contact with " + other.transform.name);
            _Target = null;
        }
        private void InteractWithCombat()
        {
            if (_Target)
            {
                if (Input.GetButtonDown("Fire3"))
                {
                    _Fighter.Attack(_Target);
                }
            }
        }

        private void MoveWithGamepad()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            _moveDirection = new Vector3(horizontal, _moveDirection.y, vertical).normalized;

            if (_moveDirection.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                if (PlayerJumped)
                {
                    //_moveDirection.y += Mathf.Sqrt(_jumpHeight *  _jumpSpeed *_gravity * Time.deltaTime);
                    _moveDirection.y = _jumpHeight;
                }
                else if (_gamePadController.isGrounded)
                {
                    _moveDirection.y = 0f;
                }
                else
                {
                    _moveDirection.y = _gravity * Time.deltaTime;
                }
                _gamePadController.Move(_moveDirection * _MovementSpeed * Time.deltaTime);

            }
        }

        private bool PlayerJumped => _gamePadController.isGrounded && Input.GetButtonDown("Fire2");

        private void UpdateAnimator()
        {

            Vector3 localVelocity = transform.InverseTransformDirection(_moveDirection);

            //print(localVelocity.ToString());
            float speed = localVelocity.z * 4;
            GetComponent<Animator>().SetFloat("forwardSpeed", Mathf.Abs(speed));

        }
    }
} 