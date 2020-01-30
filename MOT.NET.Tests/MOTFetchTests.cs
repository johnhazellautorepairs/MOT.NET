using Xunit;
using System;
using System.Security;
using System.Collections.Generic;
using System.Net.Http;
using Xunit.Abstractions;

namespace MOT.NET.Tests {
    public class MOTFetchTests {
        private IAsyncEnumerable<Models.MOT.Record> FetchWithKeys(string key, string correct) {
            SecureString ss = new SecureString();
            foreach(char c in key) ss.AppendChar(c);
            ss.MakeReadOnly();
            return new MotTestClient(ss, MOTFetchMocks.SetupAuthenticationMock(correct).Object).FetchAsync();
        }

        private IAsyncEnumerable<Models.MOT.Record> FetchWithResponse(string response) {
            return new MotTestClient(new SecureString(), MOTFetchMocks.SetupRecordResponseMock(response).Object).FetchAsync();
        }

        [Fact]
        public async void Key_Is_Passed_Into_X_API_Key_Header() {
            const string key = "a";
            await foreach(var record in FetchWithKeys(key, key)) {
                // Do nothing.
            }
        }

        [Fact]
        public void Invalid_Key_Throws_HttpRequestException() {
            Assert.ThrowsAsync<HttpRequestException>(async () => {
                await foreach(var record in FetchWithKeys("b", "a")) {
                    // Do nothing.
                }
            });
        }

        [Fact]
        public async void Valid_Record_Parses_Without_Exceptions() {
            const string response = "[{\"registration\":\"F1\",\"make\":\"BUGATTI\",\"model\":\"UNKNOWN\",\"firstUsedDate\":\"2013.12.31\",\"fuelType\":\"Petrol\",\"primaryColour\":\"Blue\",\"vehicleId\":\"5daEEplMKS0cwar_wYvLuA==\",\"registrationDate\":\"2013.01.28\",\"manufactureDate\":\"2013.12.31\",\"engineSize\":\"7993\",\"motTests\":[{\"completedDate\":\"2019.10.05 09:34:58\",\"testResult\":\"PASSED\",\"expiryDate\":\"2020.10.04\",\"odometerValue\":\"17280\",\"odometerUnit\":\"km\",\"motTestNumber\":\"917216044909\",\"odometerResultType\":\"READ\",\"rfrAndComments\":[]},{\"completedDate\":\"2018.05.23 14:47:51\",\"testResult\":\"PASSED\",\"expiryDate\":\"2019.05.22\",\"odometerValue\":\"16842\",\"odometerUnit\":\"km\",\"motTestNumber\":\"636535193340\",\"odometerResultType\":\"READ\",\"rfrAndComments\":[]},{\"completedDate\":\"2017.05.22 16:52:03\",\"testResult\":\"PASSED\",\"expiryDate\":\"2018.05.21\",\"odometerValue\":\"16802\",\"odometerUnit\":\"km\",\"motTestNumber\":\"466119672165\",\"odometerResultType\":\"READ\",\"rfrAndComments\":[]},{\"completedDate\":\"2016.04.14 18:08:47\",\"testResult\":\"PASSED\",\"expiryDate\":\"2017.04.13\",\"odometerValue\":\"10000\",\"odometerUnit\":\"mi\",\"motTestNumber\":\"601736199774\",\"odometerResultType\":\"READ\",\"rfrAndComments\":[]}]}]";
            await foreach(var record in FetchWithResponse(response)) {
                // Test Registration reading.
                Assert.Equal("F1", record.Registration);
                // Test VehicleId reading
                Assert.Equal(new Guid("1284d6e5-4c99-2d29-1cc1-aaffc18bcbb8"), record.VehicleId);
            }
        }
    }
}