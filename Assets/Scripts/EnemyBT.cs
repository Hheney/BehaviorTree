/*
 * �� ��ũ��Ʈ�� Behavior Tree ����� �� AI�� �����մϴ�.
 * Start()���� Behavior Tree�� ����
 * Update()���� �� ������ root.Tick() ȣ��
 * Tick() ȣ�� ������ �� ��尡 �ڽ��� �۾��� �����ϰ� ���¸� ��ȯ
 */

using UnityEngine;

public class EnemyBT : MonoBehaviour
{
    [SerializeField] private Transform player;             //����/���� ���
    [SerializeField] private Transform[] waypoints;        //���� ���

    float fMoveSpeed = 3.0f;      //�̵� �ӵ�
    float fStopDistance = 0.2f;   //��������Ʈ ���� ���� �Ÿ�
    float fDetectRange = 6.0f;    //���� ���� �Ÿ�
    float fAttackRange = 2.0f;    //���� ��Ÿ�

    //�ܺ� ��忡�� ������ �� �ֵ��� �б� ���� ������Ƽ(getter)
    public Transform Player { get { return player; } }
    public Transform[] Waypoints { get { return waypoints; } }
    public float MoveSpeed { get { return fMoveSpeed; } }
    public float StopDistance { get { return fStopDistance; } }
    public float DetectRange { get { return fDetectRange; } }
    public float AttackRange { get { return fAttackRange; } }

    private Node root; //��Ʈ ���

    void Start()
    {
        /* [Ʈ�� ����]
         * Root(Selector)
         *  ���� Sequence( IsPlayerInAttackRange �� Attack )
         *  ���� Sequence( IsPlayerInDetectRange �� Chase )
         *  ���� Patrol
         *  ������ ������ ��Ȳ�̸� ���� ���� ���� �������� ���õǰ�,
         *  �׷��� ������ ����, �� �� �ƴ϶�� ������ �����Ѵ�.
         */

        //���� ������
        SequenceNode attackSequence = new SequenceNode();
        attackSequence.f_AddChild(new IsPlayerInAttackRange(this)); //���� ��� �߰�
        attackSequence.f_AddChild(new AttackNode(this));            //�ൿ ��� �߰�

        //���� ������
        SequenceNode chaseSequence = new SequenceNode();
        chaseSequence.f_AddChild(new IsPlayerInDetectRange(this));  //���� ��� �߰�
        chaseSequence.f_AddChild(new ChaseNode(this));              //�ൿ ��� �߰�

        //���� ���
        PatrolNode patrol = new PatrolNode(this); //�ൿ ���

        //������ ���(List�� �߰��ϴ� ������ �켱������ �ۿ��)
        SelectorNode rootSelector = new SelectorNode(); 
        rootSelector.f_AddChild(attackSequence);    //���� ������ �߰�
        rootSelector.f_AddChild(chaseSequence);     //���� ������ �߰�
        rootSelector.f_AddChild(patrol);            //���� ��� �߰�

        root = rootSelector; //��Ʈ ��� ����
    }

    void Update()
    {
        if (root != null)
        {
            root.Tick(); //�� �����Ӹ��� Ʈ���� �����Ѵ�
        }
    }

    //��ǥ �������� ���� �̵�(���� �ܼ�ȭ �߷�, NavMesh �̻�� ���� ���ǿ��� Ȯ��)
    public void f_MoveToTarget(Vector3 targetPosition)
    {
        //y��(����)�� ���� �� �����Ͽ� ���� �̵�
        Vector3 vTargetPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        //MoveTowards �޼ҵ� : � ��ġ���� ��ǥ ��ġ�� ������ �ӵ��� �̵��ϴ� ���
        transform.position = Vector3.MoveTowards(transform.position, vTargetPos, fMoveSpeed * Time.deltaTime);
    }
}
