using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using MOT.NET.Models;

namespace MOT.NET {
    public interface IMotTestClient : IFetchable<Vehicle> {
        IMotTestClient Page(int page);

        IMotTestClient Registration(string Registration);

        IMotTestClient Date(DateTime date);

        IMotTestClient Clear();
    }
}