using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Persistence.Configurations
{
    public class TestValueConfigurator : IEntityTypeConfiguration<TestValue>
    {
        public void Configure(EntityTypeBuilder<TestValue> builder)
        {
            builder.Property(tv => tv.Name)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
