using System;
using System.Collections.Generic;

namespace MassTransit.EndpointMapping
{
    public class EndpointMapping
    {
        private static readonly Dictionary<string, string> Mappings = new Dictionary<string, string>();

        /// <summary>
        /// Adds a mapping of a Namespace to an Endpoint QueueName
        /// </summary>
        /// <param name="ns">The Namespace where a set of Commands are located</param>
        /// <param name="endpointQueueName">The endpoints queuename where commands in this namespace are to be sent</param>
        public static void AddMapping(string ns, string endpointQueueName)
        {
            Mappings.Add(ns, endpointQueueName);
        }

        internal static Uri GetEndpointUri(Uri baseUri, dynamic model)
        {
            var endpointQueue = "";

            var endpointType = model.GetType();

            if (endpointType.Namespace == null)
                throw new ApplicationException(string.Format("No namespace was found for {0}", endpointType.FullName));

            endpointQueue = Mappings[endpointType.Namespace];

            return new Uri(string.Format("{0}/{1}", baseUri, endpointQueue));
        }
    }
}