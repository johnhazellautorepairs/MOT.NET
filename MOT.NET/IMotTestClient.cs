using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using MOT.NET.Models.MOT;

namespace MOT.NET {
    public interface IMotTestClient : IFetchable<Record> {
        IMotTestClient Page(int page);

        IMotTestClient Registration(string Registration);

        IMotTestClient Date(DateTime date);
    }
}