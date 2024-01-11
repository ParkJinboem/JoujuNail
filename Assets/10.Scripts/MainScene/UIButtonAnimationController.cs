using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonAnimationController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator mainBtnAnim;

    public void Start()
    {
        //인보크를 사용안하면 데이터가 준비가 안된상태에서 Init을 실행함
        Invoke("Init", 1.0f);
    }

    public void Init()
    {
        mainBtnAnim.Rebind();
        if (!PlayerDataManager.Instance.IsCompleteTutorial(3))
        {
            mainBtnAnim.SetTrigger("NotTutorial");
        }
        else
        {
            mainBtnAnim.SetTrigger("Action");
        }
    }

    public void TutorialAnimPlay()
    {
        //튜토리얼3을 클리어하지않았으면 튜토리얼3을 클리어로 체크후 PlayerData에 저장
        if (!PlayerDataManager.Instance.IsCompleteTutorial(3))
        {
            TutorialManager.Instance.ClearTutorial(3);
            mainBtnAnim.Rebind();
            mainBtnAnim.SetTrigger("Clear");
        }
    }
}
