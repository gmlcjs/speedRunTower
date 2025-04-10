using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Exp 관련 PlayerBase 코드 참고하기

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    //점프키 Space, 아이템 수집 키는 i, 상점 접촉 가능 키는 e,
    //공격버튼은 LeftControl 혹은 마우스 왼쪽 키("Fire")
    //장비 단축키는 1, 2, 3, 4

    private Rigidbody rb;
    private Animator anim;
    public Camera followCamera;

    [Header("Character Control")]
    //점프, 이동
    public float moveSpeed = 1.0f;
    public float jumpForce = 5;
    public bool isGrounded;

    [Header("Weapon")] //무기 습득 관련 리스트 코드
    GameObject nearObject; //가까운 오브젝트 콜라이더 범위로 판별하기
    Weapon equipWeapon; //내가 장착하고 있는 무기 불러오기
    int equipWeaponIndex = -1; //장착하고 있는 무기 창의 인덱스
    public GameObject[] weapons;
    public bool[] hasWeapons;
    bool sDown1; bool sDown2; bool sDown3; //슬롯에 따른 아이템 교환
    public GameObject[] weaponImages;

    [Header("Coin")] //코인 관련
    public int coin;
    public int maxCoin; //코인 총 갯수
    public GameObject PressEKeyText;

    [Header("Potion")] //이동속도 증가 포션 관련
    public int potionCount;
    public int maxPotion; //포션 총 갯수
    public TextMeshProUGUI potionCountText;
    public int hasPotion;

    //공격 준비, 공격 속도 조정
    bool isFireReady = true;
    float fireDelay;

    //물리문제 해결
    bool isBorder;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { return; }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Turn();
        Jump();
        Interation();
        Swap();
        Attack();
    }

    void FreezeRotation()
    {
        //물리문제 고치기 파트 1
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        FreezeRotation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //점프 코드 그라운드 체크
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("isJumping", false);
        }
    }
    void Turn()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position, nextVec);
            }
        }
    }
    void Walk()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            anim.SetBool("Walk", true);
            WalkMoving();
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
    void WalkMoving()
    {
        float xInput = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float zInput = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(xInput, 0, zInput);
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            anim.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Attack()
    {
        if (equipWeapon == null) return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        
        //근거리 공격 키워드
        if (Input.GetButtonDown("Fire1") && isFireReady && equipWeapon.type == Weapon.Type.Melee)
        {
            equipWeapon.Use();
            anim.SetTrigger("Attack");
        }

        //원거리 마법 발사
        //꾸욱 누르고 있으면 자동 마법 발사로 바꾸려면 GetButton 으로 수정 가능
        if (Input.GetButtonDown("Fire1") && isFireReady && equipWeapon.type == Weapon.Type.Range)
        {
            equipWeapon.Use();
            anim.SetTrigger("MagicShot");
        }
    }

    void Swap()
    {
        //무기 변경 코드
        if (Input.GetKeyDown("1") && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (Input.GetKeyDown("2") && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;

        int weaponIndex = -1;

        if (Input.GetKeyDown("1")) weaponIndex = 0;
        if (Input.GetKeyDown("2")) weaponIndex = 1;

        if (Input.GetKeyDown("1") || Input.GetKeyDown("2"))
        {
            if (equipWeapon != null) {
                equipWeapon.gameObject.SetActive(false); }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            weaponImages[weaponIndex].SetActive(true);
        }
    }

    void Interation()
    {
        //가까이 있는 태그가 무기일 시 무기 습득
        if (Input.GetKeyDown(KeyCode.I) && nearObject != null && isGrounded) {
            if (nearObject.tag == "Weapon") {
                Item2 item = nearObject.GetComponent<Item2>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
        //가까이 있는 태그가 상점일 시 상점 들어가기
        if (Input.GetKeyDown(KeyCode.E) && nearObject.tag == "Store")
        {
            Store store = nearObject.GetComponent<Store>();
            store.Enter(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //태그와 아이템의 타입으로 Switch문 구별, 카운트 및 코인 수 증가
        Item2 item = other.GetComponent<Item2>();

        if (other.tag == "Coin" || other.tag == "Potion" || other.tag == "Weapon")
        {
            switch (item.type)
            {
                case Item2.Type.Coin:
                    coin += item.value;
                    PlayerStatus.instance.coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    Destroy(other.gameObject);
                    break;

                case Item2.Type.Weapon:
                    //무기 습득 코드
                    Inventory_Manager.instance.AddItem("" + item.value);
                    Destroy(other.gameObject); // 삭제
                    break;

                case Item2.Type.Potion:
                    potionCount++; hasPotion++;
                    potionCountText.text = potionCount.ToString();
                    Destroy(other.gameObject);
                    break;

                case Item2.Type.Material:
                    //재료 습득 코드
                    Inventory_Manager.instance.AddItem("" + item.value);
                    Destroy(other.gameObject); // 삭제
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Store")
        {
            nearObject = other.gameObject;
        }

        if (nearObject.tag == "Store")
        {
            PressEKeyText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
        else if (other.tag == "Store")
        {
            Store store = other.GetComponent<Store>();
            store.Exit();
            nearObject = null;
            PressEKeyText.SetActive(false);
        }
    }
}
