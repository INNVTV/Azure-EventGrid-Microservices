# Azure EventGrid Microservices
A proof of concept showcasing Azure EventGrid communication between two microservices:

### **Publisher:** Sends messages to Event Grid. ###

### **Subscriber:** Subscribes to and acts upon received messages. ###

# Running Sample:
Update the .env file with your Azure Event Grid settings:

**IMAGE HERE**

You should also update the appsettings.json file in both projects if debugging the projects seperatly:

**IMAGE HERE**

Build and run using Docker Compose:

     docker-compose build
     docker-compose up

You will see both console applications emit their status in your output window:

**IMAGE HERE**

# Azure Portal Setup #

If you haven't previously used Event Grid in your Azure subscription, you may need to register the Event Grid resource provider.

**In the Azure portal:**

Select Subscriptions.
Select the subscription you're using for Event Grid.
Under Settings, select Resource providers.
Find Microsoft.EventGrid.
If not registered, select Register:

[[https://github.com/INNVTV/Azure-EventGrid-Microservices/imgs/portal-resource-providers.png]]
