using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CatHUD : MonoBehaviour
{
    public CatGameManager gameManager;

    // Can still be assigned in Inspector — if left empty, auto-created below
    public Text moralityText;
    public Text goalText;
    public Text messageText;
    public Slider yellowBar;
    public Slider greenBar;

    public float messageDuration = 4f;

    private Coroutine messageRoutine;

    void Awake()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<CatGameManager>();

        EnsureHUD();
    }

    // ── Auto-build the HUD if Inspector refs are not manually assigned ────────

    void EnsureHUD()
    {
        bool allAssigned = moralityText != null && goalText != null &&
                           messageText != null && yellowBar != null && greenBar != null;
        if (allAssigned) return;

        // Canvas — Screen Space Overlay so it always sits on top of the game view
        var canvasGO = new GameObject("HUD_Canvas");
        var canvas   = canvasGO.AddComponent<Canvas>();
        canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight  = 0.5f;

        canvasGO.AddComponent<GraphicRaycaster>();

        // Unity 2023 uses LegacyRuntime.ttf; older builds used Arial.ttf
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf")
                 ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

        // ── TOP-LEFT: stat bars inside a dark panel ───────────────────────────
        var panel    = MakeRect("StatsPanel", canvasGO.transform);
        var panelImg = panel.gameObject.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.50f);
        Anchor(panel, 0f, 1f);
        panel.anchoredPosition = new Vector2(12f, -12f);
        panel.sizeDelta        = new Vector2(334f, 90f);

        MakeLabel(panel, "EnergyLabel", "Energy", font, 15, new Vector2(8f,  -14f), new Vector2(84f, 24f));
        if (yellowBar == null)
            yellowBar = MakeSlider(panel, "YellowBar", new Color(1f, 0.82f, 0f),
                                   new Vector2(100f, -14f), new Vector2(226f, 24f));

        MakeLabel(panel, "HealthLabel", "Health",  font, 15, new Vector2(8f, -52f), new Vector2(84f, 24f));
        if (greenBar == null)
            greenBar  = MakeSlider(panel, "GreenBar", new Color(0.18f, 0.82f, 0.18f),
                                   new Vector2(100f, -52f), new Vector2(226f, 24f));

        // ── TOP-RIGHT: morality ───────────────────────────────────────────────
        if (moralityText == null)
            moralityText = MakeText(canvasGO.transform, "MoralityText", "Morality: 0",
                font, 18, TextAnchor.UpperRight,
                anchor: new Vector2(1f, 1f), pivot: new Vector2(1f, 1f),
                pos: new Vector2(-14f, -14f), size: new Vector2(240f, 36f));

        // ── TOP-CENTER: current goal ──────────────────────────────────────────
        if (goalText == null)
            goalText = MakeText(canvasGO.transform, "GoalText", "",
                font, 18, TextAnchor.UpperCenter,
                anchor: new Vector2(0.5f, 1f), pivot: new Vector2(0.5f, 1f),
                pos: new Vector2(0f, -14f), size: new Vector2(700f, 36f));

        // ── BOTTOM-CENTER: temporary message ─────────────────────────────────
        if (messageText == null)
        {
            var msgPanel    = MakeRect("MessagePanel", canvasGO.transform);
            var msgPanelImg = msgPanel.gameObject.AddComponent<Image>();
            msgPanelImg.color = new Color(0f, 0f, 0f, 0.45f);
            Anchor(msgPanel, 0.5f, 0f);
            msgPanel.pivot            = new Vector2(0.5f, 0f);
            msgPanel.anchoredPosition = new Vector2(0f, 40f);
            msgPanel.sizeDelta        = new Vector2(900f, 58f);

            messageText = MakeText(msgPanel, "MessageText", "",
                font, 20, TextAnchor.MiddleCenter,
                anchor: Vector2.zero, pivot: Vector2.zero,
                pos: Vector2.zero, size: Vector2.zero,
                stretch: true);
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    static RectTransform MakeRect(string name, Transform parent)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        return go.AddComponent<RectTransform>();
    }

    static void Anchor(RectTransform r, float x, float y)
    {
        r.anchorMin = r.anchorMax = r.pivot = new Vector2(x, y);
    }

    static void Stretch(RectTransform r)
    {
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.sizeDelta = r.anchoredPosition = Vector2.zero;
    }

    static Text MakeText(Transform parent, string name, string content,
        Font font, int size, TextAnchor alignment,
        Vector2 anchor, Vector2 pivot, Vector2 pos, Vector2 size2,
        bool stretch = false)
    {
        var r = MakeRect(name, parent);

        var shadow = r.gameObject.AddComponent<Shadow>();
        shadow.effectColor    = new Color(0f, 0f, 0f, 0.85f);
        shadow.effectDistance = new Vector2(1f, -1f);

        var text = r.gameObject.AddComponent<Text>();
        text.text      = content;
        text.font      = font;
        text.fontSize  = size;
        text.color     = Color.white;
        text.alignment = alignment;

        if (stretch) { Stretch(r); return text; }

        r.anchorMin        = r.anchorMax = anchor;
        r.pivot            = pivot;
        r.anchoredPosition = pos;
        r.sizeDelta        = size2;
        return text;
    }

    static void MakeLabel(RectTransform parent, string name, string content,
        Font font, int size, Vector2 pos, Vector2 sz)
    {
        var r    = MakeRect(name, parent.transform);
        var text = r.gameObject.AddComponent<Text>();
        text.text      = content;
        text.font      = font;
        text.fontSize  = size;
        text.color     = Color.white;
        text.alignment = TextAnchor.MiddleLeft;
        Anchor(r, 0f, 1f);
        r.anchoredPosition = pos;
        r.sizeDelta        = sz;
    }

    static Slider MakeSlider(RectTransform parent, string name,
        Color fillColor, Vector2 pos, Vector2 sz)
    {
        var r      = MakeRect(name, parent.transform);
        var slider = r.gameObject.AddComponent<Slider>();
        slider.minValue  = 0f;
        slider.maxValue  = 100f;
        slider.value     = 100f;
        slider.direction = Slider.Direction.LeftToRight;
        Anchor(r, 0f, 1f);
        r.anchoredPosition = pos;
        r.sizeDelta        = sz;

        // Dark background track
        var bg    = MakeRect("Background", r.transform);
        var bgImg = bg.gameObject.AddComponent<Image>();
        bgImg.color = new Color(0.12f, 0.12f, 0.12f, 0.85f);
        Stretch(bg);
        slider.targetGraphic = bgImg;

        // Fill area (slight inset so fill doesn't overlap the track border)
        var fillArea = MakeRect("Fill Area", r.transform);
        fillArea.anchorMin        = new Vector2(0f,   0.1f);
        fillArea.anchorMax        = new Vector2(1f,   0.9f);
        fillArea.sizeDelta        = new Vector2(-4f,  0f);
        fillArea.anchoredPosition = Vector2.zero;

        // Colored fill
        var fill    = MakeRect("Fill", fillArea.transform);
        var fillImg = fill.gameObject.AddComponent<Image>();
        fillImg.color = fillColor;
        fill.anchorMin        = Vector2.zero;
        fill.anchorMax        = Vector2.one;
        fill.sizeDelta        = Vector2.zero;
        fill.anchoredPosition = Vector2.zero;
        slider.fillRect = fill;

        return slider;
    }

    // ── Game logic (identical to original) ───────────────────────────────────

    void OnEnable()
    {
        if (gameManager == null) return;
        gameManager.StateChanged  += RefreshState;
        gameManager.MessageRaised += ShowMessage;
        gameManager.GoalChanged   += SetGoal;
    }

    void Start() => RefreshState();

    void OnDisable()
    {
        if (gameManager == null) return;
        gameManager.StateChanged  -= RefreshState;
        gameManager.MessageRaised -= ShowMessage;
        gameManager.GoalChanged   -= SetGoal;
    }

    void RefreshState()
    {
        if (gameManager == null) return;

        if (moralityText != null)
            moralityText.text = "Morality: " + gameManager.morality;

        if (yellowBar != null)
        {
            yellowBar.maxValue = gameManager.maxStat;
            yellowBar.value    = gameManager.yellowStat;
        }

        if (greenBar != null)
        {
            greenBar.maxValue = gameManager.maxStat;
            greenBar.value    = gameManager.greenStat;
        }
    }

    void SetGoal(string goal)
    {
        if (goalText != null)
            goalText.text = "Goal: " + goal;
    }

    void ShowMessage(string message)
    {
        if (messageText == null) return;
        messageText.text = message;
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(ClearMessageAfterDelay());
    }

    IEnumerator ClearMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        if (messageText != null) messageText.text = "";
    }
}
