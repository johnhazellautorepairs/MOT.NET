using System;
using Xunit;
using MOT.NET;
using System.Security;

namespace MOT.NET.Tests {
    public class MOTRequestBuilderParameterTests {
        private Core _core = new Core(new SecureString());

        [Fact]
        public void Registation_Query_Cannot_Specify_Date()
            => Assert.Throws<InvalidParametersException>(() => _core.MOTs().Registration("F1").Date(DateTime.Today));

        [Fact]
        public void Date_Query_Cannot_Specify_Registration()
            => Assert.Throws<InvalidParametersException>(() => _core.MOTs().Registration("F1").Date(DateTime.Today));

        [Fact]
        public void Registration_Query_Cannot_Specify_Page()
            => Assert.Throws<InvalidParametersException>(() => _core.MOTs().Registration("F1").Page(1));
        

        [Fact]
        public void Page_Query_Cannot_Specify_Registration()
            => Assert.Throws<InvalidParametersException>(() => _core.MOTs().Page(1).Registration("F1"));

        [Fact]
        public void Date_Query_Can_Be_Paginated()
            => _core.MOTs().Date(DateTime.Today).Page(1);
    }
}
