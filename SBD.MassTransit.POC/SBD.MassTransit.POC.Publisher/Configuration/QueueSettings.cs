using SBD.MassTransist.POC.Intergration.Configuration;
using System.Configuration;

namespace SBD.MassTransit.POC.Publisher.Configuration
{
    /// <summary>
    /// Concrete implementation of IQueueSettings.  Publisher will read from appSettings for most of the queue properties.
    /// </summary>
    public class QueueSettings : IQueueSettings
    {
        private string _host;
        /// <summary>
        /// Hostname of target RabbitMQ instance
        /// </summary>
        public string Host
        {
            get => ConfigurationManager.AppSettings["RabbitMqHost"];
            set => _host = value;
        }

        private string _rabbitMqUri;
        /// <summary>
        /// Uri of target RabbitMQ instance including port
        /// </summary>
        public string RabbitMqUri
        {
            get => ConfigurationManager.AppSettings["RabbitMqUri"];
            set => _rabbitMqUri = value;
        }

        private string _virtualHost;
        /// <summary>
        /// Virtual host of the queue
        /// </summary>
        public string VirtualHost
        {
            get => _virtualHost;
            set => _virtualHost = value;
        }

        private string _queueName;
        /// <summary>
        /// Uri of target RabbitMQ instance including port
        /// </summary>
        public string QueueName
        {
            get => _queueName;
            set => _queueName = value;
        }

        private string _username;
        /// <summary>
        /// Username for RabbitMQ instance
        /// </summary>
        public string UserName
        {
            get => ConfigurationManager.AppSettings["UserName"];
            set => _username = value;
        }

        private string _password;
        /// <summary>
        /// Password for RabbitMQ instance
        /// </summary>
        public string Password
        {
            get => ConfigurationManager.AppSettings["Password"];
            set => _password = value;
        }

        private string _schedulerQueueName;
        /// <summary>
        /// Password for RabbitMQ instance
        /// </summary>
        public string SchedulerQueueName
        {
            get => ConfigurationManager.AppSettings["SchedulerQueueName"];
            set => _schedulerQueueName = value;
        }
    }
}