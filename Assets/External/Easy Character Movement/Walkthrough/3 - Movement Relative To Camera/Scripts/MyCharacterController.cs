using ECM.Common;
using ECM.Controllers;
using UnityEngine;

namespace ECM.Walkthrough.MovementRelativeToCamera
{
    /// <summary>
    /// Custom character controller. This shows how make the character move relative to MainCamera view direction.
    /// </summary>

    public class MyCharacterController : BaseCharacterController
    {
        [Tooltip("Dashing duration in seconds.")]
        public float dashDuration = 0.15f;

        [Tooltip("Dashing impulse, e.g. an instant velocity change while dashing.")]
        public float dashImpulse = 10.0f;

        private Animator anim;

        private bool _isDashing;
        private float _dashingTime;


        #region Dash
        /// <summary>
        /// Determines whether this character is dashing.
        /// </summary>
        public bool IsDashing()
        {
            return _isDashing;
        }

        /// <summary>
        /// Starts a dash.
        /// </summary>
        public void Dash()
        {
            if (IsDashing())
                return;

            _isDashing = true;
            _dashingTime = 0.0f;
        }

        /// <summary>
        /// Stops the character from dashing.
        /// </summary>

        public void StopDashing()
        {
            if (!IsDashing())
                return;

            _isDashing = false;
            _dashingTime = 0.0f;

            // Cancel dash momentum, if not grounded, preserve gravity

            if (isGrounded)
                movement.velocity = Vector3.zero;
            else
                movement.velocity = Vector3.Project(movement.velocity, transform.up);
        }

        /// <summary>
        /// Handle Dashing state.
        /// </summary>

        protected virtual void Dashing()
        {
            // Bypass acceleration, deceleration and friction while dashing

            movement.Move(moveDirection * dashImpulse, dashImpulse);

            // cancel any vertical velocity while dashing on air (e.g. Cancel gravity)

            if (!movement.isOnGround)
                movement.velocity = Vector3.ProjectOnPlane(movement.velocity, transform.up);

            // Update dash timer, if time completes, stops dashing

            _dashingTime += Time.deltaTime;

            if (_dashingTime > dashDuration)
                StopDashing();
        }

        /// <summary>
        /// Extends Move method to handle dashing state.
        /// </summary>

        protected override void Move()
        {
            if (IsDashing())
            {
                // Dashing state

                Dashing();
            }
            else
            {
                // Default state(s)

                base.Move();
            }
        }

        /// <summary>
        /// Handles the dashing input.
        /// </summary>

        protected virtual void HandleDashingInput()
        {
            // Starts a dash

            if (Input.GetKeyDown(KeyCode.LeftShift))
                Dash();

            if (IsDashing())
            {
                // If dashing, keep character's facing dash direction

                moveDirection = moveDirection;
            }
        }
        #endregion
        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }
        protected override void Animate()
        {
            Debug.Log(moveDirection.magnitude);
            //anim.SetFloat("Speed",moveDirection.magnitude);
            // Add animator related code here...
        }

        protected override void HandleInput()
        {
            // Toggle pause / resume.
            // By default, will restore character's velocity on resume (eg: restoreVelocityOnResume = true)

            if (Input.GetKeyDown(KeyCode.P))
                pause = !pause;

            // Handle user input

            //jump = Input.GetButton("Jump");

            //crouch = Input.GetKey(KeyCode.C);

            moveDirection = new Vector3
            {
                x = Input.GetAxis("Horizontal"),
                y = 0.0f,
                z = Input.GetAxis("Vertical")
            };

            HandleDashingInput();

            // Transform the given moveDirection to be relative to the main camera's view direction.
            // Here we use the included extension .relativeTo...

            var mainCamera = Camera.main;
            if (mainCamera != null)
                moveDirection = moveDirection.relativeTo(mainCamera.transform);

        }

        protected override void UpdateRotation()
        {
            //base.UpdateRotation();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up,Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                RotateTowards(point);
            }
        }
    }
}
