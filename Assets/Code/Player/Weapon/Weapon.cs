using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public Transform bulletPos; //불 마법 발생 지점
    public GameObject bullet; //불 마법 프리팹 지정

    //실행 순서 : Use의 메인루틴 -> Swing의 서브루틴 -> Use의 메인 루틴
    // Use의 메인루틴은 Swing의 코루틴과 함께 실행됨

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        if (type == Type.Range)
        {
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        //1
        yield return new WaitForSeconds(0.1f); //1프레임 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        //2
        yield return new WaitForSeconds(0.3f); //1프레임 대기
        meleeArea.enabled = false;
        //3
        yield return new WaitForSeconds(0.3f); //1프레임 대기
        trailEffect.enabled = false;
    }
    IEnumerator Shot()
    {
        //불 발사
        GameObject FireBullet = Instantiate(bullet,
                    bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = FireBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 50;

        yield return null;
    }
}
