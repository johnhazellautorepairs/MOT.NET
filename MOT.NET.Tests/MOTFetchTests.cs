using Xunit;
using System.Security;
using System.Collections.Generic;

namespace MOT.NET.Tests {
    public class MOTFetchTests {
        private IAsyncEnumerable<Models.MOT.Record> FetchWithKeys(string key, string correct) {
            SecureString ss = new SecureString();
            foreach(char c in key) ss.AppendChar(c);
            ss.MakeReadOnly();
            return new Core(ss, MOTFetchMocks.SetupAuthenticationMock(correct).Object).MOTs().FetchAsync();
        }

        [Fact]
        public void Key_Is_Passed_Into_X_API_Key_Header() {
            const string key = "a";
            var result = FetchWithKeys(key, key);
        }
    }
}