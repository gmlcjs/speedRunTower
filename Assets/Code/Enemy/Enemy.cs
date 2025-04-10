using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public Transform target;

    Rigidbody rb;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;

    public bool isChase; // �÷��̾� ������

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        //���׸��� - �޽��������� ���� ������Ʈ ��ġ �� Ÿ�Կ� ����
        //���ͺ��� Ÿ�� �ٸ��� �����ϱ�
        //mat = GetComponent<MeshRenderer>().material;
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void ChaseStart()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (!isChase && distance <= 5f)
        {
            isChase = true;
            anim.SetBool("isWalk", true);
        }
        if (isChase)
        {
            nav.SetDestination(target.position);
        }

        if (isChase && distance > 5f)
        {
            isChase = false;
            anim.SetBool("isWalk", false);
        }
    }

    private void Update()
    {
        ChaseStart();
    }

    private void FixedUpdate()
    {
        FreezeVelocity();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            //Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage());
            Debug.Log("Enemy HP : "+curHealth);
        }
        else if (other.tag == "Bullet")
        {
            FireBullet bullet = other.GetComponent<FireBullet>();
            curHealth -= bullet.damage; //�ҷ��� ������ �ҷ��ͼ� ü�� ����
            //Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject); //���� �浹�� �ҷ��� ����
            StartCoroutine(OnDamage());
            Debug.Log("Enemy HP : " + curHealth);
        }
    }

    IEnumerator OnDamage()
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.gray;
            //�ٸ� ���̾�� �������� �ʰ� �����ص� ���̾�(EnemyDead)�� ����
            gameObject.layer = 12; //���� �� �浹ȿ�� X
            isChase = false; //��ô ��Ȱ��ȭ
            nav.enabled = false; //�׾��� �� ����޽� ��Ȱ��ȭ
            anim.SetTrigger("isDie");

/*            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rb.AddForce(reactVec * 5, ForceMode.Impulse);*/

            Destroy(gameObject, 3);
        }
    }
}
