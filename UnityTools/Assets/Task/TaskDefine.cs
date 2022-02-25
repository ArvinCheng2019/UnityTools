using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Arvin.Task
{
    public enum NextType
    {
        [LabelText("确定")] OK,
        [LabelText("取消")] Cancel,
        [LabelText("分支_1")] Flag_1,
        [LabelText("分支_2")] Flag_21,
        [LabelText("分支_3")] Flag_3,
    }

    public enum TaskEvent
    {
        [LabelText("死亡")]Dead,
        [LabelText("成功")]Success,
        [LabelText("失败")]Failed,
    }


    public class TaskDefine
    {
    }

    [System.Serializable]
    public class TaskAttribute
    {
        [LabelText("生命")] public float HP;
        [LabelText("攻击力")] public float ATTACK;
        [LabelText("防御力")] public float Defense;
        [LabelText("幸运值")] public float Lucky;
    }
}