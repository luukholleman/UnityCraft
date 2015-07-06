using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour {

	public Transform exitButton;
	public string 	 exitAnimationName;

	// Use this for initialization
	void Start () {
	
		UIButton uiButton  		= exitButton.GetComponent<UIButton>();
		UIEventListener events  = exitButton.GetComponent<UIEventListener>();

		if (events == null)
		{
			events = exitButton.gameObject.AddComponent<UIEventListener>();			
		}
		events.onClick = this.OnExit;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnExit(GameObject obj)
	{

		AnimationEntity ani = this.GetComponent<AnimationEntity>();//
		if (ani == null )
		{
			ani = this.gameObject.AddComponent<AnimationEntity>();
		}
		
        AnimationEntity.OnAnimationFinishDelegate OnAnimationFinished = delegate(AnimationEntity animationEntity)
        {
            //GameObject.Destroy(ani.gameObject);
            this.gameObject.SetActive(false);
        };

		ani.Play(exitAnimationName, OnAnimationFinished, true);
	}
}
