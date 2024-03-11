using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sophia_Carriers;
using System.Drawing;
using Sophia.Entitys;

public class Stage : MonoBehaviour
{
    #region Enum Members
    public enum PORTAL_TYPE { NORMAL, BOSS, }
    public enum STAGE_CHILD { TILE, WALL, PORTAL, OBSTACLE, MOB, }
    #endregion
    //  StageGenerator //Stage[10];
    //  Stage[(int).Enum.Boss];
    //  Stage[(int).Enum.Boss];

    //private화 하기
    public StageGenerator stageGenerator;
    public MobGenerator mobGenerator;
    public Sophia.Instantiates.GachaComponent gachaComponent;
    //GachaComponent gachaComponent;

    #region Serial Member
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
        set
        {
            mIsClear = value;
            if (value == true)
            {
                GameManager.Instance.GlobalEvent.OnStageClear.ForEach(e => e.Invoke(this));
            }
        }
    }

    public int stageSizeRandom;
    #endregion

    void Awake()
    {
        TryGetComponent<StageGenerator>(out stageGenerator);
        TryGetComponent<MobGenerator>(out mobGenerator);
        TryGetComponent<Sophia.Instantiates.GachaComponent>(out gachaComponent);
        //TryGetComponent<GachaComponent>(out gachaComponent);
        IsClear = false;
    }

    void Start()
    {
        System.Random rand = new System.Random();

        if (Type == "normal")
        {
            stageSizeRandom = rand.Next(1, 4);
        }
        else
        {
            stageSizeRandom = 1;
        }

        stageGenerator.InitStageGenerator(stageSizeRandom);

        if (Type == "normal")
        {
            stageGenerator.SetFloorType();
        }

        stageGenerator.InstantiateTile();
        stageGenerator.InstantiateWall();
        stageGenerator.InstantiatePortal();
        stageGenerator.GenerateNevMesh();

        if (Type == "normal")
        {
            stageGenerator.InstantiateObstacle(stageGenerator.obstacleAmount);
            mobGenerator.InitMobGenerator();
            mobGenerator.InstantiateMob();
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
        if (!IsClear)
        {
            if (mobGenerator.CurrentMobList.Count == 0 && GameManager.Instance.CurrentStage == this.gameObject)
            {
                StageClear();
            }
        }
    }

    public void SetOnStage()
    {
        //GameManager.Instance.CurrentStage = this.gameObject;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == (int)STAGE_CHILD.PORTAL)
                continue;
            transform.GetChild(i).gameObject.SetActive(true);
        }
        mobGenerator.SetMobsMovementOn();
    }

    public void SetOffStage()
    {
        mobGenerator.SetMobsMovementOff();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == (int)STAGE_CHILD.PORTAL)
                continue;
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void StageClear()
    {
        IsClear = true;
        for (int i = 0; i < stageGenerator.portalArray.Length; i++)
        {
            if (stageGenerator.portalArray[i] == null)
                continue;
            stageGenerator.portalArray[i].GetComponent<Portal>().animator.SetBool("IsOpen", true);
        }
        if (Type == "normal")
        {
            // gachaComponent.instantPivot.position = transform.position;
            // gachaComponent.InstantiateReward(gachaComponent.instantPivot);
            List<Sophia.Instantiates.ItemObject> positionedItem = gachaComponent.InstantiateReward();
            if(positionedItem.Count == 0) return;
            foreach(var item in gachaComponent.InstantiateReward()) {
                item.Activate();
            }
        }
        else if (Type == "boss")
        {

        }
        else if (Type == "hidden")
        {

        }
    }
}
