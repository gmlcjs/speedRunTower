using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Store : MonoBehaviour
{
    public GameObject uiGroup;
    public Animator anim;

    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform[] itemPos;
    public string[] talkData;
    public TextMeshProUGUI talkText;

    PlayerController playerController;
    PlayerStatus playerStatus;

    public void Enter(PlayerController player)
    {
        uiGroup.SetActive(true);
        talkText.text = talkData[0];
    }


    public void Exit()
    {
        uiGroup.SetActive(false);
        anim.SetTrigger("doHello");
    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if (price > PlayerStatus.instance.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }
        PlayerStatus.instance.coin -= price; 
        StopCoroutine(Talk());
        StartCoroutine(Thank());
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3)
                        + Vector3.forward * Random.Range(-3, 3);
        Instantiate(itemObj[index], itemPos[index].position + ranVec,
            itemPos[index].rotation);
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(3f);
        talkText.text = talkData[0];
    }

    IEnumerator Thank()
    {
        talkText.text = talkData[2];
        yield return new WaitForSeconds(3f);
    }
}
