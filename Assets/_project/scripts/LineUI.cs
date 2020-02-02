using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LineUI : MonoBehaviour
{
	public Transform start, end;
	public Vector3 startOffset, endOffset;
	public float width;

    //void Start() { UpdateLine(); }

	void UpdateLine() {
		Vector3 s = startOffset, e = endOffset;
		if (start != null) { s += start.position; }
		if (end != null) { e += end.position; }
		Vector3 delta = e - s;
		Vector3 midpoint = s + delta / 2;
		float length = delta.magnitude;
		Vector3 dir = delta / length;
		dir.z = 0;
		RectTransform rt = GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(length, width);
		rt.position = midpoint;
		float angle = Vector2.Angle(Vector2.right, dir);
		if(delta.y < 0) { angle *= -1; }
		rt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public static LineUI Create(ref GameObject lineObject, Transform start, Transform end, Vector2 startOffset, Vector2 endOffset, Color color, float lineWidth) {
		if(lineObject == null) {
			lineObject = new GameObject("lineUI");
		}
		Image img = lineObject.GetComponent<Image>();
		if(img == null) {
			img = lineObject.AddComponent<Image>();
		}
		img.color = color;
		LineUI line = lineObject.GetComponent<LineUI>();
		if(line == null) {
			line = lineObject.AddComponent<LineUI>();
		}
		line.start = start;
		line.end = end;
		line.startOffset = startOffset;
		line.endOffset = endOffset;
		line.width = lineWidth;
		line.UpdateLine();
		return line;
	}

	//public float angle;
	//void FixedUpdate() { UpdateLine(); }
}
