using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFaceMouse : MonoBehaviour
{
	public Camera cam;
	public CharacterMove body;
	public SingleJoystick faceJoystick;

	/// called when created by Unity Editor
	void Reset()
	{
		if(cam == null)
		{
			cam = Camera.main;
		}
	}

	void Start() { }

	void AlignBodyWithFace()
	{
		Vector3 delta = lookingAt - body.transform.position;
		Vector3 right = Vector3.Cross(Vector3.up, delta);
		if(right == Vector3.zero) { right = Vector3.Cross(cam.transform.up, delta); } // prevent bug when looking directly up or down
		Vector3 dirAlongHorizon = Vector3.Cross(right, Vector3.up).normalized;
		body.transform.rotation = Quaternion.LookRotation(dirAlongHorizon, Vector3.up);
	}

	[HideInInspector] public RaycastHit raycastHit;
	[HideInInspector] public Ray ray;
	[HideInInspector] public Vector3 lookingAt;

	public void LookAtMouse(Vector3 screenCoordinate)
	{
		ray = cam.ScreenPointToRay(screenCoordinate);
		if (Physics.Raycast(ray, out raycastHit)) {
			lookingAt = raycastHit.point;
		} else {
			lookingAt = ray.origin + ray.direction;
		}
		Look();
	}

	/// sets lookingAt and adjusts direction of transform, does not adjust ray or raycastHit
	public void ForceLookAt(Vector3 position)
	{
		lookingAt = position;
		Look();
	}

	void Look()
	{
		if (body != null && !body.move.lookForwardMoving) { AlignBodyWithFace(); }
		transform.LookAt(lookingAt, Vector3.up);
	}

	void TurnAccordingToJoystick(Vector3 joystickFace)
	{
		Vector3 f = (cam != null) ? cam.transform.forward : transform.forward;
		Vector3 up = Vector3.up;
		Vector3 right = Vector3.Cross(up, f);
		if (right == Vector3.zero) { right = Vector3.Cross(cam.transform.up, f); } // prevent bug when looking directly up or down
		right.Normalize();
		if (cam != null && Vector3.Dot(cam.transform.up, Vector3.up) < 0) { right *= -1; } // handle upside-down turning
		Vector3 dirAlongHorizon = Vector3.Cross(right, up).normalized;
		Vector3 dir = joystickFace.x * right + joystickFace.y * dirAlongHorizon;
		lookingAt = transform.position + dir;
		transform.LookAt(lookingAt, up);
	}

	void FixedUpdate()
	{
		Vector3 joystickFace = (faceJoystick != null) ? faceJoystick.GetInputDirection() : Vector3.zero;
		if (joystickFace != Vector3.zero)
		{
			TurnAccordingToJoystick(joystickFace);
		} else
		{
			if(body == null || !body.move.lookForwardMoving || body.move.moveDirection == Vector3.zero)
			{
				LookAtMouse(Input.mousePosition);
			}
		}
	}
}
