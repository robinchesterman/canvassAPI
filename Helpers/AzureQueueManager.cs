using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;
using Canvass.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace Canvass.Api.Helpers
{
    public interface IQueueManager
    {
        Task AddQueueMessageAsync(string queueName, CloudQueueMessage message);
        Task<CloudQueueMessage> GetQueueMessageAsync(string queueName);
        Task DeleteQueueMessageAsync(string queueName, CloudQueueMessage msg);
        Task<List<CloudQueueMessage>> GetQueueMessages(string queueName, int count);
    }
    public class QueueManager : IQueueManager
    {
        private AppSettings _appSettings;
        private readonly CloudQueueClient _queueClient;
        private CloudStorageAccount _storageAccount;

        public QueueManager(IConfiguration configuration)
        {
            _appSettings = configuration.Get<AppSettings>();
            _storageAccount = GetStorageAccount();
            _queueClient = _storageAccount.CreateCloudQueueClient();
        }

        public async Task AddQueueMessageAsync(string queueName, CloudQueueMessage message)
        {
            var queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();
            await queue.AddMessageAsync(message);
        }

        public async Task<List<CloudQueueMessage>> GetQueueMessages(string queueName, int count)
        {
            var queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();
            var messages = await queue.GetMessagesAsync(count);
            return messages.ToList();
        }

        public async Task<CloudQueueMessage> GetQueueMessageAsync(string queueName)
        {
            var queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();
            var message = await queue.GetMessageAsync();
            return message;
        }

        public async Task DeleteQueueMessageAsync(string queueName, CloudQueueMessage msg)
        {
            var queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();
            await queue.DeleteMessageAsync(msg);
        }

        private CloudStorageAccount GetStorageAccount()
        {
            var account = _appSettings.Azure.StorageAccountName;
            var key = _appSettings.Azure.StorageAccountName;
            var connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", account, key);
            return CloudStorageAccount.Parse(connectionString);
        }
    }
}