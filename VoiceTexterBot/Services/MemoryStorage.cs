using System.Collections.Concurrent;
using VoiceTexterBot.Models;

namespace VoiceTexterBot.Services;

public class MemoryStorage : IStorage
{
    private ConcurrentDictionary<long, Session> _sessions;

    public MemoryStorage()
    {
        _sessions = new ConcurrentDictionary<long, Session>();
    }

    public Session GetSession(long chatId)
    {
        if (_sessions.ContainsKey(chatId))
        {
            return _sessions[chatId];
        }

        var newSession = new Session() {LanguageCode = "ru", DetectorSpeed = "fast"};

        _sessions.TryAdd(chatId, newSession);
        return newSession;
    }
}