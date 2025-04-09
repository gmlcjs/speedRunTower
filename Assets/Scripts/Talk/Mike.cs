using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Mike : MonoBehaviour
{
    public TotalSTTGPT totalSTTGPT; // UI 연결

    private AudioSource audioSource; // 마이크 오디오를 재생하는 AudioSource
    public int recTime; // 녹음 시간 (초)
    public TextMeshProUGUI clientInputText; // 클라이언트 음성 텍스트 표시 UI
    public TextMeshProUGUI mikeState; // 마이크 상태 표시 UI
    public string clientVoiceText = ""; // 인식된 텍스트 저장 (클라이언트가 이야기한 내용)
    public GoogleSTT googleSTT;



    void Start()
    {
        // AudioSource 컴포넌트가 없으면 자동 추가
        audioSource = gameObject.AddComponent<AudioSource>();
        clientInputText = totalSTTGPT.clientInputText; // 클라이언트 음성 텍스트 UI
        mikeState = totalSTTGPT.mikeState;  // 마이크 상태 UI
        recTime = totalSTTGPT.recTime; //녹음시작
    }

    /// <summary>
    /// 녹음된 오디오를 재생하거나 STT로 전송하는 함수
    /// </summary>
    /// <param name="onlyText">true이면 오디오 재생과 STT만 수행</param>
    public void PlaySnd(string onlyType)
    {
        if (audioSource.clip == null)
        {
            UpdateMikeState("재생할 오디오가 없습니다");
            return;
        }

        StopRecording(); // 녹음 중지
    
        Debug.Log("녹음 실행");

        if(onlyType == "1" || onlyType == "Play"){
            // 오디오 재생
            audioSource.Play(); // onlyText가 false일 때만 재생
        }else if(onlyType == "2" || onlyType == "STT" || onlyType == "5" || onlyType == "CTTS" || onlyType == "4" || onlyType == "TTS"){
            // 오디오 STT로 전송
            StartCoroutine(SendAudioToGoogleSTT(audioSource.clip));
        }else if(onlyType == "3" || onlyType == "PlayAndSTT"){
            // 오디오 재생 및 STT로 전송
            audioSource.Play(); //오디오 재생
            StartCoroutine(SendAudioToGoogleSTT(audioSource.clip));
        // }else if(onlyType == "4" || onlyType == "TTS"){
        //     // 오디오 재생 및 STT로 전송
        //     StartCoroutine(SendAudioToGoogleSTT(audioSource.clip));
        //     SupertoneTTS supertoneTTS = FindObjectOfType<SupertoneTTS>();
        //     if (supertoneTTS == null) Debug.LogError("SupertoneTTS 컴포넌트가 씬에 존재하지 않습니다.");
        //     supertoneTTS.StartTTS(aiVoiceText);
        }else {
            UpdateMikeState("오디오 데이터 타입을 지정해주세요");
        }

    }

    /// <summary>
    /// 마이크 녹음을 시작하는 함수
    /// </summary>
    public void RecSnd()
    {
        Debug.Log("녹음 시작");
        if (Microphone.devices.Length == 0)
        {
            UpdateMikeState("마이크 장치가 감지되지 않았습니다.");
            return;
        }
        StopRecording(); // 녹음 중지

        UpdateMikeState("말하는 중");
        audioSource.clip = Microphone.Start(Microphone.devices[0], false, recTime, 44100);
        StartCoroutine(RecordAndProcess());
    }

    /// <summary>
    /// 일정 시간이 지난 후 녹음을 중지하고 STT 처리하는 코루틴
    /// </summary>
    private IEnumerator RecordAndProcess()
    {
        yield return new WaitForSeconds(recTime);
        StopRecording();
    }

    /// <summary>
    /// 오디오 데이터를 Google STT로 전송하는 코루틴
    /// </summary>
    private IEnumerator SendAudioToGoogleSTT(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("STT로 전송할 오디오가 없습니다.");
            yield break;
        }
        UpdateMikeState("듣는중");

        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        byte[] byteData = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = (short)(samples[i] * short.MaxValue);
            byteData[i * 2] = (byte)(sample & 0xff);
            byteData[i * 2 + 1] = (byte)((sample >> 8) & 0xff);
        }

        
        if (googleSTT != null){
            
            googleSTT.SendAudioToGoogleSTT(byteData);
        }else
            Debug.LogError("GoogleSTT 스크립트를 찾을 수 없습니다.");
    }

    /// <summary>
    /// 플레이어 보이스 내용 텍스트
    /// </summary>
    public void UpdateClientText(string text)
    {
        // if (clientInputText != null)
        // {
            clientInputText.text = text;
            clientVoiceText = text;
        // }
        // else
        //     Debug.LogError("입력값이 설정되지 않았습니다.");
    }
    
    /// <summary>
    /// 마이크 상태 UI를 업데이트하는 함수
    /// </summary>
    public void UpdateMikeState(string text)
    {
        if (mikeState != null)
        {
            mikeState.lineSpacing = 10f;
            mikeState.text = text;
        }
        // else{
        //     mikeState.text = text;
        //     Debug.LogError("mikeState가 설정되지 않았습니다.");
        // }
    }

    /// <summary>
    /// 마이크 녹음을 중지하는 함수
    /// </summary>
    public void StopRecording()
    {
        if (Microphone.IsRecording(Microphone.devices[0]))
        {
            Microphone.End(Microphone.devices[0]);
        }
        else
        {
            // Debug.LogWarning("마이크가 녹음 중이 아닙니다.");
        }
        UpdateMikeState("쉬고있습니다.");
    }

    /// <summary>
    /// 클라이언트 목소리 텍스트 반환 함수
    /// </summary>
    public string ClientVoiceText()
    {
        Debug.Log("클라이언트 목소리 텍스트 반환 : " + clientVoiceText);
        return clientVoiceText;
    }

   

    
}
