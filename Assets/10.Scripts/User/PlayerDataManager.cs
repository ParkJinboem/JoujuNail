using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public partial class PlayerDataManager : MonoSingleton<PlayerDataManager>
{
    [Serializable]
    public class S
    {
        public Option option;
        public Info info; 
        public Quest quest;
    }

    [Serializable]
    public class SL
    {
        public List<Tutorial> tutorials;
        //public List<Hand> hands;
        public List<Collection> collections;
    }

    [Serializable]
    public class HandData
    {
        //public List<Tutorial> tutorials;
        public List<Hand> hands;
        //public List<Collection> collections;
    }

    public S s; // 자료형
    public SL sl; // 리스트
    public HandData handData;

    private bool isOption;
    private bool isInfoSave;
    private bool isQuest;
    private bool isTutorialSave;
    private bool isHandSave;
    private bool iscollctionItem;

    protected override bool CheckDontDestroyOnLoad()
    {
        return true;
    }

    public void Init()
    {
        isHandSave = true;
        LoadData();
        SaveData();
        SetHandImage();
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    public void SaveData()
    {
        //S데이터 저장
        if (isOption == true || isInfoSave == true  || isQuest == true)
        {
            PlayerPrefs.SetString("s", JsonConvert.SerializeObject(s));

            isOption = false;
            isInfoSave = false;
            isQuest = false;
     
        }
        //SL데이터 저장
        if (isTutorialSave == true || iscollctionItem == true)
        {
            PlayerPrefs.SetString("sl", JsonConvert.SerializeObject(sl));

            isTutorialSave = false;
            iscollctionItem = false;
        }
        if(isHandSave == true)
        {
            PlayerPrefs.SetString("handData", JsonConvert.SerializeObject(handData));

            isHandSave = false;
        }
    }

    /// <summary>
    /// 데이터 로드
    /// </summary>
    public void LoadData()
    {
        if (PlayerPrefs.HasKey("s"))
        {
            s = JsonConvert.DeserializeObject<S>(PlayerPrefs.GetString("s"));
            sl = JsonConvert.DeserializeObject<SL>(PlayerPrefs.GetString("sl"));
            handData = JsonConvert.DeserializeObject<HandData>(PlayerPrefs.GetString("handData"));
        }

        // S 초기화
        if (s.option.language == "")
        {
            s.option = new Option
            {
                language = "Korean",
                isSound = true,
                soundVolume = 0.5f
            };
        }
        // SL 초기화
        if (s.info == null) s.info = new Info();
        if (s.quest == null) s.quest = new Quest();
        if (sl.tutorials == null) sl.tutorials = new List<Tutorial>();
        if (sl.collections == null) sl.collections = new List<Collection>();
        if (handData.hands == null) handData.hands = new List<Hand>();
        //if (sl.hands == null) sl.hands = new List<Hand>();
    }
}
