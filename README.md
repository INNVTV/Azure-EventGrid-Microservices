[![Build Status](https://dev.azure.com/Github-Samples/Azure-EventGrid-Microservices/_apis/build/status/INNVTV.Azure-EventGrid-Microservices)](https://dev.azure.com/Github-Samples/Azure-EventGrid-Microservices/_build/latest?definitionId=1)

# Azure EventGrid Microservices
A template showcasing Azure EventGrid communication between two microservices (publisher/subscriber) as well as a message queue that is read by a validator.

![Architecture](https://github.com/INNVTV/Azure-EventGrid-Microservices/blob/master/_docs/imgs/architecture.png)


# Publisher:
A console app that sends test notifications to Event Grid topics in batches on a set schedule.

# Subscriber:
A web api that subscribes to and acts upon received events via webhooks. Once a notification is received it sends a message to a storage queue for the validator to pick up.

# Validator:
A console app that polls the storage queue, verifies that events are flowing through the system and writes status updates to the console.

# Running Sample:
Update the appsettings.json and .env files in the following locations with your Azure Event Grid and Azure Storage Account settings:

    .env
    Publisher/appsettings.json
    Subscriber/appsettings.json
    Validator/appsettings.json

Deploy the WebApi project to Azure. For our sample we have an Azure Pipelines project set up for the deployment:

https://dev.azure.com/github-samples/azure-eventgrid-microservices/_build

The build process in outlined within [**azure-pipelines.yml**](azure-pipelines.yml)

Docker Compose will only build the 2 console apps to run locally. *See architecture above*. The WebApi should be hosted on Azure and linked to your topics as a WebHook using GridEvent types.

Build and run using Docker Compose:

     docker-compose build
     docker-compose up

You should see both console applications emit their status in your output window as events pass through the grid and are processed by the Subscriber webhook and validated via message queues:

![Portal Resource Providers](https://github.com/INNVTV/Azure-EventGrid-Microservices/blob/master/_docs/imgs/terminal.png)

# Azure Portal Setup

If you haven't previously used Event Grid in your Azure subscription, you may need to register the Event Grid resource provider.

**In the Azure portal:**

Select Subscriptions.
Select the subscription you're using for Event Grid.
Under Settings, select Resource providers.
Find Microsoft.EventGrid.
If not registered, select Register:

![Portal Resource Providers](https://github.com/INNVTV/Azure-EventGrid-Microservices/blob/master/_docs/imgs/portal-resource-providers.png)

Create your topics using "Event Grid Topic" resource type:

![Event Grid Topic](https://github.com/INNVTV/Azure-EventGrid-Microservices/blob/master/_docs/imgs/event-grid-topic.png)


Each topic will have a unique access key and topic endpoint. These should be updated in the global .env file and the appsettings.json files within the Publisher console app so that the publisher can send notifications to each topic.

**The Subscriber webapi project provides a webhook for each topic exposing only a [post] method per topic:**

https://event-grid-subscriber.azurewebsites.net/webhook/topic1

https://event-grid-subscriber.azurewebsites.net/webhook/topic2

You will need to configure each EventGrid topic to call the associated webhook by configuring the Event Subscription settings within the Azure Portal. Keep in mind that GridEvent types are sent as an array while the CloudEvent type is sent one at a time.

![Event Grid Subscription](https://github.com/INNVTV/Azure-EventGrid-Microservices/blob/master/_docs/imgs/event-grid-subscription.png)


Every time someone subscribes to an event, Event Grid sends a validation event to the endpoint with a validationCode in the data payload. The endpoint is required to echo this back in the response body to prove the endpoint is valid and owned by you. 

The Validator and Subscriber will only need the access keys, storage & queue names for the storage queue resource so that they can communicate via message queues to validate event notifications are passing through the system.