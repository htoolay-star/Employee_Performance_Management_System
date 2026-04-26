using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.IPositionRepository;
using EPMS.Shared.PositionDTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace EPMS.Domain.Repository.PositionRepo
{
    public class PositionRepository : IPositionRepository
    {
        private readonly AppDbContext _context;
        public PositionRepository(AppDbContext context) => _context = context;

        //  GetAll
        public Task<IEnumerable<PositionResponseDto>> GetAllAsync()
        {
            var result = _context.Database
                .SqlQueryRaw<PositionResponseDto>("EXEC sp_GetAllPositions")
                .AsEnumerable()
                .ToList();

            return Task.FromResult<IEnumerable<PositionResponseDto>>(result);
        }

        //  GetById
        public Task<PositionResponseDto?> GetByIdAsync(int id)
        {
            var result = _context.Database
                .SqlQueryRaw<PositionResponseDto>(
                    "EXEC sp_GetPositionById @Position_ID",
                    new SqlParameter("@Position_ID", id))
                .AsEnumerable()
                .FirstOrDefault();

            return Task.FromResult(result);
        }

        //  Create
        public Task<(bool, string, int?)> CreateAsync(CreatePositionDto dto)
        {
            var result = _context.Database
                .SqlQueryRaw<SpResult>(
                    "EXEC sp_CreatePosition @Position_Name, @Level_ID, @Department_ID, @Team_ID",
                    new SqlParameter("@Position_Name", dto.Position_Name),
                    new SqlParameter("@Level_ID", dto.Level_ID),
                    new SqlParameter("@Department_ID", dto.Department_ID),
                    new SqlParameter("@Team_ID", (object?)dto.Team_ID ?? DBNull.Value))
                .AsEnumerable()
                .FirstOrDefault();

            var response = result?.Status == "SUCCESS"
                ? (true, result.Message, result.Position_ID)
                : (false, result?.Message ?? "Error", (int?)null);

            return Task.FromResult(response);
        }

        //  Update
        public Task<(bool, string)> UpdateAsync(int id, UpdatePositionDto dto)
        {
            var result = _context.Database
                .SqlQueryRaw<SpResult>(
                    "EXEC sp_UpdatePosition @Position_ID, @Position_Name, @Level_ID, @Department_ID, @Team_ID, @Is_Active",
                    new SqlParameter("@Position_ID", id),
                    new SqlParameter("@Position_Name", dto.Position_Name),
                    new SqlParameter("@Level_ID", dto.Level_ID),
                    new SqlParameter("@Department_ID", dto.Department_ID),
                    new SqlParameter("@Team_ID", (object?)dto.Team_ID ?? DBNull.Value),
                    new SqlParameter("@Is_Active", dto.Is_Active))
                .AsEnumerable()
                .FirstOrDefault();

            var response = (result?.Status == "SUCCESS", result?.Message ?? "Error");
            return Task.FromResult(response);
        }

        //  Delete
        public Task<(bool, string)> DeleteAsync(int id)
        {
            var result = _context.Database
                .SqlQueryRaw<SpResult>(
                    "EXEC sp_DeletePosition @Position_ID",
                    new SqlParameter("@Position_ID", id))
                .AsEnumerable()
                .FirstOrDefault();

            var response = (result?.Status == "SUCCESS", result?.Message ?? "Error");
            return Task.FromResult(response);
        }
    }
}

public class SpResult
{
    public string Status { get; set; } = "";
    public string Message { get; set; } = "";
    public int? Position_ID { get; set; }
}
