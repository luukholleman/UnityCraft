using UnityEngine;

namespace Assets.Code.GUI
{
    public class CrossHair : MonoBehaviour {

        public Texture2D CrosshairTexture;
        public float CrosshairScale = 1;

        void OnGUI()
        {
            if (CrosshairTexture != null)
                UnityEngine.GUI.DrawTexture(new Rect((Screen.width - CrosshairTexture.width * CrosshairScale) / 2, (Screen.height - CrosshairTexture.height * CrosshairScale) / 2, CrosshairTexture.width * CrosshairScale, CrosshairTexture.height * CrosshairScale), CrosshairTexture);
        }
    }
}
