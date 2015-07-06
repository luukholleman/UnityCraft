using UnityEngine;
using System.Collections;

public class OpenPopup : MonoBehaviour {

	public Transform targetPopup;

	private void OnClick()
	{
		targetPopup.gameObject.SetActive(true);
	}
}
