using UnityEngine;
using System.Collections;

public class AnimationEntity : MonoBehaviour
{
	public  delegate void OnAnimationFinishDelegate(AnimationEntity animationEntity);
	public  OnAnimationFinishDelegate OnAnimationFinishCallbackDelegate;

	private Animation _animation = null;

	protected AnimationEntity()						 
	{
		
	}

	private void Awake()
	{
		this._animation = gameObject.GetComponent<Animation>();
	}

	public bool IsPlaying(string name)
	{
		return _animation.IsPlaying(name);
	}

	public bool IsPlaying()
	{
		return _animation.isPlaying;
	}

	public void SetAnimationEntity()//Animation animation)
	{
		_animation = gameObject.GetComponent<Animation>();
	}

	public void Play(string animationName,
					 OnAnimationFinishDelegate OnFinishEventCallback = null,
					 bool isPlayNow = true)
	{
		if(_animation == null)
			SetAnimationEntity();

		QueueMode mode = QueueMode.CompleteOthers;
		if(isPlayNow)// && this._animation.isPlaying)
		{			
			//Logger.Log("DEBUG","OnAnimatiWasntPlaying");
			mode = QueueMode.PlayNow;
			this.Stop();
		}
		//Logger.Log("DEBUG","OnAnimatiWasntPlaying");

		this.OnAnimationFinishCallbackDelegate	 = OnFinishEventCallback;
		this._animation.PlayQueued(animationName, mode);
		this._animation.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(StartCheckingEnd());
	}

	public void Play(string animationAppear, string animationDisappear, 
					 OnAnimationFinishDelegate OnFollowingAnimationFinished = null, bool isPlayNow = true)
	{
		AnimationEntity.OnAnimationFinishDelegate OnFollowingAnimation = delegate(AnimationEntity animationEntity)
        {
        	animationEntity.Play(animationDisappear, OnFollowingAnimationFinished, isPlayNow);
        };

		this.Play(animationAppear, OnFollowingAnimation, isPlayNow);
	}

	public void Stop()
	{
		this._animation.Stop();	
	}

	public void SetAnimationSpeed(string animationName, float speed)
	{
		//Logger.Log("DEBUG","animationName: " + animationName);
		
		this._animation[animationName].speed = speed;
	}


	public float GetAnimationLength(string animationName)
	{
		return this._animation[animationName].length;
	}

	public IEnumerator StartCheckingEnd()
	{
		while(_animation.isPlaying)
		{
			//if(this.tag == Defines.TAG_ITEM )
			//Logger.Log("DEBUG","an animation is still playing");

			yield return null;
		}

		if(OnAnimationFinishCallbackDelegate != null)
		{	
			OnAnimationFinishCallbackDelegate(this);
		}
	}
}
