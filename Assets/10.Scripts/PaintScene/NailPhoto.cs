using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class NailPhoto : MonoBehaviour
{
    public PlayableDirector paintScenePlayableDirector;
    public PlayableDirector questPaintScenePlayableDirector;
    public AudioSource timeLineAudio;

    public void TImeLineStart(Hand hand, IQuestable questable, IFreeable freeable, bool soundOn)
    {
        timeLineAudio.volume = PlayerDataManager.Instance.GetOptionData().soundVolume;
        StopAllCoroutines();
        StartCoroutine(ITimeLineStart(hand, questable, freeable, soundOn));
    }

    IEnumerator ITimeLineStart(Hand hand, IQuestable questable, IFreeable freeable, bool soundOn)
    {
        if (Statics.gameMode == GameMode.Free)
        {
            timeLineAudio.mute = !soundOn;
            paintScenePlayableDirector.Play();
            yield return new WaitUntil(() => paintScenePlayableDirector.state == PlayState.Paused);
        }
        else if (Statics.gameMode == GameMode.Quest)
        {
            timeLineAudio.mute = !soundOn;
            questPaintScenePlayableDirector.Play();
            yield return new WaitUntil(() => questPaintScenePlayableDirector.state == PlayState.Paused);
        }

        //타임라인 재생되는 시간
        //yield return new WaitForSeconds(9f);
        
        if (Statics.gameMode == GameMode.Free)
        {
            HandDataCountCheck(hand);
            PaintSceneManager.Instance.Hide();
            if (Statics.selectType == SelectType.Hand)
            {
                freeable.AllReset();
            }
            else if (Statics.selectType == SelectType.Foot)
            {
                freeable.AllReset();
            }
            MainSceneController.Instance.MoveToAlbumScene();
        }
        else if (Statics.gameMode == GameMode.Quest)
        {
            HandDataCountCheck(hand);
            //모든 데이터 저장후 베인 화면으로 이동
            questable.MoveToMainScene();
        }
    }

    private void HandDataCountCheck(Hand saveHand)
    {
        int saveDataCount = PlayerDataManager.Instance.GetHandData().Count;
        if(saveDataCount > 24)
        {
            for (int i = 0; i < saveDataCount - 24; i++)
            {
                PlayerDataManager.Instance.handData.hands.RemoveAt(i);
            }
        }
        PlayerDataManager.Instance.AddHand(saveHand);

    }
}
