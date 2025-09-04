using Microsoft.EntityFrameworkCore;
using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Data.EntityFramework;

public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
{
    public DbSet<DepositEventPayload> Deposits { get; set; }
    public DbSet<TransactionStatus> TransactionStatus { get; set; }
}