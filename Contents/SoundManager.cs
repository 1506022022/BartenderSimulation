using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    SoundList m_Sounds;
    AudioSource m_Audio;

    private void Start()
    {
        m_Sounds = Resources.Load("SoundList") as SoundList;
        m_Audio = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (m_Sounds == null) return;
        string key = GetClipNameOnCollision(collision.gameObject.tag);
        if (key != null)
        {
            m_Sounds.PlaySound(m_Audio, key);
        }
    }
    public void ButtonSound()
    {
        m_Sounds.PlaySound(m_Audio, "BlenderMotor");
    }

    string GetClipNameOnCollision(string targetTag)
    {
        var myTag = gameObject.tag;

        if (myTag == "Bottle")
        {
            if (targetTag == "Table") return "Bottle_Table";
        }

        if (myTag == "Glass")
        {
            if (targetTag == "Barspoon") return "Barspoon_Glass";
            if (targetTag == "Glass") return "Glass_Glass";
            if (targetTag == "Ice") return "Glass_Ice";
        }

        if (myTag == "BlenderCase")
        {
            if (targetTag == "Ice") return "BlenderCase_Ice";


        }
        if (myTag == "ShakerBody")
        {
            if (targetTag == "Ice") return "Ice_Shaker";
        }
        if (myTag == "Scoop")
        {
            if (targetTag == "Ice") return "Ice_Scoop";
        }

        return null;
    }
}
