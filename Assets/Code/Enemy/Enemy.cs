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

    public bool isChase; // 플레이어 추적중

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        //머테리얼 - 메쉬렌더러는 몬스터 오브젝트 위치 및 타입에 따라
        //몬스터별로 타입 다르게 지정하기
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
            curHealth -= bullet.damage; //불렛의 데미지 불러와서 체력 감소
            //Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject); //적과 충돌한 불렛은 삭제
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
            //다른 레이어와 교류하지 않게 설정해둔 레이어(EnemyDead)로 변경
            gameObject.layer = 12; //죽은 뒤 충돌효과 X
            isChase = false; //추척 비활성화
            nav.enabled = false; //죽었을 때 내비메쉬 비활성화
            anim.SetTrigger("isDie");

/*            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rb.AddForce(reactVec * 5, ForceMode.Impulse);*/

            Destroy(gameObject, 3);
        }
    }
}
