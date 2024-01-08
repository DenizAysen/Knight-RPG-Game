using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRawImage : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed, verticalSpeed;

    RawImage _rawImage;
    Rect _currentUv;
    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();
        _currentUv = _rawImage.uvRect;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _currentUv.x -= Time.deltaTime * horizontalSpeed;
        _currentUv.y -= Time.deltaTime * verticalSpeed;

        if(_currentUv.x <= -1f || _currentUv.x >= 1f)
        {
            _currentUv.x = 0;
        }
        if (_currentUv.y <= -1f || _currentUv.y >= 1f)
        {
            _currentUv.y = 0;
        }

        _rawImage.uvRect = _currentUv;
    }
}
