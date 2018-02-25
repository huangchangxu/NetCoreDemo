using Nest;
using PaymentCenter.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.ConsoleApp
{
    /// <summary>
    /// Elasticseach测试样例
    /// </summary>
    public class ESTest
    {
        public static void Add()
        {
            string[] nameArray = { "Cody", "Blake", "Dennis", "Evan ", "Harris", "Jason ", "Lambert ", "Louis ", "Milton ", "Cody" };
            string[] skillArray = { "c#", "c++", "java", "python", "php", "Linux", "ruby", "matlab", "perl", "powershell" };
            long[] ageRange = { 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };

            for (int i = 0; i < 10; i++)
            {
                var resume = new Resume
                {
                    Id = Guid.NewGuid(),
                    Name = nameArray[i],
                    Age = ageRange[i],
                    Skills = "My skill is Azure and " + skillArray[i]
                };
                //Infrastructure.Elasticsearch.ElasticsearchHelper<Resume>.GetIndexResponse(resume);
                new ResumeEsResponse().AddDocument(resume);
            }
        }

        public static void Query()
        {
            var data = new ResumeEsResponse().Get();
            Console.WriteLine(data.ToJson());
        }
    }
    
    public class Resume
    {
        public Guid? Id { get; set; }
        
        public string Name { get; set; }
        
        public long Age { get; set; }
        public string Skills { get; set; }
    }

    public class ResumeEsResponse : Infrastructure.Elasticsearch.ElasticsearchBaseResponse<Resume>
    {
        public override string IndexName { get => "resume"; }


        public object Get()
        {
          var search=  elasticClient.Search<Resume>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .MatchAll())
                );
            return search.Documents;
        }
    }
}
