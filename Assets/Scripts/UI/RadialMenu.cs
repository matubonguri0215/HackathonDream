using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Input;
using Debug = MyDebugLogger;
using System;

public class RadialMenu : MonoBehaviour
{
    [Header("メニュー設定")]
    [SerializeField]
    private Sprite donutSprite; // ドーナツ状のスプライト
    [SerializeField]
    [Range(2, 8)]
    private int sectionCount = 4; // 分割数
    [SerializeField]
    private string[] menuLabels = { "攻撃", "防御", "アイテム", "逃げる" };

    [Header("ビジュアル設定")]
    [SerializeField]
    private Color normalColor = new Color(0.8f, 0.8f, 0.8f, 0.9f);
    [SerializeField]
    private Color highlightColor = new Color(1f, 1f, 0.5f, 1f);
    [SerializeField]
    private float donutSize = 200f; // ドーナツのサイズ

    [Header("テキスト設定")]
    [SerializeField]
    private float textRadius = 100f; // テキストを配置する半径
    [SerializeField]
    private int fontSize = 20;

    private List<GameObject> menuItems = new List<GameObject>();
    private List<GameObject> textObjects = new List<GameObject>();
    private int selectedIndex = -1;
    [SerializeField]
    private bool isMenuActive = false;

    private event Action<int> onMenuItemSelected;
    public event Action<int> OnMenuItemSelected
    {
        add { onMenuItemSelected += value; }
        remove { onMenuItemSelected -= value; }
    }

    void Start()
    {
        CreateRadialMenu();
        SetMenuActive(false);
    }

    void Update()
    {
        if (isMenuActive)
        {
            UpdateSelection();

            // 左クリックで選択を確定
            if (InputManager.WasReleased(ActionType.WeaponSelector) && selectedIndex >= 0)
            {
                Debug.Log($"ラジアルメニュー {selectedIndex} が選択されました。", Debug.LogCategory.UI);
                ExecuteMenuItem(selectedIndex);
                Hide();
            }
        }
    }

    void CreateRadialMenu()
    {
        // 既存のメニューをクリア
        foreach (var item in menuItems)
        {
            if (item != null) Destroy(item);
        }
        foreach (var text in textObjects)
        {
            if (text != null) Destroy(text);
        }
        menuItems.Clear();
        textObjects.Clear();

        // menuLabelsの長さをsectionCountに合わせる
        int labelCount = Mathf.Min(sectionCount, menuLabels.Length);

        for (int i = 0; i < sectionCount; i++)
        {
            // ドーナツの各セクションを作成
            GameObject item = CreateDonutSection(i);
            menuItems.Add(item);

            // テキストラベルを作成（ラベルがある場合のみ）
            string label = i < labelCount ? menuLabels[i] : $"項目{i + 1}";
            GameObject textObj = CreateTextLabel(i, label);
            textObjects.Add(textObj);
        }
    }

    GameObject CreateDonutSection(int index)
    {
        GameObject item = new GameObject($"DonutSection_{index}");
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;

        // Image コンポーネントを追加
        Image img = item.AddComponent<Image>();
        img.sprite = donutSprite;
        img.color = normalColor;
        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Radial360;
        img.fillOrigin = (int)Image.Origin360.Top;
        img.fillClockwise = true;

        // 各セクションの塗りつぶし範囲を設定
        img.fillAmount = 1f / sectionCount;

        RectTransform rect = item.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(donutSize, donutSize);

        // 回転させて配置
        float anglePerSection = 360f / sectionCount;
        float rotationOffset = (sectionCount == 4) ? -45f : 0f; // 4分割の時は45度オフセット
        rect.localRotation = Quaternion.Euler(0, 0, -anglePerSection * index + rotationOffset);

        return item;
    }

    GameObject CreateTextLabel(int index, string label)
    {
        // 角度を計算（上から時計回り）
        float anglePerSection = 360f / sectionCount;
        float angleOffset = (sectionCount == 4) ? -45f : -90f; // 4分割の時は45度オフセット
        float angle = index * anglePerSection + angleOffset;
        float rad = angle * Mathf.Deg2Rad;

        // テキストの位置を計算
        Vector2 pos = new Vector2(
            Mathf.Cos(rad) * textRadius,
            Mathf.Sin(rad) * textRadius
        );

        GameObject textObj = new GameObject($"Text_{index}");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = pos;

        Text text = textObj.AddComponent<Text>();
        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = fontSize;
        text.color = Color.white;

        // テキストに影を追加
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(1, -1);

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(120, 50);

        return textObj;
    }

    void UpdateSelection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = new Vector2(mousePos.x, -mousePos.y);
        Vector2 centerPos = Screen.width / 2f * Vector2.right + Screen.height / 2f * Vector2.up;
        Vector2 dir = InputManager.GetAxisRight();

        //// 中心からの距離をチェック
        //if (InputManager.GetAxisRight().magnitude < 30f)
        //{
        //    SetSelectedIndex(-1);
        //    return;
        //}

        // 角度からインデックスを計算
        float angle = Mathf.Atan2(-dir.y, dir.x) * Mathf.Rad2Deg;

        // 4分割の時は45度オフセット（上下左右に合わせる）
        if (sectionCount == 4)
        {
            angle += 45f;
        }
        else
        {
            angle += 90f;
        }

        if (angle < 0) angle += 360f;

        float anglePerSection = 360f / sectionCount;
        int index = Mathf.FloorToInt(angle / anglePerSection) % sectionCount;
        SetSelectedIndex(index);
    }

    void SetSelectedIndex(int index)
    {
        if (selectedIndex == index) return;

        // 前の選択を解除
        if (selectedIndex >= 0 && selectedIndex < menuItems.Count)
        {
            menuItems[selectedIndex].GetComponent<Image>().color = normalColor;
        }

        selectedIndex = index;

        // 新しい選択をハイライト
        if (selectedIndex >= 0 && selectedIndex < menuItems.Count)
        {
            menuItems[selectedIndex].GetComponent<Image>().color = highlightColor;
        }
    }

    void ExecuteMenuItem(int index)
    {
        Debug.Log($"選択されたメニュー: {menuLabels[index]}");
        // ここに各メニュー項目の処理を実装
        onMenuItemSelected?.Invoke(index);
    }

    void SetMenuActive(bool active)
    {
        isMenuActive = active;
        gameObject.SetActive(active);

        if (!active)
        {
            SetSelectedIndex(-1);
        }
    }

    // 公開メソッド
    public void Show()
    {
        SetMenuActive(true);
    }

    public void Hide()
    {
        SetMenuActive(false);
    }
}