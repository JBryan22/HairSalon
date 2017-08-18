using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon;
using System;
using HairSalon.Models;
using System.Collections.Generic;

namespace HairSalon.Tests
{
  [TestClass]
  public class ClientTests : IDisposable
  {

     public ClientTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=hair_salon_test;";
    }

    public void Dispose()
    {
      Client.DeleteAll();
      Client.DeleteAll();
    }

    [TestMethod]
    public void GetAll_CheckToSeeIfClientTableStartsEmpty_0()
    {
      int result = Client.GetAll().Count;

      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_TestSameForSameProperties_True()
    {
      Client newClient = new Client("Jesse", 1, 1);
      Client secondClient = new Client("Jesse", 1, 1);

      bool result = secondClient.Equals(newClient);

      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void GetAll_ReturnsClientsAfterTheyHaveBeenSavedOrderedByName_ClientList()
    {
      //Assign
      Client firstClient = new Client("Jesse", 1);
      firstClient.Save();
      Client secondClient = new Client("Bryan", 1);
      secondClient.Save();
      List<Client> expected = new List<Client>{secondClient, firstClient};

      //Act
      List<Client> actual = Client.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Save_DBAssignsIdToClient_Id()
    {
      Client testClient = new Client("Jesse", 1);
      testClient.Save();

      Client savedClient = Client.GetAll()[0];

      int result = savedClient.GetId();
      int testId = testClient.GetId();


      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsClientInDatabase_Client()
    {
      Client testClient = new Client("Jesse", 1);
      testClient.Save();

      Client result = Client.Find(testClient.GetId());

      Assert.AreEqual(testClient, result);
    }

    [TestMethod]
    public void DeleteThis_DeletesTheSpecifiedClientFromTheDatabase_ClientList()
    {
      //Assign
      Client firstClient = new Client("Jesse", 1);
      firstClient.Save();
      Client secondClient = new Client("Bryan", 1);
      secondClient.Save();
      List<Client> expected = new List<Client>{secondClient};

      //Act
      firstClient.DeleteThis();
      List<Client> actual = Client.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, actual);
    }
  }
}