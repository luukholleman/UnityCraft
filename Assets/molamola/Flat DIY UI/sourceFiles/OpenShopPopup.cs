using UnityEngine;
using System.Collections;

public class OpenShopPopup : MonoBehaviour {

	public Transform targetPopup;
	public Transform targetTab;

	private void OnClick()
	{
		targetPopup.gameObject.SetActive(true);

		ToggleGrid toggle = targetTab.GetComponent<ToggleGrid>();
		
		if(toggle)
		{
			Debug.Log("fffffffffffffffffffffffffff");
			toggle. goldTab.value = true;
			toggle.OnGoldTab(null);
		}
	}
}
