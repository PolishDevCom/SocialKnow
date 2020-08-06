using SK.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Persistence.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
