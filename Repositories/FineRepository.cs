using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;
namespace LibraryApi.Repositories;

public class FineRepository : IFineRepository
{
    private readonly LibraryDbContext _db;
    public FineRepository(LibraryDbContext db) => _db = db;

    public async Task<List<Fine>> GetAllAsync(int? memberId, string? status)
    {
        var q = _db.Fines.AsNoTracking().Include(f => f.Payments).AsQueryable();
        if (memberId.HasValue) q = q.Where(f => f.MemberId == memberId);
        if (!string.IsNullOrWhiteSpace(status)) q = q.Where(f => f.Status == status);
        return await q.OrderByDescending(f => f.CreatedAt).ToListAsync();
    }

    public Task<Fine?> GetByIdAsync(int id) =>
        _db.Fines.Include(f => f.Payments).FirstOrDefaultAsync(f => f.Id == id);

    public async Task AddAsync(Fine fine) => await _db.Fines.AddAsync(fine);

    public void Remove(Fine fine) => _db.Fines.Remove(fine);

    // جداول Members و Loans بتاعة أعضاء تانيين في الفريق، فبنسأل عنها بـ SQL
    // مباشر عشان منعتمدش على الـ Entities بتاعتهم.
    public Task<bool> MemberExistsAsync(int memberId) =>
        _db.Database.SqlQuery<int>(
            $"SELECT Id AS Value FROM Members WHERE Id = {memberId} AND IsDeleted = 0").AnyAsync();

    public Task<bool> LoanBelongsToMemberAsync(int loanId, int memberId) =>
        _db.Database.SqlQuery<int>(
            $"SELECT Id AS Value FROM Loans WHERE Id = {loanId} AND MemberId = {memberId}").AnyAsync();

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
