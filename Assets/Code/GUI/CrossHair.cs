using UnityEngine;

namespace Assets.Code.GUI
{
    public class CrossHair : MonoBehaviour {

        public Texture2D CrosshairTexture;
        public float CrosshairScale = 1;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        void OnGUI()
        {
            if (CrosshairTexture != null)
                UnityEngine.GUI.DrawTexture(new Rect((Screen.width - CrosshairTexture.width * CrosshairScale) / 2, (Screen.height - CrosshairTexture.height * CrosshairScale) / 2, CrosshairTexture.width * CrosshairScale, CrosshairTexture.height * CrosshairScale), CrosshairTexture);
        }
    }
}
