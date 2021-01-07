using System;
using UnityEngine;
using UnityEngine.UI;

public class FrameAnimator : MonoBehaviour
{
    private createRemote remote;
    public Sprite[] frames = null;

    public Sprite[] Frames
    {
        get => frames;
        set => frames = value;
    }

    public float framerate = 60.0f;

    public float Framerate
    {
        get => framerate;
        set => framerate = value;
    }

    private bool ignoreTimeScale = false;

    public bool IgnoreTimeScale
    {
        get => ignoreTimeScale;
        set => ignoreTimeScale = value;
    }

    public bool loop = true;

    public bool Loop
    {
        get => loop;
        set => loop = value;
    }

    private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1, 0, 0), new Keyframe(1, 1, 0, 0));

    public event Action FinishEvent;
    private Image image;
    private SpriteRenderer spriteRenderer;
    public int currentFrameIndex = 1;
    private float timer = 0;
    private float currentFramerate = 20.0f;

    public void Reset()
    {
        currentFrameIndex = framerate <1 ? frames.Length - 1 : 1;
    }

    public void Play()
    {
        this.enabled = true;
    }

    public void Pause()
    {
        this.enabled = false;
    }

    public void Stop()
    {
        Pause();
        Reset();
    }

    private void Start()
    {
        remote = this.GetComponent<createRemote>();
        remote.GetRemoteAd(
            () =>
            {
                image = this.GetComponent<Image>();
                spriteRenderer = this.GetComponent<SpriteRenderer>();
                frames = remote.sprites;
                this.Play();
            });
    }

    private void Update()
    {
        if (this.frames == null || this.frames.Length == 0)
        {
            this.enabled = false;
            return;
        }

        //从曲线值计算当前帧率
        float curveValue = curve.Evaluate((float) currentFrameIndex / frames.Length);
        float curvedFramerate = curveValue * framerate;
        //帧率有效
        if (curvedFramerate != 0)
        {
            //获取当前时间
            float time = ignoreTimeScale ? Time.unscaledTime : Time.time;
            //计算帧间隔时间
            float interval = Mathf.Abs(1.0f / curvedFramerate);
            //满足更新条件，执行更新操作
            if (time - timer > interval)
            {
                doUpdate();
            }
        }
    }

//具体更新操作
    private void doUpdate()
    {
        //计算新的索引
        int nextIndex = currentFrameIndex + (int) Mathf.Sign(currentFramerate);
        //索引越界，表示已经到结束帧
        if (nextIndex < 0 || nextIndex >= frames.Length)
        {
            //广播事件
            if (FinishEvent != null)
            {
                FinishEvent();
            }

            //非循环模式，禁用脚本
            if (loop == false)
            {
                currentFrameIndex = Mathf.Clamp(currentFrameIndex, 0, frames.Length - 1);
                this.enabled = false;
                return;
            }
        }

        //钳制索引
        currentFrameIndex = nextIndex % frames.Length;
        currentFrameIndex = currentFrameIndex == 0 ? 1 : currentFrameIndex;
        //更新图片
        if (image != null)
        {
            image.sprite = frames[currentFrameIndex];
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.sprite = frames[currentFrameIndex];
        }

        //设置计时器为当前时间
        timer = ignoreTimeScale ? Time.unscaledTime : Time.time;
    }
}