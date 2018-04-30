using MassTransit;
using MassTransit.EntityFrameworkIntegration.Audit;
using RabbitMQ.Client;
using SBD.MassTransit.POC.Integration.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SBD.MassTransit.POC.Integration
{
    /// <summary>
    /// BasePublisher sends a message to specified queue via MassTransit.
    /// </summary>
    public class BasePublisher
    {
        /// <summary>
        /// Send provided message to the specified queue.
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="queueSettings">QueueSettings ojbect</param>
        /// <param name="message">Message</param>
        /// <param name="auditConnectionStringName">string Name of Connection String</param>
        /// <param name="auditSchemaName">string Schema Name</param>
        /// <param name="auditTableName">string Table Name (AuditRecord)</param>
        /// <returns>Task object</returns>
        public static async Task SendMessage<T>(IQueueSettings queueSettings, T message, DateTime? scheduledTime = null, string auditConnectionStringName = null, string auditSchemaName = null, string auditTableName = null) where T : class
        {
            try
            {
                //Check if the exchange and queues exist
                if (ValidateExchange(queueSettings, message))
                {
                    // Configure bus
                    var bus = ConfigureBus(queueSettings);

                    // Audit
                    // TODO: Audit store not implemented in POC, but all that really needs to be done is add a connection string to the config
                    // and point it to a table that matches the MassTransit audit table schema, MassTransit EntityFrameworkAuditStore will take care of creating the connection and writing to the table
                    if (auditConnectionStringName != null)
                        bus.ConnectSendAuditObservers(new EntityFrameworkAuditStore(auditConnectionStringName, auditSchemaName + "." + auditTableName), null);

                    // Helper function to construct destination uri
                    var sendToUri = SendToUri(queueSettings);

                    // If a scheduled time is provided, set the queue endpoint to the scheduler queue name other set it to the message type
                    var endPoint = (scheduledTime.HasValue) ?
                        await bus.GetSendEndpoint(new Uri(queueSettings.RabbitMqUri + queueSettings.VirtualHost + "/" + queueSettings.SchedulerQueueName))
                        : await bus.GetSendEndpoint(new Uri(sendToUri + queueSettings.QueueName));

                    //Start the bus
                    await bus.StartAsync();

                    // If a scheduled time is provided, MassTransit provides a ScheduleSend message to use, otherwise simply use Send
                    if (scheduledTime.HasValue)
                        await endPoint.ScheduleSend(new Uri(sendToUri + queueSettings.QueueName), scheduledTime.Value, message);
                    else
                        await endPoint.Send(message);

                    //Stop the bus
                    await bus.StopAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception("BasePublisher.SendMessage : " + e.Message);
            }
        }

        /// <summary>
        /// Build SendToUri based on QueueSettings.
        /// </summary>
        /// <param name="queueSettings">QueueSettings</param>
        /// <returns>string Uri</returns>
        private static string SendToUri(IQueueSettings queueSettings)
        {
            try
            {
                var uri = new StringBuilder(queueSettings.RabbitMqUri);
                uri.Append(string.IsNullOrEmpty(queueSettings.VirtualHost) ? "" : queueSettings.VirtualHost + "/");
                return uri.ToString();
            }
            catch (Exception e)
            {
                throw new Exception("BasePublisher.SendToUri : " + e.Message);
            }

        }

        /// <summary>
        /// Configure bus based on QueueSettings.
        /// </summary>
        /// <param name="queueSettings">QueueSettings</param>
        /// <returns>IBusControl</returns>
        private static IBusControl ConfigureBus(IQueueSettings queueSettings)
        {
            try
            {
                return BusConfigurator.ConfigureBus(queueSettings);
            }
            catch (Exception e)
            {
                throw new Exception("BasePublisher.ConfigureBus : " + e.Message);
            }
        }

        /// <summary>
        /// Verify the exchange infrastructure exists.
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="queueSettings">QueueSettings</param>
        /// <param name="message">Message</param>
        /// <returns>bool</returns>
        private static bool ValidateExchange<T>(IQueueSettings queueSettings, T message)
        {
            try
            {
                //Setup RabbitMQ connection
                var factory = new ConnectionFactory()
                {
                    HostName = queueSettings.Host,
                    VirtualHost = queueSettings.VirtualHost,
                    UserName = queueSettings.UserName,
                    Password = queueSettings.Password
                };

                //Connect to RabbitMQ, will throw an exception for various reasons such as invalid credentials or trying to connect to an invalid virtual host
                using (var connection = factory.CreateConnection())
                {
                    using (var model = connection.CreateModel())
                    {
                        ///PASSIVE QUEUE DECLARATION
                        ///Using the passive declare methods, an exception is thrown if the queue/exchange does not exist
                        ///This is important because we..
                        /// 1) Want to throw an appropriate exception if the queue doesnt exist
                        /// 2) Not allow automatic creation of queues based entirely on user input
                        /// 3) If the queue already exists, no exception is thrown
                        var messageContract = message.GetType().GetInterface("I" + message.GetType().Name);                        
                        model.ExchangeDeclarePassive(queueSettings.QueueName);
                        model.QueueDeclarePassive(queueSettings.QueueName);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("BasePublisher.VerifyExchangeExists : " + e.Message);
            }
        }
    }
}
