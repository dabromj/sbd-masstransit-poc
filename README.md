# MassTransit -> RabbitMQ web publisher proof of concept.

Proof of concept work for MassTransit -> RabbitMQ generic web publisher.  The goal of this project was to find a way to follow the MassTransit Send pattern in a fashion that did not require many type-specific operations or complex internal mapping logic.

My intention for putting this on github was to show the work that was done and inspire others looking for a similiar solution.  This is not intended to evolve further than a proof of concept on github.

The backend of the process where a document is actually picked up by a consuing queue is not included in the project at the time.

### To run the project.

- Open SBD.MassTransit.POC.sln
- Run the SBD.MassTransit.POC.Publisher project.
- Invoke the /api/message method using the Swagger interface.

#### Prerequisites

- A valid RabbitMQ setup either locally or deployed
- A queue and exchange created that is valid for your message type

There is passive validation being done to see if the exchange and queues exist in RabbitMQ.  They should be created before running the project.

##### In order to submit messages you will need a local or configured RabbitMQ instance.  Update the publisher web.config with your settings.

	<appSettings>
    <add key="RabbitMqHost" value="localhost" />
    <add key="RabbitMqUri" value="rabbitmq://localhost/" />
    <add key="UserName" value="local" />
    <add key="Password" value="password" />
    <add key="SchedulerQueueName" value="Quartz_Scheduler" />
    </appSettings>
    
##### Example request:
    
    POST /message
    {
       "MessageType": "SaveAgency",
       "DestinationId": "local",
       "ScheduledTime": "2018-04-12T05:00:00.000Z",
       "Body": 
        {
            "ExternalSystemKey" : "MAS",
            "ExternalSystemId" : "001",
            "LastModifiedOn" : "2018-03-25T05:00:00.000Z",
            "IsRemoved" : "false",
            "AddressLine1" : "8450 Sunlight Dr.",
            "AddressLine2" : null,
            "City" : "Fishers",
            "State" : "IN",
            "Zip" : "46037",
            "Phone1" : "3175555555",
            "Phone2" : null,
            "SecondarySourceKey" : "002",
            "AgencyType" : "SOC",
            "AgencyName" : "Fisher's SOC"
            }
    }
 
 ##### References 
 - [MassTransit](http://masstransit-project.com/)
 - [RabbitMQ](https://www.rabbitmq.com/)