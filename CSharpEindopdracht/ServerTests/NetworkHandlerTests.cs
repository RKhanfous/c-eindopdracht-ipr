using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ServerTests
{
    [TestClass]
    public class NetworkHandlerTests
    {
        [TestMethod]
        public void checkClientsForUser_checkIfClientExists_returnTrue()
        {
            //here we create a instance of the NetworkHandler object.
            NetworkHandler networkHandler = new NetworkHandler();

            //here we fill up a list of clients for the Networkhandler.
            List<Client> clients = new List<Client>();
            for(int i = 0; i < 5; i++)
            {
                clients.Add(new Client(new TcpClient(), new NetworkHandler(), i));
            }
            networkHandler.clients = clients;

            //here we assert if client with the id of 2 exists in the list of clients.
            Assert.IsTrue(networkHandler.checkClientsForUser(2));
        }

        public void checkClientsForUser_checkIfClientDoesntExist_returnFalse()
        {
            //here we create a instance of the NetworkHandler object.
            NetworkHandler networkHandler = new NetworkHandler();

            //here we fill up a list of clients for the Networkhandler.
            List<Client> clients = new List<Client>();
            for (int i = 0; i < 5; i++)
            {
                clients.Add(new Client(new TcpClient(), new NetworkHandler(), i));
            }
            networkHandler.clients = clients;

            //here we assert if client with the clientID 2 exists in the list of clients.
            Assert.IsFalse(networkHandler.checkClientsForUser(7));
        }

        public void getClientByUser_getRequestedClient_returnClientWithClientID()
        {
            //here we create a instance of the NetworkHandler object.
            NetworkHandler networkHandler = new NetworkHandler();

            //here we fill up a list of clients for the Networkhandler.
            List<Client> clients = new List<Client>();
            for (int i = 0; i < 5; i++)
            {
                clients.Add(new Client(new TcpClient(), new NetworkHandler(), i));
            }
            networkHandler.clients = clients;

            //here we make our expected client with the clientID 2.
            Client expectedClient = new Client(new TcpClient(), new NetworkHandler(), 2);

            //here we request for a client with clientID 2 and save it in result.
            Client result = networkHandler.getClientByUser(2);

            //here we assert if the result client equals the expected client.
            Assert.AreEqual(result.clientID, expectedClient.clientID);
        }

    }
}
