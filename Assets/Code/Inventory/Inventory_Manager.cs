using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Manager : MonoBehaviour
{
    public static Inventory_Manager instance;// 싱글톤 인스턴스


    [Header("UI 설정")]
    public GameObject InventoryItemPrepep ; // 인벤토리 UI 프리팹
    public GameObject DynamicCanvas; // 인벤토리 UI
    public GameObject[] ItemSlotUI; // 인벤토리 슬롯 UI

    LinkedList<Item_Information> weaponSlot = new LinkedList<Item_Information>(); // 무기 슬롯 배열 데이터
    LinkedList<Item_Information> potionSlot = new LinkedList<Item_Information>(); // 포션 슬롯 배열 데이터
    LinkedList<Item_Information> materialSlot = new LinkedList<Item_Information>(); // 재료 슬롯 배열 데이터
    //private int maxNumPerCell =  20; // 슬롯당 최대 아이템 수 


    
    private void Awake()
    {
        // 싱글톤 초기화
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("디버깅 시작");
    }

    // 아이템 획득 기본 1
    public void AddItem(string itemType){AddItem(itemType, 1);}  
    // 아이템 획득 
    public void AddItem(string itemType, int iCount){  // 아이템구분 , 아이템갯수 
        // 1: 하급무기(검) 2 :중급무기(검) 3 :상급무기(검)
        // 11: 하급무기(스태프) 12 :중급무기(스태프) 13 :상급무기(스태프)
        // 21: 하급포션(스피드) 22 :중급포션(스피드) 23:상급포션(스피드)
        // 31: 하급포션(점프력) 32 :중급포션(점프력) 33:상급포션(점프력)
        // 41: 맛있는 버섯 42: 두꺼운 거미줄 43 : 오크의 방망이 44 : 골렘의 근원 45 용 비늘 
        
        // Debug.Log("아이템 추가: " + itemType); // 아이템 추가 로그 출력

       
        // 아이템 스프라이트 이미지 이름
        switch (itemType)
        {
            case "1":
                weaponSlot.AddLast(new Item_Information(itemType,iCount)); // 1: 하급무기(검)
                break;
            case "2":
                weaponSlot.AddLast(new Item_Information(itemType,iCount)); //2 :중급무기(검)
                break;
            case "3":
                weaponSlot.AddLast(new Item_Information(itemType,iCount)); // 3 :상급무기(검)
                break;
            case "11":
                weaponSlot.AddLast(new Item_Information(itemType,iCount)); // 11: 하급무기(스태프)
                break;
            case "12":
                weaponSlot.AddLast(new Item_Information(itemType,iCount)); //  12 :중급무기(스태프)
                break;
            case "13":
                weaponSlot.AddLast(new Item_Information(itemType,iCount)); //  13 :상급무기(스태프)
                break;
            case "21":
                potionSlot.AddLast(new Item_Information(itemType,iCount)); // 21: 하급포션(스피드)
                break;
            case "22":
                potionSlot.AddLast(new Item_Information(itemType,iCount)); //  22 :중급포션(스피드)
                break;
            case "23":
                potionSlot.AddLast(new Item_Information(itemType,iCount)); // 23:상급포션(스피드)
                break;
            case "31":
                potionSlot.AddLast(new Item_Information(itemType,iCount)); // 31: 하급포션(점프력)
                break;
            case "32":
                potionSlot.AddLast(new Item_Information(itemType,iCount)); // 32 :중급포션(점프력)
                break;
            case "33":
                potionSlot.AddLast(new Item_Information(itemType,iCount)); // 33:상급포션(점프력)
                break;
            case "41":
                materialSlot.AddLast(new Item_Information(itemType,iCount)); // 41: 맛있는 버섯
                break;
            case "42":
                materialSlot.AddLast(new Item_Information(itemType,iCount)); // 42: 두꺼운 거미줄
                break;
            case "43":
                materialSlot.AddLast(new Item_Information(itemType,iCount)); // 43 : 오크의 방망이
                break;
            case "44":
                materialSlot.AddLast(new Item_Information(itemType,iCount)); // 44 : 골렘의 근원
                break;
            case "45":
                materialSlot.AddLast(new Item_Information(itemType,iCount)); // 45 용 비늘 
                break;
        }
        Debug.Log("아이템 슬롯 추가: " + itemType); // 아이템 슬롯 추가 로그 출력
        foreach (var slot in weaponSlot)
        {
            Debug.Log("무기 슬롯: " + slot.iKey); // 무기 슬롯 로그 출력
            Debug.Log("무기 이름: " + slot.iName); // 무기 슬롯 로그 출력
        }
        Debug.Log("무기 총갯수: " + weaponSlot.Count); // 포션 슬롯 추가 로그 출력
    }

    // 인벤토리창 조회
    public void ShowInventory(string itemType)
    {
        Debug.Log("디버깅 시작 2");
        // Debug.Log("인벤토리 조회: " + itemType); // 인벤토리 조회 로그 출력
        // 슬롯 초기화
        foreach (GameObject slot in ItemSlotUI)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject); // 기존 아이템 정보 오브젝트 제거
            }
        }
        LinkedList<Item_Information> currentList = null;
        // 아이템 타입 분기
        if (itemType == "weapon" || itemType == "1") currentList = weaponSlot;
        else if (itemType == "potion" || itemType == "2") currentList = potionSlot;
        else if (itemType == "material" || itemType == "3") currentList = materialSlot;
        
        // 인벤토리 UI 프리팹 생성
        int index = 0 ;
        foreach (var item in currentList)
        {

            if (index >= ItemSlotUI.Length) break; // 슬롯 UI 개수 초과 방지
    
            GameObject itemUI = Instantiate(InventoryItemPrepep, ItemSlotUI[index].transform); // 슬롯 UI에 아이템 UI 프리팹 생성

            // 하위 오브젝트 접근
            Image itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>(); // 아이템 이미지
            TextMeshProUGUI itemCountText = itemUI.transform.Find("ItemCount").GetComponent<TextMeshProUGUI>(); // 아이템 갯수
            TextMeshProUGUI Information = itemUI.transform.Find("Information").GetComponent<TextMeshProUGUI>(); // 아이템 정보
            TextMeshProUGUI itemName = itemUI.transform.Find("InformName").GetComponent<TextMeshProUGUI>(); // 아이템 이름


            // 아이템 정보 설정
            Sprite loadedSprite = item.image; // 아이템 스프라이트 로드

             Debug.Log("아이템 갯수 설정 : " + item.iCount.ToString()); // 아이템 스프라이트 로드 로그 출력
            itemCountText.text = item.iCount.ToString(); // 아이템 갯수 설정
            // Debug.Log("아이템 갯수 222 설정 : " + itemCountText.text); // 아이템 스프라이트 로드 로그 출력
            Information.text = item.iKey; // 아이템 정보 설정 key + type
            itemName.text = item.iName; // 아이템 이름 설정
            Debug.Log("itemName.text : " + itemName.text);
            
                
            if (loadedSprite != null)
                itemImage.sprite = loadedSprite;
            else
                Debug.LogWarning("스프라이트를 찾을 수 없습니다: " + item.path + item.iName);

            index++;
        }

        if(!DynamicCanvas.activeSelf) DynamicCanvas.SetActive(true); // 인벤토리 UI 활성화

    }

    // 인벤토리 UI 비활성화 (닫기)
    public void CloseInventory()
    {
        DynamicCanvas.SetActive(false); // 인벤토리 UI 비활성화
    }
}
