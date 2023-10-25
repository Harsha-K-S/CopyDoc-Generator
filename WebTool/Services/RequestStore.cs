using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace WebTool
{
    public class RequestStore
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RequestStore(string filePath)
        {
            _filePath = filePath;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public RequestCollection FindAll(string authorId = null)
        {
            List<Request> requests = null;
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                requests = JsonSerializer.Deserialize<List<Request>>(json);
                requests = requests.Where(r => authorId == null || r.AuthorId == authorId).ToList();
            }

            requests ??= new List<Request>();

            return new RequestCollection(requests);
        }

        public void Save(RequestCollection requests)
        {
            string json = JsonSerializer.Serialize(requests, _jsonSerializerOptions);
            File.WriteAllText(_filePath, json);
        }
    }
}