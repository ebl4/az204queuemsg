// Get the connection string from app settings
using Azure.Storage.Queues;
using System.Configuration;

string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
string queueName = "QUEUE_NAME";
string mensage = "Teste Mensage";

// Instantiate a QueueClient which will be used to create and manipulate the queue
QueueClient queueClient = new QueueClient(connectionString, queueName);

// Create the queue
queueClient.CreateIfNotExists();

if (queueClient.Exists())
{
    // Send a mensage to the queue
    queueClient.SendMessage(mensage);
}