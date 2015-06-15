using UnityEngine;

namespace Assets.Code.GUI
{
    public class CrossHair : MonoBehaviour {

        public Texture2D CrosshairTexture;
        public float CrosshairScale = 1;

        private Pausing _pausing;

        void Awake()
        {
            _pausing = GameObject.Find("General").GetComponent<Pausing>();
        }

        void Update()
        {
            if (_pausing.Paused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        void OnGUI()
        {
            if (CrosshairTexture != null)
                UnityEngine.GUI.DrawTexture(new Rect((Screen.width - CrosshairTexture.width * CrosshairScale) / 2, (Screen.height - CrosshairTexture.height * CrosshairScale) / 2, CrosshairTexture.width * CrosshairScale, CrosshairTexture.height * CrosshairScale), CrosshairTexture);
        }
    }
}
