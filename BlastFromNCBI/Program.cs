using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace BlastFromNCBI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fasta;
            Console.WriteLine("Enter a FASTA");
            fasta = Console.ReadLine();
            //string RID = Blast.SendRequest("MLEAESLVKSFGKLIAVDHFSMNIERGEVKALIGPNGAGKTTFVNLVTGALTPDEGRIRLDSRCIVRLPSYKRARLGIARTYQIPKPYPNLTVLENVMVSSIYAAGLEGEDAMSAAVEALKYVGIYNMADKLASELNAEQQKLLDFARALAARPKYLLIDEIGAGLSVDELEGLASKIRAVAKQGVGVLYIGHIMKMVKSVADWVIVMNEGRKVTEGTYDDIANNEEVIKIYLGEKGAVE");
            Blast.SendRequest(fasta);
            Blast.Finished += Blast_Finished;
            
            while(true)
            {
                Console.WriteLine($"{Blast.NumberOfWaitingRequests} : {Blast.NumberOfRunningRequests}");
                Thread.Sleep(10000);
            }

            Console.ReadKey();
        }

        private static void Blast_Finished(string i_RID, TimeSpan i_TimeSinceStarted, List<BlastGene> i_Genes)
        {
            if (i_Genes == null)
            {
                Console.WriteLine("Not Found");
            }
            else
            {
                foreach (BlastGene gene in i_Genes)
                {
                    Console.WriteLine(gene);
                }
            }
        }
    }
}
