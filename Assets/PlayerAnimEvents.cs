using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();//�ӵ�ǰ����ĸ��������в��ҹ����� Player �ű�����������������ø�ֵ������
    }

     private void AnimationTrigger()
    {
        player.AttackOver();
    }

}
