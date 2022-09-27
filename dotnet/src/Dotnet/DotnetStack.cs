using Constructs;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using System.Collections.Generic;
using Amazon.CDK.AWS.DynamoDB;
using System;
//cdk synth --app "dotnet  bin/Dotnet.dll"    
// https://www.xerris.com/insights/creating-an-aws-serverless-c-dotnet-application-s3-dynamodb-api-gateway-lambdas-c-sharp-and-cdk/ 
namespace Dotnet
{
    public class DotnetStack : Stack
    {
        internal DotnetStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // The code that defines your stack goes here
            //Stage setting for Deployment (Need to have Deploy = false in RestApiProps to configure the Stage
            string environment = "PRD";

           //STORAGE INFRASTRUCTURE
            // default RemovalPolicy is RETAIN (on a "cdk destroy")
            //Create an S3 Bucket (s3 Buckets must be unique for each region)
            //S3 Buckets must be unique by region
            //NOTE: If you put objects in this S3 bucket you will be required to delete it manually
            var rand = new Random();
            int randNumber = rand.Next(100000);
            var s3Bucket = new Bucket(this, "MyS3Bucket", new BucketProps
            {
                BucketName = (environment+"-MyS3Bucket"+randNumber).ToLower(),
                RemovalPolicy = RemovalPolicy.DESTROY
            });

             //COMPUTE INFRASTRUCTURE
            //Basic Lambda
            var simpleLambdaHandler = new Function(this, "simpleLambdaHandler", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                FunctionName = "simpleLambda",
                //Where to get the code
                Code = Code.FromAsset("src\\LambdaApi\\src\\LambdaApi\\bin\\Debug\\net6.0"),
                Handler = "LambdaApi::LambdaApi.Function::FunctionHandler",
                Environment = new Dictionary<string, string>
                {
                    ["ENVIRONMENT"] = environment,
                    ["BUCKET"] = s3Bucket.BucketName
                }
            });
            s3Bucket.GrantReadWrite(simpleLambdaHandler);

            //S3 Lambda
            //var s3LambdaHandler = new Function(this, "s3LambdaHandler", new FunctionProps
            //{
            //    Runtime = Runtime.DOTNET_CORE_3_1,
            //    Timeout = Duration.Seconds(20), 
            //    FunctionName = "s3Lambda",
            //    //Where to get the code
            //    Code = Code.FromAsset("LambdaApi\\src\\LambdaApi\\bin\\Debug\\netcoreapp3.1"),
            //    Handler = "Lambdas::Lambdas.Function::S3LambdaHandler",
            //    Environment = new Dictionary<string, string>
            //    {
            //        ["ENVIRONMENT"] = environment,
            //        ["BUCKET"] = s3Bucket.BucketName,
            //        ["REGION"] = this.Region
            //    }
            //});
            //s3Bucket.GrantReadWrite(s3LambdaHandler);




            //This is the name of the API in the APIGateway
            var api = new RestApi(this, "CDKAPI", new RestApiProps
            {
                RestApiName = "cdkAPI",
                Description = "This our CDKAPI",
                Deploy = false
            });

            var deployment = new Deployment(this, "My Deployment", new DeploymentProps { Api=api });
            var stage = new Amazon.CDK.AWS.APIGateway.Stage(this, "stage name", new Amazon.CDK.AWS.APIGateway.StageProps {
                Deployment = deployment,
                StageName = environment
            });
            api.DeploymentStage = stage;

            //Lambda integrations
            var simpleLambdaIntegration = new LambdaIntegration(simpleLambdaHandler, new LambdaIntegrationOptions
            {
                RequestTemplates = new Dictionary<string, string>
                {
                    ["application/json"] = "{ \"statusCode\": \"200\" }"
                }
            });
            
            //var s3LambdaIntegration = new LambdaIntegration(s3LambdaHandler, new LambdaIntegrationOptions
            //{
            //    RequestTemplates = new Dictionary<string, string>
            //    {
            //        ["application/json"] = "{ \"statusCode\": \"200\" }"
            //    }
            //});



            //It is up to you if you want to structure your lambdas in separate APIGateway APIs (RestApi)

            //Option 1: Adding at the top level of the APIGateway API
            // api.Root.AddMethod("POST", simpleLambdaIntegration);

            //Option 2: Or break out resources under one APIGateway API as follows
            var simpleResource = api.Root.AddResource("simple");
            var simpleMethod = simpleResource.AddMethod("POST", simpleLambdaIntegration);
            var s3Resource = api.Root.AddResource("s3");
            //var s3Method = s3Resource.AddMethod("POST", s3LambdaIntegration);



            //Output results of the CDK Deployment
            new CfnOutput(this, "A Region:", new CfnOutputProps() { Value = this.Region });
            new CfnOutput(this, "B S3 Bucket:", new CfnOutputProps() {Value = s3Bucket.BucketName});
            new CfnOutput(this, "C API Gateway API:", new CfnOutputProps() { Value = api.Url });
            string urlPrefix = api.Url.Remove(api.Url.Length - 1);
            new CfnOutput(this, "D Simple Lambda:", new CfnOutputProps() { Value = urlPrefix + simpleMethod.Resource.Path });
            //new CfnOutput(this, "E S3 Lambda:", new CfnOutputProps() { Value = urlPrefix + s3Method.Resource.Path });

        }
    }
}
