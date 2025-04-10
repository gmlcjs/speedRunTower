using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    //public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinCountText;

    public int level;
    public int kill;
    //public int exp;
    //public int[] nextExp = { 3, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    public int coin;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { return; }
    }

    private void LateUpdate()
    {
        //levelText.text = string.Format("Lv.{0:F0}", level);
        coinCountText.text = string.Format("{0}", coin);
    }

/*    public void GetExp()
    {
        exp++;
        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }*/

    /* 관여 코드로 Enemy 코드에
    GameManager.instance.kill++;
    GameManager.instance.GetExp(); */
}
