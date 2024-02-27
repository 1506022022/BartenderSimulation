using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenChangeEffect : MonoBehaviour
{
    [SerializeField] private CameraEffect cameraEffect;
    [SerializeField] private FlowChanger flowChanger;
    [SerializeField] private WaterDrinkSount drinkSoundManager;
    [SerializeField] private TItle_Information titleInformation;

    public void TitleToGame()
    {
        StartCoroutine(TitleToGameEffect());
    }

    IEnumerator TitleToGameEffect()
    {
        cameraEffect.PadeOutEffect();
        titleInformation.Disable();
        yield return new WaitForSecondsRealtime(2f);
        drinkSoundManager.DrinkSoundPlay();
        yield return new WaitForSecondsRealtime(3f);
        //flowChanger.CurrentFlow = Flow.Game;
        //cameraEffect.PadeInEffect();
    }
}
