﻿namespace VoiceTexterBot.Extensions;

public class DirectoryExtension
{
    
    // <summary>
    // Получаем путь до каталога с .sln файлом
    // </summary>
    public static string GetSolutionRoot()
    {
        var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        var fullname = Directory.GetParent(dir).FullName;
        var projectRoot = Directory.GetParent(fullname)?.FullName;
        var parentProject = Directory.GetParent(projectRoot)?.FullName;

        return Directory.GetParent(parentProject)?.FullName!;
    }
}