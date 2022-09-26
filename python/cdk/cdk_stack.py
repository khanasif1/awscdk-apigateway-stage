from crypt import methods
from hashlib import new
from logging import root
from aws_cdk import (
    Stack,
    aws_lambda as _lambda,
    aws_apigateway as apigw,
    aws_iam as iam
)
from constructs import Construct

class CdkServerlessStack(Stack):

    def __init__(self, scope: Construct, construct_id: str, **kwargs) -> None:
        super().__init__(scope, construct_id, **kwargs)

        # The code that defines your stack goes here
        lambdaApi = _lambda.Function(
            self, 'lambdaAPI',
            runtime=_lambda.Runtime.PYTHON_3_7,
            code=_lambda.Code.from_asset('lambda'),
            handler='hello.handler',
        )
        principal = iam.ServicePrincipal("apigateway.amazonaws.com") 
        lambdaApi.grant_invoke(principal)

        dev_deployment = apigw.StageOptions(
            stage_name="dev",
            variables={
                "lambdaAlias":"dev"
            }
        )
        api = apigw.LambdaRestApi(
            self,"devLambdaApiendpoint",
            handler=lambdaApi,
            deploy_options=dev_deployment,
            endpoint_types=[apigw.EndpointType.REGIONAL]
            
        )
        prod_deployment = apigw.Deployment(
            self, "ProdDeployment16822",
            api=api,
            retain_deployments=True,
            description="Lambda Prod deployment"
        )
        prod_stage = apigw.Stage(
            self, "ProdStage",
            deployment=prod_deployment,
            stage_name="prod",
            variables={
                "lambdaAlias": "prod"
            }
        )
        #api.deployment_stage = prod_stage