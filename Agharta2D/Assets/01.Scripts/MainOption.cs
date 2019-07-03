using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
//유은호
public class MainOption : MonoBehaviour
{
    private int select = 0;
    public GameObject[] menu;

    void Start()
    {
        coloring();//시작하면 바로 선택된 메뉴(0) 색칠
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))//엔터를 쳤을시
        {
            switch (select)
            {
                case 0:
                    SceneManager.LoadScene("Game");//게임시작Scene
                    break;
                case 1:
                    SceneManager.LoadScene("Option");//게임옵션Scene
                    break;
                case 2:
                    Quit();
                    //게임종료
                    break;
            }
        }
        else if(Input.anyKeyDown)
        {
            MenuSelect();
            coloring();
        }
    }

    void MenuSelect()//메뉴선택
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))//메뉴선택 위 방향키 
        {
            select -= 1;
            if (select == -1)//맨위에서 한번더 위로 눌렀을시
            {
                select = 2;//맨아래 선택지로 내려감
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))//메뉴선택 아래 방향키 
        {
            select += 1;
            if (select == 3)//맨위에서 한번더 아래 눌렀을시
            {
                select = 0;//맨위 선택지로 올라감
            }
        }
    }

    void coloring()//선택되있는 메뉴 표시
    {
        for(int i=0; i<menu.Length; i++)//선택되있는 메뉴확인 및 텍스트 색 변경 ※추후 메뉴옆 화살표 띄우는것으로 변경 예정
        {
            if(select == i)
            {
                menu[i].GetComponent<TextMeshProUGUI>().color = Color.red;
            }
            else
            {
                menu[i].GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    }
}
