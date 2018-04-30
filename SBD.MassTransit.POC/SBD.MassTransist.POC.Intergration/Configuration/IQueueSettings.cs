namespace SBD.MassTransist.POC.Integration.Configuration
{
    /// <summary>
    /// Interface to ensure that connected components provide all the information required by the base class components.
    /// </summary>
    public interface IQueueSettings
    {
        /// <summary>
        /// Host name of RabbitMQ instance
        /// </summary>
        string Host { get; set; }
        /// <summary>
        /// Base URI
        /// </summary>
        string RabbitMqUri { get; set; }
        /// <summary>
        /// Virtual Host
        /// </summary>
        string VirtualHost { get; set; }
        /// <summary>
        /// Scheduler Queue Name
        /// </summary>
        string SchedulerQueueName { get; set; }
        /// <summary>
        /// Queue Name
        /// </summary>
        string QueueName { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        string Password { get; set; }
    }
}
