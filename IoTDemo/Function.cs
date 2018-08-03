using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Amazon.Lambda.Core;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace IoTDemo
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a IoTButton click event and sends to Kinesis Data Stream
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(JObject input, ILambdaContext context)
        {
            LambdaLogger.Log(input.ToString());
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(input.ToString()) + Environment.NewLine);
            var stream = new MemoryStream(byteArray);

            using (var kinesisClient = new AmazonKinesisClient(Amazon.RegionEndpoint.USEast1))
            {
                Task.WaitAll(kinesisClient.PutRecordsAsync(new PutRecordsRequest()
                {
                    StreamName = "MyButtonStream",
                    Records = new List<PutRecordsRequestEntry>() { new PutRecordsRequestEntry() { Data = stream, PartitionKey = "AAA" } }
                }));
            }
        }
    }
}
