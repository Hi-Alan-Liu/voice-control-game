using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxJumpForce = 20f; // 最大跳躍力
    public float forwardSpeed = 5f;  // 水平移動速度
    public float gravityScale = 1f;  // 自定義重力縮放
    public float volumeThreshold = 0.04f; // 聲音門檻
    private Rigidbody2D rb;
    public Animator animator;
    
    public bool isGrounded = true;  // 判斷是否在地面上
    public bool isInhale = false;  // 判斷是否在吸氣狀態

    private bool flyButton = false; // 添加 flyButton 的狀態

    // private WebGLMicrophoneInput microphoneInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // 設置重力縮放
        // microphoneInput = FindObjectOfType<WebGLMicrophoneInput>();
    }

    void Update()
    {
        if (flyButton)
        {
            Debug.Log("flyButton");
            Move(10f);
            flyButton = false;
        }

        // if (microphoneInput == null) return;
        // float volume = microphoneInput.GetRMSVolume();

        // 根據音量調整角色的垂直和水平速度
        // if (!isInhale)  // 如果不是吸氣狀態才允許移動
        // {
        //     // Move(volume);
        //     Move(50);
        // }

        if (rb.velocity.y > 0)
        {
            animator.SetBool("IsFly", true);
        }
        else if (isGrounded) // 當角色碰到地面，設置 IsFly 為 false
        {
            animator.SetBool("IsFly", false);
        }

        // 如果是吸氣狀態且當前是飛行狀態，則取消飛行並讓角色下墜
        if (isInhale && animator.GetBool("IsFly"))
        {
            animator.SetBool("IsFly", false);
            // rb.velocity = new Vector2(rb.velocity.x, 0);  // 立即開始下墜
        }
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityScale - 1) * Time.fixedDeltaTime;
        }
    }

    void Move(float volume)
    {

        // float jumpForce = Mathf.Clamp(volume * maxJumpForce, 0, maxJumpForce);
        // float horizontalSpeed = volume > volumeThreshold ? forwardSpeed : 0f;
        // rb.velocity = new Vector2(horizontalSpeed, jumpForce);

        // rb.velocity = Vector2.up * volume;

        rb.velocity = new Vector2(forwardSpeed, volume);
    }

    // 當角色碰到地面時觸發
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsFly", false);
        }
    }

    // 當角色離開地面時觸發
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsFly", true);
        }
    }

    public void OnClickJump()
    {
        flyButton = true;
    }

    public void InhaleButtonUp() 
    {
        isInhale = false;
        animator.SetBool("IsInhale", false);  // 結束吸氣動畫
    }
    
    public void InhaleButtonDown() 
    {
        isInhale = true;
        animator.SetBool("IsInhale", true);  // 設置吸氣動畫
    }
}