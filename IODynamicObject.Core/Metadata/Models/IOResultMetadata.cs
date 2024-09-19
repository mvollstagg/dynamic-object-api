using IODynamicObject.Core.Metadata.Enumeration;
using System.Collections.Generic;

namespace IODynamicObject.Core.Metadata.Models
{
    public class IOResultMetadata
    {
        public IOResultStatusEnum Status { get; set; }
        public List<KeyValuePair<string, List<string>>>? Messages { get; set; }

        public IOResultMetadata()
        {
            Status = IOResultStatusEnum.Success;
            Messages = new List<KeyValuePair<string, List<string>>>();
        }

        public IOResultMetadata(IOResultStatusEnum status, List<KeyValuePair<string, List<string>>>? messages = null)
        {
            Status = status;
            Messages = messages ?? new List<KeyValuePair<string, List<string>>>();
        }

        public IOResultMetadata(IOResultStatusEnum status, string message)
        {
            Status = status;
            Messages = new List<KeyValuePair<string, List<string>>>
            {
                new KeyValuePair<string, List<string>>("Message", new List<string> { message })
            };
        }

        public void AddMessage(string key, string message)
        {
            if (Messages == null)
            {
                Messages = new List<KeyValuePair<string, List<string>>>();
            }

            // Find the index of the KeyValuePair with the matching key
            int index = Messages.FindIndex(kvp => kvp.Key == key);

            if (index >= 0)
            {
                // Get the existing list of messages
                var existingMessages = Messages[index].Value ?? new List<string>();

                // Add the new message to the existing list
                existingMessages.Add(message);

                // Replace the KeyValuePair at the found index with the updated list
                Messages[index] = new KeyValuePair<string, List<string>>(key, existingMessages);
            }
            else
            {
                // Key not found, add a new KeyValuePair to the list
                Messages.Add(new KeyValuePair<string, List<string>>(key, new List<string> { message }));
            }
        }
    }
}