using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// Unity UI를 통해 ChatGPT와 상호작용하는 클래스
/// </summary>
public class ChatGPT_UI : MonoBehaviour
{
    public TotalSTTGPT totalSTTGPT; // UI 연결
    
    public ChatGPT chatGPT; // ChatGPT API와 통신을 담당하는 스크립트
    public TMP_InputField questionAndAiText ; // 사용자가 AI에게 질문할 입력 필드
    public TextMeshProUGUI aiResponseText; // ChatGPT 응답 표시 UI
    public string aiVoiceText = ""; // 인식된 텍스트 저장 (GTP가 대답한 내용)

    void Start()
    {
        aiResponseText = totalSTTGPT.aiResponseText; // ChatGPT 응답 표시 UI
        questionAndAiText = totalSTTGPT.questionAndAiText; // 사용자 입력 필드
    }

    /// <summary>
    /// 사용자가 메시지를 입력하고 "전송" 버튼을 눌렀을 때 실행되는 함수
    /// </summary>
    public void OnSendMessage()
    {
        string userMessage = questionAndAiText.text;
        StartCoroutine(chatGPT.SendMessageToChatGPT(userMessage, OnResponseReceived));
    }

    /// <summary>
    /// ChatGPT에 메시지를 비동기적으로 전송하는 코루틴
    /// </summary>
    public IEnumerator OnSendMessageCoroutine(string message)
    {
        yield return new WaitForSeconds(1); // 예제 코드: 1초 대기
        StartCoroutine(chatGPT.SendMessageToChatGPT(message, OnResponseReceived));
    }

    /// <summary>
    /// ChatGPT 응답을 받아 UI에 표시하는 함수
    /// </summary>
    private void OnResponseReceived(string response)
    {
        aiResponseText.text = response;
        aiVoiceText = response;
    }

    /// <summary>
    /// 클라이언트 목소리 텍스트 반환 함수
    /// </summary>
    public string AiVoiceText()
    {
        return aiVoiceText;
    }

}
   