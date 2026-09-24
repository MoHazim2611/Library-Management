using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;
namespace LibraryApi.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly LibraryDbContext _db;
    public PaymentRepository(LibraryDbContext db) => _db = db;

    public async Task<List<Payment>> GetAllAsync(int? fineId)
    {
        var q = _db.Payments.AsNoTracking().AsQueryable();
        if (fineId.HasValue) q = q.Where(p => p.FineId == fineId);
        return await q.OrderByDescending(p => p.PaidAt).ToListAsync();
    }

    public Task<Payment?> GetByIdAsync(int id) =>
        _db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Payment payment) => await _db.Payments.AddAsync(payment);
}
