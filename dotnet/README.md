# Welcome to your CDK C# project for Lambda & API Gateway (recommended Windows machine)

This project is C# version of CDK to deploy Lambda with Api Gateway

The `cdk.json` file tells the CDK Toolkit how to execute your app.

.NET Version 6

It uses the [.NET Core CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template

## Steps to run the project
* Get the code using `git clone https://github.com/khanasif1/awscdk-apigateway-stage.git` 
* Open terminal/powershell or open solution in Visual Studio
* For terminal navigate to `{your folder path }\\awscdk-apigateway-stage\dotnet\src` &  `dotnet build`
* Once build is success
* Navigate to termainla/shell `{your folder path }\\awscdk-apigateway-stage` 
* Bootstrap CDK `cdk bootstrap`
* Next `cdk synth`
* You should have a CloudFormation Template `CdkdotnetStack.template` in folder `cdk.out`
* Connect to your aws account using `aws configure`
* Finally `cdk deploy`
* Navigate to console --> CloudFormation you should see a new stack CdkdotnetStack