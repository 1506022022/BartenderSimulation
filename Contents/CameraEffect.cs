using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CameraEffect : MonoBehaviour
{
    [SerializeField] private Volume volume;

    [SerializeField] private float effectTime;
    [SerializeField] private Image darkeningEffect;
    Vignette vignette;

    bool isEffecting = false;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)) { PadeInEffect(); }
        if(Input.GetKeyDown(KeyCode.X)) { PadeOutEffect(); }
    }

    // 밝은 곳에서 어두운 곳으로
    public void PadeOutEffect()
    {
        if (!isEffecting)
        {
            isEffecting = true;
            //volume.profile.TryGet(out vignette);
            //vignette.intensity.Interp(0f, 1f, effectTime);
            //Invoke("EffectEnd", effectTime);
            StartCoroutine(PadeOut());
        }
    }

    // 어두운 곳으로 밝은 곳으로
    public void PadeInEffect()
    {
        if (!isEffecting)
        {
            isEffecting = true;
            //volume.profile.TryGet(out vignette);
            StartCoroutine(PadeIn());
        }
    }

    IEnumerator PadeOut()
    {
        //var intensity = vignette.intensity;
        //while (intensity.GetValue<float>() <= 1f)
        //{
        //    var value = intensity.GetValue<float>() + 0.025f;
        //    intensity.Override(value);
        //    yield return null;
        //}

        //intensity.Override(1f);
        //isEffecting = false;

        float time = 0f;
        while(time < effectTime)
        {
            time+= Time.deltaTime;
            Color darkeingColor = darkeningEffect.color;
            darkeingColor.a = time / effectTime;
            darkeningEffect.color = darkeingColor;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator PadeIn()
    {
        //var intensity = vignette.intensity;
        //while (intensity.GetValue<float>() >= 0f)
        //{
        //    var value = intensity.GetValue<float>() - 0.025f;
        //    intensity.Override(value);
        //    yield return null;
        //}

        //intensity.Override(0f);
        //isEffecting = false;

        float time = 0f;
        while (time < effectTime)
        {
            time += Time.deltaTime;
            Color darkeingColor = darkeningEffect.color;
            darkeingColor.a = 256f - (256f / effectTime * time);
            darkeningEffect.color = darkeingColor;

            yield return new WaitForFixedUpdate();
        }
    }
}
