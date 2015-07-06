using UnityEngine;
using System.Collections;

public class ToggleGrid : MonoBehaviour {

	public Transform 	goldItems;
	public Transform 	cashItems;

	public UIToggle 	goldTab;
	public UIToggle 	cashTab;

	// Use this for initialization
	void Start () 
	{
		UIEventListener events  = goldTab.GetComponent<UIEventListener>();

		if (events == null)
		{
			events = goldTab.gameObject.AddComponent<UIEventListener>();			
		}
		events.onClick = this.OnGoldTab;

		UIEventListener eventsCash  = cashTab.GetComponent<UIEventListener>();

		if (eventsCash == null)
		{
			eventsCash = cashTab.gameObject.AddComponent<UIEventListener>();			
		}
		eventsCash.onClick = this.OnCashTab;		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnGoldTab(GameObject btn)
	{
		goldItems.gameObject.SetActive(true);
		cashItems.gameObject.SetActive(false);
	}

	public void OnCashTab(GameObject btn)
	{
		cashItems.gameObject.SetActive(true);
		goldItems.gameObject.SetActive(false);
	}
}


