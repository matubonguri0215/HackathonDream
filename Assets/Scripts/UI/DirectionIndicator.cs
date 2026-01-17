using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの周りを回転して目的地方向を指すUI
/// </summary>
public class DirectionIndicator : MonoBehaviour
{

    [SerializeField]
    private AreaController areaController;
    [Header("参照")]
    [SerializeField] private Transform player;          // プレイヤーのTransform
    [SerializeField] private Transform target;          // 目的地のTransform
    [SerializeField] private RectTransform indicator;   // 方向指示UI（矢印など）

    [Header("設定")]
    [SerializeField] private float radius = 100f;       // プレイヤーからの距離（ピクセル）
    [SerializeField] private bool smoothRotation = true; // 滑らかな回転
    [SerializeField] private float rotationSpeed = 10f;  // 回転の滑らかさ

    [Header("オプション: 距離表示")]
    [SerializeField] private Text distanceText;         // 距離を表示するText（任意）
    [SerializeField] private bool showDistance = true;

    private Camera mainCamera;

    void Start()
    {
        areaController.OnChangeNextArea += ChangeTarget;
        mainCamera = Camera.main;

        if (player == null)
        {
            Debug.LogError("Playerが設定されていません！");
        }
        if (target == null)
        {
            Debug.LogError("Targetが設定されていません！");
        }
        if (indicator == null)
        {
            Debug.LogError("Indicatorが設定されていません！");
        }
    }

    void Update()
    {
        if (player == null || target == null || indicator == null)
            return;

        UpdateIndicatorPosition();

        if (showDistance && distanceText != null)
        {
            UpdateDistanceText();
        }
    }

    /// <summary>
    /// インジケーターの位置と回転を更新
    /// </summary>
    void UpdateIndicatorPosition()
    {
        // プレイヤーから目的地への方向ベクトル（ワールド座標）
        Vector3 directionWorld = target.position - player.position;

        // 2D見降ろしの場合、Y軸を無視（3Dの場合）
        // 完全に2Dの場合はこの行を削除してください
        directionWorld.y = 0;

        // 方向ベクトルから角度を計算（度数法）
        float angle = Mathf.Atan2(directionWorld.z, directionWorld.x) * Mathf.Rad2Deg;

        // 2D（XY平面）の場合は以下を使用
        // float angle = Mathf.Atan2(directionWorld.y, directionWorld.x) * Mathf.Rad2Deg;

        // カメラの回転を考慮（必要に応じて）
        angle -= mainCamera.transform.eulerAngles.y;

        // 滑らかな回転
        float currentAngle = indicator.localEulerAngles.z;
        if (smoothRotation)
        {
            // 角度の差分を-180~180の範囲に正規化
            float angleDiff = Mathf.DeltaAngle(currentAngle, -angle + 90);
            currentAngle += angleDiff * rotationSpeed * Time.deltaTime;
        }
        else
        {
            currentAngle = -angle + 90;
        }

        // インジケーターを回転
        indicator.localEulerAngles = new Vector3(0, 0, currentAngle);

        // プレイヤーの周りの円周上に配置
        float angleRad = currentAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad)) * radius;
        indicator.anchoredPosition = offset;
    }

    /// <summary>
    /// 距離テキストを更新
    /// </summary>
    void UpdateDistanceText()
    {
        float distance = Vector3.Distance(player.position, target.position);
        distanceText.text = $"{distance:F1}m";
    }
    private void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}


/* ========================================
 * セットアップ手順
 * ========================================
 * 
 * 1. Canvas上に以下を作成:
 *    - 空のGameObject「DirectionIndicatorContainer」を作成
 *    - その子に Image「Arrow」を作成（矢印画像を設定）
 *    - 必要に応じて Text「Distance」を作成
 * 
 * 2. DirectionIndicatorContainerのRectTransform設定:
 *    - Anchor: Center-Center
 *    - Position: プレイヤーのスクリーン位置（通常は画面中央）
 * 
 * 3. Arrow Imageの設定:
 *    - Anchor: Center-Center
 *    - Pivot: (0.5, 0.5)
 *    - 矢印は上向き（Unity座標で0度）にしておく
 * 
 * 4. このスクリプトをDirectionIndicatorContainerにアタッチ
 * 
 * 5. インスペクターで設定:
 *    - Player: プレイヤーのTransform
 *    - Target: 目的地のTransform
 *    - Indicator: Arrow ImageのRectTransform
 *    - Radius: プレイヤーからの距離（100~150推奨）
 *    - Distance Text: (任意) 距離表示用のText
 * 
 * ======================================== */