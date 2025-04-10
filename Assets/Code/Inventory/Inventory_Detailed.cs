using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Detailed : MonoBehaviour
{
    [Header("Detailed UI 설정")]
    public GameObject ItemDetailUI; // 상세 프리팹


    [Header("아이템 정보")]
    public string itemInform =""; // 아이템타입,아이템명,고유번호
    public string itemType =""; // 아이템 구분
    public string iName  =""; // 아이템 명(프리팹 명)
    public Sprite itemImage; // 아이템 이미지
    // 인벤토리 상세 조회
    public void OnClickThisItem(GameObject gameObject) // 아이템 상세조회
    {
        // 자식오브젝트가 없으면 리턴
        if (gameObject.transform.childCount == 0) { 
            Debug.Log("자식 오브젝트가 없습니다."); // 자식 오브젝트가 없을 때 로그 출력
            ItemDetailUI.SetActive(false); 
            return;
        }

        // 자식 오브젝트 찾기 (예: 이름으로)


        // 아이템 정보 UI 설정
        if(!ItemDetailUI.activeSelf) ItemDetailUI.SetActive(true); // 상세 UI 활성화
        
        string name = gameObject.transform.GetChild(0).Find("InformName").GetComponent<TextMeshProUGUI>().text; // 아이템 이름 (한글)설정
        Debug.Log("아이템명 :" +name);
        if(name != "" && name != null) ItemDetailUI.transform.Find("DetailItemText").GetComponent<TextMeshProUGUI>().text = name;
        
        string inform = gameObject.transform.GetChild(0).Find("Information").GetComponent<TextMeshProUGUI>().text; // 아이템 정보
        if(inform != "" && inform != null) ItemDetailUI.transform.Find("ItemInform").GetComponent<TextMeshProUGUI>().text = inform;

        Sprite sprite = gameObject.transform.GetChild(0).Find("ItemImage").GetComponent<Image>().sprite; // 아이템 이미지 
        if(sprite != null) ItemDetailUI.transform.Find("DetailItemImage").GetComponent<Image>().sprite = sprite;
        
    }
    

}