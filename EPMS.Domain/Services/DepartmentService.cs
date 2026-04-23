using AutoMapper;
using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.HR;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Services;

public class DepartmentService : IDepartmentService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _context.Departments.ToListAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetByIdAsync(long id)
    {
        var department = await _context.Departments.FindAsync(id);
        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<long> CreateAsync(DepartmentDto dto)
    {
        var entity = _mapper.Map<Department>(dto);
        _context.Departments.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(long id, DepartmentDto dto)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department != null)
        {
        
            _mapper.Map(dto, department);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(long id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}