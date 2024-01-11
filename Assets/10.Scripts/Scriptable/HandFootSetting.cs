using System.Collections.Generic;
using UnityEngine;

public enum HandFootType
{
    Hand = 0,
    Foot = 1
}

public enum SceneType
{
    Quest = 0,
    Free = 1
}

[System.Serializable]
public struct HandFootData
{
    public SceneType sceneType;
    public HandFootType handFootType;
    public AnimationCurve baseAnimationCurve;
    public Vector3 basePosition;
    public List<float> rots;
    public List<AnimationCurve> scaleAnimationCurve;
    public List<Vector3> pos;
    public bool enabled;
    
}

[CreateAssetMenu(fileName = "HandFootSetting", menuName = "ScriptableObjects/HandFootSettings")]
public class HandFootSetting : ScriptableObject
{
    [SerializeField] private List<HandFootData> datas;

    public HandFootData GetData(HandFootType handFootType, SceneType sceneType)
    {
        return datas.FindAll(x => x.sceneType == sceneType).Find(x => x.handFootType == handFootType);
    }
}
