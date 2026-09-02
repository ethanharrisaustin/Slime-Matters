using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float globalVolume;

    public Sound[] sounds;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < sounds.Length; ++i)
        {
            GameObject newGO = new GameObject();

            newGO.name = sounds[i].name;
            newGO.transform.parent = transform;

            sounds[i].SetUp(this, newGO);
        }
    }

    public static void Play(string sound)
    {
        for (int i = 0; i < instance.sounds.Length; ++i)
        {
            if (instance.sounds[i].name != sound) continue;

            instance.sounds[i].Play();
        }
    }

    public static void Stop(string sound)
    {
        for (int i = 0; i < instance.sounds.Length; ++i)
        {
            if (instance.sounds[i].name != sound) continue;

            instance.sounds[i].Stop();
        }
    }

    public static void Play3D(string sound, Vector3 locationToPlayFrom)
    {
        for (int i = 0; i < instance.sounds.Length; ++i)
        {
            if (instance.sounds[i].name != sound) continue;

            instance.sounds[i].Play3D(locationToPlayFrom);
        }
    }
}
