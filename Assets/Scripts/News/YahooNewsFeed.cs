using UnityEngine.Networking;
using UnityEngine;
using System.Threading;
using UniRx;
using Cysharp.Threading.Tasks;
using System;
using System.Text;
using System.Xml.Linq;

internal sealed class YahooNewsFeed : BaseNewsFeed {
    
    private const string URL = "https://news.yahoo.co.jp/rss/topics/top-picks.xml";
    private const string ITEMS = "item";
    private const string TITLE = "title";
    private const int NUMBER_OF_NEWS_LINE = 4;
    private const int MAX_REQUEST_ITERATION = 3;
    private const int TIME_OUT = 5;

    public YahooNewsFeed() { }

    public override async UniTask<string> GetNews(CancellationToken token) {
        using (UnityWebRequest request = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET)) {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = TIME_OUT;
            int requestIteration = 0;

            while (requestIteration < MAX_REQUEST_ITERATION) {
                try {
                    token.ThrowIfCancellationRequested();
                    await request.SendWebRequest();
                }
                catch (OperationCanceledException) {
                    throw new OperationCanceledException();
                }
                catch {
                    requestIteration++;
                }
            }

            if (request.responseCode == WebUtility.HTTP_STATUS_OKAY) {
                XDocument xml = XDocument.Parse(request.downloadHandler.text);

                var formatted = xml.Descendants(ITEMS).Elements(TITLE);

                StringBuilder builder = new StringBuilder();
                
                int i = 0;
                foreach (XElement item in formatted) {
                    i++;
                    builder.Append(item.Value + ' ' + '\n');
                    if(i > NUMBER_OF_NEWS_LINE - 1) {
                        break;
                    }
                }
                return builder.ToString();
            }
        }

        return "情報を取得できませんでした...";
    }
}
