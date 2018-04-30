using SBD.MassTransist.POC.Integration;
using SBD.MassTransit.POC.Publisher.Configuration;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SBD.MassTransit.POC.Publisher.Controllers
{
    /// <summary>
    /// BaseController provides common functionality used by application controllers.
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// GetQueueSettings creates a QueueSettings object for the provided virtual host and queue.
        /// </summary>
        /// <param name="virtualHost">string</param>
        /// <param name="queueName">string</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected QueueSettings GetQueueSettings(string virtualHost, string queueName)
        {
            return new QueueSettings() { VirtualHost = virtualHost, QueueName = queueName };
        }

        /// <summary>
        /// SendMessage provides a common method for sending messages to their queues.
        /// </summary>
        /// <typeparam name="T">Type of Message</typeparam>
        /// <param name="queueSettings">QueueSettings</param>
        /// <param name="message">Message</param>
        /// <param name="scheduledTime">Nullable, Scheduled time to send message to target queue</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected Task SendMessage<T>(QueueSettings queueSettings, T message, DateTime? scheduledTime = null) where T : class
        {
            try
            {
                return BasePublisher.SendMessage(queueSettings,
                    message, scheduledTime);
            }
            catch (Exception e)
            {
                throw new Exception("BaseController.SendMessage : " + e.Message);
            }

        }
    }
}