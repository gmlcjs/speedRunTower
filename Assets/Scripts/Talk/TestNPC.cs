using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : MonoBehaviour
{
    public TotalSTTGPT totalSTTGPT; // 대화 토탈관리 스크립트
    void OnTriggerStay(Collider other)
    {
        // 플레이어가 트리거 안에 들어와있을때 A키를 누르면 대화 시작
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("녹음실행");
            totalSTTGPT.OnRecordButtonClicked("4"); // 대화시작
        }
    }
}
