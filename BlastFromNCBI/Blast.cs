using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading;

namespace BlastFromNCBI
{
    public delegate void FinishedBlastRequestDelegate(string i_RID, TimeSpan i_TimeSinceStarted, List<BlastGene> i_Genes);

    public static class Blast
    {
        public const int MaxNumberOfRequestsAtTime = 9;
        public static event FinishedBlastRequestDelegate Finished;
        private static readonly HttpClient r_client = new HttpClient();

        private static Queue<KeyValuePair<string,bool>> r_Requests = new Queue<KeyValuePair<string, bool>>();
        private static System.Timers.Timer r_RequestsTimer = new System.Timers.Timer(3000);

        public static int NumberOfWaitingRequests
        {
            get { return r_Requests.Count; }
        }

        private static int s_NumberOfRunningRequests;
        public static int NumberOfRunningRequests
        {
            get { return s_NumberOfRunningRequests; }
        }

        static Blast()
        {
            r_RequestsTimer.Elapsed += async (sender, e) => 
            {
                if (s_NumberOfRunningRequests == MaxNumberOfRequestsAtTime)
                {
                    r_RequestsTimer.Enabled = false;
                }
                else
                {
                    s_NumberOfRunningRequests++;
                    KeyValuePair<string, bool> request = r_Requests.Dequeue();
                    await getRequest(request.Key);

                    if(request.Value)
                    {
                        deleteRequest(request.Key);
                    }
                }
            };
        }

        public static void SendRequest(string query, bool DELETE = true, string MATRIX = "BLOSUM62", string DATABASE = "nr", string PROGRAM = "blastp", int THRESHOLD = 11)
        {
            string url = $"https://blast.ncbi.nlm.nih.gov/Blast.cgi?CMD=put&QUERY={query}&DATABASE={DATABASE}&PROGRAM={PROGRAM}&THRESHOLD={THRESHOLD}&email=elad1segev@gmail.com&tool=Cliquely";
            var response = r_client.GetStringAsync(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response.Result);

            string RID = doc.DocumentNode
                        .SelectNodes("//input[@name='RID']")
                        .First()
                        .Attributes["value"].Value;

            r_Requests.Enqueue(new KeyValuePair<string, bool>(RID, DELETE));
            if(!r_RequestsTimer.Enabled)
            {
                r_RequestsTimer.Enabled = true;
            }
        }

        private static async Task getRequest(string RID)
        {
            if (r_Requests.Count == 0)
            {
                r_RequestsTimer.Enabled = false;
            }

            int sleepMul = 0;
            string url = $"https://blast.ncbi.nlm.nih.gov/Blast.cgi?CMD=get&RID={RID}&FORMAT_TYPE=XML&email=elad1segev@gmail.com&tool=Cliquely";
            await Task.Run(() =>
            {
                DateTime timeStarted = DateTime.Now;
                HtmlNode blastNode;
                do {
                    Thread.Sleep(30000 + 30000 * sleepMul++);
                    var response = r_client.GetStringAsync(url);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(response.Result);


                    blastNode = doc.DocumentNode.SelectSingleNode("//blastoutput");
                        
                }while (blastNode == null) ;

                s_NumberOfRunningRequests--;
                if (r_Requests.Count > 0 && !r_RequestsTimer.Enabled)
                {
                    r_RequestsTimer.Enabled = true;
                }

                var hits = blastNode.SelectSingleNode("blastoutput_iterations")
                     .SelectSingleNode("iteration")
                     .SelectSingleNode("iteration_hits")
                     .SelectNodes("hit");
                if (hits != null)
                {
                    var nodes = hits.Select(y => y.SelectSingleNode("hit_hsps").SelectSingleNode("hsp"))
                    .Select(x => new BlastGene
                    {
                        Sequence = x.SelectSingleNode("hsp_hseq").InnerText,
                        MatchingPercentage = int.Parse(x.SelectSingleNode("hsp_identity").InnerText) * 100f / int.Parse(x.SelectSingleNode("hsp_align-len").InnerText)
                    }).OrderByDescending(x => x.MatchingPercentage);

                    OnFinished(RID, DateTime.Now - timeStarted, nodes.ToList());
                }
                else
                {
                    OnFinished(RID, DateTime.Now - timeStarted, null);
                }
            });
        }

        private static void deleteRequest(string RID)
        {
            string url = $"https://blast.ncbi.nlm.nih.gov/Blast.cgi?CMD=delete&RID={RID}&email=elad1segev@gmail.com&tool=Cliquely";
            r_client.GetAsync(url);
        }

        private static void OnFinished(string i_RID, TimeSpan i_TimeSinceStarted, List<BlastGene> i_Genes)
        {
            Finished?.Invoke(i_RID, i_TimeSinceStarted, i_Genes);
        }
    }
}
