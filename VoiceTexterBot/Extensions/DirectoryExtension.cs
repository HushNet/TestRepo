using System.Threading.Channels;

namespace VoiceTexterBot.Extensions;

public class DirectoryExtension
{
    
    // <summary>
    // Получаем путь до каталога с .sln файлом
    // </summary>
    public static string GetSolutionRoot()
    {
        var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        
        return Directory.GetParent(dir)?.FullName;
    }
}