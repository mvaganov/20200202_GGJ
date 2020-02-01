// http://codegiraffe.com/unity/NonStandardPlayer.unitypackage
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof( Rigidbody ) )]
public class CharacterMove : MonoBehaviour {
    Rigidbody rb;
    public CharacterFaceMouse head;


    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if ( move.cameraOfPlayer != null && move.clickToMove.eventSystem == null ) {
            move.clickToMove.eventSystem = KeyCodeRoute.GetEventSystem();
        }
    }
    public float Jump { get; set; }
    public bool ClickToMove { get { return move.enableClickToMove; } set { move.enableClickToMove = value; } }
    public void ToggleClickToMove() { ClickToMove = !ClickToMove; }

    [System.Serializable]
    public struct ClickToMoveDetails {
        public bool isNPC;
        public UnityEngine.EventSystems.EventSystem eventSystem;
        [Tooltip("To enable click-to-move with left-click, make this value \"Mouse 0\"")]
        public KeyCode clickToMoveKey;
        [HideInInspector] public Vector3 targetPosition;
        /// callback for implementing AI pathing or path-finding
        public System.Action whatToDoWhenTargetIsReached;

        public void UpdateClickToMove( ref CharacterMoveControls move ) {
            if ( !isNPC ) {
                if ( ( clickToMoveKey != KeyCode.None && move.cameraOfPlayer != null && Input.GetKeyDown( clickToMoveKey ) && eventSystem.currentSelectedGameObject == null ) ) {
                    if ( move.cameraOfPlayer == null ) { Debug.LogError( "click-to-move only works with a designated camera" ); return; }
                    move.autoMoveToTargetPosition = true;
                    Ray ray = move.cameraOfPlayer.ScreenPointToRay(Input.mousePosition);
                    RaycastHit rh = new RaycastHit();
                    if ( Physics.Raycast( ray, out rh ) ) {
                        targetPosition = rh.point;
                    }
                }
            }
        }
    }

    [System.Serializable]
    public struct CharacterMoveControls {
        public float speed;
        public bool enableClickToMove;
        public bool canMoveInAir;
        public bool lookForwardMoving;
        [HideInInspector] public bool isStableOnGround;
        [HideInInspector] public bool autoMoveToTargetPosition; // set to false after the character reaches the target position
        [Tooltip("See [Edit]->[Project Settings]->[Input]->[Axes]")]
//		public string horizontalAxisName, verticalAxisName, jumpButtonName;
		public SingleJoystick moveJoystick;
        [HideInInspector] public float strafeRightMovement;
        [HideInInspector] public float moveForwardMovement;
        [HideInInspector] public float turnClockwise;

        [HideInInspector] public Vector3 moveDirection;
        [Tooltip("Set this to enable click-to-move")]
        public Camera cameraOfPlayer;
        public ClickToMoveDetails clickToMove;

        public void SetAutoMovePosition( Vector3 position ) {
            clickToMove.targetPosition = position;
            autoMoveToTargetPosition = true;
            enableClickToMove = true;
        }
        public void ClearAutoMove() {
            autoMoveToTargetPosition = false;
            enableClickToMove = false;
        }

        public void ApplyMoveFromInput( CharacterMove c ) {
            float h = strafeRightMovement;//!string.IsNullOrEmpty(horizontalAxisName) ? Input.GetAxis(horizontalAxisName) : 0;
            float v = moveForwardMovement;//!string.IsNullOrEmpty(verticalAxisName) ? Input.GetAxis(verticalAxisName) : 0;
            Vector3 joystickMove = (moveJoystick != null) ? moveJoystick.GetInputDirection() : Vector3.zero;
            if ( joystickMove != Vector3.zero ) { h = joystickMove.x; v = joystickMove.y; }
            //if (h == 0) { h = strafeRightMovement; }
            //if (v == 0) { v = moveForwardMovement; }
            Vector3 moveVelocity = Vector3.zero;
            float gravity = c.rb.velocity.y;
            moveDirection = Vector3.zero;
            // check if the character is receiving input
            if ( h != 0 || v != 0 ) {
                Vector3 lookForward = cameraOfPlayer ? cameraOfPlayer.transform.forward : c.transform.forward;
                Vector3 right = Vector3.Cross(Vector3.up, lookForward).normalized;
                if ( h != 0 ) {
                    moveVelocity += right * h * speed;
                }
                if ( v != 0 ) {
                    Vector3 forward = Vector3.Cross(right, Vector3.up).normalized;
                    moveVelocity += forward * v * speed;
                }
                if ( lookForwardMoving ) {
                    moveDirection = moveVelocity.normalized;
                }
            }
            moveVelocity.y = gravity;
            // disable click-to-move if character is receiving input
            if ( h != 0 || v != 0 ) { autoMoveToTargetPosition = false; }
            if ( autoMoveToTargetPosition ) {
                Vector3 delta = clickToMove.targetPosition - c.transform.position;
                delta.y = 0; // clamp to horizon
                float dist = delta.magnitude;
                if ( dist <= Time.deltaTime * speed ) {
                    //transform.position = targetPosition; // technically, this causes teleportation, so it's commented out.
                    autoMoveToTargetPosition = false;
                    moveVelocity = Vector3.zero;
                    if ( clickToMove.whatToDoWhenTargetIsReached != null ) {
                        clickToMove.whatToDoWhenTargetIsReached.Invoke();
                    }
                }
                else {
                    moveDirection = delta / dist;
                    Vector3 motion = moveDirection * speed;
                    moveVelocity.x = motion.x;
                    moveVelocity.z = motion.z;
                }
            }
            if ( lookForwardMoving && moveDirection != Vector3.zero && cameraOfPlayer != null ) {
                c.transform.rotation = Quaternion.LookRotation( moveDirection, Vector3.up );
                if ( c.head != null ) { c.head.transform.localRotation = Quaternion.identity; } // turn head straight while walking
            }
            c.rb.velocity = moveVelocity;
        }

        public void FixedUpdate( CharacterMove c ) {
            if ( isStableOnGround || canMoveInAir ) {
                ApplyMoveFromInput( c );
            }
        }
    }
    public CharacterMoveControls move = new CharacterMoveControls {
        speed = 5,
        lookForwardMoving = true,
        clickToMove = new ClickToMoveDetails { clickToMoveKey = KeyCode.Mouse0 },
		//horizontalAxisName = "Horizontal",
		//verticalAxisName = "Vertical",
		//jumpButtonName = "Jump"
	};

    private void Update() {
        if ( ClickToMove ) { move.clickToMove.UpdateClickToMove( ref move ); }
        jump.PressJump = Jump;
        //if (!string.IsNullOrEmpty(move.jumpButtonName)) {
        //	jump.PressJump = Input.GetButton(move.jumpButtonName) ? 1 : 0;
        //}
        //if(jump.PressJump == 0) {
        //	jump.PressJump = Jump;
        //}
    }

    void FixedUpdate() {
        move.FixedUpdate( this );
        jump.FixedUpdate( this );
        if ( jump.jumpIcon != null ) {
            jump.jumpIcon.fillAmount = move.isStableOnGround ? 1 : ( 1-( ( float ) jump.jumpsSoFar / jump.maxJumps ) );
        }
        move.isStableOnGround = false;
    }

    public bool IsStableOnGround() {
        return move.isStableOnGround;
    }

    /// anything steeper than this cannot be moved on
    private float maxStableAngle = 60;

    private void OnCollisionStay( Collision collision ) {
        // identify that the character is on the ground if it's colliding with something that is angled like ground
        for ( int i = 0; i < collision.contacts.Length; ++i ) {
            float a = Vector3.Angle(Vector3.up, collision.contacts[i].normal);
            if ( a <= maxStableAngle ) {
                move.isStableOnGround = true;
                break;
            }
        }
    }

    public Jumping jump = new Jumping();

    [System.Serializable]
    public class Jumping {
        public float minJumpHeight = 0.25f, maxJumpHeight = 2;
        [Tooltip("How long the jump button must be pressed to jump the maximum height")]
        public float fullJumpPressDuration = 0.25f;
        [Tooltip("for double-jumping, put a 2 here. To eliminate jumping, put a 0 here.")]
        public int maxJumps = 1;
        public UnityEngine.UI.Image jumpIcon;
        /// <summary>Whether or not the jumper wants to press jump (specifically, how many seconds of jump)
        /// <code>jump.PressJump = Input.GetButton("Jump") ? 1 : 0;</code></summary>
        [HideInInspector]
        public float PressJump;
        protected float currentJumpVelocity, heightReached, heightReachedTotal, timeHeld, targetHeight;
        protected bool impulseActive, inputHeld, peaked = false;
        public bool Peaked { get { return peaked; } }

        [Tooltip("if false, double jumps won't 'restart' a jump, just add jump velocity")]
        private bool jumpStartResetsVerticalMotion = true;
        public int jumpsSoFar { get; protected set; }
        /// <returns>if this instance is trying to jump</returns>
        public bool IsJumping { get { return inputHeld; } set { inputHeld = value; } }
        /// <summary>pretends to hold the jump button for the specified duration</summary>
        public void FixedUpdate( CharacterMove p ) {
            if ( inputHeld = ( PressJump > 0 ) ) { PressJump -= Time.deltaTime; }
            if ( impulseActive && !inputHeld ) { impulseActive = false; }
            if ( !inputHeld ) { return; }
            bool isStableOnGround = p.IsStableOnGround();
            // check stable footing for the jump
            if ( isStableOnGround ) {
                jumpsSoFar = 0;
                heightReached = 0;
                currentJumpVelocity = 0;
                timeHeld = 0;
            }
            // calculate the jump
            float gForce = -Physics.gravity.y * p.rb.mass;
            Vector3 jump_force = Vector3.zero, jumpDirection = Vector3.up;//-p.gravity.dir;
                                                                          // if the user wants to jump, and is allowed to jump again
            if ( !impulseActive && ( jumpsSoFar < maxJumps ) ) {
                heightReached = 0;
                timeHeld = 0;
                jumpsSoFar++;
                targetHeight = minJumpHeight * p.rb.mass;
                float velocityRequiredToJump = Mathf.Sqrt(targetHeight * 2 * gForce);
                // cancel out current jump/fall forces
                if ( jumpStartResetsVerticalMotion ) {
                    float motionInVerticalDirection = Vector3.Dot(jumpDirection, p.rb.velocity);
                    jump_force -= ( motionInVerticalDirection * jumpDirection ) / Time.deltaTime;
                }
                // apply proper jump force
                currentJumpVelocity = velocityRequiredToJump;
                peaked = false;
                jump_force += ( jumpDirection * currentJumpVelocity ) / Time.deltaTime;
                impulseActive = true;
            }
            else
                // if a jump is happening      
                if ( currentJumpVelocity > 0 ) {
                // handle jump height: the longer you hold jump, the higher you jump
                if ( inputHeld ) {
                    timeHeld += Time.deltaTime;
                    if ( timeHeld >= fullJumpPressDuration ) {
                        targetHeight = maxJumpHeight;
                        timeHeld = fullJumpPressDuration;
                    }
                    else {
                        targetHeight = minJumpHeight + ( ( maxJumpHeight - minJumpHeight ) * timeHeld / fullJumpPressDuration );
                        targetHeight *= p.rb.mass;
                    }
                    if ( heightReached < targetHeight ) {
                        float requiredJumpVelocity = Mathf.Sqrt((targetHeight - heightReached) * 2 * gForce);
                        float forceNeeded = requiredJumpVelocity - currentJumpVelocity;
                        jump_force += ( jumpDirection * forceNeeded ) / Time.deltaTime;
                        currentJumpVelocity = requiredJumpVelocity;
                    }
                }
            }
            else {
                impulseActive = false;
            }
            if ( currentJumpVelocity > 0 ) {
                float moved = currentJumpVelocity * Time.deltaTime;
                heightReached += moved;
                heightReachedTotal += moved;
                currentJumpVelocity -= gForce * Time.deltaTime;
            }
            else if ( !peaked && !isStableOnGround ) {
                peaked = true;
                impulseActive = false;
            }
            p.rb.AddForce( jump_force );
        }
    }
}
