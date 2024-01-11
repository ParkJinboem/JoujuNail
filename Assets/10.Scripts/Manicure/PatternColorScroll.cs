using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PatternColorScroll : SelectColorTriggerBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private PatternScrollCheckBox patternScrollCheckBox;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject center;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private List<GameObject> colorObjects;
    [SerializeField] private List<float> quaternions;

    public List<GameObject> childObjects;
    public GameObject currrentColorObject;

    public Image baseColorImage;

    private float time = 0;

    public int currentNumber;
    public int newNumber;

    private bool isInit;
    public bool IsInit
    {
        set { isInit = value; }
    }
    private bool isRot = false;
    public bool autoRot = false;
    public bool correctSoundCheck = false;
    public bool RotSoundCheck = false;

    private IQuestable questable;
    private IFreeable freeable;

    /// <summary>
    /// 초기값 설정
    /// </summary>
    public void Init()
    {
        autoRot = true;
        if (Statics.gameMode == GameMode.Free)
        {
            freeable = PaintSceneManager.Instance.Freeables[0];
            questable = null;
        }
        else
        {
            freeable = null;
            questable = QuestSceneManager.Instance.Questables[0];
        }

        currrentColorObject = patternScrollCheckBox.CollisionObject;

        for (int i = 0; i < colorObjects.Count; i++)
        {
            PatternColorConverter patternColorConverter = colorObjects[i].GetComponent<PatternColorConverter>();
            patternColorConverter.Init(i, questable);
        }

        if (isInit == false)
        {
            PatternColorAnim();
        }
        else
        {
            if (currrentColorObject != null)
            {
                InitSelectColor();
            }
        }
        isInit = true;
    }

    /// <summary>
    /// 첫 실행인지 확인후 색상 설정
    /// </summary>
    private void InitSelectColor()
    {
        StopCoroutine(IInitSelectColor());
        StartCoroutine(IInitSelectColor());
    }

    IEnumerator IInitSelectColor()
    {
        yield return new WaitForSeconds(0.75f);
        SelectColor();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Statics.gameMode == GameMode.Quest)
        {
            if (questable.IsClearing() == true)
            {
                return;
            }
        }
        center.transform.Rotate(0f, 0f, Input.GetAxis("Mouse Y") * Time.deltaTime * 200, Space.World);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PatternColorConverter patternColorConverter = currrentColorObject.GetComponent<PatternColorConverter>();
        Quaternion rot = Quaternion.Euler(0, 0, quaternions[patternColorConverter.ColorData.id]);
        center.transform.rotation = rot;
        if (Statics.gameMode == GameMode.Quest)
        {
            questable.QuestConfirm();
        }
    }

    /// <summary>
    /// 베이스 컬러 셋팅
    /// </summary>
    /// <param name="manicureInfoData"></param>
    public void InitBaseColor(ManicureInfoData manicureInfoData)
    {
        baseColorImage.color = manicureInfoData.baseColor;
        PatternColorConverter patternColorConverter1 = baseColorImage.GetComponent<PatternColorConverter>();
        PatternColorConverter patternColorConverter2 = currrentColorObject.GetComponent<PatternColorConverter>();
        if (patternColorConverter1 == patternColorConverter2)
        {
            if (Statics.gameMode == GameMode.Free)
            {
                patternColorConverter1.Init(0, null);
                freeable.ChageColorPatternObject(patternColorConverter1.ColorData.id, baseColorImage.color);
            }
            else if (Statics.gameMode == GameMode.Quest)
            {
                patternColorConverter1.Init(0, questable);
                questable.ChageColorPatternObject(baseColorImage.color);
            }
        }
        else
        {
            ColorData colorData = new ColorData()
            {
                id = 0,
                color = baseColorImage.color
            };
            patternColorConverter1.colorData = colorData;
        }
    }

    /// <summary>
    /// 컬러 선택
    /// </summary>
    public void SelectColor()
    {
        if (isInit == false)
        {
            return;
        }

        //가이드핸드 애니메이션 종료
        if (Statics.gameMode == GameMode.Free && !autoRot)
        {
            PaintSceneManager.Instance.FreeGuideHandSetUp(4);
        }
        else if (Statics.gameMode == GameMode.Quest && !autoRot)
        {
            QuestSceneManager.Instance.QuestGuideHandSetUp(4);
        }

        if (currrentColorObject != null)
        {
            currrentColorObject = patternScrollCheckBox.CollisionObject;
        }
        else
        {
            currrentColorObject = childObjects[0];
            currentNumber = 0;
        }

        PatternColorConverter patternColorConverter = currrentColorObject.GetComponent<PatternColorConverter>();
        if(patternColorConverter == null)
        {
            return;
        }
        Color color = patternColorConverter.ColorData.color;
        childObjects = ChildNumbers(patternColorConverter.ColorData.id);
        if(Statics.gameMode == GameMode.Quest)
        {
            questable.PatternColor = color;
        }
        
        if (Statics.gameMode == GameMode.Free)
        {
            freeable.ChageColorPatternObject(patternColorConverter.ColorData.id, color);
        }
        else if (Statics.gameMode == GameMode.Quest)
        {
            if(!autoRot)
            {
                questable.ChageColorPatternObject(color);
            }
            if (questable.IsPatternScroll == false)
            {
                QuestSceneManager.Instance.CheckPatternColor(correctSoundCheck);
            }
        }

        patternScrollCheckBox.IsTrigger = false;
    }

    /// <summary>
    /// 진입시 패턴컬러스크롤 회전
    /// </summary>
    private void PatternColorAnim()
    {
        autoRot = true;
        center.transform.rotation = Quaternion.Euler(0,0,30);
        StopCoroutine(IPatternColorAnim());
        StartCoroutine(IPatternColorAnim());
    }

    IEnumerator IPatternColorAnim()
    {
        if (Statics.gameMode == GameMode.Quest)
        {
            questable.IsPatternScroll = true;
        }
        //animator.ResetTrigger("isRotation");
        animator.Rebind();
        yield return new WaitForSeconds(0.75f);
        //animator.SetBool("isRotation", true);
        animator.SetTrigger("isRotation");
        //yield return new WaitForSeconds(1.5f);
        //animator.SetBool("isRotation", false);
        //center.transform.rotation = Quaternion.Euler(0, 0, 30);
        yield return new WaitForSeconds(0.375f);
        if (Statics.gameMode == GameMode.Quest)
        {
            questable.IsPatternScroll = false;
            questable.QuestConfirm();
        }
        yield return new WaitForSeconds(2.0f);
        autoRot = false;
    }

    /// <summary>
    /// 패턴컬러 선택
    /// </summary>
    /// <param name="index"></param>
    public override void TriggerOn(int index)
    {
        if (isRot)
        {
            return;
        }

        for (int i = 0; i < childObjects.Count; i++)
        {
            if (childObjects[i] == currrentColorObject)
            {
                currentNumber = i;
            }
        }

        for (int i = 0; i < childObjects.Count; i++)
        {
            if (childObjects[i] == colorObjects[index])
            {
                newNumber = i;
            }
        }

        RotatePatternColor();
    }

    /// <summary>
    /// 선택한 패턴컬러쪽으로 회전
    /// </summary>
    private void RotatePatternColor()
    {
        StopCoroutine(IRotatePatternColor());
        StartCoroutine(IRotatePatternColor());
    }

    IEnumerator IRotatePatternColor()
    {
        isRot = true;
    
        PatternColorConverter patternColorConverter = childObjects[currentNumber].GetComponent<PatternColorConverter>();
    
        int difference = newNumber - currentNumber;
        if (difference < 0)
        {
            while (time <= 1)
            {
                time += (Time.deltaTime * 5) / -difference;
                center.transform.rotation = Quaternion.Euler(0, 0, quaternions[patternColorConverter.ColorData.id] + (difference * animationCurve.Evaluate(time)));
                yield return null;
            }
        }
        else if (0 < difference)
        {
            while (time <= 1)
            {
                time += (Time.deltaTime * 5) / difference;
                center.transform.rotation = Quaternion.Euler(0, 0, quaternions[patternColorConverter.ColorData.id] + (difference * animationCurve.Evaluate(time)));
                yield return null;
            }
        }
        else if (difference == 0)
        {
            yield return null;
        }

        if (Statics.gameMode == GameMode.Quest)
        {
            questable.QuestConfirm();
        }

        time = 0;
        isRot = false;
    }

    public void playAnim(bool autoRot)
    {
        this.autoRot = autoRot;
    }
    /// <summary>
    /// 리스트 재분배 -- 수정 필요
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private List<GameObject> ChildNumbers(int index)
    {
        List<GameObject> childObjects = new List<GameObject>();

        switch (index)
        {
            case 0:
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                break;
            case 1:
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                break;
            case 2:
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                break;
            case 3:
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                break;
            case 4:
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                break;
            case 5:
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                break;
            case 6:
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                break;
            case 7:
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                break;
            case 8:
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                break;
            case 9:
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                break;
            case 10:
                childObjects.Add(colorObjects[7]);
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                break;
            case 11:
                childObjects.Add(colorObjects[8]);
                childObjects.Add(colorObjects[9]);
                childObjects.Add(colorObjects[10]);
                childObjects.Add(colorObjects[11]);
                childObjects.Add(colorObjects[0]);
                childObjects.Add(colorObjects[1]);
                childObjects.Add(colorObjects[2]);
                childObjects.Add(colorObjects[3]);
                childObjects.Add(colorObjects[4]);
                childObjects.Add(colorObjects[5]);
                childObjects.Add(colorObjects[6]);
                childObjects.Add(colorObjects[7]);
                break;
        }
        return childObjects;
    }
}