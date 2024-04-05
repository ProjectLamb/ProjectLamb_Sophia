using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject map;
    public Image img;
    Dictionary<int, int> imgDic;
    List<Image> imgList;

    public int offsetX;
    public int offsetY;
    public bool BlackSheepWall;
    int startStageNumber;

    public void Print()
    {
        map = GameManager.Instance.ChapterGenerator;
        int interval = (int)img.rectTransform.sizeDelta.x;
        float x = 0;
        float y = 0;
        for (int i = 1; i <= 15 * 15; i++)
        {
            Vector2 pos = new Vector2(x, y);
            if (!map.GetComponent<ChapterGenerator>().stage[i].Vacancy)
            {
                imgDic.Add(map.GetComponent<ChapterGenerator>().stage[i].StageNumber, imgList.Count);
                Image tmp;
                tmp = Instantiate(img, transform);
                tmp.transform.localPosition = new Vector3(pos.x - 250, pos.y, 0);
                if (map.GetComponent<ChapterGenerator>().stage[i].Type == "start")
                {
                    tmp.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    startStageNumber = map.GetComponent<ChapterGenerator>().stage[i].StageNumber;
                }
                else if (map.GetComponent<ChapterGenerator>().stage[i].Type == "boss")
                {
                    tmp.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                }
                else if (map.GetComponent<ChapterGenerator>().stage[i].Type == "shop")
                {
                    tmp.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                }
                Color tmpAlpha = tmp.transform.GetChild(0).GetComponent<Image>().color;
                tmpAlpha.a = 0.4f;
                tmp.transform.GetChild(0).GetComponent<Image>().color = tmpAlpha;
                imgList.Add(tmp);
            }

            if (i % 15 == 0)
            {
                x = 0;
                y += interval;
            }
            else
            {
                x += interval;
            }
        }
    }

    void Start()
    {
        imgList = new List<Image>();
        imgDic = new Dictionary<int, int>();
        Print();
        if (!BlackSheepWall)
        {
            foreach (Image img in imgList)
            {
                img.gameObject.SetActive(false);
            }
        }
        imgList[imgDic[startStageNumber]].gameObject.SetActive(true);
        ChangeCurrentPosition(startStageNumber, startStageNumber);
    }

    public void ChangeCurrentPosition(int depart, int arrive)
    {
        imgList[imgDic[depart]].transform.GetChild(1).gameObject.SetActive(false);
        if (GameManager.Instance.ChapterGenerator.GetComponent<ChapterGenerator>().stage[depart].Discovered)
        {
            // if(GameManager.Instance.ChapterGenerator.GetComponent<ChapterGenerator>().stage[depart].Type == "hidden")
            // {
            //     imgList[imgDic[depart]].GetComponent<Image>().transform.GetChild(0).GetComponent<Image>().color = Color.magenta;
            // }
            Color departColor = imgList[imgDic[depart]].GetComponent<Image>().transform.GetChild(0).GetComponent<Image>().color;
            departColor.a = 0.7f;
            imgList[imgDic[depart]].GetComponent<Image>().transform.GetChild(0).GetComponent<Image>().color = departColor;
        }
        imgList[imgDic[arrive]].transform.GetChild(1).gameObject.SetActive(true);
        Color arriveColor = imgList[imgDic[arrive]].gameObject.GetComponent<Image>().transform.GetChild(0).GetComponent<Image>().color;
        if (GameManager.Instance.ChapterGenerator.GetComponent<ChapterGenerator>().stage[arrive].Type == "hidden")
        {
            arriveColor = Color.magenta;
        }
        arriveColor.a = 1;
        imgList[imgDic[arrive]].gameObject.GetComponent<Image>().transform.GetChild(0).GetComponent<Image>().color = arriveColor;

        if (!BlackSheepWall)
        {
            if (map.GetComponent<ChapterGenerator>().stage[arrive].East != null)
                imgList[imgDic[map.GetComponent<ChapterGenerator>().stage[arrive].East.StageNumber]].gameObject.SetActive(true);
            if (map.GetComponent<ChapterGenerator>().stage[arrive].West != null)
                imgList[imgDic[map.GetComponent<ChapterGenerator>().stage[arrive].West.StageNumber]].gameObject.SetActive(true);
            if (map.GetComponent<ChapterGenerator>().stage[arrive].South != null)
                imgList[imgDic[map.GetComponent<ChapterGenerator>().stage[arrive].South.StageNumber]].gameObject.SetActive(true);
            if (map.GetComponent<ChapterGenerator>().stage[arrive].North != null)
                imgList[imgDic[map.GetComponent<ChapterGenerator>().stage[arrive].North.StageNumber]].gameObject.SetActive(true);
        }
    }
}
