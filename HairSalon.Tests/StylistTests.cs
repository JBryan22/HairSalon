using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon;
using System;
using HairSalon.Models;
using System.Collections.Generic;

namespace HairSalon.Tests
{
  [TestClass]
  public class StylistsTest : IDisposable
  {

     public StylistsTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=hair_salon_test;";
    }

    public void Dispose()
    {
      Client.DeleteAll();
      Stylist.DeleteAll();
    }

    [TestMethod]
    public void GetAll_CheckToSeeIfStylistTableStartsEmpty_0()
    {
      int result = Stylist.GetAll().Count;

      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_TestSameForSameProperties_True()
    {
      Stylist newStylist = new Stylist("Jesse", 1);
      Stylist secondStylist = new Stylist("Jesse", 1);

      bool result = secondStylist.Equals(newStylist);

      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void GetAll_ReturnsStylistsAfterTheyHaveBeenSavedOrderedByName_StylistList()
    {
      //Assign
      Stylist firstStylist = new Stylist("Jesse");
      firstStylist.Save();
      Stylist secondStylist = new Stylist("Bryan");
      secondStylist.Save();
      List<Stylist> expected = new List<Stylist>{secondStylist, firstStylist};

      //Act
      List<Stylist> actual = Stylist.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Save_DBAssignsIdToStylist_Id()
    {
      Stylist testStylist = new Stylist("Jesse");
      testStylist.Save();

      Stylist savedStylist = Stylist.GetAll()[0];

      int result = savedStylist.GetId();
      int testId = testStylist.GetId();


      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsStylistInDatabase_Stylist()
    {
      Stylist testStylist = new Stylist("Jesse");
      testStylist.Save();

      Stylist result = Stylist.Find(testStylist.GetId());

      Assert.AreEqual(testStylist, result);
    }

    [TestMethod]
    public void DeleteThis_DeletesTheSpecifiedStylistFromTheDatabase_StylistList()
    {
      //Assign
      Stylist firstStylist = new Stylist("Jesse");
      firstStylist.Save();
      Stylist secondStylist = new Stylist("Bryan");
      secondStylist.Save();
      List<Stylist> expected = new List<Stylist>{secondStylist};

      //Act
      firstStylist.DeleteThis();
      List<Stylist> actual = Stylist.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetClients_AListOfClientsAssociatedWithTheStylistIsReturned_ClientList()
    {
      //Assign
      Stylist firstStylist = new Stylist("Jesse");
      firstStylist.Save();
      Client firstClient = new Client("Bob", firstStylist.GetId());
      firstClient.Save();
      Client secondClient = new Client("Mary", firstStylist.GetId());
      secondClient.Save();
      List<Client> expected = new List<Client>{firstClient, secondClient};

      //Act
      List<Client> result = firstStylist.GetAllClients();

      //Assert
      CollectionAssert.AreEqual(expected, result);
    }
  }
}