using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Exp ���� PlayerBase �ڵ� �����ϱ�

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    //����Ű Space, ������ ���� Ű�� i, ���� ���� ���� Ű�� e,
    //���ݹ�ư�� LeftControl Ȥ�� ���콺 ���� Ű("Fire")
    //��� ����Ű�� 1, 2, 3, 4

    private Rigidbody rb;
    private Animator anim;
    public Camera followCamera;

    [Header("Character Control")]
    //����, �̵�
    public float moveSpeed = 1.0f;
    public float jumpForce = 5;
    public bool isGrounded;

    [Header("Weapon")] //���� ���� ���� ����Ʈ �ڵ�
    GameObject nearObject; //����� ������Ʈ �ݶ��̴� ������ �Ǻ��ϱ�
    Weapon equipWeapon; //���� �����ϰ� �ִ� ���� �ҷ�����
    int equipWeaponIndex = -1; //�����ϰ� �ִ� ���� â�� �ε���
    public GameObject[] weapons;
    public bool[] hasWeapons;
    bool sDown1; bool sDown2; bool sDown3; //���Կ� ���� ������ ��ȯ
    public GameObject[] weaponImages;

    [Header("Coin")] //���� ����
    public int coin;
    public int maxCoin; //���� �� ����
    public GameObject PressEKeyText;

    [Header("Potion")] //�̵��ӵ� ���� ���� ����
    public int potionCount;
    public int maxPotion; //���� �� ����
    public TextMeshProUGUI potionCountText;
    public int hasPotion;

    //���� �غ�, ���� �ӵ� ����
    bool isFireReady = true;
    float fireDelay;

    //�������� �ذ�
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
        //�������� ��ġ�� ��Ʈ 1
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        FreezeRotation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //���� �ڵ� �׶��� üũ
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
        
        //�ٰŸ� ���� Ű����
        if (Input.GetButtonDown("Fire1") && isFireReady && equipWeapon.type == Weapon.Type.Melee)
        {
            equipWeapon.Use();
            anim.SetTrigger("Attack");
        }

        //���Ÿ� ���� �߻�
        //�ٿ� ������ ������ �ڵ� ���� �߻�� �ٲٷ��� GetButton ���� ���� ����
        if (Input.GetButtonDown("Fire1") && isFireReady && equipWeapon.type == Weapon.Type.Range)
        {
            equipWeapon.Use();
            anim.SetTrigger("MagicShot");
        }
    }

    void Swap()
    {
        //���� ���� �ڵ�
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
        //������ �ִ� �±װ� ������ �� ���� ����
        if (Input.GetKeyDown(KeyCode.I) && nearObject != null && isGrounded) {
            if (nearObject.tag == "Weapon") {
                Item2 item = nearObject.GetComponent<Item2>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
        //������ �ִ� �±װ� ������ �� ���� ����
        if (Input.GetKeyDown(KeyCode.E) && nearObject.tag == "Store")
        {
            Store store = nearObject.GetComponent<Store>();
            store.Enter(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�±׿� �������� Ÿ������ Switch�� ����, ī��Ʈ �� ���� �� ����
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
                    //���� ���� �ڵ�
                    Inventory_Manager.instance.AddItem("" + item.value);
                    Destroy(other.gameObject); // ����
                    break;

                case Item2.Type.Potion:
                    potionCount++; hasPotion++;
                    potionCountText.text = potionCount.ToString();
                    Destroy(other.gameObject);
                    break;

                case Item2.Type.Material:
                    //��� ���� �ڵ�
                    Inventory_Manager.instance.AddItem("" + item.value);
                    Destroy(other.gameObject); // ����
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
