using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField]
    RectTransform logoSprite, playButton, instructionButton, storeButton, creditsButton, hackButton, quitButton;
    void Start()
    {
        PlayAnimation();
    }

   //LeanTween para criar as animaçoes do menu inicial.
    void PlayAnimation()
    {
        LTDescr tween;
        tween = LeanTween.scale(this.logoSprite, Vector3.one, 2);
        tween.setEase(LeanTweenType.easeOutBounce);
        tween.setFrom(Vector3.zero);

        tween = LeanTween.moveX(this.playButton, 0, 1.5f);
        tween.setFrom(-1000);
        tween.setEase(LeanTweenType.easeOutBounce);

        tween = LeanTween.moveX(this.instructionButton, 0, 1.5f);
        tween.setFrom(1000);
        tween.setEase(LeanTweenType.easeOutBounce);

        tween = LeanTween.moveX(this.storeButton, 0, 1.5f);
        tween.setFrom(-1000);
        tween.setEase(LeanTweenType.easeOutBounce);

        tween = LeanTween.moveX(this.creditsButton, 0, 1.5f);
        tween.setFrom(1000);
        tween.setEase(LeanTweenType.easeOutBounce);

        tween = LeanTween.moveX(this.hackButton, 0, 1.5f);
        tween.setFrom(-1000);
        tween.setEase(LeanTweenType.easeOutBounce);

        tween = LeanTween.moveX(this.quitButton, 0, 1.5f);
        tween.setFrom(1000);
        tween.setEase(LeanTweenType.easeOutBounce);

    }
}
