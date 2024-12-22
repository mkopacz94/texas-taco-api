namespace TexasTaco.Authentication.Core.Repositories
{
    //internal class AccountDeletedOutboxMessagesRepository(
    //    AuthDbContext context)
    //    : IAccountDeletedOutboxMessagesRepository
    //{
    //    private readonly AuthDbContext _context = context;

    //    public async Task AddAsync(AccountDeletedOutboxMessage message)
    //    {
    //        await _context.AddAsync(message);
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task UpdateAsync(AccountDeletedOutboxMessage message)
    //    {
    //        _context
    //            .AccountDeletedOutboxMessages
    //            .Update(message);

    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task<IEnumerable<AccountDeletedOutboxMessage>> GetNonPublishedMessages()
    //    {
    //        return await _context
    //            .AccountDeletedOutboxMessages
    //            .Where(uo => uo.MessageStatus == OutboxMessageStatus.ToBePublished)
    //            .ToListAsync();
    //    }
    //}
}
