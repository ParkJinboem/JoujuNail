public partial class PlayerDataManager : MonoSingleton<PlayerDataManager>
{
    public delegate void PlayerQuestChangedHandler();
    public static event PlayerQuestChangedHandler OnPlayerQuestChanged;

    /// <summary>
    /// 미션 데이터 반환
    /// </summary>
    /// <returns></returns>
    public Quest GetQuest()
    {
        if (s.quest.level == 0)
        {
            //시작 난이도 조_231220_박진범
            s.quest.level = 1;
            //s.quest.level = 7;
            s.quest.allClear = false;
        }

        return s.quest;
    }

    /// <summary>
    /// 미션 보유 여부
    /// </summary>
    public bool IsHaveQuest()
    {
        if(s.quest.level == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 미션 데이터 저장
    /// </summary>
    /// <param name="quest"></param>
    private void SaveQuest(Quest quest)
    {
        s.quest = quest;

        isQuest = true;

        OnPlayerQuestChanged?.Invoke();
    }

    /// <summary>
    /// 미션 추가
    /// </summary>
    /// <param name="level"></param>
    private void AddQuest(int level)
    {
        
        Quest quest;
        //모든 스테이지 클리어시 랜덤으로 스테이지를 부여
        if (s.quest.allClear)
        {
            quest = new Quest()
            {
                level = UnityEngine.Random.Range(1, 141),
                allClear = true
            };
        }
        //마지막 레벨을 클리어하면 더이상 레벨을 올려주지 않음(현재 MaxLevel = 140)
        else
        {
            if (level >= Statics.maxLevel)
            {
                quest = new Quest()
                {
                    level = UnityEngine.Random.Range(1, 141),
                    allClear = true
                };
            }
            else
            {
                quest = new Quest()
                {
                    level = level + 1
                };
            }
        }

        
        SaveQuest(quest);

        OnPlayerQuestChanged?.Invoke();
    }

    /// <summary>
    /// 미션 완료
    /// </summary>
    /// <param name="level"></param>
    public void CompleteMission(int level)
    {
        AddQuest(level);
    }
}