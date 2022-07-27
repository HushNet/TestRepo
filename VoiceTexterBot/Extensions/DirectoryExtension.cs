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
        var fullname = Directory.GetParent(dir).FullName;
        var projectRoot = fullname.Substring(0, fullname.Length - 4);

        Console.WriteLine(Directory.GetParent(projectRoot)?.FullName);
        return Directory.GetParent(projectRoot)?.FullName;
    }
}