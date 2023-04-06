using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject map;
    public Image img;

    public int offsetX;
    public int offsetY;

    public void Print()
    {
        map = GameObject.Find("MapGenerator");
        int interval = (int)img.rectTransform.sizeDelta.x;
        float x = 0;
        float y = 0;
        for(int i = 1; i <= 15 * 15; i++)
        {

            Vector2 pos = new Vector2(x,y);
            if(!map.GetComponent<MapGenerator>().room[i].IsVacant())
            {
                Image tmp;
                tmp = Instantiate(img, transform);
                tmp.transform.localPosition = new Vector3(pos.x - 250, pos.y, 0);
                if(map.GetComponent<MapGenerator>().room[i].GetRoomType() == "start")
                    tmp.color = Color.green;
                else if (map.GetComponent<MapGenerator>().room[i].GetRoomType() == "boss")
                    tmp.color = Color.red;
                else if (map.GetComponent<MapGenerator>().room[i].GetRoomType() == "shop")
                    tmp.color = Color.blue;
            }

            if(i % 15 == 0)
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
        Print();
    }
}
