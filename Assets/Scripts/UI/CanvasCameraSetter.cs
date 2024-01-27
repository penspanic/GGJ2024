using System;
using UnityEngine;

namespace UI
{
    public class CanvasCameraSetter : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}