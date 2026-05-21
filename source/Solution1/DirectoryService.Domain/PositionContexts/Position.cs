using System;
using System.Collections.Generic;
using System.Net;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace DirectoryService.Domain.PositionContext;

public class Position
{
	public void Rename(PositionName newName)
	{
		if (LifeTime.IsActivate == false)
		{
			throw new InvalidOperationException("Невозможно переименовать должность, так как она неактивна.");
		}

		Name = newName;
		LifeTime = LifeTime.Update();
	}

	private Position() { }

	public PositionId Id { get; private set; } = null!;
	public PositionName Name { get; private set; } = null!;
	public PositionDescription Description { get; private set; } = null!;
	public IsActive IsActive { get; private set; } = null!;
	public EntityLifeTime LifeTime { get; private set; } = null!;

	private Position(
		PositionId id,
		PositionName name,
		PositionDescription description,
		IsActive isActive,
		EntityLifeTime lifeTime
	)
	{
		Id = id ?? throw new ArgumentNullException(nameof(id));
		Name = name ?? throw new ArgumentNullException(nameof(name));
		Description = description ?? throw new ArgumentNullException(nameof(description));
		IsActive = isActive ?? throw new ArgumentNullException(nameof(isActive));
		LifeTime = lifeTime ?? throw new ArgumentNullException(nameof(lifeTime));
	}

	public Position(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name cannot be empty", nameof(name));

		Id = PositionId.Create();
		Name = PositionName.Create(name);
		Description = PositionDescription.Create(string.Empty);
		IsActive = IsActive.Create(true);
		LifeTime = EntityLifeTime.CreateNew();
	}

	public static Position Create(
		PositionName name,
		PositionDescription description,
		IsActive isActive,
		EntityLifeTime lifeTime,
		PosVerification posVerification
	)
	{
		var toCheck = new Position(PositionId.Create(), name, description, isActive, lifeTime);

		if (!posVerification.CheckUniqueness(toCheck))
			throw new InvalidOperationException($"{name} уже существует.");

		return toCheck;
	}

	public static Position Restore(
		PositionId id,
		PositionName name,
		PositionDescription description,
		IsActive isActive,
		EntityLifeTime lifeTime
	)
	{
		return new Position(id, name, description, isActive, lifeTime);
	}

	public void Update(PositionName name, PositionDescription description, IsActive isActive)
	{
		Name = name;
		Description = description;
		IsActive = isActive;
	}

	public static Position CreateInitial()
	{
		return new Position(
			PositionId.Create(),
			PositionName.Create("Initial"),
			PositionDescription.Create("Default position"),
			IsActive.Create(true),
			EntityLifeTime.CreateNew()
		);
	}

	public static Position Create(int priority)
	{
		return new Position(
			PositionId.Create(),
			PositionName.Create($"Position_{priority}"),
			PositionDescription.Create($"Position {priority}"),
			IsActive.Create(true),
			EntityLifeTime.CreateNew()
		);
	}

	public Position Archive()
	{
		throw new NotImplementedException();
	}
}
