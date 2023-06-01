using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    private float _width;
    private float _height;
    private Vector3 _position;

    [Tooltip("Factor with how fast the camera moves when being dragged around")]
    [SerializeField] private float _dragFactor = 0.1f;

    private Vector2 _camLimits;
    private Transform _camTransform;

    private Vector3 mpS;
    private Vector3 mp;
    private Vector3 camPS;
    
    

    // Start is called before the first frame update
    void Awake()
    {
        _width = (float)Screen.width / 2.0f;
        _height = (float)Screen.height / 2.0f;
        _camLimits = new Vector2(65,39);
        _camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 pos = touch.position;
                pos.x = (pos.x - _width) / _width;
                pos.y = (pos.y - _height) / _height;
                _position = new Vector3(-pos.x, pos.y, 0.0f);
            }
            
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 pos = touch.position;
                pos.x = (pos.x - _width) / _width;
                pos.y = (pos.y - _height) / _height;
            }
        }*/
    
        if (Input.GetMouseButtonDown(0))
        {
            mpS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camPS = Camera.main.transform.position;
            Debug.Log("test");

        }
        if (Input.GetMouseButton(0))
        {
            Vector3 dif = mpS - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mpS);
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log(dif);
            Vector3 newPos = camPS + ( dif  * _dragFactor) ;
            _camTransform.position = new Vector3(Mathf.Max(-_camLimits.x, Mathf.Min(_camLimits.x, newPos.x)),
                Mathf.Max(-_camLimits.y, Mathf.Min(_camLimits.y, newPos.y)), _camTransform.position.z);
        }
    }
}
