using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
	
	public enum AxisOption
	{
		// Options for which axes to use
		Both, // Use both
		OnlyHorizontal, // Only horizontal
		OnlyVertical // Only vertical
	}
	
	public int MovementRange = 100;
	public AxisOption axesToUse = AxisOption.Both;
	
	public float AxisX;
	public float AxisY;

    public Transform _center;

	Vector3 m_StartPos;
	bool m_UseX; // Toggle for using the x axis
	bool m_UseY; // Toggle for using the Y axis
	
	// Use this for initialization
	void Start () {
        ButtonManager.This.RegisterAxisRaw(EButtonCode.MoveX, GetAxisX);
        ButtonManager.This.RegisterAxisRaw(EButtonCode.MoveY, GetAxisY);
        m_StartPos = _center.position;
		CreateVirtualAxes();
	}

    public float GetAxisX()
    {
        if (AxisX != 0.0f)
        {
            return (AxisX / Math.Abs(AxisX));
        }
        else
            return 0.0f;
    }

    public float GetAxisY()
    {
        if (AxisY != 0.0f)
        {
            return (AxisY / Math.Abs(AxisY));
        }
        else
            return 0.0f;
    }

    private void UpdateVirtualAxes (Vector3 value) {
		
		var delta = m_StartPos - value;
		delta.y = -delta.y;
		delta /= MovementRange;
		if (m_UseX)
			AxisX = -delta.x;
		
		if (m_UseY)
			AxisY = delta.y;
		
	}
	
	void CreateVirtualAxes()
	{
		// set axes to use
		m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
		m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);
	}
	
	public void OnDrag(PointerEventData data)
	{
		Vector3 newPos = Vector3.zero;
		
		if (m_UseX) {
			int delta = (int)(data.position.x - m_StartPos.x);
			delta = Mathf.Clamp (delta, - MovementRange, MovementRange);
			newPos.x = delta;
		}
		
		if (m_UseY) {
			int delta = (int)(data.position.y - m_StartPos.y);
			delta = Mathf.Clamp (delta, -MovementRange, MovementRange);
			newPos.y = delta;
		}
        _center.position = new Vector3(m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z);
		UpdateVirtualAxes (_center.position);
	}
	
	public void OnPointerUp(PointerEventData data)
	{
        _center.position = m_StartPos;
		UpdateVirtualAxes(m_StartPos);
	}
	public void OnPointerDown(PointerEventData data)
    {
        Vector3 newPos = Vector3.zero;

        if (m_UseX)
        {
            int delta = (int)(data.position.x - m_StartPos.x);
            delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
            newPos.x = delta;
        }

        if (m_UseY)
        {
            int delta = (int)(data.position.y - m_StartPos.y);
            delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
            newPos.y = delta;
        }
        _center.position = new Vector3(m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z);
        UpdateVirtualAxes(_center.position);
    }
}
