using SBD.MassTransit.POC.Messages.Base;

namespace SBD.MassTransit.POC.Messages
{
    /// <summary>
    /// Interface to implement the SaveAgency message type
    /// </summary>
    public interface ISaveAgency : IBaseAddress, IBaseMessage
    {
        /// <summary>
        /// Secondary key used to help identify agency
        /// </summary>
        string SecondarySourceKey { get; }
        /// <summary>
        /// Agency type
        /// </summary>
        string AgencyType { get; }
        /// <summary>
        /// Name of agency
        /// </summary>
        string AgencyName { get; }        
    }
}
