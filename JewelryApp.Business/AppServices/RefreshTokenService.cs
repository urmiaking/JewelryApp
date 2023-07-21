﻿using AutoMapper;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IMapper _mapper;
    private readonly IRepository<RefreshToken> _repository;

    public RefreshTokenService(IMapper mapper, IRepository<RefreshToken> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Guid> AddAsync(Guid userId, TimeSpan lifeTime, Guid jwtId)
    {
        var model = new RefreshToken
        {
            UserId = userId,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.Add(lifeTime),
            JwtId = jwtId
        };

        _repository.Add(model);
        await _repository.SaveChangesAsync();

        return model.Id;
    }

    public async Task<RefreshTokenDto?> FindAsync(Guid id)
    {
        var query = _repository.Get().Where(x => x.Id == id);

        return await _mapper.ProjectTo<RefreshTokenDto>(query).FirstOrDefaultAsync();
    }

    public async Task SetUsedAsync(Guid id)
    {
        var model = await _repository.Get()
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (model != null)
        {
            model.Used = true;

            await _repository.SaveChangesAsync();
        }
    }

    public async Task SetInvalidatedAsync(Guid id)
    {
        var model = await _repository.Get()
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (model != null)
        {
            model.Invalidated = true;

            await _repository.SaveChangesAsync();
        }
    }
}