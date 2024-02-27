using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterDrinkSount : MonoBehaviour
{
    SoundList m_Sounds;
    AudioSource m_Audio;

    private void Start()
    {
        m_Sounds = Resources.Load("SoundList") as SoundList;
        m_Audio = GetComponent<AudioSource>();
    }

    public void DrinkSoundPlay()
    {
        m_Sounds.PlaySound(m_Audio, "Drink");
    }
}
