using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class GoogleSTT : MonoBehaviour
{
    private string apiKey;
    public Mike mike;
    // Mike mike = FindObjectOfType<Mike>()
    
    void Start()
    {
        apiKey = GetApiKey(); // API 키를 보안 강화된 방식으로 가져옴
    }

    // 보안 강화된 방식으로 API 키를 가져오는 메서드
    private string GetApiKey()
    {
        return SecretKey.GOOGLE_TTS_API_KEY; // 추후 환경 변수 또는 보안 Vault 사용 고려
    }

    // 음성 인식 결과를 처리하는 메서드
    public void SendAudioToGoogleSTT(byte[] audioData)
    {
        if (apiKey == null)
        {
            Debug.LogError("API 키가 설정되지 않았습니다.");
            return;
        }

        string url = "https://speech.googleapis.com/v1/speech:recognize?key=" + apiKey;

        // 요청 바디 작성
        string json = $"{{\"config\":{{\"encoding\":\"LINEAR16\",\"sampleRateHertz\":44100,\"languageCode\":\"ko-KR\"}},\"audio\":{{\"content\":\"{System.Convert.ToBase64String(audioData)}\"}}}}";
        byte[] postData = Encoding.UTF8.GetBytes(json);

        // UnityWebRequest를 사용하여 POST 요청
        StartCoroutine(SendRequest(url, postData));
    }

    private IEnumerator SendRequest(string url, byte[] postData)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                try
                {
                    var json = JsonUtility.FromJson<GoogleSTTResponse>(jsonResponse);
                    if (json.results != null && json.results.Length > 0 && json.results[0].alternatives.Length > 0)
                    {
                        string clientVoiceText = json.results[0].alternatives[0].transcript; // 클라이언트 보이스 글자변환
                        
                        // clientVoiceText += "단, 답변은 한국어로 30글자이내로 설명해줘"; // 짧게 대답하게 하기 위해 빈 문자열 추가

                        if (mike != null) mike.UpdateClientText(clientVoiceText); // 클라이언트 대화 text 저장
                    }
                    else
                    {
                        Debug.LogWarning("음성을 인식할 수 없습니다.");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON 파싱 오류: " + e.Message);
                }
            }
            else
            {
                Debug.LogError($"Google STT Error: {request.error}");
            }
        }
    }
}

// Google STT 응답을 파싱하기 위한 클래스
[System.Serializable]
public class GoogleSTTResponse
{
    public GoogleSTTResult[] results;
}

[System.Serializable]
public class GoogleSTTResult
{
    public GoogleSTTAlternative[] alternatives;
}

[System.Serializable]
public class GoogleSTTAlternative
{
    public string transcript;
}
