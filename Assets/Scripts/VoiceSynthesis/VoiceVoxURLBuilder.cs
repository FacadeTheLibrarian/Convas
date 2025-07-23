using System.Runtime.CompilerServices;
using System.Text;

internal sealed class VoiceVoxURLBuilder {

    private readonly string FQDN = "localhost:50021";
    private readonly string AUDIO_QUERY = "/audio_query";
    private readonly string SYNTHESIS = "/synthesis";
    private readonly string SPEAKERS = "/speakers";
    private readonly string IS_SPEAKER_INITIALIZED = "/is_initialized_speaker?speaker=";

    public VoiceVoxURLBuilder() { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetIsSpeakerInitialized(int speakerId) {
        StringBuilder url = new StringBuilder($"{FQDN}{IS_SPEAKER_INITIALIZED}{speakerId}");
        return url.ToString();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetAudioQuery(int speakerId, in string text) {
        StringBuilder url = new StringBuilder($"{FQDN}{AUDIO_QUERY}?speaker={speakerId}&text={text}");
        return url.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetSynthesisVoice(int speakerId) {
        StringBuilder url = new StringBuilder($"{FQDN}{SYNTHESIS}?speaker={speakerId}");
        return url.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetSpeakers() {
        StringBuilder url = new StringBuilder($"{FQDN}{SPEAKERS}");
        return url.ToString();
    }
}
