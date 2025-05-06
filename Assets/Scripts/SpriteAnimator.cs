using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteRenderer mySpriteRenderer;
    public AnimationData baseAnimation;
    Coroutine previousAnimation;

    private void Start()
    {
        PlayAnimation(baseAnimation);
    }

    public void PlayAnimation(AnimationData data)
    {
        //stop previos animation
        if(previousAnimation != null)
        {
            StopCoroutine(previousAnimation);
        }
        //start new animation
        previousAnimation = StartCoroutine(PlayAnimationCoroutine(data));
        
    }

    public IEnumerator PlayAnimationCoroutine(AnimationData data)
    {
        if(data == null){
            data = baseAnimation;
        }
        int spritesAmount = data.sprites.Length;
        int i = 0;
        float waitTime = data.framesOfGap * AnimationData.targetFrameTime;
        while (spritesAmount > i)
        {
            mySpriteRenderer.sprite = data.sprites[i++]; // changes sprite and increse i
            yield return new WaitForSeconds(waitTime);

            if (data.loop && i>= spritesAmount)
            {
                i = 0;
            }
        }
        if (data.returnToBase && data != baseAnimation)
        {
            PlayAnimation(baseAnimation);
        }

        yield return null;
    }


}
