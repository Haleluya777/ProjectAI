using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    //겜 매니저.
    //싱글톤 패턴으로 아무데서나 접근할 수 있게 함.
    //다른 매니저 클래스(이벤트 매니저, ui매니저 등)또한 변수로 접근하게 함으로써 싱글톤 패턴 더 안만들어도 됨.
    public static GameManager instance;

    public GameObject enemyObj;
    public GameObject playerObj;

    public ObjectPoolingManager objectPoolManger_DmgTxt;
    public ObjectPoolingManager1 objectPoolManger_EffectTime;
    public EventManager eventManager;
    public PlayerUIManager uIManager;
    public DialogueFuncManager dialogueFunc;

    private WaitForSeconds battleTime = new WaitForSeconds(5f);
    public bool inBattle = false;
    private Coroutine inBattleCoroutine;
    public IBlackBoard globalBlackBoard = new BlackBoard(); //공용 블랙보드.
    public Dictionary<string, object> operandDic = new Dictionary<string, object>();
    public int testCondition = 5;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }

        OperandInit(operandDic);
        dialogueFunc.InitMethods();
        globalBlackBoard.Set<Transform>("PlayerCenter", playerObj.transform.GetChild(2).transform);
    }

    private void OperandInit(Dictionary<string, object> dic)
    {
        dic.Add("Level", 15);
    }

    void Update()
    {
        UpdateDataPerFrame();
        if (Input.GetKeyDown(KeyCode.Escape) && !inBattle)
        {
            uIManager.menuUIPanel.SetActive(!uIManager.menuUIPanel.activeSelf ? true : false);
        }
    }

    private void UpdateDataPerFrame() //매 프레임당 업데이트 해야 할 공용 블랙보드 데이터
    {
        globalBlackBoard.Set<Transform>("PlayerTransform", playerObj.transform);

    }

    public void InBattleState()
    {
        if (!inBattle)
        {
            inBattle = true;
            uIManager.menuUIPanel.SetActive(false);
            inBattleCoroutine = StartCoroutine(EndBattleState());
        }
        else
        {
            StopCoroutine(inBattleCoroutine);
            inBattleCoroutine = StartCoroutine(EndBattleState());
        }
    }

    IEnumerator EndBattleState()
    {
        yield return new WaitForSeconds(5f);
        inBattle = false;
    }
}
