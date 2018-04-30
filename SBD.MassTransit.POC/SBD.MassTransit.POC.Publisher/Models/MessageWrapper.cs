using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBD.MassTransit.POC.Publisher.Models
{
    /// <summary>
    /// MessageWrapper provides the envelope for messages being delivered via the publish API.
    /// </summary>
    public class MessageWrapper : IValidatableObject
    {
        /// <summary>
        /// /// Message Type (SaveAgency, SaveSite, DeleteSite, etc)
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string MessageType { get; set; }

        /// <summary>
        /// Destination ID (Virtual Host)
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string DestinationId { get; set; }

        /// Body - The Actual Message
        [Required]
        public object Body { get; set; }

        /// Scheduled Time (If null, process immediately otherwise pass to scheduler)
        public DateTime? ScheduledTime { get; set; }

        /// <summary>
        /// Apply custom validation logic for MessageWrapper properties.
        /// </summary>
        /// <param name="validationContext">The validation context</param>
        /// <returns>A collection that holds failed-validation information</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            try
            {
                // MessageType
                if (string.Equals("null", MessageType, StringComparison.CurrentCultureIgnoreCase))
                    results.Add(new ValidationResult("The MessageType field is required.", new[] { "MessageType" }));


                // DestinationId
                if (string.Equals("null", DestinationId, StringComparison.CurrentCultureIgnoreCase))
                    results.Add(new ValidationResult("The DestinationId field is required.", new[] { "DestinationId" }));

                // Validate message using BaseMessage
                var baseMessage = (BaseMessage)JsonConvert.DeserializeObject(Body.ToString(), typeof(BaseMessage));
                if (baseMessage == null)
                {
                    results.Add(new ValidationResult("The Body field is required.", new[] { "Body" }));
                }
                else
                {
                    //We will always need at least the data contained within the BaseMessage so we validate that
                    var subResults = new List<ValidationResult>();
                    var isValidBody = Validator.TryValidateObject(baseMessage, new ValidationContext(baseMessage, null, null), subResults);
                    if (!isValidBody)
                    {
                        results.Add(new ValidationResult(subResults[0].ErrorMessage, new[] { "Body." + ((string[])subResults[0].MemberNames)[0] }));
                    }
                }

                return results;
            }
            catch (Exception e)
            {
                throw new Exception("MessageWrapper.Validate : " + e.Message);
            }
        }
    }
}