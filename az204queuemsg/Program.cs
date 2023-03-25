// Get the connection string from app settings
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
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

    // Peek at the next message
    GetPeekedMessages(queueClient);

    GetContentOfQueueMessage(queueClient);

    DequeueNextMessage(queueClient);

    GetQueueLength(queueClient);

    DeleteQueue(queueClient);
}

PeekedMessage[] GetPeekedMessages(QueueClient queueClient)
{
    return queueClient.PeekMessages();
}

// This saves the state of work associated with the message,
// and gives the client another minute to continue working on the message.
void GetContentOfQueueMessage(QueueClient queueClient)
{
    // Get the message from the queue
    QueueMessage[] message = queueClient.ReceiveMessages();

    // Update the message contents
    queueClient.UpdateMessage(message[0].MessageId,
        message[0].PopReceipt,
        "Update contents",
        TimeSpan.FromSeconds(60.0) // Make it invisible for another 60 seconds
        );
}

void DequeueNextMessage(QueueClient queueClient)
{
    // Get the next message
    QueueMessage[] messages = queueClient.ReceiveMessages();

    Console.WriteLine($"Dequeued message: '{messages[0].Body}'");

    queueClient.DeleteMessage(messages[0].MessageId, messages[0].PopReceipt);
}

void GetQueueLength(QueueClient queueClient)
{
    QueueProperties properties = queueClient.GetProperties();

    // Retrieve the cached approximate message count
    int cachedMessagesCount = properties.ApproximateMessagesCount;

    Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");
}

void DeleteQueue(QueueClient queueClient)
{
    // Delete the queue
    queueClient.Delete();
}