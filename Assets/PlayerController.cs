using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxJumpForce = 20f; // 最大跳躍力
    public float forwardSpeed = 5f;  // 水平移動速度
    public float gravityScale = 1f;  // 自定義重力縮放
    public float volumeThreshold = 0.04f; // 聲音門檻
    private Rigidbody2D rb;
    // private MicrophoneInput microphoneInput;
    private WebGLMicrophoneInput microphoneInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // 設置重力縮放
        // microphoneInput = FindObjectOfType<MicrophoneInput>();
        microphoneInput = FindObjectOfType<WebGLMicrophoneInput>();
    }

    void Update()
    {
        if (microphoneInput == null) return;

        float volume = microphoneInput.GetRMSVolume();

        Debug.Log(volume);

        // 根據音量調整角色的垂直和水平速度
        Move(volume);
    }

    void FixedUpdate()
    {
        // 控制角色的下墜速度
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityScale - 1) * Time.fixedDeltaTime;
        }
    }

    void Move(float volume)
    {
        float jumpForce = Mathf.Clamp(volume * maxJumpForce, 0, maxJumpForce);
        float horizontalSpeed = volume > volumeThreshold ? forwardSpeed : 0f;

        rb.velocity = new Vector2(horizontalSpeed, jumpForce);
    }
}
