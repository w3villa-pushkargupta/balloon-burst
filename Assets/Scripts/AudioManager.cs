using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;   
    [SerializeField] private AudioClip popClip;         
    [SerializeField] private AudioClip shakeClip;      

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayPopSound()
    {
        if (popClip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(popClip);
        }
    }

    public void PlayShakeSound()
    {
        if (shakeClip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.2f);
            audioSource.PlayOneShot(shakeClip);
        }
    }
}
