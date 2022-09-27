# Welcome to your CDK C# project!

This is a blank project for CDK development with C#.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET Core CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template


cd ~/_code/_git/awscdk-apigateway-stage/dotnet --> Mac
cd C:/User/...../awscdk-apigateway-stage/dotnet --> Windows
cdk bootstrap --app "dotnet  bin/Dotnet.dll" 
cdk synth --app "dotnet  bin/Dotnet.dll"     
cdk deploy --app "dotnet  bin/Dotnet.dll"  