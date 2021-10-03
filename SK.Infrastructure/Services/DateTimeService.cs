using SK.Application.Common.Interfaces;
using System;

namespace SK.Persistence.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}