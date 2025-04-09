using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System;

public class SupertoneTTS : MonoBehaviour
{
    public AudioSource audioSource; // 음성 재생용 AudioSource
    private string apiKey = SecretKey.SUPERTONE_API; // API 키 입력
    private string apiUrl = "https://supertoneapi.com/v1/text-to-speech/sogqLoeXGEFnUjjk4y8juY"; // 실제 엔드포인트 확인 필요

    public Mike mike; // mike 선언

    // TTS API를 호출하는 메서드
    public void StartTTS(string text)
    {
        StartCoroutine(GetTTS(text));
    }

    // TTS API를 호출하는 코루틴
    IEnumerator GetTTS(string text)
    {
        
        mike.UpdateMikeState("말하는중");
        // JSON 형식으로 요청 데이터 생성
        string jsonBody = "{\n" +
                  "  \"language\": \"ko\",\n" +
                  "  \"model\": \"turbo\",\n" +
                  "  \"voice_settings\": {\n" +
                  "    \"speed\": 1\n" +
                  "  },\n" +
                  "  \"text\": \"" + text + "\"\n" +
                  "}";

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("x-sup-api-key", apiKey); // API 키가 제대로 설정되었는지 확인
            request.SetRequestHeader("Content-Type", "application/json");
            
            // 요청 전송 및 응답 대기
            yield return request.SendWebRequest();

            // 오류 처리
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TTS API 오류: " + request.error);
            }
            else
            {
                // 받은 오디오 파일을 저장 후 재생
                // string filePath = Path.Combine(Application.persistentDataPath, "output.wav");
                string filePath = Path.Combine("D:/유니티 테스트", DateTime.Now.ToString("yyyyMMddHHmmss") + "output" + ".wav");
                File.WriteAllBytes(filePath, request.downloadHandler.data);

                 // 파일이 다 저장된 후 PlayAudio를 호출
                if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
                // 오디오 할당
                StartCoroutine(PlayAudio(filePath));
            }
            mike.UpdateMikeState("뭘도와드릴까요.");
        }
    }

    // 오디오 파일을 재생하는 메서드
    IEnumerator PlayAudio(string filePath)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.WAV))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("오디오 로드 실패: " + request.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}

/**
    연령대 설정 태그 4가지 구성
    Age
    child(어린이), young-adult(젊은), middle-aged(중년), elder(노인)

    성병 태그 2가지 구성
    Gender
    male(남자), female(여자)

    Use Case
    추천사용 태그 6가지 구성
    advertisement(광고), announcement(공지), audiobook(오디오북), documentary(다큐멘터리), education(교육), game(게임)

    Language
    언어 태그 3가지 구성
    ko(한국), ja(일본), en(엉어)

    목소리 출력 URL
    private string apiUrl = "https://supertoneapi.com/v1/text-to-speech/" + voice_id; // 슈퍼톤 URL경로
**/
