namespace Analyzers.Models.DDLAnalyzer.Enums;

/// <summary>
/// Типы обнаруживаемых изменений.
/// </summary>
public enum FileCheckResultType
{
    /// <summary>
    /// Изменения, которые не затронут работоспособность текущих приложений.
    /// </summary>
    Minor,

    /// <summary>
    /// Изменения, которые не нарушают работоспособность текущего приложения, но могут внести изменения в логику работы приложения
    /// </summary>
    Moderate,

    /// <summary>
    /// Изменения, которые могут нарушить работоспособность текущих приложений
    /// </summary>
    Dangerous,
}
