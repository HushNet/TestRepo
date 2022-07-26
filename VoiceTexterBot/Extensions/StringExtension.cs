namespace VoiceTexterBot.Extensions;

public class StringExtension
{
    public static string UppercaseFirst(string s)
    {
        // <summary>
        // Преобразуем строку, чтобы она начиналась с заглавной буквы
        // </summary>
        
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        return char.ToUpper(s[0]) + s.Substring(1);
    }
}