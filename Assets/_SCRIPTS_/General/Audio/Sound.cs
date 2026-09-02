using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume;
    public float pitch;
    public float pan;
    public bool loop;
    public bool playOnAwake = false;

    AudioSource audioSource;

    AudioManager audioManager;

    GameObject go;

    public void SetUp(AudioManager audioManager, GameObject soundGO)
    {
        this.audioManager = audioManager;

        audioSource = soundGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.panStereo = pan;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;

        go = soundGO;

        if (playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        audioSource.volume = volume * audioManager.globalVolume;

        audioSource.spatialBlend = 0f;

        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Play3D(Vector3 locationToPlayFrom)
    {
        go.transform.position = locationToPlayFrom;

        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        audioSource.volume = volume * audioManager.globalVolume;

        audioSource.PlayOneShot(clip);
    }
}
