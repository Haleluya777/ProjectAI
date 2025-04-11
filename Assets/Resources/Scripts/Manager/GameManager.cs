using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    //겜 매니저.
    //싱글톤 패턴으로 아무데서나 접근할 수 있게 함.
    //다른 매니저 클래스(이벤트 매니저, ui매니저 등)또한 변수로 접근하게 함으로써 싱글톤 패턴 더 안만들어도 됨.
    public static GameManager instance;

    public GameObject playerObj;

    public ObjectPoolingManager objectPoolManger_DmgTxt;
    public ObjectPoolingManager1 objectPoolManger_EffectTime;
    public EventManager eventManager;
    public PlayerUIManager uIManager;

    private WaitForSeconds battleTime = new WaitForSeconds(5f);
    public bool inBattle = false;
    private Coroutine inBattleCoroutine;
    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !inBattle)
        {
            Debug.Log("할렐루야!");
            uIManager.menuUIPanel.SetActive(!uIManager.menuUIPanel.activeSelf ? true : false);
        }
    }

    public void InBattleState()
    {
        if(!inBattle)
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
