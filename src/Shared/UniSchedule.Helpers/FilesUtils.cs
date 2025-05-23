using System.Text.Json;
using System.Text.Json.Serialization;
using UniSchedule.Extensions.Exceptions.Base;

namespace UniSchedule.Helpers;

/// <summary>
///     Утилиты для работы с файлами
/// </summary>
public static class FilesUtils
{
    /// <summary>
    ///     Получение файлов из директории
    /// </summary>
    /// <param name="directoryPath">Путь к директории</param>
    /// <returns>Список файлов</returns>
    /// <exception cref="NotFoundException">Директория не найдена</exception>
    public static List<string> GetFiles(string directoryPath)
    {
        var isDirectoryExists = Directory.Exists(directoryPath);
        if (!isDirectoryExists)
        {
            throw new NotFoundException("Директория не найдена");
        }

        var files = Directory.GetFiles(directoryPath);

        return files.ToList();
    }

    /// <summary>
    ///     Десериализация из файла
    /// </summary>
    /// <param name="file">Файл</param>
    /// <typeparam name="TData">Тип данных</typeparam>
    /// <returns>Данные</returns>
    /// <exception cref="ArgumentException">Неверный формат данных</exception>
    public static TData DeserializeFromFile<TData>(string file)
    {
        using var stream = File.OpenRead(file);
        var data = JsonSerializer.Deserialize<TData>(stream, GetJsonSerializerOptions());
        if (data == null)
        {
            throw new ArgumentException("Неверный формат данных");
        }

        return data;
    }

    /// <summary>
    ///     Получение настроек сериализации
    /// </summary>
    /// <returns>Настройки сериализации</returns>
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, Converters = { new JsonStringEnumConverter() }
        };

        return serializerOptions;
    }
}