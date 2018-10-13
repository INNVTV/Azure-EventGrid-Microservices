# Azure EventGrid Microservices
A proof of concept showcasing Azure EventGrid communication between two microservices (publisher/subscriber) as well as a message queue that is read by a validator:

### **Publisher:** Sends messages to Event Grid (Console).

### **Subscriber:** Subscribes to and acts upon received events via webhook. Sends a message to a storage queue for the validator to pick up (WebApi).

### **Validator:** Checks message queue to verify that events are flowing through the system.

# Architecture
Here is the archtecture of the composed microservices:

![Architecture](https://github.com/INNVTV/Azure-EventGrid-Microservices/blob/master/_docs/imgs/architecture.png)


# Running Sample:
Update the .env file with your Azure Event Grid settings

**IMAGE HERE**

You should also update the appsettings.json file in both projects if debugging the projects seperatly:

**IMAGE HERE**

Build and run using Docker Compose:

     docker-compose build
     docker-compose up

You will see both console applications emit their status in your output window:

**IMAGE HERE**

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


Each topic will have a unique access key and topic endpoint. These should be updated in the global .env file and the appsettings.json files within each console app.

### **Subscribe to custom topic**
You subscribe to an event grid topic to tell Event Grid which events you want to track, and where to send the events.