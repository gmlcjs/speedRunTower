using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// 음성 녹음, Google STT, ChatGPT, TTS를 통합하여 대화를 주고받는 기능을 담당하는 클래스
/// </summary>
public class TotalSTTGPT : MonoBehaviour
{
    [Header("UI 관리 설정")]
    public TextMeshProUGUI clientInputText; // 클라이언트 음성 텍스트 표시 UI
    public TextMeshProUGUI mikeState; // 마이크 상태 표시 UI
    public TextMeshProUGUI aiResponseText; // ChatGPT 응답 표시 UI
    public TMP_InputField questionAndAiText ; // 사용자가 AI에게 질문할 입력 필드 (현재 사용 안함 2025.04.01)

    [Header("설정관리")]
    public int recTime = 5; // 녹음 시간


    [Header("스크립트 정의")]
    public ChatGPT_UI chatGPTui; // ChatGPT API와 통신을 담당하는 스크립트
    public Mike mike; // 마이크 컨트롤러 스크립트
    public GoogleSTT googleSTT; // Google STT API와 통신을 담당하는 스크립트
    public SupertoneTTS supertoneTTS; // Supertone TTS API와 통신을 담당하는 스크립트

    /// <summary>
    /// 사용자가 녹음 버튼을 클릭하면 녹음 후 GPT가 응답하는 함수
    /// 1. 녹음만 실행 2. TTS실행 3. 녹음재생 및 TTS 실행 4. AI STT 실행 5. 클라이언트 STT실행 
    /// </summary>
    public void OnRecordButtonClicked(string onlyType)
    {
        if(onlyType == null || onlyType == ""){
            onlyType = "STT"; //텍스트만지원
        }
        StartCoroutine(OnRecordButtonClickedCoroutine(onlyType));
    }

    /// <summary>
    /// 녹음 → STT 변환 → ChatGPT 전송을 순차적으로 처리하는 코루틴
    /// </summary>
    private IEnumerator OnRecordButtonClickedCoroutine(string onlyType)
    {
        Debug.Log("대화 시작");
        // 1. 마이크 녹음 시작
        mike.RecSnd();
        yield return new WaitForSeconds(mike.recTime); // 녹음 완료 대기

       
        // 2. 녹음된 음성을 STT로 변환 (텍스트 출력만 수행)
        mike.PlaySnd(onlyType);
        yield return new WaitForSeconds(1.5f); // STT 처리 대기 (필요 시 조정 가능)

        // 3. 변환된 텍스트 가져오기
        string clientVoiceText = mike.ClientVoiceText();
        
        yield return new WaitForSeconds(1.5f); // TTS 처리 대기 (필요 시 조정 가능)

        if(onlyType == "5" || onlyType == "CTTS"){
            supertoneTTS.StartTTS(clientVoiceText); // 목소리 재생
        }
        
        // 4. ChatGPT에 텍스트 전송 후 응답 받기
        yield return StartCoroutine(chatGPTui.OnSendMessageCoroutine(clientVoiceText));

        yield return new WaitForSeconds(1.5f); // TTS 처리 대기 (필요 시 조정 가능)
        // 5. ChatGPT 응답을 TTS로 변환하여 음성 출력
        string aiVoiceText = chatGPTui.AiVoiceText();
        if(onlyType == "4" || onlyType == "TTS"){
            // yield return new WaitForSeconds(1.5f); // TTS 처리 대기 (필요 시 조정 가능)
            supertoneTTS.StartTTS(aiVoiceText); // 목소리 재생
        }
    }
    
}


/// IC 서버
/// IDC 
