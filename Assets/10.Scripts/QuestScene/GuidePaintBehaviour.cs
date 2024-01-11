using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GuidePaintBehaviour : MonoBehaviour
{
    private QuestHandData questHandData;

    [SerializeField] private RawImage[] fingerGudiePaints;
    [SerializeField] private RawImage[] toesGudiePaints;
    [SerializeField] private AnimationCurve colorAlphaCurve;

    private float guideTexturedBlinkTime;
    public float GuideTexturedBlinkTime
    {
        set { guideTexturedBlinkTime = value; }
    }

    private bool isBlink = true;

    /// <summary>
    /// 초기값 설정
    /// </summary>
    /// <param name="questHandData"></param>
    public void Init(QuestHandData questHandData)
    {
        this.questHandData = questHandData;

        if (questHandData.selectType == SelectType.Hand)
        {
            SetGuidePaint(fingerGudiePaints);
        }
        else
        {
            SetGuidePaint(toesGudiePaints);
        }
    }

    /// <summary>
    /// 가이드 손톱 설정
    /// </summary>
    /// <param name="gudiePaints"></param>
    private void SetGuidePaint(RawImage[] gudiePaints)
    {
        for (int i = 0; i < 5; i++)
        {
            Texture2D tex = HandFootTextureMaker.Instance.GetTexture2D(questHandData.level, questHandData.selectType, i);
            string targetTexture = "_MainTex";

            if (gudiePaints[i].gameObject.GetComponent<MeshRenderer>() == null)
            {
                MeshRenderer meshRenderer = gudiePaints[i].gameObject.AddComponent<MeshRenderer>();
                meshRenderer.material.SetTexture(targetTexture, tex);
            }
            gudiePaints[i].texture = tex;
        }
    }

    /// <summary>
    /// 가이드 손톱 리셋
    /// </summary>
    /// <param name="fingerNum"></param>
    public void ResetGuidePaint(int fingerNum)
    {
        StopAllCoroutines();
        if (questHandData.selectType == SelectType.Hand)
        {
            fingerGudiePaints[fingerNum].color = new Color(1, 1, 1, 0);
        }
        else
        {
            toesGudiePaints[fingerNum].color = new Color(1, 1, 1, 0);
        }
    }

    /// <summary>
    /// 가이드페인트 셋팅
    /// </summary>
    /// <param name="fingerNum"></param>
    /// <param name="sceneName"></param>
    public void SetupGuidePaint(int fingerNum, string sceneName)
    {
        fingerGudiePaints[fingerNum].gameObject.SetActive(true);
        GuidePaintBlink(true, 2.0f, sceneName, fingerNum);
    }

    /// <summary>
    /// 가이트 페인트 알파값 조절
    /// </summary>
    /// <param name="isBlinked"></param>
    /// <param name="secend"></param>
    /// <param name="sceneName"></param>
    /// <param name="fingerNum"></param>
    private void GuidePaintBlink(bool isBlinked, float secend, string sceneName, int fingerNum)
    {
        StopCoroutine(IGuidePaintBlink(isBlinked, secend, sceneName, fingerNum));
        StartCoroutine(IGuidePaintBlink(isBlinked, secend, sceneName, fingerNum));
    }

    IEnumerator IGuidePaintBlink(bool isBlinked, float secend, string sceneName, int fingerNum)
    {
        RawImage[] gudiePaints;
        if (questHandData.selectType == SelectType.Hand)
        {
            gudiePaints = fingerGudiePaints;
        }
        else
        {
            gudiePaints = toesGudiePaints;
        }

        guideTexturedBlinkTime = 0;
        yield return new WaitForSeconds(secend);

        //if (isBlinked == true && (sceneName == "ManicureScene" || sceneName == "Null"))
        {
            while (true)
            {
                if (isBlink)
                {
                    while (guideTexturedBlinkTime <= 1.5f)
                    {
                        guideTexturedBlinkTime += Time.deltaTime;
                        gudiePaints[fingerNum].color = new Color(1, 1, 1, colorAlphaCurve.Evaluate(guideTexturedBlinkTime));
                        yield return null;
                    }
                    guideTexturedBlinkTime = 1.5f;
                    isBlink = false;
                }
                else if (!isBlink)
                {
                    while (guideTexturedBlinkTime >= 0)
                    {
                        guideTexturedBlinkTime -= Time.deltaTime;
                        gudiePaints[fingerNum].color = new Color(1, 1, 1, colorAlphaCurve.Evaluate(guideTexturedBlinkTime));
                        yield return null;
                    }
                    guideTexturedBlinkTime = 0;
                    isBlink = true;
                }
            }
        }
        //else if (isBlinked == false && sceneName != "ManicureScene")
        //{
        //    Debug.Log("bbb");
        //    gudiePaints[fingerNum].color = new Color(1, 1, 1, 0);
        //    guideTexturedBlinkTime = 0;
        //}
    }

    public void HideGuidePaint(int selectFingerIndex)
    {
        fingerGudiePaints[selectFingerIndex].gameObject.SetActive(false);
    }
}