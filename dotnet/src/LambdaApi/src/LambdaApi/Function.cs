using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using System.Net;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaApi;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public object FunctionHandler(string input, ILambdaContext context)
    {
        //return input.ToUpper();
         bool success = true;
            string message = "";
            string responseText = "";
            string requestBody = "";
            try
            {
                string environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
                responseText += "SimpleLambda CDK Lambda call at " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss") + " ";
                responseText += "with Environment variable=" + environment;

                //Reading the incoming request body to show we received it
                var request = JObject.Parse("" + input);
                requestBody = request["body"].ToString();
            }
            catch (Exception exc)
            {
                message+= "SimpleLambdaHandler Exception:" + exc.Message + "," + exc.StackTrace;
                success = false;
            }

            //create the responseBody for the response
            string responseBody = "{\n";
            responseBody += " \"request\":" + requestBody + ",\n";
            responseBody += " \"response\":\"" + responseText + "\",\n";
            responseBody += " \"success\":\"" + success + "\",\n";
            responseBody += " \"message\":\"" + message + "\"\n";
            responseBody += "}";

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = responseBody,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
            return response;
    }
}
