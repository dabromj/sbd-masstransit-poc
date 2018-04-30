using SBD.MassTransit.POC.Messages.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBD.MassTransit.POC.Publisher.Models
{
    /// <summary>
    /// BaseMessage provides the definition of required message properties and their validation.
    /// </summary>
    public class BaseMessage : IBaseMessage, IValidatableObject
    {
        /// <summary>
        /// Key field used to identify record in external system
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string ExternalSystemKey { get; set; }
        /// <summary>
        /// Id used to identify external system
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string ExternalSystemId { get; set; }
        /// <summary>
        /// Flag to signal record was removed from external system
        /// </summary>
        public string IsRemoved { get; set; }
        /// <summary>
        /// Last modifed timestamp in external system
        /// </summary>
        public string LastModifiedOn { get; set; }

        /// <summary>
        /// Provides a method for applying custom validation logic for MessageBase properties.
        /// </summary>
        /// <param name="validationContext">The validation context</param>
        /// <returns>A collection that holds failed-validation information</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            try
            {
                // SourceSystemKey
                if (string.Equals("null", ExternalSystemKey, StringComparison.CurrentCultureIgnoreCase))
                    results.Add(new ValidationResult("The ExternalSystemKey field is required.", new[] { "ExternalSystemKey" }));

                // SourceKey
                if (string.Equals("null", ExternalSystemId, StringComparison.CurrentCultureIgnoreCase))
                    results.Add(new ValidationResult("The ExternalSystemId field is required.", new[] { "ExternalSystemId" }));

                return results;
            }
            catch (Exception e)
            {
                throw new Exception("BaseMessage.Validate : " + e.Message);
            }
        }
    }
}