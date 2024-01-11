public partial class DataManager : MonoSingleton<DataManager>
{
    public delegate void DataInitedHandler();
    public static event DataInitedHandler OnDataInited;

    /// <summary>
    /// 초기 설정
    /// </summary>
    public void Init()
    {
        InitHandFootPartsData();
        InitManicureData();
        InitQuestData();
        InitRewardData();

        OnDataInited?.Invoke();
    }
}