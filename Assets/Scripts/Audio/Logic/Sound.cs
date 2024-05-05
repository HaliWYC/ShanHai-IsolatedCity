using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void SetSound(SoundDetails soundDetails)
    {
        audioSource.clip = soundDetails.soundClip;
        audioSource.volume = soundDetails.soundVolume;
        audioSource.pitch = Random.Range(soundDetails.soundMinPitch, soundDetails.soundMaxPitch);
    }
}