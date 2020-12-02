using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.PlayerControls
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _anim;

        [Header("Controller Settings")]
        [SerializeField] private float _speed = 2.0f;
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravity = -9.81f;

        [Header("Camera Settings")]
        [SerializeField] private float _cameraSensitivity = 2f;
        [SerializeField] private Camera _tpCamera;
        [SerializeField] private Camera _fpCamera;

        private CharacterController _controller;
        private Vector3 _velocity;
        private Vector3 _direction;
        

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;

            if (_controller == null)
            {
                Debug.LogError("Character Controller is null");
            }
        }

        void Update()
        {
            CalculateMovement();
            CameraController();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                _tpCamera.enabled = !_tpCamera.enabled;
                _fpCamera.enabled = !_fpCamera.enabled;
            }

            //float speed = Mathf.Abs(Input.GetAxis("Vertical"));
            //float rotation = Input.GetAxis("Horizontal");
            //float rotate = Mathf.Abs(rotation);

            //_anim.SetFloat("Turn", rotation);

            //if (Input.GetKey(KeyCode.W))
            //{
            //    _anim.SetFloat("Speed", speed);
            //}

            //if (Input.GetKey(KeyCode.S))
            //{
            //    _anim.SetBool("Move Back", true);
            //}
            //else
            //{
            //    _anim.SetBool("Walk Back", false);
            //}

            //if (Input.GetKey(KeyCode.A))
            //{
            //    _anim.SetBool("Turn Left", true);
            //}
            //else
            //{
            //    _anim.SetBool("Turn Left", false);
            //}

            //if (Input.GetKey(KeyCode.D))
            //{
            //    _anim.SetBool("Turn Right", true);
            //}
            //else
            //{
            //    _anim.SetBool("Turn Right", false);
            //}

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    _anim.SetTrigger("Jump");
            //}
        }

        private void CalculateMovement()
        {
            if (_controller.isGrounded == true)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                _direction = new Vector3(horizontal, 0, vertical);
                _velocity = _direction * _speed;

                //_anim.SetFloat("Speed", vertical);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _velocity.y = _jumpHeight;
                }
            }

            _velocity.y += _gravity * Time.deltaTime;
            _velocity = transform.TransformDirection(_velocity);
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void CameraController()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Look left and right
            Vector3 currentRotation = transform.localEulerAngles;
            currentRotation.y += mouseX * _cameraSensitivity;
            transform.localRotation = Quaternion.AngleAxis(currentRotation.y, Vector3.up);

            // Look up and down
            Vector3 tpCameraRotation = _tpCamera.gameObject.transform.localEulerAngles;
            tpCameraRotation.x -= mouseY * _cameraSensitivity;
            tpCameraRotation.x = Mathf.Clamp(tpCameraRotation.x, 0, 26);
            _tpCamera.gameObject.transform.localRotation = Quaternion.AngleAxis(tpCameraRotation.x, Vector3.right);

            Vector3 fpCameraRotation = _fpCamera.gameObject.transform.localEulerAngles;
            fpCameraRotation.x -= mouseY * _cameraSensitivity;
            fpCameraRotation.x = Mathf.Clamp(fpCameraRotation.x, 0, 26);
            _fpCamera.gameObject.transform.localRotation = Quaternion.AngleAxis(fpCameraRotation.x, Vector3.right);
        }
    }
}


