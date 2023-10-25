using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WebTool
{
    public class HtmlStore
    {
        private readonly string _directoryPath;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HtmlStore(string directoryPath)
        {
            _directoryPath = directoryPath;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public HtmlContainerCollection OpenLatest(string url, string authorId)
        {
            string filePath = FindLatestFile(url, authorId);
            string json = File.ReadAllText(filePath);
            HtmlContainerCollection containers = JsonSerializer.Deserialize<HtmlContainerCollection>(json, _jsonSerializerOptions);

            return containers;
        }

        public void Save(HtmlContainerCollection containers, string url, string authorId)
        {
            if (Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }

            string authorDirectory = Path.Combine(_directoryPath, authorId);
            if (!Directory.Exists(authorDirectory))
            {
                Directory.CreateDirectory(authorDirectory);
            }

            string fileName = ConvertToWindowsFileName(url);
            fileName += $"{DateTime.Now:yyyyMMddHHmmss}.json";

            string filePath = Path.Combine(authorDirectory, fileName);
            string json = JsonSerializer.Serialize(containers, _jsonSerializerOptions);

            File.WriteAllText(filePath, json);
        }

        private string FindLatestFile(string url, string authorId)
        {
            string searchFileName = ConvertToWindowsFileName(url);
            string authorDirectory = Path.Combine(_directoryPath, authorId);
            string[] files = Directory.GetFiles(authorDirectory, $"{searchFileName}*").Where(n => Path.GetFileNameWithoutExtension(n).Length == searchFileName.Length + 14).ToArray();
            string latest = files.OrderByDescending(n => n).FirstOrDefault();

            return latest;
        }

        private static string ConvertToWindowsFileName(string urlText)
        {
            List<string> urlParts = new List<string>();
            string rt = string.Empty;
            Regex r = new Regex(@"[a-z]+", RegexOptions.IgnoreCase);

            foreach (Match m in r.Matches(urlText))
            {
                urlParts.Add(m.Value);
            }

            for (int i = 0; i < urlParts.Count; i++)
            {
                rt += urlParts[i] + "_";
            }

            return rt.Substring(0, Math.Min(rt.Length, 100));
        }
    }
}