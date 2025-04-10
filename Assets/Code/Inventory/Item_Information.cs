using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Information : MonoBehaviour
{
    public string iKey; // 고유번호
    public string itemType; // 아이템 구분
    public string iName; // 아이템 명(프리팹 명)
    public int iCount; // 아이템 갯수
    public string path = "data/"; // 경로
    public Sprite image; // 아이템 이미지


    public Item_Information(string itemType, int iCount) // 슬롯 생성자
    {
        this.itemType = itemType; // 슬롯 번호 설정
        this.iCount = iCount; // 슬롯 아이템 갯수 설정

        //  slotNum에 따른 슬롯 아이템 정보 넣기
        // 1: 하급무기(검) 2 :중급무기(검) 3 :상급무기(검)
        // 11: 하급무기(스태프) 12 :중급무기(스태프) 13 :상급무기(스태프)
        // 21: 하급포션(스피드) 22 :중급포션(스피드) 23:상급포션(스피드)
        // 31: 하급포션(점프력) 32 :중급포션(점프력) 33:상급포션(점프력)
        // 41: 맛있는 버섯 42: 두꺼운 거미줄 43 : 오크의 방망이 44 : 골렘의 근원 45 용 비늘 

        if(itemType == "1"){// 하급무기(검)
            this.iName = "하급무기검";
            this.iKey = "Weapon,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "2"){ // 중급무기(검)
            this.iName = "중급무기검";
            this.iKey = "Weapon,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "3"){ // 상급무기(검)
            this.iName = "상급무기검";
            this.iKey = "Weapon,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "11"){ // 하급무기(스태프)
            this.iName = "하급스태프";
            this.iKey = "Weapon," + Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if (itemType == "12")
        { // 중급무기(스태프)
            this.iName = "중급스태프";
            this.iKey = "Weapon," + Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if (itemType == "13")
        { // 상급무기(스태프)
            this.iName = "상급스태프";
            this.iKey = "Weapon," + Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if (itemType == "21")
        { // 하급포션(스피드)
            this.iName = "하급포션스피드";
            this.iKey = "Potion," + Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "22"){ // 중급포션(스피드)
            this.iName = "중급포션스피드";
            this.iKey = "Potion,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "23"){ // 상급포션(스피드)
            this.iName = "상급포션스피드";
            this.iKey = "Potion,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "31"){ // 하급포션(점프력)
            this.iName = "하급포션점프력";
            this.iKey = "Potion,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "32"){ // 중급포션(점프력)
            this.iName = "중급포션점프력";
            this.iKey = "Potion,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "33"){ // 상급포션(점프력)
            this.iName = "상급포션점프력";
            this.iKey = "Potion,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "41"){ // 맛있는 버섯
            this.iName = "맛있는버섯";
            this.iKey = "Material,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "42"){ // 두꺼운 거미줄
            this.iName = "두꺼운 거미줄";
            this.iKey = "Material,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "43"){ // 오크의 방망이
            this.iName = "오크의 방망이";
            this.iKey = "Material,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "44"){ // 골렘의 근원
            this.iName = "골렘의 근원";
            this.iKey = "Material,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }else if(itemType == "45"){ // 용 비늘
            this.iName = "용 비늘";
            this.iKey = "Material,"+Guid.NewGuid().ToString(); // 고유키
            this.image = Resources.Load<Sprite>(path + iName); // 아이템 스프라이트 로드
            this.iCount = iCount; // 아이템 갯수 설정
        }
        // 스프라이트 가 비여있을때 비였음 실행
        if (image == null)
        {
            this.image = Resources.Load<Sprite>(path + "NULL"); // NULL 스프라트
        }

    }


}
