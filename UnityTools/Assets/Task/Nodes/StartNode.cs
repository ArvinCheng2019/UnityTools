using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;
using UnityEditor;

namespace Arvin.Task
{
    public class StartNode : Node
    {
        [LabelText("编辑_任务ID")] public string TaskID;
        [LabelText("开始标题")] public string ShowTitle;

        [PreviewField(Alignment = ObjectFieldAlignment.Left), LabelText("开始的图片")]
        [OnValueChanged("OnSpriteValueChanged")]
        public Sprite Sprite;

        public void OnSpriteValueChanged()
        {
            SpriteAddressable = Sprite.name;
        }

        [LabelText("程序用图地址"), ShowIf("Sprite"), ReadOnly]
        public string SpriteAddressable;

        [Input, LabelText("初始属性")] public TaskAttribute TaskAttribute;

        [Output, LabelText("点击确定")] public BaseNode ok;
        [Output, LabelText("点击取消")] public BaseNode cancel;
    }
}