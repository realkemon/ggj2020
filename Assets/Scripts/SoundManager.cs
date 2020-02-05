using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip buildingGrowClip;
    public AudioClip[] buildingFinishedClip;
    public AudioClip[] converterUsedClip;
    public AudioClip debrisHitClip;
    public AudioClip itemPickUpClip;
    public AudioClip itemAcceptClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBuildingGrowSound()
    {
        audioSource.PlayOneShot(buildingGrowClip, 0.8f);
    }

    public void PlayBuildingFinishedSound(int index)
    {
        audioSource.PlayOneShot(buildingFinishedClip[index], 0.8f);
    }

    public void PlayConverterUsedSound(int index)
    {
        audioSource.PlayOneShot(converterUsedClip[index], 0.85f);
    }

    public void PlayDebrisHitSound()
    {
        audioSource.PlayOneShot(debrisHitClip, 0.3f);
    }

    public void PlayItemPickUpSound()
    {
        audioSource.PlayOneShot(itemPickUpClip, 0.15f);
    }

    public void PlayItemAcceptSound()
    {
        audioSource.PlayOneShot(itemAcceptClip, 0.15f);
    }
}