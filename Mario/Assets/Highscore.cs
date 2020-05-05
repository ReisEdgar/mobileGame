using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class Highscore : MonoBehaviour
{

    public Text Text;
    public Text Text1;
    public Text Text2;
    public Text Text3;
    public Text Text4;
    public Text Text5;
    public Text Text6;
    public Text Text7;
    public Text Text8;
    public Text Text9;
    public List<Text> List;
    public List<string> HighScoores;
    void Start()
    {
        HighScoores = new List<string>(10);
        for(int i = 0; i < 10; i++)
        {
            HighScoores.Add(PlayerPrefs.GetString(i.ToString()));
        }

        Text = transform.Find("Text").GetComponent<Text>();
        Text1 = transform.Find("Text (1)").GetComponent<Text>();
        Text2 = transform.Find("Text (2)").GetComponent<Text>();
        Text3 = transform.Find("Text (3)").GetComponent<Text>();
        Text4 = transform.Find("Text (4)").GetComponent<Text>();
        Text5 = transform.Find("Text (5)").GetComponent<Text>();
        Text6 = transform.Find("Text (6)").GetComponent<Text>();
        Text7 = transform.Find("Text (7)").GetComponent<Text>();
        Text8 = transform.Find("Text (8)").GetComponent<Text>();
        Text9 = transform.Find("Text (9)").GetComponent<Text>();


        List.Add(Text);
        List.Add(Text1);
        List.Add(Text2);
        List.Add(Text3);
        List.Add(Text4);
        List.Add(Text5);
        List.Add(Text6);
        List.Add(Text7);
        List.Add(Text8);
        List.Add(Text9);

    }

    void Update()
    {
            for (int i = 0; i < HighScoores.Count; i++)
            {
                List[i].text = HighScoores[i];
            }
    }
    public void Add(string str)
    {
        if (HighScoores == null)
        {
            HighScoores = new List<string>();
        }
        if (HighScoores.Count == 10)
        {
            var tmp = HighScoores.GetRange(1, 9);
            HighScoores = tmp;
            for (int i = 0; i < 9; i++)
            {
                PlayerPrefs.SetString(i.ToString(), HighScoores[i]);
            }
        }
        HighScoores.Add(str);
        PlayerPrefs.SetString((HighScoores.Count -1).ToString(), HighScoores.Last());

    }
}