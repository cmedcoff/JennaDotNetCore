using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;


namespace RestVerbsPractice1.Data;

// public class User
// {
//     public int Id { get; set; }
//     public string? Name { get; set; }
//     public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
// }
/// <summary>
/// These are my comments.
/// </summary>
public class TimeEntry
{
    public int Id { get; set; }
    // public int UserId { get; set; }

    /// <summary>
    /// Description of how time was spent
    /// </summary>  
    [Required]
    public string? Description { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class TimeEntryPost
{
    [Required]
    public string? Description { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public partial class SqliteContext : DbContext
{
    // public DbSet<User> Users { get; set; }
    public DbSet<TimeEntry> TimeEntries { get; set; }

    public SqliteContext()
    {
    }

    // public SqliteContext(DbContextOptions<SqliteContext> options)
    //     : base(options)
    // {
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    /* warning To protect potentially sensitive information in your
     * connection string,
     * you should move it out of source code. You can avoid scaffolding the
     * connection string by using the Name= syntax to read it from
     * configuration - see https://go.microsoft.com/fwlink/?linkid=2131148.
     * For more guidance on storing connection strings, see
     * https://go.microsoft.com/fwlink/?LinkId=723263.  =>
     * optionsBuilder.UseSqlite("Data Source=sqlite.db");
     */
         optionsBuilder.UseSqlite("Data Source=sqlite.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

public interface ITimeEntryRepository
{
    TimeEntry Create(TimeEntry timeEntry);
    TimeEntry? Find(int id);
    IEnumerable<TimeEntry> List();
    TimeEntry? Update(int id, TimeEntry timeEntry);
    void Delete(int id);
    public bool TimeEntryExists(int id);
}

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly ILogger<TimeEntryRepository> _logger;
    private readonly SqliteContext _context;

    public TimeEntryRepository(ILogger<TimeEntryRepository> logger,
            SqliteContext context)
    {
        _logger = logger;
        _context = context;
    }

    public TimeEntry Create(TimeEntry timeEntry)
    {
        _context.TimeEntries.Add(timeEntry);
        _context.SaveChanges();
        return timeEntry;
    }

    public TimeEntry? Find(int id)
    {
        return _context.TimeEntries.Find(id);
    }

    public IEnumerable<TimeEntry> List()
    {
        return _context.TimeEntries.ToList();
    }

    public TimeEntry? Update(int id, TimeEntry timeEntry)
    {
        try
        {
            _context.Entry(timeEntry).State = EntityState.Modified;
            _context.SaveChanges();
            return timeEntry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update failed");
            if (!TimeEntryExists(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }
    }

    public void Delete(int id)
    {
        TimeEntry? timeEntry = _context.TimeEntries.Find(id);
        if (timeEntry == null)
        {
            return; // this seems like a silent failure
        }
        _context.TimeEntries.Remove(timeEntry);
        _context.SaveChanges();
    }

    public bool TimeEntryExists(int id)
    {
        return _context.TimeEntries.Any(e => e.Id == id);
    }
}
