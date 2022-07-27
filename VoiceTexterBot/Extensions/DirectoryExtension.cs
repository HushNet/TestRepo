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
        Console.WriteLine(dir);
        var fullname = Directory.GetParent(dir).FullName;
        Console.WriteLine(fullname);
        var projectRoot = fullname.Substring(0, fullname.Length - 4);
        Console.WriteLine(projectRoot);

        Console.WriteLine(Directory.GetParent(projectRoot)?.FullName);
        return Directory.GetParent(projectRoot)?.FullName;
    }
}