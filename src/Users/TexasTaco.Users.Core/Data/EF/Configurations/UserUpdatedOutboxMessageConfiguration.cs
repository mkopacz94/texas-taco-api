using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Data.EF.Configurations
{
    internal class UserUpdatedOutboxMessageConfiguration
        : IEntityTypeConfiguration<UserUpdatedOutboxMessage>
    {
        public void Configure(EntityTypeBuilder<UserUpdatedOutboxMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new UserUpdatedOutboxMessageId(value));

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<UserUpdatedEventMessage>(m)!);
        }
    }
}
