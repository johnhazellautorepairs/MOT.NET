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
            return new Core(ss, MOTFetchMocks.SetupAuthenticationMock(correct).Object).MOTs().FetchAsync();
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
    }
}