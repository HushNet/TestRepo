using VoiceTexterBot.Models;

namespace VoiceTexterBot.Services;

public interface IStorage
{
    public Session GetSession(long chatId);
}