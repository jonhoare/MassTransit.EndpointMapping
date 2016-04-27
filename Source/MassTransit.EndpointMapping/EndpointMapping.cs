using System;
using System.Collections.Generic;

namespace MassTransit.EndpointMapping
{
    public class EndpointMapping
    {
        private static readonly Dictionary<string, string> Mappings = new Dictionary<string, string>();

        public static void AddMapping(string ns, string endpointUri)
        {
            Mappings.Add(ns, endpointUri);
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