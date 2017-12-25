using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace PaymentCenter.Infrastructure.Elasticsearch
{
    /// <summary>
    /// ES 查询基础仓储类
    /// </summary>
    public abstract class ElasticsearchBaseResponse<T> where T : class, new()
    {
        public IElasticClient elasticClient;

        public ElasticsearchBaseResponse()
        {
            elasticClient = GetElasticClient(UriPool, IndexName);
        }

        public virtual Uri[] UriPool
        {
            get
            {
                return new Uri[] { new Uri("http://192.168.3.41:9200") };
            }
        }

        public abstract string IndexName { get; }

        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <returns></returns>
        private IElasticClient GetElasticClient (Uri[] nodes,string indexName)
        {
            var connectionPool = new SniffingConnectionPool(nodes);
            var settings = new ConnectionSettings(connectionPool).DefaultIndex(indexName).RequestTimeout(TimeSpan.FromSeconds(60)).DisableDirectStreaming();
            var client = new ElasticClient(settings);
            var res = client.ClusterHealth();
            Console.WriteLine("ES状态：{0}", res.Status);

            if (!client.IndexExists(indexName).Exists)
            {
                var createIndexResponse = client.CreateIndex(indexName);
                if (!createIndexResponse.IsValid)
                {
                    //发生错误输出错误原因
                    Console.WriteLine(createIndexResponse.ServerError.Error.Reason);
                }
            }
            return client;
        }

        public ElasticsearchExceResult AddDocument(T data)
        {
            var res = elasticClient.Index(data, p => p.Type(typeof(T)));
            if (res.IsValid && res.Created)
                return new ElasticsearchExceResult { Result = res.IsValid };
            else
            {
                var result =new ElasticsearchExceResult { Result = false };
                if (res.ServerError != null)
                {
                    result.ErrorReason = res.ServerError.Error.Reason;
                    result.ErrorStatus = res.ServerError.Status;
                }
                return result;
            }
        }

        //public Task<ElasticsearchExceResult> AddDocumentAsync(T data)
        //{
        //    var res = elasticClient.IndexAsync(data, p => p.Type(typeof(T)));
        //}
    }
}
