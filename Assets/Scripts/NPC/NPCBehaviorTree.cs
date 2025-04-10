using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 🌟 모든 행동 트리 노드의 기본 클래스 (추상 클래스) 
public abstract class Node
{
    // 🌟 행동 트리를 실행하는 추상 메서드 (각 노드에서 구현)
    // 실행 시 성공하면 true, 실패하면 false 반환
    public abstract bool Execute();
}

// 🌟 선택 노드 (Selector) - 하나라도 성공하면 성공 (OR 로직) 
public class Selector : Node
{
    private List<Node> children = new List<Node>(); // 🌟 자식 노드 리스트

    // 🌟 자식 노드를 추가하는 메서드  
    public void AddChild(Node child) => children.Add(child);

    // 🌟 실행 메서드 - 하나라도 성공하면 true 반환
    public override bool Execute()
    {
        foreach (var child in children)
        {
            if (child.Execute()) return true; // 하나라도 성공하면 성공
        }
        return false; // 모든 자식이 실패하면 실패
    }
}

// 🌟 순차 노드 (Sequence) - 모든 노드가 성공해야 성공 (AND 로직)   
public class Sequence : Node
{
    private List<Node> children = new List<Node>(); // 🌟 자식 노드 리스트

    // 🌟 자식 노드를 추가하는 메서드
    public void AddChild(Node child) => children.Add(child);

    // 🌟 실행 메서드 - 하나라도 실패하면 전체 실패
    public override bool Execute()
    {
        foreach (var child in children)
        {
            if (!child.Execute()) return false; // 하나라도 실패하면 전체 실패
        }
        return true; // 모든 자식이 성공하면 성공
    }
}

// 🌟 NPC가 대기하는 행동 (항상 성공)
public class IdleNode : Node
{
    public override bool Execute()
    {
        Debug.Log("NPC가 대기 중...");
        return true;
    }
}

// 🌟 NPC가 주변에서 "Player" 태그를 가진 오브젝트를 감지하는 노드
public class FindNearbyPlayerNode : Node
{
    private Transform npc; // 🌟 NPC의 Transform
    private float detectRange; // 🌟 감지 범위 (예: 5m)
    public Transform TargetPlayer { get; private set; } // 🌟 감지된 플레이어
    public Vector3 LastKnownPosition { get; private set; } // 🌟 마지막으로 감지된 위치

    // 🌟 생성자 (NPC와 감지 범위를 설정)
    public FindNearbyPlayerNode(Transform npc, float detectRange)
    {
        this.npc = npc;
        this.detectRange = detectRange;
    }

    // 🌟 5m 안에 플레이어가 있으면 감지하고 위치 업데이트
    public override bool Execute()
    {
        Collider[] colliders = Physics.OverlapSphere(npc.position, detectRange);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player")) // 🌟 "Player" 태그가 있는 경우
            {
                TargetPlayer = collider.transform;
                LastKnownPosition = TargetPlayer.position; // 🌟 위치 저장
                return true;
            }
        }
        TargetPlayer = null;
        return false;
    }
}

// 🌟 NPC가 플레이어 위치로 이동하는 행동 (땅 위에서만 가능)
public class MoveToPlayerNode : Node
{
    private Transform npc; // 🌟 NPC Transform
    private Rigidbody npcRigidbody; // npc Rigidboy
    private LayerMask groundLayer; // 땅 레이아웃감지
    private FindNearbyPlayerNode findPlayerNode; // 🌟 플레이어 찾기 노드
    private Vector3 lastPosition; // 🌟 NPC의 마지막 위치 저장
    private float moveSpeed = 2f;

