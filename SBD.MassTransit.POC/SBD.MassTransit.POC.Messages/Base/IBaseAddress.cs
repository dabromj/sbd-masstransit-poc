namespace SBD.MassTransit.POC.Messages.Base
{
    /// <summary>
    /// Base interface used to implement common properties for any contracts that need an address
    /// </summary>
    public interface IBaseAddress
    {
        /// <summary>
        /// Line one of address
        /// </summary>
        string AddressLine1 { get; }
        /// <summary>
        /// Line two of address
        /// </summary>
        string AddressLine2 { get; }
        /// <summary>
        /// City
        /// </summary>
        string City { get; }
        /// <summary>
        /// State
        /// </summary>
        string State { get; }
        /// <summary>
        /// Zip (6 digit)
        /// </summary>
        string Zip { get; }
        /// <summary>
        /// Primary phone number (10 digit)
        /// </summary>
        string Phone1 { get; }
        /// <summary>
        /// Alternate phone number (10 digit)
        /// </summary>
        string Phone2 { get; }
    }
}
