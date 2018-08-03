# Purpose
The solution creates Lambda function using C# that processes events from a AWS IoT Button and publishes the events to a Kinesis Stream. The Streaming data is further pushed to a S3 bucket through Kinesis Firehose.

# Technologies used
1. AWS Lambda
2. Amazon Kinesis Stream
3. Amazon Kinesis Firehose

# Setup Instructions
## Create a Kinesis Stream
* Go to the Amazon Kinesis home page on the AWS Console
* Under _Data Streams_, click on **Create Kinesis Stream**
* Give the Kinesis stream a name and enter **1**  for the _Number of Shards_ text field
* Click on **Create Kinesis Stream** button to create the stream
## Create a Kinesis Firehose
* Click _Data Firehose_ on the Amazon Kinesis home page and click button **Create delivery stream**
* Give the delivery stream a name
* Choose **Kinesis Stream** as source and in the select the Kinesis stream you created in the earlier step as the source
* Click **Next** and leave everything as is on this page and click **Next** again
* Select **Amazon S3** under _Select Destination_
* Choose the S3 bucket you want the stream data to be published to. You can also enter a prefix for the filenames that will be created on S3 if you wish so and click **Next**
* Change _Buffer interval_ to **60** for demo purposes. This helps the streams gets processed faster
* Follow the instructions to create a new IAM Role or use choose an existing one as approprite and click **Next**
* Click **Create delivery stream** in the _Review_ screen

## Project setup (for Visual Studio on Windows with AWS Toolkit installed)
* Clone this repo on your machine
* Change the **StreamName** property to the name of the Kinesis Data Stream you created in the first step 
* Make sure the AWS region is appropriate as well
```
using (var kinesisClient = new AmazonKinesisClient(Amazon.RegionEndpoint.USEast1))
            {
                Task.WaitAll(kinesisClient.PutRecordsAsync(new PutRecordsRequest()
                {
                    StreamName = "MyButtonStream",
                    Records = new List<PutRecordsRequestEntry>() { new PutRecordsRequestEntry() { Data = stream, PartitionKey = "AAA" } }
                }));
            }
```
* Publish the Lambda function to AWS by right clicking on the project and choosing **Publish to AWS Lambda**
* Make sure the Lambda function has is assigned to a Role that has access to the following policies
   > - AWSLambdaFullAccess
   > - AWSKinesisFullAccess
## Setup your AWS IoTButton
* [Follow this instructions](https://docs.aws.amazon.com/iot/latest/developerguide/configure-iot.html) to setup your IoTButton
* Once setup, you should be able to see the Lambda function on the AWS Button phone app. Configure the button to send the events to the lambda function by associating the function in the app
* Now the button is all set to send events to the Lambda function you just created
# Test it all out
* Press the button one to send a _SINGLE_ event, twice to send a _DOUBLE_ event and long press to send a _LONG_ event 
* Go to CloudWatch logs and see the events getting logged. Here is a sample event you should be able to see
 
> ```{ "serialNumber": "G030JF051206XX00", "batteryVoltage": "1631mV", "clickType": "SINGLE" }```
* Go to the S3 bucket you configured to receive Kinesis Firehose stream and check the files getting created

