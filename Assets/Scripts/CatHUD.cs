using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CatHUD : MonoBehaviour
{
    public CatGameManager gameManager;

    // Legacy Inspector refs kept for compatibility — display now uses OnGUI
    public Text moralityText;
    public Text goalText;
    public Text messageText;
    public Slider yellowBar;
    public Slider greenBar;

    public float messageDuration = 4f;

    private string currentGoal    = "";
    private string currentMessage = "";
    private Coroutine messageRoutine;

    private Texture2D _whiteTex;
    Texture2D White
    {
        get
        {
            if (_whiteTex == null)
            {
                _whiteTex = new Texture2D(1, 1);
                _whiteTex.SetPixel(0, 0, Color.white);
                _whiteTex.Apply();
            }
            return _whiteTex;
        }
    }

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<CatGameManager>();
    }

    void OnEnable()
    {
        if (gameManager == null) return;
        gameManager.StateChanged  += RefreshState;
        gameManager.MessageRaised += ShowMessage;
        gameManager.GoalChanged   += SetGoal;
    }

    void OnDisable()
    {
        if (gameManager == null) return;
        gameManager.StateChanged  -= RefreshState;
        gameManager.MessageRaised -= ShowMessage;
        gameManager.GoalChanged   -= SetGoal;
    }

    void Start() => RefreshState();

    void RefreshState() { /* values read live from gameManager in OnGUI */ }

    void SetGoal(string goal) => currentGoal = goal;

    void ShowMessage(string message)
    {
        currentMessage = message;
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(ClearAfterDelay());
    }

    IEnumerator ClearAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        currentMessage = "";
    }

    // ── OnGUI — draws directly onto the screen, no Canvas needed ─────────────

    void OnGUI()
    {
        if (gameManager == null) return;

        float sw  = Screen.width;
        float sh  = Screen.height;
        float s   = sh / 1080f;   // scale factor relative to 1080p

        float pad  = 14 * s;
        float barW = 280 * s;
        float barH = 26 * s;
        int   fs   = Mathf.Max(12, Mathf.RoundToInt(15 * s));

        // ── Energy bar (yellow) — top left ────────────────────────────────
        float ratio1 = gameManager.maxStat > 0 ? gameManager.yellowStat / gameManager.maxStat : 1f;
        DrawBar(pad, pad, barW, barH, ratio1, new Color(1f, 0.82f, 0f), "Energy", fs);

        // ── Health bar (green) — below energy ─────────────────────────────
        float ratio2 = gameManager.maxStat > 0 ? gameManager.greenStat / gameManager.maxStat : 1f;
        DrawBar(pad, pad + barH + 8 * s, barW, barH, ratio2, new Color(0.2f, 0.85f, 0.2f), "Health", fs);

        // ── Morality — top right ──────────────────────────────────────────
        string morStr   = "Morality: " + gameManager.morality;
        float  morW     = 230 * s;
        var    morStyle = MakeStyle(Mathf.Max(12, Mathf.RoundToInt(17 * s)), TextAnchor.UpperRight);
        ShadowLabel(new Rect(sw - morW - pad, pad, morW, 34 * s), morStr, morStyle);

        // ── Current goal — top center ─────────────────────────────────────
        if (!string.IsNullOrEmpty(currentGoal))
        {
            float goalW     = 700 * s;
            var   goalStyle = MakeStyle(Mathf.Max(12, Mathf.RoundToInt(17 * s)), TextAnchor.UpperCenter);
            ShadowLabel(new Rect((sw - goalW) * 0.5f, pad, goalW, 34 * s),
                        "Goal: " + currentGoal, goalStyle);
        }

        // ── Message — bottom center ───────────────────────────────────────
        if (!string.IsNullOrEmpty(currentMessage))
        {
            float msgW  = Mathf.Min(900 * s, sw - pad * 2);
            float msgH  = 64 * s;
            float msgX  = (sw - msgW) * 0.5f;
            float msgY  = sh - msgH - 50 * s;

            DrawRect(new Rect(msgX - 10, msgY - 6, msgW + 20, msgH + 12),
                     new Color(0f, 0f, 0f, 0.5f));

            var msgStyle = MakeStyle(Mathf.Max(12, Mathf.RoundToInt(19 * s)), TextAnchor.MiddleCenter);
            msgStyle.wordWrap = true;
            ShadowLabel(new Rect(msgX, msgY, msgW, msgH), currentMessage, msgStyle);
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    void DrawBar(float x, float y, float w, float h,
                 float fill, Color fillColor, string label, int fontSize)
    {
        float labelW = 74 * (h / 26f);  // proportional label column
        float totalW = w + 8 + labelW;

        // panel background
        DrawRect(new Rect(x - 5, y - 5, totalW + 10, h + 10), new Color(0f, 0f, 0f, 0.5f));
        // dark track
        DrawRect(new Rect(x, y, w, h), new Color(0.12f, 0.12f, 0.12f, 0.9f));
        // colored fill
        DrawRect(new Rect(x, y, w * Mathf.Clamp01(fill), h), fillColor);

        // label to the right
        var style = MakeStyle(fontSize, TextAnchor.MiddleLeft);
        ShadowLabel(new Rect(x + w + 8, y, labelW, h), label, style);
    }

    void DrawRect(Rect r, Color c)
    {
        var prev = GUI.color;
        GUI.color = c;
        GUI.DrawTexture(r, White);
        GUI.color = prev;
    }

    static GUIStyle MakeStyle(int size, TextAnchor alignment)
    {
        return new GUIStyle(GUI.skin.label)
        {
            fontSize  = size,
            alignment = alignment,
            normal    = { textColor = Color.white }
        };
    }

    static void ShadowLabel(Rect r, string text, GUIStyle style)
    {
        var shadow = new GUIStyle(style);
        shadow.normal.textColor = new Color(0f, 0f, 0f, 0.85f);
        GUI.Label(new Rect(r.x + 1, r.y + 1, r.width, r.height), text, shadow);
        GUI.Label(r, text, style);
    }
}
