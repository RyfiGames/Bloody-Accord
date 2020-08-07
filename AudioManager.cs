using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] audioClips;
    public List<AudioSource> audioSources;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlaySound(string sName)
    {
        List<AudioClip> acl = new List<AudioClip>();

        foreach (AudioClip ac in audioClips)
        {
            if (ac.name.Contains(sName))
            {
                acl.Add(ac);
            }
        }

        if (acl.Count > 0)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.clip = acl[Random.Range(0, acl.Count)];
            audioSources.Add(audioSource);
            audioSource.Play();
        }
        else
        {
            print(sName + " doesnt exist.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<AudioSource> aSR = new List<AudioSource>();
        foreach (AudioSource aS in audioSources)
        {
            if (!aS.isPlaying)
            {
                aSR.Add(aS);
            }
        }
        foreach (AudioSource aS in aSR)
        {
            audioSources.Remove(aS);
            Destroy(aS);
        }
    }
}
