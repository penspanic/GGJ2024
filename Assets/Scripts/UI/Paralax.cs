using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField]
    float intensity = 1f;

    private Vector3 originalPos;
    void Start()
    {
        originalPos = transform.position;
    }
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = originalPos + new Vector3(-mouseWorldPos.x * intensity, -mouseWorldPos.y * intensity, 0);

        //transform.position =  new Vector3( Screen.width / 2 - (Input.mousePosition.x - Screen.width / 2) * (intensity / 10), transform.position.y, 0);
    }
}
