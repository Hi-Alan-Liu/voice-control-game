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
    public Animator animator;
    public bool isGrounded = true;  // 判斷是否在地面上
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

        if (rb.velocity.y > 0)
        {
            animator.SetBool("IsFly", true);
        }
        else if (isGrounded) // 當角色碰到地面，設置 IsFly 為 false
        {
            animator.SetBool("IsFly", false);
        }
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

    // 當角色碰到地面時觸發
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢測是否碰到地面，並更新isGrounded狀態
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsFly", false);  // 當碰到地面時，停止飛行動畫
        }
    }

    // 當角色離開地面時觸發
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 檢測是否離開地面，並更新isGrounded狀態
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
