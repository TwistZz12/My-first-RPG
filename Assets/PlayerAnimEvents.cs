using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();//从当前对象的父级对象中查找挂载了 Player 脚本的组件，并将其引用赋值给变量
    }

     private void AnimationTrigger()
    {
        player.AttackOver();
    }

}
