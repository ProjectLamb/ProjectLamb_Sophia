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
                    tmp.color = Color.green;
                    startStageNumber = map.GetComponent<ChapterGenerator>().stage[i].StageNumber;
                }
                else if (map.GetComponent<ChapterGenerator>().stage[i].Type == "boss")
                    tmp.color = Color.red;
                else if (map.GetComponent<ChapterGenerator>().stage[i].Type == "shop")
                    tmp.color = Color.blue;
                else if (map.GetComponent<ChapterGenerator>().stage[i].Type == "middleboss")
                    tmp.color = Color.yellow;
                else if (map.GetComponent<ChapterGenerator>().stage[i].Type == "hidden")
                    tmp.color = Color.grey;
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
        imgList[imgDic[depart]].transform.GetChild(0).gameObject.SetActive(false);
        imgList[imgDic[arrive]].transform.GetChild(0).gameObject.SetActive(true);
        Color tmpColor = imgList[imgDic[arrive]].gameObject.GetComponent<Image>().color;
        tmpColor.a = 1;
        imgList[imgDic[arrive]].gameObject.GetComponent<Image>().color = tmpColor;
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
