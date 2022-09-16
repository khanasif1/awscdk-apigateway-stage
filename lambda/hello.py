import json

def handler(event, context):
    print('request: {}'.format(json.dumps(event)))
    stage = event['stageVariables']['lambdaAlias']
    #print('Stage varibale :{}'.format(stage))

    return {
        'statusCode': 200,
        'headers': {
            'Content-Type': 'text/plain'
        },
        'body': 'Hello, CDK! You have hit {}\n'.format(event['path'])+' Stage Name: {}'.format(stage)
    }
#'body': 'Hello, CDK! You have hit {}\n'.format(event['path']+' Stage Name: {}'.format(stage))