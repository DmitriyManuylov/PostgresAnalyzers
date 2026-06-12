using Analyzers.Models.DDLAnalyzer.Enums;

namespace Analyzers.Models.DDLAnalyzer;

/// <summary>
/// Модель результатов проверки файла на наличие опасных и требующих внимания изменений.
/// </summary>
public class FileCheckModel
{
    /// <summary>
    /// Тип обнаруженного изменения, требующего внимания.
    /// </summary>
    public FileCheckResultType CheckResultType { get; set; }

    /// <summary>
    /// Описание обнаруженного изменения.
    /// </summary>
    public string? CheckComment { get; set; }

    /// <summary>
    /// Путь к файлу в репозитории.
    /// </summary>
    public string? FilePath { get; set; }
}
