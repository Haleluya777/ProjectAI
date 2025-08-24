using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDataUI : MonoBehaviour
{
    private ISkillDataAccessable skillDataAccessable;
    [SerializeField] private Text skillNameTxt;
    [SerializeField] private Text skillDetailTxt;

    private int uiNum;

    private new RectTransform transform;
    float width, height;

    void Awake()
    {
        skillDataAccessable = GameManager.instance.playerObj.GetComponent<ISkillDataAccessable>();
        transform = this.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        width = transform.rect.width;
        height = transform.rect.height;
    }

    public void SettingPosition()
    {
        this.gameObject.SetActive(true);

        Debug.Log(skillDataAccessable == null);
        uiNum = int.Parse(transform.parent.name.Split('_')[1]);

        skillNameTxt.text = skillDataAccessable.AccessSkillData[uiNum].SkillName;
        skillDetailTxt.text = skillDataAccessable.AccessSkillData[uiNum].SkillDetail;

        transform.anchoredPosition = new Vector2(width / 2, height / 2);
    }

    public void MouseExit()
    {
        this.gameObject.SetActive(false);
    }
}
