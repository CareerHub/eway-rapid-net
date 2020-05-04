﻿using System.Threading.Tasks;
using eWAY.Rapid.Enums;
using eWAY.Rapid.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eWAY.Rapid.Tests.IntegrationTests {
    [TestClass]
    public class SettlementSearchTests : SdkTestBase {
        [TestMethod]
        public async Task SettlementSearch_ByDate_Test() {
            var client = CreateRapidApiClient();
            //Arrange

            //Act
            var settlementSearch = new SettlementSearchRequest() { ReportMode = SettlementSearchMode.Both, SettlementDate = "2016-02-01" };
            var settlementResponse = await client.SettlementSearchAsync(settlementSearch);

            //Assert
            TestUtil.AssertNoErrors(settlementResponse);
        }

        /*
        [TestMethod]
        public async Task SettlementSearch_ByDateRange_Test() {
            var client = CreateRapidApiClient();
            //Arrange

            //Act
            var settlementSearch = new SettlementSearchRequest() {
                ReportMode = SettlementSearchMode.Both,
                StartDate = "2016-02-01",
                EndDate = "2016-02-08",
                CardType = CardType.ALL,
            };
            var settlementResponse = await client.SettlementSearchAsync(settlementSearch);

            //Assert
            TestUtil.AssertNoErrors(settlementResponse);
            Assert.IsTrue(settlementResponse.SettlementTransactions.Length > 1);
            Assert.IsTrue(settlementResponse.SettlementSummaries.Length > 1);
        }
        */

        [TestMethod]
        public async Task SettlementSearch_WithPage_Test() {
            var client = CreateRapidApiClient();
            //Arrange

            //Act
            var settlementSearch = new SettlementSearchRequest() {
                ReportMode = SettlementSearchMode.TransactionOnly,
                SettlementDate = "2016-02-01",
                Page = 1,
                PageSize = 5
            };
            var settlementResponse = await client.SettlementSearchAsync(settlementSearch);

            //Assert
            TestUtil.AssertNoErrors(settlementResponse);
            Assert.IsTrue(settlementResponse.SettlementTransactions.Length < 6);
        }

    }
}
