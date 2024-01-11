using System.Collections.Generic;

public partial class PlayerDataManager : MonoSingleton<PlayerDataManager>
{
    public delegate void PlayerItemChangedHandler(int itemId);
    public static event PlayerItemChangedHandler OnPlayerItemChanged;

    /// <summary>
    /// 컬렌션 데이터 반환
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public Collection GetCollectionById(int itemId)
    {
        Collection collection = sl.collections.Find(x => x.id == itemId);
        return collection;
    }

    public List<Collection> GetCollectionData()
    {
        return sl.collections;
    }

    /// <summary>
    /// 컬렉션데이터 저장
    /// </summary>
    /// <param name="collection"></param>
    public void SaveItemData(Collection collection)
    {
        int index = sl.collections.FindIndex(x => x.id == collection.id);
        if (index == -1)
        {
            sl.collections.Add(collection);
        }
        else
        {
            sl.collections[index] = collection;
        }

        iscollctionItem = true;

        SaveData();

        OnPlayerItemChanged?.Invoke(collection.id);
    }

    /// <summary>
    /// 컬렉션을 가지고있는지 확인
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public bool IsHaveCollection(int itemId)
    {
        Collection collection = GetCollectionById(itemId);
        if (collection == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 컬렉션 추가
    /// </summary>
    /// <param name="collectionData"></param>
    public void AddCollection(Collection collectionData)
    {
        if (IsHaveCollection(collectionData.id))
        {
            return;
        }

        Collection collection = new Collection()
        {
            id = collectionData.id,
        };

        SaveItemData(collection);

        OnPlayerItemChanged?.Invoke(collection.id);
    }

    #region Sound

    public Option GetOptionData()
    {
        return s.option;
    }

    public void SaveSoundOnOff(bool isOn)
    {
        s.option.isSound = isOn;
        isOption = true;
        SaveData();
    }

    public void SaveSoundVolume(float value)
    {
        s.option.soundVolume = value;
        isOption = true;
        SaveData();
    }

    #endregion

    #region Language

    public void SaveLanguage(string language)
    {
        s.option.language = language;
        isOption = true;
        SaveData();
    }

    #endregion
}