     public MoveToPlayerNode(Transform npc, Rigidbody npcRigidbody, FindNearbyPlayerNode findPlayerNode, LayerMask groundLayer)
    {
        this.npc = npc;
        this.npcRigidbody = npcRigidbody;
        this.findPlayerNode = findPlayerNode;
        this.groundLayer = groundLayer;
        this.lastPosition = npc.position; // 🌟 시작할 때 현재 위치 저장
    }
    // 🌟 실행 메서드 - 플레이어를 향해 이동
    public override bool Execute()
    {
        if (findPlayerNode.TargetPlayer == null) {ResetPosition(); return false; } // 🌟 플레이어가 없으면 이동 안 함

        if (IsOnGround()) // 🌟 땅 위에 있을 때만 이동
        {
            npc.position = Vector3.MoveTowards(npc.position, findPlayerNode.LastKnownPosition, Time.deltaTime * 2);
            return true;
        }else{
            ResetPosition();
        }

        return false;
    }
    // 🌟 NPC가 "Ground" 태그가 있는 땅 위에 있는지 확인
    private bool IsOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(npc.position, Vector3.down, out hit, 1.1f))
        {
            return hit.collider.CompareTag("Ground");
        }
        return false;
    }

    // 🌟 NPC가 원래 위치로 돌아가도록 설정
    private void ResetPosition()
    {
        Debug.Log("🛑 NPC가 플레이어를 찾지 못해 원래 위치로 돌아갑니다.");
        Vector3 direction = (lastPosition - npc.position).normalized;
        npcRigidbody.velocity = direction * moveSpeed; // 🌟 마지막 위치로 돌아가기
    } 
  
}

// 🌟 NPC가 "Player" 태그를 가진 대상을 공격하는 노드 (10cm 뒤로 밀어냄)
public class AttackNode : Node
{
    private Transform npc; // 🌟 NPC Transform
    private FindNearbyPlayerNode findPlayerNode; // 🌟 플레이어 찾기 노드
    private float attackRange; // 🌟 공격 범위

    public AttackNode(Transform npc, FindNearbyPlayerNode findPlayerNode, float attackRange)
    {
        this.npc = npc;
        this.findPlayerNode = findPlayerNode;
        this.attackRange = attackRange;
    }

    public override bool Execute()
    {
        if (findPlayerNode.TargetPlayer == null) return false; // 🌟 플레이어가 없으면 공격 안 함

        float distance = Vector3.Distance(npc.position, findPlayerNode.TargetPlayer.position);
        if (distance < attackRange) // 🌟 공격 범위 안에 있을 때만 공격
        {
            Debug.Log("NPC가 공격합니다!");

            // 🌟 플레이어를 10cm 뒤로 밀어냄
            Vector3 knockbackDirection = (findPlayerNode.TargetPlayer.position - npc.position).normalized;
            findPlayerNode.TargetPlayer.position += knockbackDirection * 0.1f; // 10cm(0.1m) 뒤로 이동

            return true;
        }
        return false;
    }
}

// 🌟 NPC의 행동을 제어하는 MonoBehaviour 스크립트
public class NPCBehaviorTree : MonoBehaviour
{
    private Node behaviorTree; // 🌟 행동 트리의 루트 노드
    private LayerMask groundLayer; // 🌟 땅 판별 레이어
    private FindNearbyPlayerNode findPlayerNode; // 🌟 플레이어 감지 노드
    private Rigidbody npcRigidbody; // 🌟 NPC의 Rigidbody

    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground"); // 🌟 "Ground" 태그 감지하는 레이어 설정
        findPlayerNode = new FindNearbyPlayerNode(transform, 5f); // 🌟 5m 안에서 플레이어 탐색
        npcRigidbody = GetComponent<Rigidbody>(); // 🌟 Rigidbody 가져오기

        // 🌟 행동 트리 구성
        Selector root = new Selector(); // 최상위 노드 (Selector)
        Sequence chaseAndAttack = new Sequence(); // 추적 + 공격 노드

        // 🌟 순차 실행: 플레이어 찾기 -> 이동 -> 공격
        chaseAndAttack.AddChild(findPlayerNode);
        chaseAndAttack.AddChild(new MoveToPlayerNode(transform, npcRigidbody, findPlayerNode, groundLayer));
        chaseAndAttack.AddChild(new AttackNode(transform, findPlayerNode, 1.5f));

        root.AddChild(chaseAndAttack); // 🌟 추적 + 공격 추가
        root.AddChild(new IdleNode()); // 🌟 기본적으로 대기

        behaviorTree = root; // 🌟 행동 트리 설정
    }

    void Update()
    {
        behaviorTree.Execute(); // 🌟 행동 트리 실행
    }

}
