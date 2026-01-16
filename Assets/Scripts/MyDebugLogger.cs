using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;


/// <summary>
/// エディター拡張で指定クラスだけのログを出力とかもあり、
/// エディター拡張でカテゴリ選択、
/// エンジンのログと、インゲームログを分ける
/// </summary>
public static class MyDebugLogger
{
    #region Utility
    private enum LogLevel
    {
        Info,
        Warning,
        Error,
    }
    public enum LogCategory
    {
        General,
        Input,
        UI,
        GamePlay,
        Network,
        Audio,
        Debug,
        Initialization,
        GameOver,
    }

    /// <summary>
    /// ログの色変換用関数
    /// </summary>
    /// <param name="color">ログの色</param>
    /// <returns>ログの色のHex</returns>
    private static string ColorToHex(Color color)
    {
        Color32 c = color;
        return $"#{c.r:X2}{c.g:X2}{c.b:X2}";
    }

    /// <summary>
    /// 呼び出し元のメソッド名とファイル名、行番号を取得する関数
    /// </summary>
    /// <returns></returns>
    private static string GetCaller()
    {
        StackTrace stack = new StackTrace(true);
        foreach (StackFrame frame in stack.GetFrames())
        {
            MethodBase method = frame.GetMethod();
            string typeName = method.DeclaringType?.FullName ?? "";
            if (!typeName.Contains(nameof(MyDebugLogger)))
            {
                string file = frame.GetFileName() ?? "UnknownFile";
                int line = frame.GetFileLineNumber();
                return $"{typeName}.{method.Name} ({file}:{line})";
            }
        }
        return "(Caller not found)";
    }
    #endregion

    private static readonly Dictionary<LogLevel, Color> LevelColors = new()
    {
        { LogLevel.Warning, Color.yellow },
        { LogLevel.Error, Color.red },
        { LogLevel.Info, Color.white },
    };

    private static readonly Dictionary<LogCategory, Color> CategoryColors = new()
    {
        { LogCategory.General, Color.white },
        { LogCategory.Input, Color.green },
        { LogCategory.UI, Color.cyan },
        { LogCategory.GamePlay, Color.blue },
        { LogCategory.Network, Color.magenta },
        { LogCategory.Audio, new Color(1f, 0f, 1f) },
        { LogCategory.Debug, Color.yellow },
        { LogCategory.Initialization, Color.gray },
        { LogCategory.GameOver, Color.red },
    };

    private static readonly HashSet<LogCategory> enabledCategories = new();

    public static void SetEnabledCategories(params LogCategory[] categories)
    {
        enabledCategories.Clear();
        foreach (var category in categories)
        {
            enabledCategories.Add(category);
        }
    }

    /// <summary>
    /// 情報をログ出力する関数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="category"></param>
    /// <param name="color"></param>
    public static void Log(
        string message,
        LogCategory category = LogCategory.General,
        Color? color = null)
    {
        if (color == null)
        {
            color = LevelColors.TryGetValue(LogLevel.Info, out var col) ? col : Color.white;
        }
        LogInternal(message, category, LogLevel.Info, ColorToHex(color.Value));
    }

    /// <summary>
    /// 警告をログ出力する関数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="category"></param>
    /// <param name="color"></param>
    public static void LogWarning(
        string message,
        LogCategory category = LogCategory.General,
        Color? color = null)
    {
        if (color == null)
        {
            color = LevelColors.TryGetValue(LogLevel.Warning, out var col) ? col : Color.yellow;
        }
        LogInternal(message, category, LogLevel.Warning, ColorToHex(color.Value));
    }

    /// <summary>
    /// エラーをログ出力する関数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="category"></param>
    /// <param name="color"></param>
    public static void LogError(
        string message,
        LogCategory category = LogCategory.General,
        Color? color = null)
    {
        if (color == null)
        {
            color = LevelColors.TryGetValue(LogLevel.Error, out var col) ? col : Color.red;
        }
        LogInternal(message, category, LogLevel.Error, ColorToHex(color.Value));
    }

    /// <summary>
    /// 内部ログ出力関数
    /// </summary>
    /// <param name="message">出力メッセージ</param>
    /// <param name="category">出力ログのカテゴリ</param>
    /// <param name="level">出力ログのレベル（Normal・Warning・Error）</param>
    /// <param name="color">ログの色指定</param>
    private static void LogInternal(string message, LogCategory category, LogLevel level, string color)
    {
#if UNITY_EDITOR
        if (enabledCategories.Count > 0 && !enabledCategories.Contains(category)) return;

        string categoryColorHex = CategoryColors.TryGetValue(category, out var catColor)
            ? ColorToHex(catColor) : ColorToHex(Color.white);
        string categoryLabel = $"<color={categoryColorHex}>{category}</color>";
        string header = $"[{categoryLabel}] <color={color}>{message}</color>";

        string trace = GetCaller();

        switch (level)
        {
            case LogLevel.Info:
                UnityEngine.Debug.Log($"{header} <i>({trace})</i>");
                break;
            case LogLevel.Warning:
                UnityEngine.Debug.LogWarning($"{header} <i>({trace})</i>");
                break;
            case LogLevel.Error:
                UnityEngine.Debug.LogError($"{header} <i>({trace})</i>");
                break;
        }
#endif
    }

}
