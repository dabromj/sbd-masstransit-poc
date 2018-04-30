using SBD.MassTransit.POC.Messages;

namespace SBD.MassTransit.POC.Publisher.Models
{
    /// <summary>
    /// Concrete implementation of ISaveAgency message
    /// </summary>
    public class SaveAgency : ISaveAgency
    {
        /// <summary>
        /// Secondary key used to help identify agency
        /// </summary>
        public string SecondarySourceKey { get; set; }
        /// <summary>
        /// Agency type
        /// </summary>
        public string AgencyType { get; set; }
        /// <summary>
        /// Name of agency
        /// </summary>
        public string AgencyName { get; set; }
        /// <summary>
        /// Line one of address
        /// </summary>
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Line two of address
        /// </summary>
        public string AddressLine2 { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Zip
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// Primary phone number (10 digit)
        /// </summary>
        public string Phone1 { get; set; }
        /// <summary>
        /// Alternate phone number (10 digit)
        /// </summary>
        public string Phone2 { get; set; }
        /// <summary>
        /// Key field used to identify record in external system
        /// </summary>
        public string ExternalSystemKey { get; set; }
        /// <summary>
        /// Id used to identify external system
        /// </summary>
        public string ExternalSystemId { get; set; }
        /// <summary>
        /// Last modifed timestamp in external system
        /// </summary>
        public string LastModifiedOn { get; set; }
        /// <summary>
        /// Flag to signal record was removed from external system
        /// </summary>
        public string IsRemoved { get; set; }
    }
}