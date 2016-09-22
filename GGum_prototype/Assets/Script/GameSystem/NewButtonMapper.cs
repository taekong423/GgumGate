using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class NewButtonMapper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    bool _isDown = false;

    public EButtonCode _buttonCode;

    public bool _downMapping = false;

	// Use this for initialization
	void Start () {

        if (_downMapping)
            NewButtonManager.RegisterButtonDownFunc(_buttonCode, IsDown);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("MapperTest");
        }
    }

    void OnDestroy()
    {
        Debug.Log("MapperDestroy");
        NewButtonManager.RemoveButtonDwonFunc(_buttonCode, IsDown);
    }

    public bool IsDown()
    {
        bool isDown = _isDown;

        if (_isDown)
            _isDown = false;

         return isDown;

    }
    public void OnPointerDown(PointerEventData data)
    {
        _isDown = true;
        
        //_isPress = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        //_isPress = false;
    }

}
