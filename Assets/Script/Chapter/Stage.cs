using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sophia_Carriers;

public class Stage : MonoBehaviour
{
    public enum PORTAL_TYPE { NORMAL, BOSS, }
    public enum STAGE_CHILD { TILE, WALL, PORTAL, OBSTACLE, MOB, }
    //  StageGenerator //Stage[10];
    //  Stage[(int).Enum.Boss];
    //  Stage[(int).Enum.Boss];
    public StageGenerator stageGenerator;
    public MobGenerator mobGenerator;
    GachaComponent gachaComponent;

    [SerializeField]
    private int mStageNumber;
    public int StageNumber
    {
        get
        {
            return mStageNumber;
        }
        set
        {
            mStageNumber = value;
        }
    }
    [SerializeField]
    private string mType;
    public string Type
    {
        get
        {
            return mType;
        }
        set
        {
            mType = value;
        }
    }


    [SerializeField]
    private bool mIsClear;
    public bool IsClear
    {
        get { return mIsClear; }
        set { 
            mIsClear = value; 
            GameManager.Instance.GlobalEvent.OnStageClear.ForEach(e => e.Invoke(value));
        }
    }

    public void SetOnStage()
    {
        //GameManager.Instance.CurrentStage = this.gameObject;
        foreach (var m in mobGenerator.mobArray)
        {
            if (m != null)
            {
                if (m.tag != "Enemy")
                {
                    m.GetComponent<RaptorFlocks>().Chase(true);
                }
                else
                    m.GetComponent<Enemy>().isRecog = true;
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == (int)STAGE_CHILD.PORTAL)
                continue;
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetOffStage()
    {
        foreach (var m in mobGenerator.mobArray)
        {
            if (m != null)
            {
                if (m.tag != "Enemy")
                {
                    m.GetComponent<RaptorFlocks>().Chase(false);
                }
                else
                    m.GetComponent<Enemy>().isRecog = false;
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == (int)STAGE_CHILD.PORTAL)
                continue;
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void StageClear()
    {
        mIsClear = true;
        for (int i = 0; i < stageGenerator.portalArray.Length; i++)
        {
            if (stageGenerator.portalArray[i] == null)
                continue;
            stageGenerator.portalArray[i].GetComponent<Portal>().animator.SetBool("IsOpen", true);
        }
        if(Type == "normal")
        {
            gachaComponent.instantPivot.position = transform.position;
            gachaComponent.InstantiateReward(gachaComponent.instantPivot);
        }
        else if (Type == "boss")
        {

        }
        else if (Type == "hidden")
        {

        }
    }

    void Awake()
    {
        stageGenerator = GetComponent<StageGenerator>();
        mobGenerator = GetComponent<MobGenerator>();
        gachaComponent = GetComponent<GachaComponent>();
        mIsClear = false;
    }
    void Start()
    {
        stageGenerator.width = stageGenerator.initWidth;
        stageGenerator.increase = stageGenerator.initIncrease;
        if (Type == "normal")
        {
            stageGenerator.stageSizeRandom = Random.Range(1, 4);   //1, 2, 3
        }
        else
            stageGenerator.stageSizeRandom = 1;

        stageGenerator.width += stageGenerator.increase * stageGenerator.stageSizeRandom;
        stageGenerator.height = stageGenerator.width;
        stageGenerator.tileArray = new int[stageGenerator.width + 1, stageGenerator.height + 1];

        for (int i = 1; i <= stageGenerator.width; i++)
            for (int j = 1; j <= stageGenerator.height; j++)
                stageGenerator.tileArray[i, j] = 1;

        if (Type == "normal")
        {
            stageGenerator.SetFloorType();
        }

        stageGenerator.tileGameObjectArray = new GameObject[stageGenerator.width + 1, stageGenerator.height + 1];
        stageGenerator.InstantiateTile(stageGenerator.width, stageGenerator.height);
        stageGenerator.InstantiatePortal();
        stageGenerator.size = stageGenerator.width * stageGenerator.height;
        stageGenerator.GenerateNevMesh();
        switch (stageGenerator.stageSizeRandom)    //��ֹ� ����
        {
            case 1:
                stageGenerator.obstacleAmount = Random.Range(0, 11);
                break;
            case 2:
                stageGenerator.obstacleAmount = Random.Range(0, 21);
                break;
            case 3:
                stageGenerator.obstacleAmount = Random.Range(10, 31);
                break;
        }

        if (Type == "normal")
        {
            stageGenerator.InstantiateObstacle(stageGenerator.obstacleAmount);
            mobGenerator.InstantiateMob(mobGenerator.mobCount);
        }
        else if (Type == "shop")
        {
            float x = 0;
            float y = 0;
            float z = 0;
            GameObject instance;
            if (stageGenerator.PortalE)
            {
                y = 180f;
            }
            else if (stageGenerator.PortalW)
            {

            }
            else if (stageGenerator.PortalN)
            {
                y = 90f;
            }
            else if (stageGenerator.PortalS)
            {
                y = 270f;
            }
            instance = Instantiate(stageGenerator.shop, transform.position, Quaternion.Euler(x, y, z));
            instance.transform.parent = transform;
            StageClear();
        }
        else if (Type == "hidden")
        {

        }
        else if (Type == "boss")
        {
            GameObject instance;
            instance = Instantiate(mobGenerator.ElderOne, transform.position, Quaternion.identity);
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.MOB);
        }
        mobGenerator.CurrentMobCount = mobGenerator.mobArray.Count;
        if (mType == "start")
        {
            GameObject character = GameManager.Instance.PlayerGameObject;
            GameManager.Instance.CurrentStage = gameObject;
            StageClear();
            character.transform.position = new Vector3(transform.position.x, character.transform.position.y, transform.position.z);
        }
        else
        {
            SetOffStage();
        }
        transform.parent.GetComponent<ChapterGenerator>().LoadingStage++;   //�ε� �Ϸ�
    }
    void Update()
    {
        if (!mIsClear)
        {
            if (mobGenerator.CurrentMobCount == 0 && GameManager.Instance.CurrentStage == this.gameObject)
            {
                StageClear();
            }
        }
    }
}
