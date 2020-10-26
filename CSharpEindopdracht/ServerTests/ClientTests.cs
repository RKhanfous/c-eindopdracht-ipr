using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace ServerTests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void addByteToByteArray_addingNewByte_newByteAtEndOfArray()
        {
            //here we make the test values for the method.
            byte testByte = 0x03;
            byte[] testByteArray = new byte[2];

            //here we add random values to our test byte array.
            testByteArray[0] = 0x01;
            testByteArray[1] = 0x02;

            //here we make a byte array with the expected values.
            byte[] expectedArray = new byte[3];
            expectedArray[0] = 0x01;
            expectedArray[1] = 0x02;
            expectedArray[2] = 0x03;

            //here we create a instance of the client object.
            Client client = new Client(new System.Net.Sockets.TcpClient(), new NetworkHandler(null), 10);

            //here we create the byte array with the values that are coming out of the method.
            byte[] result = client.addByteToArray(testByteArray, testByte);

            //here we assert if the method returns the expected result.
            Assert.AreEqual(result, expectedArray);
        }
    }
}
