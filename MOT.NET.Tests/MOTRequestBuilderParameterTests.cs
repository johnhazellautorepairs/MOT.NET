using System;
using Xunit;
using MOT.NET;
using System.Security;

namespace MOT.NET.Tests {
    public class MOTRequestBuilderParameterTests {
        private Core Setup() {
            return new Core(new SecureString());
        }

        [Fact]
        public void Registration_Query_Cannot_Have_Date_Parameter() {
            Core core = Setup();
            Assert.Throws<InvalidParametersException>(() => core.MOTs().Registration("F1").Date(DateTime.Today));
            Assert.Throws<InvalidParametersException>(() => core.MOTs().Date(DateTime.Today).Registration("F1"));
        }

        [Fact]
        public void Registration_Query_Cannot_Be_Paginated() {
            Core core = Setup();
            Assert.Throws<InvalidParametersException>(() => core.MOTs().Registration("F1").Page(1));
            Assert.Throws<InvalidParametersException>(() => core.MOTs().Page(1).Registration("F1"));
        }
    }
}
