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

    public Transform bulletPos; //�� ���� �߻� ����
    public GameObject bullet; //�� ���� ������ ����

    //���� ���� : Use�� ���η�ƾ -> Swing�� �����ƾ -> Use�� ���� ��ƾ
    // Use�� ���η�ƾ�� Swing�� �ڷ�ƾ�� �Բ� �����

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
        yield return new WaitForSeconds(0.1f); //1������ ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        //2
        yield return new WaitForSeconds(0.3f); //1������ ���
        meleeArea.enabled = false;
        //3
        yield return new WaitForSeconds(0.3f); //1������ ���
        trailEffect.enabled = false;
    }
    IEnumerator Shot()
    {
        //�� �߻�
        GameObject FireBullet = Instantiate(bullet,
                    bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = FireBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 50;

        yield return null;
    }
}
