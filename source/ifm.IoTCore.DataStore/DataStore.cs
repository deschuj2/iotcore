namespace ifm.IoTCore.DataStore;

using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Contracts;
using Microsoft.Extensions.Configuration;

public class DataStore : IDataStore
{
    protected readonly string DataStoreFile;
    private const int Timeout = 5000;
    private readonly ReaderWriterLockSlim _lock = new (LockRecursionPolicy.SupportsRecursion);

    public DataStore(string fileName)
    {
        DataStoreFile = fileName;

        if (fileName == null)
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        if (fileName.Equals(string.Empty))
        {
            throw new InvalidDataException();
        }

        if (!File.Exists(DataStoreFile))
        {
            throw new FileNotFoundException($"File {fileName} not found");
        }
    }

    public T Get<T>(string sectionKey, string configKey)
    {
        if (!_lock.TryEnterReadLock(Timeout))
        {
            throw new FieldAccessException("The file is already in use by another task. Try it later.");
        }

        try
        {
            return new ConfigurationBuilder()
                .AddJsonFile(DataStoreFile)
                .Build()
                .GetSection(sectionKey)
                .GetSection(configKey)
                .Get<T>();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void Set<T>(string sectionKey, string configKey, T config)
    {
        if (!_lock.TryEnterWriteLock(Timeout))
        {
            throw new FieldAccessException("The file is already in use by another task. Try it later.");
        }

        try
        {
            var json = File.ReadAllText(DataStoreFile);
            var stream = File.Open(DataStoreFile, FileMode.Create);

            var documentOptions = new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip };
                
            var jsonWriter = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            var jsonDocument = JsonDocument.Parse(json, documentOptions);
            var written = false;

            jsonWriter.WriteStartObject();
            foreach (var sectionElement in jsonDocument.RootElement.EnumerateObject())
            {
                if (sectionElement.Name.Equals(sectionKey))
                {
                    jsonWriter.WriteStartObject(sectionKey);
                    foreach (var configElement in sectionElement.Value.EnumerateObject())
                    {
                        if (configElement.Name.Equals(configKey))
                        {
                            written = WriteConfig(configKey, jsonWriter, config);
                        }
                        else
                        {
                            configElement.WriteTo(jsonWriter);
                        }
                    }

                    if (!written)
                    {
                        written = WriteConfig(configKey, jsonWriter, config);
                    }

                    jsonWriter.WriteEndObject();
                }
                else
                {
                    sectionElement.WriteTo(jsonWriter);
                }
            }

            if (!written)
            {
                jsonWriter.WriteStartObject(sectionKey);
                WriteConfig(configKey, jsonWriter, config);
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.Dispose();
            stream.Close();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    private bool WriteConfig<T>(string key, Utf8JsonWriter writer, T config)
    {
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new JsonStringEnumConverter(), new DoubleJsonNetFrameworkConverter(), new FloatJsonNetFrameworkConverter() },
            WriteIndented = true
        });

        var tmpDocument = JsonDocument.Parse(json);
        writer.WritePropertyName(key);
        tmpDocument.RootElement.WriteTo(writer);
        return true;
    }
}