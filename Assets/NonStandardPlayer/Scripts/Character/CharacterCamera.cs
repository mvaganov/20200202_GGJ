﻿using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
	[Tooltip("which transform to follow with the camera")]
	public Transform target;
	public float keypressRotateSpeed = 90;
	public SingleJoystick joystick;

	public void UpdateRotationWithUserInput(ref float rotH, ref float rotV, ref float zoom, System.Action onMouseLookStart, System.Action onMouseLookStop) {
		if (joystick != null && rotH == 0 && rotV == 0)
		{
			Vector3 inputDir = joystick.GetInputDirection();
			rotH = inputDir.x * keypressRotateSpeed * Time.deltaTime;
			rotV = inputDir.y * keypressRotateSpeed * Time.deltaTime;
		}
		if (Input.mouseScrollDelta.y != 0) { zoom -= Input.mouseScrollDelta.y; }
	}

	/// how far the camera wants to be from the target
	private float targetDistance = 10;
	/// keep track of rotation, so it can be un-rotated and cleanly re-rotated 
	private float pitch, yaw;

	/// publicly accessible variables that can be modified by external scripts or UI
	[HideInInspector] public float horizontalRotateInput, verticalRotateInput, zoomInput;
	public float HorizontalRotateInput { get { return horizontalRotateInput; } set { horizontalRotateInput = value; } }
	public float VerticalRotateInput { get { return verticalRotateInput; } set { verticalRotateInput = value; } }
	public float ZoomInput { get { return zoomInput; } set { zoomInput = value; } }

	private void Start()
	{
		if(target != null)
		{
			Vector3 delta = transform.position - target.position;
			targetDistance = delta.magnitude;
		}
		Vector3 right = Vector3.Cross(transform.forward, Vector3.up);
		Vector3 straightForward = Vector3.Cross(Vector3.up, right).normalized;
		pitch = Vector3.Angle(straightForward, transform.forward);
		yaw = Vector3.Angle(Vector3.forward, straightForward);
		if (Vector3.Dot(straightForward, Vector3.right) < 0) { yaw *= -1; }
		if (Vector3.Dot(Vector3.up, transform.forward) > 0) { pitch *= -1; }
	}

	void Update()
    {
		float rotH = horizontalRotateInput * Time.deltaTime, rotV = verticalRotateInput * Time.deltaTime, zoom = zoomInput * Time.deltaTime;
		// TODO toggle swaps between modes, toggle mouselook by default, toggle mouse selection by default
		UpdateRotationWithUserInput(ref rotH, ref rotV, ref zoom, ()=> {
			CharacterFaceMouse face = target.GetComponent<CharacterFaceMouse>();
			if (face) face.enabled = false;
		}, () => {
			CharacterFaceMouse face = target.GetComponent<CharacterFaceMouse>();
			if(face) face.enabled = true;
		});
		targetDistance += zoom;
		if (rotH != 0 || rotV != 0)
		{
			transform.rotation = Quaternion.identity;
			yaw += rotH;
			pitch -= rotV;
			if (yaw < -180) { yaw += 360; }
			if (yaw >= 180) { yaw -= 360; }
			if (pitch < -180) { pitch += 360; }
			if (pitch >= 180) { pitch -= 360; }
			transform.Rotate(pitch, yaw, 0);
		}
		if (targetDistance < 0) { targetDistance = 0; }
	}

	private void LateUpdate()
	{
		if(target != null)
		{
			transform.position = target.position - transform.forward * targetDistance;
		}
	}
}