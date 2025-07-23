using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

internal sealed class ChatSystem : BaseChatSystem {
    private const int FALSE_LOADING_TIME = 1000;

    private BaseCensorship _censorship = default;
    private BaseNewsFeed _newsFeed = default;
    private GeminiChatBot _aiChatBot = default;

    public ChatSystem(in IBadWords badWords, in NecessaryProperties properties) : base(badWords) {
        _newsFeed = new YahooNewsFeed();
        _censorship = new YahooCensorship(badWords, properties);
        _aiChatBot = new GeminiChatBot(properties);
    }
    public override void Dispose() {
        _newsFeed = null;
    }

    //FIXME: 今後の修正ポイント 連想配列やタプル等を使う、木にするなど...
    public override async UniTask<string> BuildAnswerAsync(string input, CancellationToken token) {
        BaseCensorship.e_censorshipResult isBadWordIncluded = BaseCensorship.e_censorshipResult.NG;
        try {
            isBadWordIncluded = await _censorship.IsBadWordIncluded(input, token);
        }
        catch (OperationCanceledException) {
            throw new OperationCanceledException();
        }

        if (isBadWordIncluded == BaseCensorship.e_censorshipResult.NG) {
            PublishAnswerReady("その言葉は使えないよ");
            return "その言葉は使えないよ";
        }
        if (isBadWordIncluded == BaseCensorship.e_censorshipResult.ERROR) {
            PublishAnswerReady("もう一度話してください");
            return "もう一度話してください";
        }

        if (Regex.IsMatch(input, @"時間") || Regex.IsMatch(input, @"何時") || Regex.IsMatch(input, @"時刻")) {
            DateTime current = DateTime.Now;
            StringBuilder time = new StringBuilder($"今は{current.Month}月{current.Day}日、{current.Hour}時{current.Minute}分だよ");
            PublishAnswerReady(time.ToString());
            return time.ToString();
        }

        if (Regex.IsMatch(input, @"こんにちは") || Regex.IsMatch(input, @"おはよう") || Regex.IsMatch(input, @"こんばんは")) {
            PublishAnswerReady("こんにちは");
            return "こんにちは";
        }

        if (Regex.IsMatch(input, @"ニュース")) {
            string newsText = "";
            try {
                newsText = await _newsFeed.GetNews(token);
                await UniTask.Delay(FALSE_LOADING_TIME, cancellationToken: token, cancelImmediately: true);
            }
            catch (OperationCanceledException) {
                throw new OperationCanceledException();
            }

            StringBuilder modifiedText = new StringBuilder();
            modifiedText.Append(@"この時間帯のニュースは" + '\n' + newsText + "だよ");
            PublishAnswerReady(modifiedText.ToString());
            return modifiedText.ToString();
        }

        GeminiJsonBuilder builder = new GeminiJsonBuilder();
        string answer = "";
        try {
            answer = await _aiChatBot.Ask(input, token);
        }
        catch {
            throw new OperationCanceledException();
        }
        PublishAnswerReady(answer);
        return answer;
    }
}
