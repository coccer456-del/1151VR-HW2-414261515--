using UnityEngine;

public class movement : MonoBehaviour
{
    // 移動速度
    public float speed = 3.0f;

    // 使用 Vector2 陣列記錄完整來回路徑
    private Vector2[] pathPoints;
    
    // 當前走到的陣列索引
    private int currentIndex = 0;

    // 控制圖片翻轉的組件
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // 取得 SpriteRenderer 組件
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 去程起始面向右邊
        spriteRenderer.flipX = true;

        // 取得人物當前的 2D 起點位置
        Vector2 origin = transform.position;

        // 將「去程」與「回程」的路徑點全部依序放入同一個陣列
        pathPoints = new Vector2[]
        {
            // === 去程 ===
            origin + new Vector2(2.0f, 0.0f), // 點 0：往前平移
            origin + new Vector2(3.5f, 2.5f), // 點 1：往右上方跳躍（上跳最高點）
            origin + new Vector2(5.0f, 0.0f), // 點 2：往右下方落地（下跳）
            origin + new Vector2(7.0f, 0.0f), // 點 3：去程終點（再往前平移）

            // === 回程（原路返回） ===
            origin + new Vector2(5.0f, 0.0f), // 點 4：回程起步，退回點 2 的位置
            origin + new Vector2(3.5f, 2.5f), // 點 5：往左上方跳躍（回程上跳最高點）
            origin + new Vector2(2.0f, 0.0f), // 點 6：往左下方落地（回程下跳）
            origin                            // 點 7：回到最初起點
        };

        // 目標從點 0 開始出發
        currentIndex = 0;
    }

    void Update()
    {
        // 如果還沒走完整個來回陣列
        if (currentIndex < pathPoints.Length)
        {
            // 每一幀朝當前的目標點平滑移動
            transform.position = Vector2.MoveTowards(
                transform.position,
                pathPoints[currentIndex],
                speed * Time.deltaTime
            );

            // 檢查是否已抵達當前目標點
            if (Vector2.Distance(transform.position, pathPoints[currentIndex]) < 0.01f)
            {
                // 當剛抵達去程終點（點 3），準備往點 4 前進時，翻轉圖片面向左邊
                if (currentIndex == 3)
                {
                    spriteRenderer.flipX = false;
                }

                // 目標索引直接往後加 1
                currentIndex++;

                // 如果已經走完回程（回到原點），恢復初始朝向
                if (currentIndex >= pathPoints.Length)
                {
                    spriteRenderer.flipX = true;
                }
            }
        }
    }
}