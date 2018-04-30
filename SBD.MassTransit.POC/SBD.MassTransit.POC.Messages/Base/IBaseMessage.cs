namespace SBD.MassTransit.POC.Messages.Base
{
    /// <summary>
    /// Base interface used to implement common properties to help identify external systems
    /// </summary>
    public interface IBaseMessage
    {
        /// <summary>
        /// Key field used to identify record in external system
        /// </summary>
        string ExternalSystemKey { get; }
        /// <summary>
        /// Id used to identify external system
        /// </summary>
        string ExternalSystemId { get; }
        /// <summary>
        /// Last modifed timestamp in external system
        /// </summary>
        string LastModifiedOn { get; }         
        /// <summary>
        /// Flag to signal record was removed from external system
        /// </summary>
        string IsRemoved { get; }
    }
}
