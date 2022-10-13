using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerAttack.UI
{
    //Attach to the canvas contains health bar and damage text.
    public class CameraFacing : MonoBehaviour
    {
        private Transform UITransform;
        private Transform cameraTransform;

        void Awake()
        {
            UITransform=GetComponent<Transform>();
            cameraTransform=Camera.main.GetComponent<Transform>();
        }

        //Let the health bar always face to main camera.
        private void LateUpdate()
        {
            UITransform.forward = cameraTransform.forward;
        }
    }
}
