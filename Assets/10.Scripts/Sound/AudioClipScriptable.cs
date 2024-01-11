using UnityEngine;
using System.Linq;

[System.Serializable] public class AudioClipDictionary : SerializableDictionary<string, AudioClip> { }

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioClipTables", order = 1)]
public class AudioClipScriptable : ScriptableObject
{
    [SerializeField] private AudioClipDictionary stringByClip = new AudioClipDictionary();

    public AudioClip GetClip(string key)
    {
        if (!stringByClip.ContainsKey(key))
        {
            return null;
        }
        else
        {
            return stringByClip[key];
        }
    }

    public string GetKey(AudioClip audioClip)
    {
        if(!stringByClip.ContainsValue(audioClip))
        {
            return null;
        }
        else
        {
            return stringByClip.FirstOrDefault(x => x.Value == audioClip).Key;
        }
    }
}