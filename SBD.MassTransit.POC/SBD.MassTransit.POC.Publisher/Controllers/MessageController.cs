using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using SBD.MassTransit.POC.Publisher.Models;

namespace SBD.MassTransit.POC.Publisher.Controllers
{
    /// <summary>
    /// MessageController accepts a MessageWrapper object and delivers the message it 
    /// contains to the proper virtual host and message queue.
    /// </summary>
    public class MessageController : BaseController
    {
        /// <summary>
        /// Accepts a MessageWrapper object and delivers the message it contains to the proper 
        /// virtual host and message queue.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /message
        ///     {
        ///        "MessageType": "SaveAgency",
        ///        "DestinationId": "local",
        ///        "ScheduledTime": "2018-04-12T05:00:00.000Z",
        ///        "Body": 
        ///         {
        ///             "ExternalSystemKey" : "MAS",
        ///             "ExternalSystemId" : "001",
        ///             "LastModifiedOn" : "2018-03-25T05:00:00.000Z",
        ///             "IsRemoved" : "false",
        ///             "AddressLine1" : "8450 Sunlight Dr.",
        ///             "AddressLine2" : null,
        ///             "City" : "Fishers",
        ///             "State" : "IN",
        ///             "Zip" : "46037",
        ///             "Phone1" : "3175555555",
        ///             "Phone2" : null,
        ///             "SecondarySourceKey" : "002",
        ///             "AgencyType" : "SOC",
        ///             "AgencyName" : "Fisher's SOC"
        ///             }
        ///     }
        ///
        /// </remarks>
        /// <param name="messageWrapper">MessageWrapper object</param>
        /// <returns>IHttpActionResult</returns>
        /// <response code="400">Request body failed validation.</response>
        /// <response code="500">Message was unable to be submitted to the queue.</response>  
        [HttpPost]
        public async Task<IHttpActionResult> Post(MessageWrapper messageWrapper)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                // Get type
                Type type = Type.GetType($"SBD.MassTransit.POC.Publisher.Models.{messageWrapper.MessageType}");
                if (type == null)
                    return BadRequest(messageWrapper.MessageType + " is not a valid message type.");

                // Convert message to proper type
                messageWrapper.Body = JsonConvert.DeserializeObject(messageWrapper.Body.ToString(), type);

                // Send message
                MethodInfo sendMessage = typeof(MessageController).GetMethod("SendMessage", BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(type);
                if (sendMessage == null)
                    throw new Exception("Failed to get SendMessage method.");

                await ((Task)sendMessage.Invoke(this, new[] { GetQueueSettings(messageWrapper.DestinationId, messageWrapper.MessageType), messageWrapper.Body, messageWrapper.ScheduledTime }));

                return Ok();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("{\"Message\":\"" + e.Message + "\"}")
                });
            }
        }
    }
}