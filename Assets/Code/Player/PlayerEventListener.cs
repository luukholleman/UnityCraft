using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Messenger;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Code.Player
{
    class PlayerEventListener : MonoBehaviour
    {
        private FirstPersonController _firstPersonController;

        void Awake()
        {
            _firstPersonController = GetComponent<FirstPersonController>();

            Postman.AddListener("popup opened", PopupOpened);
            Postman.AddListener("popup closed", PopupClosed);
        }

        void PopupOpened()
        {
            _firstPersonController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void PopupClosed()
        {
            _firstPersonController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
