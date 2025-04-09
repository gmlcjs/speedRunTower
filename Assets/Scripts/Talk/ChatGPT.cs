using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic; // List 사용을 위해 추가

/// <summary>
/// Unity에서 OpenAI의 ChatGPT API를 호출하여 대화를 주고받는 기능을 담당하는 클래스
/// </summary>
public class ChatGPT : MonoBehaviour
{
    public Mike mike; //마이크 클래스
    private string apiKey = SecretKey.OPEN_AI_API_KEY; // OpenAI API 키

    // OpenAI API의 엔드포인트 URL (v1 버전의 chat completions API 사용)
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    /// <summary>
    /// ChatGPT에 메시지를 보내고 응답을 받아오는 코루틴
    /// </summary>
    /// <param name="userMessage">사용자가 입력한 메시지</param>
    /// <param name="callback">ChatGPT의 응답을 받아 처리할 콜백 함수</param>
    /// <returns>코루틴을 실행하여 비동기적으로 API 요청을 수행</returns>
    public IEnumerator SendMessageToChatGPT(string userMessage, Action<string> callback)
    {
        Debug.Log("------------------" + userMessage);
        
        mike.UpdateMikeState("생각 중");
        if(userMessage == null || userMessage == ""){
            Debug.Log("사용자 질문 NULL");
            yield break;
        }
        
        userMessage += ", 단 30글자 이내로 설명해줘";
        // API 요청 데이터 생성
        var requestData = new RequestData
        {
            model = "gpt-3.5-turbo",  // 사용할 모델 지정 (gpt-4 또는 gpt-3.5-turbo)
            messages = new List<MessageData> { new MessageData { role = "user", content = userMessage } },
            max_tokens = 100  // 최대 응답 길이 설정 (토큰 수 제한)
        };

        // 요청 데이터를 JSON 형식으로 변환
        string jsonData = JsonUtility.ToJson(requestData);
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        // UnityWebRequest를 사용하여 OpenAI API에 HTTP POST 요청 전송
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);  // 요청 본문 설정
            request.downloadHandler = new DownloadHandlerBuffer();  // 응답 데이터를 받을 버퍼 설정
            request.SetRequestHeader("Content-Type", "application/json");  // JSON 데이터 전송을 위한 헤더 설정
            request.SetRequestHeader("Authorization", "Bearer " + apiKey); // API 키를 포함한 인증 헤더 추가

            // API 요청을 비동기적으로 전송하고 응답을 받을 때까지 대기
            yield return request.SendWebRequest();

            // 요청이 성공했을 경우 응답 처리
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseJson = request.downloadHandler.text; // 응답을 JSON 형식의 문자열로 받음
                OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(responseJson); // JSON 데이터를 객체로 변환

                // ChatGPT의 응답 메시지를 추출
                string chatGPTResponse = response.choices[0].message.content;
                // 콜백 함수 호출하여 결과 전달
                callback?.Invoke(chatGPTResponse);
            }
            else
            {
                // 요청 실패 시 오류 메시지를 출력
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}

// API 요청 데이터 구조
[Serializable]
public class RequestData
{
    public string model;
    public List<MessageData> messages; // List로 수정
    public int max_tokens;
}

// 메시지 데이터 구조
[Serializable]
public class MessageData
{
    public string role;
    public string content;
}

/// <summary>
/// OpenAI API 응답을 담을 데이터 모델
/// </summary>
[Serializable]
public class OpenAIResponse
{
    public Choice[] choices; // API 응답에서 "choices" 배열을 저장하는 필드
}

/// <summary>
/// ChatGPT 응답 메시지를 담는 클래스
/// </summary>
[Serializable]
public class Choice
{
    public Message message; // 응답 메시지를 포함하는 객체
}

/// <summary>
/// ChatGPT의 개별 메시지를 표현하는 클래스
/// </summary>
[Serializable]
public class Message
{
    public string content; // ChatGPT가 생성한 응답 텍스트
}
